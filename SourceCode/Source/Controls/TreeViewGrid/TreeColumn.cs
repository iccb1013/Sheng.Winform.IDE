/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
namespace Sheng.SailingEase.Controls.TreeViewGrid
{
	public class TreeColumn: IDisposable
	{
		private StringFormat _headerFormat;
		private SETreeViewGrid _treeView;
		internal SETreeViewGrid TreeView
		{
			get { return _treeView; }
			set { _treeView = value; }
		}
		private int _index;
		[Browsable(false)]
		public int Index
		{
			get { return _index; }
			internal set { _index = value; }
		}
		private string _header;
		[Localizable(true)]
		public string Header
		{
			get { return _header; }
			set 
			{ 
				_header = value;
				if (TreeView != null)
					TreeView.UpdateHeaders();
			}
		}
		private int _width;
		[DefaultValue(50), Localizable(true)]
		public int Width
		{
			get { return _width; }
			set 
			{
				if (_width != value)
				{
					if (value < 0)
						throw new ArgumentOutOfRangeException("value");
					_width = value;
					if (TreeView != null)
						TreeView.ChangeColumnWidth(this);
				}
			}
		}
		private bool _visible = true;
		[DefaultValue(true)]
		public bool IsVisible
		{
			get { return _visible; }
			set 
			{ 
				_visible = value;
				if (TreeView != null)
					TreeView.FullUpdate();
			}
		}
		private HorizontalAlignment _textAlign = HorizontalAlignment.Left;
		[DefaultValue(HorizontalAlignment.Left)]
		public HorizontalAlignment TextAlign
		{
			get { return _textAlign; }
			set { _textAlign = value; }
		}
		public TreeColumn(): 
			this(string.Empty, 50)
		{
		}
		public TreeColumn(string header, int width)
		{
			_header = header;
			_width = width;
			_headerFormat = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.MeasureTrailingSpaces);
			_headerFormat.LineAlignment = StringAlignment.Center;
			_headerFormat.Trimming = StringTrimming.EllipsisCharacter;
		}
		public override string ToString()
		{
			if (string.IsNullOrEmpty(Header))
				return GetType().Name;
			else
				return Header;
		}
		public void Draw(Graphics gr, Rectangle bounds, Font font)
		{
			_headerFormat.Alignment = TextHelper.TranslateAligment(TextAlign);
			gr.DrawString(Header + " ", font, SystemBrushes.WindowText, bounds, _headerFormat);
		}
		public void Dispose()
		{
			Dispose(true);
		}
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
				_headerFormat.Dispose();
		}
	}
}
