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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    /// <summary>
    /// Summary description for LogWriterFixture
    /// </summary>
    [TestClass]
    public class LogWriterFixture
    {
        const string TotalLoggingEventsRaised = "Total Logging Events Raised";
        const string TotalTraceListenerEntriesWritten = "Total Trace Listener Entries Written";
        const string counterCategoryName = "Enterprise Library Logging Counters";

        const string applicationInstanceName = "applicationInstanceName";
        const string instanceName = "Foo";
        string formattedInstanceName;

        EnterpriseLibraryPerformanceCounter totalLoggingEventsRaised;
        EnterpriseLibraryPerformanceCounter totalTraceListenerEntriesWritten;
        EnterpriseLibraryPerformanceCounter totalTraceOperationsStartedCounter;
        AppDomainNameFormatter nameFormatter;
        LoggingInstrumentationListener listener;
        TracerInstrumentationListener tracerListener;

        [TestInitialize]
        public void SetUp()
        {
            nameFormatter = new AppDomainNameFormatter(applicationInstanceName);
            listener = new LoggingInstrumentationListener(instanceName, true, true, true, applicationInstanceName);
            tracerListener = new TracerInstrumentationListener(true);
            formattedInstanceName = nameFormatter.CreateName(instanceName);
            totalLoggingEventsRaised = new EnterpriseLibraryPerformanceCounter(counterCategoryName, TotalLoggingEventsRaised, formattedInstanceName);
            totalTraceListenerEntriesWritten = new EnterpriseLibraryPerformanceCounter(counterCategoryName, TotalTraceListenerEntriesWritten, formattedInstanceName);

            // Use AppDomainFriendlyName for the instance name
            nameFormatter = new AppDomainNameFormatter();
            formattedInstanceName = nameFormatter.CreateName(instanceName);

            totalTraceOperationsStartedCounter = new EnterpriseLibraryPerformanceCounter(TracerInstrumentationListener.counterCategoryName, TracerInstrumentationListener.TotalTraceOperationsStartedCounterName, formattedInstanceName);
        }

        [TestMethod]
        public void TotalTraceOperationsStartedCounterIncremented()
        {
            tracerListener.TracerOperationStarted("foo");

            long expected = 1;
            Assert.AreEqual(expected, totalTraceOperationsStartedCounter.Value);
        }

        [TestMethod]
        public void TotalLoggingEventsRaisedCounterIncremented()
        {
            listener.LoggingEventRaised(this, EventArgs.Empty);

            long expected = 1;
            Assert.AreEqual(expected, totalLoggingEventsRaised.Value);
        }

        [TestMethod]
        public void TotalTraceListenerEntriesWrittenIncremented()
        {
            listener.TraceListenerEntryWritten(this, EventArgs.Empty);

            long expected = 1;
            Assert.AreEqual(expected, totalTraceListenerEntriesWritten.Value);
        }

        [TestMethod]
        public void CanCreateLogWriterUsingConstructor()
        {
            LogWriter writer = new LogWriter(new List<ILogFilter>(), new Dictionary<string, LogSource>(), new LogSource("errors"), "default");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreationOfLogWriterUsingConstructorWithNullFiltersThrows()
        {
            LogWriter writer = new LogWriter(null, new Dictionary<string, LogSource>(), new LogSource("errors"), "default");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreationOfLogWriterUsingConstructorWithNullTraceSourcesThrows()
        {
            LogWriter writer = new LogWriter(new List<ILogFilter>(), (IDictionary<string, LogSource>)null, new LogSource("errors"), "default");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreationOfLogWriterUsingConstructorWithNullErrorsTraceSourceThrows()
        {
            LogWriter writer = new LogWriter(new List<ILogFilter>(), new Dictionary<string, LogSource>(), null, "default");
        }

        [TestMethod]
        public void CanCreateLogWriterUsingFactory()
        {
            LogWriter writer = EnterpriseLibraryFactory.BuildUp<LogWriter>();
            Assert.IsNotNull(writer);
        }

        [TestMethod]
        public void CanGetLogFiltersByType()
        {
            ICollection<ILogFilter> filters = new List<ILogFilter>();

            ICollection<string> categories = new List<string>();
            categories.Add("cat1");
            categories.Add("cat2");
            categories.Add("cat3");
            categories.Add("cat4");
            filters.Add(new CategoryFilter("category", categories, CategoryFilterMode.AllowAllExceptDenied));
            filters.Add(new PriorityFilter("priority", 100));
            filters.Add(new LogEnabledFilter("enable", true));

            LogWriter writer = new LogWriter(filters, new Dictionary<string, LogSource>(), new LogSource("errors"), "default");
            CategoryFilter categoryFilter = writer.GetFilter<CategoryFilter>();
            PriorityFilter priorityFilter = writer.GetFilter<PriorityFilter>();
            LogEnabledFilter enabledFilter = writer.GetFilter<LogEnabledFilter>();

            Assert.IsNotNull(categoryFilter);
            Assert.AreEqual(4, categoryFilter.CategoryFilters.Count);
            Assert.IsNotNull(priorityFilter);
            Assert.AreEqual(100, priorityFilter.MinimumPriority);
            Assert.IsNotNull(enabledFilter);
            Assert.IsTrue(enabledFilter.Enabled);
        }

        [TestMethod]
        public void CanGetLogFiltersByNameAndType()
        {
            ICollection<ILogFilter> filters = new List<ILogFilter>();

            ICollection<string> categories = new List<string>();
            categories.Add("cat1");
            categories.Add("cat2");
            categories.Add("cat3");
            categories.Add("cat4");
            filters.Add(new CategoryFilter("category", categories, CategoryFilterMode.AllowAllExceptDenied));
            filters.Add(new PriorityFilter("priority1", 100));
            filters.Add(new LogEnabledFilter("enable", true));
            filters.Add(new PriorityFilter("priority2", 200));

            LogWriter writer = new LogWriter(filters, new Dictionary<string, LogSource>(), new LogSource("errors"), "default");
            PriorityFilter priorityFilter1 = writer.GetFilter<PriorityFilter>("priority1");
            PriorityFilter priorityFilter2 = writer.GetFilter<PriorityFilter>("priority2");

            Assert.IsNotNull(priorityFilter1);
            Assert.AreEqual(100, priorityFilter1.MinimumPriority);
            Assert.IsNotNull(priorityFilter2);
            Assert.AreEqual(200, priorityFilter2.MinimumPriority);
        }

        [TestMethod]
        public void VerfiyTraceListenerPerfCounter()
        {
            Logger.Reset();

            string counterName = "Trace Listener Entries Written/sec";
            int initialCount = GetCounterValue(counterName);
            LogEntry entry = new LogEntry();
            entry.Severity = TraceEventType.Error;
            entry.Message = "";
            entry.Categories.Add("FormattedCategory");
            Logger.Write(entry);
            int loggedCount = GetCounterValue(counterName);
            Assert.IsTrue((loggedCount - initialCount) == 1);
            initialCount = loggedCount;
            Logger.Write(entry);
            loggedCount = GetCounterValue(counterName);
            Assert.IsTrue((loggedCount - initialCount) == 1);
        }

        int GetCounterValue(string counterName)
        {
            string categoryName = "Enterprise Library Logging Counters";
            string instanceName = new AppDomainNameFormatter().CreateName("Total");
            if (PerformanceCounterCategory.InstanceExists(instanceName, categoryName))
            {
                using (PerformanceCounter counter = new PerformanceCounter())
                {
                    counter.CategoryName = categoryName;
                    counter.CounterName = counterName;
                    counter.InstanceName = instanceName;
                    return (int)counter.RawValue;
                }
            }
            return 0;
        }

        [TestMethod]
        public void WmiFiredWhenDeliveryToErrorSourceFails()
        {
            TraceListener badTraceListener = new BadTraceListener(new Exception("test exception"));
            LogSource badSource = new LogSource("badSource");
            badSource.Listeners.Add(badTraceListener);

            Dictionary<string, LogSource> logSources = new Dictionary<string, LogSource>();
            logSources.Add("foo", badSource);

            LogWriter writer = new LogWriter(new List<ILogFilter>(), logSources, badSource, "foo");
            new ReflectionInstrumentationBinder().Bind(writer.GetInstrumentationEventProvider(), new LoggingInstrumentationListener(false, false, true, "applicationInstanceName"));

            using (WmiEventWatcher eventListener = new WmiEventWatcher(1))
            {
                writer.Write(CommonUtil.GetDefaultLogEntry());

                eventListener.WaitForEvents();

                Assert.AreEqual(1, eventListener.EventsReceived.Count);
                Assert.AreEqual("LoggingFailureLoggingErrorEvent", eventListener.EventsReceived[0].ClassPath.ClassName);
                string exceptionMessage = (string)eventListener.EventsReceived[0].GetPropertyValue("ExceptionMessage");
                Assert.IsTrue(-1 != exceptionMessage.IndexOf("test exception"));
                Assert.IsNotNull(eventListener.EventsReceived[0].GetPropertyValue("ErrorMessage"));
            }
        }

        [TestMethod]
        public void EventLogWrittenWhenDeliveryToErrorSourceFails()
        {
            TraceListener badTraceListener = new BadTraceListener(new Exception("test exception"));
            LogSource badSource = new LogSource("badSource");
            badSource.Listeners.Add(badTraceListener);

            Dictionary<string, LogSource> logSources = new Dictionary<string, LogSource>();
            logSources.Add("foo", badSource);

            LogWriter writer = new LogWriter(new List<ILogFilter>(), logSources, badSource, "foo");
            new ReflectionInstrumentationBinder().Bind(writer.GetInstrumentationEventProvider(), new LoggingInstrumentationListener(false, true, false, "applicationInstanceName"));

            writer.Write(CommonUtil.GetDefaultLogEntry());

            string lastEventLogEntry = CommonUtil.GetLastEventLogEntry();
            Assert.IsTrue(-1 != lastEventLogEntry.IndexOf("test exception"));
        }

        [TestMethod]
        public void CanGetLogFiltersByName()
        {
            ICollection<ILogFilter> filters = new List<ILogFilter>();

            ICollection<string> categories = new List<string>();
            categories.Add("cat1");
            categories.Add("cat2");
            categories.Add("cat3");
            categories.Add("cat4");
            filters.Add(new CategoryFilter("category", categories, CategoryFilterMode.AllowAllExceptDenied));
            filters.Add(new PriorityFilter("priority1", 100));
            filters.Add(new LogEnabledFilter("enable", true));
            filters.Add(new PriorityFilter("priority2", 200));

            LogWriter writer = new LogWriter(filters, new Dictionary<string, LogSource>(), new LogSource("errors"), "default");
            ILogFilter categoryFilter = writer.GetFilter("category");
            ILogFilter priorityFilter = writer.GetFilter("priority2");

            Assert.IsNotNull(categoryFilter);
            Assert.AreEqual(typeof(CategoryFilter), categoryFilter.GetType());
            Assert.IsNotNull(priorityFilter);
            Assert.AreEqual(typeof(PriorityFilter), priorityFilter.GetType());
        }
    }
}
