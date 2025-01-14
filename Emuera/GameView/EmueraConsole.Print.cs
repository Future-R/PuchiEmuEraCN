﻿using MinorShift._Library;
using MinorShift.Emuera.GameProc.Function;
using MinorShift.Emuera.Sub;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MinorShift.Emuera.GameView
{
    //1820 EmueraConsoleのうちdisplayLineListやprintBufferに触るもの
    //いつかEmueraConsoleから分離したい
    internal sealed partial class EmueraConsole : IDisposable
    {
        private readonly List<ConsoleDisplayLine> displayLineList;
        public bool noOutputLog = false;
        public Color bgColor = Config.BackColor;

        private readonly PrintStringBuffer printBuffer;
        private readonly StringMeasure stringMeasure = new StringMeasure();

        public void ClearDisplay()
        {
            CBProc.ClearScreen();

            displayLineList.Clear();
            logicalLineCount = 0;
            lineNo = 0;
            lastDrawnLineNo = -1;
            verticalScrollBarUpdate();
            window.Refresh();//OnPaint発行
        }

        #region Print系

        //private bool useUserStyle = true;
        public bool UseUserStyle { get; set; }

        public bool UseSetColorStyle { get; set; }
        private StringStyle defaultStyle = new StringStyle(Config.ForeColor, FontStyle.Regular, null);
        private StringStyle userStyle = new StringStyle(Config.ForeColor, FontStyle.Regular, null);

        //private StringStyle style = new StringStyle(Config.ForeColor, FontStyle.Regular, null);
        private StringStyle Style
        {
            get
            {
                if (!UseUserStyle)
                    return defaultStyle;
                if (UseSetColorStyle)
                    return userStyle;
                //PRINTD系(SETCOLORを無視する)
                if (userStyle.Color == defaultStyle.Color)
                    return userStyle;
                return new StringStyle(defaultStyle.Color, userStyle.FontStyle, userStyle.Fontname);
            }
        }

        //private StringStyle Style { get { return (useUserStyle ? userStyle : defaultStyle); } }
        public StringStyle StringStyle { get { return userStyle; } }

        public void SetStringStyle(FontStyle fs)
        {
            userStyle.FontStyle = fs;
        }

        public void SetStringStyle(Color color)
        {
            userStyle.Color = color; userStyle.ColorChanged = (color != Config.ForeColor);
        }

        public void SetFont(string fontname)
        {
            if (!string.IsNullOrEmpty(fontname))
                userStyle.Fontname = fontname;
            else
                userStyle.Fontname = Config.FontName;
            PrintEdgeFont.fontName = userStyle.Fontname; // sworve: this is dumb but so is edge fonts
            
        }

        private DisplayLineAlignment alignment = DisplayLineAlignment.LEFT;
        public DisplayLineAlignment Alignment { get { return alignment; } set { alignment = value; } }

        public void ResetStyle()
        {
            userStyle = defaultStyle;
            alignment = DisplayLineAlignment.LEFT;
        }

        public bool EmptyLine { get { return printBuffer.IsEmpty; } }

        /// <summary>
        /// DRAWLINE用文字列
        /// </summary>
        private string stBar = null;

        private uint lastBgColorChange = 0;
        private bool forceTextBoxColor = false;

        public void SetBgColor(Color color)
        {
            this.bgColor = color;
            forceTextBoxColor = true;
            //REDRAWされない場合はTextBoxの色は変えずにフラグだけ立てる
            //最初の再描画時に現在の背景色に合わせる
            if (redraw == ConsoleRedraw.None && window.ScrollBar.Value == window.ScrollBar.Maximum)
                return;
            uint sec = WinmmTimer.TickCount - lastBgColorChange;
            //色変化が速くなりすぎないように一定時間以内の再呼び出しは強制待ちにする
            while (sec < 200)
            {
                Application.DoEvents();
                sec = WinmmTimer.TickCount - lastBgColorChange;
            }
            RefreshStrings(true);
            lastBgColorChange = WinmmTimer.TickCount;
        }

        /// <summary>
        /// 最後に描画した時にlineNoの値
        /// </summary>
        private int lastDrawnLineNo = -1;

        private int lineNo = 0;
        private Int64 logicalLineCount = 0;
        public long LineCount { get { return logicalLineCount; } }

        private void addRangeDisplayLine(ConsoleDisplayLine[] lineList)
        {
            for (int i = 0; i < lineList.Length; i++)
                addDisplayLine(lineList[i], false);
        }

        private void addDisplayLine(ConsoleDisplayLine line, bool force_LEFT)
        {
            CBProc.AddLine(line, force_LEFT);

            if (LastLineIsTemporary)
                deleteLine(1);
            //不適正なFontのチェック
            AConsoleDisplayPart errorStr = null;
            foreach (ConsoleButtonString button in line.Buttons)
            {
                foreach (AConsoleDisplayPart css in button.StrArray)
                {
                    if (css.Error)
                    {
                        errorStr = css;
                        break;
                    }
                }
            }
            if (errorStr != null)
            {
                MessageBox.Show("Emueraの表示処理中に不適正なフォントを検出しました\n描画処理を続行できないため強制終了します", "フォント不適正");
                this.Quit();
                return;
            }
            if (force_LEFT)
                line.SetAlignment(DisplayLineAlignment.LEFT);
            else
                line.SetAlignment(alignment);
            line.LineNo = lineNo;
            displayLineList.Add(line);
            lineNo++;
            if (line.IsLogicalLine)
                logicalLineCount++;
            if (lineNo == int.MaxValue)
            {
                lastDrawnLineNo = -1;
                lineNo = 0;
            }
            if (logicalLineCount == long.MaxValue)
            {
                logicalLineCount = 0;
            }
            if (displayLineList.Count > Config.MaxLog)
                displayLineList.RemoveAt(0);
        }

        public void deleteLine(int argNum)
        {
            CBProc.DelLine(Math.Min(argNum, displayLineList.Count)); //FIXIT - Do we need to worry about the count?

            int delNum = 0;
            int num = argNum;
            while (delNum < num)
            {
                if (displayLineList.Count == 0)
                    break;
                ConsoleDisplayLine line = displayLineList[displayLineList.Count - 1];
                displayLineList.RemoveAt(displayLineList.Count - 1);
                lineNo--;
                if (line.IsLogicalLine)
                {
                    delNum++;
                    logicalLineCount--;
                }
            }
            if (lineNo < 0)
                lineNo += int.MaxValue;
            lastDrawnLineNo = -1;
            //固定画像が削除した行に該当する場合は消去する
            fixImgPrint.clearLineFixImage(lineNo);  //182101 PCDRP-Update:画像固定表示機能で修正
                                                    //RefreshStrings(true);
        }

        public bool LastLineIsTemporary
        {
            get
            {
                if (displayLineList.Count == 0)
                    return false;
                return displayLineList[displayLineList.Count - 1].IsTemporary;
            }
        }

        //最終行を書き換え＋次の行追加時にはその行を再利用するように設定
        public void PrintTemporaryLine(string str)
        {
            PrintSingleLine(str, true);
        }

        //最終行だけを書き換える
        private void changeLastLine(string str)
        {
            deleteLine(1);
            PrintSingleLine(str, false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <param name="position"></param>
        /// <param name="level">警告レベル.0:軽微なミス.1:無視できる行.2:行が実行されなければ無害.3:致命的</param>
        public void PrintWarning(string str, ScriptPosition position, int level)
        {
            if (level < Config.DisplayWarningLevel && !Program.AnalysisMode)
                return;
            //警告だけは強制表示
            bool b = force_temporary;
            force_temporary = false;
            if (position != null)
            {
                if (position.LineNo >= 0)
                {
                    PrintErrorButton(string.Format("Warning Lv{0}:{1}: at line {2}:{3}", level, position.Filename, position.LineNo, str), position);
                    if (position.RowLine != null)
                        PrintError(position.RowLine);
                }
                else
                    PrintErrorButton(string.Format("Warning Lv{0}:{1}:{2}", level, position.Filename, str), position);
            }
            else
            {
                PrintError(string.Format("Warning Lv{0}:{1}", level, str));
            }
            force_temporary = b;
        }

        /// <summary>
        /// ユーザー指定のフォントを無視する。ウィンドウサイズを考慮せず確実に一行で書く。システム用。
        /// </summary>
        /// <param name="str"></param>
        public void PrintSystemLine(string str)
        {
            PrintFlush(false);
            //RefreshStrings(false);
            UseUserStyle = false;
            PrintSingleLine(str, false);
        }

        public void PrintError(string str)
        {
            if (string.IsNullOrEmpty(str))
                return;
            if (Program.DebugMode)
            {
                this.DebugPrint(str);
                this.DebugNewLine();
            }
            PrintFlush(false);
            UseUserStyle = false;
            ConsoleDisplayLine dispLine = PrintPlainwithSingleLine(str);
            if (dispLine == null)
                return;
            addDisplayLine(dispLine, true);
            RefreshStrings(false);
        }

        internal void PrintErrorButton(string str, ScriptPosition pos)
        {
            if (string.IsNullOrEmpty(str))
                return;
            if (Program.DebugMode)
            {
                this.DebugPrint(str);
                this.DebugNewLine();
            }
            UseUserStyle = false;
            ConsoleDisplayLine dispLine = printBuffer.AppendAndFlushErrButton(str, Style, ErrorButtonsText, pos, stringMeasure);
            if (dispLine == null)
                return;
            addDisplayLine(dispLine, true);
            RefreshStrings(false);
        }

        /// <summary>
        /// 1813 従来のPrintLineを用途を考慮してPrintSingleLineとPrintSystemLineに分割
        /// </summary>
        /// <param name="str"></param>
        public void PrintSingleLine(string str) { PrintSingleLine(str, false); }

        public void PrintSingleLine(string str, bool temporary)
        {
            if (string.IsNullOrEmpty(str))
                return;
            PrintFlush(false);
            printBuffer.Append(str, Style);
            ConsoleDisplayLine dispLine = BufferToSingleLine(true, temporary);
            if (dispLine == null)
                return;
            addDisplayLine(dispLine, false);
            RefreshStrings(false);
        }

        public void Print(string str)
        {
            if (string.IsNullOrEmpty(str))
                return;
            if (str.Contains("\n"))
            {
                int newline = str.IndexOf('\n');
                string upper = str.Substring(0, newline);
                printBuffer.Append(upper, Style);
                NewLine();
                if (newline < str.Length - 1)
                {
                    string lower = str.Substring(newline + 1);
                    Print(lower);
                }
                return;
            }
            printBuffer.Append(str, Style);
            return;
        }

        public void PrintImg(string str)
        {
            printBuffer.Append(new ConsoleImagePart(str, null, 0, 0, 0, -1, -1, false, -1, -1.0f, "")); //182101 PCDRP-Update:画像固定表示機能で修正(コンストラクタの引数追加)
        }

        public void PrintShape(string type, int[] param)
        {
            ConsoleShapePart part = ConsoleShapePart.CreateShape(type, param, userStyle.Color, userStyle.ButtonColor, false);
            printBuffer.Append(part);
        }

        public void PrintHtml(string str, bool noLineBreaks = false)
        {
            if (string.IsNullOrEmpty(str))
                return;
            if (!this.Enabled)
                return;
            if (!printBuffer.IsEmpty)
            {
                ConsoleDisplayLine[] dispList = printBuffer.Flush(stringMeasure, force_temporary);
                addRangeDisplayLine(dispList);
            }
            if (noLineBreaks)
            {
                ConsoleDisplayLine oldLine = displayLineList[displayLineList.Count - 1];
                //throwaway variables for the outs that are only used in the normal method
                bool throwaway;
                DisplayLineAlignment throwaway2;

                List<ConsoleButtonString> buttonList = new List<ConsoleButtonString>();
                buttonList.AddRange(oldLine.Buttons);
                buttonList.AddRange(HtmlManager.Html2ButtonList(str, this, out throwaway, out throwaway2));

                //PrintStringBuffer.setWidthToButtonList(buttonList, stringMeasure, true);

                List<ConsoleDisplayLine> lines = new List<ConsoleDisplayLine>();
                List<ConsoleButtonString> lineButtons = new List<ConsoleButtonString>();
                foreach(var button in buttonList)
                {
                    if(button == null)
                    {
                        if(lineButtons.Count > 0)
                        {
                            lines.Add(new ConsoleDisplayLine(lineButtons.ToArray(), false, false));
                            lineButtons.Clear();
                        }
                    }
                    else
                    {
                        // PRINTL for linebreak fix
                        // TODO: make good
                        if (button.StrArray[0].ToString() == " " && lines.Count == 0)
                            continue;
                        lineButtons.Add(button);
                    }
                }

                if (lineButtons.Count > 0)
                {
                    lines.Add(new ConsoleDisplayLine(lineButtons.ToArray(), false, false));
                }

                foreach (var line in lines)
                {
                    int pointX = 0;
                    foreach (var button in line.Buttons)
                    {
                        button.CalcWidth(stringMeasure, button.XsubPixel);
                        button.CalcPointX(pointX);
                        pointX = button.PointX + button.Width - 1;
                    }
                }

                if(lines[0] != null)
                    displayLineList[displayLineList.Count - 1] = lines[0];
                lines.RemoveAt(0);
                addRangeDisplayLine(lines.ToArray());
            }
            else
            {
                addRangeDisplayLine(HtmlManager.Html2DisplayLine(str, stringMeasure, this));
                RefreshStrings(false);
            }
        }

        private int printCWidth = -1;
        private int printCWidthL = -1;
        private int printCWidthL2 = -1;

        public void PrintC(string str, bool alignmentRight)
        {
            if (string.IsNullOrEmpty(str))
                return;

            printBuffer.Append(CreateTypeCString(str, alignmentRight), Style, true);
        }

        private void calcPrintCWidth(StringMeasure stringMeasure)
        {
            string str = new string(' ', Config.PrintCLength);
            Font font = Config.Font;
            printCWidth = stringMeasure.GetDisplayLength(str, font);

            str += " ";
            printCWidthL = stringMeasure.GetDisplayLength(str, font);

            str += " ";
            printCWidthL2 = stringMeasure.GetDisplayLength(str, font);
        }

        private string CreateTypeCString(string str, bool alignmentRight)
        {
            if (printCWidth == -1)
                calcPrintCWidth(stringMeasure);
            int length = 0;
            int width = 0;
            if (str != null)
                length = Config.Encode.GetByteCount(str);
            int printcLength = Config.PrintCLength;
            Font font = null;
            try
            {
                font = new Font(Style.Fontname, Config.Font.Size, Style.FontStyle, GraphicsUnit.Pixel);
            }
            catch
            {
                return str;
            }

            if ((alignmentRight) && (length < printcLength))
            {
                str = new string(' ', printcLength - length) + str;
                width = stringMeasure.GetDisplayLength(str, font);
                while (width > printCWidth)
                {
                    if (str[0] != ' ')
                        break;
                    str = str.Remove(0, 1);
                    width = stringMeasure.GetDisplayLength(str, font);
                }
            }
            else if ((!alignmentRight) && (length < printcLength + 1))
            {
                str += new string(' ', printcLength + 1 - length);
                width = stringMeasure.GetDisplayLength(str, font);
                while (width > printCWidthL)
                {
                    if (str[str.Length - 1] != ' ')
                        break;
                    str = str.Remove(str.Length - 1, 1);
                    width = stringMeasure.GetDisplayLength(str, font);
                }
            }
            return str;
        }

        internal void PrintButton(string str, string p)
        {
            if (string.IsNullOrEmpty(str))
                return;
            printBuffer.AppendButton(str, Style, p);
        }

        internal void PrintButton(string str, long p)
        {
            if (string.IsNullOrEmpty(str))
                return;
            printBuffer.AppendButton(str, Style, p);
        }

        internal void PrintButtonC(string str, string p, bool isRight)
        {
            if (string.IsNullOrEmpty(str))
                return;
            printBuffer.AppendButton(CreateTypeCString(str, isRight), Style, p);
        }

        internal void PrintButtonC(string str, long p, bool isRight)
        {
            if (string.IsNullOrEmpty(str))
                return;
            printBuffer.AppendButton(CreateTypeCString(str, isRight), Style, p);
        }

        internal void PrintPlain(string str)
        {
            if (string.IsNullOrEmpty(str))
                return;
            printBuffer.AppendPlainText(str, Style);
        }

        public void NewLine()
        {
            PrintFlush(true);
            RefreshStrings(false);
        }

        public ConsoleDisplayLine BufferToSingleLine(bool force, bool temporary)
        {
            if (!this.Enabled)
                return null;
            if (!force && printBuffer.IsEmpty)
                return null;
            if (force && printBuffer.IsEmpty)
                printBuffer.Append(" ", Style);
            ConsoleDisplayLine dispLine = printBuffer.FlushSingleLine(stringMeasure, temporary | force_temporary);
            return dispLine;
        }

        internal ConsoleDisplayLine PrintPlainwithSingleLine(string str)
        {
            if (!this.Enabled)
                return null;
            if (string.IsNullOrEmpty(str))
                return null;
            printBuffer.AppendPlainText(str, Style);
            ConsoleDisplayLine dispLine = printBuffer.FlushSingleLine(stringMeasure, false);
            return dispLine;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="force">バッファーが空でも改行する</param>
        public void PrintFlush(bool force)
        {
            if (!this.Enabled)
                return;
            if (!force && printBuffer.IsEmpty)
                return;
            if (force && printBuffer.IsEmpty)
                printBuffer.Append(" ", Style);
            ConsoleDisplayLine[] dispList = printBuffer.Flush(stringMeasure, force_temporary);
            //ConsoleDisplayLine[] dispList = printBuffer.Flush(stringMeasure, temporary | force_temporary);
            addRangeDisplayLine(dispList);
            //1819描画命令は分離
            //RefreshStrings(false);
        }

        /// <summary>
        /// DRAWLINE命令に対応。これのフォントを変更できると面倒なことになるのでRegularに固定する。
        /// </summary>
        public void PrintBar()
        {
            //初期に設定済みなので見る必要なし
            //if (stBar == null)
            //    setStBar(StaticConfig.DrawLineString);

            //1806beta001 CompatiDRAWLINEの廃止、CompatiLinefeedAs1739へ移行
            //CompatiLinefeedAs1739の処理はPrintStringBuffer.csで行う
            //if (Config.CompatiDRAWLINE)
            //	PrintFlush(false);
            StringStyle ss = userStyle;
            userStyle.FontStyle = FontStyle.Regular;
            Print(stBar);
            userStyle = ss;
        }

        public void printCustomBar(string barStr)
        {
            if (string.IsNullOrEmpty(barStr))
                throw new CodeEE("空文字列によるDRAWLINEが行われました");
            StringStyle ss = userStyle;
            userStyle.FontStyle = FontStyle.Regular;
            Print(getStBar(barStr));
            userStyle = ss;
        }

        public string getDefStBar()
        {
            return stBar;
        }

        public string getStBar(string barStr)
        {
            StringBuilder bar = new StringBuilder();
            bar.Append(barStr);
            int width = 0;
            Font font = Config.Font;
            while (width < Config.DrawableWidth)
            {//境界を越えるまで一文字ずつ増やす
                bar.Append(barStr);
                width = stringMeasure.GetDisplayLength(bar.ToString(), font);
            }
            while (width > Config.DrawableWidth)
            {//境界を越えたら、今度は超えなくなるまで一文字ずつ減らす（barStrに複数字の文字列がきた場合に対応するため）
                bar.Remove(bar.Length - 1, 1);
                width = stringMeasure.GetDisplayLength(bar.ToString(), font);
            }
            return bar.ToString();
        }

        public void setStBar(string barStr)
        {
            stBar = getStBar(barStr);
        }

        #endregion Print系

        private bool outputLog(string fullpath)
        {
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(fullpath, false, Encoding.Unicode);
                foreach (ConsoleDisplayLine line in displayLineList)
                {
                    writer.WriteLine(line.ToString());
                }
            }
            catch (Exception)
            {
                MessageBox.Show("ログの出力に失敗しました", "ログ出力失敗");
                return false;
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
            return true;
        }

        public bool OutputLog(string filename)
        {
            if (filename == null)
                filename = Program.ExeDir + "emuera.log";

            if (!filename.StartsWith(Program.ExeDir, StringComparison.CurrentCultureIgnoreCase))
            {
                MessageBox.Show("ログファイルは実行ファイル以下のディレクトリにのみ保存できます", "ログ出力失敗");
                return false;
            }

            if (outputLog(filename))
            {
                if (window.Created)
                {
                    PrintSystemLine("※※※ログファイルを" + filename + "に出力しました※※※");
                    RefreshStrings(true);
                }
                return true;
            }
            else
                return false;
        }

        // used exclusively for clipboard, so it's gonna do some HTML tag stripping for us
        public void GetDisplayStrings(StringBuilder builder)
        {
            if (displayLineList.Count == 0)
                return;
            for (int i = 0; i < displayLineList.Count; i++)
            {
                builder.AppendLine(ClipboardProcessor.StripHTML(displayLineList[i].ToString()));
            }
        }

        public ConsoleDisplayLine[] GetDisplayLines(Int64 lineNo)
        {
            if (lineNo < 0 || lineNo > displayLineList.Count)
                return null;
            int count = 0;
            List<ConsoleDisplayLine> list = new List<ConsoleDisplayLine>();
            for (int i = displayLineList.Count - 1; i >= 0; i--)
            {
                if (count == lineNo)
                    list.Insert(0, displayLineList[i]);
                if (displayLineList[i].IsLogicalLine)
                    count++;
                if (count > lineNo)
                    break;
            }
            if (list.Count == 0)
                return null;
            ConsoleDisplayLine[] ret = new ConsoleDisplayLine[list.Count];
            list.CopyTo(ret);
            return ret;
        }

        public ConsoleDisplayLine[] PopDisplayingLines()
        {
            if (!this.Enabled)
                return null;
            if (printBuffer.IsEmpty)
                return null;
            return printBuffer.Flush(stringMeasure, force_temporary);
        }
    }
}