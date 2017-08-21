/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public sealed class PerformanceCountersDefinitionAttribute : Attribute
    {
        PerformanceCounterCategoryType categoryType;
        string categoryName;
        string categoryHelp;
        public PerformanceCounterCategoryType CategoryType
        {
            get { return categoryType; }
        }
        public string CategoryName
        {
            get { return categoryName; }
        }
        public string CategoryHelp
        {
            get { return categoryHelp; }
        }
        public PerformanceCountersDefinitionAttribute(string categoryName, string categoryHelp)
            : this(categoryName, categoryHelp, PerformanceCounterCategoryType.MultiInstance)
        {
        }
        public PerformanceCountersDefinitionAttribute(string categoryName, string categoryHelp, PerformanceCounterCategoryType categoryType)
        {
            this.categoryName = categoryName;
            this.categoryHelp = categoryHelp;
            this.categoryType = categoryType;
        }
    }
}
