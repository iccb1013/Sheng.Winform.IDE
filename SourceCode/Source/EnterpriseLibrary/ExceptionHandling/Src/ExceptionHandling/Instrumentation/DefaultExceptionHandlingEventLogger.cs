/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation
{
	[EventLogDefinition("Application", "Enterprise Library ExceptionHandling")]
	[CustomFactory(typeof(DefaultExceptionHandlingEventLoggerCustomFactory))]
	public class DefaultExceptionHandlingEventLogger : InstrumentationListener
	{
		private IEventLogEntryFormatter eventLogEntryFormatter;
		public DefaultExceptionHandlingEventLogger(
			bool performanceCountersEnabled,
			bool eventLoggingEnabled,
			bool wmiEnabled,
			string applicationInstanceName)
			: base(performanceCountersEnabled, eventLoggingEnabled, wmiEnabled, new AppDomainNameFormatter(applicationInstanceName))
		{
			this.eventLogEntryFormatter = new EventLogEntryFormatter(Resources.BlockName);
		}
		public DefaultExceptionHandlingEventLogger(bool eventLoggingEnabled, bool wmiEnabled)
			: this(false, eventLoggingEnabled, wmiEnabled, null)
		{ }
		public void LogConfigurationError(Exception exception, string policyName)
		{
			if (WmiEnabled) FireManagementInstrumentation(new ExceptionHandlingConfigurationFailureEvent(policyName, exception.Message));
			if (EventLoggingEnabled)
			{
				string eventLogMessage
					= string.Format(
						Resources.Culture,
						Resources.ConfigurationFailureCreatingPolicy,
						policyName);
				string entryText = eventLogEntryFormatter.GetEntryText(eventLogMessage, exception);
				EventLog.WriteEntry(GetEventSourceName(), entryText, EventLogEntryType.Error);
			}
		}
		public void LogInternalError(string policyName, string exceptionMessage)
		{
			if (WmiEnabled) FireManagementInstrumentation(new ExceptionHandlingFailureEvent(policyName, exceptionMessage));
			if (EventLoggingEnabled)
			{
				string entryText = eventLogEntryFormatter.GetEntryText(exceptionMessage);
				EventLog.WriteEntry(GetEventSourceName(), entryText, EventLogEntryType.Error);
			}
		}
		[InstrumentationConsumer("ExceptionHandlingErrorOccurred")]
		public void ExceptionHandlingErrorOccurred(object sender, DefaultExceptionHandlingErrorEventArgs e)
		{
			LogInternalError(e.PolicyName, e.Message);
		}
	}
}
