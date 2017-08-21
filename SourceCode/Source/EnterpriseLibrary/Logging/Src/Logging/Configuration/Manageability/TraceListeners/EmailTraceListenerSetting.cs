/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
	[ManagementEntity]
	public class EmailTraceListenerSetting : TraceListenerSetting
	{
		string formatter;
		string fromAddress;
		int smtpPort;
		string smtpServer;
		string subjectLineEnder;
		string subjectLineStarter;
		string toAddress;
		public EmailTraceListenerSetting(EmailTraceListenerData sourceElement,
										   string name,
										   string formatter,
										   string fromAddress,
										   int smtpPort,
										   string smtpServer,
										   string subjectLineEnder,
										   string subjectLineStarter,
										   string toAddress,
										   string traceOutputOptions,
										   string filter)
			: base(sourceElement, name, traceOutputOptions, filter)
		{
			this.formatter = formatter;
			this.fromAddress = fromAddress;
			this.smtpPort = smtpPort;
			this.smtpServer = smtpServer;
			this.subjectLineEnder = subjectLineEnder;
			this.subjectLineStarter = subjectLineStarter;
			this.toAddress = toAddress;
		}
		[ManagementConfiguration]
		public string Formatter
		{
			get { return formatter; }
			set { formatter = value; }
		}
		[ManagementConfiguration]
		public string FromAddress
		{
			get { return fromAddress; }
			set { fromAddress = value; }
		}
		[ManagementConfiguration]
		public int SmtpPort
		{
			get { return smtpPort; }
			set { smtpPort = value; }
		}
		[ManagementConfiguration]
		public string SmtpServer
		{
			get { return smtpServer; }
			set { smtpServer = value; }
		}
		[ManagementConfiguration]
		public string SubjectLineEnder
		{
			get { return subjectLineEnder; }
			set { subjectLineEnder = value; }
		}
		[ManagementConfiguration]
		public string SubjectLineStarter
		{
			get { return subjectLineStarter; }
			set { subjectLineStarter = value; }
		}
		[ManagementConfiguration]
		public string ToAddress
		{
			get { return toAddress; }
			set { toAddress = value; }
		}
		[ManagementBind]
		public static EmailTraceListenerSetting BindInstance(string ApplicationName,
															 string SectionName,
															 string Name)
		{
			return BindInstance<EmailTraceListenerSetting>(ApplicationName, SectionName, Name);
		}
		[ManagementEnumerator]
		public static IEnumerable<EmailTraceListenerSetting> GetInstances()
		{
			return GetInstances<EmailTraceListenerSetting>();
		}
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return EmailTraceListenerDataWmiMapper.SaveChanges(this, sourceElement);
		}
	}
}
