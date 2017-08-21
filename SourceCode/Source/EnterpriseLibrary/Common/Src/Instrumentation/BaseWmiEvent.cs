/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Management.Instrumentation;
using System.Text;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
    [InstrumentationClass(InstrumentationType.Event)]
    public abstract class BaseWmiEvent
    {
        private DateTime utcTimeStamp = DateTime.UtcNow;
        public DateTime UtcTimeStamp
        {
            get { return utcTimeStamp; }
        }
    }
}
