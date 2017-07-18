//182101 PCDRP-Update:�摜�Œ�\���@�\�ŐV�K�쐬
//�Œ�\���摜��\�����邽�߂̃N���X
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using MinorShift.Emuera.Content;

namespace MinorShift.Emuera.GameView
{
    public class FixImgPrint
    {
        //�摜�Œ�\���@�\�̃O���[�v���C���[
        Bitmap[] fixBitmapImg;
        Graphics[] fixGraph;
        public float[] fixFadeAlpha;
        public bool isGroupEnabled;
        public int[] shakeDx;
        public bool[] effectTarget;
        public Bitmap allFadeBackBitmapImg; //�S�̃t�F�[�h���̃r�b�g�}�b�v�ۑ��p
        public Graphics allFadeBackGraph;   //�S�̃t�F�[�h���̉摜�����p
        internal EffectStatus EffectStatus = 0;       //�S�̃t�F�[�h���̏��  
        internal EffectTarget effectTargetType = EffectTarget.NotEffect;		//�G�t�F�N�g�̑Ώ�(�S�̂��O���[�v��)
        //public float[] effectDifference;//�����摜����x�Ƀt�F�[�h���鎞�Ɏg�������Ǝv���������g�p
        public float[] shakeDAlpha;
        private readonly List<ConsoleImagePart> displayFixImgLineList;

