using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using MinorShift.Emuera.GameData;
using MinorShift.Emuera.Sub;
using MinorShift.Emuera.GameView;
using MinorShift.Emuera.GameData.Expression;
using MinorShift.Emuera.GameData.Variable;
using MinorShift.Emuera.GameProc.Function;
using MinorShift.Emuera.GameData.Function;

namespace MinorShift.Emuera.GameProc
{

	internal sealed partial class Process
	{
		public Process(EmueraConsole view)
		{
			console = view;
		}

        public LogicalLine getCurrentLine { get { return state.CurrentLine; } }

		/// <summary>
		/// @~~と$~~を集めたもの。CALL命令などで使う
		/// 実行順序はLogicalLine自身が保持する。
		/// </summary>
		LabelDictionary labelDic;
		public LabelDictionary LabelDictionary { get { return labelDic; } }

		/// <summary>
		/// 変数全部。スクリプト中で必要になる変数は（ユーザーが直接触れないものも含め）この中にいれる
		/// </summary>
		private VariableEvaluator vEvaluator;
		public VariableEvaluator VEvaluator { get { return vEvaluator; } }
		private ExpressionMediator exm;
		private GameBase gamebase;
		readonly EmueraConsole console;
		private IdentifierDictionary idDic;
		ProcessState state;
		ProcessState originalState;//リセットする時のために
        bool noError = false;
        //色々あって復活させてみる
        bool initialiing;
        public bool inInitializeing { get { return initialiing;  } }

