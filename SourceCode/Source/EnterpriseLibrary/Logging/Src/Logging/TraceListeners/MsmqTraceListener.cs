/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using System.Messaging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
	public class MsmqTraceListener : FormattedTraceListenerBase
	{
		readonly MessagePriority messagePriority;
		readonly IMsmqSendInterfaceFactory msmqInterfaceFactory;
		readonly string queuePath;
		readonly bool recoverable;
		readonly TimeSpan timeToBeReceived;
		readonly TimeSpan timeToReachQueue;
		readonly MessageQueueTransactionType transactionType;
		readonly bool useAuthentication;
		readonly bool useDeadLetterQueue;
		readonly bool useEncryption;
		public MsmqTraceListener(string name,
								 string queuePath,
								 ILogFormatter formatter,
								 MessagePriority messagePriority,
								 bool recoverable,
								 TimeSpan timeToReachQueue,
								 TimeSpan timeToBeReceived,
								 bool useAuthentication,
								 bool useDeadLetterQueue,
								 bool useEncryption,
								 MessageQueueTransactionType transactionType)
			: this(name, queuePath, formatter, messagePriority, recoverable,
				   timeToReachQueue, timeToBeReceived,
				   useAuthentication, useDeadLetterQueue, useEncryption,
				   transactionType, new MsmqSendInterfaceFactory()) { }
		public MsmqTraceListener(string name,
								 string queuePath,
								 ILogFormatter formatter,
								 MessagePriority messagePriority,
								 bool recoverable,
								 TimeSpan timeToReachQueue,
								 TimeSpan timeToBeReceived,
								 bool useAuthentication,
								 bool useDeadLetterQueue,
								 bool useEncryption,
								 MessageQueueTransactionType transactionType,
								 IMsmqSendInterfaceFactory msmqInterfaceFactory)
			: base(formatter)
		{
			this.queuePath = queuePath;
			this.messagePriority = messagePriority;
			this.recoverable = recoverable;
			this.timeToReachQueue = timeToReachQueue;
			this.timeToBeReceived = timeToBeReceived;
			this.useAuthentication = useAuthentication;
			this.useDeadLetterQueue = useDeadLetterQueue;
			this.useEncryption = useEncryption;
			this.transactionType = transactionType;
			this.msmqInterfaceFactory = msmqInterfaceFactory;
		}
		public string QueuePath
		{
			get { return queuePath; }
		}
		public Message CreateMessage(LogEntry logEntry)
		{
			string formattedLogEntry = FormatEntry(logEntry);
			return CreateMessage(formattedLogEntry, logEntry.Title);
		}
		Message CreateMessage(string messageBody,
							  string messageLabel)
		{
			Message queueMessage = new Message();
			queueMessage.Body = messageBody;
			queueMessage.Label = messageLabel;
			queueMessage.Priority = messagePriority;
			queueMessage.TimeToBeReceived = timeToBeReceived;
			queueMessage.TimeToReachQueue = timeToReachQueue;
			queueMessage.Recoverable = recoverable;
			queueMessage.UseAuthentication = useAuthentication;
			queueMessage.UseDeadLetterQueue = useDeadLetterQueue;
			queueMessage.UseEncryption = useEncryption;
			return queueMessage;
		}
		string FormatEntry(LogEntry entry)
		{
			entry.CollectIntrinsicProperties();
			string formattedMessage = Formatter.Format(entry);
			return formattedMessage;
		}
		void SendMessageToQueue(string message)
		{
			using (IMsmqSendInterface messageQueueInterface = msmqInterfaceFactory.CreateMsmqInterface(queuePath))
			{
				using (Message queueMessage = CreateMessage(message, string.Empty))
				{
					messageQueueInterface.Send(queueMessage, transactionType);
					messageQueueInterface.Close();
				}
			}
		}
		void SendMessageToQueue(LogEntry logEntry)
		{
			using (IMsmqSendInterface messageQueueInterface = msmqInterfaceFactory.CreateMsmqInterface(queuePath))
			{
				using (Message queueMessage = CreateMessage(logEntry))
				{
					messageQueueInterface.Send(queueMessage, transactionType);
					messageQueueInterface.Close();
				}
			}
		}
		public override void TraceData(TraceEventCache eventCache,
									   string source,
									   TraceEventType eventType,
									   int id,
									   object data)
		{
			if ((Filter == null) || Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null))
			{
				if (data is LogEntry)
				{
					SendMessageToQueue(data as LogEntry);
					InstrumentationProvider.FireTraceListenerEntryWrittenEvent();
				}
				else if (data is string)
				{
					Write(data as string);
				}
				else
				{
					base.TraceData(eventCache, source, eventType, id, data);
				}
			}
		}
		public override void Write(string message)
		{
			SendMessageToQueue(message);
		}
		public override void WriteLine(string message)
		{
			Write(message);
		}
	}
}
