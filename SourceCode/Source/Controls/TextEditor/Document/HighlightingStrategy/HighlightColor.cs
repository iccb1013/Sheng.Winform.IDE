/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Xml;
namespace Sheng.SailingEase.Controls.TextEditor.Document
{
	public class HighlightColor
	{
		Color  color;
		Color  backgroundcolor = System.Drawing.Color.WhiteSmoke;
		bool   bold   = false;
		bool   italic = false;
		bool   hasForeground = false;
		bool   hasBackground = false;
		public bool HasForeground {
			get {
				return hasForeground;
			}
		}
		public bool HasBackground {
			get {
				return hasBackground;
			}
		}
		public bool Bold {
			get {
				return bold;
			}
		}
		public bool Italic {
			get {
				return italic;
			}
		}
		public Color BackgroundColor {
			get {
				return backgroundcolor;
			}
		}
		public Color Color {
			get {
				return color;
			}
		}
		public Font GetFont(FontContainer fontContainer)
		{
			if (Bold) {
				return Italic ? fontContainer.BoldItalicFont : fontContainer.BoldFont;
			}
			return Italic ? fontContainer.ItalicFont : fontContainer.RegularFont;
		}
		Color ParseColorString(string colorName)
		{
			string[] cNames = colorName.Split('*');
			PropertyInfo myPropInfo = typeof(System.Drawing.SystemColors).GetProperty(cNames[0], BindingFlags.Public |
			                                                                          BindingFlags.Instance |
			                                                                          BindingFlags.Static);
			Color c = (Color)myPropInfo.GetValue(null, null);
			if (cNames.Length == 2) {
				double factor = Double.Parse(cNames[1]) / 100;
				c = Color.FromArgb((int)((double)c.R * factor), (int)((double)c.G * factor), (int)((double)c.B * factor));
			}
			return c;
		}
		public HighlightColor(XmlElement el)
		{
			Debug.Assert(el != null, "Sheng.SailingEase.Controls.TextEditor.Document.SyntaxColor(XmlElement el) : el == null");
			if (el.Attributes["bold"] != null) {
				bold = Boolean.Parse(el.Attributes["bold"].InnerText);
			}
			if (el.Attributes["italic"] != null) {
				italic = Boolean.Parse(el.Attributes["italic"].InnerText);
			}
			if (el.Attributes["color"] != null) {
				string c = el.Attributes["color"].InnerText;
				if (c[0] == '#') {
					color = ParseColor(c);
				} else if (c.StartsWith("SystemColors.")) {
					color = ParseColorString(c.Substring("SystemColors.".Length));
				} else {
					color = (Color)(Color.GetType()).InvokeMember(c, BindingFlags.GetProperty, null, Color, new object[0]);
				}
				hasForeground = true;
			} else {
				color = Color.Transparent; 
			}
			if (el.Attributes["bgcolor"] != null) {
				string c = el.Attributes["bgcolor"].InnerText;
				if (c[0] == '#') {
					backgroundcolor = ParseColor(c);
				} else if (c.StartsWith("SystemColors.")) {
					backgroundcolor = ParseColorString(c.Substring("SystemColors.".Length));
				} else {
					backgroundcolor = (Color)(Color.GetType()).InvokeMember(c, BindingFlags.GetProperty, null, Color, new object[0]);
				}
				hasBackground = true;
			}
		}
		public HighlightColor(XmlElement el, HighlightColor defaultColor)
		{
			Debug.Assert(el != null, "Sheng.SailingEase.Controls.TextEditor.Document.SyntaxColor(XmlElement el) : el == null");
			if (el.Attributes["bold"] != null) {
				bold = Boolean.Parse(el.Attributes["bold"].InnerText);
			} else {
				bold = defaultColor.Bold;
			}
			if (el.Attributes["italic"] != null) {
				italic = Boolean.Parse(el.Attributes["italic"].InnerText);
			} else {
				italic = defaultColor.Italic;
			}
			if (el.Attributes["color"] != null) {
				string c = el.Attributes["color"].InnerText;
				if (c[0] == '#') {
					color = ParseColor(c);
				} else if (c.StartsWith("SystemColors.")) {
					color = ParseColorString(c.Substring("SystemColors.".Length));
				} else {
					color = (Color)(Color.GetType()).InvokeMember(c, BindingFlags.GetProperty, null, Color, new object[0]);
				}
				hasForeground = true;
			} else {
				color = defaultColor.color;
			}
			if (el.Attributes["bgcolor"] != null) {
				string c = el.Attributes["bgcolor"].InnerText;
				if (c[0] == '#') {
					backgroundcolor = ParseColor(c);
				} else if (c.StartsWith("SystemColors.")) {
					backgroundcolor = ParseColorString(c.Substring("SystemColors.".Length));
				} else {
					backgroundcolor = (Color)(Color.GetType()).InvokeMember(c, BindingFlags.GetProperty, null, Color, new object[0]);
				}
				hasBackground = true;
			} else {
				backgroundcolor = defaultColor.BackgroundColor;
			}
		}
		public HighlightColor(Color color, bool bold, bool italic)
		{
			hasForeground = true;
			this.color  = color;
			this.bold   = bold;
			this.italic = italic;
		}
		public HighlightColor(Color color, Color backgroundcolor, bool bold, bool italic)
		{
			hasForeground = true;
			hasBackground  = true;
			this.color            = color;
			this.backgroundcolor  = backgroundcolor;
			this.bold             = bold;
			this.italic           = italic;
		}
		public HighlightColor(string systemColor, string systemBackgroundColor, bool bold, bool italic)
		{
			hasForeground = true;
			hasBackground  = true;
			this.color = ParseColorString(systemColor);
			this.backgroundcolor = ParseColorString(systemBackgroundColor);
			this.bold         = bold;
			this.italic       = italic;
		}
		public HighlightColor(string systemColor, bool bold, bool italic)
		{
			hasForeground = true;
			this.color = ParseColorString(systemColor);
			this.bold         = bold;
			this.italic       = italic;
		}
		static Color ParseColor(string c)
		{
			int a = 255;
			int offset = 0;
			if (c.Length > 7) {
				offset = 2;
				a = Int32.Parse(c.Substring(1,2), NumberStyles.HexNumber);
			}
			int r = Int32.Parse(c.Substring(1 + offset,2), NumberStyles.HexNumber);
			int g = Int32.Parse(c.Substring(3 + offset,2), NumberStyles.HexNumber);
			int b = Int32.Parse(c.Substring(5 + offset,2), NumberStyles.HexNumber);
			return Color.FromArgb(a, r, g, b);
		}
		public override string ToString()
		{
			return "[HighlightColor: Bold = " + Bold +
				", Italic = " + Italic +
				", Color = " + Color +
				", BackgroundColor = " + BackgroundColor + "]";
		}
	}
}
