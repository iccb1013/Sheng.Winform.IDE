/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using System.Messaging;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestClass]
    public class MsmqTraceListenerNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInMsmqTraceListenerNodeThrows()
        {
            new MsmqTraceListenerNode(null);
        }
        [TestMethod]
        public void MsmqTraceListenerNodeTest()
        {
            string name = "some name";
            string messageQueuePath = "some mq path";
            bool useDeadLetterQueue = true;
            bool useAuthentication = true;
            bool useEncryption = true;
            bool recoverable = false;
            TimeSpan timeToBeReceived = new TimeSpan(123);
            TimeSpan timeToReachQueue = new TimeSpan(123);
            MessagePriority messagePriority = MessagePriority.VeryHigh;
            MessageQueueTransactionType transactionType = MessageQueueTransactionType.Automatic;
            SourceLevels filter = SourceLevels.Critical;
            MsmqTraceListenerNode msmqTraceListenerNode = new MsmqTraceListenerNode();
            msmqTraceListenerNode.Name = name;
            msmqTraceListenerNode.QueuePath = messageQueuePath;
            msmqTraceListenerNode.MessagePriority = messagePriority;
            msmqTraceListenerNode.TransactionType = transactionType;
            msmqTraceListenerNode.UseEncryption = useEncryption;
            msmqTraceListenerNode.UseAuthentication = useAuthentication;
            msmqTraceListenerNode.UseDeadLetterQueue = useDeadLetterQueue;
            msmqTraceListenerNode.TimeToReachQueue = timeToReachQueue;
            msmqTraceListenerNode.TimeToBeReceived = timeToBeReceived;
            msmqTraceListenerNode.Recoverable = recoverable;
            msmqTraceListenerNode.Filter = filter;
            ApplicationNode.AddNode(msmqTraceListenerNode);
            MsmqTraceListenerData nodeData = (MsmqTraceListenerData)msmqTraceListenerNode.TraceListenerData;
            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(messageQueuePath, nodeData.QueuePath);
            Assert.AreEqual(transactionType, nodeData.TransactionType);
            Assert.AreEqual(messagePriority, nodeData.MessagePriority);
            Assert.AreEqual(useDeadLetterQueue, nodeData.UseDeadLetterQueue);
            Assert.AreEqual(useAuthentication, nodeData.UseAuthentication);
            Assert.AreEqual(useEncryption, nodeData.UseEncryption);
            Assert.AreEqual(timeToBeReceived, nodeData.TimeToBeReceived);
            Assert.AreEqual(timeToReachQueue, nodeData.TimeToReachQueue);
            Assert.AreEqual(recoverable, nodeData.Recoverable);
            Assert.AreEqual(filter, nodeData.Filter);
        }
        [TestMethod]
        public void MsmqTraceListenerNodeDataTest()
        {
            string name = "some name";
            string messageQueuePath = "some mq path";
            bool useDeadLetterQueue = true;
            bool useAuthentication = true;
            bool useEncryption = true;
            bool recoverable = false;
            TimeSpan timeToBeReceived = new TimeSpan(123);
            TimeSpan timeToReachQueue = new TimeSpan(123);
            MessagePriority messagePriority = MessagePriority.VeryHigh;
            MessageQueueTransactionType transactionType = MessageQueueTransactionType.Automatic;
            SourceLevels filter = SourceLevels.Critical;
            MsmqTraceListenerData msmqTraceListenerData = new MsmqTraceListenerData();
            msmqTraceListenerData.Name = name;
            msmqTraceListenerData.QueuePath = messageQueuePath;
            msmqTraceListenerData.MessagePriority = messagePriority;
            msmqTraceListenerData.TransactionType = transactionType;
            msmqTraceListenerData.UseEncryption = useEncryption;
            msmqTraceListenerData.UseAuthentication = useAuthentication;
            msmqTraceListenerData.UseDeadLetterQueue = useDeadLetterQueue;
            msmqTraceListenerData.TimeToReachQueue = timeToReachQueue;
            msmqTraceListenerData.TimeToBeReceived = timeToBeReceived;
            msmqTraceListenerData.Recoverable = recoverable;
            msmqTraceListenerData.Filter = filter;
            MsmqTraceListenerNode msmqTraceListenerNode = new MsmqTraceListenerNode(msmqTraceListenerData);
            ApplicationNode.AddNode(msmqTraceListenerNode);
            Assert.AreEqual(name, msmqTraceListenerNode.Name);
            Assert.AreEqual(messageQueuePath, msmqTraceListenerNode.QueuePath);
            Assert.AreEqual(transactionType, msmqTraceListenerNode.TransactionType);
            Assert.AreEqual(messagePriority, msmqTraceListenerNode.MessagePriority);
            Assert.AreEqual(useDeadLetterQueue, msmqTraceListenerNode.UseDeadLetterQueue);
            Assert.AreEqual(useAuthentication, msmqTraceListenerNode.UseAuthentication);
            Assert.AreEqual(useEncryption, msmqTraceListenerNode.UseEncryption);
            Assert.AreEqual(timeToBeReceived, msmqTraceListenerNode.TimeToBeReceived);
            Assert.AreEqual(timeToReachQueue, msmqTraceListenerNode.TimeToReachQueue);
            Assert.AreEqual(recoverable, msmqTraceListenerNode.Recoverable);
            Assert.AreEqual(filter, msmqTraceListenerNode.Filter);
        }
    }
}
