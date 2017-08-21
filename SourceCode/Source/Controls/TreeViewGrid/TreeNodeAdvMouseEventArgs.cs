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
using System.Drawing;
using Sheng.SailingEase.Controls.TreeViewGrid.NodeControls;
namespace Sheng.SailingEase.Controls.TreeViewGrid
{
	public class TreeNodeAdvMouseEventArgs : MouseEventArgs
	{
		private TreeNodeAdv _node;
		public TreeNodeAdv Node
		{
			get { return _node; }
			internal set { _node = value; }
		}
		private NodeControl _control;
		public NodeControl Control
		{
			get { return _control; }
			internal set { _control = value; }
		}
		private Point _viewLocation;
		public Point ViewLocation
		{
			get { return _viewLocation; }
			internal set { _viewLocation = value; }
		}
		private Point _absoluteLocation;
		public Point AbsoluteLocation
		{
			get { return _absoluteLocation; }
			internal set { _absoluteLocation = value; }
		}
		private Keys _modifierKeys;
		public Keys ModifierKeys
		{
			get { return _modifierKeys; }
			internal set { _modifierKeys = value; }
		}
		private bool _handled;
		public bool Handled
		{
			get { return _handled; }
			internal set { _handled = value; }
		}
		private Rectangle _controlBounds;
		public Rectangle ControlBounds
		{
			get { return _controlBounds; }
			internal set { _controlBounds = value; }
		}
		public TreeNodeAdvMouseEventArgs(MouseEventArgs args)
			: base(args.Button, args.Clicks, args.X, args.Y, args.Delta)
		{
		}
	}
}
