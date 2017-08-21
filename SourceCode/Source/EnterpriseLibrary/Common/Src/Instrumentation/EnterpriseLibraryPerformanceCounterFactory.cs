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
	public class EnterpriseLibraryPerformanceCounterFactory
	{
		Dictionary<string, PerformanceCounter> counterCache = new Dictionary<string, PerformanceCounter>();
		object lockObject = new object();
		public EnterpriseLibraryPerformanceCounter CreateCounter(string categoryName, string counterName, string[] instanceNames)
		{
			string combinedCounterNameRoot = categoryName.ToLowerInvariant() + counterName.ToLowerInvariant();
			PerformanceCounter[] counters = new PerformanceCounter[instanceNames.Length];
			for (int i = 0; i < instanceNames.Length; i++)
			{
				string combinedCounterName = combinedCounterNameRoot + instanceNames[i].ToLowerInvariant();
				lock (lockObject)
				{
					if (counterCache.ContainsKey(combinedCounterName) == false)
					{
						PerformanceCounter newCounter = new PerformanceCounter(categoryName, counterName, instanceNames[i], false);
						counterCache.Add(combinedCounterName, newCounter);
					}
					counters[i] = counterCache[combinedCounterName]; 
				}
			}
			return new EnterpriseLibraryPerformanceCounter(counters);
		}
		public void ClearCachedCounters()
		{
			counterCache.Clear();
		}
	}
}
