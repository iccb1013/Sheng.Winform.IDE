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
    public class LoggingFailureLoggingErrorEvent : LoggingEvent
    {
		private string errorMessage;
        private string exceptionMessage;
        public LoggingFailureLoggingErrorEvent(string errorMessage, string exceptionMessage)
        {
			this.errorMessage = errorMessage;
            this.exceptionMessage = exceptionMessage;
        }
		public string ErrorMessage
		{
			get { return errorMessage; }
		}
        public string ExceptionMessage
        {
            get { return exceptionMessage; }
        }
    }
}
