/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
namespace Sheng.SailingEase.Controls.TextEditor.Undo
{
	public class UndoStack
	{
		Stack<IUndoableOperation> undostack = new Stack<IUndoableOperation>();
		Stack<IUndoableOperation> redostack = new Stack<IUndoableOperation>();
		public TextEditorControlBase TextEditorControl = null;
		public event EventHandler ActionUndone;
		public event EventHandler ActionRedone;
		public event OperationEventHandler OperationPushed;
		internal bool AcceptChanges = true;
		public bool CanUndo {
			get {
				return undostack.Count > 0;
			}
		}
		public bool CanRedo {
			get {
				return redostack.Count > 0;
			}
		}
		public int UndoItemCount {
			get {
				return undostack.Count;
			}
		}
		public int RedoItemCount {
			get {
				return redostack.Count;
			}
		}
		int undoGroupDepth;
		int actionCountInUndoGroup;
		public void StartUndoGroup()
		{
			if (undoGroupDepth == 0) {
				actionCountInUndoGroup = 0;
			}
			undoGroupDepth++;
		}
		public void EndUndoGroup()
		{
			if (undoGroupDepth == 0)
				throw new InvalidOperationException("There are no open undo groups");
			undoGroupDepth--;
			if (undoGroupDepth == 0 && actionCountInUndoGroup > 1) {
				UndoQueue op = new UndoQueue(undostack, actionCountInUndoGroup);
				undostack.Push(op);
				if (OperationPushed != null) {
					OperationPushed(this, new OperationEventArgs(op));
				}
			}
		}
		public void AssertNoUndoGroupOpen()
		{
			if (undoGroupDepth != 0) {
				undoGroupDepth = 0;
				throw new InvalidOperationException("No undo group should be open at this point");
			}
		}
		public void Undo()
		{
			AssertNoUndoGroupOpen();
			if (undostack.Count > 0) {
				IUndoableOperation uedit = (IUndoableOperation)undostack.Pop();
				redostack.Push(uedit);
				uedit.Undo();
				OnActionUndone();
			}
		}
		public void Redo()
		{
			AssertNoUndoGroupOpen();
			if (redostack.Count > 0) {
				IUndoableOperation uedit = (IUndoableOperation)redostack.Pop();
				undostack.Push(uedit);
				uedit.Redo();
				OnActionRedone();
			}
		}
		public void Push(IUndoableOperation operation)
		{
			if (operation == null) {
				throw new ArgumentNullException("operation");
			}
			if (AcceptChanges) {
				StartUndoGroup();
				undostack.Push(operation);
				actionCountInUndoGroup++;
				if (TextEditorControl != null) {
					undostack.Push(new UndoableSetCaretPosition(this, TextEditorControl.ActiveTextAreaControl.Caret.Position));
					actionCountInUndoGroup++;
				}
				EndUndoGroup();
				ClearRedoStack();
			}
		}
		public void ClearRedoStack()
		{
			redostack.Clear();
		}
		public void ClearAll()
		{
			AssertNoUndoGroupOpen();
			undostack.Clear();
			redostack.Clear();
			actionCountInUndoGroup = 0;
		}
		protected void OnActionUndone()
		{
			if (ActionUndone != null) {
				ActionUndone(null, null);
			}
		}
		protected void OnActionRedone()
		{
			if (ActionRedone != null) {
				ActionRedone(null, null);
			}
		}
		class UndoableSetCaretPosition : IUndoableOperation
		{
			UndoStack stack;
			TextLocation pos;
			TextLocation redoPos;
			public UndoableSetCaretPosition(UndoStack stack, TextLocation pos)
			{
				this.stack = stack;
				this.pos = pos;
			}
			public void Undo()
			{
				redoPos = stack.TextEditorControl.ActiveTextAreaControl.Caret.Position;
				stack.TextEditorControl.ActiveTextAreaControl.Caret.Position = pos;
				stack.TextEditorControl.ActiveTextAreaControl.SelectionManager.ClearSelection();
			}
			public void Redo()
			{
				stack.TextEditorControl.ActiveTextAreaControl.Caret.Position = redoPos;
				stack.TextEditorControl.ActiveTextAreaControl.SelectionManager.ClearSelection();
			}
		}
	}
	public class OperationEventArgs : EventArgs
	{
		public OperationEventArgs(IUndoableOperation op)
		{
			this.op = op;
		}
		IUndoableOperation op;
		public IUndoableOperation Operation {
			get {
				return op;
			}
		}
	}
	public delegate void OperationEventHandler(object sender, OperationEventArgs e);
}
