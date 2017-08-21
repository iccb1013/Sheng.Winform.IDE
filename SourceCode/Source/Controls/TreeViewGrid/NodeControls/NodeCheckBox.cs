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
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.ComponentModel;
using Sheng.SailingEase.Controls.Properties;
namespace Sheng.SailingEase.Controls.TreeViewGrid.NodeControls
{
	public class NodeCheckBox : BindableControl
	{
		public const int ImageSize = 13;
		public const int Width = 13;
		private Bitmap _check;
		private Bitmap _uncheck;
		private Bitmap _unknown;
		private bool _threeState;
		[DefaultValue(false)]
		public bool ThreeState
		{
			get { return _threeState; }
			set { _threeState = value; }
		}
		private bool _editEnabled = true;
		[DefaultValue(true)]
		public bool EditEnabled
		{
			get { return _editEnabled; }
			set { _editEnabled = value; }
		}
		public NodeCheckBox()
			: this(string.Empty)
		{
		}
		public NodeCheckBox(string propertyName)
		{
            _check = Resources.Check;
			_uncheck = Resources.Uncheck;
			_unknown = Resources.Unknown;
			DataPropertyName = propertyName;
		}
		public override Size MeasureSize(TreeNodeAdv node)
		{
			return new Size(Width, Width);
		}
		public override void Draw(TreeNodeAdv node, DrawContext context)
		{
			Rectangle r = context.Bounds;
			int dy = (int)Math.Round((float)(r.Height - ImageSize) / 2);
			CheckState state = GetCheckState(node);
			if (Application.RenderWithVisualStyles)
			{
				VisualStyleRenderer renderer;
				if (state == CheckState.Indeterminate)
					renderer = new VisualStyleRenderer(VisualStyleElement.Button.CheckBox.MixedNormal);
				else if (state == CheckState.Checked)
					renderer = new VisualStyleRenderer(VisualStyleElement.Button.CheckBox.CheckedNormal);
				else
					renderer = new VisualStyleRenderer(VisualStyleElement.Button.CheckBox.UncheckedNormal);
				renderer.DrawBackground(context.Graphics, new Rectangle(r.X, r.Y + dy, ImageSize, ImageSize));
			}
			else
			{
				Image img;
				if (state == CheckState.Indeterminate)
					img = _unknown;
				else if (state == CheckState.Checked)
					img = _check;
				else
					img = _uncheck;
				context.Graphics.DrawImage(img, new Point(r.X, r.Y + dy));
			}
		}
		protected virtual CheckState GetCheckState(TreeNodeAdv node)
		{
			object obj = GetValue(node);
			if (obj is CheckState)
				return (CheckState)obj;
			else if (obj is bool)
				return (bool)obj ? CheckState.Checked : CheckState.Unchecked;
			else
				return CheckState.Unchecked;
		}
		protected virtual void SetCheckState(TreeNodeAdv node, CheckState value)
		{
			Type type = GetPropertyType(node);
			if (type == typeof(CheckState))
			{
				SetValue(node, value);
				OnCheckStateChanged(node);
			}
			else if (type == typeof(bool))
			{
				SetValue(node, value != CheckState.Unchecked);
				OnCheckStateChanged(node);
			}
		}
		public override void MouseDown(TreeNodeAdvMouseEventArgs args)
		{
			if (args.Button == MouseButtons.Left && EditEnabled)
			{
				CheckState state = GetCheckState(args.Node);
				state = GetNewState(state);
				SetCheckState(args.Node, state);
				args.Handled = true;
			}
		}
		public override void MouseDoubleClick(TreeNodeAdvMouseEventArgs args)
		{
			args.Handled = true;
		}
		private CheckState GetNewState(CheckState state)
		{
			if (state == CheckState.Indeterminate)
				return CheckState.Unchecked;
			else if(state == CheckState.Unchecked)
				return CheckState.Checked;
			else 
				return ThreeState ? CheckState.Indeterminate : CheckState.Unchecked;
		}
		public override void KeyDown(KeyEventArgs args)
		{
			if (args.KeyCode == Keys.Space && EditEnabled)
			{
				Parent.BeginUpdate();
				try
				{
					if (Parent.CurrentNode != null)
					{
						CheckState value = GetNewState(GetCheckState(Parent.CurrentNode));
						foreach (TreeNodeAdv node in Parent.Selection)
							SetCheckState(node, value);
					}
				}
				finally
				{
					Parent.EndUpdate();
				}
				args.Handled = true;
			}
		}
		public event EventHandler<TreePathEventArgs> CheckStateChanged;
		protected void OnCheckStateChanged(TreePathEventArgs args)
		{
			if (CheckStateChanged != null)
				CheckStateChanged(this, args);
		}
		protected void OnCheckStateChanged(TreeNodeAdv node)
		{
			TreePath path = this.Parent.GetPath(node);
			OnCheckStateChanged(new TreePathEventArgs(path));
		}
	}
}
