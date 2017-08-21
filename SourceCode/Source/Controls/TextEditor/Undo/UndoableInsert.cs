/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using Sheng.SailingEase.Controls.TextEditor.Document;
namespace Sheng.SailingEase.Controls.TextEditor.Undo
{
	public class UndoableInsert : IUndoableOperation
	{
		IDocument document;
		int      offset;
		string   text;
		public UndoableInsert(IDocument document, int offset, string text)
		{
			if (document == null) {
				throw new ArgumentNullException("document");
			}
			if (offset < 0 || offset > document.TextLength) {
				throw new ArgumentOutOfRangeException("offset");
			}
			Debug.Assert(text != null, "text can't be null");
			this.document = document;
			this.offset   = offset;
			this.text     = text;
		}
		public void Undo()
		{
			document.UndoStack.AcceptChanges = false;
			document.Remove(offset, text.Length);
			document.UndoStack.AcceptChanges = true;
		}
		public void Redo()
		{
			document.UndoStack.AcceptChanges = false;
			document.Insert(offset, text);
			document.UndoStack.AcceptChanges = true;
		}
	}
}
