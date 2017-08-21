/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace Sheng.SailingEase.Controls.TextEditor.Undo
{
	internal sealed class UndoQueue : IUndoableOperation
	{
		List<IUndoableOperation> undolist = new List<IUndoableOperation>();
		public UndoQueue(Stack<IUndoableOperation> stack, int numops)
		{
			if (stack == null)  {
				throw new ArgumentNullException("stack");
			}
			Debug.Assert(numops > 0 , "Sheng.SailingEase.Controls.TextEditor.Undo.UndoQueue : numops should be > 0");
			if (numops > stack.Count) {
				numops = stack.Count;
			}
			for (int i = 0; i < numops; ++i) {
				undolist.Add(stack.Pop());
			}
		}
		public void Undo()
		{
			for (int i = 0; i < undolist.Count; ++i) {
				undolist[i].Undo();
			}
		}
		public void Redo()
		{
			for (int i = undolist.Count - 1 ; i >= 0 ; --i) {
				undolist[i].Redo();
			}
		}
	}
}
