using MinorShift.Emuera.Sub;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;

namespace MinorShift.Emuera.Content
{
	static class AppContents
	{

		static public T GetContent<T>(string name)where T :AContentItem
		{
			if (name == null)
				return null;
			name = name.ToUpper();
			if (!itemDic.ContainsKey(name))
				return null;
			return itemDic[name] as T;
		}

		static public void LoadContents()
		{
			if (!Directory.Exists(Program.ContentDir))
				return;
			try
			{
				List<string> bmpfilelist = new List<string>();
				bmpfilelist.AddRange(Directory.GetFiles(Program.ContentDir, "*.png", SearchOption.AllDirectories));		//182101 PCDRP-Update pngファイル大量にあるのでフォルダ分けできるようにした
				bmpfilelist.AddRange(Directory.GetFiles(Program.ContentDir, "*.bmp", SearchOption.TopDirectoryOnly));
				bmpfilelist.AddRange(Directory.GetFiles(Program.ContentDir, "*.jpg", SearchOption.TopDirectoryOnly));
				bmpfilelist.AddRange(Directory.GetFiles(Program.ContentDir, "*.gif", SearchOption.TopDirectoryOnly));
				foreach (var filename in bmpfilelist)
				{//リスト化のみ。Loadはまだ
					string name = Path.GetFileName(filename).ToUpper();
					resourceDic.Add(name, new BaseImage(name, filename));
				}
				string[] csvFiles = Directory.GetFiles(Program.ContentDir, "*.csv", SearchOption.TopDirectoryOnly);
				foreach (var filename in csvFiles)
				{
					string[] lines = File.ReadAllLines(filename, Config.Encode);
					foreach (var line in lines)
					{
						if (line.Length == 0)
							continue;
						string str = line.Trim();
						if (str.Length == 0 || str.StartsWith(";"))
							continue;
						string[] tokens = str.Split(',');
						AContentItem item = CreateFromCsv(tokens);
						if (item != null && !itemDic.ContainsKey(item.Name))
							itemDic.Add(item.Name, item);
					}
				}
			}
			catch
			{
				throw new CodeEE("An error occurred while loading the resource file");
			}
		}

		static public void UnloadContents()
		{
			foreach (var img in resourceDic.Values)
				img.Dispose();
			resourceDic.Clear();
			itemDic.Clear();
		}

		static private AContentItem CreateFromCsv(string[] tokens)
		{

			if(tokens.Length < 2)
				return null;
			string name = tokens[0].Trim().ToUpper();
			string parentName = tokens[1].ToUpper();
			if (name.Length == 0 || parentName.Length == 0)
				return null;
			if (!resourceDic.ContainsKey(parentName))
				return null;
			AContentFile parent = resourceDic[parentName];
			if(parent is BaseImage)
			{
				BaseImage parentImage = parent as BaseImage;

                //182101 PCDRP-Update:画像ロードタイミングを変更↓--------------------------
                //画像の量が多い場合に一度にロードするとかなりの時間を要するため
                //必要になったタイミングで読み込まれていなかったらロードするように変更
                //parentImage.Load(Config.TextDrawingMode == TextDrawingMode.WINAPI);
                //if (!parentImage.Enabled)
                //		return null;
                //Rectangle rect = new Rectangle(new Point(0, 0), parentImage.Bitmap.Size);
                Rectangle rect = new Rectangle(0, 0, 0, 0);
                //182101 PCDRP-Update:画像ロードタイミングを変更↑--------------------------
                bool noresize = false;
                CroppedImage.FixImgInfo fImgInfo = new CroppedImage.FixImgInfo();	//182101 PCDRP-Update 固定表示用のクラスを追加
				if(tokens.Length >= 6)
				{
					int[] rectValue = new int[4];
					bool sccs = true;
					for (int i = 0; i < 4; i++)
						sccs &= int.TryParse(tokens[i + 2], out rectValue[i]);
					if (sccs)
						rect = new Rectangle(rectValue[0], rectValue[1], rectValue[2], rectValue[3]);
					if(tokens.Length >= 7)
					{
						string[] keywordTokens = tokens[6].Split('|');
						foreach(string keyword in keywordTokens)
						{
							switch(keyword.Trim().ToUpper())
							{
                                case "NORESIZE":
                                    throw new NotImplCodeEE();
                                    //noresize = true;
                                    //break;
                                //182101 PCDRP-Update:画像固定表示機能で修正↓--------------------------
                                case "FIXED":
                                    string[] fixTokens = tokens[7].Split('|');
                                    //グルーピングを使わない時は3つ、使う時は5つ指定されている
                                    if (fixTokens.Length == 3 || fixTokens.Length == 5) {
                                        NumberStyles style = NumberStyles.Float | NumberStyles.AllowDecimalPoint;
                                        var culture = new CultureInfo("zh-CN");

                                        sccs &= int.TryParse(fixTokens[0], out fImgInfo.fixPosX);
                                        sccs &= int.TryParse(fixTokens[1], out fImgInfo.fixPosY);
                                        sccs &= float.TryParse(fixTokens[2], style, culture, out fImgInfo.fixAlpha);
                                        if (fixTokens.Length == 5) {
                                            if (fixTokens[3].Equals("ROOT", StringComparison.OrdinalIgnoreCase))
                                            {
                                                fImgInfo.isRoot = true;
                                            }
                                            else
                                            {
                                                fImgInfo.isRoot = false;
                                            }
                                            sccs &= int.TryParse(fixTokens[4], out fImgInfo.groupNum);
                                        }
                                    }
                                    //全部成功した場合は固定表示を有効にする
                                    if (sccs) {
                                        noresize = true;
                                        fImgInfo.isFixed = true;
                                    }
                                    break;
                                //182101 PCDRP-Update:画像固定表示機能で修正↑--------------------------
                            }
                        }
					}
				}
                CroppedImage image = new CroppedImage(name, parentImage, rect, noresize, fImgInfo);	//182101 PCDRP-Update:画像固定表示機能で修正
				return image;
			}
			return null;
		}


		static Dictionary<string, AContentFile> resourceDic = new Dictionary<string, AContentFile>();
		static Dictionary<string, AContentItem> itemDic = new Dictionary<string, AContentItem>();

	}
}
