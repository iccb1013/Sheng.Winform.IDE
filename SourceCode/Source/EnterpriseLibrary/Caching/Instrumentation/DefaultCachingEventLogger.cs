/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using ManagementInstrumentation = System.Management.Instrumentation.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
    [EventLogDefinition("Application", EventLogSourceName)]
    [CustomFactory(typeof(DefaultCachingEventLoggerCustomFactory))]
    public class DefaultCachingEventLogger : InstrumentationListener
    {
        public const string EventLogSourceName = CachingInstrumentationListener.EventLogSourceName;
        readonly IEventLogEntryFormatter eventLogEntryFormatter;
        public DefaultCachingEventLogger(bool eventLoggingEnabled,
                                         bool wmiEnabled)
            : base((string)null, false, eventLoggingEnabled, wmiEnabled, null)
        {
            eventLogEntryFormatter = new EventLogEntryFormatter(Resources.BlockName);
        }
        public void LogConfigurationError(string instanceName,
                                          Exception exception)
        {
            if (WmiEnabled) FireManagementInstrumentation(new CacheConfigurationFailureEvent(instanceName, exception.ToString()));
            if (EventLoggingEnabled)
            {
                string errorMessage
                    = string.Format(
                        Resources.Culture,
                        Resources.ErrorCacheConfigurationFailedMessage,
                        instanceName);
                string entryText = eventLogEntryFormatter.GetEntryText(errorMessage, exception);
                EventLog.WriteEntry(GetEventSourceName(), entryText, EventLogEntryType.Error);
            }
        }
    }
}