        public bool Initialize()
		{
			LexicalAnalyzer.UseMacro = false;
            state = new ProcessState(console);
            originalState = state;
            initialiing = true;
            try
            {
				ParserMediator.Initialize(console);
				if (ParserMediator.HasWarning)
				{
					ParserMediator.FlushWarningList();
					if(MessageBox.Show("There is a problem with the config file.\nWould you like to close Emuera?","Config Error", MessageBoxButtons.YesNo)
						== DialogResult.Yes)
					{
						console.PrintSystemLine("Processing was terminated because there was a problem with the config file and exit was selected");
						return false;
					}
				}
                //182101 PCDRP-Update:画像固定表示機能,画面エフェクト機能で修正↓--------------------------
                //CONFIGの組み合わせチェック。現在画像固定表示機能はGRAPHICSを固定で使用するため、
                //従来描写のGDIと混ぜて使用するとExceptionが発生する。
                //このためWINAPI指定を禁止する。
                //(TODO:今のところ頑張るつもりはないが、画像固定表示をGDI対応したら撤廃するかも…)
                if (Config.FixImgGroupNumber >= 0 && Config.TextDrawingMode == TextDrawingMode.WINAPI)
                {
                    console.PrintSystemLine("画像固定表示機能使用時は描画インターフェースをWINAPIに指定できません。TEXTRENDERERかGRAPHICSを使用してください。");
                    console.PrintSystemLine("コンフィグファイルに異常があり、終了が選択されたため処理を終了しました");
					return false;
				}
                //182101 PCDRP-Update:画像固定表示機能,画面エフェクト機能で修正↑--------------------------
                //画像の読み込みはここ
				Content.AppContents.LoadContents();
				
                if (Config.UseKeyMacro && !Program.AnalysisMode)
                {
                    if (File.Exists(Program.ExeDir + "macro.txt"))
                    {
                        if (Config.DisplayReport)
							console.PrintSystemLine("Loading macro.txt...");
                        KeyMacro.LoadMacroFile(Program.ExeDir + "macro.txt");
                    }
                }
                if (Config.UseReplaceFile && !Program.AnalysisMode)
                {
					if (File.Exists(Program.CsvDir + "_Replace.csv"))
					{
						if (Config.DisplayReport)
							console.PrintSystemLine("Loading _Replace.csv...");
						ConfigData.Instance.LoadReplaceFile(Program.CsvDir + "_Replace.csv");
						if (ParserMediator.HasWarning)
						{
							ParserMediator.FlushWarningList();
							if (MessageBox.Show("Abnormality is found in _Replace.csv.\nWould you like to close Emuera?", "_Replace.csv Error", MessageBoxButtons.YesNo)
								== DialogResult.Yes)
							{
								console.PrintSystemLine("Processing was terminated because _Replace.csv file had an error and exit was selected");
								return false;
							}
						}
					}
                }
                Config.SetReplace(ConfigData.Instance);
                //ここでBARを設定すれば、いいことに気づいた予感
                console.setStBar(Config.DrawLineString);

				if (Config.UseRenameFile)
                {
					if (File.Exists(Program.CsvDir + "_Rename.csv"))
                    {
                        if (Config.DisplayReport || Program.AnalysisMode)
							console.PrintSystemLine("Loading _Rename.csv...");
						ParserMediator.LoadEraExRenameFile(Program.CsvDir + "_Rename.csv");
                    }
                    else
                        console.PrintError("csv\\_Rename.csv is missing");
                }
                if (!Config.DisplayReport)
                {
                    console.PrintSingleLine(Config.LoadLabel);
                    console.RefreshStrings(true);
                }
				gamebase = new GameBase();
                if (!gamebase.LoadGameBaseCsv(Program.CsvDir + "GAMEBASE.CSV"))
                {
                    console.PrintSystemLine("Processing was terminated because a problem occurred while loading GAMEBASE.CSV");
                    return false;
                }
				console.SetWindowTitle(gamebase.ScriptWindowTitle);
				GlobalStatic.GameBaseData = gamebase;

				ConstantData constant = new ConstantData(gamebase);
				constant.LoadData(Program.CsvDir, console, Config.DisplayReport);
				GlobalStatic.ConstantData = constant;
				TrainName = constant.GetCsvNameList(VariableCode.TRAINNAME);

                vEvaluator = new VariableEvaluator(gamebase, constant);
				GlobalStatic.VEvaluator = vEvaluator;

				idDic = new IdentifierDictionary(vEvaluator.VariableData);
				GlobalStatic.IdentifierDictionary = idDic;

				StrForm.Initialize();
				VariableParser.Initialize();

				exm = new ExpressionMediator(this, vEvaluator, console);
				GlobalStatic.EMediator = exm;

				labelDic = new LabelDictionary();
				GlobalStatic.LabelDictionary = labelDic;
				HeaderFileLoader hLoader = new HeaderFileLoader(console, idDic, this);

				LexicalAnalyzer.UseMacro = false;
				if (!hLoader.LoadHeaderFiles(Program.ErbDir, Config.DisplayReport))
				{
					console.PrintSystemLine("Processing was terminated because a problem occurred while loading ERH files");
					return false;
				}
				LexicalAnalyzer.UseMacro = idDic.UseMacro();

				ErbLoader loader = new ErbLoader(console, exm, this);
                if (Program.AnalysisMode)
                    noError = loader.loadErbs(Program.AnalysisFiles, labelDic);
                else
                    noError = loader.LoadErbFiles(Program.ErbDir, Config.DisplayReport, labelDic);
                initSystemProcess();
                initialiing = false;
            }
			catch (Exception e)
			{
				handleException(e, null, true);
				console.PrintSystemLine("Processing was terminated because a fatal error has occurred during initialization");
				return false;
			}
			if (labelDic == null)
			{
				return false;
			}
			state.Begin(BeginType.TITLE);
			GC.Collect();
            return true;
		}

		public void ReloadErb()
		{
			saveCurrentState(false);
			state.SystemState = SystemStateCode.System_Reloaderb;
			ErbLoader loader = new ErbLoader(console, exm, this);
            loader.LoadErbFiles(Program.ErbDir, false, labelDic);
			console.ReadAnyKey();
		}

		public void ReloadPartialErb(List<string> path)
		{
			saveCurrentState(false);
			state.SystemState = SystemStateCode.System_Reloaderb;
			ErbLoader loader = new ErbLoader(console, exm, this);
			loader.loadErbs(path, labelDic);
			console.ReadAnyKey();
		}

		public void SetCommnds(Int64 count)
		{
			coms = new List<long>((int)count);
			isCTrain = true;
			Int64[] selectcom = vEvaluator.SELECTCOM_ARRAY;
			if (count >= selectcom.Length)
			{
				throw new CodeEE("The value of the arguments of the CALLTRAIN instruction exceeds the number of SELECTCOM elements");
			}
			for (int i = 0; i < (int)count; i++)
			{
				coms.Add(selectcom[i + 1]);
			}
		}

