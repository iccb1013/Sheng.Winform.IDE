/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Drawing;
using Sheng.SailingEase.Controls.TextEditor.Document;
namespace Sheng.SailingEase.Controls.TextEditor.Actions
{
	public class ShiftCaretRight : CaretRight
	{
		public override void Execute(TextArea textArea)
		{
			TextLocation oldCaretPos  = textArea.Caret.Position;
			base.Execute(textArea);
			textArea.AutoClearSelection = false;
			textArea.SelectionManager.ExtendSelection(oldCaretPos, textArea.Caret.Position);
		}
	}
	public class ShiftCaretLeft : CaretLeft
	{
		public override void Execute(TextArea textArea)
		{
			TextLocation oldCaretPos  = textArea.Caret.Position;
			base.Execute(textArea);
			textArea.AutoClearSelection = false;
			textArea.SelectionManager.ExtendSelection(oldCaretPos, textArea.Caret.Position);
		}
	}
	public class ShiftCaretUp : CaretUp
	{
		public override void Execute(TextArea textArea)
		{
			TextLocation oldCaretPos  = textArea.Caret.Position;
			base.Execute(textArea);
			textArea.AutoClearSelection = false;
			textArea.SelectionManager.ExtendSelection(oldCaretPos, textArea.Caret.Position);
		}
	}
	public class ShiftCaretDown : CaretDown
	{
		public override void Execute(TextArea textArea)
		{
			TextLocation oldCaretPos  = textArea.Caret.Position;
			base.Execute(textArea);
			textArea.AutoClearSelection = false;
			textArea.SelectionManager.ExtendSelection(oldCaretPos, textArea.Caret.Position);
		}
	}
	public class ShiftWordRight : WordRight
	{
		public override void Execute(TextArea textArea)
		{
			TextLocation oldCaretPos  = textArea.Caret.Position;
			base.Execute(textArea);
			textArea.AutoClearSelection = false;
			textArea.SelectionManager.ExtendSelection(oldCaretPos, textArea.Caret.Position);
		}
	}
	public class ShiftWordLeft : WordLeft
	{
		public override void Execute(TextArea textArea)
		{
			TextLocation oldCaretPos  = textArea.Caret.Position;
			base.Execute(textArea);
			textArea.AutoClearSelection = false;
			textArea.SelectionManager.ExtendSelection(oldCaretPos, textArea.Caret.Position);
		}
	}
	public class ShiftHome : Home
	{
		public override void Execute(TextArea textArea)
		{
			TextLocation oldCaretPos  = textArea.Caret.Position;
			base.Execute(textArea);
			textArea.AutoClearSelection = false;
			textArea.SelectionManager.ExtendSelection(oldCaretPos, textArea.Caret.Position);
		}
	}
	public class ShiftEnd : End
	{
		public override void Execute(TextArea textArea)
		{
			TextLocation oldCaretPos  = textArea.Caret.Position;
			base.Execute(textArea);
			textArea.AutoClearSelection = false;
			textArea.SelectionManager.ExtendSelection(oldCaretPos, textArea.Caret.Position);
		}
	}
	public class ShiftMoveToStart : MoveToStart
	{
		public override void Execute(TextArea textArea)
		{
			TextLocation oldCaretPos  = textArea.Caret.Position;
			base.Execute(textArea);
			textArea.AutoClearSelection = false;
			textArea.SelectionManager.ExtendSelection(oldCaretPos, textArea.Caret.Position);
		}
	}
	public class ShiftMoveToEnd : MoveToEnd
	{
		public override void Execute(TextArea textArea)
		{
			TextLocation oldCaretPos  = textArea.Caret.Position;
			base.Execute(textArea);
			textArea.AutoClearSelection = false;
			textArea.SelectionManager.ExtendSelection(oldCaretPos, textArea.Caret.Position);
		}
	}
	public class ShiftMovePageUp : MovePageUp
	{
		public override void Execute(TextArea textArea)
		{
			TextLocation oldCaretPos  = textArea.Caret.Position;
			base.Execute(textArea);
			textArea.AutoClearSelection = false;
			textArea.SelectionManager.ExtendSelection(oldCaretPos, textArea.Caret.Position);
		}
	}
	public class ShiftMovePageDown : MovePageDown
	{
		public override void Execute(TextArea textArea)
		{
			TextLocation oldCaretPos  = textArea.Caret.Position;
			base.Execute(textArea);
			textArea.AutoClearSelection = false;
			textArea.SelectionManager.ExtendSelection(oldCaretPos, textArea.Caret.Position);
		}
	}
	public class SelectWholeDocument : AbstractEditAction
	{
		public override void Execute(TextArea textArea)
		{
			textArea.AutoClearSelection = false;
			TextLocation startPoint = new TextLocation(0, 0);
			TextLocation endPoint   = textArea.Document.OffsetToPosition(textArea.Document.TextLength);
			if (textArea.SelectionManager.HasSomethingSelected) {
				if (textArea.SelectionManager.SelectionCollection[0].StartPosition == startPoint &&
				    textArea.SelectionManager.SelectionCollection[0].EndPosition   == endPoint) {
					return;
				}
			}
			textArea.Caret.Position = textArea.SelectionManager.NextValidPosition(endPoint.Y);
			textArea.SelectionManager.ExtendSelection(startPoint, endPoint);
			textArea.SetDesiredColumn();
		}
	}
	public class ClearAllSelections : AbstractEditAction
	{
		public override void Execute(TextArea textArea)
		{
			textArea.SelectionManager.ClearSelection();
		}
	}
}
