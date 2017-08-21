/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using System.Text;
namespace Sheng.SailingEase.Controls.TextEditor.Document
{
	public sealed class TextUtilities
	{
		public static string LeadingWhiteSpaceToTabs(string line, int tabIndent) {
			StringBuilder sb = new StringBuilder(line.Length);
			int consecutiveSpaces = 0;
			int i = 0;
			for(i = 0; i < line.Length; i++) {
				if(line[i] == ' ') {
					consecutiveSpaces++;
					if(consecutiveSpaces == tabIndent) {
						sb.Append('\t');
						consecutiveSpaces = 0;
					}
				}
				else if(line[i] == '\t') {
					sb.Append('\t');
					consecutiveSpaces = 0;
				}
				else {
					break;
				}
			}
			if(i < line.Length) {
				sb.Append(line.Substring(i-consecutiveSpaces));
			}
			return sb.ToString();
		}
		public static bool IsLetterDigitOrUnderscore(char c)
		{
			if(!Char.IsLetterOrDigit(c)) {
				return c == '_';
			}
			return true;
		}
		public enum CharacterType {
			LetterDigitOrUnderscore,
			WhiteSpace,
			Other
		}
		public static string GetExpressionBeforeOffset(TextArea textArea, int initialOffset)
		{
			IDocument document = textArea.Document;
			int offset = initialOffset;
			while (offset - 1 > 0) {
				switch (document.GetCharAt(offset - 1)) {
					case '\n':
					case '\r':
					case '}':
						goto done;
					case ']':
						offset = SearchBracketBackward(document, offset - 2, '[',']');
						break;
					case ')':
						offset = SearchBracketBackward(document, offset - 2, '(',')');
						break;
					case '.':
						--offset;
						break;
					case '"':
						if (offset < initialOffset - 1) {
							return null;
						}
						return "\"\"";
					case '\'':
						if (offset < initialOffset - 1) {
							return null;
						}
						return "'a'";
					case '>':
						if (document.GetCharAt(offset - 2) == '-') {
							offset -= 2;
							break;
						}
						goto done;
					default:
						if (Char.IsWhiteSpace(document.GetCharAt(offset - 1))) {
							--offset;
							break;
						}
						int start = offset - 1;
						if (!IsLetterDigitOrUnderscore(document.GetCharAt(start))) {
							goto done;
						}
						while (start > 0 && IsLetterDigitOrUnderscore(document.GetCharAt(start - 1))) {
							--start;
						}
						string word = document.GetText(start, offset - start).Trim();
						switch (word) {
							case "ref":
							case "out":
							case "in":
							case "return":
							case "throw":
							case "case":
								goto done;
						}
						if (word.Length > 0 && !IsLetterDigitOrUnderscore(word[0])) {
							goto done;
						}
						offset = start;
						break;
				}
			}
		done:
			if (offset < 0)
				return string.Empty;
			string resText=document.GetText(offset, textArea.Caret.Offset - offset ).Trim();
			int pos=resText.LastIndexOf('\n');
			if (pos>=0) {
				offset+=pos+1;
			}
			string expression = document.GetText(offset, textArea.Caret.Offset - offset ).Trim();
			return expression;
		}
		public static CharacterType GetCharacterType(char c)
		{
			if(IsLetterDigitOrUnderscore(c))
				return CharacterType.LetterDigitOrUnderscore;
			if(Char.IsWhiteSpace(c))
				return CharacterType.WhiteSpace;
			return CharacterType.Other;
		}
		public static int GetFirstNonWSChar(IDocument document, int offset)
		{
			while (offset < document.TextLength && Char.IsWhiteSpace(document.GetCharAt(offset))) {
				++offset;
			}
			return offset;
		}
		public static int FindWordEnd(IDocument document, int offset)
		{
			LineSegment line   = document.GetLineSegmentForOffset(offset);
			int     endPos = line.Offset + line.Length;
			while (offset < endPos && IsLetterDigitOrUnderscore(document.GetCharAt(offset))) {
				++offset;
			}
			return offset;
		}
		public static int FindWordStart(IDocument document, int offset)
		{
			LineSegment line = document.GetLineSegmentForOffset(offset);
			int lineOffset = line.Offset;
			while (offset > lineOffset && IsLetterDigitOrUnderscore(document.GetCharAt(offset - 1))) {
				--offset;
			}
			return offset;
		}
		public static int FindNextWordStart(IDocument document, int offset)
		{
			int originalOffset = offset;
			LineSegment line   = document.GetLineSegmentForOffset(offset);
			int     endPos = line.Offset + line.Length;
			CharacterType t = GetCharacterType(document.GetCharAt(offset));
			while (offset < endPos && GetCharacterType(document.GetCharAt(offset)) == t) {
				++offset;
			}
			while (offset < endPos && GetCharacterType(document.GetCharAt(offset)) == CharacterType.WhiteSpace) {
				++offset;
			}
			return offset;
		}
		public static int FindPrevWordStart(IDocument document, int offset)
		{
			int originalOffset = offset;
			if (offset > 0) {
				LineSegment line = document.GetLineSegmentForOffset(offset);
				CharacterType t = GetCharacterType(document.GetCharAt(offset - 1));
				while (offset > line.Offset && GetCharacterType(document.GetCharAt(offset - 1)) == t) {
					--offset;
				}
				if(t == CharacterType.WhiteSpace && offset > line.Offset) {
					t = GetCharacterType(document.GetCharAt(offset - 1));
					while (offset > line.Offset && GetCharacterType(document.GetCharAt(offset - 1)) == t) {
						--offset;
					}
				}
			}
			return offset;
		}
		public static string GetLineAsString(IDocument document, int lineNumber)
		{
			LineSegment line = document.GetLineSegment(lineNumber);
			return document.GetText(line.Offset, line.Length);
		}
		public static int SearchBracketBackward(IDocument document, int offset, char openBracket, char closingBracket)
		{
			return document.FormattingStrategy.SearchBracketBackward(document, offset, openBracket, closingBracket);
		}
		public static int SearchBracketForward(IDocument document, int offset, char openBracket, char closingBracket)
		{
			return document.FormattingStrategy.SearchBracketForward(document, offset, openBracket, closingBracket);
		}
		public static bool IsEmptyLine(IDocument document, int lineNumber)
		{
			return IsEmptyLine(document, document.GetLineSegment(lineNumber));
		}
		public static bool IsEmptyLine(IDocument document, LineSegment line)
		{
			for (int i = line.Offset; i < line.Offset + line.Length; ++i) {
				char ch = document.GetCharAt(i);
				if (!Char.IsWhiteSpace(ch)) {
					return false;
				}
			}
			return true;
		}
		static bool IsWordPart(char ch)
		{
			return IsLetterDigitOrUnderscore(ch) || ch == '.';
		}
		public static string GetWordAt(IDocument document, int offset)
		{
			if (offset < 0 || offset >= document.TextLength - 1 || !IsWordPart(document.GetCharAt(offset))) {
				return String.Empty;
			}
			int startOffset = offset;
			int endOffset   = offset;
			while (startOffset > 0 && IsWordPart(document.GetCharAt(startOffset - 1))) {
				--startOffset;
			}
			while (endOffset < document.TextLength - 1 && IsWordPart(document.GetCharAt(endOffset + 1))) {
				++endOffset;
			}
			Debug.Assert(endOffset >= startOffset);
			return document.GetText(startOffset, endOffset - startOffset + 1);
		}
	}
}