        /* *************************************************************
        * �R���X�g���N�^
        **************************************************************** */
        public FixImgPrint(int fixImgGroupNumber, int width, int height)
        {
            //�w�肪�Ȃ��A�������̓O���[�v���C���[����0�̏ꍇ�͖��g�p
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
                //�Œ�\���p�̃r�b�g�}�b�v�𐶐����Ă���
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
        * �摜�\��
        **************************************************************** */
        public void drawFixImage(int topLineNo, int bottomLineNo, Graphics graph, int pointY, bool isBackLog, bool force)
        {
            //�S�̃t�F�[�h���ł��łɈ�x�`�ʎ��s���̏ꍇ�̓o�b�t�@�����摜�ŏ㏑�����ďI��
            if (effectTargetType == EffectTarget.Screen)
            {
                if (EffectStatus == EffectStatus.EffectBuffered)
                {
                    graph.DrawImage(allFadeBackBitmapImg, new Rectangle(0, 0, allFadeBackBitmapImg.Width, allFadeBackBitmapImg.Height),
                        0, 0, allFadeBackBitmapImg.Width, allFadeBackBitmapImg.Height, GraphicsUnit.Pixel, null);
                    return;
                }
                else if (EffectStatus == EffectStatus.EffectStart) {
                    //�o�b�t�@�����̏���̏ꍇ�̓t�F�[�h�o�b�t�@�p�̃I�u�W�F�N�g���N���A���Ă���
                    allFadeBackGraph.Clear(Color.Transparent);
                }
            }

            //�R���t�B�O�ŃO���[�v�����w�肵�Ă���ꍇ�O���[�v�p���C���[�̈���N���A����
            if (isGroupEnabled) {
                for (int i = 0; i < Config.FixImgGroupNumber; i++)
                {
                    //�G�t�F�N�g��(�o�b�t�@�ς�)�̏ꍇ�̓r�b�g�}�b�v���g���񂹂�̂ŃN���A���Ȃ�
                    //(���w�i�O���[�v�͎g���񂹂Ȃ����w�i�������ɃN���A����)
                    if (EffectStatus != EffectStatus.EffectBuffered)
                    {
                        fixGraph[i].Clear(Color.Transparent);
                    }
                }
            }

            //���X�g�ɉ摜�����݂��Ȃ��A�o�b�N���O���ꍇ�͏I��
            if (displayFixImgLineList.Count == 0 || isBackLog) {
                return;
            }

            //�Œ�\���摜�O���[�v�p���C���X�^���X�̐���
            List<FixGroupInfo> fgInfo = new List<FixGroupInfo>();
            int backGroupNum = -1;

            List<int> backGroupIdInfo =new List<int>();
            //�����͌Œ�\���摜�ȊO�̃X�N���[����Ԃ��m�F���ĕ\�����Ă������A
            //���ꂾ�ƌ��Ǖ��͂��ς������ꍇ�ɌŒ�\���摜���͈͂���R��ĕ\������Ȃ��Ȃ�̂�
            //���̏����͔�����
            for (int i = 0; i < displayFixImgLineList.Count; i++)
            {
                //�O���[�vID���w�肳��Ă��Ȃ����A����̖������I�[�o�[�����w��̏ꍇ�͖�������
                //�ʏ�̉�ʂɏ�������
                if (displayFixImgLineList[i].fImgInfo.groupNum == -1 ||
                    displayFixImgLineList[i].fImgInfo.groupNum >= Config.FixImgGroupNumber)
                {
                    displayFixImgLineList[i].DrawTo(graph, pointY, isBackLog, true, Config.TextDrawingMode);
                }
                else
                {
                    //�O���[�v�@�\�𗘗p���Ă���A�Y���̍s���e�ƂȂ�摜�̏ꍇ�͓��ߗ��Ɣ��]�ݒ��ۑ����Ă���
                    if (displayFixImgLineList[i].fImgInfo.isRoot)
                    {
                        if (fgInfo.Count < Config.FixImgGroupNumber)
                        {
                            fgInfo.Add(new FixGroupInfo(displayFixImgLineList[i].fImgInfo.fixAlpha * fixFadeAlpha[displayFixImgLineList[i].fImgInfo.groupNum] + shakeDAlpha[displayFixImgLineList[i].fImgInfo.groupNum],
                                displayFixImgLineList[i].fImgInfo.groupNum,
                                displayFixImgLineList[i].fImgInfo.isReverse));
                        }
                        //�Ώۂ̍s���O���[�v�摜�̔w�i�����w�������Ă���ꍇ�͓����p�̏����擾���Ă���
                        if (displayFixImgLineList[i].isBackGrouped) {
                            //���̉摜�͔w�i�摜�B�w�i��Graphics�I�u�W�F�N�g�͕K���������g�p����
                            backGroupNum = fgInfo.Count - 1;
                            
                            //�G�t�F�N�g���ɂ��o�b�t�@�����O���Ă���ꍇ�͔w�i�̈���폜���čď�������
                            if (EffectStatus == EffectStatus.EffectBuffered) {
                                fixGraph[fgInfo[backGroupNum].groupID].Clear(Color.Transparent);
                                displayFixImgLineList[i].DrawTo(fixGraph[fgInfo[backGroupNum].groupID], pointY, isBackLog, true, Config.TextDrawingMode);
                            }
                            backGroupIdInfo = displayFixImgLineList[i].backGroupList;
                        }
                    }
                    //�G�t�F�N�g��(�o�b�t�@�ς�)�ŁA���w�i�ȊO�̃��C���[�͎g���񂹂�̂ŏ������Ȃ�
                    if (EffectStatus != EffectStatus.EffectBuffered)
                    {
                        //���\�[�X���Ŏw�肳�ꂽ�O���[�v�ɏ�������
                        displayFixImgLineList[i].DrawTo(fixGraph[displayFixImgLineList[i].fImgInfo.groupNum], pointY, isBackLog, true, Config.TextDrawingMode);
                    }
                }
                //�Œ�摜�̏ꍇ�s�Ƃ����T�O���Ȃ��̂�pointY�̒l�����Z���Ȃ�
            }

            //�Ώۂ̍s���O���[�v�摜�̔w�i�����w�������Ă���ꍇ
            if (backGroupNum != -1)
            {
                for (int i = 0; i < fgInfo.Count; i++)
                {
                    //�w�i�摜�ł͂Ȃ��A���w�i�����Ɏw�肳�ꂽ�O���[�vID�̏ꍇ
                    if (i != backGroupNum && backGroupIdInfo.Contains(fgInfo[i].groupID)) {
                        //�O���[�v�摜���Ɏw�肳�ꂽ���ߗ��͖������ĕs�����ŏo��
                        //�e�摜�ɔ��]�w�肪�������ꍇ�͔��]���ĕ\������
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
                //�Ō�ɔw�i�摜���݂̏������C����ʂɏo�͂���
                //�e�摜�ɔ��]�w�肪�������ꍇ�͔��]���ĕ\������
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

            //�s�w��̏������݂��I������i�K�ŁA�Œ�\���@�\�̃C���[�W��W���̗̈�ɏ�������
            for (int i = 0; i < fgInfo.Count; i++)
            {
                //�O���[�v�摜�̔w�i�����w��@�\�̑ΏۊO�̃O���[�v�̏ꍇ
                if (!backGroupIdInfo.Contains(fgInfo[i].groupID)) {
                    //�e�摜�ɔ��]�w�肪�������ꍇ�͔��]���ĕ\������
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
        * �摜����
        **************************************************************** */
        public bool clearFixImage(string resourceName, int groupNum)
        {
            bool bRet = false;

            //���\�[�X����CLEARALL�w�肾�����ꍇ�͑S����
            //TODO:�o�b�N���O�Ή����C�ɂȂ邪�c
            if (resourceName.Equals("CLEARALL", StringComparison.OrdinalIgnoreCase))
            {
                //�O���[�v�w�肪�Ȃ��ꍇ�͑S����
                if (groupNum == -1)
                {
                    displayFixImgLineList.Clear();
                    bRet = true;
                    return bRet;

                }
                //�O���[�v�w�肪����ꍇ�͊Y���̃O���[�v�̂ݍ폜����
                else
                {
                    for (int i = displayFixImgLineList.Count - 1; i >= 0; i--)
                    {
                        //�w�肳�ꂽ���\�[�X���Ɠ������O�����X�g�ɑ��݂���ꍇ�͍폜�w���Ɣ��f���ă��X�g�������
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
                //�w�肳�ꂽ���\�[�X���Ɠ������O�����X�g�ɑ��݂���ꍇ�͍폜�w���Ɣ��f���ă��X�g�������
                if (displayFixImgLineList[i].ResourceName.Equals(resourceName, StringComparison.OrdinalIgnoreCase))
                {
                    displayFixImgLineList.RemoveAt(i);
                    bRet = true;
                }
            }
            return bRet;
        }

        /* *************************************************************
        * �摜����(CLEARLINE)
        **************************************************************** */
        public void clearLineFixImage(int argNum)
        {
            for (int i = displayFixImgLineList.Count - 1; i >= 0; i--)
            {
                //���ݖ����̍s�ԍ����傫���s�ԍ������摜��CLEARLINE�ɂ��������ꂽ�s��
                //�Y������Ɣ��f���ėv�f���폜
                if (displayFixImgLineList[i].fixLineNo > argNum)
                {
                    displayFixImgLineList.RemoveAt(i);
                }
            }
        }
        /* *************************************************************
        * �w�胊�\�[�X���Œ�摜�\���@�\���𔻒f����
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
        * �����̃��X�g�ɐV�����摜��ǉ�����
        **************************************************************** */
        internal void addFixImage(ConsoleImagePart ciPart, string resourceName)
        {
            bool isCleared = clearFixImage(resourceName, ciPart.fImgInfo.groupNum);
            bool isIgnore = false;
            //�����̃��X�g�ɉ摜���\�����Ȃ������m�F���A�\���\�Ɣ��f�����ꍇ�̓��X�g�ɒǉ�����
            if (!isCleared)
            {
                //�i�[�Ώۂ̉摜���e�摜�łȂ��ꍇ�́A�e�摜�̍s�ԍ����擾����
                //�܂��A�e�摜�̈ʒu�����I�ɂ��炳��Ă����ꍇ���̉摜�������p�ɂ��炷
                if (!ciPart.fImgInfo.isRoot && ciPart.fImgInfo.groupNum != -1)
                {
                    //�q���̉摜�Ȃ̂ɐe�摜�����݂��Ȃ��ꍇ�͂��̉摜���̂𖳎�����
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
