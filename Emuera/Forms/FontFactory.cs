using MinorShift.Emuera;
using System.Collections.Generic;
using System.Drawing;

internal class FontFactory
{

	static readonly Dictionary<(string fontname, int fontSize, FontStyle fontStyle), Font> fontDic = new Dictionary<(string fontname, int fontSize, FontStyle fontStyle), Font>();

	public static Font GetFont(string requestFontName, FontStyle style)
	{
		string fn = requestFontName;
		if (string.IsNullOrEmpty(requestFontName))
			fn = Config.FontName;
		if (!fontDic.ContainsKey((fn, Config.FontSize, style)))
		{
			var font = new Font(fn, Config.FontSize, style, GraphicsUnit.Pixel);
			if (font != null)
				fontDic.Add((fn, Config.FontSize, style), font);

		}
		Dictionary<FontStyle, Font> fontStyleDic = new Dictionary<FontStyle, Font>();
		if (!fontStyleDic.ContainsKey(style))
		{
			int fontsize = Config.FontSize;
			Font styledFont;
			try
			{
				#region EE_フォントファイル対応
				foreach (FontFamily ff in GlobalStatic.Pfc.Families)
				{
					if (ff.Name == fn)
					{
						styledFont = new Font(ff, fontsize, style, GraphicsUnit.Pixel);
						goto foundfont;
					}
				}
				styledFont = new Font(fn, fontsize, style, GraphicsUnit.Pixel);
			}
			catch
			{
				return null;
			}
		foundfont:
			#endregion
			fontStyleDic.Add(style, styledFont);
		}
		return fontStyleDic[style];
	}

	public static void ClearFont()
	{
		foreach (var font in fontDic)
		{
			font.Value.Dispose();
		}
		fontDic.Clear();
	}
}
