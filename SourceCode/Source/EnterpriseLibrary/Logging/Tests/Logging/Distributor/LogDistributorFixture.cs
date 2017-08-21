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
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Tests
{
    [TestClass]
    public class LogWriterFixture
    {
        static LogWriter logWriter;
        static ICollection<ILogFilter> emptyFilters = new List<ILogFilter>(0);
        static LogSource emptyTraceSource;
        const string originalMessage = "---- ORIGINAL MESSAGE ----";

        [TestInitialize]
        public void SetUp()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);

            logWriter = EnterpriseLibraryFactory.BuildUp<LogWriter>();
            MockTraceListener.Reset();
            ErrorsMockTraceListener.Reset();

            emptyTraceSource = new LogSource("none");
            if (emptyTraceSource.Listeners.Count == 1)
                emptyTraceSource.Listeners.RemoveAt(0);
        }

        [TestCleanup]
        public void TearDown()
        {
            MockTraceListener.Reset();
            ErrorsMockTraceListener.Reset();
        }

        [TestMethod]
        public void ProcessLogsSyncConstructor()
        {
            Assert.IsNotNull(logWriter);
        }

        [TestMethod]
        public void SendMessageToOneSink()
        {
            string[] categories = new string[] { "MockCategoryOne" };

            //  build a message
            LogEntry msg = CommonUtil.GetDefaultLogEntry();
            msg.Categories = categories;
            msg.Severity = TraceEventType.Error;

            logWriter.Write(msg);
            logWriter.Dispose();

            Assert.AreEqual(1, MockTraceListener.Entries.Count);
            Assert.AreEqual(CommonUtil.MsgBody, ((LogEntry)MockTraceListener.LastEntry).Message, "Body");
            Assert.AreEqual(CommonUtil.MsgTitle, ((LogEntry)MockTraceListener.LastEntry).Title, "Header");
            Assert.AreEqual(categories, ((LogEntry)MockTraceListener.LastEntry).Categories, "Categories");
            Assert.AreEqual(CommonUtil.MsgEventID, ((LogEntry)MockTraceListener.LastEntry).EventId, "EventID");
            Assert.AreEqual(TraceEventType.Error, ((LogEntry)MockTraceListener.LastEntry).Severity, "Severity");
        }

        [TestMethod]
        public void SendMessageToManySinks()
        {
            string[] categories = new string[] { "MockCategoryMany" };

            //  build a message
            LogEntry msg = CommonUtil.GetDefaultLogEntry();
            msg.Categories = categories;
            msg.Severity = TraceEventType.Error;

            logWriter.Write(msg);
            logWriter.Dispose();

            Assert.AreEqual(2, MockTraceListener.Entries.Count);
            Assert.AreEqual(CommonUtil.MsgBody, ((LogEntry)MockTraceListener.LastEntry).Message, "Body");
            Assert.AreEqual(CommonUtil.MsgBody, ((LogEntry)MockTraceListener.LastEntry).Message, "Body");
        }

        [TestMethod]
        public void SendManyMessagesToManyFlatFileAndEventLog()
        {
            int numberOfWrites = 4;
            EventLog log = CommonUtil.GetCustomEventLog();
            log.Clear();
            int numberOfEventLogs = log.Entries.Count; //should be zero after the clear
            string fileName = @"FlatFileTestOutput\EntLibLog.txt";
            string header = "----------header------------------";

            if (File.Exists(fileName))
                File.Delete(fileName);

            string[] categories = new string[] { "RealCategoryMany" };

            //  build a message
            LogEntry msg = CommonUtil.GetDefaultLogEntry();
            msg.Categories = categories;
            msg.Severity = TraceEventType.Error;

            //set autoflush = false
            Trace.AutoFlush = false;

            for (int writeLoop = 0; writeLoop < numberOfWrites; writeLoop++)
                logWriter.Write(msg);
            logWriter.Dispose();

            int headersFound = NumberOfHeaders(fileName, header);

            Assert.AreEqual(numberOfWrites, headersFound);
            Assert.AreEqual(numberOfWrites, log.Entries.Count - numberOfEventLogs);
        }

        [TestMethod]
        public void SendManyMessagesToManyFlatFileAndEventLogWithAutoFlush()
        {
            int numberOfWrites = 4;
            EventLog log = CommonUtil.GetCustomEventLog();
            log.Clear();
            int numberOfEventLogs = log.Entries.Count; //should be zero after the clear
            string fileName = @"FlatFileTestOutput\EntLibLog.txt";
            string header = "----------header------------------";

            if (File.Exists(fileName))
                File.Delete(fileName);

            string[] categories = new string[] { "RealCategoryMany" };

            //  build a message
            LogEntry msg = CommonUtil.GetDefaultLogEntry();
            msg.Categories = categories;
            msg.Severity = TraceEventType.Error;

            //set autoflush = false
            Trace.AutoFlush = true;

            for (int writeLoop = 0; writeLoop < numberOfWrites; writeLoop++)
                logWriter.Write(msg);
            logWriter.Dispose();

            int headersFound = NumberOfHeaders(fileName, header);

            Assert.AreEqual(numberOfWrites, headersFound);
            Assert.AreEqual(numberOfWrites, log.Entries.Count - numberOfEventLogs);
        }

        [TestMethod]
        public void LogWriterCanGetConfiguredCategories()
        {
            LogWriter logWriter = EnterpriseLibraryFactory.BuildUp<LogWriter>();

            LogSource source = null;

            foreach (string key in logWriter.TraceSources.Keys)
            {
                logWriter.TraceSources.TryGetValue(key, out source);
                Assert.IsNotNull(source, key);
                Assert.AreEqual(key, source.Name);
            }

            source = null;
            logWriter.TraceSources.TryGetValue("AppTest", out source);
            logWriter.Dispose();

            Assert.IsNotNull(source);
        }

        // tests for matching
        [TestMethod]
        public void CanFindMatchingCategories()
        {
            Dictionary<string, LogSource> traceSources = new Dictionary<string, LogSource>();
            traceSources.Add("newcat1", new LogSource("newcat1"));
            traceSources.Add("newcat2", new LogSource("newcat2"));
            traceSources.Add("newcat3", new LogSource("newcat3"));
            traceSources.Add("newcat4", new LogSource("newcat4"));
            LogWriter logWriter = new LogWriter(emptyFilters, traceSources, new LogSource("errors"), "default");

            string[] categories = new string[] { "newcat1", "newcat2", "newcat5", "newcat6" };
            LogEntry logEntry = new LogEntry();
            logEntry.Categories = categories;
            IEnumerable<LogSource> matchingTraceSources = logWriter.GetMatchingTraceSources(logEntry);

            logWriter.Dispose();

            Dictionary<string, LogSource> matchingTraceSourcesDictionary = new Dictionary<string, LogSource>();
            foreach (LogSource traceSource in matchingTraceSources)
            {
                matchingTraceSourcesDictionary.Add(traceSource.Name, traceSource);
            }

            Assert.AreEqual(2, matchingTraceSourcesDictionary.Count);
            Assert.IsTrue(matchingTraceSourcesDictionary.ContainsKey(categories[0]));
            Assert.IsTrue(matchingTraceSourcesDictionary.ContainsKey(categories[1]));
            Assert.IsFalse(matchingTraceSourcesDictionary.ContainsKey(categories[2]));
        }

        [TestMethod]
        public void UsesDefaultTraceSourceWhenThereAreMissingCategoriesAndDefaultIsConfigured()
        {
            Dictionary<string, LogSource> traceSources = new Dictionary<string, LogSource>();
            traceSources.Add("newcat1", new LogSource("newcat1"));
            traceSources.Add("newcat2", new LogSource("newcat2"));
            traceSources.Add("newcat3", new LogSource("newcat3"));
            traceSources.Add("newcat4", new LogSource("newcat4"));
            LogSource notProcessedTraceSource = new LogSource("default");
            LogSource errorsTraceSource = new LogSource("errors", SourceLevels.All);
            errorsTraceSource.Listeners.Add(new ErrorsMockTraceListener());
            LogWriter logWriter = new LogWriter(emptyFilters, traceSources, emptyTraceSource, notProcessedTraceSource, errorsTraceSource, "default", false, false);

            string[] categories = new string[] { "newcat1", "newcat2", "newcat5", "newcat6" };
            LogEntry logEntry = new LogEntry();
            logEntry.Categories = categories;
            IEnumerable<LogSource> matchingTraceSources = logWriter.GetMatchingTraceSources(logEntry);

            logWriter.Dispose();

            Dictionary<string, LogSource> matchingTraceSourcesDictionary = new Dictionary<string, LogSource>();
            foreach (LogSource traceSource in matchingTraceSources)
            {
                matchingTraceSourcesDictionary.Add(traceSource.Name, traceSource);
            }

            Assert.AreEqual(3, matchingTraceSourcesDictionary.Count);
            Assert.IsTrue(matchingTraceSourcesDictionary.ContainsKey(categories[0]));
            Assert.IsTrue(matchingTraceSourcesDictionary.ContainsKey(categories[1]));
            Assert.IsTrue(matchingTraceSourcesDictionary.ContainsKey(notProcessedTraceSource.Name));
            Assert.AreEqual(0, ErrorsMockTraceListener.Entries.Count);
        }

        [TestMethod]
        public void DoesNotUseDefaultTraceSourceWhenThereAreNotMissingCategoriesAndDefaultIsConfigured()
        {
            Dictionary<string, LogSource> traceSources = new Dictionary<string, LogSource>();
            traceSources.Add("newcat1", new LogSource("newcat1"));
            traceSources.Add("newcat2", new LogSource("newcat2"));
            traceSources.Add("newcat3", new LogSource("newcat3"));
            traceSources.Add("newcat4", new LogSource("newcat4"));
            LogSource notProcessedTraceSource = new LogSource("default");
            LogSource errorsTraceSource = new LogSource("errors", SourceLevels.All);
            errorsTraceSource.Listeners.Add(new ErrorsMockTraceListener());
            LogWriter logWriter = new LogWriter(emptyFilters, traceSources, emptyTraceSource, notProcessedTraceSource, errorsTraceSource, "default", false, false);

            string[] categories = new string[] { "newcat1", "newcat2" };
            LogEntry logEntry = new LogEntry();
            logEntry.Categories = categories;
            IEnumerable<LogSource> matchingTraceSources = logWriter.GetMatchingTraceSources(logEntry);

            logWriter.Dispose();
            Dictionary<string, LogSource> matchingTraceSourcesDictionary = new Dictionary<string, LogSource>();
            foreach (LogSource traceSource in matchingTraceSources)
            {
                matchingTraceSourcesDictionary.Add(traceSource.Name, traceSource);
            }

            Assert.AreEqual(2, matchingTraceSourcesDictionary.Count);
            Assert.IsTrue(matchingTraceSourcesDictionary.ContainsKey(categories[0]));
            Assert.IsTrue(matchingTraceSourcesDictionary.ContainsKey(categories[1]));
            Assert.IsFalse(matchingTraceSourcesDictionary.ContainsKey(notProcessedTraceSource.Name));
            Assert.AreEqual(0, ErrorsMockTraceListener.Entries.Count);
        }

        [TestMethod]
        public void ReportsMissingCategoriesWhenThereAreMissingCategoriesAndDefaultIsNotConfigured()
        {
            Dictionary<string, LogSource> traceSources = new Dictionary<string, LogSource>();
            traceSources.Add("newcat1", new LogSource("newcat1"));
            traceSources.Add("newcat2", new LogSource("newcat2"));
            traceSources.Add("newcat3", new LogSource("newcat3"));
            traceSources.Add("newcat4", new LogSource("newcat4"));
            LogSource errorsTraceSource = new LogSource("errors", SourceLevels.All);
            errorsTraceSource.Listeners.Add(new ErrorsMockTraceListener());
            LogWriter logWriter = new LogWriter(emptyFilters, traceSources, emptyTraceSource, emptyTraceSource, errorsTraceSource, "default", false, true);

            string[] categories = new string[] { "newcat1", "newcat2", "newcat5", "newcat6" };
            LogEntry logEntry = new LogEntry();
            logEntry.Categories = categories;
            IEnumerable<LogSource> matchingTraceSources = logWriter.GetMatchingTraceSources(logEntry);

            logWriter.Dispose();
            Dictionary<string, LogSource> matchingTraceSourcesDictionary = new Dictionary<string, LogSource>();
            foreach (LogSource traceSource in matchingTraceSources)
            {
                matchingTraceSourcesDictionary.Add(traceSource.Name, traceSource);
            }

            Assert.AreEqual(2, matchingTraceSourcesDictionary.Count);
            Assert.AreEqual(1, ErrorsMockTraceListener.Entries.Count);
            Assert.IsTrue(MatchTemplate(ErrorsMockTraceListener.LastEntry.Message, Resources.MissingCategories));
        }

        [TestMethod]
        public void DoesNotReportMissingCategoriesWhenThereAreMissingCategoriesAndDefaultIsNotConfiguredButLogWarningFlagIsFalse()
        {
            Dictionary<string, LogSource> traceSources = new Dictionary<string, LogSource>();
            traceSources.Add("newcat1", new LogSource("newcat1"));
            traceSources.Add("newcat2", new LogSource("newcat2"));
            traceSources.Add("newcat3", new LogSource("newcat3"));
            traceSources.Add("newcat4", new LogSource("newcat4"));
            LogSource errorsTraceSource = new LogSource("errors", SourceLevels.All);
            errorsTraceSource.Listeners.Add(new ErrorsMockTraceListener());
            LogWriter logWriter = new LogWriter(emptyFilters, traceSources, emptyTraceSource, emptyTraceSource, errorsTraceSource, "default", false, false);

            string[] categories = new string[] { "newcat1", "newcat2", "newcat5", "newcat6" };
            LogEntry logEntry = new LogEntry();
            logEntry.Categories = categories;
            IEnumerable<LogSource> matchingTraceSources = logWriter.GetMatchingTraceSources(logEntry);

            logWriter.Dispose();
            Dictionary<string, LogSource> matchingTraceSourcesDictionary = new Dictionary<string, LogSource>();
            foreach (LogSource traceSource in matchingTraceSources)
            {
                matchingTraceSourcesDictionary.Add(traceSource.Name, traceSource);
            }

            Assert.AreEqual(2, matchingTraceSourcesDictionary.Count);
            Assert.AreEqual(0, ErrorsMockTraceListener.Entries.Count);
        }

        [TestMethod]
        public void DoesNotReportMissingCategoriesWhenThereAreNotMissingCategoriesAndDefaultIsNotConfigured()
        {
            Dictionary<string, LogSource> traceSources = new Dictionary<string, LogSource>();
            traceSources.Add("newcat1", new LogSource("newcat1"));
            traceSources.Add("newcat2", new LogSource("newcat2"));
            traceSources.Add("newcat3", new LogSource("newcat3"));
            traceSources.Add("newcat4", new LogSource("newcat4"));
            LogSource errorsTraceSource = new LogSource("errors", SourceLevels.All);
            errorsTraceSource.Listeners.Add(new ErrorsMockTraceListener());
            LogWriter logWriter = new LogWriter(emptyFilters, traceSources, emptyTraceSource, emptyTraceSource, errorsTraceSource, "default", false, false);

            string[] categories = new string[] { "newcat1", "newcat2" };
            LogEntry logEntry = new LogEntry();
            logEntry.Categories = categories;
            IEnumerable<LogSource> matchingTraceSources = logWriter.GetMatchingTraceSources(logEntry);

            logWriter.Dispose();
            Dictionary<string, LogSource> matchingTraceSourcesDictionary = new Dictionary<string, LogSource>();
            foreach (LogSource traceSource in matchingTraceSources)
            {
                matchingTraceSourcesDictionary.Add(traceSource.Name, traceSource);
            }

            Assert.AreEqual(2, matchingTraceSourcesDictionary.Count);
            Assert.AreEqual(0, ErrorsMockTraceListener.Entries.Count);
        }

        [TestMethod]
        public void UsesMandatoryTraceSourceIfAllCategoriesMatch()
        {
            Dictionary<string, LogSource> traceSources = new Dictionary<string, LogSource>();
            traceSources.Add("newcat1", new LogSource("newcat1"));
            traceSources.Add("newcat2", new LogSource("newcat2"));
            traceSources.Add("newcat3", new LogSource("newcat3"));
            traceSources.Add("newcat4", new LogSource("newcat4"));
            LogSource mandatoryTraceSource = new LogSource("mandatory");
            LogSource errorsTraceSource = new LogSource("errors", SourceLevels.All);
            LogWriter logWriter = new LogWriter(emptyFilters, traceSources, mandatoryTraceSource, emptyTraceSource, errorsTraceSource, "default", false, false);

            string[] categories = new string[] { "newcat1", "newcat2" };
            LogEntry logEntry = new LogEntry();
            logEntry.Categories = categories;
            IEnumerable<LogSource> matchingTraceSources = logWriter.GetMatchingTraceSources(logEntry);

            logWriter.Dispose();
            Dictionary<string, LogSource> matchingTraceSourcesDictionary = new Dictionary<string, LogSource>();
            foreach (LogSource traceSource in matchingTraceSources)
            {
                matchingTraceSourcesDictionary.Add(traceSource.Name, traceSource);
            }

            Assert.AreEqual(3, matchingTraceSourcesDictionary.Count);
            Assert.IsTrue(matchingTraceSourcesDictionary.ContainsKey(categories[0]));
            Assert.IsTrue(matchingTraceSourcesDictionary.ContainsKey(categories[1]));
            Assert.IsTrue(matchingTraceSourcesDictionary.ContainsKey(mandatoryTraceSource.Name));
            Assert.AreEqual(0, ErrorsMockTraceListener.Entries.Count);
        }

        [TestMethod]
        public void UsedMandatoryTraceSourceIfAllCategoriesAreMissing()
        {
            Dictionary<string, LogSource> traceSources = new Dictionary<string, LogSource>();
            traceSources.Add("newcat1", new LogSource("newcat1"));
            traceSources.Add("newcat2", new LogSource("newcat2"));
            traceSources.Add("newcat3", new LogSource("newcat3"));
            traceSources.Add("newcat4", new LogSource("newcat4"));
            LogSource mandatoryTraceSource = new LogSource("mandatory");
            LogSource errorsTraceSource = new LogSource("errors", SourceLevels.All);
            errorsTraceSource.Listeners.Add(new ErrorsMockTraceListener());
            LogWriter logWriter = new LogWriter(emptyFilters, traceSources, mandatoryTraceSource, emptyTraceSource, errorsTraceSource, "default", false, false);

            string[] categories = new string[] { "newcat5", "newcat6" };
            LogEntry logEntry = new LogEntry();
            logEntry.Categories = categories;
            IEnumerable<LogSource> matchingTraceSources = logWriter.GetMatchingTraceSources(logEntry);

            logWriter.Dispose();
            Dictionary<string, LogSource> matchingTraceSourcesDictionary = new Dictionary<string, LogSource>();
            foreach (LogSource traceSource in matchingTraceSources)
            {
                matchingTraceSourcesDictionary.Add(traceSource.Name, traceSource);
            }

            Assert.AreEqual(1, matchingTraceSourcesDictionary.Count);
            Assert.IsTrue(matchingTraceSourcesDictionary.ContainsKey(mandatoryTraceSource.Name));
            Assert.AreEqual(0, ErrorsMockTraceListener.Entries.Count);
        }

        [TestMethod]
        public void UsedMandatoryTraceSourceIfThereAreMatchingAndMissingCategories()
        {
            Dictionary<string, LogSource> traceSources = new Dictionary<string, LogSource>();
            traceSources.Add("newcat1", new LogSource("newcat1"));
            traceSources.Add("newcat2", new LogSource("newcat2"));
            traceSources.Add("newcat3", new LogSource("newcat3"));
            traceSources.Add("newcat4", new LogSource("newcat4"));
            LogSource mandatoryTraceSource = new LogSource("mandatory");
            LogSource errorsTraceSource = new LogSource("errors", SourceLevels.All);
            LogWriter logWriter = new LogWriter(emptyFilters, traceSources, mandatoryTraceSource, emptyTraceSource, errorsTraceSource, "default", false, false);

            string[] categories = new string[] { "newcat1", "newcat2", "newcat5", "newcat6" };
            LogEntry logEntry = new LogEntry();
            logEntry.Categories = categories;
            IEnumerable<LogSource> matchingTraceSources = logWriter.GetMatchingTraceSources(logEntry);

            logWriter.Dispose();
            Dictionary<string, LogSource> matchingTraceSourcesDictionary = new Dictionary<string, LogSource>();
            foreach (LogSource traceSource in matchingTraceSources)
            {
                matchingTraceSourcesDictionary.Add(traceSource.Name, traceSource);
            }

            Assert.AreEqual(3, matchingTraceSourcesDictionary.Count);
            Assert.IsTrue(matchingTraceSourcesDictionary.ContainsKey(categories[0]));
            Assert.IsTrue(matchingTraceSourcesDictionary.ContainsKey(categories[1]));
            Assert.IsTrue(matchingTraceSourcesDictionary.ContainsKey(mandatoryTraceSource.Name));
            Assert.AreEqual(0, ErrorsMockTraceListener.Entries.Count);
        }

        [TestMethod]
        public void UsesOnlyMandatoryTraceSourceIfThereAreMissingCategoriesAndDefaultIsConfigured()
        {
            Dictionary<string, LogSource> traceSources = new Dictionary<string, LogSource>();
            traceSources.Add("newcat1", new LogSource("newcat1"));
            traceSources.Add("newcat2", new LogSource("newcat2"));
            traceSources.Add("newcat3", new LogSource("newcat3"));
            traceSources.Add("newcat4", new LogSource("newcat4"));
            LogSource mandatoryTraceSource = new LogSource("mandatory");
            LogSource defaultTraceSource = new LogSource("default");
            LogSource errorsTraceSource = new LogSource("errors", SourceLevels.All);
            errorsTraceSource.Listeners.Add(new ErrorsMockTraceListener());
            LogWriter logWriter =
                new LogWriter(emptyFilters, traceSources, mandatoryTraceSource, defaultTraceSource, errorsTraceSource, "default", false, false);

            string[] categories = new string[] { "newcat1", "newcat2", "newcat5", "newcat6" };
            LogEntry logEntry = new LogEntry();
            logEntry.Categories = categories;
            IEnumerable<LogSource> matchingTraceSources = logWriter.GetMatchingTraceSources(logEntry);

            logWriter.Dispose();
            Dictionary<string, LogSource> matchingTraceSourcesDictionary = new Dictionary<string, LogSource>();
            foreach (LogSource traceSource in matchingTraceSources)
            {
                matchingTraceSourcesDictionary.Add(traceSource.Name, traceSource);
            }

            Assert.AreEqual(3, matchingTraceSourcesDictionary.Count);
            Assert.IsTrue(matchingTraceSourcesDictionary.ContainsKey(categories[0]));
            Assert.IsTrue(matchingTraceSourcesDictionary.ContainsKey(categories[1]));
            Assert.IsTrue(matchingTraceSourcesDictionary.ContainsKey(mandatoryTraceSource.Name));
            Assert.AreEqual(0, ErrorsMockTraceListener.Entries.Count);
        }

        [TestMethod]
        public void UsesDefaultTraceSourceIfThereAreMissingCategoriesAndDefaultIsConfiguredAndMandatoryIsNotConfigured()
        {
            Dictionary<string, LogSource> traceSources = new Dictionary<string, LogSource>();
            traceSources.Add("newcat1", new LogSource("newcat1"));
            traceSources.Add("newcat2", new LogSource("newcat2"));
            traceSources.Add("newcat3", new LogSource("newcat3"));
            traceSources.Add("newcat4", new LogSource("newcat4"));
            LogSource mandatoryTraceSource = null;
            LogSource defaultTraceSource = new LogSource("default");
            LogSource errorsTraceSource = new LogSource("errors", SourceLevels.All);
            errorsTraceSource.Listeners.Add(new ErrorsMockTraceListener());
            LogWriter logWriter =
                new LogWriter(emptyFilters, traceSources, mandatoryTraceSource, defaultTraceSource, errorsTraceSource, "default", false, false);

            string[] categories = new string[] { "newcat1", "newcat2", "newcat5", "newcat6" };
            LogEntry logEntry = new LogEntry();
            logEntry.Categories = categories;
            IEnumerable<LogSource> matchingTraceSources = logWriter.GetMatchingTraceSources(logEntry);

            logWriter.Dispose();
            Dictionary<string, LogSource> matchingTraceSourcesDictionary = new Dictionary<string, LogSource>();
            foreach (LogSource traceSource in matchingTraceSources)
            {
                matchingTraceSourcesDictionary.Add(traceSource.Name, traceSource);
            }

            Assert.AreEqual(3, matchingTraceSourcesDictionary.Count);
            Assert.IsTrue(matchingTraceSourcesDictionary.ContainsKey(categories[0]));
            Assert.IsTrue(matchingTraceSourcesDictionary.ContainsKey(categories[1]));
            Assert.IsTrue(matchingTraceSourcesDictionary.ContainsKey(defaultTraceSource.Name));
            Assert.AreEqual(0, ErrorsMockTraceListener.Entries.Count);
        }

        [TestMethod]
        public void ErrorWhileCheckingFiltersLogsWarningAndLogsOriginalMessage()
        {
            LogSource errorsTraceSource = new LogSource("errors", SourceLevels.All);
            errorsTraceSource.Listeners.Add(new ErrorsMockTraceListener());

            LogSource loggingTraceSource = new LogSource("logging", SourceLevels.All);
            loggingTraceSource.Listeners.Add(new MockTraceListener());

            ILogFilter failingFilter = new ExceptionThrowingLogFilter("failing");

            LogWriter writer =
                new LogWriter(new ILogFilter[] { failingFilter },
                              new LogSource[] { loggingTraceSource },
                              errorsTraceSource,
                              "default");

            LogEntry logEntry = new LogEntry();
            logEntry.Message = originalMessage;
            logEntry.Severity = TraceEventType.Critical;
            logEntry.Categories = new string[] { "logging" };

            writer.Write(logEntry);

            writer.Dispose();

            Assert.AreEqual(1, ErrorsMockTraceListener.Entries.Count);
            Assert.AreEqual(TraceEventType.Error, ErrorsMockTraceListener.LastEntry.Severity);
            Assert.IsTrue(MatchTemplate(ErrorsMockTraceListener.LastEntry.Message, Resources.FilterEvaluationFailed));
            Assert.AreEqual(1, MockTraceListener.Entries.Count);
            Assert.AreSame(logEntry, MockTraceListener.LastEntry);
        }

        [TestMethod]
        public void ErrorWhileSendingToTraceSourceLogsWarningAndLogsOriginalMessageToNonFailingTraceSources()
        {
            LogSource errorsTraceSource = new LogSource("errors", SourceLevels.All);
            errorsTraceSource.Listeners.Add(new ErrorsMockTraceListener());

            LogSource failingTraceSource = new LogSource("failing", SourceLevels.All);
            failingTraceSource.Listeners.Add(new ExceptionThrowingMockTraceListener());
            LogSource loggingTraceSource = new LogSource("logging", SourceLevels.All);
            loggingTraceSource.Listeners.Add(new MockTraceListener());

            LogWriter writer =
                new LogWriter(new ILogFilter[0],
                              new LogSource[] { failingTraceSource, loggingTraceSource },
                              errorsTraceSource,
                              "default");

            LogEntry logEntry = new LogEntry();
            logEntry.Message = originalMessage;
            logEntry.Severity = TraceEventType.Critical;
            logEntry.Categories = new string[] { "failing", "logging" };

            writer.Write(logEntry);

            writer.Dispose();
            Assert.AreEqual(1, ErrorsMockTraceListener.Entries.Count);
            Assert.AreEqual(TraceEventType.Error, ErrorsMockTraceListener.LastEntry.Severity);
            Assert.IsTrue(MatchTemplate(ErrorsMockTraceListener.LastEntry.Message, Resources.TraceSourceFailed));
            Assert.AreEqual(1, MockTraceListener.Entries.Count);
            Assert.AreSame(logEntry, MockTraceListener.LastEntry);
        }

        [TestMethod]
        public void NoErrorsWhileSendingToTraceSourceOnlyLogsOriginalMessageToCategoryTraceSources()
        {
            LogSource errorsTraceSource = new LogSource("errors", SourceLevels.All);
            errorsTraceSource.Listeners.Add(new ErrorsMockTraceListener());

            LogSource failingTraceSource = new LogSource("logging", SourceLevels.All);
            failingTraceSource.Listeners.Add(new MockTraceListener());
            LogSource loggingTraceSource = new LogSource("logging2", SourceLevels.All);
            loggingTraceSource.Listeners.Add(new MockTraceListener());

            LogWriter writer =
                new LogWriter(new ILogFilter[0],
                              new LogSource[] { failingTraceSource, loggingTraceSource },
                              errorsTraceSource,
                              "default");

            LogEntry logEntry = new LogEntry();
            logEntry.Message = originalMessage;
            logEntry.Severity = TraceEventType.Critical;
            logEntry.Categories = new string[] { "logging", "logging2" };

            writer.Write(logEntry);

            writer.Dispose();
            Assert.AreEqual(0, ErrorsMockTraceListener.Entries.Count);
            Assert.AreEqual(2, MockTraceListener.Entries.Count);
            Assert.AreSame(logEntry, MockTraceListener.LastEntry);
        }

        [TestMethod]
        public void MissingCategoriesWarningIsLoggedIfLogWarningFlagIsTrue()
        {
            LogSource errorsTraceSource = new LogSource("errors", SourceLevels.All);
            errorsTraceSource.Listeners.Add(new ErrorsMockTraceListener());

            LogSource failingTraceSource = new LogSource("logging", SourceLevels.All);
            failingTraceSource.Listeners.Add(new MockTraceListener());
            LogSource loggingTraceSource = new LogSource("logging2", SourceLevels.All);
            loggingTraceSource.Listeners.Add(new MockTraceListener());

            LogWriter writer =
                new LogWriter(new ILogFilter[0],
                              new LogSource[] { failingTraceSource, loggingTraceSource },
                              emptyTraceSource,
                              emptyTraceSource,
                              errorsTraceSource,
                              "default",
                              false,
                              true);

            LogEntry logEntry = new LogEntry();
            logEntry.Message = originalMessage;
            logEntry.Severity = TraceEventType.Critical;
            logEntry.Categories = new string[] { "logging", "logging2", "logging3" };

            writer.Write(logEntry);

            writer.Dispose();
            Assert.AreEqual(1, ErrorsMockTraceListener.Entries.Count);
            Assert.AreEqual(TraceEventType.Error, ErrorsMockTraceListener.LastEntry.Severity);
            Assert.IsTrue(MatchTemplate(ErrorsMockTraceListener.LastEntry.Message, Resources.MissingCategories));
            Assert.AreEqual(2, MockTraceListener.Entries.Count);
            Assert.AreSame(logEntry, MockTraceListener.LastEntry);
        }

        bool MatchTemplate(string stringTomatch,
                           string template)
        {
            string pattern = template;
            pattern = pattern.Replace("(", @"\(");
            pattern = pattern.Replace(")", @"\)");
            pattern = Regex.Replace(pattern, @"\{[0-9]\}", "(.*?)");

            return Regex.IsMatch(stringTomatch, pattern);
        }

        int NumberOfHeaders(string fileName,
                            string header)
        {
            int headersFound = 0;
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    string strFileContents;
                    while ((strFileContents = reader.ReadLine()) != null)
                    {
                        if (strFileContents.Equals(header))
                            headersFound++;
                    }
                    reader.Close();
                }
            }
            return headersFound;
        }
    }
}
