﻿using System.Text;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System;
using System.Windows.Forms;
using MinorShift._Library;

namespace MinorShift.Emuera
{

	internal static class Config
	{

		#region config

		public static Encoding Encode = Encoding.GetEncoding("GBK");
		public static Encoding SaveEncode = Encoding.GetEncoding("GBK");
		private static Dictionary<ConfigCode, string> nameDic = null;
		public static string GetConfigName(ConfigCode code)
		{
			return nameDic[code];
		}

		public static void SetConfig(ConfigData instance)
		{
			nameDic = instance.GetConfigNameDic();
			IgnoreCase = instance.GetConfigValue<bool>(ConfigCode.IgnoreCase);
			CompatiFunctionNoignoreCase = instance.GetConfigValue<bool>(ConfigCode.CompatiFunctionNoignoreCase);
			ICFunction = IgnoreCase && !CompatiFunctionNoignoreCase;
			ICVariable = IgnoreCase;
			if (IgnoreCase)
			{
				if (CompatiFunctionNoignoreCase)
					SCFunction = StringComparison.Ordinal;
				else
					SCFunction = StringComparison.OrdinalIgnoreCase;
				SCVariable = StringComparison.OrdinalIgnoreCase;
			}
			else
			{
				SCFunction = StringComparison.Ordinal;
				SCVariable = StringComparison.Ordinal;
			}
			UseRenameFile = instance.GetConfigValue<bool>(ConfigCode.UseRenameFile);
			UseReplaceFile = instance.GetConfigValue<bool>(ConfigCode.UseReplaceFile);
			UseMouse = instance.GetConfigValue<bool>(ConfigCode.UseMouse);
			UseMenu = instance.GetConfigValue<bool>(ConfigCode.UseMenu);
			UseDebugCommand = instance.GetConfigValue<bool>(ConfigCode.UseDebugCommand);
			AllowMultipleInstances = instance.GetConfigValue<bool>(ConfigCode.AllowMultipleInstances);
			AutoSave = instance.GetConfigValue<bool>(ConfigCode.AutoSave);
			UseKeyMacro = instance.GetConfigValue<bool>(ConfigCode.UseKeyMacro);
			SizableWindow = instance.GetConfigValue<bool>(ConfigCode.SizableWindow);
			//UseImageBuffer = instance.GetConfigValue<bool>(ConfigCode.UseImageBuffer);
			TextDrawingMode = instance.GetConfigValue<TextDrawingMode>(ConfigCode.TextDrawingMode);
			WindowX = instance.GetConfigValue<int>(ConfigCode.WindowX);
			WindowY = instance.GetConfigValue<int>(ConfigCode.WindowY);
			WindowPosX = instance.GetConfigValue<int>(ConfigCode.WindowPosX);
			WindowPosY = instance.GetConfigValue<int>(ConfigCode.WindowPosY);
			SetWindowPos = instance.GetConfigValue<bool>(ConfigCode.SetWindowPos);
			MaxLog = instance.GetConfigValue<int>(ConfigCode.MaxLog);
			PrintCPerLine = instance.GetConfigValue<int>(ConfigCode.PrintCPerLine);
            PrintCLength = instance.GetConfigValue<int>(ConfigCode.PrintCLength);
            PrintCShopLength = instance.GetConfigValue<int>(ConfigCode.PrintCShopLength);   //182101 PCDRP-Update:ショップだけ文字数が違い過ぎるので対応個別
            ForeColor = instance.GetConfigValue<Color>(ConfigCode.ForeColor);
			BackColor = instance.GetConfigValue<Color>(ConfigCode.BackColor);
			FocusColor = instance.GetConfigValue<Color>(ConfigCode.FocusColor);
			LogColor = instance.GetConfigValue<Color>(ConfigCode.LogColor);
			FontSize = instance.GetConfigValue<int>(ConfigCode.FontSize);
			FontName = instance.GetConfigValue<string>(ConfigCode.FontName);
			LineHeight = instance.GetConfigValue<int>(ConfigCode.LineHeight);
			FPS = instance.GetConfigValue<int>(ConfigCode.FPS);
			//SkipFrame = instance.GetConfigValue<int>(ConfigCode.SkipFrame);
			ScrollHeight = instance.GetConfigValue<int>(ConfigCode.ScrollHeight);
			InfiniteLoopAlertTime = instance.GetConfigValue<int>(ConfigCode.InfiniteLoopAlertTime);
			SaveDataNos = instance.GetConfigValue<int>(ConfigCode.SaveDataNos);
			WarnBackCompatibility = instance.GetConfigValue<bool>(ConfigCode.WarnBackCompatibility);
			WindowMaximixed = instance.GetConfigValue<bool>(ConfigCode.WindowMaximixed);
			WarnNormalFunctionOverloading = instance.GetConfigValue<bool>(ConfigCode.WarnNormalFunctionOverloading);
			SearchSubdirectory = instance.GetConfigValue<bool>(ConfigCode.SearchSubdirectory);
			SortWithFilename = instance.GetConfigValue<bool>(ConfigCode.SortWithFilename);
            FixImgGroupNumber = instance.GetConfigValue<int>(ConfigCode.FixImgGroupNumber);         //182101 PCDRP-Update
            ScreenEffectInterval = instance.GetConfigValue<int>(ConfigCode.ScreenEffectInterval);   //182101 PCDRP-Update
            EdgeColor = instance.GetConfigValue<Color>(ConfigCode.EdgeColor);                       //182101 PCDRP-Update
            // Anchor edge color so we can reset to config value later if we want via an instruction
            GameView.PrintEdgeFont.currentEdgeColor = EdgeColor;


            AllowFunctionOverloading = instance.GetConfigValue<bool>(ConfigCode.AllowFunctionOverloading);//182101 PCDRP-Update
			if (!AllowFunctionOverloading)
				WarnFunctionOverloading = true;
			else
				WarnFunctionOverloading = instance.GetConfigValue<bool>(ConfigCode.WarnFunctionOverloading);

			DisplayWarningLevel = instance.GetConfigValue<int>(ConfigCode.DisplayWarningLevel);
			DisplayReport = instance.GetConfigValue<bool>(ConfigCode.DisplayReport);
			ReduceArgumentOnLoad = instance.GetConfigValue<ReduceArgumentOnLoadFlag>(ConfigCode.ReduceArgumentOnLoad);
			IgnoreUncalledFunction = instance.GetConfigValue<bool>(ConfigCode.IgnoreUncalledFunction);
			FunctionNotFoundWarning = instance.GetConfigValue<DisplayWarningFlag>(ConfigCode.FunctionNotFoundWarning);
			FunctionNotCalledWarning = instance.GetConfigValue<DisplayWarningFlag>(ConfigCode.FunctionNotCalledWarning);


			ChangeMasterNameIfDebug = instance.GetConfigValue<bool>(ConfigCode.ChangeMasterNameIfDebug);
			LastKey = instance.GetConfigValue<long>(ConfigCode.LastKey);
			ButtonWrap = instance.GetConfigValue<bool>(ConfigCode.ButtonWrap);

			TextEditor = instance.GetConfigValue<string>(ConfigCode.TextEditor);
            EditorType = instance.GetConfigValue<TextEditorType>(ConfigCode.EditorType);
			EditorArg = instance.GetConfigValue<string>(ConfigCode.EditorArgument);

			CompatiErrorLine = instance.GetConfigValue<bool>(ConfigCode.CompatiErrorLine);
			CompatiCALLNAME = instance.GetConfigValue<bool>(ConfigCode.CompatiCALLNAME);
			UseSaveFolder = instance.GetConfigValue<bool>(ConfigCode.UseSaveFolder);
			CompatiRAND = instance.GetConfigValue<bool>(ConfigCode.CompatiRAND);
			//CompatiDRAWLINE = instance.GetConfigValue<bool>(ConfigCode.CompatiDRAWLINE);
			CompatiLinefeedAs1739 = instance.GetConfigValue<bool>(ConfigCode.CompatiLinefeedAs1739);
			SystemAllowFullSpace = instance.GetConfigValue<bool>(ConfigCode.SystemAllowFullSpace);
			SystemSaveInUTF8 = instance.GetConfigValue<bool>(ConfigCode.SystemSaveInUTF8);
			if (SystemSaveInUTF8)
				SaveEncode = Encoding.GetEncoding("UTF-8");
			SystemSaveInBinary = instance.GetConfigValue<bool>(ConfigCode.SystemSaveInBinary);
			SystemIgnoreTripleSymbol = instance.GetConfigValue<bool>(ConfigCode.SystemIgnoreTripleSymbol);
			
			CompatiFuncArgAutoConvert = instance.GetConfigValue<bool>(ConfigCode.CompatiFuncArgAutoConvert);
			CompatiFuncArgOptional = instance.GetConfigValue<bool>(ConfigCode.CompatiFuncArgOptional);
			CompatiCallEvent = instance.GetConfigValue<bool>(ConfigCode.CompatiCallEvent);
			CompatiSPChara = instance.GetConfigValue<bool>(ConfigCode.CompatiSPChara);

            AllowLongInputByMouse = instance.GetConfigValue<bool>(ConfigCode.AllowLongInputByMouse);

           TimesNotRigorousCalculation = instance.GetConfigValue<bool>(ConfigCode.TimesNotRigorousCalculation);
            //一文字変数の禁止オプションを考えた名残
		   //ForbidOneCodeVariable = instance.GetConfigValue<bool>(ConfigCode.ForbidOneCodeVariable);
		   SystemNoTarget = instance.GetConfigValue<bool>(ConfigCode.SystemNoTarget);

            CBUseClipboard = instance.GetConfigValue<bool>(ConfigCode.CBUseClipboard);
            CBIgnoreTags = instance.GetConfigValue<bool>(ConfigCode.CBIgnoreTags);
            CBReplaceTags = instance.GetConfigValue<string>(ConfigCode.CBReplaceTags);
            CBNewLinesOnly = instance.GetConfigValue<bool>(ConfigCode.CBNewLinesOnly);
            CBClearBuffer = instance.GetConfigValue<bool>(ConfigCode.CBClearBuffer);
            CBTriggerLeftClick = instance.GetConfigValue<bool>(ConfigCode.CBTriggerLeftClick);
            CBTriggerMiddleClick = instance.GetConfigValue<bool>(ConfigCode.CBTriggerMiddleClick);
            CBTriggerDoubleLeftClick = instance.GetConfigValue<bool>(ConfigCode.CBTriggerDoubleLeftClick);
            CBTriggerAnyKeyWait = instance.GetConfigValue<bool>(ConfigCode.CBTriggerAnyKeyWait);
            CBTriggerInputWait = instance.GetConfigValue<bool>(ConfigCode.CBTriggerInputWait);
            CBMaxCB = instance.GetConfigValue<int>(ConfigCode.CBMaxCB);
            CBBufferSize = instance.GetConfigValue<int>(ConfigCode.CBBufferSize);
            CBScrollCount = instance.GetConfigValue<int>(ConfigCode.CBScrollCount);
            CBMinTimer = instance.GetConfigValue<int>(ConfigCode.CBMinTimer);

            UseLanguage lang = instance.GetConfigValue<UseLanguage>(ConfigCode.useLanguage);
            switch (lang)
            {
                case UseLanguage.JAPANESE:
                    Language = 0x0411; LangManager.setEncode(932); break;
                case UseLanguage.KOREAN:
                    Language = 0x0412; LangManager.setEncode(949);  break;
                case UseLanguage.CHINESE_HANS:
                    Language = 0x0804; LangManager.setEncode(936); break;
                case UseLanguage.CHINESE_HANT:
                    Language = 0x0404; LangManager.setEncode(950); break;
            }

			if (FontSize < 8)
			{
				MessageBox.Show("字体大小过小(至少为8)", "设置错误");
				FontSize = 8;
			}
			if (LineHeight < FontSize)
			{
				MessageBox.Show("由于行高比字体小，因此解释为与字体大小相同的高度", "设置错误");
				LineHeight = FontSize;
			}
			if (SaveDataNos < 20)
			{
				MessageBox.Show("可显示的存档数据数量过少(至少为20)", "设置错误");
				SaveDataNos = 20;
			}
			if (SaveDataNos > 80)
			{
				MessageBox.Show("可显示的存档数据数量过多(至多为80)", "设置错误");
				SaveDataNos = 80;
			}
			if (MaxLog < 500)
			{
				MessageBox.Show("日志行数过少(至少为500)", "设置错误");
				MaxLog = 500;
			}

			DrawingParam_ShapePositionShift = 0;
			if (TextDrawingMode != TextDrawingMode.WINAPI)
				DrawingParam_ShapePositionShift = Math.Max(2, FontSize / 6);
			DrawableWidth = WindowX - DrawingParam_ShapePositionShift;

			if (UseSaveFolder)
				SavDir = Program.ExeDir + "sav\\";
			else
				SavDir = Program.ExeDir;
			if (UseSaveFolder && !Directory.Exists(SavDir))
				createSavDirAndMoveFiles();
		}


