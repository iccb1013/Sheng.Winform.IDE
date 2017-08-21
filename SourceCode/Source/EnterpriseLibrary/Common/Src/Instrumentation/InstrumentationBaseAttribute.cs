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
    public class InstrumentationBaseAttribute : Attribute
    {
        string subjectName;
        protected InstrumentationBaseAttribute(string subjectName)
        {
            if (String.IsNullOrEmpty(subjectName)) throw new ArgumentException("subjectName");
            this.subjectName = subjectName;
        }
        public string SubjectName { get { return subjectName; } }
    }
}
