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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
	[ConfigurationElementType(typeof(EmailTraceListenerData))]
	public class EmailTraceListener : FormattedTraceListenerBase
	{
		string toAddress = String.Empty;
		string fromAddress = String.Empty;
		string subjectLineStarter = String.Empty;
		string subjectLineEnder = String.Empty;
		string smtpServer = String.Empty;
		int smtpPort = 25;
		public EmailTraceListener()
			: base()
		{
		}
		public EmailTraceListener(ILogFormatter formatter)
			: base(formatter)
		{
		}
		public EmailTraceListener(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, ILogFormatter formatter)
			: this(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, 25, formatter)
		{
		}
		public EmailTraceListener(
			string toAddress,
			string fromAddress,
			string subjectLineStarter,
			string subjectLineEnder,
			string smtpServer,
			int smtpPort,
			ILogFormatter formatter
		)
			: base(formatter)
		{
			this.toAddress = toAddress;
			this.fromAddress = fromAddress;
			this.subjectLineStarter = subjectLineStarter;
			this.subjectLineEnder = subjectLineEnder;
			this.smtpServer = smtpServer;
			this.smtpPort = smtpPort;
		}
		public EmailTraceListener(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer)
			: this(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, 25)
		{
		}
		public EmailTraceListener(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort)
			: base()
		{
			this.toAddress = toAddress;
			this.fromAddress = fromAddress;
			this.subjectLineStarter = subjectLineStarter;
			this.subjectLineEnder = subjectLineEnder;
			this.smtpServer = smtpServer;
			this.smtpPort = smtpPort;
		}
		public override void Write(string message)
		{
			EmailMessage mailMessage = new EmailMessage(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, message, this.Formatter);
			mailMessage.Send();
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
                    EmailMessage message = new EmailMessage(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, data as LogEntry, this.Formatter);
                    message.Send();
                    InstrumentationProvider.FireTraceListenerEntryWrittenEvent();
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
		protected override string[] GetSupportedAttributes()
		{
			return new string[7] { "formatter", "toAddress", "fromAddress", "subjectLineStarter", "subjectLineEnder", "smtpServer", "smtpPort" };
		}
	}
}
