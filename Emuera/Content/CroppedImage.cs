using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MinorShift.Emuera.Content
{
	internal sealed class CroppedImage : AContentItem
	{
		public CroppedImage(string name, BaseImage image, Rectangle rect, bool noresize) : base(name)
		{
			BaseImage = image;
			Rectangle = rect;
			if (image != null)
				this.Enabled = image.Enabled;
			if (rect.Width <= 0 || rect.Height <= 0)
				this.Enabled = false;
			this.NoResize = noresize;
		}
        //182101 PCDRP-Update:画像固定表示機能で修正↓--------------------------
        public FixImgInfo fImgInfo;					//182101 PCDRP-Update:画像固定表示機能で修正
        public class FixImgInfo {
            public bool isFixed;   //固定表示である事を示すフラグ
            public int fixPosX;    //固定表示座標(X)
            public int fixPosY;    //固定表示座標(Y)
            public float fixAlpha;      //透過率(固定表示時のみ使用)
            public bool isRoot;        //グルーピング設定の親かどうか
            public bool isReverse;     //画像の左右を反転するかどうか
            public int groupNum;       //グルーピングID(0以上で有効)
            public FixImgInfo()
		    {
                isFixed = false;
                fixPosX = 0;
                fixPosY = 0;
                fixAlpha = 1.0f;
                isRoot = false;
                groupNum = -1;
            }
        }

        public CroppedImage(string name, BaseImage image, Rectangle rect, bool noresize, FixImgInfo fimginfo) : base(name)
        {
            BaseImage = image;
            Rectangle = rect;
            if (image != null)
                this.Enabled = image.Enabled;
            if (rect.Width <= 0 || rect.Height <= 0)
                this.Enabled = false;
            this.NoResize = noresize;
            this.fImgInfo = fimginfo;
        }
        //182101 PCDRP-Update:画像ロードタイミングを変更↓--------------------------
        //対象画像が必要になったタイミングでロードするようにしたため読み取り専用を解除
        //(お行儀悪いけど自分専用なので…)
        public BaseImage BaseImage;
        public Rectangle Rectangle;
        //public readonly BaseImage BaseImage;
        //public readonly Rectangle Rectangle;
        //182101 PCDRP-Update:画像ロードタイミングを変更↑--------------------------
        public readonly bool NoResize;
	}
}
