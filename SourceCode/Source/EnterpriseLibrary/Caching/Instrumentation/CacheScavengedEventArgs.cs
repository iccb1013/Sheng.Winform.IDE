/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
	public class CacheScavengedEventArgs : EventArgs
	{
		private long itemsScavenged;
		public CacheScavengedEventArgs(long itemsScavenged)
		{
			this.itemsScavenged = itemsScavenged;
		}
		public long ItemsScavenged
		{
			get { return itemsScavenged; }
		}
	}
}
