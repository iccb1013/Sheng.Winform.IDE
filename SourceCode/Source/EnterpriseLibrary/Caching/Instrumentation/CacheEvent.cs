/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
	public abstract class CacheEvent : BaseWmiEvent
	{
		private string instanceName;
		protected CacheEvent(string instanceName)
		{
			this.instanceName = instanceName;
		}
		public string InstanceName
		{
			get { return instanceName; }
		}
	}
}
