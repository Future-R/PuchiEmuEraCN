using MinorShift.Emuera.GameView;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MinorShift.Emuera.GameProc.Function
{
    internal class ClipboardProcessor
    {
        private bool useCB;
        private bool classicMode; // New Lines Only mode

        private readonly MainWindow mainWin;

        private bool minTimePassed; //Has enough time passed since the last Clipboard update?
        private bool postWaiting; //Is there text waiting to be sent to clipboard?
        private static System.Timers.Timer minTimer = null; //Minimum timer for refrehsing the clipboard to prevent spam

        private int MaxCB; //Max length in lines of the output to clipboard
        private int ScrollPos; //Position of the clipboard output in the buffer
        private int ScrollCount; //Lines to scroll at a time
        private int NewLineCount; //Number of new lines
        private int OldNewLineCount; //Number of lines in the last update, used for Classic mode + scrolling back to bottom
        private StringBuilder OldText; //Last set of lines sent to the clipboard
        private CircularBuffer<string> lineBuffer; //Buffer for processed strings ready for clipboard

        public string GetLatestString
        {
            get
            {
                return OldText.ToString();
            }
        }

        internal enum CBTriggers
        {
            LeftClick,
            MiddleClick,
            DoubleLeftClick,
            AnyKeyWait,
            InputWait,
        }

        public delegate void SetClipboardDelegate(string text, bool copy, int times, int delay); //Just for using the timer to set the clipboard.

        public ClipboardProcessor(MainWindow parent)
        {
            useCB = Config.CBUseClipboard;
            classicMode = Config.CBNewLinesOnly;

            mainWin = parent;

            minTimePassed = true;
            postWaiting = false;

            MaxCB = Config.CBMaxCB;
            ScrollPos = 0; //FIXIT - Expand it, add a button, etc
            ScrollCount = Config.CBScrollCount; //FIXIT - Actually use it
            NewLineCount = 0;
            OldNewLineCount = 0;

            if (useCB)
            {
                lineBuffer = new CircularBuffer<string>(Config.CBBufferSize);
                minTimer = new System.Timers.Timer(Config.CBMinTimer);
                minTimer.AutoReset = false;
                minTimer.Elapsed += minTimerDone;
                OldText = new StringBuilder();
            }
        }

        public bool ScrollUp(int value)
        {
            if (!useCB) return false;
            if (ScrollPos == 0 && classicMode && ScrollCount > OldNewLineCount) ScrollPos = OldNewLineCount;
            else ScrollPos += ScrollCount * value;
            if (lineBuffer.Count < ScrollPos) ScrollPos = lineBuffer.Count - ScrollCount;
            SendToCB(true);
            return true;
        }

        public bool ScrollDown(int value)
        {
            if (!useCB) return false;
            ScrollPos -= ScrollCount;
            if (ScrollPos < 0) ScrollPos = 0;
            SendToCB(true);

            return true;
        }

        private void minTimerDone(Object source, System.Timers.ElapsedEventArgs e)
        {
            minTimePassed = true;
            if (postWaiting) SendToCB(true);
        }

        private bool minTimeCheck()
        {
            if (minTimePassed)
            {
                minTimePassed = false;
                minTimer.Start();
                return true;
            }
            else return false;
        }

        //FIXIT - Autoprocess old lines or just ditch?
        public void AddLine(ConsoleDisplayLine inputLine, bool left)
        {
            if (!useCB) return;

            NewLineCount++;
            string processed = ProcessLine(inputLine.ToString());
            lineBuffer.Enqueue(processed);
        }

        public void DelLine(int count)
        {
            if (!useCB || count <= 0) return;

            NewLineCount = Math.Max(0, NewLineCount - count);
            if (count >= lineBuffer.Count) lineBuffer.Clear();
            else
            {
                while (count > 0)
                {
                    lineBuffer.Dequeue();
                    count--;
                }
            }
        }

        public void ClearScreen()
        {
            if (!useCB) return;
            if (Config.CBClearBuffer)
            {
                lineBuffer.Clear();
                ScrollPos = 0;
                NewLineCount = 0;
            }
            else
            {
                lineBuffer.Enqueue("");
            }
        }

        public void Check(CBTriggers type)
        {
            if (!useCB) return;
            switch (type)
            {
                case CBTriggers.LeftClick:
                    if (!Config.CBTriggerLeftClick) return;
                    break;

                case CBTriggers.MiddleClick:
                    if (!Config.CBTriggerMiddleClick) return;
                    break;

                case CBTriggers.DoubleLeftClick:
                    if (!Config.CBTriggerDoubleLeftClick) return;
                    break;

                case CBTriggers.AnyKeyWait:
                    if (!Config.CBTriggerAnyKeyWait) return;
                    break;

                case CBTriggers.InputWait:
                    if (!Config.CBTriggerInputWait) return;
                    break;

                default:
                    return;
            }

            ScrollPos = 0;
            SendToCB(false);
        }

        public void SendToCB(bool force)
        {
            if (!useCB) return;
            if (NewLineCount == 0 && !force) return;
            if (!minTimeCheck())
            {
                postWaiting = true;
                return;
            }

            if (NewLineCount == 0 && ScrollPos == 0) NewLineCount = OldNewLineCount;

            int length;
            if (classicMode && ScrollPos == 0) length = Math.Min(NewLineCount, lineBuffer.Count);
            else length = Math.Min(MaxCB, lineBuffer.Count - ScrollPos);
            StringBuilder newText = new StringBuilder();
            if (length > 0)
            {
                for (int count = 0; count < length; count++)
                {
                    newText.AppendLine(lineBuffer[lineBuffer.Count - length - ScrollPos + count]);
                }
                if (newText.ToString().Equals(OldText.ToString())) return;
                try //FIXIT - For now it just fails silently
                {
                    SetClipboardDelegate SCBDelegate = new SetClipboardDelegate(Clipboard.SetDataObject);
                    mainWin.Invoke(new SetClipboardDelegate(Clipboard.SetDataObject), newText.ToString(), false, 3, 200);
                    if (ScrollPos == 0) OldNewLineCount = NewLineCount;
                    NewLineCount = 0;
                    OldText = newText;
                    postWaiting = false;
                }
                catch (Exception ex) { };
            }
        }

        private const string HTML_TAG_PATTERN = "<.*?>";

        public static string StripHTML(string input)
        {
            // still faster to use String.Contains to check if we need to do this at all first, supposedly
            if (Config.CBIgnoreTags && input.Contains("<"))
            {
                // regex is faster and simpler than a for loop you nerds
                return Regex.Replace(input, HTML_TAG_PATTERN, Config.CBReplaceTags);
            }
            return input;
        }

        private string ProcessLine(string input)
        {
            return StripHTML(input);
        }
    }
}