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
using System.Configuration;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Storage;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SysConfig = System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    [TestClass]
    public class TracerFixture
    {
        readonly Guid referenceGuid = new Guid("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        readonly Guid overwriteGuid = new Guid("BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB");
        readonly Guid testActivityId1 = new Guid("1CF75C3C-127F-41c9-97B2-FBEDB64F974A");
        readonly Guid testActivityId2 = new Guid("5D352811-28DF-4baf-BD18-5997FE1260A3");
        const string performanceCounterCategory = "Enterprise Library Logging Counters";
        const string performanceCounterTracesPerSecondName = "Trace Operations Started/sec";
        const string performanceCounterAvgTraceDuration = "Avg. Trace Execution Time";
        const string operation = "operation";
        const string nestedOperation = "nested operation";
        const string badOperation = "bad operation";
        const string category = "category";

        [TestMethod]
        public void UsingTracerWritesEntryAndExitMessages()
        {
            MockTraceListener.Reset();
            Guid currentActivityId = Guid.Empty;

            using (new Tracer(operation))
            {
                currentActivityId = Trace.CorrelationManager.ActivityId;
                Assert.AreEqual(1, MockTraceListener.Entries.Count);
                AssertLogEntryIsValid(MockTraceListener.LastEntry, Tracer.startTitle, operation, currentActivityId, true);
                MockTraceListener.Reset();
            }

            Assert.AreEqual(1, MockTraceListener.Entries.Count);
            AssertLogEntryIsValid(MockTraceListener.LastEntry, Tracer.endTitle, operation, currentActivityId, false);
        }

        [TestMethod]
        public void UsingTracerWritesEntryAndExitMessagesIgnoring_NoActivityIdCheck()
        {
            MockTraceListener.Reset();

            using (new Tracer(operation))
            {
                Assert.AreEqual(1, MockTraceListener.Entries.Count);
                MockTraceListener.Reset();
            }

            Assert.AreEqual(1, MockTraceListener.Entries.Count);
        }

        [TestMethod]
        public void TraceListenerWritesFullTypeNameAndMethodName()
        {
            string MyTypeAndMethodName = "Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TracerFixture.TraceListenerWritesFullTypeNameAndMethodName";

            MockTraceListener.Reset();

            using (new Tracer(operation))
            {
                Assert.AreEqual(1, MockTraceListener.Entries.Count);
                MockTraceListener.Reset();
            }

            Assert.AreEqual(1, MockTraceListener.Entries.Count);
            Assert.IsTrue(MockTraceListener.Entries[0].Message.Contains(MyTypeAndMethodName));
        }

        void SetTracingFlag(bool tracingEnabled)
        {
            ConfigurationChangeFileWatcher.SetDefaultPollDelayInMilliseconds(100);
            SystemConfigurationSource.ResetImplementation(true);
            Logger.Reset();

            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            LoggingSettings loggingSettings = config.GetSection(LoggingSettings.SectionName) as LoggingSettings;
            loggingSettings.TracingEnabled = tracingEnabled;
            config.Save();

            ConfigurationManager.RefreshSection(LoggingSettings.SectionName);
            SystemConfigurationSource.Implementation.ConfigSourceChanged(string.Empty);

            Thread.Sleep(200);
        }

        [TestMethod]
        public void UsingTracerDoesNotWriteWhenDisabled()
        {
            SystemConfigurationSource.ResetImplementation(false);

            //turn tracing off
            SetTracingFlag(false);

            MockTraceListener.Reset();
            Guid currentActivityId = Guid.Empty;

            using (new Tracer(operation))
            {
                currentActivityId = Trace.CorrelationManager.ActivityId;
                Assert.AreEqual(0, MockTraceListener.Entries.Count);
            }

            Assert.AreEqual(0, MockTraceListener.Entries.Count);

            //turn tracing back on
            SetTracingFlag(true);
        }

        [TestMethod]
        public void TracerUpdatesStackAfterEntryAndExit()
        {
            Assert.AreEqual(0, Trace.CorrelationManager.LogicalOperationStack.Count);

            using (new Tracer(operation))
            {
                Assert.AreEqual(1, Trace.CorrelationManager.LogicalOperationStack.Count);
                Assert.AreEqual(operation, Trace.CorrelationManager.LogicalOperationStack.Peek());
            }

            Assert.AreEqual(0, Trace.CorrelationManager.LogicalOperationStack.Count);
        }

        [TestMethod]
        public void NestedTracerUpdatesStackAfterEntryAndExit()
        {
            Assert.AreEqual(0, Trace.CorrelationManager.LogicalOperationStack.Count);

            using (new Tracer(operation))
            {
                Assert.AreEqual(1, Trace.CorrelationManager.LogicalOperationStack.Count);
                Assert.AreEqual(operation, Trace.CorrelationManager.LogicalOperationStack.Peek());

                using (new Tracer(nestedOperation))
                {
                    Assert.AreEqual(2, Trace.CorrelationManager.LogicalOperationStack.Count);
                    Assert.AreEqual(nestedOperation, Trace.CorrelationManager.LogicalOperationStack.Peek());
                }

                Assert.AreEqual(1, Trace.CorrelationManager.LogicalOperationStack.Count);
                Assert.AreEqual(operation, Trace.CorrelationManager.LogicalOperationStack.Peek());
            }

            Assert.AreEqual(0, Trace.CorrelationManager.LogicalOperationStack.Count);
        }

        [TestMethod]
        public void NewTracerWithoutActivityWillCreateNewActivityIdIfThereIsNoExistingActivityId()
        {
            Trace.CorrelationManager.ActivityId = Guid.Empty;

            Assert.AreEqual(Guid.Empty, Trace.CorrelationManager.ActivityId);
            using (new Tracer(operation))
            {
                Assert.IsFalse(Guid.Empty == Trace.CorrelationManager.ActivityId);
            }
        }

        [TestMethod]
        public void NewTracerWithoutActivityWillKeepExistingActivityId()
        {
            Trace.CorrelationManager.ActivityId = referenceGuid;

            Assert.AreEqual(referenceGuid, Trace.CorrelationManager.ActivityId);
            using (new Tracer(operation))
            {
                Assert.AreEqual(referenceGuid, Trace.CorrelationManager.ActivityId);
            }

            Assert.AreEqual(referenceGuid, Trace.CorrelationManager.ActivityId);
        }

        [TestMethod]
        public void NewTracerWithActivityWillOverwriteExistingActivityId()
        {
            Trace.CorrelationManager.ActivityId = referenceGuid;

            Assert.AreEqual(referenceGuid, Trace.CorrelationManager.ActivityId);
            using (new Tracer(operation, overwriteGuid))
            {
                Assert.AreEqual(overwriteGuid, Trace.CorrelationManager.ActivityId);
            }
        }

        [TestMethod]
        public void NestedTracerInheritsActivityId()
        {
            Trace.CorrelationManager.ActivityId = Guid.Empty;

            Assert.AreEqual(Guid.Empty, Trace.CorrelationManager.ActivityId);

            using (new Tracer(operation))
            {
                Assert.IsFalse(Guid.Empty == Trace.CorrelationManager.ActivityId);
                Guid outerActivityId = Trace.CorrelationManager.ActivityId;

                using (new Tracer(nestedOperation))
                {
                    Assert.AreEqual(outerActivityId, Trace.CorrelationManager.ActivityId);
                }
            }
        }

        [TestMethod]
        public void NestedTracerOverwritesActivityId()
        {
            Trace.CorrelationManager.ActivityId = Guid.Empty;

            Assert.AreEqual(Guid.Empty, Trace.CorrelationManager.ActivityId);

            using (new Tracer(operation))
            {
                Assert.IsFalse(Guid.Empty == Trace.CorrelationManager.ActivityId);
                Guid outerActivityId = Trace.CorrelationManager.ActivityId;

                using (new Tracer(nestedOperation, referenceGuid))
                {
                    Assert.AreEqual(referenceGuid, Trace.CorrelationManager.ActivityId);
                }

                Assert.AreEqual(referenceGuid, Trace.CorrelationManager.ActivityId);
            }
        }

        [TestMethod]
        public void UseTracingWithOwnLogWriter()
        {
            MockTraceListener.Reset();

            LogSource source = new LogSource("tracesource", SourceLevels.All);
            source.Listeners.Add(new MockTraceListener());

            List<LogSource> traceSources = new List<LogSource>(new LogSource[] { source });
            LogWriter logWriter = new LogWriter(new List<ILogFilter>(), new List<LogSource>(), source, null, new LogSource("errors"), "default", true, false);

            using (Tracer tracer = new Tracer("testoperation", logWriter, null))
            {
                Assert.AreEqual(1, MockTraceListener.Entries.Count);
            }

            Assert.AreEqual(2, MockTraceListener.Entries.Count);
        }

        [TestMethod]
        public void UseTracingWithDisabledLoggerDoesntWrite()
        {
            MockTraceListener.Reset();

            LogSource source = new LogSource("tracesource", SourceLevels.All);
            source.Listeners.Add(new MockTraceListener());

            List<LogSource> traceSources = new List<LogSource>(new LogSource[] { source });
            LogWriter logWriter = new LogWriter(new List<ILogFilter>(), new List<LogSource>(), source, null, new LogSource("errors"), "default", false, false);

            using (Tracer tracer = new Tracer("testoperation", logWriter, null))
            {
                Assert.AreEqual(0, MockTraceListener.Entries.Count);
            }

            Assert.AreEqual(0, MockTraceListener.Entries.Count);
        }

        [TestMethod]
        public void LoggedMessagesDuringTracerAddsCategoryIds()
        {
            MockTraceListener.Reset();

            using (new Tracer(operation))
            {
                Assert.AreEqual(1, MockTraceListener.Entries.Count);
                Assert.AreEqual(1, MockTraceListener.LastEntry.Categories.Count);
                Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(operation));
                MockTraceListener.Reset();

                Logger.Write("message", category);
                Assert.AreEqual(1, MockTraceListener.Entries.Count);
                Assert.AreEqual(2, MockTraceListener.LastEntry.Categories.Count);
                Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(operation));
                Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(category));
                MockTraceListener.Reset();
            }
            Assert.AreEqual(1, MockTraceListener.Entries.Count);
            Assert.AreEqual(1, MockTraceListener.LastEntry.Categories.Count);
            Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(operation));
        }

        [TestMethod]
        public void LoggedMessagesDuringNestedTracerAddsAllCategoryIds()
        {
            MockTraceListener.Reset();

            using (new Tracer(operation))
            {
                Assert.AreEqual(1, MockTraceListener.Entries.Count);
                Assert.AreEqual(1, MockTraceListener.LastEntry.Categories.Count);
                Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(operation));
                MockTraceListener.Reset();

                using (new Tracer(nestedOperation))
                {
                    Assert.AreEqual(1, MockTraceListener.Entries.Count);
                    Assert.AreEqual(2, MockTraceListener.LastEntry.Categories.Count);
                    Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(operation));
                    Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(nestedOperation));
                    MockTraceListener.Reset();

                    Logger.Write("message", category);
                    Assert.AreEqual(1, MockTraceListener.Entries.Count);
                    Assert.AreEqual(3, MockTraceListener.LastEntry.Categories.Count);
                    Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(operation));
                    Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(nestedOperation));
                    Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(category));
                    MockTraceListener.Reset();
                }
                Assert.AreEqual(1, MockTraceListener.Entries.Count);
                Assert.AreEqual(2, MockTraceListener.LastEntry.Categories.Count);
                Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(operation));
                Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(nestedOperation));
                MockTraceListener.Reset();
            }
            Assert.AreEqual(1, MockTraceListener.Entries.Count);
            Assert.AreEqual(1, MockTraceListener.LastEntry.Categories.Count);
            Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(operation));
        }

        [TestMethod]
        public void TraceCategoriesAreConsideredInFilters()
        {
            MockTraceListener.Reset();

            using (new Tracer(badOperation))
            {
                Assert.AreEqual(0, MockTraceListener.Entries.Count);
                MockTraceListener.Reset();
            }

            Assert.AreEqual(0, MockTraceListener.Entries.Count);
        }

        [TestMethod]
        public void TracingCanBeTurnedOnAndOff()
        {
            //turn tracing off
            SetTracingFlag(false);

            MockTraceListener.Reset();
            Guid currentActivityId = Guid.Empty;

            using (new Tracer(operation))
            {
                currentActivityId = Trace.CorrelationManager.ActivityId;
                Assert.AreEqual(0, MockTraceListener.Entries.Count);
            }

            Assert.AreEqual(0, MockTraceListener.Entries.Count);
            MockTraceListener.Reset();

            //turn tracing back on
            SetTracingFlag(true);

            currentActivityId = Guid.Empty;

            using (new Tracer(operation))
            {
                currentActivityId = Trace.CorrelationManager.ActivityId;
                Assert.AreEqual(1, MockTraceListener.Entries.Count);
                AssertLogEntryIsValid(MockTraceListener.LastEntry, Tracer.startTitle, operation, currentActivityId, true);
                MockTraceListener.Reset();
            }

            Assert.AreEqual(1, MockTraceListener.Entries.Count);
            AssertLogEntryIsValid(MockTraceListener.LastEntry, Tracer.endTitle, operation, currentActivityId, false);
            MockTraceListener.Reset();
        }

        //[TestMethod]
        //public void ActivityIdsAreUniqueOnEachThread()
        //{
        //    // This will put two events into the sink using nestedOperation and testActivityId2
        //    using (new Tracer(nestedOperation, testActivityId2))
        //    {
        //        // This will put two events into the sink using operation and testActivityId1
        //        CrossThreadTestRunner t = new CrossThreadTestRunner(new ThreadStart(this.DoOtherThreadWork));
        //        t.Run();

        //        // Confirm that the Tracer on this thread has the expected activityID
        //        Assert.AreEqual(testActivityId2, Trace.CorrelationManager.ActivityId);
        //    }

        //    //Assert.AreEqual(Guid.Empty, Trace.CorrelationManager.ActivityId);

        //    Assert.AreEqual(4, MockTraceListener.Entries.Count);
        //    AssertLogEntryIsValid(MockTraceListener.Entries[0], Tracer.startTitle, nestedOperation, testActivityId2, true);
        //    AssertLogEntryIsValid(MockTraceListener.Entries[1], Tracer.startTitle, operation, testActivityId1, true);
        //    AssertLogEntryIsValid(MockTraceListener.Entries[2], Tracer.endTitle, operation, testActivityId1, false);
        //    AssertLogEntryIsValid(MockTraceListener.Entries[3], Tracer.endTitle, nestedOperation, testActivityId2, false);
        //}

        [TestMethod]
        public void ThreadLaunchedWithinTracerInheritsLogicalOperationsStack() {}

        [TestMethod]
        public void ThreadsDoNotShareLogicalOperationsStack()
        {
            CrossThreadTestRunner t = new CrossThreadTestRunner(new ThreadStart(DoOtherThreadWork));
            t.Run();

            Assert.AreEqual(0, Trace.CorrelationManager.LogicalOperationStack.Count);
            Assert.IsFalse(testActivityId1 == Trace.CorrelationManager.ActivityId);
        }

        [TestMethod]
        public void AverageTraceDurationCounterIsUpdatedAfterTracing()
        {
            using (Tracer tracer = new Tracer(operation))
            {
                int i = new Random().Next();
            }
            int valueAfterTracing = GetCounterValue(performanceCounterAvgTraceDuration, operation);

            Assert.IsFalse(valueAfterTracing == 0);
        }

        [TestMethod]
        public void NumberOfTracePerSecondIsUpdatedAfterTracing()
        {
            int initialValue = GetCounterValue(performanceCounterTracesPerSecondName, operation);

            using (Tracer tracer = new Tracer(operation))
            {
                int i = new Random().Next();
            }
            int valueAfterTracing = GetCounterValue(performanceCounterTracesPerSecondName, operation);

            Assert.IsTrue(valueAfterTracing == 1 + initialValue);
        }

        [TestMethod]
        public void NumberOfTracePerSecondIsNotUpdatedForOtherInstances()
        {
            string otherOperationName = "otherOperation";
            int initialValue = GetCounterValue(performanceCounterTracesPerSecondName, operation);

            using (Tracer tracer = new Tracer(operation))
            {
                int i = new Random().Next();
            }
            int valueAfterTracing = GetCounterValue(performanceCounterTracesPerSecondName, operation);

            Assert.IsTrue(valueAfterTracing == 1 + initialValue);

            using (Tracer tracer = new Tracer(otherOperationName))
            {
                int i = new Random().Next();
            }
            valueAfterTracing = GetCounterValue(performanceCounterTracesPerSecondName, operation);

            Assert.IsTrue(valueAfterTracing == 1 + initialValue);
        }

        [TestMethod]
        public void InstrumentationIsTurnedOffWhenPassingNoConfigurationSource()
        {
            int initialValue = GetCounterValue(performanceCounterTracesPerSecondName, operation);

            LogWriter logWriter = EnterpriseLibraryFactory.BuildUp<LogWriter>();

            using (Tracer tracer = new Tracer(operation, logWriter, null))
            {
                int i = new Random().Next();
            }
            int valueAfterTracing = GetCounterValue(performanceCounterTracesPerSecondName, operation);

            Assert.IsTrue(valueAfterTracing == initialValue);
        }

        [TestMethod]
        public void PerformanceCountersAreNotIncrementedWhenTracingIsTurnedOff()
        {
            //turn tracing off
            SetTracingFlag(false);

            int initialValue = GetCounterValue(performanceCounterTracesPerSecondName, operation);

            using (new Tracer(operation))
            {
                int i = new Random().Next();
            }
            int valueAfterTracing = GetCounterValue(performanceCounterTracesPerSecondName, operation);

            Assert.IsTrue(valueAfterTracing == initialValue);

            //turn tracing back on
            SetTracingFlag(true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullWriterToTracerThrows()
        {
            using (new Tracer(operation, null, ConfigurationSourceFactory.Create()))
            {
                int i = new Random().Next();
            }
        }

        [TestMethod]
        public void NonStringElementsInLogicalOperationStackAreIgnored()
        {
            SetTracingFlag(true);
            MockTraceListener.Reset();
            Guid currentActivityId = Guid.Empty;
            object nonStringLogicalOperation = new object();
            object stringLogicalOperation = "another operation";
            object nullLogicalOperation = null;

            try
            {
                Trace.CorrelationManager.LogicalOperationStack.Push(nonStringLogicalOperation);
                Trace.CorrelationManager.LogicalOperationStack.Push(stringLogicalOperation);
                Trace.CorrelationManager.LogicalOperationStack.Push(nullLogicalOperation);

                using (new Tracer(operation))
                {
                    currentActivityId = Trace.CorrelationManager.ActivityId;
                    Assert.AreEqual(1, MockTraceListener.Entries.Count);
                }
            }
            finally
            {
                Trace.CorrelationManager.LogicalOperationStack.Pop();
                Trace.CorrelationManager.LogicalOperationStack.Pop();
                Trace.CorrelationManager.LogicalOperationStack.Pop();
            }

            Assert.AreEqual(2, MockTraceListener.Entries.Count);

            LogEntry logEntry = MockTraceListener.LastEntry;
            Assert.AreEqual(2, logEntry.Categories.Count);
            Assert.IsTrue(logEntry.Categories.Contains(operation));
            Assert.IsTrue(logEntry.Categories.Contains((string)stringLogicalOperation));
        }

        int GetCounterValue(string counterName,
                            string operation)
        {
            string instanceName = new AppDomainNameFormatter().CreateName(operation);
            return (int)CommonUtil.GetPerformanceCounterValue(performanceCounterCategory, instanceName, counterName);
        }

        void DoOtherThreadWork()
        {
            using (Tracer tracer = new Tracer(operation, testActivityId1))
            {
                Assert.IsNotNull(tracer);
                Assert.AreEqual(testActivityId1, Trace.CorrelationManager.ActivityId);
            }
        }

        void AssertLogEntryIsValid(LogEntry entry,
                                   string expectedTitle,
                                   string expectedCategory,
                                   Guid expectedActivityId,
                                   bool isStartMessage)
        {
            Assert.AreEqual(expectedTitle, entry.Title);

            Assert.AreEqual(expectedActivityId, entry.ActivityId);

            Assert.AreEqual(Tracer.eventId, entry.EventId);
            Assert.AreEqual(Tracer.priority, entry.Priority);
            Assert.AreEqual(Tracer.eventId, entry.EventId);
            Assert.IsTrue(entry.Categories.Contains(expectedCategory));

            if (isStartMessage)
            {
                Assert.AreEqual(TraceEventType.Start, entry.Severity);
                AssertMessageIsValidStartMessage(entry.Message);
            }
            else
            {
                Assert.AreEqual(TraceEventType.Stop, entry.Severity);
                AssertMessageIsValidEndMessage(entry.Message);
            }
        }

        void AssertMessageIsValidStartMessage(string message)
        {
            Assert.IsNotNull(message);

            string format = Resources.Tracer_StartMessageFormat;
            string pattern = ConvertFormatToRegex(format);

            Regex re = new Regex(pattern);

            Assert.IsTrue(re.IsMatch(message));

            MatchCollection matches = re.Matches(message);
            foreach (Match match in matches)
            {
                Assert.IsNotNull(match.Value);
                Assert.IsTrue(match.Value.ToString().Length > 0);
            }
        }

        void AssertMessageIsValidEndMessage(string message)
        {
            Assert.IsNotNull(message);

            string format = Resources.Tracer_EndMessageFormat;
            string pattern = ConvertFormatToRegex(format);

            Regex re = new Regex(pattern);

            Assert.IsTrue(re.IsMatch(message));

            MatchCollection matches = re.Matches(message);
            foreach (Match match in matches)
            {
                Assert.IsNotNull(match.Value);
                Assert.IsTrue(match.Value.ToString().Length > 0);
            }
        }

        string ConvertFormatToRegex(string format)
        {
            string pattern = format;
            pattern = pattern.Replace("(", @"\(");
            pattern = pattern.Replace(")", @"\)");
            pattern = Regex.Replace(pattern, @"\{[0-9]\}", "(.*?)");
            return pattern;
        }
    }
}
