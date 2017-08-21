/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Messaging;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
	public interface IMsmqSendInterface : IDisposable
	{
		void Close();
		void Send(Message message, MessageQueueTransactionType transactionType);
		bool Transactional { get; }
	}
}
