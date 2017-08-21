/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation
{
    public class LoggingConfigurationFailureEvent : LoggingEvent
    {
        private string exceptionMessage;
		public LoggingConfigurationFailureEvent(string exceptionMessage)
        {
            this.exceptionMessage = exceptionMessage;
        }
        public string ExceptionMessage
        {
            get { return this.exceptionMessage; }
        }
    }
}