		static Dictionary<string, Dictionary<FontStyle, Font>> fontDic = new Dictionary<string, Dictionary<FontStyle, Font>>();
		public static Font Font { get { return FontFactory.GetFont(null, FontStyle.Regular); } }

		public static Font GetFont(string theFontname, FontStyle style)
		{
			string fn = theFontname;
			if (string.IsNullOrEmpty(theFontname))
				fn = FontName;
			if (!fontDic.ContainsKey(fn))
				fontDic.Add(fn, new Dictionary<FontStyle, Font>());
			Dictionary<FontStyle, Font> fontStyleDic = fontDic[fn];
			if (!fontStyleDic.ContainsKey(style))
			{
				int fontsize = FontSize;
				Font styledFont = null;
				try
				{
					styledFont = new Font(fn, fontsize, style, GraphicsUnit.Pixel);
				}
				catch
				{
					return null;
				}
				fontStyleDic.Add(style, styledFont);
			}
			return fontStyleDic[style];
		}

		public static void ClearFont()
		{
			foreach (KeyValuePair<string, Dictionary<FontStyle, Font>> fontStyleDicPair in fontDic)
			{
				foreach (KeyValuePair<FontStyle, Font> pair in fontStyleDicPair.Value)
				{
					pair.Value.Dispose();
				}
				fontStyleDicPair.Value.Clear();
			}
			fontDic.Clear();
		}

