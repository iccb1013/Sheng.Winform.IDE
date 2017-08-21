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
	public class UndoableReplace : IUndoableOperation
	{
		IDocument document;
		int       offset;
		string    text;
		string    origText;
		public UndoableReplace(IDocument document, int offset, string origText, string text)
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
			this.origText = origText;
		}
		public void Undo()
		{
			document.UndoStack.AcceptChanges = false;
			document.Replace(offset, text.Length, origText);
			document.UndoStack.AcceptChanges = true;
		}
		public void Redo()
		{
			document.UndoStack.AcceptChanges = false;
			document.Replace(offset, origText.Length, text);
			document.UndoStack.AcceptChanges = true;
		}
	}
}
