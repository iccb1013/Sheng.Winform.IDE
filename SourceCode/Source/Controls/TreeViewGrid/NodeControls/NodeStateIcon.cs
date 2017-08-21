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
using Sheng.SailingEase.Controls.Properties;
namespace Sheng.SailingEase.Controls.TreeViewGrid.NodeControls
{
	public class NodeStateIcon: NodeIcon
	{
		private Image _leaf;
		private Image _opened;
		private Image _closed;
		public NodeStateIcon()
		{
			_leaf = MakeTransparent(Resources.Leaf);
			_opened = MakeTransparent(Resources.Folder);
			_closed = MakeTransparent(Resources.FolderClosed);
		}
		private static Image MakeTransparent(Bitmap bitmap)
		{
			bitmap.MakeTransparent(bitmap.GetPixel(0,0));
			return bitmap;
		}
		protected override Image GetIcon(TreeNodeAdv node)
		{
			Image icon = base.GetIcon(node);
			if (icon != null)
				return icon;
			else if (node.IsLeaf)
				return _leaf;
			else if (node.CanExpand && node.IsExpanded)
				return _opened;
			else
				return _closed;
		}
	}
}
