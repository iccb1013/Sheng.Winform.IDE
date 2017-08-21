/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Text;
namespace Sheng.SailingEase.Controls.TextEditor.Document
{
	public sealed class LineSegment : ISegment
	{
		internal LineSegmentTree.Enumerator treeEntry;
		int totalLength, delimiterLength;
		List<TextWord> words;
		SpanStack highlightSpanStack;
		public TextWord GetWord(int column)
		{
			int curColumn = 0;
			foreach (TextWord word in words) {
				if (column < curColumn + word.Length) {
					return word;
				}
				curColumn += word.Length;
			}
			return null;
		}
		public bool IsDeleted {
			get { return !treeEntry.IsValid; }
		}
		public int LineNumber {
			get { return treeEntry.CurrentIndex; }
		}
		public int Offset {
			get { return treeEntry.CurrentOffset; }
		}
		public int Length {
			get	{ return totalLength - delimiterLength; }
		}
		int ISegment.Offset {
			get { return this.Offset; }
			set { throw new NotSupportedException(); }
		}
		int ISegment.Length {
			get { return this.Length; }
			set { throw new NotSupportedException(); }
		}
		public int TotalLength {
			get { return totalLength; }
			internal set { totalLength = value; }
		}
		public int DelimiterLength {
			get { return delimiterLength; }
			internal set { delimiterLength = value; }
		}
		public List<TextWord> Words {
			get {
				return words;
			}
			set {
				words = value;
			}
		}
		public HighlightColor GetColorForPosition(int x)
		{
			if (Words != null) {
				int xPos = 0;
				foreach (TextWord word in Words) {
					if (x < xPos + word.Length) {
						return word.SyntaxColor;
					}
					xPos += word.Length;
				}
			}
			return new HighlightColor(Color.Black, false, false);
		}
		public SpanStack HighlightSpanStack {
			get {
				return highlightSpanStack;
			}
			set {
				highlightSpanStack = value;
			}
		}
		public override string ToString()
		{
			if (IsDeleted)
				return "[LineSegment: (deleted) Length = " + Length + ", TotalLength = " + TotalLength + ", DelimiterLength = " + delimiterLength + "]";
			else
				return "[LineSegment: LineNumber=" + LineNumber + ", Offset = "+ Offset +", Length = " + Length + ", TotalLength = " + TotalLength + ", DelimiterLength = " + delimiterLength + "]";
		}
		Util.WeakCollection<TextAnchor> anchors;
		public TextAnchor CreateAnchor(int column)
		{
			if (column < 0 || column > Length)
				throw new ArgumentOutOfRangeException("column");
			TextAnchor anchor = new TextAnchor(this, column);
			AddAnchor(anchor);
			return anchor;
		}
		void AddAnchor(TextAnchor anchor)
		{
			Debug.Assert(anchor.Line == this);
			if (anchors == null)
				anchors = new Util.WeakCollection<TextAnchor>();
			anchors.Add(anchor);
		}
		internal void Deleted(ref DeferredEventList deferredEventList)
		{
			treeEntry = LineSegmentTree.Enumerator.Invalid;
			if (anchors != null) {
				foreach (TextAnchor a in anchors) {
					a.Delete(ref deferredEventList);
				}
				anchors = null;
			}
		}
		internal void RemovedLinePart(ref DeferredEventList deferredEventList, int startColumn, int length)
		{
			if (length == 0)
				return;
			Debug.Assert(length > 0);
			if (anchors != null) {
				List<TextAnchor> deletedAnchors = null;
				foreach (TextAnchor a in anchors) {
					if (a.ColumnNumber > startColumn) {
						if (a.ColumnNumber >= startColumn + length) {
							a.ColumnNumber -= length;
						} else {
							if (deletedAnchors == null)
								deletedAnchors = new List<TextAnchor>();
							a.Delete(ref deferredEventList);
							deletedAnchors.Add(a);
						}
					}
				}
				if (deletedAnchors != null) {
					foreach (TextAnchor a in deletedAnchors) {
						anchors.Remove(a);
					}
				}
			}
		}
		internal void InsertedLinePart(int startColumn, int length)
		{
			if (length == 0)
				return;
			Debug.Assert(length > 0);
			if (anchors != null) {
				foreach (TextAnchor a in anchors) {
					if (a.MovementType == AnchorMovementType.BeforeInsertion
					    ? a.ColumnNumber > startColumn
					    : a.ColumnNumber >= startColumn)
					{
						a.ColumnNumber += length;
					}
				}
			}
		}
		internal void MergedWith(LineSegment deletedLine, int firstLineLength)
		{
			if (deletedLine.anchors != null) {
				foreach (TextAnchor a in deletedLine.anchors) {
					a.Line = this;
					AddAnchor(a);
					a.ColumnNumber += firstLineLength;
				}
				deletedLine.anchors = null;
			}
		}
		internal void SplitTo(LineSegment followingLine)
		{
			if (anchors != null) {
				List<TextAnchor> movedAnchors = null;
				foreach (TextAnchor a in anchors) {
					if (a.MovementType == AnchorMovementType.BeforeInsertion
					    ? a.ColumnNumber > this.Length
					    : a.ColumnNumber >= this.Length)
					{
						a.Line = followingLine;
						followingLine.AddAnchor(a);
						a.ColumnNumber -= this.Length;
						if (movedAnchors == null)
							movedAnchors = new List<TextAnchor>();
						movedAnchors.Add(a);
					}
				}
				if (movedAnchors != null) {
					foreach (TextAnchor a in movedAnchors) {
						anchors.Remove(a);
					}
				}
			}
		}
	}
}
