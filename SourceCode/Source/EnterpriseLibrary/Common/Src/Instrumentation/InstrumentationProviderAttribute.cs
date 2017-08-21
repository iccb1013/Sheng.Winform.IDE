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
    [AttributeUsage(AttributeTargets.Event, Inherited = false, AllowMultiple = false)]
    public sealed class InstrumentationProviderAttribute : InstrumentationBaseAttribute
    {
        public InstrumentationProviderAttribute(string subjectName)
        : base(subjectName)
        {
        }
    }
}