		/// <summary>
		/// ディレクトリ作成失敗のExceptionは呼び出し元で処理すること
		/// </summary>
		public static void CreateSavDir()
		{
			if (UseSaveFolder && !Directory.Exists(SavDir))
			{
				Directory.CreateDirectory(SavDir);
			}
		}

		private static void createSavDirAndMoveFiles()
		{
			try
			{
				Directory.CreateDirectory(SavDir);
			}
			catch
			{
				MessageBox.Show("创建 sav 目录失败", "创建目录失败");
				return;
			}
			bool existGlobal = File.Exists(Program.ExeDir + "global.sav");
			string[] savFiles = Directory.GetFiles(Program.ExeDir, "save*.sav", SearchOption.TopDirectoryOnly);
			if (!existGlobal && savFiles.Length == 0)
				return;
			DialogResult result = MessageBox.Show("成功创建 sav 目录\n要将当前数据移动到 sav 目录内吗？", "移动存档", MessageBoxButtons.YesNo);
			if (result != DialogResult.Yes)
				return;
			//ダイアログが開いている間にフォルダを消してしまうような邪悪なユーザーがいるかもしれない
			if (!Directory.Exists(SavDir))
			{
				MessageBox.Show("未找到创建的 sav 目录", "创建目录失败");
				return;
			}
			//ダイアログが開いている間にファイルを変更するような邪悪なユーザーがいるかもしれない
			try
			{
				if (File.Exists(Program.ExeDir + "global.sav"))
					File.Move(Program.ExeDir + "global.sav", SavDir + "global.sav");
				savFiles = Directory.GetFiles(Program.ExeDir, "save*.sav", SearchOption.TopDirectoryOnly);
				foreach (string oldpath in savFiles)
					File.Move(oldpath, SavDir + Path.GetFileName(oldpath));
			}
			catch
			{
				MessageBox.Show("移动到 sav 目录时失败", "移动失败");
			}
		}
		//先にSetConfigを呼ぶこと
		//戻り値はセーブが必要かどうか
		public static bool CheckUpdate()
		{
			if (ReduceArgumentOnLoad != ReduceArgumentOnLoadFlag.ONCE)
			{
				if (ReduceArgumentOnLoad == ReduceArgumentOnLoadFlag.YES)
					NeedReduceArgumentOnLoad = true;
				else if (ReduceArgumentOnLoad == ReduceArgumentOnLoadFlag.NO)
					NeedReduceArgumentOnLoad = false;
				return false;
			}

			bool updated = true;
			long key = getUpdateKey();
			updated = LastKey != key;
			LastKey = key;
			return updated;
		}

