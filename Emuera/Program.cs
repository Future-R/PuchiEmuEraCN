using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using MinorShift._Library;
using MinorShift.Emuera.GameView;
using MinorShift.Emuera.GameData.Expression;
using System.IO;

namespace MinorShift.Emuera
{
	static class Program
	{
		/*
		コードの開始地点。
		ここでMainWindowを作り、
		MainWindowがProcessを作り、
		ProcessがGameBase・ConstantData・Variableを作る。
		
		
		*.ERBの読み込み、実行、その他の処理をProcessが、
		入出力をMainWindowが、
		定数の保存をConstantDataが、
		変数の管理をVariableが行う。
		 
		と言う予定だったが改変するうちに境界が曖昧になってしまった。
		 
		後にEmueraConsoleを追加し、それに入出力を担当させることに。
        
        1750 DebugConsole追加
         Debugを全て切り離すことはできないので一部EmueraConsoleにも担当させる
		
		TODO: 1819 MainWindow & Consoleの入力・表示組とProcess&Dataのデータ処理組だけでも分離したい

		*/
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{

			ExeDir = Sys.ExeDir;
#if DEBUG
			//debugMode = true;
#endif
			CsvDir = ExeDir + "csv\\";
			ErbDir = ExeDir + "erb\\";
			DebugDir = ExeDir + "debug\\";
			DatDir = ExeDir + "dat\\";
			ContentDir = ExeDir + "resources\\";
			FontDir = ExeDir + "font\\";
			//エラー出力用
			//1815 .exeが東方板のNGワードに引っかかるそうなので除去
			ExeName = Path.GetFileNameWithoutExtension(Sys.ExeName);

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			ConfigData.Instance.LoadConfig();
            //二重起動の禁止かつ二重起動
			if ((!Config.AllowMultipleInstances) && (Sys.PrevInstance()))
			{
				MessageBox.Show("如果要允许游戏多开，请修改 emuera.config 文件", "游戏已经启动");
				return;
			}
			if (!Directory.Exists(CsvDir))
			{
				MessageBox.Show("找不到 csv 文件夹", "找不到目录");
				return;
			}
			if (!Directory.Exists(ErbDir))
			{
				MessageBox.Show("找不到 erb 文件夹", "找不到目录");
				return;
			}
            int argsStart = 0;
            if ((args.Length > 0)&&(args[0].Equals("-DEBUG", StringComparison.CurrentCultureIgnoreCase)))
            {
                argsStart = 1;//デバッグモードかつ解析モード時に最初の1っこ(-DEBUG)を飛ばす
				debugMode = true;
            }
			// 动态读取字体相关
			if (Directory.Exists(FontDir))
			{
				foreach (string fontFile in Directory.GetFiles(FontDir, "*.ttf", SearchOption.AllDirectories))
					GlobalStatic.Pfc.AddFontFile(fontFile);

				foreach (string fontFile in Directory.GetFiles(FontDir, "*.otf", SearchOption.AllDirectories))
					GlobalStatic.Pfc.AddFontFile(fontFile);
			}
			if (debugMode)
			{
				ConfigData.Instance.LoadDebugConfig();
				if (!Directory.Exists(DebugDir))
				{
					try
					{
						Directory.CreateDirectory(DebugDir);
					}
					catch
					{
						MessageBox.Show("创建 debug 文件夹失败", "找不到目录");
						return;
					}
				}
			}
            if (args.Length > argsStart)
            {
                AnalysisFiles = new List<string>();
                for (int i = argsStart; i < args.Length; i++)
                {
                    if (!File.Exists(args[i]) && !Directory.Exists(args[i]))
                    {
                        MessageBox.Show("与えられたファイル・フォルダは存在しません");
                        return;
                    }
                    if ((File.GetAttributes(args[i]) & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        List<KeyValuePair<string, string>> fnames = Config.GetFiles(args[i] + "\\", "*.ERB");
                        for (int j = 0; j < fnames.Count; j++)
                        {
                            AnalysisFiles.Add(fnames[j].Value);
                        }
                    }
                    else
                    {
                        if (Path.GetExtension(args[i]).ToUpper() != ".ERB")
                        {
                            MessageBox.Show("ドロップ可能なファイルはERBファイルのみです");
                            return;
                        }
                        AnalysisFiles.Add(args[i]);
                    }
                }
                AnalysisMode = true;
            }
			MainWindow win = null;
			while (true)
			{
				StartTime = WinmmTimer.TickCount;
				using (win = new MainWindow())
				{
					Application.Run(win);
					Content.AppContents.UnloadContents();
					if (!Reboot)
						break;

					RebootWinState = win.WindowState;
					if (win.WindowState == FormWindowState.Normal)
					{
						RebootClientY = win.ClientSize.Height;
						RebootLocation = win.Location;
					}
					else
					{
						RebootClientY = 0;
						RebootLocation = new Point();
					}
				}
				//条件次第ではParserMediatorが空でない状態で再起動になる場合がある
				ParserMediator.ClearWarningList();
				ParserMediator.Initialize(null);
				GlobalStatic.Reset();
				//GC.Collect();
				Reboot = false;
				ConfigData.Instance.ReLoadConfig();
			}
		}

		/// <summary>
		/// 実行ファイルのディレクトリ。最後に\を付けたstring
		/// </summary>
		public static string ExeDir { get; private set; }
		public static string CsvDir { get; private set; }
		public static string ErbDir { get; private set; }
		public static string DebugDir { get; private set; }
		public static string DatDir { get; private set; }
		public static string ContentDir { get; private set; }
		public static string ExeName { get; private set; }

		public static string FontDir { get; private set; }

		public static bool Reboot = false;
		//public static int RebootClientX = 0;
		public static int RebootClientY = 0;
        public static FormWindowState RebootWinState = FormWindowState.Normal;
		public static Point RebootLocation;

        public static bool AnalysisMode = false;
        public static List<string> AnalysisFiles = null;

		public static bool debugMode = false;
		public static bool DebugMode { get { return debugMode; } }


		public static uint StartTime { get; private set; }

	}
}