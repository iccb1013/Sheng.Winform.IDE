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
	public struct EditorContext
	{
		private TreeNodeAdv _currentNode;
		public TreeNodeAdv CurrentNode
		{
			get { return _currentNode; }
			set { _currentNode = value; }
		}
		private Control _editor;
		public Control Editor
		{
			get { return _editor; }
			set { _editor = value; }
		}
		private NodeControl _owner;
		public NodeControl Owner
		{
			get { return _owner; }
			set { _owner = value; }
		}
		private Rectangle _bounds;
		public Rectangle Bounds
		{
			get { return _bounds; }
			set { _bounds = value; }
		}
	}
}
