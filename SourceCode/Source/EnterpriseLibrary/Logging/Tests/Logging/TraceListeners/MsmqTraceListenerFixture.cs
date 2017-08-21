//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Messaging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests
{
    [TestClass]
    public class MsmqTraceListenerFixture
    {
        [TestMethod]
        public void CanSendMessageToQueue()
        {
            ILogFormatter formatter = new BinaryLogFormatter();
            MsmqTraceListener listener =
                new MsmqTraceListener("unnamed", CommonUtil.MessageQueuePath, formatter, MessagePriority.Low, true,
                                      MsmqTraceListenerData.DefaultTimeToBeReceived, MsmqTraceListenerData.DefaultTimeToReachQueue,
                                      false, false, false, MsmqTraceListenerData.DefaultTransactionType, new MockMsmqInterfaceFactory());
            LogSource source = new LogSource("unnamed", new List<TraceListener>(new TraceListener[] { listener }), SourceLevels.All);

            MockMsmqInterface.Instance.transactional = false;
            LogEntry entry = CommonUtil.GetDefaultLogEntry();
            source.TraceData(TraceEventType.Error, 1, entry);

            Message message = MockMsmqInterface.Instance.message;
            Assert.IsNotNull(message);
            Assert.IsNotNull(message.Body);
            Assert.AreEqual(message.Body.GetType(), typeof(string));
            Assert.AreEqual(MessageQueueTransactionType.None, MockMsmqInterface.Instance.transactionType);

            LogEntry deserializedEntry = BinaryLogFormatter.Deserialize(message.Body as string);
            Assert.IsNotNull(deserializedEntry);
        }

        [TestMethod]
        public void ShouldSendMessageToQueueApplyingFilter()
        {
            ILogFormatter formatter = new BinaryLogFormatter();
            MsmqTraceListener listener =
                new MsmqTraceListener("unnamed", CommonUtil.MessageQueuePath, formatter, MessagePriority.Low, true,
                                      MsmqTraceListenerData.DefaultTimeToBeReceived, MsmqTraceListenerData.DefaultTimeToReachQueue,
                                      false, false, false, MsmqTraceListenerData.DefaultTransactionType, new MockMsmqInterfaceFactory());

            // Filter only Warning
            listener.Filter = new EventTypeFilter(SourceLevels.Warning);

            LogSource source = new LogSource("unnamed", new List<TraceListener>(new TraceListener[] { listener }), SourceLevels.All);

            MockMsmqInterface.Instance.transactional = false;
            MockMsmqInterface.Instance.ResetMessageCount();

            LogEntry entry = CommonUtil.GetDefaultLogEntry();

            // Trace a Critical LogEntry
            source.TraceData(TraceEventType.Critical, 1, entry);

            Assert.AreEqual(1, MockMsmqInterface.Instance.MessageCount);
        }

        [TestMethod]
        public void ShouldNotSendMessageToQueue()
        {
            ILogFormatter formatter = new BinaryLogFormatter();
            MsmqTraceListener listener =
                new MsmqTraceListener("unnamed", CommonUtil.MessageQueuePath, formatter, MessagePriority.Low, true,
                                      MsmqTraceListenerData.DefaultTimeToBeReceived, MsmqTraceListenerData.DefaultTimeToReachQueue,
                                      false, false, false, MsmqTraceListenerData.DefaultTransactionType, new MockMsmqInterfaceFactory());

            // Filter only Critical
            listener.Filter = new EventTypeFilter(SourceLevels.Critical);

            LogSource source = new LogSource("unnamed", new List<TraceListener>(new TraceListener[] { listener }), SourceLevels.All);

            MockMsmqInterface.Instance.transactional = false;
            MockMsmqInterface.Instance.ResetMessageCount();

            LogEntry entry = CommonUtil.GetDefaultLogEntry();

            // Trace an Information LogEntry
            source.TraceData(TraceEventType.Information, 1, entry);

            Assert.AreEqual(0, MockMsmqInterface.Instance.MessageCount);
        }

        public void EnsureIntrinsicPropertiesAreInitialized()
        {
            ILogFormatter formatter = new BinaryLogFormatter();
            MsmqTraceListener listener =
                new MsmqTraceListener("unnamed", CommonUtil.MessageQueuePath, formatter, MessagePriority.Low, true,
                                      MsmqTraceListenerData.DefaultTimeToBeReceived, MsmqTraceListenerData.DefaultTimeToReachQueue,
                                      false, false, false, MsmqTraceListenerData.DefaultTransactionType, new MockMsmqInterfaceFactory());
            LogSource source = new LogSource("unnamed", new List<TraceListener>(new TraceListener[] { listener }), SourceLevels.All);

            MockMsmqInterface.Instance.transactional = false;

            LogEntry entry = CommonUtil.GetDefaultLogEntry();

            source.TraceData(TraceEventType.Error, 1, entry);

            Message message = MockMsmqInterface.Instance.message;
            Assert.IsNotNull(message);
            Assert.IsNotNull(message.Body);
            Assert.AreEqual(message.Body.GetType(), typeof(string));

            LogEntry deserializedEntry = BinaryLogFormatter.Deserialize(message.Body as string);
            Assert.IsNotNull(deserializedEntry);
        }

        public void EnsureIntrinsicPropertiesInitializedAreNotPickedUpDuringDeserialization()
        {
            ILogFormatter formatter = new BinaryLogFormatter();
            MsmqTraceListener listener =
                new MsmqTraceListener("unnamed", CommonUtil.MessageQueuePath, formatter, MessagePriority.Low, true,
                                      MsmqTraceListenerData.DefaultTimeToBeReceived, MsmqTraceListenerData.DefaultTimeToReachQueue,
                                      false, false, false, MsmqTraceListenerData.DefaultTransactionType, new MockMsmqInterfaceFactory());
            LogSource source = new LogSource("unnamed", new List<TraceListener>(new TraceListener[] { listener }), SourceLevels.All);

            MockMsmqInterface.Instance.transactional = false;

            Trace.CorrelationManager.ActivityId = Guid.NewGuid();

            LogEntry entry = CommonUtil.GetDefaultLogEntry();

            source.TraceData(TraceEventType.Error, 1, entry);

            // Resetting context before deserialization to ensure that 
            // the intrinsic properties picked ud during deserealization
            Trace.CorrelationManager.ActivityId = Guid.Empty;

            Message message = MockMsmqInterface.Instance.message;
            Assert.IsNotNull(message);
            Assert.IsNotNull(message.Body);
            Assert.AreEqual(message.Body.GetType(), typeof(string));

            LogEntry deserializedEntry = BinaryLogFormatter.Deserialize(message.Body as string);
            Assert.IsNotNull(deserializedEntry);

            // Ensure that the deserialized intrinsic properties remain the same
            // as the Log Entry
            Assert.AreEqual(deserializedEntry.ActivityId, entry.ActivityId);
        }

        [TestMethod]
        public void CanSendMessageToTransactionalQueue()
        {
            ILogFormatter formatter = new BinaryLogFormatter();
            MsmqTraceListener listener =
                new MsmqTraceListener("unnamed", CommonUtil.MessageQueuePath, formatter, MessagePriority.Low, true,
                                      MsmqTraceListenerData.DefaultTimeToBeReceived, MsmqTraceListenerData.DefaultTimeToReachQueue,
                                      false, false, false, MessageQueueTransactionType.Single, new MockMsmqInterfaceFactory());
            LogSource source = new LogSource("unnamed", SourceLevels.All);
            source.Listeners.Add(listener);

            MockMsmqInterface.Instance.transactional = true;
            LogEntry entry = CommonUtil.GetDefaultLogEntry();
            source.TraceData(TraceEventType.Error, 1, entry);

            Message message = MockMsmqInterface.Instance.message;
            Assert.IsNotNull(message);
            Assert.IsNotNull(message.Body);
            Assert.AreEqual(message.Body.GetType(), typeof(string));
            Assert.AreEqual(MessageQueueTransactionType.Single, MockMsmqInterface.Instance.transactionType);

            LogEntry deserializedEntry = BinaryLogFormatter.Deserialize(message.Body as string);
            Assert.IsNotNull(deserializedEntry);
            Assert.AreEqual(entry.Message, deserializedEntry.Message);
        }

        [TestMethod]
        public void CanBuildCorrectMessageWithBinaryLogFormatter()
        {
            ILogFormatter formatter = new BinaryLogFormatter();
            MsmqTraceListener listener =
                new MsmqTraceListener("unnamed", CommonUtil.MessageQueuePath, formatter, MessagePriority.Low, true,
                                      MsmqTraceListenerData.DefaultTimeToBeReceived, MsmqTraceListenerData.DefaultTimeToReachQueue,
                                      false, false, false, MessageQueueTransactionType.None);
            LogEntry entry = CommonUtil.GetDefaultLogEntry();

            Message message = listener.CreateMessage(entry);
            Assert.IsNotNull(message);
            Assert.IsNotNull(message.Body);
            Assert.AreEqual(message.Body.GetType(), typeof(string));

            LogEntry deserializedEntry = BinaryLogFormatter.Deserialize(message.Body as string);
            Assert.IsNotNull(deserializedEntry);
            Assert.AreEqual(entry.Message, deserializedEntry.Message);
        }

        [TestMethod]
        public void WritingMessageSendsSameMessageToMsmq()
        {
            ILogFormatter formatter = new BinaryLogFormatter();
            MsmqTraceListener listener =
                new MsmqTraceListener("unnamed", CommonUtil.MessageQueuePath, formatter, MessagePriority.Low, true,
                                      MsmqTraceListenerData.DefaultTimeToBeReceived, MsmqTraceListenerData.DefaultTimeToReachQueue,
                                      false, false, false, MsmqTraceListenerData.DefaultTransactionType, new MockMsmqInterfaceFactory());

            listener.Write("message");

            Message message = MockMsmqInterface.Instance.message;
            Assert.IsNotNull(message);
            Assert.IsNotNull(message.Body);
            Assert.AreEqual(typeof(string), message.Body.GetType());
            Assert.AreEqual("message", message.Body);
        }

        [TestMethod]
        public void LogToMsmqUsingDirectObjectOnlyResultsInOneMessage()
        {
            ILogFormatter formatter = new BinaryLogFormatter();
            MsmqTraceListener listener =
                new MsmqTraceListener("unnamed", CommonUtil.MessageQueuePath, formatter, MessagePriority.Low, true,
                                      MsmqTraceListenerData.DefaultTimeToBeReceived, MsmqTraceListenerData.DefaultTimeToReachQueue,
                                      false, false, false, MsmqTraceListenerData.DefaultTransactionType, new MockMsmqInterfaceFactory());
            TraceSource source = new TraceSource("unnamed", SourceLevels.All);
            source.Listeners.Add(listener);

            int numMessages = MockMsmqInterface.Instance.MessageCount;

            source.TraceData(TraceEventType.Error, 1, new TestCustomObject());
            source.Close();

            int newNumMessages = MockMsmqInterface.Instance.MessageCount;

            Assert.AreEqual(numMessages, newNumMessages - 1);
        }
    }

    class MockMsmqInterface : IMsmqSendInterface
    {
        internal static MockMsmqInterface Instance = new MockMsmqInterface();
        internal Message message = null;
        internal int numMessages = 0;
        internal bool transactional = true;
        internal MessageQueueTransactionType transactionType;

        public int MessageCount
        {
            get { return numMessages; }
        }

        public bool Transactional
        {
            get { return transactional; }
        }

        public void Close() {}

        public void Dispose() {}

        public void ResetMessageCount()
        {
            numMessages = 0;
        }

        public void Send(Message message,
                         MessageQueueTransactionType transactionType)
        {
            this.message = message;
            this.transactionType = transactionType;
            numMessages++;
        }
    }

    class MockMsmqInterfaceFactory : IMsmqSendInterfaceFactory
    {
        public IMsmqSendInterface CreateMsmqInterface(string queuePath)
        {
            return MockMsmqInterface.Instance;
        }
    }

    public class TestCustomObject
    {
        public override string ToString()
        {
            return "TestCustomObject";
        }
    }
}
