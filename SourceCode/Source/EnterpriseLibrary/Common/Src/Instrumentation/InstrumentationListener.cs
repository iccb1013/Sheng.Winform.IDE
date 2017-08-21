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
using ManagementInstrumentation = System.Management.Instrumentation.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
	public abstract class InstrumentationListener
	{
		private const string DefaultCounterName = "Total";
		IPerformanceCounterNameFormatter nameFormatter;
		bool performanceCountersEnabled;
		bool eventLoggingEnabled;
		bool wmiEnabled;
		public bool EventLoggingEnabled
		{
			get { return eventLoggingEnabled; }
			protected set { eventLoggingEnabled = value; }
		}
		public bool PerformanceCountersEnabled
		{
			get { return performanceCountersEnabled; }
			protected set { performanceCountersEnabled = value; }
		}
		public bool WmiEnabled
		{
			get { return wmiEnabled; }
			protected set { wmiEnabled = value; }
		}
		protected InstrumentationListener(bool performanceCountersEnabled,
									   bool eventLoggingEnabled,
									   bool wmiEnabled,
									   IPerformanceCounterNameFormatter nameFormatter)
		{
			string[] instanceNames = new string[] { DefaultCounterName };
			Initialize(performanceCountersEnabled, eventLoggingEnabled, wmiEnabled, nameFormatter, instanceNames);
		}
		protected InstrumentationListener(string instanceName,
									   bool performanceCountersEnabled,
									   bool eventLoggingEnabled,
									   bool wmiEnabled,
									   IPerformanceCounterNameFormatter nameFormatter)
			: this(CreateDefaultInstanceNames(instanceName), performanceCountersEnabled, eventLoggingEnabled, wmiEnabled, nameFormatter)
		{
		}
		protected InstrumentationListener(string[] instanceNames,
									   bool performanceCountersEnabled,
									   bool eventLoggingEnabled,
									   bool wmiEnabled,
									   IPerformanceCounterNameFormatter nameFormatter)
		{
			Initialize(performanceCountersEnabled, eventLoggingEnabled, wmiEnabled, nameFormatter, instanceNames);
		}
		protected string GetEventSourceName()
		{
			Type ourType = this.GetType();
			object[] attributes = ourType.GetCustomAttributes(typeof(EventLogDefinitionAttribute), false);
			return ((EventLogDefinitionAttribute)attributes[0]).SourceName;
		}
		private static string[] CreateDefaultInstanceNames(string instanceName)
		{
			return new string[] { DefaultCounterName, instanceName };
		}
		private void Initialize(bool performanceCountersEnabled, bool eventLoggingEnabled, bool wmiEnabled, IPerformanceCounterNameFormatter nameFormatter, string[] instanceNames)
		{
			this.performanceCountersEnabled = performanceCountersEnabled;
			this.eventLoggingEnabled = eventLoggingEnabled;
			this.wmiEnabled = wmiEnabled;
			this.nameFormatter = nameFormatter;
			if (performanceCountersEnabled)
			{
				FormatCounterInstanceNames(nameFormatter, instanceNames);
				CreatePerformanceCounters(instanceNames);
			}
		}
		private void FormatCounterInstanceNames(IPerformanceCounterNameFormatter nameFormatter, string[] instanceNames)
		{
			for (int i = 0; i < instanceNames.Length; i++)
			{
				instanceNames[i] = nameFormatter.CreateName(instanceNames[i]);
			}
		}
		protected virtual void CreatePerformanceCounters(string[] instanceNames)
		{
		}
		protected string CreateInstanceName(string nameSuffix)
		{
			return nameFormatter.CreateName(nameSuffix);
		}
		protected void FireManagementInstrumentation(BaseWmiEvent wmiEvent)
		{
			ManagementInstrumentation.Fire(wmiEvent);
		}
	}
}
