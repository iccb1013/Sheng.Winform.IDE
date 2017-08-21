/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
namespace Sheng.SailingEase.Controls.TreeViewGrid
{
	public class TreeViewAdvEventArgs: EventArgs
	{
		private TreeNodeAdv _node;
		public TreeNodeAdv Node
		{
			get { return _node; }
		}
		public TreeViewAdvEventArgs(TreeNodeAdv node)
		{
			_node = node;
		}
	}
}
