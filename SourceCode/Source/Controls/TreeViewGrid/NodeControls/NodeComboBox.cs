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
using System.Windows.Forms;
using System.Reflection;
using System.ComponentModel;
namespace Sheng.SailingEase.Controls.TreeViewGrid.NodeControls
{
	public class NodeComboBox : BaseTextControl
	{
		private object _selectedItem;
		private int _editorWidth = 100;
		[DefaultValue(100)]
		public int EditorWidth
		{
			get { return _editorWidth; }
			set { _editorWidth = value; }
		}
		private object[]_dropDownItems;
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object[] DropDownItems
		{
			get { return _dropDownItems; }
			set { _dropDownItems = value; }
		}
		public NodeComboBox()
		{
		}
		protected override Size CalculateEditorSize(EditorContext context)
		{
			if (Parent.UseColumns)
				return context.Bounds.Size;
			else
				return new Size(EditorWidth, context.Bounds.Height);
		}
		protected override Control CreateEditor(TreeNodeAdv node)
		{
			ComboBox comboBox = new ComboBox();
			if (DropDownItems != null)
				comboBox.Items.AddRange(DropDownItems);
			_selectedItem = GetValue(node);
			comboBox.SelectedItem = _selectedItem;
			comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			comboBox.KeyDown += new KeyEventHandler(EditorKeyDown);
			comboBox.SelectedIndexChanged += new EventHandler(EditorSelectedIndexChanged);
			comboBox.Disposed += new EventHandler(EditorDisposed);
			return comboBox;
		}
		void EditorDisposed(object sender, EventArgs e)
		{
			ComboBox comboBox = sender as ComboBox;
			comboBox.KeyDown -= new KeyEventHandler(EditorKeyDown);
			comboBox.SelectedIndexChanged -= new EventHandler(EditorSelectedIndexChanged);
			comboBox.Disposed -= new EventHandler(EditorDisposed);
		}
		void EditorSelectedIndexChanged(object sender, EventArgs e)
		{
			_selectedItem = (sender as ComboBox).SelectedItem;
			Parent.HideEditor();
		}
		public override void UpdateEditor(Control control)
		{
			(control as ComboBox).DroppedDown = true;
		}
		protected override void DoApplyChanges(TreeNodeAdv node)
		{
			SetValue(node, _selectedItem);
		}
		void EditorKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
				EndEdit(true);
		}
	}
}
