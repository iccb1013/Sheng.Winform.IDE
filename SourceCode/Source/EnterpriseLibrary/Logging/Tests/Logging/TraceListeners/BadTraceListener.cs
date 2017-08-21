/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners
{
    public class BadTraceListener : CustomTraceListener
    {
        private Exception exceptionToThrow;
        public BadTraceListener(Exception exceptionToThrow)
        {
            this.exceptionToThrow = exceptionToThrow;
        }
        public override void Write(string message)
        {
            throw exceptionToThrow;
        }
        public override void WriteLine(string message)
        {
            throw exceptionToThrow;
        }
    }
}
