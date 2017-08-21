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
namespace Sheng.SailingEase.Controls.TreeViewGrid.NodeControls
{
	public class NodeIcon : BindableControl
	{
		public override Size MeasureSize(TreeNodeAdv node)
		{
			Image image = GetIcon(node);
			if (image != null)
				return image.Size;
			else
				return Size.Empty;
		}
		public override void Draw(TreeNodeAdv node, DrawContext context)
		{
			Image image = GetIcon(node);
			if (image != null)
			{
				Point point = new Point(context.Bounds.X,
					context.Bounds.Y + (context.Bounds.Height - image.Height) / 2);
				context.Graphics.DrawImage(image, point);
			}
		}
		protected virtual Image GetIcon(TreeNodeAdv node)
		{
			return GetValue(node) as Image;
		}
	}
}
