/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    [TestClass]
    public class TraceSourceFixture
    {
        [TestMethod]
        public void LogSourceCallsFlushRegardlessOfAutoflushValue()
        {
            MockFlushSensingTraceListener traceListener = new MockFlushSensingTraceListener();
            List<TraceListener> traceListeners = new List<TraceListener>(1);
            traceListeners.Add(traceListener);
            LogSource logSource = new LogSource("name", traceListeners, SourceLevels.All);
            bool currentAutoFlush = Trace.AutoFlush;
            try
            {
                Trace.AutoFlush = false;
                logSource.TraceData(TraceEventType.Critical, 0, CommonUtil.GetDefaultLogEntry());
                Trace.AutoFlush = true;
                logSource.TraceData(TraceEventType.Critical, 0, CommonUtil.GetDefaultLogEntry());
                Assert.AreEqual(2, traceListener.flushCalls);
            }
            finally
            {
                Trace.AutoFlush = currentAutoFlush;
            }
        }
        [TestMethod]
        public void AutoFlushDefaultPropertyIsTrue()
        {
            string name = "name";
            bool defaultAutoFlushProperty = true;
            LogSource logSource = new LogSource(name);
            Assert.AreEqual(defaultAutoFlushProperty, logSource.AutoFlush);
        }
        [TestMethod]
        public void LogSourceDoesNotAutoFlush()
        {
            MockFlushSensingTraceListener traceListener = new MockFlushSensingTraceListener();
            List<TraceListener> listeners = new List<TraceListener>(1);
            listeners.Add(traceListener);
            LogSource logSource = new LogSource("name", listeners, SourceLevels.All);
            logSource.AutoFlush = false;
            logSource.TraceData(TraceEventType.Critical, 0, CommonUtil.GetDefaultLogEntry());
            Assert.AreEqual(0, traceListener.flushCalls);
        }
        [TestMethod]
        public void LogSourceDoesAutoFlush()
        {
            MockFlushSensingTraceListener traceListener = new MockFlushSensingTraceListener();
            List<TraceListener> listeners = new List<TraceListener>(1);
            listeners.Add(traceListener);
            LogSource logSource = new LogSource("name", listeners, SourceLevels.All, true);
            logSource.TraceData(TraceEventType.Critical, 0, CommonUtil.GetDefaultLogEntry());
            Assert.AreEqual(1, traceListener.flushCalls);
        }
        [TestMethod]
        public void UpdatesTraceEventCacheOnEachCall()
        {
            MockEventCacheSensingTraceListener traceListener1 = new MockEventCacheSensingTraceListener();
            MockEventCacheSensingTraceListener traceListener2 = new MockEventCacheSensingTraceListener();
            List<TraceListener> traceListeners = new List<TraceListener>(1);
            traceListeners.Add(traceListener1);
            traceListeners.Add(traceListener2);
            LogSource logSource = new LogSource("name", traceListeners, SourceLevels.All);
            logSource.TraceData(TraceEventType.Critical, 0, CommonUtil.GetDefaultLogEntry());
            Assert.AreEqual(traceListener1.dateTime, traceListener2.dateTime);
            Assert.IsTrue(traceListener1.dateTime > default(DateTime));
            DateTime savedDateTime = traceListener1.dateTime;
            Thread.Sleep(100);
            logSource.TraceData(TraceEventType.Critical, 0, CommonUtil.GetDefaultLogEntry());
            Assert.AreEqual(traceListener1.dateTime, traceListener2.dateTime);
            Assert.IsTrue(traceListener1.dateTime > default(DateTime));
            Assert.IsTrue(traceListener1.dateTime > savedDateTime);
        }
        public class MockFlushSensingTraceListener : TraceListener
        {
            public int flushCalls = 0;
            public override void Flush()
            {
                flushCalls++;
            }
            public override void Write(string message) {}
            public override void WriteLine(string message) {}
        }
        public class MockEventCacheSensingTraceListener : TraceListener
        {
            public DateTime dateTime;
            public override void TraceData(TraceEventCache eventCache,
                                           string source,
                                           TraceEventType eventType,
                                           int id,
                                           object data)
            {
                dateTime = eventCache.DateTime;
                base.TraceData(eventCache, source, eventType, id, data);
            }
            public override void Write(string message) {}
            public override void WriteLine(string message) {}
        }
    }
}
