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
using System.Globalization;
using System.IO;
using System.Messaging;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Xml.XPath;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    /// <summary>
    /// Constants and utility functions to support all programmer tests.
    /// </summary>
    public class CommonUtil
    {
        // Sink constants
        public const string CustomCategory = "defaultCategory";
        public const string DefaultCategory = "defaultCategory";
        public const string EventLogName = "Application";
        public const string EventLogNameCustom = "EntLib Tests";
        public const string EventLogSourceName = "Enterprise Library Unit Tests";
        public const string FileName = "FlatFileTestOutput\\EntLibLog.txt";
        public const string MessageQueuePath = @".\Private$\entlib";

        // Log entry property constants
        public const string MsgBody = "My message body";
        public const string MsgCategory = "foo";
        public const int MsgEventID = 1;
        public const string MsgTitle = "=== Header ===";
        public const string ProxyCategory = "proxyCategory";
        public const string ServiceModelCategory = "System.ServiceModel";
        public const string Xml = @"<data attr=""MyValue""/>";
        public static string[] Categories = { DefaultCategory, CustomCategory };
        static int eventLogEntryCounter = 0;
        static int eventLogEntryCounterCustom = 0;

        // Formatted message constants
        public static readonly string FormattedMessage =
            GetFormattedMessage();

        public static readonly string FormattedMessageWithDictionary =
            "Timestamp: 12/31/9999 11:59:59 PM\r\nTitle: === Header ===\r\n\r\nMessage: My message body\r\n\r\nExtended Properties:\r\nKey: key1\t\tValue: value1\r\nKey: key3\t\tValue: value3\r\nKey: key2\t\tValue: value2\r\n";

        public static readonly string MessageWithDictionaryXml =
            GetMessageWithDictionaryXml();

        public static readonly DateTime MsgDateTime = DateTime.MaxValue;

        public static void AssertLogEntries(LogEntry logEntry,
                                            LogEntry anotherLogEntry)
        {
            Assert.AreEqual(logEntry.ActivityId, anotherLogEntry.ActivityId);
            Assert.AreEqual(logEntry.ActivityIdString, anotherLogEntry.ActivityIdString);
            Assert.AreEqual(logEntry.AppDomainName, anotherLogEntry.AppDomainName);
            Assert.AreEqual(logEntry.ErrorMessages, anotherLogEntry.ErrorMessages);
            Assert.AreEqual(logEntry.EventId, anotherLogEntry.EventId);
            Assert.AreEqual(logEntry.Categories.Count, anotherLogEntry.Categories.Count);
            Assert.AreEqual(logEntry.LoggedSeverity, anotherLogEntry.LoggedSeverity);
            Assert.AreEqual(logEntry.MachineName, anotherLogEntry.MachineName);
            Assert.AreEqual(logEntry.ManagedThreadName, anotherLogEntry.ManagedThreadName);
            Assert.AreEqual(logEntry.Message, anotherLogEntry.Message);
            Assert.AreEqual(logEntry.Priority, anotherLogEntry.Priority);
            Assert.AreEqual(logEntry.ProcessId, anotherLogEntry.ProcessId);
            Assert.AreEqual(logEntry.ProcessName, anotherLogEntry.ProcessName);
            Assert.AreEqual(logEntry.Severity, anotherLogEntry.Severity);
            Assert.AreEqual(logEntry.TimeStamp, anotherLogEntry.TimeStamp);
            Assert.AreEqual(logEntry.TimeStampString, anotherLogEntry.TimeStampString);
            Assert.AreEqual(logEntry.Title, anotherLogEntry.Title);
            Assert.AreEqual(logEntry.Win32ThreadId, anotherLogEntry.Win32ThreadId);
        }

        public static void AssertXmlLogEntries(XmlLogEntry xmlLogEntry,
                                               XmlLogEntry anotherXmlLogEntry)
        {
            AssertLogEntries(xmlLogEntry, anotherXmlLogEntry);
            Assert.AreEqual(xmlLogEntry.Xml.InnerXml, anotherXmlLogEntry.Xml.InnerXml);
        }

        public static LogEntry CreateLogEntry()
        {
            return FillLogEntry(new LogEntry());
        }

        public static void CreatePrivateTestQ()
        {
            string path = MessageQueuePath;
            if (MessageQueue.Exists(path))
            {
                DeletePrivateTestQ();
            }
            MessageQueue.Create(path, false);
        }

        public static void CreateTransactionalPrivateTestQ()
        {
            string path = MessageQueuePath;
            if (!MessageQueue.Exists(path))
            {
                MessageQueue.Create(path, true);
            }
        }

        public static XmlLogEntry CreateXmlLogEntry()
        {
            return CreateXmlLogEntry(Xml, Categories);
        }

        public static XmlLogEntry CreateXmlLogEntry(string xml,
                                                    string[] categories)
        {
            XmlLogEntry entry = new XmlLogEntry();
            FillLogEntry(entry);
            entry.Categories = new List<string>(categories);
            entry.Xml = new XPathDocument(new StringReader(xml)).CreateNavigator();
            return entry;
        }

        public static void DeletePrivateTestQ()
        {
            string path = MessageQueuePath;
            if (MessageQueue.Exists(path))
            {
                MessageQueue.Delete(path);
            }
        }

        ///// <summary>
        ///// Build a formatted xml string
        ///// </summary>
        //public static string GetMessage(string body, string header, int eventID,
        //                                string category, int categoryID, Severity severity)
        //{
        //    //  build a message
        //    string msg = string.Format("<Message>" +
        //        "<Header>{0}</Header>" +
        //        "<Body>{1}</Body>" +
        //        "<Severity>{2}</Severity>" +
        //        "<LogCategory>{3}</LogCategory>" +
        //        "<EventID>{4}</EventID></Message>",
        //                               header, body, (int)severity, category, eventID);

        //    return msg;
        //}

        public static int EventLogEntryCount()
        {
            using (EventLog log = new EventLog(EventLogName))
            {
                return log.Entries.Count - eventLogEntryCounter;
            }
        }

        public static int EventLogEntryCountCustom()
        {
            return GetCustomEventLog().Entries.Count - eventLogEntryCounterCustom;
        }

        public static string ExecuteProcess(string command,
                                            string cmdarg)
        {
            ProcessStartInfo info = new ProcessStartInfo(command, cmdarg);

            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
            info.CreateNoWindow = true;
            using (Process p = Process.Start(info))
            {
                string result = p.StandardOutput.ReadToEnd();

                p.WaitForExit();

                return result;
            }
        }

        static LogEntry FillLogEntry(LogEntry entry)
        {
            entry.TimeStamp = DateTime.MaxValue;
            entry.Title = "My title";
            entry.Message = "My message body";
            return entry;
        }

        static string GetCurrentProcessId()
        {
            return NativeMethods.GetCurrentProcessId().ToString();
        }

        static string GetCurrentThreadId()
        {
            return NativeMethods.GetCurrentThreadId().ToString();
        }

        public static EventLog GetCustomEventLog()
        {
            if (!EventLog.Exists(EventLogNameCustom))
            {
                using (EventLog log = new EventLog(EventLogNameCustom, ".", EventLogNameCustom + " Source"))
                {
                    log.WriteEntry("Event Log Created");
                }
            }
            return new EventLog(EventLogNameCustom);
        }

        public static LogEntry GetDefaultLogEntry()
        {
            LogEntry entry = new LogEntry(
                MsgBody,
                MsgCategory,
                -1,
                MsgEventID,
                TraceEventType.Information,
                MsgTitle,
                null
                );

            entry.Priority = 100;

            entry.TimeStamp = DateTime.MaxValue;
            entry.MachineName = "machine";

            return entry;
        }

        static string GetFormattedMessage()
        {
            try
            {
                return "Timestamp: 12/31/9999 11:59:59 PM\r\nMessage: My message body\r\nCategory: foo\r\nPriority: 100\r\nEventId: 1\r\nSeverity: Unspecified\r\nTitle:=== Header ===\r\nMachine: machine" +
                       "\r\nApp Domain: " + AppDomain.CurrentDomain.FriendlyName + "\r\nProcessId: " + GetCurrentProcessId() +
                       "\r\nProcess Name: " + GetProcessName() + "\r\nThread Name: " + Thread.CurrentThread.Name + "\r\nWin32 ThreadId:" + GetCurrentThreadId() + "\r\nExtended Properties: ";
            }
            catch {}

            return "";
        }

        public static string GetIntrinsicInfo()
        {
            string intrinsicInfo = "<AssemblyInformation>" + Assembly.GetExecutingAssembly().FullName + "</AssemblyInformation>\r\n  " +
                                   "<ApplicationInformation>" + AppDomain.CurrentDomain.FriendlyName + "</ApplicationInformation>\r\n  " +
                                   "<UserIdentity>" + WindowsIdentity.GetCurrent().Name + "</UserIdentity>\r\n  " +
                                   "<ThreadIdentity>" + Thread.CurrentThread.ManagedThreadId.ToString(CultureInfo.InvariantCulture) + "</ThreadIdentity>\r\n";

            return intrinsicInfo;
        }

        public static string GetLastEventLogEntry()
        {
            using (EventLog log = new EventLog(EventLogName))
            {
                return log.Entries[log.Entries.Count - 1].Message;
            }
        }

        public static string GetLastEventLogEntryCustom()
        {
            EventLog log = GetCustomEventLog();
            return log.Entries[log.Entries.Count - 1].Message;
        }

        static string GetMessageWithDictionaryXml()
        {
            try
            {
                return "<EntLibMessage><Category></Category><Priority>-1</Priority><Header></Header>" +
                       "<EventID>0</EventID><Body></Body><DateTime>12/31/9999 11:59:59 PM</DateTime><Severity>Unspecified</Severity><MachineName>machine</MachineName>" +
                       "<ExtendedProperties>key1[===]value1[|||]key3[===]value3[|||]key2[===]value2</ExtendedProperties></EntLibMessage>";
            }
            catch {}

            return "";
        }

        public static int GetNumberOfMessagesOnQueue()
        {
            using (MessageQueue queue = new MessageQueue(MessageQueuePath))
            {
                Message[] messages = queue.GetAllMessages();
                return messages.Length;
            }
        }

        public static CounterSample GetPerformanceCounterSample(string categoryName,
                                                                string instanceName,
                                                                string counterName)
        {
            using (PerformanceCounter counter = new PerformanceCounter())
            {
                counter.CategoryName = categoryName;
                counter.CounterName = counterName;
                counter.InstanceName = instanceName;
                return counter.NextSample();
            }
        }

        public static long GetPerformanceCounterValue(string categoryName,
                                                      string instanceName,
                                                      string counterName)
        {
            if (PerformanceCounterCategory.InstanceExists(instanceName, categoryName))
            {
                return GetPerformanceCounterSample(categoryName, instanceName, counterName).RawValue;
            }
            return 0;
        }

        public static string GetProcessName()
        {
            StringBuilder buffer = new StringBuilder(1024);
            int length = NativeMethods.GetModuleFileName(NativeMethods.GetModuleHandle(null), buffer, buffer.Capacity);
            Debug.Assert(length > 0);
            return buffer.ToString();
        }

        public static Dictionary<string, object> GetPropertiesDictionary()
        {
            Dictionary<string, object> hash = new Dictionary<string, object>();
            hash["key1"] = "value1";
            hash["key2"] = "value2";
            hash["key3"] = "value3";

            return hash;
        }

        public static string GetTimeZone()
        {
            int hours = TimeZone.CurrentTimeZone.GetUtcOffset(new DateTime(1999, 1, 1)).Hours;
            return hours.ToString("00") + ":00";
        }

        public static bool LogEntryExists(string message)
        {
            // confirm listener started begin message written
            using (EventLog log = new EventLog(EventLogName))
            {
                string expected = message;
                string entry = log.Entries[log.Entries.Count - 1].Message;
                return (entry.IndexOf(expected) > -1);
            }
        }

        public static string ReadEventLogEntryBody()
        {
            using (EventLog log = new EventLog(EventLogName))
            {
                return log.Entries[log.Entries.Count - 1].Message;
            }
        }

        public static void ResetEventLogCounter()
        {
            using (EventLog log = new EventLog(EventLogName))
            {
                eventLogEntryCounter = log.Entries.Count;
            }
        }

        public static void ResetEventLogCounterCustom()
        {
            eventLogEntryCounterCustom = GetCustomEventLog().Entries.Count;
        }

        public static IConfigurationSource SaveSectionsAndGetConfigurationSource(LoggingSettings loggingSettings)
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = "test.exe.config";
            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            rwConfiguration.Sections.Remove(LoggingSettings.SectionName);
            rwConfiguration.Sections.Add(LoggingSettings.SectionName, loggingSettings);

            File.SetAttributes(fileMap.ExeConfigFilename, FileAttributes.Normal);
            rwConfiguration.Save();
            rwConfiguration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            FileConfigurationSource.ResetImplementation(fileMap.ExeConfigFilename, false);
            return new FileConfigurationSource(fileMap.ExeConfigFilename);
        }
    }
}