		private static long getUpdateKey()
		{
			SearchOption option = SearchOption.TopDirectoryOnly;
			if (SearchSubdirectory)
				option = SearchOption.AllDirectories;
			string[] erbFiles = Directory.GetFiles(Program.ErbDir, "*.ERB", option);
			string[] csvFiles = Directory.GetFiles(Program.CsvDir, "*.CSV", option);
			long[] writetimes = new long[erbFiles.Length + csvFiles.Length];
			for (int i = 0; i < erbFiles.Length; i++)
				if (Path.GetExtension(erbFiles[i]).Equals(".ERB", StringComparison.OrdinalIgnoreCase))
					writetimes[i] = System.IO.File.GetLastWriteTime(erbFiles[i]).ToBinary();
			for (int i = 0; i < csvFiles.Length; i++)
				if (Path.GetExtension(csvFiles[i]).Equals(".CSV", StringComparison.OrdinalIgnoreCase))
					writetimes[i + erbFiles.Length] = System.IO.File.GetLastWriteTime(csvFiles[i]).ToBinary();
			long key = 0;
			for (int i = 0; i < writetimes.Length; i++)
			{
				unchecked
				{
					key ^= writetimes[i] * 1103515245 + 12345;
				}
			}
			return key;
		}


		public static List<KeyValuePair<string, string>> GetFiles(string rootdir, string pattern)
		{
			return getFiles(rootdir, rootdir, pattern, !SearchSubdirectory, SortWithFilename);
		}

