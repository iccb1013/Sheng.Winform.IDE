/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Sheng.SailingEase.Controls.TextEditor.Document
{
	public class LineCountChangeEventArgs : EventArgs
	{
		IDocument document;
		int       start;
		int       moved;
		public IDocument Document {
			get {
				return document;
			}
		}
		public int LineStart {
			get {
				return start;
			}
		}
		public int LinesMoved {
			get {
				return moved;
			}
		}
		public LineCountChangeEventArgs(IDocument document, int lineStart, int linesMoved)
		{
			this.document = document;
			this.start    = lineStart;
			this.moved    = linesMoved;
		}
	}
	public class LineEventArgs : EventArgs
	{
		IDocument document;
		LineSegment lineSegment;
		public IDocument Document {
			get { return document; }
		}
		public LineSegment LineSegment {
			get { return lineSegment; }
		}
		public LineEventArgs(IDocument document, LineSegment lineSegment)
		{
			this.document = document;
			this.lineSegment = lineSegment;
		}
		public override string ToString()
		{
			return string.Format("[LineEventArgs Document={0} LineSegment={1}]", this.document, this.lineSegment);
		}
	}
	public class LineLengthChangeEventArgs : LineEventArgs
	{
		int lengthDelta;
		public int LengthDelta {
			get { return lengthDelta; }
		}
		public LineLengthChangeEventArgs(IDocument document, LineSegment lineSegment, int moved)
			: base(document, lineSegment)
		{
			this.lengthDelta = moved;
		}
		public override string ToString()
		{
			return string.Format("[LineLengthEventArgs Document={0} LineSegment={1} LengthDelta={2}]", this.Document, this.LineSegment, this.lengthDelta);
		}
	}
}
