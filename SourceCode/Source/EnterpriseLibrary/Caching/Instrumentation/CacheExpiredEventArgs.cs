/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
	public class CacheExpiredEventArgs : EventArgs
	{
		private long itemsExpired;
		public CacheExpiredEventArgs(long itemsExpired)
		{
			this.itemsExpired = itemsExpired;
		}
		public long ItemsExpired
		{
			get { return itemsExpired; }
		}
	}
}
