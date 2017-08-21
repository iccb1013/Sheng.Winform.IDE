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
using System.Diagnostics;
using System.Net.Mail;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests
{
    [TestClass]
    public class EmailTraceListenerFixture
    {
        static MailMessage lastMailMessageSent;

        [TestInitialize]
        public void SetUp()
        {
            lastMailMessageSent = null;
        }

        [TestMethod]
        public void ListenerWillUseFormatterIfExists()
        {
            MockEmailTraceListener listener = new MockEmailTraceListener(new TextFormatter("DUMMY\r\nTime:{timestamp}\r\nMessage:{message}DUMMY"));

            LogEntry entry = new LogEntry("Test Message", "Test Category", 42, 999, TraceEventType.Information, "Test Title", null);
            DateTime messageTimestamp = DateTime.UtcNow;

            // need to go through the source to get a TraceEventCache
            LogSource source = new LogSource("notfromconfig", SourceLevels.All);
            source.Listeners.Add(listener);
            source.TraceData(TraceEventType.Error, 0, entry);

            Assert.AreEqual("EntLib-Logging: Information has occurred", lastMailMessageSent.Subject);
            Assert.AreEqual("obviously.bad.email.address@127.0.0.1", lastMailMessageSent.To[0].Address);
            Assert.AreEqual("another.not.so.good.email.address@127.0.0.1", lastMailMessageSent.To[1].Address);
            Assert.AreEqual("logging@entlib.com", lastMailMessageSent.From.Address);
            AssertContainsSubstring(lastMailMessageSent.Body, messageTimestamp.ToString());
        }

        [TestMethod]
        public void ListenerWithParamsWillUseFormatterIfExists()
        {
            MockEmailTraceListener listener = new MockEmailTraceListener("obviously.bad.email.address@127.0.0.1;another.not.so.good.email.address@127.0.0.1", "logging@entlib.com",
                                                                         "EntLib-Logging ->", "has occurred", "smtphost", new TextFormatter("DUMMY\r\nTime:{timestamp}\r\nMessage:{message}DUMMY"));

            LogEntry entry = new LogEntry("Test Message", "Test Category", 42, 999, TraceEventType.Error, "Test Title", null);
            DateTime messageTimestamp = DateTime.UtcNow;

            LogSource source = new LogSource("notfromconfig", SourceLevels.All);
            source.Listeners.Add(listener);
            source.TraceData(TraceEventType.Error, 0, entry);

            Assert.AreEqual("EntLib-Logging -> Error has occurred", lastMailMessageSent.Subject);
            Assert.AreEqual("obviously.bad.email.address@127.0.0.1", lastMailMessageSent.To[0].Address);
            Assert.AreEqual("another.not.so.good.email.address@127.0.0.1", lastMailMessageSent.To[1].Address);
            Assert.AreEqual("logging@entlib.com", lastMailMessageSent.From.Address);
            AssertContainsSubstring(lastMailMessageSent.Body, messageTimestamp.ToString());
        }

        [TestMethod]
        public void ListenerWillFallbackToTraceEntryMessageIfFormatterDoesNotExists()
        {
            MockEmailTraceListener listener = new MockEmailTraceListener("obviously.bad.email.address@127.0.0.1;another.not.so.good.email.address@127.0.0.1", "logging@entlib.com",
                                                                         "EntLib-Logging ->", "has occurred", "smtphost");

            LogEntry entry = new LogEntry("Test Message", "Test Category", 42, 999, TraceEventType.Error, "Test Title", null);
            DateTime messageTimestamp = DateTime.UtcNow;

            LogSource source = new LogSource("notfromconfig", SourceLevels.All);
            source.Listeners.Add(listener);
            source.TraceData(TraceEventType.Error, 0, entry);

            Assert.AreEqual("EntLib-Logging -> Error has occurred", lastMailMessageSent.Subject);
            Assert.AreEqual("obviously.bad.email.address@127.0.0.1", lastMailMessageSent.To[0].Address);
            Assert.AreEqual("another.not.so.good.email.address@127.0.0.1", lastMailMessageSent.To[1].Address);
            Assert.AreEqual("logging@entlib.com", lastMailMessageSent.From.Address);
            AssertContainsSubstring(lastMailMessageSent.Body, "Test Message");
        }

        [TestMethod]
        public void LogToEmailUsingDirectObjectOnlyResultsInOneMessage()
        {
            MockEmailTraceListener listener = new MockEmailTraceListener("obviously.bad.email.address@127.0.0.1;another.not.so.good.email.address@127.0.0.1", "logging@entlib.com",
                                                                         "EntLib-Logging ->", "has occurred", "smtphost");

            TraceSource source = new TraceSource("unnamed", SourceLevels.All);
            source.Listeners.Add(listener);

            int numMessages = listener.MessagesSent;

            source.TraceData(TraceEventType.Error, 1, new TestCustomObject());
            source.Close();

            int newNumMessages = listener.MessagesSent;

            Assert.AreEqual(numMessages, newNumMessages - 1);
        }

        [TestMethod]
        public void LogToEmailApplyingFilter()
        {
            MockEmailTraceListener listener = new MockEmailTraceListener("obviously.bad.email.address@127.0.0.1;another.not.so.good.email.address@127.0.0.1", "logging@entlib.com",
                                                                         "EntLib-Logging ->", "has occurred", "smtphost");

            listener.Filter = new EventTypeFilter(SourceLevels.Warning);

            TraceSource source = new TraceSource("unnamed", SourceLevels.Error);
            source.Listeners.Add(listener);

            source.TraceData(TraceEventType.Critical, 1, new TestCustomObject());
            source.Close();

            Assert.AreEqual(1, listener.MessagesSent);
        }

        [TestMethod]
        public void ShouldFilterLog()
        {
            MockEmailTraceListener listener = new MockEmailTraceListener("obviously.bad.email.address@127.0.0.1;another.not.so.good.email.address@127.0.0.1", "logging@entlib.com",
                                                                         "EntLib-Logging ->", "has occurred", "smtphost");

            listener.Filter = new EventTypeFilter(SourceLevels.Warning);

            TraceSource source = new TraceSource("unnamed", SourceLevels.Error);
            source.Listeners.Add(listener);

            source.TraceData(TraceEventType.Information, 1, new TestCustomObject());
            source.Close();

            Assert.AreEqual(0, listener.MessagesSent);
        }

        void AssertContainsSubstring(string completeString,
                                     string subString)
        {
            Assert.IsTrue(completeString.IndexOf(subString) != -1);
        }

        public class MockEmailTraceListener : EmailTraceListener
        {
            const string mockFromAddress = "logging@entlib.com";
            const int mockSmtpPort = 25;
            const string mockSmtpServer = "smtphost";
            const string mockSubjectLineEnder = "has occurred";
            const string mockSubjectLineStarter = "EntLib-Logging:";
            const string mockToAddress = "obviously.bad.email.address@127.0.0.1;another.not.so.good.email.address@127.0.0.1";

            EmailTraceListenerData emailData;
            int numberMessagesSent = 0;

            public MockEmailTraceListener(ILogFormatter formatter)
                : this(mockToAddress, mockFromAddress, mockSubjectLineStarter, mockSubjectLineEnder, mockSmtpServer, mockSmtpPort, formatter) {}

            public MockEmailTraceListener(string toAddress,
                                          string fromAddress,
                                          string subjectLineStarter,
                                          string subjectLineEnder,
                                          string smtpServer)
                : base(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer)
            {
                emailData = new EmailTraceListenerData(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, string.Empty);
            }

            public MockEmailTraceListener(string toAddress,
                                          string fromAddress,
                                          string subjectLineStarter,
                                          string subjectLineEnder,
                                          string smtpServer,
                                          int smtpPort)
                : base(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort)
            {
                emailData = new EmailTraceListenerData(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, string.Empty);
            }

            public MockEmailTraceListener(string toAddress,
                                          string fromAddress,
                                          string subjectLineStarter,
                                          string subjectLineEnder,
                                          string smtpServer,
                                          ILogFormatter formatter)
                : base(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, formatter)
            {
                emailData = new EmailTraceListenerData(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, string.Empty);
            }

            public MockEmailTraceListener(string toAddress,
                                          string fromAddress,
                                          string subjectLineStarter,
                                          string subjectLineEnder,
                                          string smtpServer,
                                          int smtpPort,
                                          ILogFormatter formatter)
                : base(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, formatter)
            {
                emailData = new EmailTraceListenerData(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, string.Empty);
            }

            public int MessagesSent
            {
                get { return numberMessagesSent; }
            }

            public override void TraceData(TraceEventCache eventCache,
                                           string source,
                                           TraceEventType eventType,
                                           int id,
                                           object data)
            {
                EmailMessage message = null;
                if (data is LogEntry)
                {
                    message = new MockEmailMessage(emailData, data as LogEntry, Formatter);
                    message.Send();
                    numberMessagesSent++;
                }
                else if (data is string)
                {
                    Write(data);
                }
                else
                {
                    base.TraceData(eventCache, source, eventType, id, data);
                }
            }

            public override void Write(string message)
            {
                MockEmailMessage mailMessage = new MockEmailMessage(emailData.ToAddress, emailData.FromAddress, emailData.SubjectLineStarter, emailData.SubjectLineEnder, emailData.SmtpServer, emailData.SmtpPort, message, Formatter);
                mailMessage.Send();
                numberMessagesSent++;
            }
        }

        class MockEmailMessage : EmailMessage
        {
            public MockEmailMessage(EmailTraceListenerData emailParameters,
                                    LogEntry logEntry,
                                    ILogFormatter formatter)
                :
                    base(emailParameters, logEntry, formatter) {}

            public MockEmailMessage(string toAddress,
                                    string fromAddress,
                                    string subjectLineStarter,
                                    string subjectLineEnder,
                                    string smtpServer,
                                    int smtpPort,
                                    string message,
                                    ILogFormatter formatter)
                : base(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, message, formatter) {}

            public override void Send()
            {
                lastMailMessageSent = CreateMailMessage();
            }
        }
    }
}
