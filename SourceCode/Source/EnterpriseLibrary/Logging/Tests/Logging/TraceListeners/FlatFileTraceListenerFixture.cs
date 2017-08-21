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
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests
{
    [TestClass]
    public class FlatFileTraceListenerFixture
    {
        [TestInitialize]
        public void Initialize()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
        }

        [TestMethod]
        public void ListenerWillUseFormatterIfExists()
        {
            File.Delete("trace.log");

            FlatFileTraceListener listener = new FlatFileTraceListener("trace.log", new TextFormatter("DUMMY{newline}DUMMY"));

            // need to go through the source to get a TraceEventCache
            LogSource source = new LogSource("notfromconfig", SourceLevels.All);
            source.Listeners.Add(listener);
            source.TraceData(TraceEventType.Error, 0, new LogEntry("message", "cat1", 0, 0, TraceEventType.Error, "title", null));
            source.Dispose();

            string strFileContents = GetFileContents("trace.log");

            Assert.AreEqual("DUMMY" + Environment.NewLine + "DUMMY" + Environment.NewLine, strFileContents);
        }

        [TestMethod]
        public void ListenerWithHeaderAndFooterWillUseFormatterIfExists()
        {
            File.Delete("tracewithheaderandfooter.log");

            FlatFileTraceListener listener = new FlatFileTraceListener("tracewithheaderandfooter.log", "--------header------", "=======footer===========", new TextFormatter("DUMMY{newline}DUMMY"));

            // need to go through the source to get a TraceEventCache
            LogSource source = new LogSource("notfromconfig", SourceLevels.All);
            source.Listeners.Add(listener);
            source.TraceData(TraceEventType.Error, 0, new LogEntry("message", "cat1", 0, 0, TraceEventType.Error, "title", null));
            source.Dispose();

            string strFileContents = GetFileContents("tracewithheaderandfooter.log");

            Assert.AreEqual("--------header------" + Environment.NewLine + "DUMMY" + Environment.NewLine + "DUMMY" + Environment.NewLine + "=======footer===========" + Environment.NewLine, strFileContents);
        }

        [TestMethod]
        public void ListenerWillFallbackToTraceEntryToStringIfFormatterDoesNotExists()
        {
            LogEntry testLogEntry = new LogEntry("message", "cat1", 0, 0, TraceEventType.Error, "title", null);
            StreamWriter writer = new StreamWriter("trace.log");
            FlatFileTraceListener listener = new FlatFileTraceListener(writer);

            // need to go through the source to get a TraceEventCache
            LogSource source = new LogSource("notfromconfig", SourceLevels.All);
            source.Listeners.Add(listener);
            source.TraceData(TraceEventType.Error, 0, testLogEntry);
            source.Dispose();

            string strFileContents = GetFileContents("trace.log");

            string testLogEntryAsString = testLogEntry.ToString();

            Assert.IsTrue(-1 != strFileContents.IndexOf(testLogEntryAsString));
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void FlatFileListenerWillFallbackIfNotPriviledgesToWrite()
        {
            string fileName = @"trace.log";
            string fullPath = String.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), fileName);

            File.Delete(fileName);

            FileIOPermission fileIOPerm1 = new FileIOPermission(PermissionState.None);
            fileIOPerm1.SetPathList(FileIOPermissionAccess.Read, fullPath);
            fileIOPerm1.PermitOnly();

            try
            {
                FlatFileTraceListener listener = new FlatFileTraceListener(fileName, "---header---", "***footer***", new TextFormatter("DUMMY{newline}DUMMY"));

                // need to go through the source to get a TraceEventCache
                LogSource source = new LogSource("notfromconfig", SourceLevels.All);
                source.Listeners.Add(listener);
                source.TraceData(TraceEventType.Error, 0, new LogEntry("message", "cat1", 0, 0, TraceEventType.Error, "title", null));
                source.Dispose();
            }
            catch (SecurityException)
            {
                FileIOPermission.RevertAll();
                throw;
            }
        }

        [TestMethod]
        public void FlatFileListenerReplacedEnviromentVariables()
        {
            string fileName = @"%USERPROFILE%\foo.log";

            string fileNameFromListener = string.Empty;

            FlatFileTraceListener listener = new FlatFileTraceListener(fileName);

            listener.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 1, "This is a test");
            listener.Dispose();

            string expandedFileName = EnvironmentHelper.ReplaceEnvironmentVariables(fileName);

            bool result = File.Exists(expandedFileName);

            Assert.IsTrue(result);

            using (FileStream stream = File.Open(expandedFileName, FileMode.Open))
            {
                fileNameFromListener = stream.Name;
            }

            File.Delete(expandedFileName);

            Assert.AreEqual(expandedFileName, fileNameFromListener);
        }

        [TestMethod]
        public void DontWriteHeaderOrFooterWhenEventsAreFiltered()
        {
            const string header = "MockHeader";
            const string footer = "MockFooter";
            const string fileName = "rolling.log";
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\" + fileName;

            try
            {
                LogEntry log = new LogEntry("Header nor footer written", "Category", 1, 1, TraceEventType.Error, "FilteredEventsDontWriteHeaderNorFooter", null);

                RollingFlatFileTraceListener listener = new RollingFlatFileTraceListener(filePath, header, footer, null, 100, "mmddyyyy", RollFileExistsBehavior.Overwrite, RollInterval.Day);

                listener.Filter = new EventTypeFilter(SourceLevels.Off);
                listener.TraceData(null, "Error", TraceEventType.Error, 1, log);
                listener.Flush();
                listener.Close();

                Assert.IsTrue(File.Exists(filePath));

                StreamReader reader = new StreamReader(filePath);
                string content = reader.ReadToEnd();
                reader.Close();

                Assert.IsFalse(content.Contains(header));
                Assert.IsFalse(content.Contains(footer));
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [TestMethod]
        public void FlatFileListenerReplacedInexistingEnviromentVariables()
        {
            string fileName = @"%FOO%\%MY_VARIABLE%\foo.log";

            FlatFileTraceListener listener = new FlatFileTraceListener(fileName);

            listener.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 1, "This is a test");
            listener.Dispose();

            string fileNameFromListener = string.Empty;
            string expandedFileName = EnvironmentHelper.ReplaceEnvironmentVariables(fileName);
            string expectedFileName = Path.GetFileName(expandedFileName);

            bool result = File.Exists(expandedFileName);

            Assert.IsTrue(result);

            File.Delete(expandedFileName);

            Assert.AreEqual(expectedFileName, expandedFileName);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void FlatFileListenerReplacedEnviromentVariablesWillFallBackIfNotPrivilegesToRead()
        {
            string environmentVariable = "%USERPROFILE%";
            string fileName = Path.Combine(environmentVariable, "foo.log");

            EnvironmentPermission denyPermission = new EnvironmentPermission(PermissionState.Unrestricted);
            denyPermission.Deny();

            try
            {
                FlatFileTraceListener listener = new FlatFileTraceListener(fileName);
                listener.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 1, "This is a test");
                listener.Dispose();
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            finally
            {
                EnvironmentPermission.RevertAll();
            }

            Assert.Fail("Permission was not denied.");
        }

        [TestMethod]
        public void FlatFileTraceListenerMultipleWrites()
        {
            File.Delete("tracewithheaderandfootermultiplewrites.log");
            string header = "--------header------";
            int numberOfWrites = 4;

            FlatFileTraceListener listener = new FlatFileTraceListener("tracewithheaderandfootermultiplewrites.log", header, "=======footer===========", new TextFormatter("DUMMY{newline}DUMMY"));

            // need to go through the source to get a TraceEventCache
            LogSource source = new LogSource("notfromconfig", SourceLevels.All);
            source.Listeners.Add(listener);
            for (int writeLoop = 0; writeLoop < numberOfWrites; writeLoop++)
                source.TraceData(TraceEventType.Error, 0, new LogEntry("message", "cat1", 0, 0, TraceEventType.Error, "title", null));
            source.Dispose();

            StreamReader reader = new StreamReader("tracewithheaderandfootermultiplewrites.log");
            int headersFound = NumberOfItems("tracewithheaderandfootermultiplewrites.log", header);

            Assert.AreEqual(numberOfWrites, headersFound);
        }

        string GetFileContents(string fileName)
        {
            string strFileContents = String.Empty;
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

        int NumberOfItems(string fileName,
                          string item)
        {
            int itemsFound = 0;
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    string strFileContents;
                    while ((strFileContents = reader.ReadLine()) != null)
                    {
                        if (strFileContents.Equals(item))
                            itemsFound++;
                    }
                    reader.Close();
                }
            }
            return itemsFound;
        }
    }
}
