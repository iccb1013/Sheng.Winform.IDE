/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
	public class CacheUpdatedEventArgs : EventArgs
	{
		private long updatedEntriesCount;
		private long totalEntriesCount;
		public CacheUpdatedEventArgs(long updatedEntriesCount, long totalEntriesCount)
		{
			this.updatedEntriesCount = updatedEntriesCount;
			this.totalEntriesCount = totalEntriesCount;
		}
		public long UpdatedEntriesCount
		{
			get { return updatedEntriesCount; }
		}
		public long TotalEntriesCount
		{
			get { return totalEntriesCount; }
		}
	}
}
