/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Messaging;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
	internal class MsmqSendInterface : IMsmqSendInterface
	{
		private MessageQueue messageQueue;
		internal MsmqSendInterface(string queuePath)
		{
			messageQueue = new MessageQueue(queuePath, false, true);
		}
		public void Close()
		{
			messageQueue.Close() ;
		}
		public void Dispose()
		{
			messageQueue.Dispose();
		}
		public void Send(Message message, MessageQueueTransactionType transactionType)
		{
			messageQueue.Send(message, transactionType);
		}
		public bool Transactional
		{
			get { return messageQueue.Transactional; }
		}
	}
}
