/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using System.IO;
using System.Messaging;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor
{
	public class MsmqLogDistributor
	{
		private bool isCompleted = true;
		private bool stopReceiving = false;
		private LogWriter logWriter;
		private string msmqPath;
		private DistributorEventLogger eventLogger;
		public MsmqLogDistributor(LogWriter logWriter, string msmqPath, DistributorEventLogger eventLogger)
		{
			this.logWriter = logWriter;
			this.msmqPath = msmqPath;
			this.eventLogger = eventLogger;
		}
		public virtual bool IsCompleted
		{
			get { return this.isCompleted; }
		}
		public virtual bool StopReceiving
		{
			get { return this.stopReceiving; }
			set { this.stopReceiving = value; }
		}
		public virtual void CheckForMessages()
		{
			try
			{
				ReceiveQueuedMessages();
			}
			catch (MessageQueueException qex)
			{
				string errorMsg = LogMessageQueueException(qex.MessageQueueErrorCode, qex);
				throw new LoggingException(errorMsg, qex);
			}
			catch (LoggingException)
			{
				throw;
			}
			catch (Exception ex)
			{
				string errorMsg = string.Format(Resources.Culture, Resources.MsmqReceiveGeneralError, msmqPath);
				this.eventLogger.LogServiceFailure(
					errorMsg,
					ex,
					TraceEventType.Error);
				throw new LoggingException(errorMsg, ex);
			}
			finally
			{
				this.isCompleted = true;
			}
		}
		protected string LogMessageQueueException(MessageQueueErrorCode code, Exception e)
		{
			TraceEventType logType = TraceEventType.Error;
			string errorMsg = string.Empty;
			if (code == MessageQueueErrorCode.TransactionUsage)
			{
				errorMsg = string.Format(Resources.Culture, Resources.MsmqInvalidTransactionUsage, msmqPath);
			}
			else if (code == MessageQueueErrorCode.IOTimeout)
			{
				errorMsg = string.Format(Resources.Culture, Resources.MsmqReceiveTimeout, msmqPath);
				logType = TraceEventType.Warning;
			}
			else if (code == MessageQueueErrorCode.AccessDenied)
			{
				errorMsg = string.Format(Resources.Culture, Resources.MsmqAccessDenied, msmqPath, WindowsIdentity.GetCurrent().Name);
			}
			else
			{
				errorMsg = string.Format(Resources.Culture, Resources.MsmqReceiveError, msmqPath);
			}
			this.eventLogger.LogServiceFailure(
				errorMsg,
				e,
				logType);
			return errorMsg;
		}
		private MessageQueue CreateMessageQueue()
		{
			MessageQueue messageQueue = new MessageQueue(msmqPath, false, true);
			((XmlMessageFormatter)messageQueue.Formatter).TargetTypeNames = new string[] { "System.String" };
			return messageQueue;
		}
		private bool IsQueueEmpty()
		{
			bool empty = false;
			try
			{
				using (MessageQueue msmq = CreateMessageQueue())
				{
					msmq.Peek(new TimeSpan(0));
				}
			}
			catch (MessageQueueException e)
			{
				if (e.MessageQueueErrorCode == MessageQueueErrorCode.IOTimeout)
				{
					empty = true;
				}
			}
			return empty;
		}
		private void ReceiveQueuedMessages()
		{
			this.isCompleted = false;
			while (!IsQueueEmpty())
			{
				using (MessageQueue msmq = CreateMessageQueue())
				{
					Message message = msmq.Peek();
					string serializedEntry = message.Body.ToString();
					LogEntry logEntry = null;
					try
					{
						logEntry = BinaryLogFormatter.Deserialize(serializedEntry);
					}
					catch (FormatException formatException)
					{
						string logMessage = string.Format(
							Resources.Culture,
							Resources.ExceptionCouldNotDeserializeMessageFromQueue,
							message.Id,
							msmq.Path);
						this.eventLogger.LogServiceFailure(
							logMessage,
							formatException,
							TraceEventType.Error);
						throw new LoggingException(logMessage, formatException);
					}
					catch (SerializationException serializationException)
					{
						string logMessage = string.Format(
							Resources.Culture,
							Resources.ExceptionCouldNotDeserializeMessageFromQueue,
							message.Id,
							msmq.Path);
						this.eventLogger.LogServiceFailure(
							logMessage,
							serializationException,
							TraceEventType.Error);
						throw new LoggingException(logMessage, serializationException);
					}
					if (logEntry != null)
					{
						logWriter.Write(logEntry);
					}
					message = msmq.Receive();
					if (this.StopReceiving)
					{
						this.isCompleted = true;
						return;
					}
				}
			}
		}
	}
}
