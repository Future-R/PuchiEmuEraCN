//182101 PCDRP-Update:画像固定表示機能で新規作成
//固定表示画像を表示するためのクラス
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using MinorShift.Emuera.Content;

namespace MinorShift.Emuera.GameView
{
    public class FixImgPrint
    {
        //画像固定表示機能のグループレイヤー
        Bitmap[] fixBitmapImg;
        Graphics[] fixGraph;
        public float[] fixFadeAlpha;
        public bool isGroupEnabled;
        public int[] shakeDx;
        public bool[] effectTarget;
        public Bitmap allFadeBackBitmapImg; //全体フェード時のビットマップ保存用
        public Graphics allFadeBackGraph;   //全体フェード時の画像処理用
        internal EffectStatus EffectStatus = 0;       //全体フェード時の状態  
        internal EffectTarget effectTargetType = EffectTarget.NotEffect;		//エフェクトの対象(全体かグループか)
        //public float[] effectDifference;//複数画像を一度にフェードする時に使おうかと思ったが未使用
        public float[] shakeDAlpha;
        private readonly List<ConsoleImagePart> displayFixImgLineList;

        /* *************************************************************
        * コンストラクタ
        **************************************************************** */
        public FixImgPrint(int fixImgGroupNumber, int width, int height)
        {
            //指定がない、もしくはグループレイヤー数が0の場合は未使用
            if (fixImgGroupNumber == 0)
            {
                isGroupEnabled = false;
            }
            else
            {
                allFadeBackBitmapImg = new Bitmap(width, height);
                allFadeBackGraph = Graphics.FromImage(allFadeBackBitmapImg);
                fixBitmapImg = new Bitmap[Config.FixImgGroupNumber];
                fixGraph = new Graphics[Config.FixImgGroupNumber];
                fixFadeAlpha = new float[Config.FixImgGroupNumber];
                shakeDx = new int[Config.FixImgGroupNumber];
                shakeDAlpha = new float[Config.FixImgGroupNumber];
                effectTarget= new bool[Config.FixImgGroupNumber];
                //effectDifference = new float[Config.FixImgGroupNumber];
                //固定表示用のビットマップを生成しておく
                for (int i = 0; i < Config.FixImgGroupNumber; i++)
                {
                    fixBitmapImg[i] = new Bitmap(width, height);
                    fixGraph[i] = Graphics.FromImage(fixBitmapImg[i]);
                    fixFadeAlpha[i] = 1.0f;
                    shakeDx[i] = 0;
                    shakeDAlpha[i] = 0f;
                    effectTarget[i] = false;
                    //effectDifference[i] = 1.0f;
                }
                isGroupEnabled = true;
            }
            displayFixImgLineList = new List<ConsoleImagePart>();
        }
        /* *************************************************************
        * 画像表示
        **************************************************************** */
        public void drawFixImage(int topLineNo, int bottomLineNo, Graphics graph, int pointY, bool isBackLog, bool force)
        {
            //全体フェード中ですでに一度描写実行時の場合はバッファした画像で上書きして終了
            if (effectTargetType == EffectTarget.Screen)
            {
                if (EffectStatus == EffectStatus.EffectBuffered)
                {
                    graph.DrawImage(allFadeBackBitmapImg, new Rectangle(0, 0, allFadeBackBitmapImg.Width, allFadeBackBitmapImg.Height),
                        0, 0, allFadeBackBitmapImg.Width, allFadeBackBitmapImg.Height, GraphicsUnit.Pixel, null);
                    return;
                }
                else if (EffectStatus == EffectStatus.EffectStart) {
                    //バッファ処理の初回の場合はフェードバッファ用のオブジェクトをクリアしておく
                    allFadeBackGraph.Clear(Color.Transparent);
                }
            }

            //コンフィグでグループ数が指定してある場合グループ用レイヤー領域をクリアする
            if (isGroupEnabled) {
                for (int i = 0; i < Config.FixImgGroupNumber; i++)
                {
                    //エフェクト中(バッファ済み)の場合はビットマップを使い回せるのでクリアしない
                    //(※背景グループは使い回せないが背景発見時にクリアする)
                    if (EffectStatus != EffectStatus.EffectBuffered)
                    {
                        fixGraph[i].Clear(Color.Transparent);
                    }
                }
            }

            //リストに画像が存在しない、バックログ中場合は終了
            if (displayFixImgLineList.Count == 0 || isBackLog) {
                return;
            }

            //固定表示画像グループ用情報インスタンスの生成
            List<FixGroupInfo> fgInfo = new List<FixGroupInfo>();
            int backGroupNum = -1;

            List<int> backGroupIdInfo =new List<int>();
            //当初は固定表示画像以外のスクロール状態も確認して表示していたが、
            //それだと結局文章が積もった場合に固定表示画像が範囲から漏れて表示されなくなるので
            //その処理は抜いた
            for (int i = 0; i < displayFixImgLineList.Count; i++)
            {
                //グループIDが指定されていないか、既定の枚数をオーバーした指定の場合は無視して
                //通常の画面に書き込む
                if (displayFixImgLineList[i].fImgInfo.groupNum == -1 ||
                    displayFixImgLineList[i].fImgInfo.groupNum >= Config.FixImgGroupNumber)
                {
                    displayFixImgLineList[i].DrawTo(graph, pointY, isBackLog, true, Config.TextDrawingMode);
                }
                else
                {
                    //グループ機能を利用しており、該当の行が親となる画像の場合は透過率と反転設定を保存しておく
                    if (displayFixImgLineList[i].fImgInfo.isRoot)
                    {
                        if (fgInfo.Count < Config.FixImgGroupNumber)
                        {
                            fgInfo.Add(new FixGroupInfo(displayFixImgLineList[i].fImgInfo.fixAlpha * fixFadeAlpha[displayFixImgLineList[i].fImgInfo.groupNum] + shakeDAlpha[displayFixImgLineList[i].fImgInfo.groupNum],
                                displayFixImgLineList[i].fImgInfo.groupNum,
                                displayFixImgLineList[i].fImgInfo.isReverse));
                        }
                        //対象の行がグループ画像の背景統合指定をされている場合は統合用の情報を取得しておく
                        if (displayFixImgLineList[i].isBackGrouped) {
                            //この画像は背景画像。背景のGraphicsオブジェクトは必ず末尾を使用する
                            backGroupNum = fgInfo.Count - 1;
                            
                            //エフェクト中によりバッファリングしている場合は背景領域を削除して再書き込み
                            if (EffectStatus == EffectStatus.EffectBuffered) {
                                fixGraph[fgInfo[backGroupNum].groupID].Clear(Color.Transparent);
                                displayFixImgLineList[i].DrawTo(fixGraph[fgInfo[backGroupNum].groupID], pointY, isBackLog, true, Config.TextDrawingMode);
                            }
                            backGroupIdInfo = displayFixImgLineList[i].backGroupList;
                        }
                    }
                    //エフェクト中(バッファ済み)で、かつ背景以外のレイヤーは使い回せるので処理しない
                    if (EffectStatus != EffectStatus.EffectBuffered)
                    {
                        //リソース名で指定されたグループに書き込む
                        displayFixImgLineList[i].DrawTo(fixGraph[displayFixImgLineList[i].fImgInfo.groupNum], pointY, isBackLog, true, Config.TextDrawingMode);
                    }
                }
                //固定画像の場合行という概念がないのでpointYの値を加算しない
            }

            //対象の行がグループ画像の背景統合指定をされている場合
            if (backGroupNum != -1)
            {
                for (int i = 0; i < fgInfo.Count; i++)
                {
                    //背景画像ではなく、かつ背景統合に指定されたグループIDの場合
                    if (i != backGroupNum && backGroupIdInfo.Contains(fgInfo[i].groupID)) {
                        //グループ画像側に指定された透過率は無視して不透明で出力
                        //親画像に反転指定があった場合は反転して表示する
                        if (fgInfo[i].isReverse)
                        {
                            fixGraph[fgInfo[backGroupNum].groupID].DrawImage(fixBitmapImg[fgInfo[i].groupID], new Rectangle(fixBitmapImg[fgInfo[i].groupID].Width + shakeDx[fgInfo[i].groupID], 0, -fixBitmapImg[fgInfo[i].groupID].Width + shakeDx[fgInfo[i].groupID], fixBitmapImg[fgInfo[i].groupID].Height),
                                0, 0, fixBitmapImg[fgInfo[i].groupID].Width, fixBitmapImg[fgInfo[i].groupID].Height, GraphicsUnit.Pixel, fgInfo[i].ia);
                        }
                        else
                        {
                            fixGraph[fgInfo[backGroupNum].groupID].DrawImage(fixBitmapImg[fgInfo[i].groupID], new Rectangle(shakeDx[fgInfo[i].groupID], 0, fixBitmapImg[fgInfo[i].groupID].Width, fixBitmapImg[fgInfo[i].groupID].Height),
                                0, 0, fixBitmapImg[fgInfo[i].groupID].Width, fixBitmapImg[fgInfo[i].groupID].Height, GraphicsUnit.Pixel, fgInfo[i].ia);
                        }
                    }
                }
                //最後に背景画像込みの情報をメイン画面に出力する
                //親画像に反転指定があった場合は反転して表示する
                if (fgInfo[backGroupNum].isReverse)
                {
                    graph.DrawImage(fixBitmapImg[fgInfo[backGroupNum].groupID], new Rectangle(fixBitmapImg[fgInfo[backGroupNum].groupID].Width + shakeDx[fgInfo[backGroupNum].groupID], 0, -fixBitmapImg[fgInfo[backGroupNum].groupID].Width + shakeDx[fgInfo[backGroupNum].groupID], fixBitmapImg[fgInfo[backGroupNum].groupID].Height),
                        0, 0, fixBitmapImg[fgInfo[backGroupNum].groupID].Width, fixBitmapImg[fgInfo[backGroupNum].groupID].Height, GraphicsUnit.Pixel, fgInfo[backGroupNum].ia);
                    if (effectTargetType == EffectTarget.Screen && EffectStatus == EffectStatus.EffectStart) {
                        allFadeBackGraph.DrawImage(fixBitmapImg[fgInfo[backGroupNum].groupID], new Rectangle(fixBitmapImg[fgInfo[backGroupNum].groupID].Width + shakeDx[fgInfo[backGroupNum].groupID], 0, -fixBitmapImg[fgInfo[backGroupNum].groupID].Width + shakeDx[fgInfo[backGroupNum].groupID], fixBitmapImg[fgInfo[backGroupNum].groupID].Height),
                            0, 0, fixBitmapImg[fgInfo[backGroupNum].groupID].Width, fixBitmapImg[fgInfo[backGroupNum].groupID].Height, GraphicsUnit.Pixel, fgInfo[backGroupNum].ia);
                    }
                }
                else
                {
                    graph.DrawImage(fixBitmapImg[fgInfo[backGroupNum].groupID], new Rectangle(shakeDx[fgInfo[backGroupNum].groupID], 0, fixBitmapImg[fgInfo[backGroupNum].groupID].Width, fixBitmapImg[fgInfo[backGroupNum].groupID].Height),
                        0, 0, fixBitmapImg[fgInfo[backGroupNum].groupID].Width, fixBitmapImg[fgInfo[backGroupNum].groupID].Height, GraphicsUnit.Pixel, fgInfo[backGroupNum].ia);

                    if (effectTargetType == EffectTarget.Screen && EffectStatus == EffectStatus.EffectStart)
                    {
                        allFadeBackGraph.DrawImage(fixBitmapImg[fgInfo[backGroupNum].groupID], new Rectangle(shakeDx[fgInfo[backGroupNum].groupID], 0, fixBitmapImg[fgInfo[backGroupNum].groupID].Width, fixBitmapImg[fgInfo[backGroupNum].groupID].Height),
                        0, 0, fixBitmapImg[fgInfo[backGroupNum].groupID].Width, fixBitmapImg[fgInfo[backGroupNum].groupID].Height, GraphicsUnit.Pixel, fgInfo[backGroupNum].ia);
                    }
                }
            }

            //行指定の書き込みが終わった段階で、固定表示機能のイメージを標準の領域に書き込む
            for (int i = 0; i < fgInfo.Count; i++)
            {
                //グループ画像の背景統合指定機能の対象外のグループの場合
                if (!backGroupIdInfo.Contains(fgInfo[i].groupID)) {
                    //親画像に反転指定があった場合は反転して表示する
                    if (fgInfo[i].isReverse)
                    {
                        graph.DrawImage(fixBitmapImg[fgInfo[i].groupID], new Rectangle(fixBitmapImg[fgInfo[i].groupID].Width + shakeDx[fgInfo[i].groupID], 0, -fixBitmapImg[fgInfo[i].groupID].Width + shakeDx[fgInfo[i].groupID], fixBitmapImg[fgInfo[i].groupID].Height),
                            0, 0, fixBitmapImg[fgInfo[i].groupID].Width, fixBitmapImg[fgInfo[i].groupID].Height, GraphicsUnit.Pixel, fgInfo[i].ia);

                        if (effectTargetType == EffectTarget.Screen && EffectStatus == EffectStatus.EffectStart) {
                            allFadeBackGraph.DrawImage(fixBitmapImg[fgInfo[i].groupID], new Rectangle(fixBitmapImg[fgInfo[i].groupID].Width + shakeDx[fgInfo[i].groupID], 0, -fixBitmapImg[fgInfo[i].groupID].Width + shakeDx[fgInfo[i].groupID], fixBitmapImg[fgInfo[i].groupID].Height),
                            0, 0, fixBitmapImg[fgInfo[i].groupID].Width, fixBitmapImg[fgInfo[i].groupID].Height, GraphicsUnit.Pixel, fgInfo[i].ia);
                        }
                    }
                    else
                    {
                        graph.DrawImage(fixBitmapImg[fgInfo[i].groupID], new Rectangle(shakeDx[fgInfo[i].groupID], 0, fixBitmapImg[fgInfo[i].groupID].Width, fixBitmapImg[fgInfo[i].groupID].Height),
                            0, 0, fixBitmapImg[fgInfo[i].groupID].Width, fixBitmapImg[fgInfo[i].groupID].Height, GraphicsUnit.Pixel, fgInfo[i].ia);

                        if (effectTargetType == EffectTarget.Screen && EffectStatus == EffectStatus.EffectStart)
                        {
                            allFadeBackGraph.DrawImage(fixBitmapImg[fgInfo[i].groupID], new Rectangle(shakeDx[fgInfo[i].groupID], 0, fixBitmapImg[fgInfo[i].groupID].Width, fixBitmapImg[fgInfo[i].groupID].Height),
                            0, 0, fixBitmapImg[fgInfo[i].groupID].Width, fixBitmapImg[fgInfo[i].groupID].Height, GraphicsUnit.Pixel, fgInfo[i].ia);
                        }
                    }
                }
            }
        }
        /* *************************************************************
        * 画像消去
        **************************************************************** */
        public bool clearFixImage(string resourceName, int groupNum)
        {
            bool bRet = false;

            //リソース名がCLEARALL指定だった場合は全消去
            //TODO:バックログ対応が気になるが…
            if (resourceName.Equals("CLEARALL", StringComparison.OrdinalIgnoreCase))
            {
                //グループ指定がない場合は全消去
                if (groupNum == -1)
                {
                    displayFixImgLineList.Clear();
                    bRet = true;
                    return bRet;

                }
                //グループ指定がある場合は該当のグループのみ削除する
                else
                {
                    for (int i = displayFixImgLineList.Count - 1; i >= 0; i--)
                    {
                        //指定されたリソース名と同じ名前がリストに存在する場合は削除指示と判断してリストから消去
                        if (displayFixImgLineList[i].fImgInfo.groupNum == groupNum)
                        {
                            displayFixImgLineList.RemoveAt(i);
                            bRet = true;
                        }
                    }
                }
                return bRet;
            }
            for (int i = displayFixImgLineList.Count - 1; i >= 0; i--)
            {
                //指定されたリソース名と同じ名前がリストに存在する場合は削除指示と判断してリストから消去
                if (displayFixImgLineList[i].ResourceName.Equals(resourceName, StringComparison.OrdinalIgnoreCase))
                {
                    displayFixImgLineList.RemoveAt(i);
                    bRet = true;
                }
            }
            return bRet;
        }

