/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation
{
	[HasInstallableResourcesAttribute]
	[PerformanceCountersDefinition(counterCategoryName, "LoggingCountersHelpResource")]
	[CustomFactory(typeof(TracerInstrumentationListenerCustomFactory))]
	public class TracerInstrumentationListener : InstrumentationListener
	{
		static EnterpriseLibraryPerformanceCounterFactory factory = new EnterpriseLibraryPerformanceCounterFactory();
        public const string TotalTraceOperationsStartedCounterName = "Total Trace Operations Started";
		[PerformanceCounter("Trace Operations Started/sec", "TraceOperationStartedHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
		private TracerPerformanceCounter traceOperationStarted;
        [PerformanceCounter(TotalTraceOperationsStartedCounterName, "TotalTraceOperationsStartedHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
        private TracerPerformanceCounter totalTraceOperationsStartedCounter;
		[PerformanceCounter("Avg. Trace Execution Time", "AverageTraceExecutionTimeHelpResource", PerformanceCounterType.AverageCount64,
			BaseCounterName = "Avg. Trace Execution Time Base", BaseCounterHelp = "AverageTraceExecutionTimeBaseHelpResource", BaseCounterType = PerformanceCounterType.AverageBase)]
		private TracerPerformanceCounter averageTraceExecutionTime;
		private TracerPerformanceCounter averageTraceExecutionTimeBase;
		public const string counterCategoryName = "Enterprise Library Logging Counters";
		public TracerInstrumentationListener(bool performanceCountersEnabled)
			: base("", performanceCountersEnabled, false, false, new AppDomainNameFormatter())
		{
		}
		public void TracerOperationStarted(string operationName)
		{
			if (PerformanceCountersEnabled)
			{
				string instanceName = CreateInstanceName(operationName);
				traceOperationStarted.Increment(instanceName);
                totalTraceOperationsStartedCounter.Increment(instanceName);
			}
		}
		public void TracerOperationEnded(string operationName, decimal traceDurationInMilliSeconds)
		{
			if (PerformanceCountersEnabled)
			{
				string instanceName = CreateInstanceName(operationName);
				averageTraceExecutionTime.IncrementBy(instanceName, (long)traceDurationInMilliSeconds);
				averageTraceExecutionTimeBase.Increment(instanceName);
			}
		}
		protected override void CreatePerformanceCounters(string[] instanceNames)
		{
			traceOperationStarted = new TracerPerformanceCounter(counterCategoryName, "Trace Operations Started/sec");
			averageTraceExecutionTime = new TracerPerformanceCounter(counterCategoryName, "Avg. Trace Execution Time");
			averageTraceExecutionTimeBase = new TracerPerformanceCounter(counterCategoryName, "Avg. Trace Execution Time Base");
            totalTraceOperationsStartedCounter = new TracerPerformanceCounter(counterCategoryName, TotalTraceOperationsStartedCounterName);
		}
	}
}
