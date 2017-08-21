/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Sheng.SailingEase.Controls.TextEditor.Document
{
	public enum AnchorMovementType
	{
		BeforeInsertion,
		AfterInsertion
	}
	public sealed class TextAnchor
	{
		static Exception AnchorDeletedError()
		{
			return new InvalidOperationException("The text containing the anchor was deleted");
		}
		LineSegment lineSegment;
		int columnNumber;
		public LineSegment Line {
			get {
				if (lineSegment == null) throw AnchorDeletedError();
				return lineSegment;
			}
			internal set {
				lineSegment = value;
			}
		}
		public bool IsDeleted {
			get {
				return lineSegment == null;
			}
		}
		public int LineNumber {
			get {
				return this.Line.LineNumber;
			}
		}
		public int ColumnNumber {
			get {
				if (lineSegment == null) throw AnchorDeletedError();
				return columnNumber;
			}
			internal set {
				columnNumber = value;
			}
		}
		public TextLocation Location {
			get {
				return new TextLocation(this.ColumnNumber, this.LineNumber);
			}
		}
		public int Offset {
			get {
				return this.Line.Offset + columnNumber;
			}
		}
		public AnchorMovementType MovementType { get; set; }
		public event EventHandler Deleted;
		internal void Delete(ref DeferredEventList deferredEventList)
		{
			lineSegment = null;
			deferredEventList.AddDeletedAnchor(this);
		}
		internal void RaiseDeleted()
		{
			if (Deleted != null)
				Deleted(this, EventArgs.Empty);
		}
		internal TextAnchor(LineSegment lineSegment, int columnNumber)
		{
			this.lineSegment = lineSegment;
			this.columnNumber = columnNumber;
		}
		public override string ToString()
		{
			if (this.IsDeleted)
				return "[TextAnchor (deleted)]";
			else
				return "[TextAnchor " + this.Location.ToString() + "]";
		}
	}
}