		private sealed class StrIgnoreCaseComparer : IComparer<string>
		{
			public int Compare(string x, string y)
			{
				return string.Compare(x, y, StringComparison.OrdinalIgnoreCase);
			}
		}
		static StrIgnoreCaseComparer ignoreCaseComparer = new StrIgnoreCaseComparer();

		//KeyValuePair<相対パス, 完全パス>のリストを返す。
		private static List<KeyValuePair<string, string>> getFiles(string dir, string rootdir, string pattern, bool toponly, bool sort)
		{
			StringComparison strComp = StringComparison.OrdinalIgnoreCase;
			List<KeyValuePair<string, string>> retList = new List<KeyValuePair<string, string>>();
			if (!toponly)
			{//サブフォルダ内の検索
				string[] dirList = Directory.GetDirectories(dir, "*", SearchOption.TopDirectoryOnly);
				if (dirList.Length > 0)
				{
					if (sort)
						Array.Sort(dirList, ignoreCaseComparer);
					for (int i = 0; i < dirList.Length; i++)
						retList.AddRange(getFiles(dirList[i], rootdir, pattern, toponly, sort));
				}
			}
			string RelativePath = "";//相対ディレクトリ名
			if (string.Equals(dir, rootdir, strComp))//現在のパスが検索ルートパスに等しい
				RelativePath = "";
			else
			{
				if (!dir.StartsWith(rootdir, strComp))
					RelativePath = dir;
				else
					RelativePath = dir.Substring(rootdir.Length);//前方が検索ルートパスと一致するならその部分を切り取る
				if (!RelativePath.EndsWith("\\") && !RelativePath.EndsWith("/"))
					RelativePath += "\\";//末尾が\又は/で終わるように。後でFile名を直接加算できるようにしておく
			}
			//filepathsは完全パスである
			string[] filepaths = Directory.GetFiles(dir, pattern, SearchOption.TopDirectoryOnly);
			if (sort)
				Array.Sort(filepaths, ignoreCaseComparer);
			for (int i = 0; i < filepaths.Length; i++)
				if (Path.GetExtension(filepaths[i]).Length <= 4)//".erb"や".csv"であること。放置すると".erb*"等を拾う。
					retList.Add(new KeyValuePair<string, string>(RelativePath + Path.GetFileName(filepaths[i]), filepaths[i]));
			return retList;
		}
		

