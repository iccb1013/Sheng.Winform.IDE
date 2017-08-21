/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Drawing;
namespace Sheng.SailingEase.Controls.TreeViewGrid
{
	internal class ResizeColumnState: InputState
	{
		private const int MinColumnWidth = 10;
		private Point _initLocation;
		private TreeColumn _column;
		private int _initWidth;
		public ResizeColumnState(SETreeViewGrid tree, TreeColumn column, Point p)
			: base(tree)
		{
			_column = column;
			_initLocation = p;
			_initWidth = column.Width;
		}
		public override void KeyDown(KeyEventArgs args)
		{
			args.Handled = true;
			if (args.KeyCode == Keys.Escape)
				FinishResize();
		}
		public override void MouseDown(TreeNodeAdvMouseEventArgs args)
		{
		}
		public override void MouseUp(TreeNodeAdvMouseEventArgs args)
		{
			FinishResize();
		}
		private void FinishResize()
		{
			Tree.ChangeInput();
			Tree.FullUpdate();
			Tree.OnColumnWidthChanged(_column);
		}
		public override bool MouseMove(MouseEventArgs args)
		{
			int w = _initWidth + args.Location.X - _initLocation.X;
			_column.Width = Math.Max(MinColumnWidth, w);
			Tree.UpdateView();
			return true;
		}
	}
}
