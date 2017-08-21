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
	public class TreeViewAdvCancelEventArgs : TreeViewAdvEventArgs
	{
		private bool _cancel;
		public bool Cancel
		{
			get { return _cancel; }
			set { _cancel = value; }
		}
		public TreeViewAdvCancelEventArgs(TreeNodeAdv node)
			: base(node)
		{
		}
	}
}
