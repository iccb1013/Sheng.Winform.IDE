/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
    public class CacheScavengedEvent : CacheEvent
	{
		private long itemsScavenged;
		public CacheScavengedEvent(string instanceName, long itemsScavenged)
			: base(instanceName)
		{
			this.itemsScavenged = itemsScavenged;
		}
		public long ItemsScavenged
		{
			get { return itemsScavenged; }
		}
	}
}
