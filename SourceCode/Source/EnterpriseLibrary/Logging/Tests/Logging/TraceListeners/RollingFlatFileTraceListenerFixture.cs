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
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners
{
    [TestClass]
    public class RollingFlatFileTraceListenerFixture
    {
        string fileNameWithoutExtension;
        string fileName;
        const string extension = ".log";
        MockDateTimeProvider dateTimeProvider = new MockDateTimeProvider();

        [TestInitialize]
        public void SetUp()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
            fileNameWithoutExtension = Guid.NewGuid().ToString();
            fileName = fileNameWithoutExtension + extension;
        }

        [TestCleanup]
        public void TearDown()
        {
            foreach (string createdFileName in Directory.GetFiles(".", fileNameWithoutExtension + "*"))
            {
                File.Delete(createdFileName);
            }
        }

        [TestMethod]
        public void ListenerForNewFileWillUseCreationDateToCalculateRollDate()
        {
            using (RollingFlatFileTraceListener traceListener
                = new RollingFlatFileTraceListener(fileName, "header", "footer", null,
                                                   0, "yyyy", RollFileExistsBehavior.Overwrite, RollInterval.Day))
            {
                traceListener.RollingHelper.DateTimeProvider = dateTimeProvider;

                Assert.IsTrue(traceListener.RollingHelper.UpdateRollingInformationIfNecessary());

                Assert.AreEqual(traceListener.RollingHelper.CalculateNextRollDate(File.GetCreationTime(fileName)), traceListener.RollingHelper.NextRollDateTime);
                Assert.IsNull(traceListener.RollingHelper.CheckIsRollNecessary());
            }
        }

        [TestMethod]
        public void ListenerForExistingFileWillUseCreationDateToCalculateRollDate()
        {
            File.WriteAllText(fileName, "existing text");
            File.SetCreationTime(fileName, new DateTime(2000, 01, 01));

            using (RollingFlatFileTraceListener traceListener
                = new RollingFlatFileTraceListener(fileName, "header", "footer", null,
                                                   0, "yyyy", RollFileExistsBehavior.Overwrite, RollInterval.Day))
            {
                dateTimeProvider.currentDateTime = new DateTime(2008, 01, 01);
                traceListener.RollingHelper.DateTimeProvider = dateTimeProvider;

                Assert.IsTrue(traceListener.RollingHelper.UpdateRollingInformationIfNecessary());

                Assert.AreEqual(traceListener.RollingHelper.CalculateNextRollDate(File.GetCreationTime(fileName)), traceListener.RollingHelper.NextRollDateTime);
                Assert.AreEqual(dateTimeProvider.CurrentDateTime, traceListener.RollingHelper.CheckIsRollNecessary());
            }
        }

        [TestMethod]
        public void WriterKeepsTally()
        {
            using (RollingFlatFileTraceListener traceListener
                = new RollingFlatFileTraceListener(fileName, "header", "footer", null, 10, "yyyy", RollFileExistsBehavior.Overwrite, RollInterval.Day))
            {
                traceListener.RollingHelper.DateTimeProvider = dateTimeProvider;

                Assert.IsTrue(traceListener.RollingHelper.UpdateRollingInformationIfNecessary());

                traceListener.Write("12345");

                Assert.AreEqual(5L, ((RollingFlatFileTraceListener.TallyKeepingFileStreamWriter)traceListener.Writer).Tally);
            }
        }

        [TestMethod]
        public void RolledFileWillHaveCurrentDateForTimestamp()
        {
            using (RollingFlatFileTraceListener traceListener
                = new RollingFlatFileTraceListener(fileName, "header", "footer", null,
                                                   10, "yyyy", RollFileExistsBehavior.Overwrite, RollInterval.Day))
            {
                traceListener.RollingHelper.DateTimeProvider = dateTimeProvider;
                traceListener.Write("1234567890");

                Assert.IsTrue(traceListener.RollingHelper.UpdateRollingInformationIfNecessary());

                traceListener.RollingHelper.PerformRoll(new DateTime(2007, 01, 01));
                traceListener.Write("12345");

                Assert.AreEqual(5L, ((RollingFlatFileTraceListener.TallyKeepingFileStreamWriter)traceListener.Writer).Tally);
            }

            Assert.IsTrue(File.Exists(fileName));
            Assert.AreEqual("12345", File.ReadAllText(fileName));
            Assert.IsTrue(File.Exists(fileNameWithoutExtension + ".2007" + extension));
            Assert.AreEqual("1234567890", File.ReadAllText(fileNameWithoutExtension + ".2007" + extension));
        }

        [TestMethod]
        public void FallbackFileNameIsUsedForRoll()
        {
            using (FileStream fileStream = File.Open(fileName, FileMode.CreateNew, FileAccess.Write, FileShare.Read))
            {
                using (RollingFlatFileTraceListener traceListener
                    = new RollingFlatFileTraceListener(fileName, "header", "footer", null,
                                                       10, "yyyy", RollFileExistsBehavior.Overwrite, RollInterval.Day))
                {
                    traceListener.RollingHelper.DateTimeProvider = dateTimeProvider;
                    traceListener.Write("1234567890");

                    Assert.IsTrue(traceListener.RollingHelper.UpdateRollingInformationIfNecessary());

                    traceListener.RollingHelper.PerformRoll(new DateTime(2007, 01, 01));
                    traceListener.Write("12345");

                    Assert.AreEqual(5L, ((RollingFlatFileTraceListener.TallyKeepingFileStreamWriter)traceListener.Writer).Tally);
                }
            }
        }

        [TestMethod]
        public void RolledFileWithOverwriteWillOverwriteArchiveFileIfDateTemplateMatches()
        {
            using (RollingFlatFileTraceListener traceListener
                = new RollingFlatFileTraceListener(fileName, "header", "footer", null,
                                                   0, "yyyy", RollFileExistsBehavior.Overwrite, RollInterval.Day))
            {
                traceListener.RollingHelper.DateTimeProvider = dateTimeProvider;
                traceListener.Write("1234567890");

                Assert.IsTrue(traceListener.RollingHelper.UpdateRollingInformationIfNecessary());

                traceListener.RollingHelper.PerformRoll(new DateTime(2007, 01, 01));
                traceListener.Write("12345");

                Assert.IsTrue(traceListener.RollingHelper.UpdateRollingInformationIfNecessary());

                traceListener.RollingHelper.PerformRoll(new DateTime(2007, 01, 01));
                traceListener.Write("abcde");
            }

            Assert.IsTrue(File.Exists(fileName));
            Assert.AreEqual("abcde", File.ReadAllText(fileName));
            Assert.IsTrue(File.Exists(fileNameWithoutExtension + ".2007" + extension));
            Assert.AreEqual("12345", File.ReadAllText(fileNameWithoutExtension + ".2007" + extension));

            string[] archiveFiles = Directory.GetFiles(".", fileNameWithoutExtension + ".2007" + extension + "*");
            Assert.AreEqual(1, archiveFiles.Length);
        }

        [TestMethod]
        public void RolledFileWithOverwriteWillCreateArchiveFileIfDateTemplateDoesNotMatch()
        {
            using (RollingFlatFileTraceListener traceListener
                = new RollingFlatFileTraceListener(fileName, "header", "footer", null,
                                                   0, "yyyy", RollFileExistsBehavior.Overwrite, RollInterval.Day))
            {
                traceListener.RollingHelper.DateTimeProvider = dateTimeProvider;
                traceListener.Write("1234567890");

                Assert.IsTrue(traceListener.RollingHelper.UpdateRollingInformationIfNecessary());

                traceListener.RollingHelper.PerformRoll(new DateTime(2007, 01, 01));
                traceListener.Write("12345");

                Assert.IsTrue(traceListener.RollingHelper.UpdateRollingInformationIfNecessary());

                traceListener.RollingHelper.PerformRoll(new DateTime(2008, 01, 01));
                traceListener.Write("abcde");
            }

            Assert.IsTrue(File.Exists(fileName));
            Assert.AreEqual("abcde", File.ReadAllText(fileName));
            Assert.IsTrue(File.Exists(fileNameWithoutExtension + ".2008" + extension));
            Assert.AreEqual("12345", File.ReadAllText(fileNameWithoutExtension + ".2008" + extension));
            Assert.IsTrue(File.Exists(fileNameWithoutExtension + ".2007" + extension));
            Assert.AreEqual("1234567890", File.ReadAllText(fileNameWithoutExtension + ".2007" + extension));

            string[] archiveFiles = Directory.GetFiles(".", fileNameWithoutExtension + ".2007" + extension + "*");
            Assert.AreEqual(1, archiveFiles.Length);
            archiveFiles = Directory.GetFiles(".", fileNameWithoutExtension + ".2008" + extension + "*");
            Assert.AreEqual(1, archiveFiles.Length);
        }

        [TestMethod]
        public void RolledFileWithOverwriteWillFallBackToUniqueNameIfDateTemplateMatchesButArchiveFileIsInUse()
        {
            string targetArchiveFile = fileNameWithoutExtension + ".2007" + extension;

            using (FileStream stream = File.Open(targetArchiveFile, FileMode.CreateNew, FileAccess.Write, FileShare.Read))
            {
                using (RollingFlatFileTraceListener traceListener
                    = new RollingFlatFileTraceListener(fileName, "header", "footer", null,
                                                       0, "yyyy", RollFileExistsBehavior.Overwrite, RollInterval.Day))
                {
                    traceListener.RollingHelper.DateTimeProvider = dateTimeProvider;
                    traceListener.Write("1234567890");

                    Assert.IsTrue(traceListener.RollingHelper.UpdateRollingInformationIfNecessary());

                    traceListener.RollingHelper.PerformRoll(new DateTime(2007, 01, 01));
                    traceListener.Write("12345");
                }
            }

            Assert.IsTrue(File.Exists(fileName));
            Assert.AreEqual("12345", File.ReadAllText(fileName));
            Assert.IsTrue(File.Exists(targetArchiveFile));
            Assert.AreEqual("", File.ReadAllText(targetArchiveFile)); // couldn't archive

            string[] archiveFiles = Directory.GetFiles(".", targetArchiveFile + "*");
            Assert.AreEqual(2, archiveFiles.Length);
            foreach (string archiveFile in archiveFiles)
            {
                if (!Path.GetFileName(archiveFile).Equals(targetArchiveFile))
                {
                    Assert.AreEqual("1234567890", File.ReadAllText(archiveFile));
                }
            }
        }

        [TestMethod]
        public void RolledFileWithIncrementWillCreateArchiveFileIfDateTemplateDoesNotMatch()
        {
            using (RollingFlatFileTraceListener traceListener
                = new RollingFlatFileTraceListener(fileName, "header", "footer", null,
                                                   0, "yyyy", RollFileExistsBehavior.Increment, RollInterval.Day))
            {
                traceListener.RollingHelper.DateTimeProvider = dateTimeProvider;
                traceListener.Write("1234567890");

                Assert.IsTrue(traceListener.RollingHelper.UpdateRollingInformationIfNecessary());

                traceListener.RollingHelper.PerformRoll(new DateTime(2007, 01, 01));
                traceListener.Write("12345");

                Assert.IsTrue(traceListener.RollingHelper.UpdateRollingInformationIfNecessary());

                traceListener.RollingHelper.PerformRoll(new DateTime(2008, 01, 01));
                traceListener.Write("abcde");
            }

            Assert.IsTrue(File.Exists(fileName));
            Assert.AreEqual("abcde", File.ReadAllText(fileName));
            Assert.IsTrue(File.Exists(fileNameWithoutExtension + ".2008.1" + extension));
            Assert.AreEqual("12345", File.ReadAllText(fileNameWithoutExtension + ".2008.1" + extension));
            Assert.IsTrue(File.Exists(fileNameWithoutExtension + ".2007.1" + extension));
            Assert.AreEqual("1234567890", File.ReadAllText(fileNameWithoutExtension + ".2007.1" + extension));

            string[] archiveFiles = Directory.GetFiles(".", fileNameWithoutExtension + ".2007*" + extension + "*");
            Assert.AreEqual(1, archiveFiles.Length);
            archiveFiles = Directory.GetFiles(".", fileNameWithoutExtension + ".2008*" + extension + "*");
            Assert.AreEqual(1, archiveFiles.Length);
        }

        [TestMethod]
        public void RolledFileWithIncrementWillCreateArchiveFileWithMaxSequenceIfDateTemplateDoesMatch()
        {
            using (RollingFlatFileTraceListener traceListener
                = new RollingFlatFileTraceListener(fileName, "header", "footer", null,
                                                   0, "yyyy", RollFileExistsBehavior.Increment, RollInterval.Day))
            {
                traceListener.RollingHelper.DateTimeProvider = dateTimeProvider;
                traceListener.Write("1234567890");

                Assert.IsTrue(traceListener.RollingHelper.UpdateRollingInformationIfNecessary());

                traceListener.RollingHelper.PerformRoll(new DateTime(2007, 01, 01));
                traceListener.Write("12345");

                Assert.IsTrue(traceListener.RollingHelper.UpdateRollingInformationIfNecessary());

                traceListener.RollingHelper.PerformRoll(new DateTime(2007, 01, 02));
                traceListener.Write("abcde");
            }

            Assert.IsTrue(File.Exists(fileName));
            Assert.AreEqual("abcde", File.ReadAllText(fileName));
            Assert.IsTrue(File.Exists(fileNameWithoutExtension + ".2007.2" + extension));
            Assert.AreEqual("12345", File.ReadAllText(fileNameWithoutExtension + ".2007.2" + extension));
            Assert.IsTrue(File.Exists(fileNameWithoutExtension + ".2007.1" + extension));
            Assert.AreEqual("1234567890", File.ReadAllText(fileNameWithoutExtension + ".2007.1" + extension));

            string[] archiveFiles = Directory.GetFiles(".", fileNameWithoutExtension + ".2007*" + extension + "*");
            Assert.AreEqual(2, archiveFiles.Length);
        }

        [TestMethod]
        public void WillRollForDateIfEnabled()
        {
            using (RollingFlatFileTraceListener traceListener
                = new RollingFlatFileTraceListener(fileName, "header", "footer", null,
                                                   0, "yyyy", RollFileExistsBehavior.Increment, RollInterval.Day))
            {
                traceListener.RollingHelper.DateTimeProvider = dateTimeProvider;

                dateTimeProvider.currentDateTime = DateTime.Now;
                traceListener.RollingHelper.UpdateRollingInformationIfNecessary();

                dateTimeProvider.currentDateTime = DateTime.Now.AddDays(2);
                Assert.IsNotNull(traceListener.RollingHelper.CheckIsRollNecessary());
            }

            using (RollingFlatFileTraceListener traceListener
                = new RollingFlatFileTraceListener(fileName, "header", "footer", null,
                                                   0, "yyyy", RollFileExistsBehavior.Increment, RollInterval.None))
            {
                traceListener.RollingHelper.UpdateRollingInformationIfNecessary();

                traceListener.RollingHelper.DateTimeProvider = dateTimeProvider;

                dateTimeProvider.currentDateTime = DateTime.Now;
                traceListener.RollingHelper.UpdateRollingInformationIfNecessary();

                dateTimeProvider.currentDateTime = DateTime.Now.AddDays(2);
                Assert.IsNull(traceListener.RollingHelper.CheckIsRollNecessary());
            }
        }

        [TestMethod]
        public void WillRollForSize()
        {
            using (RollingFlatFileTraceListener traceListener
                = new RollingFlatFileTraceListener(fileName, "header", "footer", null,
                                                   1, "yyyy", RollFileExistsBehavior.Increment, RollInterval.Year))
            {
                traceListener.RollingHelper.UpdateRollingInformationIfNecessary();

                traceListener.Write(new string('c', 1200));

                Assert.IsNotNull(traceListener.RollingHelper.CheckIsRollNecessary());
            }

            using (RollingFlatFileTraceListener traceListener
                = new RollingFlatFileTraceListener(fileName, "header", "footer", null,
                                                   0, "yyyy", RollFileExistsBehavior.Increment, RollInterval.Year))
            {
                traceListener.RollingHelper.UpdateRollingInformationIfNecessary();

                traceListener.Write(new string('c', 1200));

                Assert.IsNull(traceListener.RollingHelper.CheckIsRollNecessary());
            }
        }

        [TestMethod]
        public void FindsLastSequenceOnFiles()
        {
            for (int i = 0; i < 15; i++)
            {
                if (i % 2 == 0 || i % 3 == 0)
                {
                    string tempfilename = fileNameWithoutExtension + "." + i + extension;
                    File.WriteAllText(tempfilename, "some text");
                }
            }

            int maxSequenceNumber
                = RollingFlatFileTraceListener.StreamWriterRollingHelper.FindMaxSequenceNumber(".",
                                                                                               fileNameWithoutExtension,
                                                                                               extension);

            Assert.AreEqual(14, maxSequenceNumber);
        }

        [TestMethod]
        public void WillNotRollWhenTracingIfNotOverThresholds()
        {
            using (RollingFlatFileTraceListener traceListener
                = new RollingFlatFileTraceListener(fileName, "header", "footer", null,
                                                   0, "yyyy", RollFileExistsBehavior.Increment, RollInterval.Day))
            {
                traceListener.RollingHelper.DateTimeProvider = dateTimeProvider;
            }
        }

        [TestMethod]
        public void WillRollExistingFileIfOverSizeThreshold()
        {
            string existingPayload = new string('c', 5000);
            DateTime currentDateTime = new DateTime(2007, 1, 1);
            File.WriteAllText(fileName, existingPayload);
            File.SetCreationTime(fileName, currentDateTime);

            using (RollingFlatFileTraceListener traceListener
                = new RollingFlatFileTraceListener(fileName, "header", "footer", null,
                                                   1, "yyyy", RollFileExistsBehavior.Overwrite, RollInterval.None))
            {
                traceListener.RollingHelper.DateTimeProvider = dateTimeProvider;
                dateTimeProvider.currentDateTime = currentDateTime;

                traceListener.TraceData(new TraceEventCache(),
                                        "source",
                                        TraceEventType.Error,
                                        0,
                                        "logged message");
            }

            Assert.AreEqual(existingPayload, File.ReadAllText(fileNameWithoutExtension + ".2007" + extension));
            Assert.IsTrue(File.ReadAllText(fileName).Contains("logged message"));
        }

        [TestMethod]
        public void WillRollExistingFileIfOverSizeThresholdAndNoPatternIsSpecifiedForIncrementBehavior()
        {
            string existingPayload = new string('c', 5000);
            DateTime currentDateTime = new DateTime(2007, 1, 1);
            File.WriteAllText(fileName, existingPayload);
            File.SetCreationTime(fileName, currentDateTime);

            using (RollingFlatFileTraceListener traceListener
                = new RollingFlatFileTraceListener(fileName, "header", "footer", null,
                                                   1, "", RollFileExistsBehavior.Increment, RollInterval.None))
            {
                traceListener.RollingHelper.DateTimeProvider = dateTimeProvider;
                dateTimeProvider.currentDateTime = currentDateTime;

                traceListener.TraceData(new TraceEventCache(),
                                        "source",
                                        TraceEventType.Error,
                                        0,
                                        "logged message");
            }

            Assert.AreEqual(existingPayload, File.ReadAllText(fileNameWithoutExtension + ".1" + extension));
            Assert.IsTrue(File.ReadAllText(fileName).Contains("logged message"));
        }

        [TestMethod]
        public void WillTruncateExistingFileIfOverSizeThresholdAndNoPatternIsSpecifiedForOverwriteBehavior()
        {
            string existingPayload = new string('c', 5000);
            DateTime currentDateTime = new DateTime(2007, 1, 1);
            File.WriteAllText(fileName, existingPayload);
            File.SetCreationTime(fileName, currentDateTime);

            using (RollingFlatFileTraceListener traceListener
                = new RollingFlatFileTraceListener(fileName, "header", "footer", null,
                                                   1, "", RollFileExistsBehavior.Overwrite, RollInterval.None))
            {
                traceListener.RollingHelper.DateTimeProvider = dateTimeProvider;
                dateTimeProvider.currentDateTime = currentDateTime;

                traceListener.TraceData(new TraceEventCache(),
                                        "source",
                                        TraceEventType.Error,
                                        0,
                                        "logged message");
            }

            Assert.IsFalse(File.ReadAllText(fileName).Contains(existingPayload));
            Assert.IsTrue(File.ReadAllText(fileName).Contains("logged message"));
        }

        [TestMethod]
        public void WillRollExistingFileIfOverDateThreshold()
        {
            string existingPayload = new string('c', 10);
            DateTime currentDateTime = new DateTime(2007, 1, 1);
            File.WriteAllText(fileName, existingPayload);
            File.SetCreationTime(fileName, currentDateTime);

            using (RollingFlatFileTraceListener traceListener
                = new RollingFlatFileTraceListener(fileName, "header", "footer", null,
                                                   1, "yyyy", RollFileExistsBehavior.Overwrite, RollInterval.Day))
            {
                traceListener.RollingHelper.DateTimeProvider = dateTimeProvider;
                dateTimeProvider.currentDateTime = currentDateTime.AddDays(2);

                traceListener.TraceData(new TraceEventCache(),
                                        "source",
                                        TraceEventType.Error,
                                        0,
                                        "logged message");
            }

            Assert.AreEqual(existingPayload, File.ReadAllText(fileNameWithoutExtension + ".2007" + extension));
            Assert.IsTrue(File.ReadAllText(fileName).Contains("logged message"));
        }

        [TestMethod]
        public void RolledAtMidnight()
        {
            DateTime rollDate = DateTime.Now.AddDays(1).Date;

            using (RollingFlatFileTraceListener traceListener
                = new RollingFlatFileTraceListener(fileName, "header", "footer", null,
                                                   0, "yyyy", RollFileExistsBehavior.Increment, RollInterval.Midnight))
            {
                traceListener.RollingHelper.DateTimeProvider = dateTimeProvider;

                dateTimeProvider.currentDateTime = rollDate;

                traceListener.RollingHelper.UpdateRollingInformationIfNecessary();

                Assert.IsNotNull(traceListener.RollingHelper.NextRollDateTime);
                Assert.IsNotNull(traceListener.RollingHelper.CheckIsRollNecessary());
                Assert.AreEqual(rollDate, traceListener.RollingHelper.NextRollDateTime);
            }
        }

        [TestMethod]
        public void RollingFlatFileTraceListenerReplacedEnviromentVariables()
        {
            string fileName = @"%USERPROFILE%\foo.log";

            string fileNameFromListener = string.Empty;

            RollingFlatFileTraceListener listener = new RollingFlatFileTraceListener(fileName, "header", "footer", null, 1, "", RollFileExistsBehavior.Increment, RollInterval.Day);
            listener.TraceData(new TraceEventCache(), "source", TraceEventType.Error, 1, "This is a test");
            listener.Dispose();

            string expandedFileName = Environment.ExpandEnvironmentVariables(fileName);

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
        public void RollingFlatFileTraceListenerReplacedInexistingEnviromentVariables()
        {
            string fileName = @"%FOO%\%MY_VARIABLE%\foo.log";

            RollingFlatFileTraceListener listener = new RollingFlatFileTraceListener(fileName, "header", "footer", null, 1, "", RollFileExistsBehavior.Increment, RollInterval.Day);
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
        public void RollingFlatFileTraceListenerReplacedEnviromentVariablesWillFallBackIfNotPrivilegesToRead()
        {
            string environmentVariable = "%USERPROFILE%";
            string fileName = Path.Combine(environmentVariable, "foo.log");

            EnvironmentPermission denyPermission = new EnvironmentPermission(PermissionState.Unrestricted);
            denyPermission.Deny();

            try
            {
                RollingFlatFileTraceListener listener = new RollingFlatFileTraceListener(fileName, "header", "footer", null, 1, "", RollFileExistsBehavior.Increment, RollInterval.Day);
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

        class MockDateTimeProvider : RollingFlatFileTraceListener.DateTimeProvider
        {
            public DateTime? currentDateTime = null;

            public override DateTime CurrentDateTime
            {
                get
                {
                    if (currentDateTime != null)
                        return currentDateTime.Value;

                    return base.CurrentDateTime;
                }
            }
        }
    }
}
