using MinorShift._Library;
using MinorShift.Emuera.Content;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
namespace MinorShift.Emuera.GameView
{
	class ConsoleImagePart : AConsoleDisplayPart
	{

		public ConsoleImagePart(string resName, string resNameb, int raw_height, int raw_width, int raw_ypos, int fix_xpos, int fix_ypos, bool fix_reverse,int fix_group, float fix_alpha, string backGroupInfo)//182101 PCDRP-Update:画像固定表示機能で修正
		{
            System.Globalization.CultureInfo customCulture = new System.Globalization.CultureInfo("ja-JP");
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            fImgInfo = new CroppedImage.FixImgInfo();	//画像固定表示機能用情報	//182101 PCDRP-Update
			top = 0;
			bottom = Config.FontSize;
			Str = "";
			ResourceName = resName ?? "";
			ButtonResourceName = resNameb;
			StringBuilder sb = new StringBuilder();
			sb.Append("<img src='");
			sb.Append(ResourceName);
			if(ButtonResourceName != null)
			{
				sb.Append("' srcb='");
				sb.Append(ButtonResourceName);
			}
			if(raw_height != 0)
			{
				sb.Append("' height='");
				sb.Append(raw_height.ToString());
			}
			if(raw_width != 0)
			{
				sb.Append("' width='");
				sb.Append(raw_width.ToString());	//182101 PCDRP-Update:多分バグだったので修正
			}
			if(raw_ypos != 0)
			{
				sb.Append("' ypos='");
				sb.Append(raw_ypos.ToString());		//182101 PCDRP-Update:多分バグだったので修正
			}
			//182101 PCDRP-Update:画像固定表示機能で修正↓--------------------------
            if (fix_xpos >= 0)
            {
                sb.Append("' fxpos='");
                sb.Append(fix_xpos.ToString());
            }
            if (fix_ypos >= 0)
            {
                sb.Append("' fypos='");
                sb.Append(fix_ypos.ToString());
            }
            if (fix_reverse)
            {
                sb.Append("' fix_reverse='");
                sb.Append(fix_reverse.ToString());
            }
            if (fix_group >= 0)
            {
                sb.Append("' fix_group='");
                sb.Append(fix_group.ToString());
            }
            if (fix_alpha >= 0)
            {
                sb.Append("' fix_alpha='");
                sb.Append(fix_alpha.ToString());
            }
            if (backGroupInfo != "")
            {
                sb.Append("' backGroupInfo='");
                sb.Append(backGroupInfo);
            }
            //182101 PCDRP-Update:画像固定表示機能で修正↑--------------------------
            sb.Append("'>");
			AltText = sb.ToString();
			
			cImage = Content.AppContents.GetContent<CroppedImage>(ResourceName);
            //182101 PCDRP-Update:画像ロードタイミングを変更↓--------------------------
            //画像が読み込まれていない場合はロードする
            if (cImage != null && !cImage.Enabled) {
                cImage.BaseImage.Load(Config.TextDrawingMode == TextDrawingMode.WINAPI);
                if (cImage.BaseImage.Enabled)
                {
                    cImage.Rectangle = new Rectangle(new Point(0, 0), cImage.BaseImage.Bitmap.Size);
                    cImage.Enabled = true;
                }
            }
            //182101 PCDRP-Update:画像ロードタイミングを変更↑--------------------------

            if (cImage != null && !cImage.Enabled)
				cImage = null;
			if (cImage == null)
			{
				Str = AltText;
				return;
			}
			//182101 PCDRP-Update:画像固定表示機能で修正↓--------------------------
            if (cImage.fImgInfo.isFixed) {
                fixLineNo = -1; //初期値設定
                //cImageのデータをコピーする(代入してしまうとcImageの内容を上書きしてしまうので)
                fImgInfo.isFixed = true;
                fImgInfo.isRoot = cImage.fImgInfo.isRoot;
                fImgInfo.fixAlpha = cImage.fImgInfo.fixAlpha;
                fImgInfo.fixPosX = cImage.fImgInfo.fixPosX;
                fImgInfo.fixPosY = cImage.fImgInfo.fixPosY;
                fImgInfo.isReverse = cImage.fImgInfo.isReverse;
                fImgInfo.groupNum = cImage.fImgInfo.groupNum;

                //プログラム側で指定されていた場合はリソースファイル側で指定された情報を上書きする
                if (fix_xpos >= 0)
                {
                    fImgInfo.fixPosX = fix_xpos;
                }
                if (fix_ypos >= 0)
                {
                    fImgInfo.fixPosY = fix_ypos;
                }
                if (fix_alpha >= 0)
                {
                    fImgInfo.fixAlpha = fix_alpha;
                }
                //CLEARALL時の指定。どのグループを削除するかどうか
                if (fix_group >= 0)
                {
                    fImgInfo.groupNum = fix_group;
                }
                //この画像がグループの親画像だった場合は現在の行番号と反転指定情報を保持する
                if (cImage.fImgInfo.isRoot) {
                    fixLineNo = GlobalStatic.Console.LineCount;
                    if (fix_reverse) {
                        fImgInfo.isReverse = true;
                    }
                }
                //グループIDの指定がない場合のみ,もしくは本画像が背景画像の場合のみ透過率を指定
                //（グループIDの指定がある場合は固定表示スクリーン用のイメージに透過率を指定するので
                //　個々のリソースで透過率指定はしない）
                if (cImage.fImgInfo.groupNum == -1 || backGroupInfo != "") {
                    //ColorMatrixオブジェクトの作成
                    System.Drawing.Imaging.ColorMatrix cm =
                        new System.Drawing.Imaging.ColorMatrix();
                    //透過率にリソースファイルもしくはプログラムで指定した透過率を指定する
                    cm.Matrix00 = 1;
                    cm.Matrix11 = 1;
                    cm.Matrix22 = 1;
                    cm.Matrix33 = fImgInfo.fixAlpha;
                    cm.Matrix44 = 1;

                    //ImageAttributesオブジェクトの作成
                    ia = new System.Drawing.Imaging.ImageAttributes();
                    //ColorMatrixを設定する
                    ia.SetColorMatrix(cm);
                }
                //グループ画像の背景との統合機能が指定されている場合
                if (backGroupInfo != "") {
                    //この画像は背景画像として扱う
                    isBackGrouped = true;

                    //透過率,グループID,グループIDグループID…の構成
                    string[] backValues = backGroupInfo.Split(',');
                    backGroupAlpha = float.Parse(backValues[0]);
                    backGroupList = new List<int>();
                    for (int i = 1; i < backValues.Length; i++) {
                        backGroupList.Add(int.Parse(backValues[i]));
                    }
                    //HTML_PRINTで指定された背景グループ用の透過率を指定する
                    fImgInfo.fixAlpha = backGroupAlpha;
                }
            }
			//182101 PCDRP-Update:画像固定表示機能で修正↑--------------------------
			int height = 0;
			if (cImage.NoResize)
			{
				height = cImage.Rectangle.Height;
				Width = cImage.Rectangle.Width;
			}
			else
			{
				
				if (raw_height == 0)
					height = Config.FontSize;
				else
					height = Config.FontSize * raw_height / 100;
				if (raw_width == 0)
				{
					Width = cImage.Rectangle.Width * height / cImage.Rectangle.Height;
					XsubPixel = ((float)cImage.Rectangle.Width * height) / cImage.Rectangle.Height - Width;
				}
				else
				{
					Width = Config.FontSize * raw_width / 100;
					XsubPixel = ((float)Config.FontSize * raw_width / 100f) - Width;
				}
			}
			top = raw_ypos * Config.FontSize / 100;
			//182101 PCDRP-Update:画像固定表示機能で修正↓--------------------------
            if (cImage.fImgInfo.isFixed)
            {
                top = 0;
                bottom = 0;
            }
			//182101 PCDRP-Update:画像固定表示機能で修正↑--------------------------
			destRect = new Rectangle(0, top, Width, height);
			if (destRect.Width < 0)
			{
				destRect.X = -destRect.Width;
				Width = -destRect.Width;
			}
			if (destRect.Height < 0)
			{
				destRect.Y = destRect.Y - destRect.Height;
				height = -destRect.Height;
			}
			bottom = top + height;
			//if(top > 0)
			//	top = 0;
			//if(bottom < Config.FontSize)
			//	bottom = Config.FontSize;
			if (ButtonResourceName != null)
			{
				cImageB = Content.AppContents.GetContent<CroppedImage>(ButtonResourceName);
				if (cImageB != null && !cImageB.Enabled)
					cImageB = null;
			}
		}
        public readonly CroppedImage.FixImgInfo fImgInfo;			//182101 PCDRP-Update:画像固定表示機能で修正
        public long fixLineNo;					        			//182101 PCDRP-Update:画像固定表示機能で修正
        public bool isBackGrouped;                                  //182101 PCDRP-Update:画像固定表示機能で修正
        public float backGroupAlpha;                                //182101 PCDRP-Update:画像固定表示機能で修正
        public List<int> backGroupList;                             //182101 PCDRP-Update:画像固定表示機能で修正


