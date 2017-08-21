/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Drawing;
using SWF = System.Windows.Forms;
namespace Sheng.SailingEase.Controls.TextEditor.Document
{
	public class Bookmark
	{
		IDocument document;
		TextAnchor anchor;
		TextLocation location;
		bool isEnabled = true;
		public IDocument Document {
			get {
				return document;
			}
			set {
				if (document != value) {
					if (anchor != null) {
						location = anchor.Location;
						anchor = null;
					}
					document = value;
					CreateAnchor();
					OnDocumentChanged(EventArgs.Empty);
				}
			}
		}
		void CreateAnchor()
		{
			if (document != null) {
				LineSegment line = document.GetLineSegment(Math.Max(0, Math.Min(location.Line, document.TotalNumberOfLines-1)));
				anchor = line.CreateAnchor(Math.Max(0, Math.Min(location.Column, line.Length)));
				anchor.MovementType = AnchorMovementType.AfterInsertion;
				anchor.Deleted += AnchorDeleted;
			}
		}
		void AnchorDeleted(object sender, EventArgs e)
		{
			document.BookmarkManager.RemoveMark(this);
		}
		public TextAnchor Anchor {
			get { return anchor; }
		}
		public TextLocation Location {
			get {
				if (anchor != null)
					return anchor.Location;
				else
					return location;
			}
			set {
				location = value;
				CreateAnchor();
			}
		}
		public event EventHandler DocumentChanged;
		protected virtual void OnDocumentChanged(EventArgs e)
		{
			if (DocumentChanged != null) {
				DocumentChanged(this, e);
			}
		}
		public bool IsEnabled {
			get {
				return isEnabled;
			}
			set {
				if (isEnabled != value) {
					isEnabled = value;
					if (document != null) {
						document.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.SingleLine, LineNumber));
						document.CommitUpdate();
					}
					OnIsEnabledChanged(EventArgs.Empty);
				}
			}
		}
		public event EventHandler IsEnabledChanged;
		protected virtual void OnIsEnabledChanged(EventArgs e)
		{
			if (IsEnabledChanged != null) {
				IsEnabledChanged(this, e);
			}
		}
		public int LineNumber {
			get {
				if (anchor != null)
					return anchor.LineNumber;
				else
					return location.Line;
			}
		}
		public int ColumnNumber {
			get {
				if (anchor != null)
					return anchor.ColumnNumber;
				else
					return location.Column;
			}
		}
		public virtual bool CanToggle {
			get {
				return true;
			}
		}
		public Bookmark(IDocument document, TextLocation location) : this(document, location, true)
		{
		}
		public Bookmark(IDocument document, TextLocation location, bool isEnabled)
		{
			this.document = document;
			this.isEnabled = isEnabled;
			this.Location = location;
		}
		public virtual bool Click(SWF.Control parent, SWF.MouseEventArgs e)
		{
			if (e.Button == SWF.MouseButtons.Left && CanToggle) {
				document.BookmarkManager.RemoveMark(this);
				return true;
			}
			return false;
		}
		public virtual void Draw(IconBarMargin margin, Graphics g, Point p)
		{
			margin.DrawBookmark(g, p.Y, isEnabled);
		}
	}
}
