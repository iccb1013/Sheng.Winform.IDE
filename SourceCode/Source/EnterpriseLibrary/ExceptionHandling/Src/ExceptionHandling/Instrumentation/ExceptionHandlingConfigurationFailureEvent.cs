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
    public class ExceptionHandlingConfigurationFailureEvent : ExceptionHandlingEvent
    {
        private string policyName;
        private string exceptionMessage;
        public ExceptionHandlingConfigurationFailureEvent(string policyName, string exceptionMessage)
        {
            this.policyName = policyName;
            this.exceptionMessage = exceptionMessage;
        }
        public string PolicyName
        {
            get { return policyName; }
        }
        public string ExceptionMessage
        {
            get { return exceptionMessage; }
        }
    }
}
