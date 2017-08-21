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
using System.Security.Permissions;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
	public class LogSource : IInstrumentationEventProvider, IDisposable
	{
        public const bool DefaultAutoFlushProperty = true;
		private LoggingInstrumentationProvider instrumentationProvider;
		private SourceLevels level;
		private string name;
		private List<TraceListener> traceListeners;
        private bool autoFlush = DefaultAutoFlushProperty;
		public LogSource(string name)
			: this(name, new List<TraceListener>(new TraceListener[] { new DefaultTraceListener() }), SourceLevels.All)
		{
		}
		public LogSource(string name, SourceLevels level)
			: this(name, new List<TraceListener>(new TraceListener[] { new DefaultTraceListener() }), level)
		{
		}
		public LogSource(string name, List<TraceListener> traceListeners, SourceLevels level)
		{
			this.name = name;
			this.traceListeners = traceListeners;
			this.level = level;
			this.instrumentationProvider = new LoggingInstrumentationProvider();
		}
        public LogSource(string name, List<TraceListener> traceListeners, SourceLevels level, bool autoFlush)
        {
            this.name = name;
            this.traceListeners = traceListeners;
            this.level = level;
            this.instrumentationProvider = new LoggingInstrumentationProvider();
            this.autoFlush = autoFlush;
        }
		public string Name
		{
			get { return name; }
		}
		public List<TraceListener> Listeners
		{
			get { return traceListeners; }
		}
		public SourceLevels Level
		{
			get { return level; }
		}
        public bool AutoFlush
        {
            get { return autoFlush; }
            set { this.autoFlush = value; }
        }
		public void TraceData(TraceEventType eventType, int id, LogEntry logEntry)
		{
			TraceData(eventType, id, logEntry, new TraceListenerFilter());
		}
		public void TraceData(TraceEventType eventType, int id, LogEntry logEntry, TraceListenerFilter traceListenerFilter)
		{
			if (!ShouldTrace(eventType)) return;
			TraceEventCache manager = new TraceEventCache();
			bool isTransfer = logEntry.Severity == TraceEventType.Transfer && logEntry.RelatedActivityId != null;
			foreach (TraceListener listener in traceListenerFilter.GetAvailableTraceListeners(traceListeners))
			{
				try
				{
					if (!listener.IsThreadSafe) Monitor.Enter(listener);
					if (!isTransfer)
					{
						listener.TraceData(manager, Name, eventType, id, logEntry);
					}
					else
					{
						listener.TraceTransfer(manager, Name, id, logEntry.Message, logEntry.RelatedActivityId.Value);
					}
					instrumentationProvider.FireTraceListenerEntryWrittenEvent();
                    if (this.AutoFlush)
                    {
                        listener.Flush();
                    }
				}
				finally
				{
					if (!listener.IsThreadSafe) Monitor.Exit(listener);
				}
			}
		}
		public object GetInstrumentationEventProvider()
		{
			return instrumentationProvider;
		}
		public void Dispose()
		{
			foreach (TraceListener listener in traceListeners)
			{
				listener.Dispose();
			}
		}
		private bool ShouldTrace(TraceEventType eventType)
		{
			return ((((TraceEventType)level) & eventType) != (TraceEventType)0);
		}
	}
}
