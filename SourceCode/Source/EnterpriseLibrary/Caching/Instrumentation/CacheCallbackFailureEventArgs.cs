/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
	public class CacheCallbackFailureEventArgs : EventArgs
	{
		private string key;
		private Exception exception;
        public CacheCallbackFailureEventArgs(string key, Exception exception)
		{
			this.key = key;
			this.exception = exception;
		}
		public string Key
		{
			get { return key; }
		}
		public Exception Exception
		{
			get { return exception; }
		}
	}
}
