/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation
{
	public class DataConfigurationFailureEvent : DataEvent
	{
		private string exceptionMessage;
		public DataConfigurationFailureEvent(string instanceName, string exceptionMessage)
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