        public bool ClearCommands()
        {
            coms.Clear();
            count = 0;
            isCTrain = false;
            skipPrint = true;
            return (callFunction("CALLTRAINEND", false, false));
        }

		public void InputInteger(Int64 i)
		{
			vEvaluator.RESULT = i;
		}
		public void InputSystemInteger(Int64 i)
		{
			systemResult = i;
		}
		public void InputString(string s)
		{
			vEvaluator.RESULTS = s;
		}

		private uint startTime = 0;
		
		public void DoScript()
		{
			startTime = _Library.WinmmTimer.TickCount;
			state.lineCount = 0;
			bool systemProcRunning = true;
			try
			{
				while (true)
				{
					methodStack = 0;
					systemProcRunning = true;
					while (state.ScriptEnd && console.IsRunning)
						runSystemProc();
					if (!console.IsRunning)
						break;
					systemProcRunning = false;
					runScriptProc();
				}
			}
			catch (Exception ec)
			{
				LogicalLine currentLine = state.ErrorLine;
				if (currentLine != null && currentLine is NullLine)
					currentLine = null;
				if (systemProcRunning)
					handleExceptionInSystemProc(ec, currentLine, true);
				else
					handleException(ec, currentLine, true);
			}
		}
		
		public void BeginTitle()
		{
			vEvaluator.ResetData();
			state = originalState;
			state.Begin(BeginType.TITLE);
		}

		private void checkInfiniteLoop()
		{
			//うまく動かない。BEEP音が鳴るのを止められないのでこの処理なかったことに（1.51）
			////フリーズ防止。処理中でも履歴を見たりできる
			//System.Windows.Forms.Application.DoEvents();
			////System.Threading.Thread.Sleep(0);

			//if (!console.Enabled)
			//{
			//    //DoEvents()の間にウインドウが閉じられたらおしまい。
			//    console.ReadAnyKey();
			//    return;
			//}
			uint time = _Library.WinmmTimer.TickCount - startTime;
			if (time < Config.InfiniteLoopAlertTime)
				return;
			LogicalLine currentLine = state.CurrentLine;
			if ((currentLine == null) || (currentLine is NullLine))
				return;//現在の行が特殊な状態ならスルー
			if (!console.Enabled)
				return;//クローズしてるとMessageBox.Showができないので。
			string caption = string.Format("There is a possibility of an infinite loop");
			string text = string.Format(
				"Currently {1} line at {0} is being executed.\nSince the last input {3} milliseconds has passed and the line was executed {2} times.\nWould you like to forcibly terminate processing?",
				currentLine.Position.Filename, currentLine.Position.LineNo, state.lineCount, time);
			DialogResult result = MessageBox.Show(text, caption, MessageBoxButtons.YesNo);
			if (result == DialogResult.Yes)
			{
				throw new CodeEE("Forced termination was selected due to suspected infinite loop");
			}
			else
			{
				state.lineCount = 0;
				startTime = _Library.WinmmTimer.TickCount;
			}
		}

		int methodStack = 0;
		public SingleTerm GetValue(SuperUserDefinedMethodTerm udmt,bool translate = false)
		{
			methodStack++;
            if (methodStack > 100)
            {
                //StackOverflowExceptionはcatchできない上に再現性がないので発生前に一定数で打ち切る。
                //環境によっては100以前にStackOverflowExceptionがでるかも？
                throw new CodeEE("Call stack of the function has overflowed (is it being recursively called indefinitely?)");
            }
            SingleTerm ret = null;
            int temp_current = state.currentMin;
            state.currentMin = state.functionCount;
            udmt.Call.updateRetAddress(state.CurrentLine);
            try
            {
				state.IntoFunction(udmt.Call, udmt.Argument, exm);
                //do whileの中でthrow されたエラーはここではキャッチされない。
				//#functionを全て抜けてDoScriptでキャッチされる。
    			runScriptProc(translate);
                ret = state.MethodReturnValue;
			}
			finally
			{
				if (udmt.Call.TopLabel.hasPrivDynamicVar)
					udmt.Call.TopLabel.Out();
                //1756beta2+v3:こいつらはここにないとデバッグコンソールで式中関数が事故った時に大事故になる
                state.currentMin = temp_current;
                methodStack--;
            }
			return ret;
		}

