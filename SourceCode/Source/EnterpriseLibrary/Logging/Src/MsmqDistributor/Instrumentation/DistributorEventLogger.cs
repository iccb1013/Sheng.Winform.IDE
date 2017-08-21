/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Instrumentation
{
	public class DistributorEventLogger
	{
		internal static string Header = ExceptionFormatter.Header;
		private static readonly string DefaultApplicationName = Resources.DistributorEventLoggerDefaultApplicationName;
		private const string DefaultLogName = "Application";
		private string logName = null;
		private string applicationName = null;
		private NameValueCollection additionalInfo = new NameValueCollection();
		public DistributorEventLogger()
		{
			this.ApplicationName = DefaultApplicationName;
		}
		public DistributorEventLogger(string logName)
		{
			this.EventLogName = logName;
			this.ApplicationName = applicationName;
		}
		public string ApplicationName
		{
			get { return ((null == applicationName) ? DefaultApplicationName : applicationName); }
			set { applicationName = value; }
		}
		public string EventLogName
		{
			get { return ((null == logName) ? DefaultLogName : logName); }
			set { logName = value; }
		}
		public void AddMessage(string message, string value)
		{
			additionalInfo.Add(message, value);
		}
		public void LogServiceStarted()
		{
			LogServiceLifecycleEvent(string.Format(Resources.Culture, Resources.ServiceStartComplete, this.ApplicationName), true);
		}
		public void LogServiceResumed()
		{
			LogServiceLifecycleEvent(string.Format(Resources.Culture, Resources.ServiceResumeComplete, this.ApplicationName), true);
		}
		public void LogServiceStopped()
		{
			LogServiceLifecycleEvent(string.Format(Resources.Culture, Resources.ServiceStopComplete, this.ApplicationName), false);
		}
		public void LogServicePaused()
		{
			LogServiceLifecycleEvent(string.Format(Resources.Culture, Resources.ServicePausedSuccess, this.ApplicationName), false);
		}
		public void LogServiceFailure(string message, Exception exception, TraceEventType eventType)
		{
			this.AddMessage(DistributorEventLogger.Header, message);
			try
			{
				System.Management.Instrumentation.Instrumentation.Fire(new DistributorServiceFailureEvent(this.GetMessage(exception), exception));
			}
			catch
			{ }
			this.WriteToLog(exception, eventType);
		}
		private string GetMessage(Exception exception)
		{
			ExceptionFormatter exFormatter = new ExceptionFormatter(additionalInfo, this.ApplicationName);
			return exFormatter.GetMessage(exception);
		}
		private EventLogEntryType GetEventLogEntryType(TraceEventType severity)
		{
			if ((severity == TraceEventType.Error) || (severity == TraceEventType.Critical))
			{
				return EventLogEntryType.Error;
			}
			if (severity == TraceEventType.Warning)
			{
				return EventLogEntryType.Warning;
			}
			return EventLogEntryType.Information;
		}
		private void WriteToLog(Exception exception, TraceEventType severity)
		{
			string finalMessage = String.Empty;
			finalMessage = GetMessage(exception);
			additionalInfo.Clear();
			EventLog.WriteEntry(this.ApplicationName, finalMessage, GetEventLogEntryType(severity));
		}
		private void LogServiceLifecycleEvent(string message, bool started)
		{
			this.AddMessage(DistributorEventLogger.Header, message);
			try
			{
				System.Management.Instrumentation.Instrumentation.Fire(new DistributorServiceLifecycleEvent(this.GetMessage(null), started));
			}
			catch
			{ }
			this.WriteToLog(null, TraceEventType.Information);
		}
	}
}
