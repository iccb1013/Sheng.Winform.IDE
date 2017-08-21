/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class InstrumentationConsumerAttribute : InstrumentationBaseAttribute
    {
        public InstrumentationConsumerAttribute(string subjectName)
        : base(subjectName)
        {
        }
    }
}
