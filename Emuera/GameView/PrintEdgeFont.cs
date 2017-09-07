//182101 PCDRP-Update:�t�H���g�̉����@�\�p
using System.Drawing;
using System.Drawing.Drawing2D;
namespace MinorShift.Emuera.GameView
{
    public static class PrintEdgeFont
    {
        public static bool edgeEnabled = false;
        public static Color currentEdgeColor = Color.Black; // will be overwritten by config load
        public static string fontName;
        public static void DrawString(Graphics graph, string Str, Color color, int PointX, int pointY, Font font = null)
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

            if(font != null)
                emSize = font.SizeInPoints * graph.DpiY / 72;  // update emSize since we have a complete font

            else
            { // if SETFONT font is specified use it otherwise use config font
                if (fontName != null)
                    font = new Font(fontName, emSize);
                else
                    font = Config.Font;
            }

            gp.AddString(Str, font.FontFamily, (int)FontStyle.Regular, emSize,
			                new Point(PointX, pointY), StringFormat.GenericDefault);

			//�p�X�̐�����`��
			Pen drawPen = new Pen(currentEdgeColor, 3.5F);
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
