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
	public enum TextMarkerType
	{
		Invisible,
		SolidBlock,
		Underlined,
		WaveLine
	}
	public class TextMarker : AbstractSegment
	{
		TextMarkerType textMarkerType;
		Color          color;
		Color          foreColor;
		string         toolTip = null;
		bool           overrideForeColor = false;
		public TextMarkerType TextMarkerType {
			get {
				return textMarkerType;
			}
		}
		public Color Color {
			get {
				return color;
			}
		}
		public Color ForeColor {
			get {
				return foreColor;
			}
		}
		public bool OverrideForeColor {
			get {
				return overrideForeColor;
			}
		}
		public bool IsReadOnly { get; set; }
		public string ToolTip {
			get {
				return toolTip;
			}
			set {
				toolTip = value;
			}
		}
		public int EndOffset {
			get {
				return Offset + Length - 1;
			}
		}
		public TextMarker(int offset, int length, TextMarkerType textMarkerType) : this(offset, length, textMarkerType, Color.Red)
		{
		}
		public TextMarker(int offset, int length, TextMarkerType textMarkerType, Color color)
		{
			if (length < 1) length = 1;
			this.offset          = offset;
			this.length          = length;
			this.textMarkerType  = textMarkerType;
			this.color           = color;
		}
		public TextMarker(int offset, int length, TextMarkerType textMarkerType, Color color, Color foreColor)
		{
			if (length < 1) length = 1;
			this.offset          = offset;
			this.length          = length;
			this.textMarkerType  = textMarkerType;
			this.color           = color;
			this.foreColor       = foreColor;
			this.overrideForeColor = true;
		}
	}
}
