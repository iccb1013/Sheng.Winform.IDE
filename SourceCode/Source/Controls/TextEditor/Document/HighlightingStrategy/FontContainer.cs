/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Drawing;
namespace Sheng.SailingEase.Controls.TextEditor.Document
{
	public class FontContainer
	{
		Font defaultFont;
		Font regularfont, boldfont, italicfont, bolditalicfont;
		public Font RegularFont {
			get {
				return regularfont;
			}
		}
		public Font BoldFont {
			get {
				return boldfont;
			}
		}
		public Font ItalicFont {
			get {
				return italicfont;
			}
		}
		public Font BoldItalicFont {
			get {
				return bolditalicfont;
			}
		}
		static float twipsPerPixelY;
		public static float TwipsPerPixelY {
			get {
				if (twipsPerPixelY == 0) {
					using (Bitmap bmp = new Bitmap(1,1)) {
						using (Graphics g = Graphics.FromImage(bmp)) {
							twipsPerPixelY = 1440 / g.DpiY;
						}
					}
				}
				return twipsPerPixelY;
			}
		}
		public Font DefaultFont {
			get {
				return defaultFont;
			}
			set {
				float pixelSize = (float)Math.Round(value.SizeInPoints * 20 / TwipsPerPixelY);
				defaultFont    = value;
				regularfont    = new Font(value.FontFamily, pixelSize * TwipsPerPixelY / 20f, FontStyle.Regular);
				boldfont       = new Font(regularfont, FontStyle.Bold);
				italicfont     = new Font(regularfont, FontStyle.Italic);
				bolditalicfont = new Font(regularfont, FontStyle.Bold | FontStyle.Italic);
			}
		}
		public static Font ParseFont(string font)
		{
			string[] descr = font.Split(new char[]{',', '='});
			return new Font(descr[1], Single.Parse(descr[3]));
		}
		public FontContainer(Font defaultFont)
		{
			this.DefaultFont = defaultFont;
		}
	}
}
