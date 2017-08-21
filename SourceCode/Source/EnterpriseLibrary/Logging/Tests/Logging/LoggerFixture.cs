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
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SysConfig = System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    [TestClass]
    public class LoggerFixture
    {
        const string message = "testing.... message and category";
        const string category = "MockCategoryOne";
        static string[] categories = new string[] { category };
        const int priority = 420;
        const int eventId = 421;
        const string title = "bar";
        const TraceEventType severity = TraceEventType.Information;

        [TestInitialize]
        public void SetUp()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
            Logger.Reset();
            MockTraceListener.Reset();
        }

        [TestCleanup]
        public void TearDown()
        {
            Logger.Reset();
            MockTraceListener.Reset();
        }

        [TestMethod]
        public void WriteMessageOnlyOverload()
        {
            Logger.Write(message);

            Assert.IsNotNull(MockTraceListener.LastEntry);
            Assert.AreEqual(message, MockTraceListener.LastEntry.Message, "message");
        }

        [TestMethod]
        public void WriteMessageAndCategoryOnlyOverload()
        {
            Logger.Write(message, category);

            Assert.IsNotNull(MockTraceListener.LastEntry);
            Assert.AreEqual(message, MockTraceListener.LastEntry.Message, "message");
            Assert.AreEqual(1, MockTraceListener.LastEntry.Categories.Count);
            Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(category));
        }

        [TestMethod]
        public void WriteMessageCategoryAndPriorityOverload()
        {
            Logger.Write(message, category, priority);

            Assert.AreEqual(message, MockTraceListener.LastEntry.Message, "message");
            Assert.AreEqual(1, MockTraceListener.LastEntry.Categories.Count);
            Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(category));
            Assert.AreEqual(priority, MockTraceListener.LastEntry.Priority, "priority");
        }

        [TestMethod]
        public void WriteMessageCategoryPriorityEventIdOverload()
        {
            Logger.Write(message, category, priority, eventId);

            Assert.AreEqual(message, MockTraceListener.LastEntry.Message, "message");
            Assert.AreEqual(1, MockTraceListener.LastEntry.Categories.Count);
            Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(category));
            Assert.AreEqual(priority, MockTraceListener.LastEntry.Priority, "priority");
            Assert.AreEqual(eventId, MockTraceListener.LastEntry.EventId, "eventid");
        }

        [TestMethod]
        public void WriteMessageCategoryPriorityEventIdSeverityOverload()
        {
            Logger.Write(message, category, priority, eventId, severity);

            Assert.AreEqual(message, MockTraceListener.LastEntry.Message, "message");
            Assert.AreEqual(1, MockTraceListener.LastEntry.Categories.Count);
            Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(category));
            Assert.AreEqual(priority, MockTraceListener.LastEntry.Priority, "priority");
            Assert.AreEqual(eventId, MockTraceListener.LastEntry.EventId, "eventid");
            Assert.AreEqual(severity, MockTraceListener.LastEntry.Severity, "severity");
        }

        [TestMethod]
        public void WriteMessageCategoryPriorityEventIdSeverityTitleOverload()
        {
            Logger.Write(message, category, priority, eventId, severity, title);

            Assert.AreEqual(message, MockTraceListener.LastEntry.Message, "message");
            Assert.AreEqual(1, MockTraceListener.LastEntry.Categories.Count);
            Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(category));
            Assert.AreEqual(priority, MockTraceListener.LastEntry.Priority, "priority");
            Assert.AreEqual(eventId, MockTraceListener.LastEntry.EventId, "eventid");
            Assert.AreEqual(severity, MockTraceListener.LastEntry.Severity, "severity");
            Assert.AreEqual(title, MockTraceListener.LastEntry.Title, "title");
        }

        [TestMethod]
        public void WriteMessageAndDictionaryOverload()
        {
            Logger.Write(message, CommonUtil.GetPropertiesDictionary());

            Assert.AreEqual(message, MockTraceListener.LastEntry.Message, "message");
            Assert.AreEqual(CommonUtil.GetPropertiesDictionary().Count,
                            MockTraceListener.LastEntry.ExtendedProperties.Count, "hash count");

            Assert.AreEqual(CommonUtil.GetPropertiesDictionary()["key1"],
                            MockTraceListener.LastEntry.ExtendedProperties["key1"], "hash count");
        }

        [TestMethod]
        public void WriteMessageCategoryAndDictionaryOverload()
        {
            Logger.Write(message, category, CommonUtil.GetPropertiesDictionary());

            Assert.AreEqual(message, MockTraceListener.LastEntry.Message, "message");
            Assert.AreEqual(1, MockTraceListener.LastEntry.Categories.Count);
            Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(category));
            Assert.AreEqual(CommonUtil.GetPropertiesDictionary().Count,
                            MockTraceListener.LastEntry.ExtendedProperties.Count, "hash count");
        }

        [TestMethod]
        public void WriteMessageCategoryPriorityAndDictionaryOverload()
        {
            Logger.Write(message, category, priority, CommonUtil.GetPropertiesDictionary());

            Assert.AreEqual(message, MockTraceListener.LastEntry.Message, "message");
            Assert.AreEqual(1, MockTraceListener.LastEntry.Categories.Count);
            Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(category));
            Assert.AreEqual(priority, MockTraceListener.LastEntry.Priority, "priority");
            Assert.AreEqual(CommonUtil.GetPropertiesDictionary().Count,
                            MockTraceListener.LastEntry.ExtendedProperties.Count, "hash count");
        }

        // -----------------------

        [TestMethod]
        public void WriteMessageAndCategoriesOnlyOverload()
        {
            Logger.Write(message, categories);

            Assert.IsNotNull(MockTraceListener.LastEntry);
            Assert.AreEqual(message, MockTraceListener.LastEntry.Message, "message");
            Assert.AreEqual(1, MockTraceListener.LastEntry.Categories.Count);
            Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(category));
        }

        [TestMethod]
        public void WriteMessageCategoriesAndPriorityOverload()
        {
            Logger.Write(message, categories, priority);

            Assert.AreEqual(message, MockTraceListener.LastEntry.Message, "message");
            Assert.AreEqual(1, MockTraceListener.LastEntry.Categories.Count);
            Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(category));
            Assert.AreEqual(priority, MockTraceListener.LastEntry.Priority, "priority");
        }

        [TestMethod]
        public void WriteMessageCategoriesPriorityEventIdOverload()
        {
            Logger.Write(message, categories, priority, eventId);

            Assert.AreEqual(message, MockTraceListener.LastEntry.Message, "message");
            Assert.AreEqual(1, MockTraceListener.LastEntry.Categories.Count);
            Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(category));
            Assert.AreEqual(priority, MockTraceListener.LastEntry.Priority, "priority");
            Assert.AreEqual(eventId, MockTraceListener.LastEntry.EventId, "eventid");
        }

        [TestMethod]
        public void WriteMessageCategoriesPriorityEventIdSeverityOverload()
        {
            Logger.Write(message, categories, priority, eventId, severity);

            Assert.AreEqual(message, MockTraceListener.LastEntry.Message, "message");
            Assert.AreEqual(1, MockTraceListener.LastEntry.Categories.Count);
            Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(category));
            Assert.AreEqual(priority, MockTraceListener.LastEntry.Priority, "priority");
            Assert.AreEqual(eventId, MockTraceListener.LastEntry.EventId, "eventid");
            Assert.AreEqual(severity, MockTraceListener.LastEntry.Severity, "severity");
        }

        [TestMethod]
        public void WriteMessageCategoriesPriorityEventIdSeverityTitleOverload()
        {
            Logger.Write(message, categories, priority, eventId, severity, title);

            Assert.AreEqual(message, MockTraceListener.LastEntry.Message, "message");
            Assert.AreEqual(1, MockTraceListener.LastEntry.Categories.Count);
            Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(category));
            Assert.AreEqual(priority, MockTraceListener.LastEntry.Priority, "priority");
            Assert.AreEqual(eventId, MockTraceListener.LastEntry.EventId, "eventid");
            Assert.AreEqual(severity, MockTraceListener.LastEntry.Severity, "severity");
            Assert.AreEqual(title, MockTraceListener.LastEntry.Title, "title");
        }

        [TestMethod]
        public void WriteMessageCategoriesAndDictionaryOverload()
        {
            Logger.Write(message, categories, CommonUtil.GetPropertiesDictionary());

            Assert.AreEqual(message, MockTraceListener.LastEntry.Message, "message");
            Assert.AreEqual(1, MockTraceListener.LastEntry.Categories.Count);
            Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(category));
            Assert.AreEqual(CommonUtil.GetPropertiesDictionary().Count,
                            MockTraceListener.LastEntry.ExtendedProperties.Count, "hash count");
        }

        [TestMethod]
        public void WriteMessageCategoriesPriorityAndDictionaryOverload()
        {
            Logger.Write(message, categories, priority, CommonUtil.GetPropertiesDictionary());

            Assert.AreEqual(message, MockTraceListener.LastEntry.Message, "message");
            Assert.AreEqual(1, MockTraceListener.LastEntry.Categories.Count);
            Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(category));
            Assert.AreEqual(priority, MockTraceListener.LastEntry.Priority, "priority");
            Assert.AreEqual(CommonUtil.GetPropertiesDictionary().Count,
                            MockTraceListener.LastEntry.ExtendedProperties.Count, "hash count");
        }

        // -----------------------

        [TestMethod]
        public void WriteDictionary()
        {
            if (!EventLog.SourceExists("Unit Test")) return;

            LogEntry entry = CommonUtil.GetDefaultLogEntry();
            entry.Categories = new string[] { "DictionaryCategory" };
            entry.Severity = TraceEventType.Error;
            entry.ExtendedProperties = CommonUtil.GetPropertiesDictionary();

            Logger.Write(entry);

            string expected = CommonUtil.FormattedMessageWithDictionary;
            string actual = CommonUtil.GetLastEventLogEntryCustom();
            Assert.IsTrue(actual.IndexOf("key1") > -1);
            Assert.IsTrue(actual.IndexOf("value1") > -1);
            Assert.IsTrue(actual.IndexOf("key2") > -1);
            Assert.IsTrue(actual.IndexOf("value2") > -1);
            Assert.IsTrue(actual.IndexOf("key3") > -1);
            Assert.IsTrue(actual.IndexOf("value3") > -1);
        }

        [TestMethod]
        public void WriteDictionaryWithHeaderAndFooter()
        {
            LogEntry entry = CommonUtil.GetDefaultLogEntry();
            entry.Categories = new string[] { "DictionaryCategoryForFlatFile" };
            entry.Severity = TraceEventType.Error;
            entry.ExtendedProperties = CommonUtil.GetPropertiesDictionary();

            Logger.Write(entry);
            Logger.Reset();

            string expected = CommonUtil.FormattedMessageWithDictionary;
            string actual = GetFileContents(CommonUtil.FileName);

            Assert.IsTrue(actual.IndexOf("key1") > -1);
            Assert.IsTrue(actual.IndexOf("value1") > -1);
            Assert.IsTrue(actual.IndexOf("key2") > -1);
            Assert.IsTrue(actual.IndexOf("value2") > -1);
            Assert.IsTrue(actual.IndexOf("key3") > -1);
            Assert.IsTrue(actual.IndexOf("value3") > -1);
        }

        [TestMethod]
        public void WriteEntryWithModeOff()
        {
            // configuration update
            System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            LoggingSettings writableSettings = (LoggingSettings)configuration.GetSection(LoggingSettings.SectionName);
            writableSettings.LogFilters.Remove("enable");
            writableSettings.LogFilters.Add(new LogEnabledFilterData("enable", false));
            configuration.Save(ConfigurationSaveMode.Minimal, true);

            try
            {
                Logger.Write("Body", "Category1", -1, 1, TraceEventType.Warning, "Header");

                Assert.IsNull(MockTraceListener.LastEntry, "assert that nothing was written");
            }
            finally
            {
                // configuration rollback
                writableSettings.LogFilters.Remove("enable");
                configuration.Save(ConfigurationSaveMode.Minimal, true);
                ConfigurationManager.RefreshSection(LoggingSettings.SectionName);
            }
        }

        [TestMethod]
        public void MessageLoggedWithSufficientPriorityShouldGetLogged()
        {
            LogEntry logEntry = CommonUtil.GetDefaultLogEntry();
            logEntry.Categories = new string[] { "MockCategoryOne" };
            logEntry.Priority = Logger.GetFilter<PriorityFilter>().MinimumPriority + 1;
            logEntry.Severity = TraceEventType.Information;

            bool shouldLog = Logger.ShouldLog(logEntry);
            Logger.Write(logEntry);

            LogEntry resultLog = MockTraceListener.LastEntry;

            Assert.IsNotNull(resultLog, "confirm that the message got logged by the strategy");
            Assert.AreEqual(logEntry.Message, resultLog.Message);
            Assert.IsTrue(shouldLog);
        }

        [TestMethod]
        public void MessageLoggedWithPriorityBelowMinimumWillNotBeLogged()
        {
            LogEntry logEntry = CommonUtil.GetDefaultLogEntry();
            logEntry.Categories = new string[] { "MockCategoryOne" };
            logEntry.Priority = Logger.GetFilter<PriorityFilter>().MinimumPriority - 1;
            logEntry.Severity = TraceEventType.Information;

            bool shouldLog = Logger.ShouldLog(logEntry);
            Logger.Write(logEntry);

            LogEntry resultLog = MockTraceListener.LastEntry;

            Assert.IsNull(resultLog, "confirm that the message did NOT get logged by the strategy");
            Assert.IsFalse(shouldLog);
        }

        [TestMethod]
        public void EmptyCategoriesRevertToDefaultCategory()
        {
            MockTraceListener.Reset();
            LoggingSettings settings = LoggingSettings.GetLoggingSettings(new SystemConfigurationSource());

            LogEntry log = new LogEntry();
            log.EventId = 1;
            log.Message = "test";
            log.Categories = new string[0];
            log.Severity = TraceEventType.Error;
            Logger.Write(log);

            Assert.IsNotNull(MockTraceListener.LastEntry);
            Assert.AreEqual("test", MockTraceListener.LastEntry.Message);
            Assert.AreEqual(1, MockTraceListener.LastEntry.Categories.Count);
            Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains(settings.DefaultCategory));
        }

        [TestMethod]
        public void CanWritetoListenerWithDefaultConstructor()
        {
            LogEntry log = new LogEntry();
            log.EventId = 1;
            log.Message = "test";
            log.Categories = new string[] { "ConsoleCategory" };
            log.Severity = TraceEventType.Error;
            Logger.Write(log);
        }

        [TestMethod]
        public void WmiEventFiredWhenLoggingConfigurationIsCorrupt()
        {
            //corrupt the logging configuration
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            LoggingSettings rwLoggingSettings = (LoggingSettings)config.GetSection(LoggingSettings.SectionName);
            string originalFirstTraceListenerName = rwLoggingSettings.TraceListeners.Get(0).Name;
            rwLoggingSettings.TraceSources.Get(0).TraceListeners.Get(0).Name = "wrongName";
            InstrumentationConfigurationSection rwInstrumentationSettings = (InstrumentationConfigurationSection)config.GetSection(InstrumentationConfigurationSection.SectionName);
            rwInstrumentationSettings.WmiEnabled = true;
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection(LoggingSettings.SectionName);
            ConfigurationManager.RefreshSection(InstrumentationConfigurationSection.SectionName);

            using (WmiEventWatcher eventListener = new WmiEventWatcher(1))
            {
                try
                {
                    Logger.Write("Some message");
                }
                catch (ConfigurationErrorsException)
                {
                    eventListener.WaitForEvents();
                    Assert.AreEqual(1, eventListener.EventsReceived.Count);
                    Assert.AreEqual("LoggingConfigurationFailureEvent", eventListener.EventsReceived[0].ClassPath.ClassName);
                    string exceptionMessage = (string)eventListener.EventsReceived[0].GetPropertyValue("ExceptionMessage");

                    Assert.IsTrue(exceptionMessage.Contains("wrongName"));
                }
                finally
                {
                    //restore the logging configuration
                    rwLoggingSettings.TraceSources.Get(0).TraceListeners.Get(0).Name = originalFirstTraceListenerName;
                    rwInstrumentationSettings.WmiEnabled = false;
                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection(LoggingSettings.SectionName);
                    ConfigurationManager.RefreshSection(InstrumentationConfigurationSection.SectionName);
                }
            }
        }

        [TestMethod]
        public void EventLogEntryWrittenWhenLoggingConfigurationIsCorrupt()
        {
            //corrupt the logging configuration
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            LoggingSettings rwLoggingSettings = (LoggingSettings)config.GetSection(LoggingSettings.SectionName);
            string originalFirstTraceListenerName = rwLoggingSettings.TraceListeners.Get(0).Name;
            rwLoggingSettings.TraceSources.Get(0).TraceListeners.Get(0).Name = "wrongName";
            config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection(LoggingSettings.SectionName);

            try
            {
                Logger.Write("SomeMessage");
            }
            catch (ConfigurationErrorsException)
            {
                using (EventLog applicationLog = new EventLog("Application"))
                {
                    EventLogEntry lastEntry = applicationLog.Entries[applicationLog.Entries.Count - 1];

                    Assert.AreEqual("Enterprise Library Logging", lastEntry.Source);
                    Assert.IsTrue(lastEntry.Message.Contains("wrongName"));
                }
            }
            finally
            {
                //restore the logging configuration
                rwLoggingSettings.TraceSources.Get(0).TraceListeners.Get(0).Name = originalFirstTraceListenerName;
                config.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection(LoggingSettings.SectionName);
            }
        }

        string GetFileContents(string fileName)
        {
            string strFileContents = string.Empty;
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    strFileContents = reader.ReadToEnd();
                    reader.Close();
                }
            }
            return strFileContents;
        }
    }
}
