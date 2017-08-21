/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
	public class CacheConfigurationFailureEvent : CacheEvent
	{
		private string exceptionMessage;
		public CacheConfigurationFailureEvent(string instanceName, string exceptionMessage)
			: base(instanceName)
		{
			this.exceptionMessage = exceptionMessage;
		}
		public string ExceptionMessage
		{
			get { return exceptionMessage; }
		}
	}
}
