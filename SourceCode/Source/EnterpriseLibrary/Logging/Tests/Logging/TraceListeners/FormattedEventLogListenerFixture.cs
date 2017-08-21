/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests
{
    [TestClass]
    public class FormattedEventLogListenerFixture
    {
        [TestInitialize]
        public void SetUp()
        {
            if (EventLog.SourceExists(CommonUtil.EventLogSourceName))
            {
                EventLog.DeleteEventSource(CommonUtil.EventLogSourceName);
            }
        }
        [TestCleanup]
        public void TearDown()
        {
            if (EventLog.SourceExists(CommonUtil.EventLogSourceName))
            {
                EventLog.DeleteEventSource(CommonUtil.EventLogSourceName);
            }
        }
        [TestMethod]
        public void ListenerWillUseFormatterIfExists()
        {
            StringWriter writer = new StringWriter();
            FormattedEventLogTraceListener listener = new FormattedEventLogTraceListener(CommonUtil.EventLogSourceName, CommonUtil.EventLogNameCustom, new TextFormatter("DUMMY{newline}DUMMY"));
            LogSource source = new LogSource("notfromconfig", SourceLevels.All);
            source.Listeners.Add(listener);
            LogEntry logEntry = CommonUtil.GetDefaultLogEntry();
            source.TraceData(TraceEventType.Error, 1, logEntry);
            Assert.AreEqual("DUMMY" + Environment.NewLine + "DUMMY", CommonUtil.GetLastEventLogEntryCustom());
        }
        [TestMethod]
        public void ListenerWillFallbackToTraceEntryToStringIfFormatterDoesNotExists()
        {
            LogEntry testEntry = new LogEntry("message", "cat1", 0, 0, TraceEventType.Error, "title", null);
            StringWriter writer = new StringWriter();
            FormattedEventLogTraceListener listener = new FormattedEventLogTraceListener(CommonUtil.EventLogSourceName, CommonUtil.EventLogNameCustom, null);
            LogSource source = new LogSource("notfromconfig", SourceLevels.All);
            source.Listeners.Add(listener);
            source.TraceData(TraceEventType.Error, 1, testEntry);
            Assert.AreEqual(testEntry.ToString(), CommonUtil.GetLastEventLogEntryCustom());
        }
        [TestMethod]
        public void CanCreateListenerWithSourceAndFormatter()
        {
            FormattedEventLogTraceListener listener = new FormattedEventLogTraceListener("unknown source", new TextFormatter("TEST"));
            Assert.IsNotNull(listener.Formatter);
            Assert.IsNotNull(listener.SlaveListener);
            Assert.AreEqual(typeof(EventLogTraceListener), listener.SlaveListener.GetType());
            Assert.AreEqual("unknown source", ((EventLogTraceListener)listener.SlaveListener).EventLog.Source);
            Assert.AreEqual("", ((EventLogTraceListener)listener.SlaveListener).EventLog.Log);
            Assert.AreEqual(".", ((EventLogTraceListener)listener.SlaveListener).EventLog.MachineName);
        }
        [TestMethod]
        public void CanCreateListenerWithSourceLogAndFormatter()
        {
            FormattedEventLogTraceListener listener = new FormattedEventLogTraceListener("unknown source", "log", new TextFormatter("TEST"));
            Assert.IsNotNull(listener.Formatter);
            Assert.IsNotNull(listener.SlaveListener);
            Assert.AreEqual(typeof(EventLogTraceListener), listener.SlaveListener.GetType());
            Assert.AreEqual("unknown source", ((EventLogTraceListener)listener.SlaveListener).EventLog.Source);
            Assert.AreEqual("log", ((EventLogTraceListener)listener.SlaveListener).EventLog.Log);
            Assert.AreEqual(".", ((EventLogTraceListener)listener.SlaveListener).EventLog.MachineName);
        }
        [TestMethod]
        public void CanCreateListenerWithSourceFormatterAndDefaultLogMachineName()
        {
            FormattedEventLogTraceListener listener = new FormattedEventLogTraceListener("unknown source", "", ".", new TextFormatter("TEST"));
            Assert.IsNotNull(listener.Formatter);
            Assert.IsNotNull(listener.SlaveListener);
            Assert.AreEqual(typeof(EventLogTraceListener), listener.SlaveListener.GetType());
            Assert.AreEqual("unknown source", ((EventLogTraceListener)listener.SlaveListener).EventLog.Source);
            Assert.AreEqual("", ((EventLogTraceListener)listener.SlaveListener).EventLog.Log);
            Assert.AreEqual(".", ((EventLogTraceListener)listener.SlaveListener).EventLog.MachineName);
        }
        [TestMethod]
        public void CanCreateListenerWithSourceFormatterLogAndMachineName()
        {
            FormattedEventLogTraceListener listener = new FormattedEventLogTraceListener("unknown source", "log", "machine", new TextFormatter("TEST"));
            Assert.IsNotNull(listener.Formatter);
            Assert.IsNotNull(listener.SlaveListener);
            Assert.AreEqual(typeof(EventLogTraceListener), listener.SlaveListener.GetType());
            Assert.AreEqual("unknown source", ((EventLogTraceListener)listener.SlaveListener).EventLog.Source);
            Assert.AreEqual("log", ((EventLogTraceListener)listener.SlaveListener).EventLog.Log);
            Assert.AreEqual("machine", ((EventLogTraceListener)listener.SlaveListener).EventLog.MachineName);
        }
        [TestMethod]
        public void CanCreateListenerWithSourceFormatterLogAndEmptyMachineName()
        {
            FormattedEventLogTraceListener listener = new FormattedEventLogTraceListener("unknown source", "log", "", new TextFormatter("TEST"));
            Assert.IsNotNull(listener.Formatter);
            Assert.IsNotNull(listener.SlaveListener);
            Assert.AreEqual(typeof(EventLogTraceListener), listener.SlaveListener.GetType());
            Assert.AreEqual("unknown source", ((EventLogTraceListener)listener.SlaveListener).EventLog.Source);
            Assert.AreEqual("log", ((EventLogTraceListener)listener.SlaveListener).EventLog.Log);
            Assert.AreEqual(".", ((EventLogTraceListener)listener.SlaveListener).EventLog.MachineName);
        }
        [TestMethod]
        public void CanWriteToEventLog()
        {
            FormattedEventLogTraceListener listener =
                new FormattedEventLogTraceListener(CommonUtil.EventLogSourceName,
                                                   CommonUtil.EventLogNameCustom,
                                                   FormattedEventLogTraceListener.DefaultMachineName,
                                                   new TextFormatter("{message}"));
            LogSource source = new LogSource("transient", SourceLevels.All);
            source.Listeners.Add(listener);
            LogEntry entry = CommonUtil.GetDefaultLogEntry();
            entry.Severity = TraceEventType.Error;
            CommonUtil.ResetEventLogCounterCustom();
            source.TraceData(entry.Severity, entry.EventId, entry);
            using (EventLog customLog = CommonUtil.GetCustomEventLog())
            {
                Assert.AreEqual(1, CommonUtil.EventLogEntryCountCustom());
                Assert.AreEqual(CommonUtil.MsgBody, customLog.Entries[customLog.Entries.Count - 1].Message);
            }
        }
        [TestMethod]
        public void WillNotWriteToTheEventLogIfRequestIsFilteredOut()
        {
            FormattedEventLogTraceListener listener =
                new FormattedEventLogTraceListener(CommonUtil.EventLogSourceName,
                                                   CommonUtil.EventLogNameCustom,
                                                   FormattedEventLogTraceListener.DefaultMachineName,
                                                   new TextFormatter("{message}"));
            listener.Filter = new EventTypeFilter(SourceLevels.Critical);
            LogSource source = new LogSource("transient", SourceLevels.All);
            source.Listeners.Add(listener);
            LogEntry entry = CommonUtil.GetDefaultLogEntry();
            entry.Severity = TraceEventType.Error;
            CommonUtil.ResetEventLogCounterCustom();
            source.TraceData(entry.Severity, entry.EventId, entry);
            using (EventLog customLog = CommonUtil.GetCustomEventLog())
            {
                Assert.AreEqual(0, CommonUtil.EventLogEntryCountCustom());
            }
        }
    }
}
