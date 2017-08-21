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
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    [TestClass]
    public class LogWriterSingletonManagementFixture
    {
        DictionaryConfigurationSourceWithHandlersQuery configurationSource;
        ConfigurationReflectionCache reflectionCache;
        LoggingSettings settings;

        [TestInitialize]
        public void SetUp()
        {
            configurationSource = new DictionaryConfigurationSourceWithHandlersQuery();

            InstrumentationConfigurationSection instrumentationConfig = new InstrumentationConfigurationSection(true, true, false);
            configurationSource.Add(InstrumentationConfigurationSection.SectionName, instrumentationConfig);

            settings = new LoggingSettings();
            configurationSource.Add(LoggingSettings.SectionName, settings);

            settings.SpecialTraceSources.ErrorsTraceSource
                = new TraceSourceData("error", SourceLevels.Off);

            TraceSourceData traceSourceData = new TraceSourceData("blocking", SourceLevels.All);
            traceSourceData.TraceListeners.Add(new TraceListenerReferenceData("listener1"));
            settings.TraceSources.Add(traceSourceData);
            traceSourceData = new TraceSourceData("nonblocking", SourceLevels.All);
            traceSourceData.TraceListeners.Add(new TraceListenerReferenceData("listener2"));
            settings.TraceSources.Add(traceSourceData);

            TraceListenerData traceListenerData = new CustomTraceListenerData("listener1", typeof(MockBlockingCustomTraceListener), "init 1");
            settings.TraceListeners.Add(traceListenerData);
            traceListenerData = new MockTraceListenerData("listener2");
            settings.TraceListeners.Add(traceListenerData);

            reflectionCache = new ConfigurationReflectionCache();

            MockTraceListener.Reset();
            MockBlockingCustomTraceListener.Reset();
        }

        [TestCleanup]
        public void TearDown()
        {
            configurationSource = null;
            MockTraceListener.Reset();
            MockBlockingCustomTraceListener.Reset();
            LogWriter.ResetLockTimeouts();
        }

        [TestMethod]
        public void LogWriterCreatedThroughFactoryRegistersHandler()
        {
            Assert.AreEqual(0, configurationSource.GetNotificationDelegates(LoggingSettings.SectionName).Length);

            LogWriter logWriter = new LogWriterFactory(configurationSource).Create();
            Assert.IsNotNull(logWriter);
            Assert.AreEqual(2, logWriter.TraceSources.Count);

            Assert.AreEqual(1, configurationSource.GetNotificationDelegates(LoggingSettings.SectionName).Length);
        }

        [TestMethod]
        public void DisposedLogWriterCreatedThroughFactoryUnregistersHandler()
        {
            Assert.AreEqual(0, configurationSource.GetNotificationDelegates(LoggingSettings.SectionName).Length);

            LogWriter logWriter = new LogWriterFactory(configurationSource).Create();
            Assert.IsNotNull(logWriter);
            Assert.AreEqual(2, logWriter.TraceSources.Count);

            Assert.AreEqual(1, configurationSource.GetNotificationDelegates(LoggingSettings.SectionName).Length);

            logWriter.Dispose();

            Assert.AreEqual(0, configurationSource.GetNotificationDelegates(LoggingSettings.SectionName).Length);
        }

        [TestMethod]
        public void ConfigurationChangeNotificationTriggersLogWriterStructureHolderUpdate()
        {
            LogWriter logWriter = new LogWriterFactory(configurationSource).Create();
            Assert.IsNotNull(logWriter);

            {
                Assert.AreEqual(2, logWriter.TraceSources.Count);
                IEnumerator<LogSource> sources = logWriter.TraceSources.Values.GetEnumerator();
                sources.MoveNext();
                Assert.AreEqual("blocking", sources.Current.Name);
            }

            settings.TraceSources.Get(0).Name = "new source";

            Thread notificationThread = new Thread(FireConfigurationSectionChangedNotification);
            notificationThread.Start();
            notificationThread.Join(100);

            {
                Assert.AreEqual(2, logWriter.TraceSources.Count);
                IEnumerator<LogSource> sources = logWriter.TraceSources.Values.GetEnumerator();
                sources.MoveNext();
                Assert.AreEqual("new source", sources.Current.Name);
            }
        }

        [TestMethod]
        public void ConfigurationChangeNotificationForDifferentSectionDoesNotTriggerLogWriterStructureHolderUpdate()
        {
            LogWriter logWriter = new LogWriterFactory(configurationSource).Create();
            Assert.IsNotNull(logWriter);

            {
                Assert.AreEqual(2, logWriter.TraceSources.Count);
                IEnumerator<LogSource> sources = logWriter.TraceSources.Values.GetEnumerator();
                sources.MoveNext();
                Assert.AreEqual("blocking", sources.Current.Name);
            }

            settings.TraceSources.Get(0).Name = "new source";

            Thread notificationThread = new Thread(FireConfigurationSectionChangedNotificationDifferentSection);
            notificationThread.Start();
            notificationThread.Join(100);

            {
                Assert.AreEqual(2, logWriter.TraceSources.Count);
                IEnumerator<LogSource> sources = logWriter.TraceSources.Values.GetEnumerator();
                sources.MoveNext();
                Assert.AreEqual("blocking", sources.Current.Name);
            }
        }

        [TestMethod]
        public void ConfigurationErrorsOnUpdateKeepExistingSetupAndLogError()
        {
            LogWriter logWriter = new LogWriterFactory(configurationSource).Create();
            Assert.IsNotNull(logWriter);

            {
                Assert.AreEqual(2, logWriter.TraceSources.Count);
                IEnumerator<LogSource> sources = logWriter.TraceSources.Values.GetEnumerator();
                sources.MoveNext();
                Assert.AreEqual("blocking", sources.Current.Name);
            }

            settings.TraceSources.Get(0).Name = "new source";
            settings.TraceSources.Get(0).TraceListeners.Get(0).Name = "invalid listener";

            using (EventLog eventLog = new EventLog("Application", ".", "Enterprise Library Logging"))
            {
                int previousEventCount = eventLog.Entries.Count;
                Thread notificationThread = new Thread(FireConfigurationSectionChangedNotification);
                notificationThread.Start();
                notificationThread.Join(1000);

                Assert.IsTrue(eventLog.Entries.Count > 0);
                EventLogEntry lastEntry = eventLog.Entries[eventLog.Entries.Count - 1];
                Assert.AreEqual("Enterprise Library Logging", lastEntry.Source);
            }

            {
                Assert.AreEqual(2, logWriter.TraceSources.Count);
                IEnumerator<LogSource> sources = logWriter.TraceSources.Values.GetEnumerator();
                sources.MoveNext();
                Assert.AreEqual("blocking", sources.Current.Name);
            }
        }

        [TestMethod]
        // depends on timing
        public void MultipleConcurrentLoggingRequestsSucceed()
        {
            LogWriter logWriter = new LogWriterFactory(configurationSource).Create();
            Assert.IsNotNull(logWriter);

            Thread blockingLogThread = new Thread(new MultiThreadLoggingHelper(logWriter, "blocking").DoLogging);
            Thread nonblockingLogThread = new Thread(new MultiThreadLoggingHelper(logWriter, "nonblocking").DoLogging);

            try
            {
                blockingLogThread.Start();
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(10);
                    if (MockBlockingCustomTraceListener.PendingTraceRequests == 1) break;
                }

                Assert.AreEqual(0, MockBlockingCustomTraceListener.ProcessedTraceRequests, "there should have been no written entries so far");
                Assert.AreEqual(1, MockBlockingCustomTraceListener.PendingTraceRequests, "there should be one pending request");
                Assert.AreEqual(0, MockTraceListener.ProcessedTraceRequests);

                nonblockingLogThread.Start();
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(10);
                    if (MockTraceListener.ProcessedTraceRequests == 1) break;
                }

                Assert.AreEqual(0, MockBlockingCustomTraceListener.ProcessedTraceRequests, "there should have been no written entries so far");
                Assert.AreEqual(1, MockBlockingCustomTraceListener.PendingTraceRequests, "there should be one pending request");
                Assert.AreEqual(1, MockTraceListener.ProcessedTraceRequests, "the request to the non blocking listener should have succedded");

                lock (MockBlockingCustomTraceListener.traceRequestMonitor)
                {
                    Monitor.Pulse(MockBlockingCustomTraceListener.traceRequestMonitor);
                }
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(10);
                    if (MockBlockingCustomTraceListener.ProcessedTraceRequests == 1) break;
                }

                Assert.AreEqual(1, MockBlockingCustomTraceListener.ProcessedTraceRequests, "there should have been one written entries so far");
                Assert.AreEqual(0, MockBlockingCustomTraceListener.PendingTraceRequests, "there should be no pending request");
                Assert.AreEqual(1, MockTraceListener.ProcessedTraceRequests, "the request to the non blocking listener should have succedded");
            }
            finally
            {
                try
                {
                    nonblockingLogThread.Join(50);
                }
                catch {}
                try
                {
                    blockingLogThread.Join(50);
                }
                catch {}
            }
        }

        [TestMethod]
        // depends on timing
        public void ChangeNotificationDuringTracingDelaysUpdate()
        {
            LogWriter logWriter = new LogWriterFactory(configurationSource).Create();
            Assert.IsNotNull(logWriter);

            Thread blockingLogThread1 = new Thread(new MultiThreadLoggingHelper(logWriter, "blocking").DoLogging);
            Thread blockingLogThread2 = new Thread(new MultiThreadLoggingHelper(logWriter, "blocking").DoLogging);
            Thread blockingLogThread3 = new Thread(new MultiThreadLoggingHelper(logWriter, "blocking").DoLogging);
            Thread notificationThread = new Thread(FireConfigurationSectionChangedNotification);

            try
            {
                blockingLogThread1.Start();
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(10);
                    if (MockBlockingCustomTraceListener.PendingTraceRequests == 1) break;
                }

                Assert.AreEqual(0, MockBlockingCustomTraceListener.ProcessedTraceRequests, "there should have been no written entries so far");
                Assert.AreEqual(1, MockBlockingCustomTraceListener.PendingTraceRequests, "the first request should have made it through");

                blockingLogThread2.Start();
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(10);
                    if (MockBlockingCustomTraceListener.PendingTraceRequests == 2) break;
                }

                Assert.AreEqual(0, MockBlockingCustomTraceListener.ProcessedTraceRequests, "there should have been no written entries so far");
                Assert.AreEqual(2, MockBlockingCustomTraceListener.PendingTraceRequests, "the second request should have made it through");

                notificationThread.Start();
                Thread.Sleep(100);

                Assert.AreEqual(0, MockBlockingCustomTraceListener.ProcessedTraceRequests, "there should have been no written entries so far");
                Assert.AreEqual(2, MockBlockingCustomTraceListener.PendingTraceRequests, "the two requests should still be waiting");

                blockingLogThread3.Start();
                Thread.Sleep(100);

                Assert.AreEqual(0, MockBlockingCustomTraceListener.ProcessedTraceRequests, "there should have been no written entries so far");
                Assert.AreEqual(2, MockBlockingCustomTraceListener.PendingTraceRequests, "the third request should have been delayed by the udpate request");

                lock (MockBlockingCustomTraceListener.traceRequestMonitor)
                {
                    Monitor.PulseAll(MockBlockingCustomTraceListener.traceRequestMonitor);
                }
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(10);
                    if (MockBlockingCustomTraceListener.PendingTraceRequests == 1 &&
                        MockBlockingCustomTraceListener.ProcessedTraceRequests == 2)
                    {
                        break;
                    }
                }

                Assert.AreEqual(2, MockBlockingCustomTraceListener.ProcessedTraceRequests, "the two initial requests should have been written");
                Assert.AreEqual(1, MockBlockingCustomTraceListener.PendingTraceRequests, "the third request should have made it through");

                lock (MockBlockingCustomTraceListener.traceRequestMonitor)
                {
                    Monitor.PulseAll(MockBlockingCustomTraceListener.traceRequestMonitor);
                }
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(10);
                    if (MockBlockingCustomTraceListener.PendingTraceRequests == 0) break;
                }

                Assert.AreEqual(3, MockBlockingCustomTraceListener.ProcessedTraceRequests, "all three requests should have been written");
                Assert.AreEqual(0, MockBlockingCustomTraceListener.PendingTraceRequests, "there should be no pending requests");

                Assert.IsTrue(
                    ReferenceEquals(MockBlockingCustomTraceListener.Instances[0], MockBlockingCustomTraceListener.Instances[1]),
                    "the two first requests shared the same listener instance");
                Assert.IsFalse(
                    ReferenceEquals(MockBlockingCustomTraceListener.Instances[0], MockBlockingCustomTraceListener.Instances[2]),
                    "the third request used a new listener instance, after the configuration change");
                Assert.IsTrue(MockBlockingCustomTraceListener.Instances[0].disposeCalled);
                Assert.IsFalse(MockBlockingCustomTraceListener.Instances[2].disposeCalled);
            }
            finally
            {
                try
                {
                    blockingLogThread1.Join(50);
                }
                catch {}
                try
                {
                    blockingLogThread2.Join(50);
                }
                catch {}
                try
                {
                    blockingLogThread3.Join(50);
                }
                catch {}
                try
                {
                    notificationThread.Join(50);
                }
                catch {}
            }
        }

        [TestMethod]
        public void FailureToAcquireReadLockLogsException()
        {
            LogWriter.SetLockTimeouts(150, 150);

            LogWriter logWriter = new LogWriterFactory(configurationSource).Create();
            Assert.IsNotNull(logWriter);

            Thread blockingLogThread1 = new Thread(new MultiThreadLoggingHelper(logWriter, "blocking").DoLogging);
            Thread notificationThread = new Thread(FireConfigurationSectionChangedNotification);

            try
            {
                MockBlockingCustomTraceListener.waitOnDispose = true;

                // block whild holding writer lock 
                notificationThread.Start();
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(10);
                    if (MockBlockingCustomTraceListener.PendingDisposeRequests == 1) break;
                }
                Assert.AreEqual(1, MockBlockingCustomTraceListener.PendingDisposeRequests, "there should be one pending dispose request");

                // try to log - shouldn't even try to
                blockingLogThread1.Start();
                Thread.Sleep(400);
                blockingLogThread1.Join();

                Assert.AreEqual(0, MockBlockingCustomTraceListener.PendingTraceRequests);
                Assert.AreEqual(0, MockBlockingCustomTraceListener.ProcessedTraceRequests);
                using (EventLog applicationLog = new EventLog("Application"))
                {
                    EventLogEntry lastEntry = applicationLog.Entries[applicationLog.Entries.Count - 1];

                    Assert.AreEqual("Enterprise Library Logging", lastEntry.Source);
                    Assert.IsTrue(lastEntry.Message.Contains(Resources.ExceptionFailedToAcquireLockToWriteLog));
                }

                // let the update finish
                lock (MockBlockingCustomTraceListener.disposeMonitor)
                {
                    Monitor.PulseAll(MockBlockingCustomTraceListener.disposeMonitor);
                }
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(10);
                    if (MockBlockingCustomTraceListener.PendingDisposeRequests == 0) break;
                }
                Assert.AreEqual(0, MockBlockingCustomTraceListener.PendingDisposeRequests, "there should be no pending dispose requests");
            }
            finally
            {
                try
                {
                    blockingLogThread1.Join(50);
                }
                catch {}
                try
                {
                    notificationThread.Join(50);
                }
                catch {}
            }
        }

        [TestMethod]
        // depends on timing
        public void FailureToAcquireWriteLockLogsException()
        {
            LogWriter.SetLockTimeouts(150, 150);

            LogWriter logWriter = new LogWriterFactory(configurationSource).Create();
            Assert.IsNotNull(logWriter);

            Thread blockingLogThread1 = new Thread(new MultiThreadLoggingHelper(logWriter, "blocking").DoLogging);
            Thread notificationThread = new Thread(FireConfigurationSectionChangedNotification);

            try
            {
                MockBlockingCustomTraceListener.waitOnDispose = true;

                // block whild holding writer lock 
                blockingLogThread1.Start();
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(10);
                    if (MockBlockingCustomTraceListener.PendingTraceRequests == 1) break;
                }
                Assert.AreEqual(1, MockBlockingCustomTraceListener.PendingTraceRequests, "there should be one pending dispose request");

                // try to update
                notificationThread.Start();
                Thread.Sleep(400);
                notificationThread.Join(50);

                using (EventLog applicationLog = new EventLog("Application"))
                {
                    EventLogEntry lastEntry = applicationLog.Entries[applicationLog.Entries.Count - 1];

                    Assert.AreEqual("Enterprise Library Logging", lastEntry.Source);
                    Assert.IsTrue(lastEntry.Message.Contains(Resources.ExceptionFailedToAcquireLockToUpdate));
                }

                // let the trace finish
                lock (MockBlockingCustomTraceListener.traceRequestMonitor)
                {
                    Monitor.PulseAll(MockBlockingCustomTraceListener.traceRequestMonitor);
                }
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(10);
                    if (MockBlockingCustomTraceListener.PendingTraceRequests == 0) break;
                }
                Assert.AreEqual(0, MockBlockingCustomTraceListener.PendingTraceRequests, "there should be no pending trace requests");
            }
            finally
            {
                try
                {
                    blockingLogThread1.Join(50);
                }
                catch {}
                try
                {
                    notificationThread.Join(50);
                }
                catch {}
            }
        }

        [TestMethod]
        public void CanBuildLogWriterStructureHolder()
        {
            LogWriterStructureHolder setup
                = EnterpriseLibraryFactory.BuildUp<LogWriterStructureHolder>(configurationSource);

            Assert.IsNotNull(setup);
            Assert.AreEqual(2, setup.TraceSources.Count);
        }

        void FireConfigurationSectionChangedNotification()
        {
            configurationSource.FireConfigurationSectionChangedNotification(LoggingSettings.SectionName);
        }

        void FireConfigurationSectionChangedNotificationDifferentSection()
        {
            configurationSource.FireConfigurationSectionChangedNotification("different section");
        }

        class MultiThreadLoggingHelper
        {
            string category;
            LogWriter logWriter;

            public MultiThreadLoggingHelper(LogWriter logWriter,
                                            string category)
            {
                this.logWriter = logWriter;
                this.category = category;
            }

            public void DoLogging()
            {
                LogEntry entry = new LogEntry("msg", category, 0, 0, TraceEventType.Error, "title", null);
                logWriter.Write(entry);
            }
        }
    }

    public class DictionaryConfigurationSourceWithHandlersQuery : DictionaryConfigurationSource
    {
        static readonly Delegate[] emptyDelegatesArray = new Delegate[0];

        internal void FireConfigurationSectionChangedNotification(string sectionName)
        {
            ConfigurationChangedEventHandler callbacks = (ConfigurationChangedEventHandler)eventHandlers[sectionName];
            if (callbacks != null)
            {
                ConfigurationChangedEventArgs eventData = new ConfigurationChangedEventArgs(sectionName);
                foreach (ConfigurationChangedEventHandler callback in callbacks.GetInvocationList())
                {
                    if (callback != null)
                    {
                        callback(this, eventData);
                    }
                }
            }
        }

        internal Delegate[] GetNotificationDelegates(string sectionName)
        {
            return eventHandlers[sectionName] != null
                       ? eventHandlers[sectionName].GetInvocationList()
                       : emptyDelegatesArray;
        }
    }
}
