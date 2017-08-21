/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Sheng.SailingEase.Controls.TreeViewGrid.NodeControls;
namespace Sheng.SailingEase.Controls.TreeViewGrid
{
	public struct DrawContext
	{
		private Graphics _graphics;
		public Graphics Graphics
		{
			get { return _graphics; }
			set { _graphics = value; }
		}
		private Rectangle _bounds;
		public Rectangle Bounds
		{
			get { return _bounds; }
			set { _bounds = value; }
		}
		private Font _font;
		public Font Font
		{
			get { return _font; }
			set { _font = value; }
		}
		private DrawSelectionMode _drawSelection;
		public DrawSelectionMode DrawSelection
		{
			get { return _drawSelection; }
			set { _drawSelection = value; }
		}
		private bool _drawFocus;
		public bool DrawFocus
		{
			get { return _drawFocus; }
			set { _drawFocus = value; }
		}
		private NodeControl _currentEditorOwner;
		public NodeControl CurrentEditorOwner
		{
			get { return _currentEditorOwner; }
			set { _currentEditorOwner = value; }
		}
		private bool _enabled;
		public bool Enabled
		{
			get { return _enabled; }
			set { _enabled = value; }
		}
	}
}
