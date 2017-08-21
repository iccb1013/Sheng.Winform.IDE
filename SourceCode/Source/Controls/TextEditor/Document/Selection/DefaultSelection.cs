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
	public class DefaultSelection : ISelection
	{
		IDocument document;
		bool      isRectangularSelection;
		TextLocation     startPosition;
		TextLocation     endPosition;
		public TextLocation StartPosition {
			get {
				return startPosition;
			}
			set {
				DefaultDocument.ValidatePosition(document, value);
				startPosition = value;
			}
		}
		public TextLocation EndPosition {
			get {
				return endPosition;
			}
			set {
				DefaultDocument.ValidatePosition(document, value);
				endPosition = value;
			}
		}
		public int Offset {
			get {
				return document.PositionToOffset(startPosition);
			}
		}
		public int EndOffset {
			get {
				return document.PositionToOffset(endPosition);
			}
		}
		public int Length {
			get {
				return EndOffset - Offset;
			}
		}
		public bool IsEmpty {
			get {
				return startPosition == endPosition;
			}
		}
		public bool IsRectangularSelection {
			get {
				return isRectangularSelection;
			}
			set {
				isRectangularSelection = value;
			}
		}
		public string SelectedText {
			get {
				if (document != null) {
					if (Length < 0) {
						return null;
					}
					return document.GetText(Offset, Length);
				}
				return null;
			}
		}
		public DefaultSelection(IDocument document, TextLocation startPosition, TextLocation endPosition)
		{
			DefaultDocument.ValidatePosition(document, startPosition);
			DefaultDocument.ValidatePosition(document, endPosition);
			Debug.Assert(startPosition <= endPosition);
			this.document      = document;
			this.startPosition = startPosition;
			this.endPosition   = endPosition;
		}
		public override string ToString()
		{
			return String.Format("[DefaultSelection : StartPosition={0}, EndPosition={1}]", startPosition, endPosition);
		}
		public bool ContainsPosition(TextLocation position)
		{
			if (this.IsEmpty)
				return false;
			return startPosition.Y < position.Y && position.Y  < endPosition.Y ||
				startPosition.Y == position.Y && startPosition.X <= position.X && (startPosition.Y != endPosition.Y || position.X <= endPosition.X) ||
				endPosition.Y == position.Y && startPosition.Y != endPosition.Y && position.X <= endPosition.X;
		}
		public bool ContainsOffset(int offset)
		{
			return Offset <= offset && offset <= EndOffset;
		}
	}
}