        public void clearMethodStack()
        {
            methodStack = 0;
        }

        public int MethodStack()
        {
            return methodStack;
        }

		public ScriptPosition GetRunningPosition()
		{
			LogicalLine line = state.ErrorLine;
			if (line == null)
				return null;
			return line.Position;
		}

		private readonly string scaningScope = null;
		private string GetScaningScope()
		{
			if (scaningScope != null)
				return scaningScope;
			return state.Scope;
		}

		public LogicalLine scaningLine = null;
		internal LogicalLine GetScaningLine()
		{
			if (scaningLine != null)
				return scaningLine;
			LogicalLine line = state.ErrorLine;
			if (line == null)
				return null;
			return line;
		}
		
		
		private void handleExceptionInSystemProc(Exception exc, LogicalLine current, bool playSound)
		{
			console.ThrowError(playSound);
			if (exc is CodeEE)
			{
				console.PrintError("An error has occurred at the end of the function: " + Program.ExeName);
				console.PrintError(exc.Message);
			}
			else if (exc is ExeEE)
			{
				console.PrintError("An Emuera error has occurred at the end of the function: " + Program.ExeName);
				console.PrintError(exc.Message);
			}
			else
			{
				console.PrintError("An unexpected error has occurred at the end of the function: " + Program.ExeName);
				console.PrintError(exc.GetType().ToString() + ":" + exc.Message);
				string[] stack = exc.StackTrace.Split('\n');
				for (int i = 0; i < stack.Length; i++)
				{
					console.PrintError(stack[i]);
				}
			}
		}
		
		private void handleException(Exception exc, LogicalLine current, bool playSound)
		{
			console.ThrowError(playSound);
			ScriptPosition position = null;
			EmueraException ee = exc as EmueraException;
			if((ee != null) && (ee.Position != null))
				position = ee.Position;
            else if ((current != null) && (current.Position != null))
				position = current.Position;
			string posString = "";
			if (position != null)
			{
				if (position.LineNo >= 0)
					posString = position.Filename + " at line " + position.LineNo.ToString() + " ";
				else
					posString = position.Filename + "で";
					
			}
			if (exc is CodeEE)
			{
                if (position != null)
				{
                    InstructionLine procline = current as InstructionLine;
                    if (procline != null && procline.FunctionCode == FunctionCode.THROW)
                    {
                        console.PrintErrorButton(posString + "THROW has occurred", position);
                        if (position.RowLine != null)
                            console.PrintError(position.RowLine);
                        console.PrintError("THROW contents: " + exc.Message);
                    }
                    else
                    {
						console.PrintErrorButton(posString + "Error has occurred:" + Program.ExeName, position);
                        if (position.RowLine != null)
                            console.PrintError(position.RowLine);
                        console.PrintError("Error description: " + exc.Message);
                    }
                    console.PrintError("Function: @" + current.ParentLabelLine.LabelName + "（file:" + current.ParentLabelLine.Position.Filename + " in line " + current.ParentLabelLine.Position.LineNo.ToString() + "）");
                    console.PrintError("Function call stack: ");
                    LogicalLine parent = null;
                    int depth = 0;
                    while ((parent = state.GetReturnAddressSequensial(depth++)) != null)
                    {
                        if (parent.Position != null)
                        {
                            console.PrintErrorButton("↑" + parent.Position.Filename + " at line " + parent.Position.LineNo.ToString() + "（function@" + parent.ParentLabelLine.LabelName + "）", parent.Position);
                        }
                    } 
				}
				else
				{
					console.PrintError(posString + "Error has occurred: " + Program.ExeName);
					console.PrintError(exc.Message);
				}
			}
			else if (exc is ExeEE)
			{
				console.PrintError(posString + "Emuera has encountered an error: " + Program.ExeName);
				console.PrintError(exc.Message);
			}
			else
            {
				console.PrintError(posString + "Unexpected error has occurred: " + Program.ExeName);
				console.PrintError(exc.GetType().ToString() + ":" + exc.Message);
				string[] stack = exc.StackTrace.Split('\n');
				for (int i = 0; i < stack.Length; i++)
				{
					console.PrintError(stack[i]);
				}
			}
		}


	}
}
