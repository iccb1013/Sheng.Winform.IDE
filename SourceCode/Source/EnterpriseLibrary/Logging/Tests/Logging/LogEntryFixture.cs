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
using System.Globalization;
using System.Text;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    [TestClass]
    public class LogEntryFixture
    {
        static LogEntry log;

        [TestInitialize]
        public void SetUp()
        {
            Logger.Reset();
            MockTraceListener.Reset();
            log = new LogEntry();
        }

        [TestCleanup]
        public void TearDown()
        {
            MockTraceListener.Reset();
        }

        [TestMethod]
        public void GetSetProperties()
        {
            string stringVal = "my test string";
            int counter = 0;

            log.Categories = new string[] { stringVal + counter };
            Assert.IsTrue(log.Categories.Contains(stringVal + counter));
            counter++;
            log.Categories = new string[] { "" };
            Assert.IsTrue(log.Categories.Contains(""));

            log.EventId = counter;
            Assert.AreEqual(counter, log.EventId);
            log.EventId = -1234;
            Assert.AreEqual(-1234, log.EventId);

            log.Message = stringVal + counter;
            Assert.AreEqual(stringVal + counter, log.Message);
            counter++;
            log.Message = "";
            Assert.AreEqual("", log.Message);

            log.Priority = counter;
            Assert.AreEqual(counter, log.Priority);
            log.Priority = -1234;
            Assert.AreEqual(-1234, log.Priority);

            log.Severity = TraceEventType.Warning;
            Assert.AreEqual(TraceEventType.Warning, log.Severity);
            counter++;
            log.Severity = TraceEventType.Information;
            Assert.AreEqual(TraceEventType.Information, log.Severity);

            log.TimeStamp = DateTime.MinValue;
            Assert.AreEqual(DateTime.MinValue, log.TimeStamp);
            counter++;
            log.TimeStamp = DateTime.MaxValue;
            Assert.AreEqual(DateTime.MaxValue, log.TimeStamp);

            log.Title = stringVal + counter;
            Assert.AreEqual(stringVal + counter, log.Title);
            counter++;
            log.Title = "";
            Assert.AreEqual("", log.Title);
        }

        [TestMethod]
        public void ConfirmSeverityValuesCanBeReadAsStrings()
        {
            log.Severity = TraceEventType.Critical;
            Assert.AreEqual("Critical", log.LoggedSeverity);
        }

        [TestMethod]
        public void ConfirmThatIntrinsicPropertiesAreReadWrite()
        {
            log.TimeStamp = DateTime.MinValue;
            Assert.AreEqual(DateTime.MinValue, log.TimeStamp);

            log.MachineName = "MachineName";
            Assert.AreEqual("MachineName", log.MachineName);

            log.AppDomainName = "AppDomainName";
            Assert.AreEqual("AppDomainName", log.AppDomainName);

            log.ProcessId = "ProcessId";
            Assert.AreEqual("ProcessId", log.ProcessId);

            log.ProcessName = "ProcessName";
            Assert.AreEqual("ProcessName", log.ProcessName);

            log.ManagedThreadName = "ManagedThreadId";
            Assert.AreEqual("ManagedThreadId", log.ManagedThreadName);

            log.Win32ThreadId = "Win32ThreadId";
            Assert.AreEqual("Win32ThreadId", log.Win32ThreadId);
        }

        [TestMethod]
        public void GetSetExtendedPropertiesProperty()
        {
            Dictionary<string, object> hash = new Dictionary<string, object>();
            hash["key1"] = "val1";
            hash["key2"] = "val2";

            log.ExtendedProperties = hash;
            Assert.AreEqual(2, log.ExtendedProperties.Count);
            Assert.AreEqual("val1", log.ExtendedProperties["key1"]);
            Assert.AreEqual("val2", log.ExtendedProperties["key2"]);
        }

        [TestMethod]
        public void CanSetExtendedPropertiesWithoutProvidingHashTableFirst()
        {
            log.ExtendedProperties["foo"] = "bar";
            Assert.AreEqual(1, log.ExtendedProperties.Count);
            Assert.AreEqual("bar", log.ExtendedProperties["foo"]);
        }

        [TestMethod]
        public void GetSetTimeStampString()
        {
            string expected = DateTime.Parse("12/31/9999 11:59:59 PM", CultureInfo.InvariantCulture).ToString();
            log.TimeStamp = DateTime.MaxValue;
            Assert.AreEqual(expected, log.TimeStampString);
        }

        [TestMethod]
        public void ConfirmIntrinsicPropertiesContainExpectedValues()
        {
            // NOTE: There is no way to test Timestamp

            Assert.AreEqual(Environment.MachineName, log.MachineName);
            Assert.AreEqual(AppDomain.CurrentDomain.FriendlyName, log.AppDomainName);
            Assert.AreEqual(NativeMethods.GetCurrentProcessId().ToString(), log.ProcessId);
            Assert.AreEqual(GetExpectedProcessName(), log.ProcessName);
            Assert.AreEqual(Thread.CurrentThread.Name, log.ManagedThreadName);
            Assert.AreEqual(NativeMethods.GetCurrentThreadId().ToString(), log.Win32ThreadId);
        }

        [TestMethod]
        public void ReuseLogEntryForMultipleWrites()
        {
            log.Categories = new string[] { "MockCategoryOne" };

            log.Message = "apples";
            Logger.Write(log);
            Assert.AreEqual("apples", MockTraceListener.LastEntry.Message);

            log.Message = "oranges";
            Logger.Write(log);
            Assert.AreEqual("oranges", MockTraceListener.LastEntry.Message);
        }

        [TestMethod]
        public void ConfirmCloneWorksWithICloneableExtendedProperties()
        {
            LogEntry entry = CommonUtil.GetDefaultLogEntry();
            entry.ExtendedProperties = new Dictionary<string, object>();
            entry.ExtendedProperties.Add("one", "two");

            LogEntry entry2 = entry.Clone() as LogEntry;
            Assert.IsNotNull(entry2);
            Assert.AreEqual("two", entry2.ExtendedProperties["one"]);
        }

        [TestMethod]
        public void ConfirmCloningNonCloneableExtendedPropertiesReplacesExtendedPropertiesCollection()
        {
            LogEntry originalLogEntry = CommonUtil.GetDefaultLogEntry();
            Dictionary<string, object> extendedProperties = new Dictionary<string, object>();
            originalLogEntry.ExtendedProperties = extendedProperties;
            originalLogEntry.ExtendedProperties.Add("one", "two");

            LogEntry clonedLogEntry = originalLogEntry.Clone() as LogEntry;
            Assert.IsNotNull(clonedLogEntry);
            Assert.IsFalse(ReferenceEquals(extendedProperties, clonedLogEntry.ExtendedProperties));
            Assert.AreEqual(1, clonedLogEntry.ExtendedProperties.Count);
        }

        [TestMethod]
        public void ClonedLogEntryHasEqualButNotSameErrorMessagesIfSourceLogEntryHasNotNullErrorMessages()
        {
            LogEntry entry = CommonUtil.GetDefaultLogEntry();
            entry.AddErrorMessage("message 1");
            entry.AddErrorMessage("message 2");

            LogEntry entry2 = entry.Clone() as LogEntry;
            Assert.IsNotNull(entry2);
            Assert.IsNotNull(entry2.ErrorMessages);
            Assert.AreEqual(entry.ErrorMessages, entry2.ErrorMessages);
            Assert.IsTrue(entry.ErrorMessages.Equals(entry2.ErrorMessages));

            entry2.AddErrorMessage("message 3");
            Assert.IsFalse(entry.ErrorMessages.Equals(entry2.ErrorMessages));
        }

        [TestMethod]
        public void ClonedLogEntryHasNullErrorMessagesIfSourceLogEntryHasNullErrorMessages()
        {
            LogEntry entry = CommonUtil.GetDefaultLogEntry();

            LogEntry entry2 = entry.Clone() as LogEntry;
            Assert.IsNotNull(entry2);
            Assert.IsNull(entry2.ErrorMessages);
            Assert.AreEqual(entry.ErrorMessages, entry2.ErrorMessages);
        }

        [TestMethod]
        public void ClonedEntryGetsActivityId()
        {
            LogEntry entry = CommonUtil.GetDefaultLogEntry();

            LogEntry entry2 = entry.Clone() as LogEntry;
            Assert.AreEqual(entry.ActivityId, entry2.ActivityId);

            entry.ActivityId = Guid.NewGuid();
            LogEntry entry3 = entry.Clone() as LogEntry;
            Assert.AreEqual(entry.ActivityId, entry3.ActivityId);
        }

        [TestMethod]
        public void ClonedEntryGetsClonedCategories()
        {
            string[] categories = new string[] { "cat1", "cat2", "cat3" };

            LogEntry entry = CommonUtil.GetDefaultLogEntry();
            entry.Categories = new List<string>(categories);

            LogEntry entry2 = entry.Clone() as LogEntry;

            Assert.IsNotNull(entry2);
            Assert.IsFalse(entry.Categories == entry2.Categories);
            Assert.AreEqual(entry.Categories.Count, entry2.Categories.Count);
            foreach (string category in categories)
            {
                Assert.IsTrue(entry2.Categories.Contains(category));
            }
        }

        [TestMethod]
        public void SingleCategoryConstructorBuildsCorrectCategoryCollection()
        {
            LogEntry entry = new LogEntry("message", "category", 0, 0, TraceEventType.Error, "title", null);

            Assert.AreEqual(1, entry.Categories.Count);
            Assert.IsTrue(entry.Categories.Contains("category"));
        }

        [TestMethod]
        public void EventLogMultipleTimesTest()
        {
            LogEntry entry = null;

            for (int i = 0; i < 4; i++)
            {
                entry = new LogEntry();
                entry.Message = "Test multiple category";
                entry.Severity = TraceEventType.Stop;
                entry.Categories.Add("FormattedCategory");
            }

            Assert.AreEqual(1, entry.Categories.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorCallWithNullMessageThrows()
        {
            new LogEntry(null, "category", 0, 0, TraceEventType.Error, "title", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorCallWithNullCategoryThrows()
        {
            new LogEntry("message", (string)null, 0, 0, TraceEventType.Error, "title", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorCallWithNullCategoriesListThrows()
        {
            new LogEntry("message", (ICollection<string>)null, 0, 0, TraceEventType.Error, "title", null);
        }

        [TestMethod]
        public void LogEntryCloneMethodTest()
        {
            LogEntry entry = new LogEntry();
            entry.ActivityId = new Guid("AAAABBBBCCCCDDDDAAAABBBBCCCCDDDD");
            entry.AddErrorMessage("LogEntryCloneMethodTest exception");
            entry.Categories.Add("Error");
            entry.EventId = 1;
            entry.ExtendedProperties.Add("key1", "value1");
            entry.Message = "To test the cloning method";
            entry.Priority = 10;
            entry.Severity = TraceEventType.Critical;
            entry.Title = "LogEntryCloneMethodTest";

            LogEntry clonedEntry = (LogEntry)entry.Clone();

            Assert.AreEqual(entry.ActivityIdString, clonedEntry.ActivityIdString);
            Assert.AreEqual(entry.Categories.Count, clonedEntry.Categories.Count);
            Assert.AreEqual(entry.EventId, clonedEntry.EventId);
            Assert.AreEqual(entry.ExtendedProperties.Count, clonedEntry.ExtendedProperties.Count);
            Assert.AreEqual(entry.Message, clonedEntry.Message);
            Assert.AreEqual(entry.Priority, clonedEntry.Priority);
            Assert.AreEqual(entry.ProcessId, clonedEntry.ProcessId);
            Assert.AreEqual(entry.ProcessName, clonedEntry.ProcessName);
            Assert.AreEqual(entry.Severity, clonedEntry.Severity);
            Assert.AreEqual(entry.TimeStamp, clonedEntry.TimeStamp);
            Assert.AreEqual(entry.TimeStampString, clonedEntry.TimeStampString);
            Assert.AreEqual(entry.Title, clonedEntry.Title);
            Assert.AreEqual(entry.Win32ThreadId, clonedEntry.Win32ThreadId);
            Assert.AreEqual(entry.ManagedThreadName, clonedEntry.ManagedThreadName);
            Assert.AreEqual(entry.MachineName, clonedEntry.MachineName);
            Assert.AreEqual(entry.ErrorMessages, clonedEntry.ErrorMessages);
            Assert.AreEqual(entry.AppDomainName, clonedEntry.AppDomainName);

            clonedEntry.Categories.Add("Debug");
            clonedEntry.ExtendedProperties.Add("key2", "value2");
            clonedEntry.ActivityId = new Guid("EEEEFFFFEEEEFFFFEEEEFFFFEEEEFFFF");

            Assert.IsTrue(entry.Categories.Count == 1);
            Assert.IsTrue(clonedEntry.Categories.Count == 2);
            Assert.IsTrue(entry.ExtendedProperties.Count == 1);
            Assert.IsTrue(clonedEntry.ExtendedProperties.Count == 2);
            Assert.IsFalse(entry.ActivityIdString == clonedEntry.ActivityIdString);
        }

        string GetExpectedProcessName()
        {
            StringBuilder buffer = new StringBuilder(1024);
            NativeMethods.GetModuleFileName(NativeMethods.GetModuleHandle(null), buffer, buffer.Capacity);
            return buffer.ToString();
        }
    }
}
