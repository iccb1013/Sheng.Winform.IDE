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

using System.Collections.Generic;
using System.Configuration;
using System.Security;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests
{
    /// <summary>
    /// Summary description for SystemConfigurationSourceImplementationFixture
    /// </summary>
    [TestClass]
    public class SystemConfigurationSourceImplementationFixture
    {
        const string nonExistingSection = "dummy.nonexisting";
        const string localSection = "dummy.local";
        const string localSection2 = "dummy.local2";
        const string externalSection = "dummy.external";
        const string externalSection2 = "dummy.external2";
        const string localSectionSource = "";
        const string externalSectionSource = "dummy.external.config";
        const string externalSectionSourceAlt = "dummy.external.alt.config";
        const string externalSection2Source = "dummy.external2.config";

        IDictionary<string, int> updatedSectionsTally;

        [TestInitialize]
        public void Setup()
        {
            SystemConfigurationSource.ResetImplementation(false);

            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            DummySection rwSection;

            rwConfiguration.Sections.Remove(localSection);
            rwConfiguration.Sections.Add(localSection, rwSection = new DummySection());
            rwSection.Name = localSection;
            rwSection.Value = 10;
            rwSection.SectionInformation.ConfigSource = localSectionSource;

            rwConfiguration.Sections.Remove(externalSection);
            rwConfiguration.Sections.Add(externalSection, rwSection = new DummySection());
            rwSection.Name = externalSection;
            rwSection.Value = 20;
            rwSection.SectionInformation.ConfigSource = externalSectionSource;

            rwConfiguration.Sections.Remove(localSection2);
            rwConfiguration.Sections.Add(localSection2, rwSection = new DummySection());
            rwSection.Name = localSection2;
            rwSection.Value = 30;
            rwSection.SectionInformation.ConfigSource = localSectionSource;

            rwConfiguration.Save();

            ConfigurationManager.RefreshSection(localSection);
            ConfigurationManager.RefreshSection(localSection2);
            ConfigurationManager.RefreshSection(externalSection);

            ConfigurationChangeFileWatcher.ResetDefaultPollDelay();

            updatedSectionsTally = new Dictionary<string, int>(0);
        }

        [TestMethod]
        public void CanGetExistingSectionInAppConfig()
        {
            SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(false);
            object section = implementation.GetSection(localSection);

            Assert.IsNotNull(section);
        }

        [TestMethod]
        public void CanGetExistingSectionInAppConfigEvenIfTheAppDomainDoesNotHaveFileIOPermission()
        {
            try
            {
                new FileIOPermission(PermissionState.Unrestricted).Deny();

                CanGetExistingSectionInAppConfig();
            }
            finally
            {
                CodeAccessPermission.RevertDeny();
            }
        }

        [TestMethod]
        public void CanGetExistingSectionInExternalFile()
        {
            SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(false);
            object section = implementation.GetSection(externalSection);

            Assert.IsNotNull(section);
        }

        [TestMethod]
        public void GetsNullIfSectionDoesNotExist()
        {
            SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(false);
            object section = implementation.GetSection(nonExistingSection);

            Assert.IsNull(section);
        }

        [TestMethod]
        public void NewInstanceHasNoWatchers()
        {
            SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(false);

            Assert.AreEqual(0, implementation.WatchedConfigSources.Count);
            Assert.AreEqual(0, implementation.WatchedSections.Count);
        }

        [TestMethod]
        public void RequestForNonexistentSectionCreatesNoWatcher()
        {
            SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(false);

            object section = implementation.GetSection(nonExistingSection);

            Assert.IsNull(section);
            Assert.AreEqual(0, implementation.WatchedConfigSources.Count);
            Assert.AreEqual(0, implementation.WatchedSections.Count);
        }

        [TestMethod]
        public void FirstRequestForSectionInAppConfigCreatesWatcherForAppConfig()
        {
            SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(true);

            object section = implementation.GetSection(localSection);

            Assert.IsNotNull(section);
            Assert.AreEqual(1, implementation.WatchedConfigSources.Count);
            Assert.IsTrue(implementation.WatchedConfigSources.Contains(localSectionSource));
            Assert.AreEqual(1, implementation.WatchedSections.Count);
            Assert.IsTrue(implementation.WatchedSections.Contains(localSection));

            Assert.IsNotNull(implementation.ConfigSourceWatcherMappings[localSectionSource].Watcher);
            Assert.AreEqual(implementation.ConfigSourceWatcherMappings[localSectionSource].Watcher.GetType(), typeof(ConfigurationChangeFileWatcher));

            implementation.Dispose();
        }

        [TestMethod]
        public void SecondRequestForSameSectionInAppConfigDoesNotCreateSecondWatcherForAppConfig()
        {
            SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(false);

            object section1 = implementation.GetSection(localSection);
            object section2 = implementation.GetSection(localSection);

            Assert.IsNotNull(section1);
            Assert.IsNotNull(section2);
            Assert.AreEqual(1, implementation.WatchedConfigSources.Count);
            Assert.IsTrue(implementation.WatchedConfigSources.Contains(localSectionSource));
            Assert.AreEqual(1, implementation.WatchedSections.Count);
            Assert.IsTrue(implementation.WatchedSections.Contains(localSection));
        }

        [TestMethod]
        public void SecondRequestForDifferentSectionInAppConfigDoesNotCreateSecondWatcherForAppConfig()
        {
            SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(false);

            object section1 = implementation.GetSection(localSection);
            object section2 = implementation.GetSection(localSection2);

            Assert.IsNotNull(section1);
            Assert.IsNotNull(section2);
            Assert.AreEqual(1, implementation.WatchedConfigSources.Count);
            Assert.IsTrue(implementation.WatchedConfigSources.Contains(localSectionSource));
            Assert.AreEqual(2, implementation.WatchedSections.Count);
            Assert.IsTrue(implementation.WatchedSections.Contains(localSection));
            Assert.IsTrue(implementation.WatchedSections.Contains(localSection2));
        }

        [TestMethod]
        public void FirstRequestForSectionInExternalFileCreatesWatchersForExternalFileAndAppConfig()
        {
            SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(false);

            object section = implementation.GetSection(externalSection);

            Assert.IsNotNull(section);
            Assert.AreEqual(2, implementation.WatchedConfigSources.Count);
            Assert.IsTrue(implementation.WatchedConfigSources.Contains(localSectionSource));
            Assert.IsTrue(implementation.WatchedConfigSources.Contains(externalSectionSource));
            Assert.AreEqual(1, implementation.WatchedSections.Count);
            Assert.IsTrue(implementation.WatchedSections.Contains(externalSection));
        }

        [TestMethod]
        public void SecondRequestForSameSectionInExternalFileDoesNotCreateWatcherForExternalFile()
        {
            SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(false);

            object section1 = implementation.GetSection(externalSection);
            object section2 = implementation.GetSection(externalSection);

            Assert.IsNotNull(section1);
            Assert.IsNotNull(section2);
            Assert.AreEqual(2, implementation.WatchedConfigSources.Count);
            Assert.IsTrue(implementation.WatchedConfigSources.Contains(localSectionSource));
            Assert.IsTrue(implementation.WatchedConfigSources.Contains(externalSectionSource));
            Assert.AreEqual(1, implementation.WatchedSections.Count);
            Assert.IsTrue(implementation.WatchedSections.Contains(externalSection));
        }

        [TestMethod]
        public void RequestsForAppConfigAndExternalFileCreatesWatchersForBoth()
        {
            SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(false);

            object section1 = implementation.GetSection(localSection);
            object section2 = implementation.GetSection(externalSection);

            Assert.IsNotNull(section1);
            Assert.IsNotNull(section2);
            Assert.AreEqual(2, implementation.WatchedConfigSources.Count);
            Assert.IsTrue(implementation.WatchedConfigSources.Contains(localSectionSource));
            Assert.IsTrue(implementation.WatchedConfigSources.Contains(externalSectionSource));
            Assert.AreEqual(2, implementation.WatchedSections.Count);
            Assert.IsTrue(implementation.WatchedSections.Contains(localSection));
            Assert.IsTrue(implementation.WatchedSections.Contains(externalSection));
        }

        [TestMethod]
        public void WatchedSectionInAppConfigValuesAreUpdatedIfAppConfigChangesAndNotificationIsFired()
        {
            SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(false);

            object section1 = implementation.GetSection(localSection);
            Assert.IsNotNull(section1);
            DummySection dummySection1 = section1 as DummySection;
            Assert.AreEqual(localSection, dummySection1.Name);
            Assert.AreEqual(10, dummySection1.Value);

            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            DummySection rwSection = rwConfiguration.GetSection(localSection) as DummySection;
            rwSection.Value = 15;
            rwConfiguration.Save();

            implementation.ConfigSourceChanged(localSectionSource);

            section1 = implementation.GetSection(localSection);
            Assert.IsNotNull(section1);
            dummySection1 = section1 as DummySection;
            Assert.AreEqual(localSection, dummySection1.Name);
            Assert.AreEqual(15, dummySection1.Value);
        }

        [TestMethod]
        public void WatchedSectionInExternalFileValuesAreUpdatedIfExternalFileChangesAndNotificationIsFired()
        {
            SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(false);

            object section1 = implementation.GetSection(externalSection);
            Assert.IsNotNull(section1);
            DummySection dummySection1 = section1 as DummySection;
            Assert.AreEqual(externalSection, dummySection1.Name);
            Assert.AreEqual(20, dummySection1.Value);

            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            DummySection rwSection = rwConfiguration.GetSection(externalSection) as DummySection;
            rwSection.Value = 25;
            rwConfiguration.Save();

            implementation.ExternalConfigSourceChanged(externalSectionSource);

            section1 = implementation.GetSection(externalSection);
            Assert.IsNotNull(section1);
            dummySection1 = section1 as DummySection;
            Assert.AreEqual(externalSection, dummySection1.Name);
            Assert.AreEqual(25, dummySection1.Value);
        }

        [TestMethod]
        public void WatchedExistingSectionIsNoLongerWatchedIfRemovedFromConfiguration()
        {
            SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(false);
            DummySection dummySection1 = implementation.GetSection(localSection) as DummySection;
            DummySection dummySection2 = implementation.GetSection(localSection2) as DummySection;
            Assert.IsNotNull(dummySection1);
            Assert.IsNotNull(dummySection2);
            Assert.AreEqual(1, implementation.WatchedConfigSources.Count);
            Assert.IsTrue(implementation.WatchedConfigSources.Contains(localSectionSource));
            Assert.AreEqual(2, implementation.WatchedSections.Count);
            Assert.IsTrue(implementation.WatchedSections.Contains(localSection));
            Assert.IsTrue(implementation.WatchedSections.Contains(localSection2));

            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            rwConfiguration.Sections.Remove(localSection2);
            rwConfiguration.Save();

            implementation.ConfigSourceChanged(localSectionSource);

            Assert.AreEqual(2, implementation.WatchedConfigSources.Count);
            Assert.IsTrue(implementation.WatchedConfigSources.Contains(localSectionSource));
            Assert.IsTrue(implementation.WatchedConfigSources.Contains(SystemConfigurationSourceImplementation.NullConfigSource));
            Assert.AreEqual(2, implementation.WatchedSections.Count);
            Assert.IsTrue(implementation.WatchedSections.Contains(localSection));
            Assert.IsTrue(implementation.WatchedSections.Contains(localSection2));
            Assert.AreEqual(1, implementation.ConfigSourceWatcherMappings[string.Empty].WatchedSections.Count);
            Assert.IsTrue(implementation.ConfigSourceWatcherMappings[string.Empty].WatchedSections.Contains(localSection));
            Assert.AreEqual(1, implementation.ConfigSourceWatcherMappings[SystemConfigurationSourceImplementation.NullConfigSource].WatchedSections.Count);
            Assert.IsTrue(implementation.ConfigSourceWatcherMappings[SystemConfigurationSourceImplementation.NullConfigSource].WatchedSections.Contains(localSection2));
        }

        [TestMethod]
        public void WatchedExistingSectionInExternalFileIsNoLongerWatchedIfRemovedFromConfigurationAndExternalFileWatcherIsRemoved()
        {
            SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(false);
            DummySection dummySection1 = implementation.GetSection(localSection) as DummySection;
            DummySection dummySection2 = implementation.GetSection(externalSection) as DummySection;
            Assert.IsNotNull(dummySection1);
            Assert.IsNotNull(dummySection2);
            Assert.AreEqual(2, implementation.WatchedConfigSources.Count);
            Assert.IsTrue(implementation.WatchedConfigSources.Contains(localSectionSource));
            Assert.IsTrue(implementation.WatchedConfigSources.Contains(externalSectionSource));
            Assert.AreEqual(2, implementation.WatchedSections.Count);
            Assert.IsTrue(implementation.WatchedSections.Contains(localSection));
            Assert.IsTrue(implementation.WatchedSections.Contains(externalSection));

            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            rwConfiguration.Sections.Remove(externalSection);
            rwConfiguration.Save();

            implementation.ConfigSourceChanged(localSectionSource);

            Assert.AreEqual(2, implementation.WatchedConfigSources.Count);
            Assert.IsTrue(implementation.WatchedConfigSources.Contains(localSectionSource));
            Assert.IsTrue(implementation.WatchedConfigSources.Contains(SystemConfigurationSourceImplementation.NullConfigSource));
            Assert.AreEqual(2, implementation.WatchedSections.Count);
            Assert.IsTrue(implementation.WatchedSections.Contains(localSection));
            Assert.IsTrue(implementation.WatchedSections.Contains(externalSection));
            Assert.AreEqual(1, implementation.ConfigSourceWatcherMappings[string.Empty].WatchedSections.Count);
            Assert.IsTrue(implementation.ConfigSourceWatcherMappings[string.Empty].WatchedSections.Contains(localSection));
            Assert.AreEqual(1, implementation.ConfigSourceWatcherMappings[SystemConfigurationSourceImplementation.NullConfigSource].WatchedSections.Count);
            Assert.IsTrue(implementation.ConfigSourceWatcherMappings[SystemConfigurationSourceImplementation.NullConfigSource].WatchedSections.Contains(externalSection));
        }

        //[Ignore("")]		// System.Configuration won't pick this change up
        //[TestMethod]
        //public void WatchedSectionInAppConfigValuesAreUpdatedIfAppConfigChangesAutomatically()
        //{
        //    ConfigurationChangeWatcher.SetDefaultPollDelayInMilliseconds(100);

        //    SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(true);

        //    object section1 = implementation.GetSection(localSection);
        //    Assert.IsNotNull(section1);
        //    DummySection dummySection1 = section1 as DummySection;
        //    Assert.AreEqual(localSection, dummySection1.Name);
        //    Assert.AreEqual(10, dummySection1.Value);

        //    System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        //    DummySection rwSection = rwConfiguration.GetSection(localSection) as DummySection;
        //    rwSection.Value = 15;
        //    rwConfiguration.Save();

        //    Thread.Sleep(150);

        //    section1 = implementation.GetSection(localSection);
        //    Assert.IsNotNull(section1);
        //    dummySection1 = section1 as DummySection;
        //    Assert.AreEqual(localSection, dummySection1.Name);
        //    Assert.AreEqual(15, dummySection1.Value);
        //}

        //[Ignore("")]		// System.Configuration won't pick this change up
        //[TestMethod]
        //public void WatchedSectionInExternalFileValuesAreUpdatedIfExternalFileChangesAutomatically()
        //{
        //    ConfigurationChangeWatcher.SetDefaultPollDelayInMilliseconds(100);

        //    SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(true);

        //    object section1 = implementation.GetSection(externalSection);
        //    Assert.IsNotNull(section1);
        //    DummySection dummySection1 = section1 as DummySection;
        //    Assert.AreEqual(externalSection, dummySection1.Name);
        //    Assert.AreEqual(20, dummySection1.Value);

        //    System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        //    DummySection rwSection = rwConfiguration.GetSection(externalSection) as DummySection;
        //    rwSection.Value = 25;
        //    rwConfiguration.Save();

        //    Thread.Sleep(150);

        //    section1 = implementation.GetSection(externalSection);
        //    Assert.IsNotNull(section1);
        //    dummySection1 = section1 as DummySection;
        //    Assert.AreEqual(externalSection, dummySection1.Name);
        //    Assert.AreEqual(25, dummySection1.Value);
        //}

        //[Ignore("")]		// System.Configuration won't pick this change up
        //[TestMethod]
        //public void WatchedSectionChangingFromAppConfigToExternalFileIsAppropriatelyWatched()
        //{
        //}

        //[Ignore("")]		// System.Configuration won't pick this change up
        //[TestMethod]
        //public void WatchedSectionChangingFromExternalFileToAppConfigIsAppropriatelyWatched()
        //{
        //    SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(false);

        //    object section1 = implementation.GetSection(externalSection);
        //    Assert.IsNotNull(section1);
        //    DummySection dummySection1 = section1 as DummySection;
        //    Assert.AreEqual(externalSection, dummySection1.Name);
        //    Assert.AreEqual(20, dummySection1.Value);
        //    Assert.AreEqual(externalSectionSource, dummySection1.SectionInformation.ConfigSource);

        //    System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        //    DummySection rwSection = rwConfiguration.GetSection(externalSection) as DummySection;
        //    rwSection.Value = 25;
        //    rwSection.SectionInformation.ConfigSource = localSectionSource;
        //    rwConfiguration.Save();

        //    // what changed is the app.config/web.config file
        //    implementation.ConfigSourceChanged(localSectionSource);

        //    section1 = implementation.GetSection(externalSection);
        //    Assert.IsNotNull(section1);
        //    dummySection1 = section1 as DummySection;
        //    Assert.AreEqual(externalSection, dummySection1.Name);
        //    Assert.AreEqual(25, dummySection1.Value);
        //    Assert.AreEqual(localSectionSource, dummySection1.SectionInformation.ConfigSource);
        //}

        [TestMethod]
        public void RegisteredObjectIsNotifiedOfSectionChangesForAppConfig()
        {
            SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(false);
            implementation.GetSection(localSection);
            implementation.AddSectionChangeHandler(localSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));

            implementation.ConfigSourceChanged(localSectionSource);

            Assert.AreEqual(1, updatedSectionsTally[localSection]);
        }

        [TestMethod]
        public void AllRegisteredObjectsAreNotifiedOfSectionChangesForAppConfig()
        {
            SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(false);
            implementation.GetSection(localSection);
            implementation.AddSectionChangeHandler(localSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            implementation.AddSectionChangeHandler(localSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            implementation.AddSectionChangeHandler(localSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));

            implementation.ConfigSourceChanged(localSectionSource);

            Assert.AreEqual(3, updatedSectionsTally[localSection]);
        }

        [TestMethod]
        public void RegisteredObjectForNonRequestedSectionIsNotNotified()
        {
            SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(false);
            implementation.AddSectionChangeHandler(localSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));

            implementation.ConfigSourceChanged(localSectionSource);

            Assert.IsFalse(updatedSectionsTally.ContainsKey(localSection));
        }

        [TestMethod]
        public void AllRegisteredObjectsAreNotifiedOfDifferentSectionsChangesForAppConfig()
        {
            SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(false);
            implementation.GetSection(localSection);
            implementation.GetSection(localSection2);
            implementation.AddSectionChangeHandler(localSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            implementation.AddSectionChangeHandler(localSection2, new ConfigurationChangedEventHandler(OnConfigurationChanged));

            implementation.ConfigSourceChanged(localSectionSource);

            Assert.AreEqual(1, updatedSectionsTally[localSection]);
            Assert.AreEqual(1, updatedSectionsTally[localSection2]);
        }

        [TestMethod]
        public void RegisteredObjectIsNotifiedOfSectionChangesForExternalFile()
        {
            SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(false);
            implementation.GetSection(externalSection);
            implementation.AddSectionChangeHandler(externalSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));

            implementation.ExternalConfigSourceChanged(externalSectionSource);

            Assert.AreEqual(1, updatedSectionsTally[externalSection]);
        }

        [TestMethod]
        public void AllRegisteredObjectsAreNotifiedOfSectionChangesForExternalFile()
        {
            SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(false);
            implementation.GetSection(externalSection);
            implementation.AddSectionChangeHandler(externalSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            implementation.AddSectionChangeHandler(externalSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            implementation.AddSectionChangeHandler(externalSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));

            implementation.ExternalConfigSourceChanged(externalSectionSource);

            Assert.AreEqual(3, updatedSectionsTally[externalSection]);
        }

        [TestMethod]
        public void RegisteredObjectForExternalFileIsNotNotifiedOfSectionChangesForAppConfigIfConfigSourceForExternalSectionHasNotChanged()
        {
            SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(false);
            implementation.GetSection(externalSection);
            implementation.AddSectionChangeHandler(externalSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));

            implementation.ConfigSourceChanged(localSectionSource);

            Assert.IsFalse(updatedSectionsTally.ContainsKey(externalSection));
        }

        [TestMethod]
        public void RegisteredObjectForExternalFileIsNotifiedOfSectionChangesForAppConfigIfConfigSourceForExternalSectionNotChanged()
        {
            SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(false);
            implementation.GetSection(externalSection);
            implementation.AddSectionChangeHandler(externalSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));

            implementation.ConfigSourceChanged(localSectionSource);

            Assert.IsFalse(updatedSectionsTally.ContainsKey(externalSection));
        }

        [TestMethod]
        public void CanAddAndRemoveHandlers()
        {
            SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(false);
            object section = implementation.GetSection(externalSection);
            Assert.IsNotNull(section);

            implementation.AddSectionChangeHandler(externalSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            implementation.AddSectionChangeHandler(externalSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            implementation.ExternalConfigSourceChanged(externalSectionSource);
            Assert.AreEqual(2, updatedSectionsTally[externalSection]);

            implementation.RemoveSectionChangeHandler(externalSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            implementation.ExternalConfigSourceChanged(externalSectionSource);
            Assert.AreEqual(3, updatedSectionsTally[externalSection]);

            implementation.RemoveSectionChangeHandler(externalSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            implementation.ExternalConfigSourceChanged(externalSectionSource);
            Assert.AreEqual(3, updatedSectionsTally[externalSection]);
        }

        // Watchers are removed if section changed source and none left (except for "").
        // watchers aren't removed if section changed source and other left.
        [TestMethod]
        public void FirstRequestForSectionGetsFreshInformation()
        {
            DummySection section1, section2;

            section1 = ConfigurationManager.GetSection(localSection) as DummySection;
            Assert.AreEqual(10, section1.Value);

            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            DummySection rwSection1 = rwConfiguration.GetSection(localSection) as DummySection;
            rwSection1.Value = 15;
            DummySection rwSection2 = rwConfiguration.GetSection(localSection2) as DummySection;
            rwSection2.Value = 25;
            rwConfiguration.Save();

            section1 = ConfigurationManager.GetSection(localSection) as DummySection;
            section2 = ConfigurationManager.GetSection(localSection2) as DummySection;
            Assert.AreEqual(10, section1.Value); // gets old value for cached section
            Assert.AreEqual(25, section2.Value); // gets new value for fresh section
        }

        [TestMethod]
        public void RemovedSectionGetsNotificationOnRemovalAndDoesNotGetFurtherNotifications()
        {
            ConfigurationChangeWatcher.SetDefaultPollDelayInMilliseconds(100);

            SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(false);

            object section1 = implementation.GetSection(localSection);
            Assert.IsNotNull(section1);
            object section2 = implementation.GetSection(localSection2);
            Assert.IsNotNull(section2);

            implementation.AddSectionChangeHandler(localSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            implementation.AddSectionChangeHandler(localSection2, new ConfigurationChangedEventHandler(OnConfigurationChanged));

            // a change in system config notifies both sections
            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            rwConfiguration.Save();

            implementation.ConfigSourceChanged(localSectionSource);
            Assert.AreEqual(1, updatedSectionsTally[localSection]);
            Assert.AreEqual(1, updatedSectionsTally[localSection2]);

            // removal of the section notifies both sections
            rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            rwConfiguration.Sections.Remove(localSection2);
            rwConfiguration.Save();

            implementation.ConfigSourceChanged(localSectionSource);
            Assert.AreEqual(2, updatedSectionsTally[localSection]);
            Assert.AreEqual(2, updatedSectionsTally[localSection2]);

            // further updates only notify the remaining section
            rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            rwConfiguration.Save();

            implementation.ConfigSourceChanged(localSectionSource);
            Assert.AreEqual(3, updatedSectionsTally[localSection]);
            Assert.AreEqual(2, updatedSectionsTally[localSection2]);
        }

        [TestMethod]
        public void RestoredSectionGetsNotificationOnRestoreAndGetsFurtherNotifications()
        {
            SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(false);

            object section1 = implementation.GetSection(localSection);
            Assert.IsNotNull(section1);
            object section2 = implementation.GetSection(localSection2);
            Assert.IsNotNull(section2);

            implementation.AddSectionChangeHandler(localSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            implementation.AddSectionChangeHandler(localSection2, new ConfigurationChangedEventHandler(OnConfigurationChanged));

            // a change in system config notifies both sections
            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            rwConfiguration.Save();

            implementation.ConfigSourceChanged(localSectionSource);
            Assert.AreEqual(1, updatedSectionsTally[localSection]);
            Assert.AreEqual(1, updatedSectionsTally[localSection2]);

            // removal of the section notifies both sections
            rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            rwConfiguration.Sections.Remove(localSection2);
            rwConfiguration.Save();

            implementation.ConfigSourceChanged(localSectionSource);
            Assert.AreEqual(2, updatedSectionsTally[localSection]);
            Assert.AreEqual(2, updatedSectionsTally[localSection2]);

            // further updates only notify the remaining section
            rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            rwConfiguration.Save();

            implementation.ConfigSourceChanged(localSectionSource);
            Assert.AreEqual(3, updatedSectionsTally[localSection]);
            Assert.AreEqual(2, updatedSectionsTally[localSection2]);

            // restore of section gets notified
            DummySection rwSection = new DummySection();
            rwSection.Name = localSection2;
            rwSection.Value = 30;
            rwSection.SectionInformation.ConfigSource = localSectionSource;
            rwConfiguration.Sections.Add(localSection2, rwSection);
            rwConfiguration.Save();

            implementation.ConfigSourceChanged(localSectionSource);
            Assert.AreEqual(4, updatedSectionsTally[localSection]);
            Assert.AreEqual(3, updatedSectionsTally[localSection2]);

            // further updates notify both sections
            rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            rwConfiguration.Save();

            implementation.ConfigSourceChanged(localSectionSource);
            Assert.AreEqual(5, updatedSectionsTally[localSection]);
            Assert.AreEqual(4, updatedSectionsTally[localSection2]);
        }

        void OnConfigurationChanged(object sender,
                                    ConfigurationChangedEventArgs args)
        {
            if (updatedSectionsTally.ContainsKey(args.SectionName))
            {
                updatedSectionsTally[args.SectionName] = updatedSectionsTally[args.SectionName] + 1;
            }
            else
            {
                updatedSectionsTally[args.SectionName] = 1;
            }
        }
    }
}
