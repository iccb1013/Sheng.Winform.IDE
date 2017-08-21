/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
	public abstract class FormattedTraceListenerBase : TraceListener, IInstrumentationEventProvider
	{
		private ILogFormatter formatter;
		private LoggingInstrumentationProvider instrumentationProvider;
		protected LoggingInstrumentationProvider InstrumentationProvider
		{
			get { return instrumentationProvider; }
		}
		public FormattedTraceListenerBase()
		{
			instrumentationProvider = new LoggingInstrumentationProvider();
		}
		public FormattedTraceListenerBase(ILogFormatter formatter)
		{
			this.Formatter = formatter;
			instrumentationProvider = new LoggingInstrumentationProvider();
		}
        public override bool IsThreadSafe
        {
            get
            {
                return true;
            }
        }
		public ILogFormatter Formatter
		{
			get
			{
				return this.formatter;
			}
			set
			{
				this.formatter = value;
			}
		}
		public object GetInstrumentationEventProvider()
		{
			return instrumentationProvider;
		}
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null))
            {
                string text1 = string.Empty;
                if (data != null)
                {
                    text1 = data.ToString();
                    this.WriteLine(text1);
                }
            }
        }
	}
}
