/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Text;
using System.Collections.ObjectModel;
namespace Sheng.SailingEase.Controls.TreeViewGrid
{
	public class TreePath
	{
		public static readonly TreePath Empty = new TreePath();
		private object[] _path;
		public object[] FullPath
		{
			get { return _path; }
		}
		public object LastNode
		{
			get
			{
				if (_path.Length > 0)
					return _path[_path.Length - 1];
				else
					return null;
			}
		}
		public object FirstNode
		{
			get
			{
				if (_path.Length > 0)
					return _path[0];
				else
					return null;
			}
		}
		public TreePath()
		{
			_path = new object[0];
		}
		public TreePath(object node)
		{
			_path = new object[] { node };
		}
		public TreePath(object[] path)
		{
			_path = path;
		}
		public bool IsEmpty()
		{
			return (_path.Length == 0);
		}
	}
}
