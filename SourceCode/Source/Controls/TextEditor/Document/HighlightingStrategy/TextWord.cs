/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using System.Drawing;
namespace Sheng.SailingEase.Controls.TextEditor.Document
{
	public enum TextWordType {
		Word,
		Space,
		Tab
	}
	public class TextWord
	{
		HighlightColor  color;
		LineSegment     line;
		IDocument       document;
		int          offset;
		int          length;
		public sealed class SpaceTextWord : TextWord
		{
			public SpaceTextWord()
			{
				length = 1;
			}
			public SpaceTextWord(HighlightColor color)
			{
				length = 1;
				base.SyntaxColor = color;
			}
			public override Font GetFont(FontContainer fontContainer)
			{
				return null;
			}
			public override TextWordType Type {
				get {
					return TextWordType.Space;
				}
			}
			public override bool IsWhiteSpace {
				get {
					return true;
				}
			}
		}
		public sealed class TabTextWord : TextWord
		{
			public TabTextWord()
			{
				length = 1;
			}
			public TabTextWord(HighlightColor color)
			{
				length = 1;
				base.SyntaxColor = color;
			}
			public override Font GetFont(FontContainer fontContainer)
			{
				return null;
			}
			public override TextWordType Type {
				get {
					return TextWordType.Tab;
				}
			}
			public override bool IsWhiteSpace {
				get {
					return true;
				}
			}
		}
		static TextWord spaceWord = new SpaceTextWord();
		static TextWord tabWord   = new TabTextWord();
		bool hasDefaultColor;
		public static TextWord Space {
			get {
				return spaceWord;
			}
		}
		public static TextWord Tab {
			get {
				return tabWord;
			}
		}
		public int Offset {
			get {
				return offset;
			}
		}
		public int Length {
			get {
				return length;
			}
		}
		public static TextWord Split(ref TextWord word, int pos)
		{
			#if DEBUG
			if (word.Type != TextWordType.Word)
				throw new ArgumentException("word.Type must be Word");
			if (pos <= 0)
				throw new ArgumentOutOfRangeException("pos", pos, "pos must be > 0");
			if (pos >= word.Length)
				throw new ArgumentOutOfRangeException("pos", pos, "pos must be < word.Length");
			#endif
			TextWord after = new TextWord(word.document, word.line, word.offset + pos, word.length - pos, word.color, word.hasDefaultColor);
			word = new TextWord(word.document, word.line, word.offset, pos, word.color, word.hasDefaultColor);
			return after;
		}
		public bool HasDefaultColor {
			get {
				return hasDefaultColor;
			}
		}
		public virtual TextWordType Type {
			get {
				return TextWordType.Word;
			}
		}
		public string Word {
			get {
				if (document == null) {
					return String.Empty;
				}
				return document.GetText(line.Offset + offset, length);
			}
		}
		public virtual Font GetFont(FontContainer fontContainer)
		{
			return color.GetFont(fontContainer);
		}
		public Color Color {
			get {
				if (color == null)
					return Color.Black;
				else
					return color.Color;
			}
		}
		public bool Bold {
			get {
				if (color == null)
					return false;
				else
					return color.Bold;
			}
		}
		public bool Italic {
			get {
				if (color == null)
					return false;
				else
					return color.Italic;
			}
		}
		public HighlightColor SyntaxColor {
			get {
				return color;
			}
			set {
				Debug.Assert(value != null);
				color = value;
			}
		}
		public virtual bool IsWhiteSpace {
			get {
				return false;
			}
		}
		protected TextWord()
		{
		}
		public TextWord(IDocument document, LineSegment line, int offset, int length, HighlightColor color, bool hasDefaultColor)
		{
			Debug.Assert(document != null);
			Debug.Assert(line != null);
			Debug.Assert(color != null);
			this.document = document;
			this.line  = line;
			this.offset = offset;
			this.length = length;
			this.color = color;
			this.hasDefaultColor = hasDefaultColor;
		}
		public override string ToString()
		{
			return "[TextWord: Word = " + Word + ", Color = " + Color + "]";
		}
	}
}
