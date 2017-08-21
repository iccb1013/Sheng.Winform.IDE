/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using System.Management;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests
{
    [TestClass]
    public class WMIListenerFixture
    {
        bool wmiLogged;
        string wmiResult;
        string wmiPath = @"\root\EnterpriseLibrary";
        ManagementBaseObject wmiLogEntry;
        void BlockUntilWMIEventArrives()
        {
            bool loop = true;
            int count = 0;
            int timeoutCount = 100;
            while (loop)
            {
                loop = !wmiLogged && (count++ < timeoutCount);
                Thread.Sleep(100);
            }
        }
        void watcher_EventArrived(object sender,
                                  EventArrivedEventArgs args)
        {
            wmiLogged = true;
            wmiResult = args.NewEvent.GetText(TextFormat.Mof);
            wmiLogEntry = args.NewEvent;
        }
        void SendLogEntry(WmiTraceListener listener,
                          LogEntry logEntry)
        {
            ManagementScope scope = new ManagementScope(@"\\." + wmiPath);
            scope.Options.EnablePrivileges = true;
            StringBuilder sb = new StringBuilder("SELECT * FROM ");
            sb.Append("LogEntryV20");
            string query = sb.ToString();
            EventQuery eq = new EventQuery(query);
            using (ManagementEventWatcher watcher = new ManagementEventWatcher(scope, eq))
            {
                watcher.EventArrived += new EventArrivedEventHandler(watcher_EventArrived);
                watcher.Start();
                LogSource source = new LogSource("notfromconfig", SourceLevels.All);
                source.Listeners.Add(listener);
                source.TraceData(TraceEventType.Error, 1, logEntry);
                BlockUntilWMIEventArrives();
                watcher.Stop();
            }
        }
        [TestInitialize]
        public void ResetLogEntryInfo()
        {
            wmiLogged = false;
            wmiResult = string.Empty;
            wmiPath = @"\root\EnterpriseLibrary";
            wmiLogEntry = null;
        }
        [TestMethod]
        public void TestWMIEventOccurred()
        {
            WmiTraceListener listener = new WmiTraceListener();
            LogEntry logEntry = new LogEntry("message", "cat1", 0, 0, TraceEventType.Error, "title", null);
            SendLogEntry(listener, logEntry);
            Assert.IsTrue(wmiLogged);
            Assert.IsTrue(wmiResult.IndexOf("message") > -1);
        }
        [TestMethod]
        public void TestWMIEventWasFiltered()
        {
            WmiTraceListener listener = new WmiTraceListener();
            listener.Filter = new EventTypeFilter(SourceLevels.Off);
            LogEntry logEntry = new LogEntry("message", "cat1", 0, 0, TraceEventType.Error, "title", null);
            SendLogEntry(listener, logEntry);
            Assert.IsFalse(wmiLogged);
            Assert.IsFalse(wmiResult.IndexOf("message") > -1);
        }
        [TestMethod]
        public void TestWMIEventFiredApplyingFilter()
        {
            WmiTraceListener listener = new WmiTraceListener();
            listener.Filter = new EventTypeFilter(SourceLevels.Warning);
            LogEntry logEntry = new LogEntry("message", "cat1", 0, 0, TraceEventType.Error, "title", null);
            SendLogEntry(listener, logEntry);
            Assert.IsTrue(wmiLogged);
            Assert.IsTrue(wmiResult.IndexOf("message") > -1);
        }
        [TestMethod]
        public void AttributeLookupWillNotHaveAnEffect()
        {
            WmiTraceListener listener = new WmiTraceListener();
            listener.Attributes.Add("formatter", "nonexistent");
        }
        [TestMethod]
        public void TestCanGetActivityIdStringWithWMI()
        {
            WmiTraceListener listener = new WmiTraceListener();
            LogEntry logEntry = new LogEntry("message", "cat1", 0, 0, TraceEventType.Error, "title", null);
            Guid logEntryGuid = Guid.NewGuid();
            logEntry.ActivityId = logEntryGuid;
            SendLogEntry(listener, logEntry);
            Assert.IsTrue(wmiLogged);
            Assert.AreEqual(wmiLogEntry.GetPropertyValue("ActivityIdString"), logEntryGuid.ToString());
        }
        [TestMethod]
        public void TestCanGetcategoriesStringsWithWMI()
        {
            WmiTraceListener listener = new WmiTraceListener();
            LogEntry logEntry = new LogEntry("message", new string[] { "cat1", "cat2", "cat3" }, 0, 0, TraceEventType.Error, "title", null);
            SendLogEntry(listener, logEntry);
            Assert.IsTrue(wmiLogged);
            string[] categoriesStrings = (string[])wmiLogEntry.GetPropertyValue("CategoriesStrings");
            Assert.AreEqual(categoriesStrings.Length, logEntry.Categories.Count);
        }
        [TestMethod]
        public void TestLoggingACustomLogEntry()
        {
            WmiTraceListener listener = new WmiTraceListener();
            MyCustomLogEntry logEntry = new MyCustomLogEntry();
            logEntry.MyName = "Enterprise Library Tester";
            SendLogEntry(listener, logEntry);
            Assert.IsTrue(wmiLogged);
            Assert.AreEqual(wmiLogEntry.GetPropertyValue("MyName"), logEntry.MyName);
        }
    }
}
