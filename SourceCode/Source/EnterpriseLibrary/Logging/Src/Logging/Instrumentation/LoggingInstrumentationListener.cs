/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using ManagementInstrumentation = System.Management.Instrumentation.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation
{
	[HasInstallableResourcesAttribute]
	[PerformanceCountersDefinition(counterCategoryName, "LoggingCountersHelpResource")]
	[EventLogDefinition("Application", "Enterprise Library Logging")]
	public class LoggingInstrumentationListener : InstrumentationListener
	{
		static EnterpriseLibraryPerformanceCounterFactory factory = new EnterpriseLibraryPerformanceCounterFactory();
        private const string TotalLoggingEventsRaised = "Total Logging Events Raised";
        private const string TotalTraceListenerEntriesWritten = "Total Trace Listener Entries Written";
		[PerformanceCounter("Logging Events Raised/sec", "LoggingEventRaisedHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
		private EnterpriseLibraryPerformanceCounter logEventRaised;
        [PerformanceCounter(TotalLoggingEventsRaised, "TotalLoggingEventsRaisedHelpResource", PerformanceCounterType.NumberOfItems32)]
        private EnterpriseLibraryPerformanceCounter totalLoggingEventsRaised;
		[PerformanceCounter("Trace Listener Entries Written/sec", "TraceListenerEntryWrittenHelpResource", PerformanceCounterType.RateOfCountsPerSecond32)]
		private EnterpriseLibraryPerformanceCounter traceListenerEntryWritten;
        [PerformanceCounter(TotalTraceListenerEntriesWritten, "TotalTraceListenerEntriesWrittenHelpResource", PerformanceCounterType.NumberOfItems32)]
        private EnterpriseLibraryPerformanceCounter totalTraceListenerEntriesWritten;
		private const string counterCategoryName = "Enterprise Library Logging Counters";
		private IEventLogEntryFormatter eventLogEntryFormatter;
		public LoggingInstrumentationListener(bool performanceCountersEnabled,
											  bool eventLoggingEnabled,
											  bool wmiEnabled,
                                              string applicationInstanceName)
            : base(performanceCountersEnabled, eventLoggingEnabled, wmiEnabled, new AppDomainNameFormatter(applicationInstanceName))
		{
			this.eventLogEntryFormatter = new EventLogEntryFormatter(Resources.BlockName);
		}
		public LoggingInstrumentationListener(string instanceName,
											  bool performanceCountersEnabled,
											  bool eventLoggingEnabled,
											  bool wmiEnabled,
                                              string applicationInstanceName)
            : base(instanceName, performanceCountersEnabled, eventLoggingEnabled, wmiEnabled, new AppDomainNameFormatter(applicationInstanceName))
		{
			this.eventLogEntryFormatter = new EventLogEntryFormatter(Resources.BlockName);
		}
		[InstrumentationConsumer("FailureLoggingError")]
		public void FailureLoggingError(object sender, FailureLoggingErrorEventArgs e)
		{
			if (WmiEnabled) FireManagementInstrumentation(new LoggingFailureLoggingErrorEvent(e.ErrorMessage, e.Exception.ToString()));
			if (EventLoggingEnabled)
			{
				string entryText = eventLogEntryFormatter.GetEntryText(e.ErrorMessage, e.Exception);
				EventLog.WriteEntry(GetEventSourceName(), entryText, EventLogEntryType.Error);
			}
		}
		[InstrumentationConsumer("LoggingEventRaised")]
		public void LoggingEventRaised(object sender, EventArgs e)
		{
            if (PerformanceCountersEnabled)
            {
                logEventRaised.Increment();
                totalLoggingEventsRaised.Increment();
            }
		}
		[InstrumentationConsumer("TraceListenerEntryWritten")]
		public void TraceListenerEntryWritten(object sender, EventArgs e)
		{
            if (PerformanceCountersEnabled)
            {
                traceListenerEntryWritten.Increment();
                totalTraceListenerEntriesWritten.Increment();
            }
		}
		[InstrumentationConsumer("ConfigurationFailure")]
		public void ConfigurationFailure(object sender, LoggingConfigurationFailureEventArgs e)
		{
			if (WmiEnabled) FireManagementInstrumentation(new LoggingConfigurationFailureEvent(e.Exception.Message));
			if (EventLoggingEnabled)
			{
				string entryText = eventLogEntryFormatter.GetEntryText(Resources.ConfigurationFailureUpdating, e.Exception);
				EventLog.WriteEntry(GetEventSourceName(), entryText, EventLogEntryType.Error);
			}
		}
		[InstrumentationConsumer("LockAcquisitionError")]
		public void LockAcquisitionError(object sender, LockAcquisitionErrorEventArgs e)
		{
			if (EventLoggingEnabled)
			{
				string entryText = eventLogEntryFormatter.GetEntryText(e.ErrorMessage);
				EventLog.WriteEntry(GetEventSourceName(), entryText, EventLogEntryType.Error);
			}
		}
		protected override void CreatePerformanceCounters(string[] instanceNames)
		{
			logEventRaised = factory.CreateCounter(counterCategoryName, "Logging Events Raised/sec", instanceNames);
			traceListenerEntryWritten = factory.CreateCounter(counterCategoryName, "Trace Listener Entries Written/sec", instanceNames);
            totalLoggingEventsRaised = factory.CreateCounter(counterCategoryName, TotalLoggingEventsRaised, instanceNames);
            totalTraceListenerEntriesWritten = factory.CreateCounter(counterCategoryName, TotalTraceListenerEntriesWritten, instanceNames);
		}
	}
}