		/// <summary>
		/// IgnoreCaseはprivateに。代わりにICFunctionかICVariableを使う。
		/// </summary>
		private static bool IgnoreCase { get; set; }
		private static bool CompatiFunctionNoignoreCase { get; set; }
		

		/// <summary>
		/// 関数名・属性名的な名前のIgnoreCaseフラグ
		/// 関数・属性・BEGINのキーワード 
		/// どうせeramaker用の互換処理なのでEmuera専用構文については適当に。
		/// </summary>
		public static bool ICFunction { get; private set; }
		
		/// <summary>
		/// 変数名、命令名的な名前のIgnoreCaseフラグ 
		/// 変数・命令・$ラベル名、GOTOの引数 
		/// </summary>
		public static bool ICVariable { get; private set; }

		/// <summary>
		/// 関数名・属性名的な名前の比較フラグ
		/// </summary>
		public static StringComparison SCFunction { get; private set; }
		/// <summary>
		/// 変数名、命令名的な名前の比較フラグ
		/// </summary>
		public static StringComparison SCVariable { get; private set; }
		/// <summary>
		/// ファイル名的な名前の比較フラグ
		/// </summary>
		public const StringComparison SCIgnoreCase = StringComparison.OrdinalIgnoreCase;
		/// <summary>
		/// 式中での文字列比較フラグ
		/// </summary>
		public const StringComparison SCExpression = StringComparison.Ordinal;

		/// <summary>
		/// GDI+利用時に発生する文字列と図形・画像間の位置ずれ補正
		/// </summary>
		public static int DrawingParam_ShapePositionShift { get; private set; }


		public static bool UseRenameFile { get; private set; }
		public static bool UseReplaceFile { get; private set; }
		public static bool UseMouse { get; private set; }
		public static bool UseMenu { get; private set; }
		public static bool UseDebugCommand { get; private set; }
		public static bool AllowMultipleInstances { get; private set; }
		public static bool AutoSave { get; private set; }
		public static bool UseKeyMacro { get; private set; }
		public static bool SizableWindow { get; private set; }
		//public static bool UseImageBuffer { get; private set; }
		public static TextDrawingMode TextDrawingMode { get; private set; }
		public static int WindowX { get; private set; }
		/// <summary>
		/// 実際に描画可能な横幅
		/// </summary>
		public static int DrawableWidth { get; private set; }
		public static int WindowY { get; private set; }
		public static int WindowPosX { get; private set; }
		public static int WindowPosY { get; private set; }
		public static bool SetWindowPos { get; private set; }
		public static int MaxLog { get; private set; }
		public static int PrintCPerLine { get; private set; }
		public static int PrintCLength { get; private set; }
        public static int PrintCShopLength { get; private set;}     //182101 PCDRP-Update:SHOPだけ文字数が違い過ぎるので個別対応
		public static Color ForeColor { get; private set; }
		public static Color BackColor { get; private set; }
		public static Color FocusColor { get; private set; }
        public static Color EdgeColor { get; private set; }     //182101 PCDRP-Update:フォント縁取り色
		public static Color LogColor { get; private set; }
		public static int FontSize { get; private set; }
		public static string FontName { get; private set; }
		public static int LineHeight { get; private set; }
		public static int FPS { get; private set; }
		//public static int SkipFrame { get; private set; }
		public static int ScrollHeight { get; private set; }
		public static int InfiniteLoopAlertTime { get; private set; }
		public static int SaveDataNos { get; private set; }
		public static bool WarnBackCompatibility { get; private set; }
		public static bool WindowMaximixed { get; private set; }
		public static bool WarnNormalFunctionOverloading { get; private set; }
		public static bool SearchSubdirectory { get; private set; }
		public static bool SortWithFilename { get; private set; }

