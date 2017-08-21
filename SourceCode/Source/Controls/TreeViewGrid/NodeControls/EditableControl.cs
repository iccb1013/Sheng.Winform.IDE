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
	public abstract class EditableControl: BindableControl
	{
		private Timer _timer;
		private bool _editFlag;
		private bool _discardChanges;
		private TreeNodeAdv _editNode;
		private bool _editEnabled = false;
		[DefaultValue(false)]
		public bool EditEnabled
		{
			get { return _editEnabled; }
			set { _editEnabled = value; }
		}
		protected EditableControl()
		{
			_timer = new Timer();
			_timer.Interval = 500;
			_timer.Tick += new EventHandler(TimerTick);
		}
		private void TimerTick(object sender, EventArgs e)
		{
			_timer.Stop();
			if (_editFlag)
				BeginEdit();
			_editFlag = false;
		}
		public void SetEditorBounds(EditorContext context)
		{
			Size size = CalculateEditorSize(context);
			context.Editor.Bounds = new Rectangle(context.Bounds.X, context.Bounds.Y,
				Math.Min(size.Width, context.Bounds.Width), context.Bounds.Height);
		}
		protected abstract Size CalculateEditorSize(EditorContext context);
		protected virtual bool CanEdit(TreeNodeAdv node)
		{
			return (node.Tag != null);
		}
		public void BeginEdit()
		{
			if (EditEnabled && Parent.CurrentNode != null && CanEdit(Parent.CurrentNode))
			{
				CancelEventArgs args = new CancelEventArgs();
				OnEditorShowing(args);
				if (!args.Cancel)
				{
					_discardChanges = false;
					Control control = CreateEditor(Parent.CurrentNode);
					_editNode = Parent.CurrentNode;
					control.Disposed += new EventHandler(EditorDisposed);
					Parent.DisplayEditor(control, this);
				}
			}
		}
		public void EndEdit(bool cancel)
		{
			_discardChanges = cancel;
			Parent.HideEditor();
		}
		public virtual void UpdateEditor(Control control)
		{
		}
		private void EditorDisposed(object sender, EventArgs e)
		{
			OnEditorHided();
			if (!_discardChanges && _editNode != null)
				ApplyChanges(_editNode);
			_editNode = null;
		}
		private void ApplyChanges(TreeNodeAdv node)
		{
			try
			{
				DoApplyChanges(node);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Value is not valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}
		protected abstract void DoApplyChanges(TreeNodeAdv node);
		protected abstract Control CreateEditor(TreeNodeAdv node);
		public override void MouseDown(TreeNodeAdvMouseEventArgs args)
		{
			_editFlag = (!Parent.UseColumns && args.Button == MouseButtons.Left
				&& args.ModifierKeys == Keys.None && args.Node.IsSelected);
		}
		public override void MouseUp(TreeNodeAdvMouseEventArgs args)
		{
			if (_editFlag && args.Node.IsSelected)
				_timer.Start();
		}
		public override void MouseDoubleClick(TreeNodeAdvMouseEventArgs args)
		{
			_timer.Stop();
			_editFlag = false;
			if (Parent.UseColumns)
			{
				args.Handled = true;
				BeginEdit();
			}
		}
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
				_timer.Dispose();
		}
		public event CancelEventHandler EditorShowing;
		protected void OnEditorShowing(CancelEventArgs args)
		{
			if (EditorShowing != null)
				EditorShowing(this, args);
		}
		public event EventHandler EditorHided;
		protected void OnEditorHided()
		{
			if (EditorHided != null)
				EditorHided(this, EventArgs.Empty);
		}
	}
}
