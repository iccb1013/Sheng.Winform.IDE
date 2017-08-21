/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using ManagementInstrumentation = System.Management.Instrumentation.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
	public class WmiTraceListener : TraceListener, IInstrumentationEventProvider
	{
		private LoggingInstrumentationProvider instrumentationProvider;
		public WmiTraceListener()
			: base()
		{
			instrumentationProvider = new LoggingInstrumentationProvider();
		}
		public override void Write(string message)
		{
			LogEntry logEntry = new LogEntry();
			logEntry.Message = message;
			ManagementInstrumentation.Fire(logEntry);
		}
		public override void WriteLine(string message)
		{
			Write(message);
		}
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
		{
            if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null))
            {
                if (data is LogEntry)
                {
                    ManagementInstrumentation.Fire(data as LogEntry);
                    instrumentationProvider.FireTraceListenerEntryWrittenEvent();
                }
                else if (data is string)
                {
                    Write(data);
                }
                else
                {
                    base.TraceData(eventCache, source, eventType, id, data);
                }
            }
		}
		public object GetInstrumentationEventProvider()
		{
			return instrumentationProvider;
		}
	}
}