		public static bool AllowFunctionOverloading { get; private set; }
		public static bool WarnFunctionOverloading { get; private set; }

		public static int DisplayWarningLevel { get; private set; }
		public static bool DisplayReport { get; private set; }
		public static ReduceArgumentOnLoadFlag ReduceArgumentOnLoad { get; private set; }
		public static bool IgnoreUncalledFunction { get; private set; }
		public static DisplayWarningFlag FunctionNotFoundWarning { get; private set; }
		public static DisplayWarningFlag FunctionNotCalledWarning { get; private set; }

		public static bool ChangeMasterNameIfDebug { get; private set; }
		public static long LastKey { get; private set; }
		public static bool ButtonWrap { get; private set; }

		public static string TextEditor { get; private set; }
        public static TextEditorType EditorType { get; private set; }
		public static string EditorArg { get; private set; }

		public static bool CompatiErrorLine { get; private set; }
		public static bool CompatiCALLNAME { get; private set; }
		public static bool UseSaveFolder { get; private set; }
		public static bool CompatiRAND { get; private set; }
		//public static bool CompatiDRAWLINE { get; private set; }
		public static bool CompatiLinefeedAs1739 { get; private set; }
		public static bool SystemAllowFullSpace { get; private set; }
		public static bool SystemSaveInUTF8 { get; private set; }
		public static bool SystemSaveInBinary { get; private set; }
		public static bool CompatiFuncArgAutoConvert { get; private set; }
		public static bool CompatiFuncArgOptional { get; private set; }
		public static bool CompatiCallEvent { get; private set; }
		public static bool CompatiSPChara { get; private set; }
		public static bool SystemIgnoreTripleSymbol { get; private set; }
		public static bool SystemNoTarget { get; private set; }
		public static int FixImgGroupNumber { get; private set; }      			//182101 PCDRP-Update
        public static int ScreenEffectInterval { get; private set; }      	    //182101 PCDRP-Update

        public static bool CBUseClipboard { get; private set; }
        public static bool CBIgnoreTags { get; private set; }
        public static string CBReplaceTags { get; private set; }
        public static bool CBNewLinesOnly { get; private set; }
        public static bool CBClearBuffer { get; private set; }
        public static bool CBTriggerLeftClick { get; private set; }
        public static bool CBTriggerMiddleClick { get; private set; }
        public static bool CBTriggerDoubleLeftClick { get; private set; }
        public static bool CBTriggerAnyKeyWait { get; private set; }
        public static bool CBTriggerInputWait { get; private set; }
        public static int  CBMaxCB { get; private set; }
        public static int  CBBufferSize { get; private set; }
        public static int  CBScrollCount { get; private set; }
        public static int  CBMinTimer { get; private set; }


        public static int Language { get; private set; }
		
		public static string SavDir { get; private set; }

		public static bool NeedReduceArgumentOnLoad { get; private set; }

        public static bool AllowLongInputByMouse { get; private set; }

