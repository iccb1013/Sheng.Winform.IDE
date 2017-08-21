/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
	public class CacheCallbackFailureEvent : CacheEvent
	{
		private string key;
		private string exceptionMessage;
       public CacheCallbackFailureEvent(string instanceName, string key, string exceptionMessage)
			: base(instanceName)
		{
			this.key = key;
			this.exceptionMessage = exceptionMessage;
		}
		public string Key
		{
			get { return key; }
		}
        public string ExceptionMessage
		{
			get { return exceptionMessage; }
		}
	}
}
