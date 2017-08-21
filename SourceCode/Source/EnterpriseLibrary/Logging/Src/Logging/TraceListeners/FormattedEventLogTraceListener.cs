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
	public class FormattedEventLogTraceListener : FormattedTraceListenerWrapperBase
	{
		public const string DefaultLogName = "";
		public const string DefaultMachineName = ".";
		public FormattedEventLogTraceListener()
			: base(new EventLogTraceListener())
		{
		}
		public FormattedEventLogTraceListener(ILogFormatter formater)
			: base(new EventLogTraceListener(), formater)
		{
		}
		public FormattedEventLogTraceListener(EventLog eventLog)
			: base(new EventLogTraceListener(eventLog))
		{
		}
		public FormattedEventLogTraceListener(EventLog eventLog, ILogFormatter formatter)
			: base(new EventLogTraceListener(eventLog), formatter)
		{
		}
		public FormattedEventLogTraceListener(string source)
			: base(new EventLogTraceListener(source))
		{
		}
		public FormattedEventLogTraceListener(string source, ILogFormatter formatter)
			: base(new EventLogTraceListener(source), formatter)
		{
		}
		public FormattedEventLogTraceListener(string source, string log, ILogFormatter formatter)
			: base(new EventLogTraceListener(new EventLog(log, DefaultMachineName, source)), formatter)
		{
		}
		public FormattedEventLogTraceListener(string source, string log, string machineName, ILogFormatter formatter)
			: base(new EventLogTraceListener(new EventLog(log, NormalizeMachineName(machineName), source)), formatter)
		{
		}
		private static string NormalizeMachineName(string machineName)
		{
			return string.IsNullOrEmpty(machineName) ? DefaultMachineName : machineName;
		}
	}
}
