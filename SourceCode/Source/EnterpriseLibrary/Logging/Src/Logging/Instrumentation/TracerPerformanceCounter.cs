/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using System.Diagnostics;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation
{
    public class TracerPerformanceCounter: EnterpriseLibraryPerformanceCounter
    {
        public TracerPerformanceCounter(string counterCategoryName, string counterName)
            : base(counterCategoryName, counterName, new string[0])
        {
        }
        public void Increment(string instanceName)
        {
            PerformanceCounter counter = InstantiateCounter(instanceName);
            counter.Increment();
        }
        public void IncrementBy(string instanceName, long value)
        {
            PerformanceCounter counter = InstantiateCounter(instanceName);
            counter.IncrementBy(value);
        }
    }
}
