/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Net.Mail;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	public class EmailMessage
	{
		private EmailTraceListenerData configurationData;
		private ILogFormatter formatter;
		private LogEntry logEntry;
		public EmailMessage(EmailTraceListenerData configurationData, LogEntry logEntry, ILogFormatter formatter)
		{
			this.configurationData = configurationData;
			this.logEntry = logEntry;
			this.formatter = formatter;
		}
		public EmailMessage(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, LogEntry logEntry, ILogFormatter formatter)
		{
			this.configurationData = new EmailTraceListenerData(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, string.Empty);
			this.logEntry = logEntry;
			this.formatter = formatter;
		}
		public EmailMessage(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, string message, ILogFormatter formatter)
		{
			this.configurationData = new EmailTraceListenerData(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, string.Empty);
			this.logEntry = new LogEntry();
			logEntry.Message = message;
			this.formatter = formatter;
		}
		private bool IsEmpty(string subjectLineMarker)
		{
			return subjectLineMarker == null || subjectLineMarker.Length == 0;
		}
		private string GenerateSubjectPrefix(string subjectLineField)
		{
			return IsEmpty(subjectLineField)
				? ""
				: subjectLineField + " ";
		}
		private string GenerateSubjectSuffix(string subjectLineField)
		{
			return IsEmpty(subjectLineField)
				? ""
				: " " + subjectLineField;
		}
		protected MailMessage CreateMailMessage()
		{
			string header = GenerateSubjectPrefix(configurationData.SubjectLineStarter);
			string footer = GenerateSubjectSuffix(configurationData.SubjectLineEnder);
			string sendToSmtpSubject = header + logEntry.Severity.ToString() + footer;
			MailMessage message = new MailMessage();
			string[] toAddresses = configurationData.ToAddress.Split(';');
			foreach (string toAddress in toAddresses)
			{
				message.To.Add(new MailAddress(toAddress));
			}
			message.From = new MailAddress(configurationData.FromAddress);
			message.Body = (formatter != null) ? formatter.Format(logEntry) : logEntry.Message;
			message.Subject = sendToSmtpSubject;
			message.BodyEncoding = Encoding.UTF8;
			return message;
		}
		public virtual void Send()
		{
			using (MailMessage message = CreateMailMessage())
			{
				SendMessage(message);
			}
		}
		public virtual void SendMessage(MailMessage message)
		{
			SmtpClient smtpClient = new SmtpClient(configurationData.SmtpServer, configurationData.SmtpPort);
			smtpClient.Send(message);
		}
	}
}
