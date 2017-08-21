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
using System.Windows.Forms.VisualStyles;
using Sheng.SailingEase.Controls.Properties;
namespace Sheng.SailingEase.Controls.TreeViewGrid.NodeControls
{
	internal class NodePlusMinus : NodeControl
	{
		public const int ImageSize = 9;
		public const int Width = 16;
		private Bitmap _plus;
		private Bitmap _minus;
		public NodePlusMinus()
		{
			_plus = Resources.Plus2;
			_minus = Resources.Minus2;
		}
		public override Size MeasureSize(TreeNodeAdv node)
		{
			return new Size(Width, Width);
		}
		public override void Draw(TreeNodeAdv node, DrawContext context)
		{
			if (node.CanExpand)
			{
				Rectangle r = context.Bounds;
				int dy = (int)Math.Round((float)(r.Height - ImageSize) / 2);
				if (Application.RenderWithVisualStyles)
				{
					VisualStyleRenderer renderer;
					if (node.IsExpanded)
						renderer = new VisualStyleRenderer(VisualStyleElement.TreeView.Glyph.Opened);
					else
						renderer = new VisualStyleRenderer(VisualStyleElement.TreeView.Glyph.Closed);
					renderer.DrawBackground(context.Graphics, new Rectangle(r.X, r.Y + dy, ImageSize, ImageSize));
				}
				else
				{
					Image img;
					if (node.IsExpanded)
						img = _minus;
					else
						img = _plus;
					context.Graphics.DrawImageUnscaled(img, new Point(r.X, r.Y + dy));
				}
			}
		}
		public override void MouseDown(TreeNodeAdvMouseEventArgs args)
		{
			if (args.Button == MouseButtons.Left)
			{
				args.Handled = true;
				if (args.Node.CanExpand)
					args.Node.IsExpanded = !args.Node.IsExpanded;
			}
		}
		public override void MouseDoubleClick(TreeNodeAdvMouseEventArgs args)
		{
			args.Handled = true; 
		}
	}
}
