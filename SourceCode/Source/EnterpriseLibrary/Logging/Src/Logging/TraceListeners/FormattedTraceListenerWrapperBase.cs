/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
    public abstract class FormattedTraceListenerWrapperBase : FormattedTraceListenerBase
    {
        private readonly TraceListener slaveListener;
        protected FormattedTraceListenerWrapperBase()
        {
        }
        protected FormattedTraceListenerWrapperBase(TraceListener slaveListener)
        {
            this.slaveListener = slaveListener;
        }
        protected FormattedTraceListenerWrapperBase(TraceListener slaveListener, ILogFormatter formater)
            : base(formater)
        {
            this.slaveListener = slaveListener;
        }
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType severity, int id, params object[] data)
        {
            if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, severity, id, null, null, null, data))
            {
                this.slaveListener.TraceData(eventCache, source, severity, id, data);
            }
        }
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType severity, int id, object data)
        {
            if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, severity, id, null, null, data, null))
            {
                if (data is LogEntry)
                {
                    if (this.Formatter != null)
                    {
                        this.slaveListener.TraceData(eventCache, source, severity, id, this.Formatter.Format(data as LogEntry));
                    }
                    else
                    {
                        this.slaveListener.TraceData(eventCache, source, severity, id, data);
                    }
                    InstrumentationProvider.FireTraceListenerEntryWrittenEvent();
                }
                else
                {
                    this.slaveListener.TraceData(eventCache, source, severity, id, data);
                }
            }
        }
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType severity, int id, string message)
        {
            if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, severity, id, message, null, null, null))
            {
                this.slaveListener.TraceEvent(eventCache, source, severity, id, message);
            }
        }
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType severity, int id, string format, params object[] args)
        {
            if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, severity, id, format, args, null, null))
            {
                this.slaveListener.TraceEvent(eventCache, source, severity, id, format, args);
            }
        }
        public override void Write(string message)
        {
            this.slaveListener.Write(message);
        }
        public override void WriteLine(string message)
        {
            this.slaveListener.WriteLine(message);
        }
        public TraceListener SlaveListener
        {
            get { return this.slaveListener; }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.slaveListener.Dispose();
            }
        }
    }
}
