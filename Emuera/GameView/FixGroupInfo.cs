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

            // �Œ�摜�\���p�̃I�t�X�N���[���o�b�t�@��K�p
            //ColorMatrix�I�u�W�F�N�g�̍쐬
            System.Drawing.Imaging.ColorMatrix cm =
                new System.Drawing.Imaging.ColorMatrix();
            //���ߗ��Ƀ��\�[�X�t�@�C���Ŏw�肵�����ߗ����w�肷��
            cm.Matrix00 = 1;
            cm.Matrix11 = 1;
            cm.Matrix22 = 1;
            cm.Matrix33 = fixAlpha;
            cm.Matrix44 = 1;

            //ImageAttributes�I�u�W�F�N�g�̍쐬
            this.ia = new System.Drawing.Imaging.ImageAttributes();
            //ColorMatrix��ݒ肷��
            this.ia.SetColorMatrix(cm);
        }
    }
}
