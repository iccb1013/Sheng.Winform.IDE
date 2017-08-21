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
	public class TreeColumnEventArgs: EventArgs
	{
		private TreeColumn _column;
		public TreeColumn Column
		{
			get { return _column; }
		}
		public TreeColumnEventArgs(TreeColumn column)
		{
			_column = column;
		}
	}
}
