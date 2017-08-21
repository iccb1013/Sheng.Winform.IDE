/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using ManagementInstrumentation = System.Management.Instrumentation.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
using System.Diagnostics;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation
{
	[EventLogDefinition("Application", "Enterprise Library Logging")]
	[CustomFactory(typeof(DefaultLoggingEventLoggerCustomFactory))]
	public class DefaultLoggingEventLogger : InstrumentationListener
	{
		private readonly IEventLogEntryFormatter eventLogEntryFormatter;
		public DefaultLoggingEventLogger(bool eventLoggingEnabled, bool wmiEnabled)
			: base((string)null, false, eventLoggingEnabled, wmiEnabled, null)
		{
			this.eventLogEntryFormatter = new EventLogEntryFormatter(Resources.BlockName);
		}
		public void LogConfigurationError(Exception exception)
		{
			if (WmiEnabled) FireManagementInstrumentation(new LoggingConfigurationFailureEvent(exception.Message));
			if (EventLoggingEnabled)
			{
				string entryText = eventLogEntryFormatter.GetEntryText(Resources.ConfigurationFailureLogging, exception);
				EventLog.WriteEntry(GetEventSourceName(), entryText, EventLogEntryType.Error);
			}
		}
	}
}
