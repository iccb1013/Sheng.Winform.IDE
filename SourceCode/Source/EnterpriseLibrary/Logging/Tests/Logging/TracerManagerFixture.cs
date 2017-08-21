/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    [TestClass]
    public class TracerManagerFixture
    {
        [TestMethod]
        public void GetTracerFromTraceManagerWithNoInstrumentation()
        {
            MockTraceListener.Reset();
            LogSource source = new LogSource("tracesource", SourceLevels.All);
            source.Listeners.Add(new MockTraceListener());
            List<LogSource> traceSources = new List<LogSource>(new LogSource[] { source });
            LogWriter lg = new LogWriter(new List<ILogFilter>(), new List<LogSource>(), source, null, new LogSource("errors"), "default", true, false);
            TraceManager tm = new TraceManager(lg);
            Assert.IsNotNull(tm);
            using (tm.StartTrace("testoperation"))
            {
                Assert.AreEqual(1, MockTraceListener.Entries.Count);
            }
            Assert.AreEqual(2, MockTraceListener.Entries.Count);
        }
        [TestMethod]
        public void GetTracerFromTraceManagerWithInstrumentationEnabled()
        {
            MockTraceListener.Reset();
            LogSource source = new LogSource("tracesource", SourceLevels.All);
            source.Listeners.Add(new MockTraceListener());
            List<LogSource> traceSources = new List<LogSource>(new LogSource[] { source });
            LogWriter lg = new LogWriter(new List<ILogFilter>(), new List<LogSource>(), source, null, new LogSource("errors"), "default", true, false);
            TracerInstrumentationListener instrumentationListener = new TracerInstrumentationListener(true);
            TraceManager tm = new TraceManager(lg, instrumentationListener);
            Assert.IsNotNull(tm);
            using (tm.StartTrace("testoperation"))
            {
                Assert.AreEqual(1, MockTraceListener.Entries.Count);
            }
            Assert.AreEqual(2, MockTraceListener.Entries.Count);
        }
    }
}
