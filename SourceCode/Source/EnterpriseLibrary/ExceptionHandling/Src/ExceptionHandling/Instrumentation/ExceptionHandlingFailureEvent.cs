/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation
{
    public class ExceptionHandlingFailureEvent : ExceptionHandlingEvent
    {
        string exceptionMessage;
		string instanceName;
        public ExceptionHandlingFailureEvent(string instanceName, string exceptionMessage)
        {
			this.instanceName = instanceName;
            this.exceptionMessage = exceptionMessage;
        }
        public string ExceptionMessage
        {
            get { return exceptionMessage; }
        }
		public string InstanceName
		{
			get { return instanceName; }
		}
	}
}
