/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Sheng.SailingEase.Controls.TextEditor.Document;
namespace Sheng.SailingEase.Controls.TextEditor.Actions 
{
	public class ToggleBookmark : AbstractEditAction
	{
		public override void Execute(TextArea textArea)
		{
			textArea.Document.BookmarkManager.ToggleMarkAt(textArea.Caret.Position);
			textArea.Document.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.SingleLine, textArea.Caret.Line));
			textArea.Document.CommitUpdate();
		}
	}
	public class GotoPrevBookmark : AbstractEditAction
	{
		Predicate<Bookmark> predicate = null;
		public GotoPrevBookmark(Predicate<Bookmark> predicate)
		{
			this.predicate = predicate;
		}
		public override void Execute(TextArea textArea)
		{
			Bookmark mark = textArea.Document.BookmarkManager.GetPrevMark(textArea.Caret.Line, predicate);
			if (mark != null) {
				textArea.Caret.Position = mark.Location;
				textArea.SelectionManager.ClearSelection();
				textArea.SetDesiredColumn();
			}
		}
	}
	public class GotoNextBookmark : AbstractEditAction
	{
		Predicate<Bookmark> predicate = null;
		public GotoNextBookmark(Predicate<Bookmark> predicate)
		{
			this.predicate = predicate;
		}
		public override void Execute(TextArea textArea)
		{
			Bookmark mark = textArea.Document.BookmarkManager.GetNextMark(textArea.Caret.Line, predicate);
			if (mark != null) {
				textArea.Caret.Position = mark.Location;
				textArea.SelectionManager.ClearSelection();
				textArea.SetDesiredColumn();
			}
		}
	}
	public class ClearAllBookmarks : AbstractEditAction
	{
		Predicate<Bookmark> predicate = null;
		public ClearAllBookmarks(Predicate<Bookmark> predicate)
		{
			this.predicate = predicate;
		}
		public override void Execute(TextArea textArea)
		{
			textArea.Document.BookmarkManager.RemoveMarks(predicate);
			textArea.Document.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.WholeTextArea));
			textArea.Document.CommitUpdate();
		}
	}
}
