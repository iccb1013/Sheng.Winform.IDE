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
using ManagementInstrumentation = System.Management.Instrumentation.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation
{
	[HasInstallableResourcesAttribute]
	[PerformanceCountersDefinition(counterCategoryName, "ExceptionHandlingHelpResourceName")]
	[EventLogDefinition("Application", "Enterprise Library ExceptionHandling")]
	public class ExceptionHandlingInstrumentationListener : InstrumentationListener
	{
		static EnterpriseLibraryPerformanceCounterFactory factory = new EnterpriseLibraryPerformanceCounterFactory();
        private const string TotalExceptionHandlersExecuted = "Total Exception Handlers Executed";
        private const string TotalExceptionsHandled = "Total Exceptions Handled";
		[PerformanceCounter("Exceptions Handled/sec", "ExceptionHandledHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
		EnterpriseLibraryPerformanceCounter exceptionHandledCounter;
        [PerformanceCounter(TotalExceptionsHandled, "TotalExceptionsHandledHelpResource", PerformanceCounterType.NumberOfItems32)]
        EnterpriseLibraryPerformanceCounter totalExceptionsHandledCounter;
        [PerformanceCounter("Exception Handlers Executed/sec", "ExceptionHandlerExecutedHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
		EnterpriseLibraryPerformanceCounter exceptionHandlerExecutedCounter;
        [PerformanceCounter(TotalExceptionHandlersExecuted, "TotalExceptionHandlersExecutedHelpResource", PerformanceCounterType.NumberOfItems32)]
        EnterpriseLibraryPerformanceCounter totalExceptionHandlersExecutedCounter;
		private const string counterCategoryName = "Enterprise Library Exception Handling Counters";
		private string instanceName;
        public ExceptionHandlingInstrumentationListener(string instanceName,
                                           bool performanceCountersEnabled,
                                           bool eventLoggingEnabled,
                                           bool wmiEnabled,
                                           string applicationInstanceName)
            : this(instanceName, performanceCountersEnabled, eventLoggingEnabled, wmiEnabled, new AppDomainNameFormatter(applicationInstanceName))
        {
        }
		public ExceptionHandlingInstrumentationListener(string instanceName,
                                           bool performanceCountersEnabled,
                                           bool eventLoggingEnabled,
                                           bool wmiEnabled,
                                           IPerformanceCounterNameFormatter nameFormatter)
		: base(instanceName, performanceCountersEnabled, eventLoggingEnabled, wmiEnabled, nameFormatter)
        {
			this.instanceName = instanceName;
        }
        [InstrumentationConsumer("ExceptionHandled")]
		public void ExceptionHandled(object sender, EventArgs e)
		{
            if (PerformanceCountersEnabled)
            {
                exceptionHandledCounter.Increment();
                totalExceptionsHandledCounter.Increment();
            }
		}
		[InstrumentationConsumer("ExceptionHandlerExecuted")]
		public void ExceptionHandlerExecuted(object sender, EventArgs e)
		{
            if (PerformanceCountersEnabled)
            {
                exceptionHandlerExecutedCounter.Increment();
                totalExceptionHandlersExecutedCounter.Increment();
            }
		}
		[InstrumentationConsumer("ExceptionHandlingErrorOccurred")]
		public void ExceptionHandlingErrorOccurred(object sender, ExceptionHandlingErrorEventArgs e)
		{
			if (EventLoggingEnabled)
			{
				string errorMessage
					= string.Format(
						Resources.Culture,
						Resources.ErrorHandlingExceptionMessage,
						instanceName);
				string entryText = new EventLogEntryFormatter(Resources.BlockName).GetEntryText(errorMessage, e.Message);
				EventLog.WriteEntry(GetEventSourceName(), entryText, EventLogEntryType.Error);
			}
            if (WmiEnabled) FireManagementInstrumentation(new ExceptionHandlingFailureEvent(instanceName, e.Message));
		}
        protected override void CreatePerformanceCounters(string[] instanceNames)
		{
            exceptionHandledCounter = factory.CreateCounter(counterCategoryName, "Exceptions Handled/sec", instanceNames);
            exceptionHandlerExecutedCounter = factory.CreateCounter(counterCategoryName, "Exception Handlers Executed/sec", instanceNames);
            totalExceptionsHandledCounter = factory.CreateCounter(counterCategoryName, TotalExceptionsHandled, instanceNames);
            totalExceptionHandlersExecutedCounter = factory.CreateCounter(counterCategoryName, TotalExceptionHandlersExecuted, instanceNames);
		}
	}
}
