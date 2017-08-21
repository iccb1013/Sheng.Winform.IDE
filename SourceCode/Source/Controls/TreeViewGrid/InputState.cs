/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Windows.Forms;
namespace Sheng.SailingEase.Controls.TreeViewGrid
{
	internal abstract class InputState
	{
		private SETreeViewGrid _tree;
		public SETreeViewGrid Tree
		{
			get { return _tree; }
		}
		public InputState(SETreeViewGrid tree)
		{
			_tree = tree;
		}
		public abstract void KeyDown(System.Windows.Forms.KeyEventArgs args);
		public abstract void MouseDown(TreeNodeAdvMouseEventArgs args);
		public abstract void MouseUp(TreeNodeAdvMouseEventArgs args);
		public virtual bool MouseMove(MouseEventArgs args)
		{
			return false;
		}
	}
}