        public static bool TimesNotRigorousCalculation { get; private set; }
        //一文字変数の禁止オプションを考えた名残
        //public static bool ForbidOneCodeVariable { get; private set; }
		#endregion

		#region debug
		public static void SetDebugConfig(ConfigData instance)
		{
			DebugShowWindow = instance.GetConfigValue<bool>(ConfigCode.DebugShowWindow);
			DebugWindowTopMost = instance.GetConfigValue<bool>(ConfigCode.DebugWindowTopMost);
			DebugWindowWidth = instance.GetConfigValue<int>(ConfigCode.DebugWindowWidth);
			DebugWindowHeight = instance.GetConfigValue<int>(ConfigCode.DebugWindowHeight);
			DebugSetWindowPos = instance.GetConfigValue<bool>(ConfigCode.DebugSetWindowPos);
			DebugWindowPosX = instance.GetConfigValue<int>(ConfigCode.DebugWindowPosX);
			DebugWindowPosY = instance.GetConfigValue<int>(ConfigCode.DebugWindowPosY);
		}
		public static bool DebugShowWindow { get; private set; }
		public static bool DebugWindowTopMost { get; private set; }
		public static int DebugWindowWidth { get; private set; }
		public static int DebugWindowHeight { get; private set; }
		public static bool DebugSetWindowPos { get; private set; }
		public static int DebugWindowPosX { get; private set; }
		public static int DebugWindowPosY { get; private set; }


		#endregion

		#region replace
		public static void SetReplace(ConfigData instance)
		{
			MoneyLabel = instance.GetConfigValue<string>(ConfigCode.MoneyLabel);
			MoneyFirst = instance.GetConfigValue<bool>(ConfigCode.MoneyFirst);
			LoadLabel = instance.GetConfigValue<string>(ConfigCode.LoadLabel);
			MaxShopItem = instance.GetConfigValue<int>(ConfigCode.MaxShopItem);
			DrawLineString = instance.GetConfigValue<string>(ConfigCode.DrawLineString);
			if (string.IsNullOrEmpty(DrawLineString))
				DrawLineString = "-";
			BarChar1 = instance.GetConfigValue<char>(ConfigCode.BarChar1);
			BarChar2 = instance.GetConfigValue<char>(ConfigCode.BarChar2);
			TitleMenuString0 = instance.GetConfigValue<string>(ConfigCode.TitleMenuString0);
			TitleMenuString1 = instance.GetConfigValue<string>(ConfigCode.TitleMenuString1);
			ComAbleDefault = instance.GetConfigValue<int>(ConfigCode.ComAbleDefault);
			StainDefault = instance.GetConfigValue<List<Int64>>(ConfigCode.StainDefault);
			TimeupLabel = instance.GetConfigValue<string>(ConfigCode.TimeupLabel);
			ExpLvDef = instance.GetConfigValue<List<Int64>>(ConfigCode.ExpLvDef);
			PalamLvDef = instance.GetConfigValue<List<Int64>>(ConfigCode.PalamLvDef);
			PbandDef = instance.GetConfigValue<Int64>(ConfigCode.pbandDef);
            RelationDef = instance.GetConfigValue<Int64>(ConfigCode.RelationDef);
		}

		public static string MoneyLabel { get; private set; }
		public static bool MoneyFirst { get; private set; }
		public static string LoadLabel { get; private set; }
		public static int MaxShopItem { get; private set; }
		public static string DrawLineString { get; private set; }
		public static char BarChar1 { get; private set; }
		public static char BarChar2 { get; private set; }
		public static string TitleMenuString0 { get; private set; }
		public static string TitleMenuString1 { get; private set; }
		public static int ComAbleDefault { get; private set; }
		public static List<Int64> StainDefault { get; private set; }
		public static string TimeupLabel { get; private set; }
		public static List<Int64> ExpLvDef { get; private set; }
		public static List<Int64> PalamLvDef { get; private set; }
		public static Int64 PbandDef { get; private set; }
        public static Int64 RelationDef { get; private set; }
		#endregion
		
		
		
	}
}