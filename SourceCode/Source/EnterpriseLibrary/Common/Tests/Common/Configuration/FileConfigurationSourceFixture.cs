//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using System.IO;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests
{
    [TestClass]
    public class FileConfigurationSourceFixture
    {
        const string localSection = "dummy.local";
        const string localSectionSource = "";
        const string externalSection = "dummy.external";
        const string protectedSection = "dummy.protected";
        const string externalSectionSource = "dummy.external.config";

        [TestMethod]
        public void FileConfigurationAndSystemConfigurationDoNotShareImplementation()
        {
            BaseFileConfigurationSourceImplementation configSourceImpl1 = FileConfigurationSource.GetImplementation(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            BaseFileConfigurationSourceImplementation configSourceImpl2 = SystemConfigurationSource.Implementation;

            Assert.IsFalse(ReferenceEquals(configSourceImpl1, configSourceImpl2));
        }

        [TestMethod]
        public void ConfigurationFilePathsAreResolvedBeforeImplementationIsCreated()
        {
            string fullConfigurationFilepath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            string configurationFilename = Path.GetFileName(fullConfigurationFilepath);

            FileConfigurationSource.ResetImplementation(fullConfigurationFilepath, false);
            BaseFileConfigurationSourceImplementation configSourceImpl1 = new FileConfigurationSource(configurationFilename).Implementation;
            BaseFileConfigurationSourceImplementation configSourceImpl2 = new FileConfigurationSource(fullConfigurationFilepath).Implementation;

            Assert.AreSame(configSourceImpl1, configSourceImpl2);
        }

        [TestMethod]
        public void DifferentConfigationFilesDoNotShareImplementation()
        {
            string fullConfigurationFilepath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            string otherConfigurationFilepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Other.config");
            File.Copy(fullConfigurationFilepath, otherConfigurationFilepath);
            try
            {
                BaseFileConfigurationSourceImplementation configSourceImpl1 = FileConfigurationSource.GetImplementation(fullConfigurationFilepath);
                BaseFileConfigurationSourceImplementation configSourceImpl2 = FileConfigurationSource.GetImplementation(otherConfigurationFilepath);

                Assert.IsFalse(ReferenceEquals(configSourceImpl1, configSourceImpl2));
            }
            finally
            {
                if (File.Exists(otherConfigurationFilepath))
                {
                    File.Delete(otherConfigurationFilepath);
                }
            }
        }

        [TestMethod]
        public void SharedConfigurationFilesCanHaveDifferentCasing()
        {
            string ConfigurationFilepath1 = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile.ToUpper();
            string ConfigurationFilepath2 = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile.ToLower();

            BaseFileConfigurationSourceImplementation configSourceImpl1 = FileConfigurationSource.GetImplementation(ConfigurationFilepath1);
            BaseFileConfigurationSourceImplementation configSourceImpl2 = FileConfigurationSource.GetImplementation(ConfigurationFilepath2);

            Assert.AreSame(configSourceImpl1, configSourceImpl2);
        }

        [TestMethod]
        public void SectionsCanBeAccessedThroughFileConfigurationSource()
        {
            string fullConfigurationFilepath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            string otherConfigurationFilepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Other.config");
            File.Copy(fullConfigurationFilepath, otherConfigurationFilepath);

            try
            {
                FileConfigurationSource.ResetImplementation(otherConfigurationFilepath, false);
                FileConfigurationSource otherConfiguration = new FileConfigurationSource(otherConfigurationFilepath);
                DummySection dummySection = otherConfiguration.GetSection(localSection) as DummySection;

                Assert.IsNotNull(dummySection);
            }
            finally
            {
                if (File.Exists(otherConfigurationFilepath))
                {
                    File.Delete(otherConfigurationFilepath);
                }
            }
        }

        [TestMethod]
        public void NonExistentSectionReturnsNullThroughFileConfigurationSource()
        {
            string fullConfigurationFilepath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            string otherConfigurationFilepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Other.config");
            File.Copy(fullConfigurationFilepath, otherConfigurationFilepath);

            try
            {
                FileConfigurationSource.ResetImplementation(otherConfigurationFilepath, false);
                FileConfigurationSource otherConfiguration = new FileConfigurationSource(otherConfigurationFilepath);
                object wrongSection = otherConfiguration.GetSection("wrong section");

                Assert.IsNull(wrongSection);
            }
            finally
            {
                if (File.Exists(otherConfigurationFilepath))
                {
                    File.Delete(otherConfigurationFilepath);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreatingFileConfigurationSourceWithNullArgumentThrows()
        {
            FileConfigurationSource source = new FileConfigurationSource(null);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CreatingFileConfigurationSourceForNonExistingFileThrows()
        {
            FileConfigurationSource source = new FileConfigurationSource("this.config.file.doesnt.exist.config");
        }

        [TestMethod]
        public void DifferentFileConfigurationSourcesDoNotShareEvents()
        {
            ConfigurationChangeWatcher.SetDefaultPollDelayInMilliseconds(50);
            string otherConfigurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Other.config");

            FileConfigurationSource.ResetImplementation(otherConfigurationFile, true);
            SystemConfigurationSource.ResetImplementation(true);

            File.Copy(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, otherConfigurationFile);

            try
            {
                bool sysSourceChanged = false;
                bool otherSourceChanged = false;

                SystemConfigurationSource systemSource = new SystemConfigurationSource();
                FileConfigurationSource otherSource = new FileConfigurationSource(otherConfigurationFile);

                DummySection sysDummySection = systemSource.GetSection(localSection) as DummySection;
                DummySection otherDummySection = otherSource.GetSection(localSection) as DummySection;
                Assert.IsTrue(sysDummySection != null);
                Assert.IsTrue(otherDummySection != null);

                systemSource.AddSectionChangeHandler(localSection, delegate(object o,
                                                                            ConfigurationChangedEventArgs args)
                                                                   {
                                                                       sysSourceChanged = true;
                                                                   });

                otherSource.AddSectionChangeHandler(localSection, delegate(object o,
                                                                           ConfigurationChangedEventArgs args)
                                                                  {
                                                                      Assert.AreEqual(12, ((DummySection)otherSource.GetSection(localSection)).Value);
                                                                      otherSourceChanged = true;
                                                                  });

                DummySection rwSection = new DummySection();
                System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(otherConfigurationFile);
                rwConfiguration.Sections.Remove(localSection);
                rwConfiguration.Sections.Add(localSection, rwSection = new DummySection());
                rwSection.Name = localSection;
                rwSection.Value = 12;
                rwSection.SectionInformation.ConfigSource = localSectionSource;

                rwConfiguration.SaveAs(otherConfigurationFile);

                Thread.Sleep(200);

                Assert.AreEqual(false, sysSourceChanged);
                Assert.AreEqual(true, otherSourceChanged);
            }
            finally
            {
                if (File.Exists(otherConfigurationFile))
                {
                    File.Delete(otherConfigurationFile);
                }
            }
        }

        [TestMethod]
        public void RemovingSectionCausesChangeNotification()
        {
            ConfigurationChangeWatcher.SetDefaultPollDelayInMilliseconds(50);
            string otherConfigurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Other.config");

            FileConfigurationSource.ResetImplementation(otherConfigurationFile, true);
            File.Copy(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, otherConfigurationFile);

            try
            {
                bool otherSourceChanged = false;

                FileConfigurationSource otherSource = new FileConfigurationSource(otherConfigurationFile);

                DummySection otherDummySection = otherSource.GetSection(localSection) as DummySection;
                Assert.IsTrue(otherDummySection != null);

                otherSource.AddSectionChangeHandler(localSection, delegate(object o,
                                                                           ConfigurationChangedEventArgs args)
                                                                  {
                                                                      Assert.IsNull(otherSource.GetSection(localSection));
                                                                      otherSourceChanged = true;
                                                                  });

                otherSource.Remove(otherConfigurationFile, localSection);

                Thread.Sleep(300);

                Assert.AreEqual(true, otherSourceChanged);
            }
            finally
            {
                if (File.Exists(otherConfigurationFile))
                {
                    File.Delete(otherConfigurationFile);
                }
            }
        }

        [TestMethod]
        public void ReadsLatestVersionOnFirstRequest()
        {
            string otherConfigurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Other.config");

            FileConfigurationSource.ResetImplementation(otherConfigurationFile, false);
            try
            {
                File.Copy(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, otherConfigurationFile);

                FileConfigurationSource otherSource = new FileConfigurationSource(otherConfigurationFile);

                DummySection rwSection = null;
                ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                fileMap.ExeConfigFilename = otherConfigurationFile;
                System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                rwConfiguration.Sections.Remove(localSection);
                rwConfiguration.Sections.Add(localSection, rwSection = new DummySection());
                rwSection.Name = localSection;
                rwSection.Value = 12;
                rwSection.SectionInformation.ConfigSource = localSectionSource;

                rwConfiguration.Save();

                DummySection otherSection = otherSource.GetSection(localSection) as DummySection;
                Assert.IsNotNull(otherSection);
                Assert.AreEqual(12, otherSection.Value);
            }
            finally
            {
                FileConfigurationSource.ResetImplementation(otherConfigurationFile, true);
                if (File.Exists(otherConfigurationFile))
                {
                    File.Delete(otherConfigurationFile);
                }
            }
        }

        [TestMethod]
        public void AddIsReflectedInNextRequestWithoutRefresh()
        {
            string otherConfigurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Other.config");
            FileConfigurationParameter parameter = new FileConfigurationParameter(otherConfigurationFile);

            FileConfigurationSource.ResetImplementation(otherConfigurationFile, false);
            try
            {
                File.Copy(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, otherConfigurationFile);
                FileConfigurationSource otherSource = new FileConfigurationSource(otherConfigurationFile);

                DummySection otherSection = otherSource.GetSection(localSection) as DummySection;

                // update twice, just to make sure
                DummySection newSection = new DummySection();
                newSection.Value = 13;
                otherSource.Add(parameter, localSection, newSection);

                newSection = new DummySection();
                newSection.Value = 12;
                otherSource.Add(parameter, localSection, newSection);

                otherSection = otherSource.GetSection(localSection) as DummySection;
                Assert.IsNotNull(otherSection);
                Assert.AreEqual(12, otherSection.Value);
            }
            finally
            {
                FileConfigurationSource.ResetImplementation(otherConfigurationFile, true);
                if (File.Exists(otherConfigurationFile))
                {
                    File.Delete(otherConfigurationFile);
                }
            }
        }

        [TestMethod]
        public void RemoveIsReflectedInNextRequestWithoutRefresh()
        {
            string otherConfigurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Other.config");
            FileConfigurationParameter parameter = new FileConfigurationParameter(otherConfigurationFile);

            FileConfigurationSource.ResetImplementation(otherConfigurationFile, false);
            try
            {
                File.Copy(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, otherConfigurationFile);
                FileConfigurationSource otherSource = new FileConfigurationSource(otherConfigurationFile);

                DummySection otherSection = otherSource.GetSection(localSection) as DummySection;

                DummySection newSection = new DummySection();
                newSection.Value = 13;
                otherSource.Add(parameter, localSection, newSection);

                otherSource.Remove(parameter, localSection);

                otherSection = otherSource.GetSection(localSection) as DummySection;
                Assert.IsNull(otherSection);
            }
            finally
            {
                FileConfigurationSource.ResetImplementation(otherConfigurationFile, true);
                if (File.Exists(otherConfigurationFile))
                {
                    File.Delete(otherConfigurationFile);
                }
            }
        }

        [TestMethod]
        [Ignore] // This test is broken
        public void ChangeInExternalConfigSourceIsDetected()
        {
            ConfigurationChangeWatcher.SetDefaultPollDelayInMilliseconds(50);
            string otherConfigurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Other.config");
            FileConfigurationParameter parameter = new FileConfigurationParameter(otherConfigurationFile);

            FileConfigurationSource.ResetImplementation(otherConfigurationFile, true);
            try
            {
                File.Copy(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, otherConfigurationFile);
                FileConfigurationSource otherSource = new FileConfigurationSource(otherConfigurationFile);

                DummySection rwSection;
                System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(otherConfigurationFile);
                rwConfiguration.Sections.Remove(externalSection);
                rwConfiguration.Sections.Add(externalSection, rwSection = new DummySection());
                rwSection.Name = externalSection;
                rwSection.Value = 12;
                rwSection.SectionInformation.ConfigSource = externalSectionSource;
                rwConfiguration.Save(ConfigurationSaveMode.Full);

                DummySection otherSection = otherSource.GetSection(externalSection) as DummySection;
                Assert.AreEqual(12, otherSection.Value);

                rwSection.Value = 13;
                rwConfiguration.Save(ConfigurationSaveMode.Modified);

                Thread.Sleep(1000);

                otherSection = otherSource.GetSection(externalSection) as DummySection;
                Assert.AreEqual(13, otherSection.Value);
            }
            finally
            {
                ConfigurationChangeWatcher.ResetDefaultPollDelay();
                FileConfigurationSource.ResetImplementation(otherConfigurationFile, true);
                if (File.Exists(otherConfigurationFile))
                {
                    File.Delete(otherConfigurationFile);
                }
            }
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void SavingWithEmptyProtectionProviderThrowsArgumentException()
        {
            string configurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            FileConfigurationSource fileConfigSource = new FileConfigurationSource(configurationFile);
            FileConfigurationParameter parameter = new FileConfigurationParameter(configurationFile);

            DummySection newSection = new DummySection();

            fileConfigSource.Add(parameter, localSection, newSection, string.Empty);
        }

        [TestMethod]
        public void AddingUnprotectedSectionsWithProtectionProviderWillProtectThem()
        {
            DummySection dummySection = new DummySection();

            string configurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            FileConfigurationSource fileConfigSource = new FileConfigurationSource(configurationFile);
            FileConfigurationParameter parameter = new FileConfigurationParameter(configurationFile);

            fileConfigSource.Add(parameter, protectedSection, dummySection, ProtectedConfiguration.DefaultProvider);

            FileConfigurationSource.ResetImplementation(configurationFile, true);

            ConfigurationSection section = fileConfigSource.GetSection(protectedSection);

            Assert.IsTrue(section.SectionInformation.IsProtected);
            Assert.IsNotNull(section.SectionInformation);
            Assert.AreEqual(ProtectedConfiguration.DefaultProvider, section.SectionInformation.ProtectionProvider.Name);
        }
    }
}
