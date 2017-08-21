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
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class PerformanceCounterAttribute : Attribute
    {
        private string counterName;
        private string counterHelp;
        private PerformanceCounterType counterType;
		private string baseCounterName;
		private string baseCounterHelp;
		private PerformanceCounterType baseCounterType;
        public PerformanceCounterAttribute(string counterName, string counterHelp, PerformanceCounterType counterType)
        {
            this.counterName = counterName;
            this.counterHelp = counterHelp;
            this.counterType = counterType;
        }
		public PerformanceCounterType CounterType
		{
			get { return counterType; }
		}
		public string CounterHelp
		{
			get { return counterHelp; }
		}
		public string CounterName
		{
			get { return counterName; }
		}
		public PerformanceCounterType BaseCounterType
		{
			get { return baseCounterType; }
			set { baseCounterType = value; }
		}
		public string BaseCounterHelp
		{
			get { return baseCounterHelp; }
			set { baseCounterHelp = value; }
		}
		public string BaseCounterName
		{
			get { return baseCounterName; }
			set { baseCounterName = value; }
		}
		public bool HasBaseCounter() { return baseCounterName != null; }
	}
}
