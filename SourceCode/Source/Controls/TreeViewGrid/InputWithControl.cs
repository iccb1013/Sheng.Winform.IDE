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
	internal class InputWithControl: NormalInputState
	{
		public InputWithControl(SETreeViewGrid tree): base(tree)
		{
		}
		protected override void DoMouseOperation(TreeNodeAdvMouseEventArgs args)
		{
			if (Tree.SelectionMode == TreeSelectionMode.Single)
			{
				base.DoMouseOperation(args);
			}
			else if (CanSelect(args.Node))
			{
				args.Node.IsSelected = !args.Node.IsSelected;
				Tree.SelectionStart = args.Node;
			}
		}
		protected override void MouseDownAtEmptySpace(TreeNodeAdvMouseEventArgs args)
		{
		}
	}
}
