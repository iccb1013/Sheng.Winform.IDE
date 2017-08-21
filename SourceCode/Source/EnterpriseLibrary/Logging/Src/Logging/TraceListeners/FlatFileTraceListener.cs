/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
	[ConfigurationElementType(typeof(FlatFileTraceListenerData))]
	public class FlatFileTraceListener : FormattedTextWriterTraceListener
	{
		string header = String.Empty;
		string footer = String.Empty;
		public FlatFileTraceListener()
			: base()
		{
		}
		public FlatFileTraceListener(ILogFormatter formatter)
			: base(formatter)
		{
		}
		public FlatFileTraceListener(FileStream stream, ILogFormatter formatter)
			: base(stream, formatter)
		{
		}
		public FlatFileTraceListener(FileStream stream)
			: base(stream)
		{
		}
		public FlatFileTraceListener(StreamWriter writer, ILogFormatter formatter)
			: base(writer, formatter)
		{
		}
		public FlatFileTraceListener(StreamWriter writer)
			: base(writer)
		{
		}
		public FlatFileTraceListener(string fileName, ILogFormatter formatter)
            : base(EnvironmentHelper.ReplaceEnvironmentVariables(fileName), formatter)
		{
		}
        public FlatFileTraceListener(string fileName)
            : base(EnvironmentHelper.ReplaceEnvironmentVariables(fileName))
        {
        }
		public FlatFileTraceListener(string fileName, string header, string footer, ILogFormatter formatter)
            : base(EnvironmentHelper.ReplaceEnvironmentVariables(fileName), formatter)
		{
			this.header = header;
			this.footer = footer;
		}
		public FlatFileTraceListener(string fileName, string header, string footer)
            : base(EnvironmentHelper.ReplaceEnvironmentVariables(fileName))
		{
			this.header = header;
			this.footer = footer;
		}
		public FlatFileTraceListener(FileStream stream, string name, ILogFormatter formatter)
			: base(stream, name, formatter)
		{
		}
		public FlatFileTraceListener(FileStream stream, string name)
			: base(stream, name)
		{
		}
		public FlatFileTraceListener(StreamWriter writer, string name, ILogFormatter formatter)
			: base(writer, name, formatter)
		{
		}
		public FlatFileTraceListener(StreamWriter writer, string name)
			: base(writer, name)
		{
		}
		public FlatFileTraceListener(string fileName, string name, ILogFormatter formatter)
            : base(EnvironmentHelper.ReplaceEnvironmentVariables(fileName), name, formatter)
		{
		}
		public FlatFileTraceListener(string fileName, string name)
            : base(EnvironmentHelper.ReplaceEnvironmentVariables(fileName), name)
		{
		}
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
		{
            if (this.Filter == null || this.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null))
            {
                if (header.Length > 0)
                    WriteLine(header);
                if (data is LogEntry)
                {
                    if (this.Formatter != null)
                    {
                        base.WriteLine(this.Formatter.Format(data as LogEntry));
                    }
                    else
                    {
                        base.TraceData(eventCache, source, eventType, id, data);
                    }
                    InstrumentationProvider.FireTraceListenerEntryWrittenEvent();
                }
                else
                {
                    base.TraceData(eventCache, source, eventType, id, data);
                }
                if (footer.Length > 0)
                    WriteLine(footer);
            }
		}
		protected override string[] GetSupportedAttributes()
		{
			return new string[4] { "formatter", "FileName", "header", "footer" };
		}
	}
}
