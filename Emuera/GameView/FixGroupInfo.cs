using System;
using System.Drawing.Imaging;

namespace MinorShift.Emuera.GameView
{
    public class FixGroupInfo
    {
        public float fixAlpha;
        public int groupID;
        public bool isReverse;
        public ImageAttributes ia;

        public FixGroupInfo(float fixAlpah, int groupID, bool isReverse) {
            this.fixAlpha = fixAlpah;
            this.groupID = groupID;
            this.isReverse = isReverse;

            // 固定画像表示用のオフスクリーンバッファを適用
            //ColorMatrixオブジェクトの作成
            System.Drawing.Imaging.ColorMatrix cm =
                new System.Drawing.Imaging.ColorMatrix();
            //透過率にリソースファイルで指定した透過率を指定する
            cm.Matrix00 = 1;
            cm.Matrix11 = 1;
            cm.Matrix22 = 1;
            cm.Matrix33 = fixAlpha;
            cm.Matrix44 = 1;

            //ImageAttributesオブジェクトの作成
            this.ia = new System.Drawing.Imaging.ImageAttributes();
            //ColorMatrixを設定する
            this.ia.SetColorMatrix(cm);
        }
    }
}
