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
	public class TreePathEventArgs : EventArgs
	{
		private TreePath _path;
		public TreePath Path
		{
			get { return _path; }
		}
		public TreePathEventArgs()
		{
			_path = new TreePath();
		}
		public TreePathEventArgs(TreePath path)
		{
			if (path == null)
				throw new ArgumentNullException();
			_path = path;
		}
	}
}
