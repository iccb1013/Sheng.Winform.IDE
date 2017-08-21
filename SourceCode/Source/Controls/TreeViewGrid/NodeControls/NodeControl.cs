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
using System.ComponentModel;
namespace Sheng.SailingEase.Controls.TreeViewGrid.NodeControls
{
	[DesignTimeVisible(false), ToolboxItem(false)]
	public abstract class NodeControl: Component
	{
		private SETreeViewGrid _parent;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public SETreeViewGrid Parent
		{
			get { return _parent; }
			set 
			{
				if (value != _parent)
				{
					if (_parent != null)
						_parent.NodeControls.Remove(this);
					if (value != null)
						value.NodeControls.Add(this);
				}
			}
		}
		private IToolTipProvider _toolTipProvider;
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IToolTipProvider ToolTipProvider
		{
			get { return _toolTipProvider; }
			set { _toolTipProvider = value; }
		}
		private int _column;
		[DefaultValue(0)]
		public int Column
		{
			get { return _column; }
			set 
			{
				if (_column < 0)
					throw new ArgumentOutOfRangeException("value");
				_column = value;
				if (_parent != null)
					_parent.FullUpdate();
			}
		}
        private bool _drawActive = true;
        [DefaultValue(true)]
        public bool DrawActive
        {
            get { return _drawActive; }
            set { _drawActive = value; }
        }
		internal void AssignParent(SETreeViewGrid parent)
		{
			_parent = parent;
		}
		public abstract Size MeasureSize(TreeNodeAdv node);
		public abstract void Draw(TreeNodeAdv node, DrawContext context);
		public virtual string GetToolTip(TreeNodeAdv node)
		{
			if (ToolTipProvider != null)
				return ToolTipProvider.GetToolTip(node);
			else
				return string.Empty;
		}
		public virtual void MouseDown(TreeNodeAdvMouseEventArgs args)
		{
		}
		public virtual void MouseUp(TreeNodeAdvMouseEventArgs args)
		{
		}
		public virtual void MouseDoubleClick(TreeNodeAdvMouseEventArgs args)
		{
		}
		public virtual void KeyDown(KeyEventArgs args)
		{
		}
		public virtual void KeyUp(KeyEventArgs args)
		{
		}
	}
}
