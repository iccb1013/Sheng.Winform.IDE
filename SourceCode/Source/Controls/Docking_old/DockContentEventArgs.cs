/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Sheng.SIMBE.SEControl.Docking
{
	public class DockContentEventArgs : EventArgs
	{
		private IDockContent m_content;
		public DockContentEventArgs(IDockContent content)
		{
			m_content = content;
		}
		public IDockContent Content
		{
			get	{	return m_content;	}
		}
	}
}