        private readonly CroppedImage cImage;
		private readonly CroppedImage cImageB;
		private readonly int top;
		private readonly int bottom;
		private readonly Rectangle destRect;
		private readonly ImageAttributes ia;
		public readonly string ResourceName;
		public readonly string ButtonResourceName;
		public override int Top { get { return top; } }
		public override int Bottom { get { return bottom; } }
		
		public override bool CanDivide { get { return false; } }
		public override void SetWidth(StringMeasure sm, float subPixel)
		{
			if (this.Error)
			{
				Width = 0;
				return;
			}
			if (cImage != null)
				return;
			Width = sm.GetDisplayLength(Str, Config.Font);
			XsubPixel = subPixel;
		}

		public override string ToString()
		{
			if (AltText == null)
				return "";
			return AltText;
		}

		public override void DrawTo(Graphics graph, int pointY, bool isSelecting, bool isBackLog, TextDrawingMode mode)
		{
			if (this.Error)
				return;
			CroppedImage img = cImage;
			if (isSelecting && cImageB != null)
				img = cImageB;
			Rectangle rect = destRect;

            //182101 PCDRP-Update:画像固定表示機能で修正↓--------------------------
            //固定表示の場合は表示位置をリソースファイル、もしくはHTML_PRINで指定した座標に固定する
            if (fImgInfo.isFixed)
            {
                rect.X = fImgInfo.fixPosX;
                rect.Y = fImgInfo.fixPosY;
            }
            else {
                //PointX微調整
                rect.X = destRect.X + PointX + Config.DrawingParam_ShapePositionShift;
                rect.Y = destRect.Y + pointY;
            }
	        //182101 PCDRP-Update:画像固定表示機能で修正↑--------------------------


			if (img != null)
			{
				if(ia == null)
					graph.DrawImage(img.BaseImage.Bitmap, rect, img.Rectangle, GraphicsUnit.Pixel);
				else
					graph.DrawImage(img.BaseImage.Bitmap, rect, img.Rectangle.X,img.Rectangle.Y,img.Rectangle.Width,img.Rectangle.Height , GraphicsUnit.Pixel,ia);
			}
			else
			{
                if (PrintEdgeFont.edgeEnabled)                                                                     //182101 PCDRP-Update:フォント縁取り機能で修正
                    PrintEdgeFont.DrawString(graph, Str, Config.ForeColor, PointX, pointY);                        //182101 PCDRP-Update:フォント縁取り機能で修正
                else 
                {
                    if (mode == TextDrawingMode.GRAPHICS)
                        graph.DrawString(Str, Config.Font, new SolidBrush(Config.ForeColor), new Point(PointX, pointY));
                    else
                        System.Windows.Forms.TextRenderer.DrawText(graph, Str, Config.Font, new Point(PointX, pointY), Config.ForeColor, System.Windows.Forms.TextFormatFlags.NoPrefix);
                }
            }
		}

		public override void GDIDrawTo(int pointY, bool isSelecting, bool isBackLog)
		{
			if (this.Error)
				return;
			CroppedImage img = cImage;
			if (isSelecting && cImageB != null)
				img = cImageB;
			if (img != null)
				GDI.DrawImage(PointX + destRect.X, pointY+ destRect.Y, Width, destRect.Height, img.BaseImage.GDIhDC, img.Rectangle);
			else
				GDI.TabbedTextOutFull(Config.Font, Config.ForeColor, Str, PointX, pointY);
		}
	}
}
