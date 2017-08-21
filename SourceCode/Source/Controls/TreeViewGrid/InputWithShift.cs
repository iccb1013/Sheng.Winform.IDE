/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
namespace Sheng.SailingEase.Controls.TreeViewGrid
{
	internal class InputWithShift: NormalInputState
	{
		public InputWithShift(SETreeViewGrid tree): base(tree)
		{
		}
		protected override void FocusRow(TreeNodeAdv node)
		{
			Tree.SuspendSelectionEvent = true;
			try
			{
				if (Tree.SelectionMode == TreeSelectionMode.Single || Tree.SelectionStart == null)
					base.FocusRow(node);
				else if (CanSelect(node))
				{
					SelectAllFromStart(node);
					Tree.CurrentNode = node;
					Tree.ScrollTo(node);
				}
			}
			finally
			{
				Tree.SuspendSelectionEvent = false;
			}
		}
		protected override void DoMouseOperation(TreeNodeAdvMouseEventArgs args)
		{
			if (Tree.SelectionMode == TreeSelectionMode.Single || Tree.SelectionStart == null)
			{
				base.DoMouseOperation(args);
			}
			else if (CanSelect(args.Node))
			{
				Tree.SuspendSelectionEvent = true;
				try
				{
					SelectAllFromStart(args.Node);
				}
				finally
				{
					Tree.SuspendSelectionEvent = false;
				}
			}
		}
		protected override void MouseDownAtEmptySpace(TreeNodeAdvMouseEventArgs args)
		{
		}
		private void SelectAllFromStart(TreeNodeAdv node)
		{
			Tree.ClearSelection();
			int a = node.Row;
			int b = Tree.SelectionStart.Row;
			for (int i = Math.Min(a, b); i <= Math.Max(a, b); i++)
			{
				if (Tree.SelectionMode == TreeSelectionMode.Multi || Tree.RowMap[i].Parent == node.Parent)
					Tree.RowMap[i].IsSelected = true;
			}
		}
	}
}
