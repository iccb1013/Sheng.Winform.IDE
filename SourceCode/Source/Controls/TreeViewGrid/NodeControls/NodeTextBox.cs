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
	public class NodeTextBox: BaseTextControl
	{
		private const int MinTextBoxWidth = 30;
		private TextBox _textBox;
		public NodeTextBox()
		{
		}
		protected override Size CalculateEditorSize(EditorContext context)
		{
			if (Parent.UseColumns)
				return context.Bounds.Size;
			else
			{
				TextBox textBox = context.Editor as TextBox;
				Size size = GetLabelSize(textBox.Text);
				int width = Math.Max(size.Width + Font.Height, MinTextBoxWidth); 
				return new Size(width, size.Height);
			}
		}
		public override void KeyDown(KeyEventArgs args)
		{
			if (args.KeyCode == Keys.F2 && Parent.CurrentNode != null)
			{
				args.Handled = true;
				BeginEdit();
			}
		}
		protected override Control CreateEditor(TreeNodeAdv node)
		{
			TextBox textBox = new TextBox();
			textBox.TextAlign = TextAlign;
			textBox.Text = GetLabel(node);
			textBox.BorderStyle = BorderStyle.FixedSingle;
			textBox.KeyDown += new KeyEventHandler(textBox_KeyDown);
			textBox.Disposed += new EventHandler(textBox_Disposed);
			textBox.TextChanged += new EventHandler(textBox_TextChanged);
			_label = textBox.Text;
			_textBox = textBox;
			return textBox;
		}
		private string _label;
		private void textBox_TextChanged(object sender, EventArgs e)
		{
			_label = _textBox.Text;
			Parent.UpdateEditorBounds();
		}
		private void textBox_Disposed(object sender, EventArgs e)
		{
			_textBox.KeyDown -= new KeyEventHandler(textBox_KeyDown);
			_textBox.Disposed -= new EventHandler(textBox_Disposed);
			_textBox.TextChanged -= new EventHandler(textBox_TextChanged);
			_textBox = null;
		}
		protected override void DoApplyChanges(TreeNodeAdv node)
		{
			string oldLabel = GetLabel(node);
			if (oldLabel != _label)
			{
				SetLabel(node, _label);
				OnLabelChanged();
			}
		}
		void textBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
				EndEdit(true);
			else if (e.KeyCode == Keys.Enter)
				EndEdit(false);
		}
		public void Cut()
		{
			if (_textBox != null)
				_textBox.Cut();
		}
		public void Copy()
		{
			if (_textBox != null)
				_textBox.Copy();
		}
		public void Paste()
		{
			if (_textBox != null)
				_textBox.Paste();
		}
		public void Delete()
		{
			if (_textBox != null)
			{
				int len = Math.Max(_textBox.SelectionLength, 1);
				if (_textBox.SelectionStart < _textBox.Text.Length)
				{
					int start = _textBox.SelectionStart;
					_textBox.Text = _textBox.Text.Remove(_textBox.SelectionStart, len);
					_textBox.SelectionStart = start;
				}
			}
		}
		public event EventHandler LabelChanged;
		protected void OnLabelChanged()
		{
			if (LabelChanged != null)
				LabelChanged(this, EventArgs.Empty);
		}
	}
}
