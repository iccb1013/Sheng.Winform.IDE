/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation
{
	[InstrumentationListener(typeof(LoggingInstrumentationListener))]
	public class LoggingInstrumentationProvider
	{
		[InstrumentationProvider("TraceListenerEntryWritten")]
		public event EventHandler<EventArgs> traceListenerEntryWritten;
		[InstrumentationProvider("FailureLoggingError")]
		public event EventHandler<FailureLoggingErrorEventArgs> failureLoggingError;
		[InstrumentationProvider("ConfigurationFailure")]
		public event EventHandler<LoggingConfigurationFailureEventArgs> configurationFailure;
		[InstrumentationProvider("LoggingEventRaised")]
		public event EventHandler<EventArgs> logEventRaised;
		[InstrumentationProvider("LockAcquisitionError")]
		public event EventHandler<LockAcquisitionErrorEventArgs> lockAcquisitionError;
		public void FireTraceListenerEntryWrittenEvent()
		{
			if (traceListenerEntryWritten != null) traceListenerEntryWritten(this, new EventArgs());
		}
		public void FireFailureLoggingErrorEvent(string message, Exception exception)
		{
			if (failureLoggingError != null) failureLoggingError(this, new FailureLoggingErrorEventArgs(message, exception));
		}
		public void FireConfigurationFailureEvent(System.Configuration.ConfigurationErrorsException configurationException)
		{
			if (configurationFailure != null) configurationFailure(this, new LoggingConfigurationFailureEventArgs(configurationException));
		}
		public void FireLogEventRaised()
		{
			if (logEventRaised != null) logEventRaised(this, new EventArgs());
		}
		internal void FireLockAcquisitionError(string message)
		{
			if (lockAcquisitionError != null) lockAcquisitionError(this, new LockAcquisitionErrorEventArgs(message));
		}
	}
	public class FailureLoggingErrorEventArgs : EventArgs
	{
		private string errorMessage;
		private Exception exception;
		public FailureLoggingErrorEventArgs(string errorMessage, Exception exception)
		{
			this.errorMessage = errorMessage;
			this.exception = exception;
		}
		public string ErrorMessage
		{
			get { return this.errorMessage; }
		}
		public Exception Exception
		{
			get { return exception; }
		}
	}
	public class LoggingConfigurationFailureEventArgs : EventArgs
	{
		private Exception exception;
		public LoggingConfigurationFailureEventArgs(Exception exception)
		{
			this.exception = exception;
		}
		public Exception Exception
		{
			get { return exception; }
		}
	}
	public class LockAcquisitionErrorEventArgs : EventArgs
	{
		private string errorMessage;
		public LockAcquisitionErrorEventArgs(string errorMessage)
		{
			this.errorMessage = errorMessage;
		}
		public string ErrorMessage
		{
			get
			{
				return errorMessage;
			}
		}
	}
}
