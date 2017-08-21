/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Messaging;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Tests.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Tests
{
    [TestClass]
    public class MsmqLogDistributorTransactionalBehaviorFixture
    {
        MsmqLogDistributor msmqDistributor;
        LogSource clientSource;
        LogWriter logWriter;
        DistributorEventLogger eventLogger;
        [TestInitialize]
        public void SetUp()
        {
            CommonUtil.DeletePrivateTestQ();
            CreateQueueForTesting();
            clientSource = new LogSource("unnamed", SourceLevels.All);
            clientSource.Listeners.Add(
                new MsmqTraceListener("unnamed", CommonUtil.MessageQueuePath, new BinaryLogFormatter(),
                                      MessagePriority.Normal, false, new TimeSpan(0, 1, 0), new TimeSpan(0, 1, 0),
                                      false, true, false, MessageQueueTransactionType.Single));
            LogSource distributorSource = new LogSource("unnamed", SourceLevels.All);
            distributorSource.Listeners.Add(new MockTraceListener());
            Dictionary<string, LogSource> traceSources = new Dictionary<string, LogSource>();
            logWriter = new LogWriter(new List<ILogFilter>(), traceSources, distributorSource, null, new LogSource("errors"), "default", false, false);
            eventLogger = new DistributorEventLogger();
            msmqDistributor = new MsmqLogDistributor(logWriter, CommonUtil.MessageQueuePath, eventLogger);
            msmqDistributor.StopReceiving = false;
        }
        protected virtual void CreateQueueForTesting()
        {
            CommonUtil.CreateTransactionalPrivateTestQ();
        }
        [TestCleanup]
        public void TearDown()
        {
            CommonUtil.DeletePrivateTestQ();
            MockTraceListener.Reset();
        }
        [TestMethod]
        public void Constructor()
        {
            msmqDistributor = new MsmqLogDistributor(new LogWriter(new List<ILogFilter>(), new List<LogSource>(), new LogSource("errors"), "default"), CommonUtil.MessageQueuePath, new DistributorEventLogger());
            Assert.IsNotNull(msmqDistributor);
        }
        [TestMethod]
        public void ReceiveMSMQMessage()
        {
            SendMessageToQ(CommonUtil.MsgBody);
            msmqDistributor.CheckForMessages();
            Assert.AreEqual(1, MockTraceListener.Entries.Count);
            Assert.AreEqual(CommonUtil.MsgBody, MockTraceListener.LastEntry.Message, "Body");
        }
        [TestMethod]
        public void ReceiveTwoMessages()
        {
            SendMessageToQ(CommonUtil.MsgBody);
            SendMessageToQ(CommonUtil.MsgBody + " 4 5 6");
            Assert.AreEqual(2, CommonUtil.GetNumberOfMessagesOnQueue());
            msmqDistributor.CheckForMessages();
            Assert.AreEqual(0, CommonUtil.GetNumberOfMessagesOnQueue());
            Assert.AreEqual(2, MockTraceListener.Entries.Count);
            Assert.AreEqual(CommonUtil.MsgBody + " 4 5 6", MockTraceListener.LastEntry.Message);
        }
        [TestMethod]
        public void SendTwoMessagesWithPauseReceiving()
        {
            SendMessageToQ(CommonUtil.MsgBody);
            SendMessageToQ(CommonUtil.MsgBody + " 4 5 6");
            msmqDistributor.StopReceiving = true;
            msmqDistributor.CheckForMessages();
            Assert.AreEqual(1, MockTraceListener.Entries.Count);
            Assert.AreEqual(CommonUtil.MsgBody, MockTraceListener.LastEntry.Message);
            msmqDistributor.CheckForMessages();
        }
        [TestMethod]
        public void SendCustomLogEntryViaMsmq()
        {
            CustomLogEntry log = new CustomLogEntry();
            log.TimeStamp = DateTime.MaxValue;
            log.Title = "My custom message title";
            log.Message = "My custom message body";
            log.Categories = new string[] { "CustomFormattedCategory" };
            log.AcmeCoField1 = "apple";
            log.AcmeCoField2 = "orange";
            log.AcmeCoField3 = "lemon";
            clientSource.TraceData(TraceEventType.Information, 1, log);
            msmqDistributor.CheckForMessages();
            Assert.IsFalse(MockTraceListener.LastEntry == log);
            Assert.AreEqual(MockTraceListener.LastEntry.Message, log.Message);
            Assert.AreEqual(((CustomLogEntry)MockTraceListener.LastEntry).AcmeCoField1, log.AcmeCoField1);
            Assert.AreEqual(((CustomLogEntry)MockTraceListener.LastEntry).AcmeCoField2, log.AcmeCoField2);
            Assert.AreEqual(((CustomLogEntry)MockTraceListener.LastEntry).AcmeCoField3, log.AcmeCoField3);
        }
        [TestMethod]
        public void SendLogEntryViaMsmq()
        {
            LogEntry log = new LogEntry();
            log.TimeStamp = DateTime.MaxValue;
            log.Title = "My custom message title";
            log.Message = "My custom message body";
            log.Categories = new string[] { "FormattedCategory" };
            clientSource.TraceData(TraceEventType.Information, 1, log);
            msmqDistributor.CheckForMessages();
            Assert.IsFalse(MockTraceListener.LastEntry == log);
            Assert.AreEqual(MockTraceListener.LastEntry.Message, log.Message);
        }
        [TestMethod]
        public void SendLogEntryWithDictionaryViaMsmq()
        {
            LogEntry log = new LogEntry();
            log.TimeStamp = DateTime.MaxValue;
            log.Title = "My custom message title";
            log.Message = "My custom message body";
            log.Categories = new string[] { "AppTest" };
            log.ExtendedProperties = CommonUtil.GetPropertiesDictionary();
            clientSource.TraceData(TraceEventType.Information, 1, log);
            msmqDistributor.CheckForMessages();
            Assert.IsFalse(MockTraceListener.LastEntry == log);
            Assert.AreEqual(MockTraceListener.LastEntry.Message, log.Message);
            Assert.AreEqual(MockTraceListener.LastEntry.ExtendedProperties.Count, log.ExtendedProperties.Count);
            foreach (string key in log.ExtendedProperties.Keys)
            {
                Assert.AreEqual(MockTraceListener.LastEntry.ExtendedProperties[key], log.ExtendedProperties[key]);
            }
        }
        [TestMethod]
        public void SendDictionaryWithNestedInvalidXml()
        {
            LogEntry log = new LogEntry();
            log.TimeStamp = DateTime.MaxValue;
            log.Title = "My custom message title";
            log.Message = "My custom message body";
            log.Categories = new string[] { "DictionaryCategory" };
            Dictionary<string, object> hash = new Dictionary<string, object>();
            hash["key1"] = "value1";
            hash["key2"] = "<xml>my values<field1>INVALID ><><XML</field2></xml>";
            hash["key3"] = "value3";
            log.ExtendedProperties = hash;
            clientSource.TraceData(TraceEventType.Information, 1, log);
            msmqDistributor.CheckForMessages();
            Assert.IsFalse(MockTraceListener.LastEntry == log);
            Assert.AreEqual(MockTraceListener.LastEntry.Message, log.Message);
            Assert.AreEqual(MockTraceListener.LastEntry.ExtendedProperties.Count, log.ExtendedProperties.Count);
            foreach (string key in log.ExtendedProperties.Keys)
            {
                Assert.AreEqual(MockTraceListener.LastEntry.ExtendedProperties[key], log.ExtendedProperties[key]);
            }
        }
        [TestMethod]
        public void MsmqAccessDenied()
        {
            MsmqReceiverTestWrapper testSync = new MsmqReceiverTestWrapper(logWriter, CommonUtil.MessageQueuePath, eventLogger);
            testSync.LogMsgQueueException(MessageQueueErrorCode.AccessDenied);
            string expected = string.Format(Resources.MsmqAccessDenied, CommonUtil.MessageQueuePath, WindowsIdentity.GetCurrent().Name);
            string actual = CommonUtil.GetLastEventLogEntry();
            Assert.IsTrue(actual.IndexOf(expected) > -1);
        }
        void SendMessageToQ(string body)
        {
            LogEntry logEntry = CommonUtil.GetDefaultLogEntry();
            logEntry.Categories = new string[] { "MockCategoryOne" };
            logEntry.Message = body;
            logEntry.Severity = TraceEventType.Information;
            clientSource.TraceData(logEntry.Severity, 1, logEntry);
        }
    }
}
