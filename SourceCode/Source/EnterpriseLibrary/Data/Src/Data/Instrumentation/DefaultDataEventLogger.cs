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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;
using System.Diagnostics;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation
{
	[EventLogDefinition("Application", "Enterprise Library Data")]
	[CustomFactory(typeof(DefaultDataEventLoggerCustomFactory))]
	public class DefaultDataEventLogger : InstrumentationListener
	{
		private readonly IEventLogEntryFormatter eventLogEntryFormatter;
		public DefaultDataEventLogger(bool eventLoggingEnabled, bool wmiEnabled)
			: base((string)null, false, eventLoggingEnabled, wmiEnabled, null)
		{
			this.eventLogEntryFormatter = new EventLogEntryFormatter(Resources.BlockName);
		}
		public void LogConfigurationError(Exception exception, string instanceName)
		{
			if (WmiEnabled) FireManagementInstrumentation(new DataConfigurationFailureEvent(instanceName, exception.ToString()));
			if (EventLoggingEnabled)
			{
				string eventLogMessage
					= string.Format(
						Resources.Culture,
						Resources.ConfigurationFailureCreatingDatabase,
						instanceName);
				string entryText = eventLogEntryFormatter.GetEntryText(eventLogMessage, exception);
				EventLog.WriteEntry(GetEventSourceName(), entryText, EventLogEntryType.Error);
			}
		}
	}
}
