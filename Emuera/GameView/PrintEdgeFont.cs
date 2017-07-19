//182101 PCDRP-Update:�t�H���g�̉����@�\�p
using System.Drawing;
using System.Drawing.Drawing2D;
namespace MinorShift.Emuera.GameView
{
    public static class PrintEdgeFont
    {
        public static bool edgeEnabled = false;
        public static void DrawString(Graphics graph, string Str,Color color, int PointX, int pointY)
        {
			//�A���`�G���A�X
			SmoothingMode initialMode = graph.SmoothingMode;
			graph.SmoothingMode = SmoothingMode.HighQuality;

			//�����_�����O�i��
			CompositingQuality initialQuality = graph.CompositingQuality;
			graph.CompositingQuality = CompositingQuality.HighQuality;

            //graph.PixelOffsetMode = PixelOffsetMode.HighQuality;

            GraphicsPath gp = new GraphicsPath();

			//float emSize = (float)Config.Font.Height * Config.Font.FontFamily.GetEmHeight(Config.Font.Style) / Config.Font.FontFamily.GetLineSpacing(Config.Font.Style);

            float emSize = Config.Font.SizeInPoints * graph.DpiY / 72;  // 1 inch = 72 points

            gp.AddString(Str, Config.Font.FontFamily, (int)FontStyle.Regular, emSize,
			                new Point(PointX, pointY), StringFormat.GenericDefault);

			//�p�X�̐�����`��
			Pen drawPen = new Pen(Config.EdgeColor, 3.5F);
            drawPen.LineJoin = LineJoin.Round;
            graph.DrawPath(drawPen, gp);
            graph.DrawPath(drawPen, gp);

            //�h��
            Brush fillBrush = new SolidBrush(color);
			graph.FillPath(fillBrush, gp);

			drawPen.Dispose();
			fillBrush.Dispose();

			//�O�̂��ߌ��ɖ߂�
			graph.SmoothingMode = initialMode;
			graph.CompositingQuality = initialQuality;
        }
	}
}
