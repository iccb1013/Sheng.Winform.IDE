/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
    public class EnterpriseLibraryPerformanceCounter
    {
    	private PerformanceCounter [] counters;
        private string[] instanceNames;
        private string counterName;
        private string counterCategoryName;
    	public PerformanceCounter [] Counters { get { return counters; }}
        public EnterpriseLibraryPerformanceCounter(string counterCategoryName, string counterName)
            : this(counterCategoryName, counterName, new string[] { "Total" })
        {
        }
        public EnterpriseLibraryPerformanceCounter(string counterCategoryName, string counterName, params string[] instanceNames)
        {
            this.instanceNames = instanceNames;
            this.counterName = counterName;
            this.counterCategoryName = counterCategoryName;
        	counters = new PerformanceCounter[instanceNames.Length];
        	for(int i = 0; i < counters.Length; i++)
        	{
        		counters[i] = InstantiateCounter(instanceNames[i]);
        	}
        }
    	public EnterpriseLibraryPerformanceCounter(params PerformanceCounter [] counters)
    	{
    		this.counters = counters;	
    	}
        public void Clear()
        {
            foreach (PerformanceCounter counter in counters)
            {
                counter.RawValue = 0;
            }
        }
        public long Value { get { return counters[0].RawValue; } }
        public void Increment()
        {
            foreach (PerformanceCounter counter in counters)
            {
                counter.Increment();
            }
        }
		public void IncrementBy(long value)
		{
            foreach (PerformanceCounter counter in counters)
            {
                counter.IncrementBy(value);
            }
        }
		public long GetValueFor(string instanceName)
		{
		    foreach(PerformanceCounter counter in counters)
		    {
		    	if(counter.InstanceName.Equals(instanceName)) return counter.RawValue;
		    }
			return -1;
		}
		public void SetValueFor(string instanceName, long value)
		{
			foreach (PerformanceCounter counter in counters)
			{
				if (counter.InstanceName.Equals(instanceName))
				{
					counter.RawValue = value;
					break;
				}
			}
		}
        protected PerformanceCounter InstantiateCounter(string instanceName)
        {
            return new PerformanceCounter(counterCategoryName, counterName, instanceName, false);
        }
    }
}