        /* *************************************************************
        * 画像消去(CLEARLINE)
        **************************************************************** */
        public void clearLineFixImage(int argNum)
        {
            for (int i = displayFixImgLineList.Count - 1; i >= 0; i--)
            {
                //現在末尾の行番号より大きい行番号を持つ画像はCLEARLINEにより消去された行に
                //該当すると判断して要素を削除
                if (displayFixImgLineList[i].fixLineNo > argNum)
                {
                    displayFixImgLineList.RemoveAt(i);
                }
            }
        }
        /* *************************************************************
        * 指定リソースが固定画像表示機能かを判断する
        **************************************************************** */
        public static bool isFixImage(string resourceName)
        {
            CroppedImage cImage = Content.AppContents.GetContent<CroppedImage>(resourceName);
            if (cImage == null && !cImage.fImgInfo.isFixed)
            {
                return false;
            }
            return true;
        }
        /* *************************************************************
        * 既存のリストに新しい画像を追加する
        **************************************************************** */
        internal void addFixImage(ConsoleImagePart ciPart, string resourceName)
        {
            bool isCleared = clearFixImage(resourceName, ciPart.fImgInfo.groupNum);
            bool isIgnore = false;
            //既存のリストに画像が表示しないかを確認し、表示可能と判断した場合はリストに追加する
            if (!isCleared)
            {
                //格納対象の画像が親画像でない場合は、親画像の行番号を取得する
                //また、親画像の位置が胴的にずらされていた場合この画像も同じ用にずらす
                if (!ciPart.fImgInfo.isRoot && ciPart.fImgInfo.groupNum != -1)
                {
                    //子供の画像なのに親画像が存在しない場合はこの画像自体を無視する
                    isIgnore = true;
                    for (int i = displayFixImgLineList.Count - 1; i >= 0; i--)
                    {
                        if (displayFixImgLineList[i].fImgInfo.isRoot &&
                            displayFixImgLineList[i].fImgInfo.groupNum == ciPart.fImgInfo.groupNum)
                        {
                            ciPart.fixLineNo = displayFixImgLineList[i].fixLineNo;
                            ciPart.fImgInfo.fixPosX = displayFixImgLineList[i].fImgInfo.fixPosX;
                            ciPart.fImgInfo.fixPosY = displayFixImgLineList[i].fImgInfo.fixPosY;
                            isIgnore = false;
                        }
                    }
                }
                if (!isIgnore) {
                    displayFixImgLineList.Add(ciPart);
                }
            }
        }
    }
}
