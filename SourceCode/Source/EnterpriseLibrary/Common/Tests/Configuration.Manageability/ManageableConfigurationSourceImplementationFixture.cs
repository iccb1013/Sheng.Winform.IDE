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
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests
{
    [TestClass]
    public class ManageableConfigurationSourceImplementationFixture
    {
        const String localSection1 = "local.section.1";
        const String externalSection1 = "external.section.1";

        String testConfigurationFile;
        System.Configuration.Configuration rwConfiguration;

        MockGroupPolicyNotificationRegistrationBuilder policyRegistrationBuilder;
        GroupPolicyWatcher groupPolicyWatcher;
        ConfigurationChangeWatcherCoordinator watcherCoordinator;
        ConfigurationChangeNotificationCoordinator notificationCoordinator;
        ManageableConfigurationSourceImplementation instance;

        Dictionary<String, String> valuesForNotifiedSections;

        [TestInitialize]
        public void SetUp()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);

            testConfigurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Other.config");
            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
            configMap.ExeConfigFilename = testConfigurationFile;

            File.Copy(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, testConfigurationFile, true);
            rwConfiguration = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

            policyRegistrationBuilder = new MockGroupPolicyNotificationRegistrationBuilder();
            groupPolicyWatcher = new GroupPolicyWatcher(policyRegistrationBuilder);
            watcherCoordinator = new ConfigurationChangeWatcherCoordinator(testConfigurationFile, false);
            notificationCoordinator = new ConfigurationChangeNotificationCoordinator();

            valuesForNotifiedSections = new Dictionary<string, string>();
        }

        [TestCleanup]
        public void TearDown()
        {
            if (File.Exists(testConfigurationFile))
            {
                File.Delete(testConfigurationFile);
            }
        }

        [TestMethod]
        [DeploymentItem("test.external.config")]
        public void CanCreateInstance()
        {
            Dictionary<string, ConfigurationSectionManageabilityProvider> manageabilityProviders
                = new Dictionary<string, ConfigurationSectionManageabilityProvider>();

            instance = new ManageableConfigurationSourceImplementation(testConfigurationFile,
                                                                       false,
                                                                       manageabilityProviders,
                                                                       true,
                                                                       true,
                                                                       "TestApplication");
        }

        [TestMethod]
        [DeploymentItem("test.external.config")]
        public void InstanceCreationFiresManageabilityInitialization()
        {
            MockManageabilityHelper manageabilityHelper = new MockManageabilityHelper();

            instance = new ManageableConfigurationSourceImplementation(testConfigurationFile,
                                                                       manageabilityHelper,
                                                                       groupPolicyWatcher,
                                                                       watcherCoordinator,
                                                                       notificationCoordinator);

            Assert.IsTrue(manageabilityHelper.updateCalled);
            Assert.AreEqual(1, watcherCoordinator.WatchedConfigSources.Count);
            Assert.IsTrue(watcherCoordinator.WatchedConfigSources.Contains(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));
        }

        [TestMethod]
        [DeploymentItem("test.external.config")]
        public void InstanceCreationAddsWatchersForManagedSections()
        {
            MockManageabilityHelper manageabilityHelper = new MockManageabilityHelper("external.section.1");

            instance = new ManageableConfigurationSourceImplementation(testConfigurationFile,
                                                                       manageabilityHelper,
                                                                       groupPolicyWatcher,
                                                                       watcherCoordinator,
                                                                       notificationCoordinator);

            Assert.IsTrue(manageabilityHelper.updateCalled);
            Assert.AreEqual(2, watcherCoordinator.WatchedConfigSources.Count);
            Assert.IsTrue(watcherCoordinator.WatchedConfigSources.Contains(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));
            Assert.IsTrue(watcherCoordinator.WatchedConfigSources.Contains("test.external.config"));
        }

        [TestMethod]
        [DeploymentItem("test.external.config")]
        public void InstanceCreationDoesNotAddWatchersForNonExistingManagedSections()
        {
            MockManageabilityHelper manageabilityHelper = new MockManageabilityHelper("external.section.1.nonexisting");

            instance = new ManageableConfigurationSourceImplementation(testConfigurationFile,
                                                                       manageabilityHelper,
                                                                       groupPolicyWatcher,
                                                                       watcherCoordinator,
                                                                       notificationCoordinator);

            Assert.IsTrue(manageabilityHelper.updateCalled);
            Assert.AreEqual(1, watcherCoordinator.WatchedConfigSources.Count);
            Assert.IsTrue(watcherCoordinator.WatchedConfigSources.Contains(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));
        }

        [TestMethod]
        [DeploymentItem("test.external.config")]
        public void InstanceCreationAttachesConfigurationSettingChangedEvent()
        {
            MockManageabilityHelper manageabilityHelper = new MockManageabilityHelper();

            instance = new ManageableConfigurationSourceImplementation(testConfigurationFile,
                                                                       manageabilityHelper,
                                                                       groupPolicyWatcher,
                                                                       watcherCoordinator,
                                                                       notificationCoordinator);

            Assert.IsTrue(manageabilityHelper.addHandlerCalled);
        }

        [TestMethod]
        [DeploymentItem("test.external.config")]
        public void CanGetSection()
        {
            MockManageabilityHelper manageabilityHelper = new MockManageabilityHelper(localSection1, externalSection1);

            instance = new ManageableConfigurationSourceImplementation(testConfigurationFile,
                                                                       manageabilityHelper,
                                                                       groupPolicyWatcher,
                                                                       watcherCoordinator,
                                                                       notificationCoordinator);

            TestsConfigurationSection testConfigurationSection
                = (TestsConfigurationSection)instance.GetSection(externalSection1);

            Assert.IsNotNull(testConfigurationSection);
            Assert.AreEqual("value1", testConfigurationSection.Value);
        }

        [TestMethod]
        [DeploymentItem("test.external.config")]
        public void WatcherForNonManagedSectionIsSetOnFirstRequest()
        {
            MockManageabilityHelper manageabilityHelper = new MockManageabilityHelper();

            instance = new ManageableConfigurationSourceImplementation(testConfigurationFile,
                                                                       manageabilityHelper,
                                                                       groupPolicyWatcher,
                                                                       watcherCoordinator,
                                                                       notificationCoordinator);

            Assert.IsTrue(manageabilityHelper.updateCalled);
            Assert.AreEqual(1, watcherCoordinator.WatchedConfigSources.Count);
            Assert.IsTrue(watcherCoordinator.WatchedConfigSources.Contains(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));

            TestsConfigurationSection testConfigurationSection
                = (TestsConfigurationSection)instance.GetSection(externalSection1);

            Assert.IsNotNull(testConfigurationSection);
            Assert.AreEqual("value1", testConfigurationSection.Value);
            Assert.AreEqual(2, watcherCoordinator.WatchedConfigSources.Count);
            Assert.IsTrue(watcherCoordinator.WatchedConfigSources.Contains(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));
            Assert.IsTrue(watcherCoordinator.WatchedConfigSources.Contains("test.external.config"));
        }

        [TestMethod]
        [DeploymentItem("test.external.config")]
        public void RequestForNonExistingSectionDoesNotAddWatcher()
        {
            MockManageabilityHelper manageabilityHelper = new MockManageabilityHelper();

            instance = new ManageableConfigurationSourceImplementation(testConfigurationFile,
                                                                       manageabilityHelper,
                                                                       groupPolicyWatcher,
                                                                       watcherCoordinator,
                                                                       notificationCoordinator);

            Assert.IsTrue(manageabilityHelper.updateCalled);
            Assert.AreEqual(1, watcherCoordinator.WatchedConfigSources.Count);
            Assert.IsTrue(watcherCoordinator.WatchedConfigSources.Contains(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));

            Assert.IsTrue(manageabilityHelper.updateCalled);
            Assert.AreEqual(1, watcherCoordinator.WatchedConfigSources.Count);
            Assert.IsTrue(watcherCoordinator.WatchedConfigSources.Contains(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));
        }

        [TestMethod]
        [DeploymentItem("test.external.config")]
        public void ChangeInConfigurationFileFiresManageabilityUpdate()
        {
            MockManageabilityHelper manageabilityHelper = new MockManageabilityHelper(localSection1, externalSection1);

            instance = new ManageableConfigurationSourceImplementation(testConfigurationFile,
                                                                       manageabilityHelper,
                                                                       groupPolicyWatcher,
                                                                       watcherCoordinator,
                                                                       notificationCoordinator);
            manageabilityHelper.updateCalled = false;

            watcherCoordinator.OnConfigurationChanged(null,
                                                      new ConfigurationChangedEventArgs(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));

            Assert.IsTrue(manageabilityHelper.updateCalled);
        }

        [TestMethod]
        [DeploymentItem("test.external.config")]
        public void CanGetUpdatedConfigurationValuesAfterFileChangeNotification()
        {
            MockManageabilityHelper manageabilityHelper = new MockManageabilityHelper(localSection1, externalSection1);

            instance = new ManageableConfigurationSourceImplementation(testConfigurationFile,
                                                                       manageabilityHelper,
                                                                       groupPolicyWatcher,
                                                                       watcherCoordinator,
                                                                       notificationCoordinator);
            manageabilityHelper.updateCalled = false;

            TestsConfigurationSection testConfigurationSection
                = (TestsConfigurationSection)instance.GetSection(localSection1);
            Assert.IsNotNull(testConfigurationSection);
            Assert.AreEqual("value1", testConfigurationSection.Value);

            TestsConfigurationSection rwTestConfigurationSection
                = (TestsConfigurationSection)rwConfiguration.GetSection(localSection1);
            rwTestConfigurationSection.Value = "value2";
            rwConfiguration.Save();

            watcherCoordinator.OnConfigurationChanged(null,
                                                      new ConfigurationChangedEventArgs(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));

            testConfigurationSection
                = (TestsConfigurationSection)instance.GetSection(localSection1);
            Assert.IsNotNull(testConfigurationSection);
            Assert.AreEqual("value2", testConfigurationSection.Value);
            Assert.AreNotSame(rwTestConfigurationSection, testConfigurationSection);
        }

        [TestMethod]
        [DeploymentItem("test.external.config")]
        public void RegisteredForChangesOnLocalReceivesNotificationsOnMainConfigurationFileUpdate()
        {
            MockManageabilityHelper manageabilityHelper = new MockManageabilityHelper();

            instance = new ManageableConfigurationSourceImplementation(testConfigurationFile,
                                                                       manageabilityHelper,
                                                                       groupPolicyWatcher,
                                                                       watcherCoordinator,
                                                                       notificationCoordinator);

            instance.AddSectionChangeHandler(localSection1, OnConfigurationSectionChange);

            instance.GetSection(localSection1);
            watcherCoordinator.OnConfigurationChanged(null,
                                                      new ConfigurationChangedEventArgs(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));

            Assert.AreEqual(1, valuesForNotifiedSections.Count);
            Assert.AreEqual("value1", valuesForNotifiedSections[localSection1]);
        }

        [TestMethod]
        [DeploymentItem("test.external.config")]
        public void RegisteredForChangesOnLocalReceivesNotificationsOnMainConfigurationFileUpdateWithUpdatedValuesAvailable()
        {
            MockManageabilityHelper manageabilityHelper = new MockManageabilityHelper();

            instance = new ManageableConfigurationSourceImplementation(testConfigurationFile,
                                                                       manageabilityHelper,
                                                                       groupPolicyWatcher,
                                                                       watcherCoordinator,
                                                                       notificationCoordinator);

            instance.GetSection(localSection1);
            instance.AddSectionChangeHandler(localSection1, OnConfigurationSectionChange);

            TestsConfigurationSection rwTestConfigurationSection
                = (TestsConfigurationSection)rwConfiguration.GetSection(localSection1);
            rwTestConfigurationSection.Value = "value2";
            rwConfiguration.Save();

            watcherCoordinator.OnConfigurationChanged(null,
                                                      new ConfigurationChangedEventArgs(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));

            Assert.AreEqual(1, valuesForNotifiedSections.Count);
            Assert.AreEqual("value2", valuesForNotifiedSections[localSection1]);
        }

        [TestMethod]
        [DeploymentItem("test.external.config")]
        public void ChangeOnMainConfigurationFileNotifiesHandlersForAllSections()
        {
            // if the main configuration file changes, sections with specified config sources might have changed
            // since the configuration object must be reload entirely, and config sections with specified config sources are
            // rare, notifying all the sections is a sensible trade off

            MockManageabilityHelper manageabilityHelper = new MockManageabilityHelper();

            instance = new ManageableConfigurationSourceImplementation(testConfigurationFile,
                                                                       manageabilityHelper,
                                                                       groupPolicyWatcher,
                                                                       watcherCoordinator,
                                                                       notificationCoordinator);

            instance.AddSectionChangeHandler(localSection1, OnConfigurationSectionChange);
            instance.AddSectionChangeHandler(externalSection1, OnConfigurationSectionChange);

            instance.GetSection(localSection1);
            instance.GetSection(externalSection1);
            watcherCoordinator.OnConfigurationChanged(null,
                                                      new ConfigurationChangedEventArgs(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));

            Assert.AreEqual(2, valuesForNotifiedSections.Count);
            Assert.AreEqual("value1", valuesForNotifiedSections[localSection1]);
            Assert.AreEqual("value1", valuesForNotifiedSections[externalSection1]);
        }

        [TestMethod]
        [DeploymentItem("test.external.config")]
        public void ChangeOnGroupPoliciesNotifiesHandlersForAllSections()
        {
            MockManageabilityHelper manageabilityHelper = new MockManageabilityHelper();

            instance = new ManageableConfigurationSourceImplementation(testConfigurationFile,
                                                                       manageabilityHelper,
                                                                       groupPolicyWatcher,
                                                                       watcherCoordinator,
                                                                       notificationCoordinator);

            instance.AddSectionChangeHandler(localSection1, OnConfigurationSectionChange);
            instance.AddSectionChangeHandler(externalSection1, OnConfigurationSectionChange);

            instance.GetSection(localSection1);
            instance.GetSection(externalSection1);

            Thread.Sleep(100); // let the watching thread start
            policyRegistrationBuilder.LastRegistration.MachinePolicyEvent.Set(); // fire the notification
            Thread.Sleep(250);

            Assert.AreEqual(2, valuesForNotifiedSections.Count);
            Assert.AreEqual("value1", valuesForNotifiedSections[localSection1]);
            Assert.AreEqual("value1", valuesForNotifiedSections[externalSection1]);
        }

        [TestMethod]
        [DeploymentItem("test.external.config")]
        public void RemovedHandlerReceivesNoFurtherNotifications()
        {
            MockManageabilityHelper manageabilityHelper = new MockManageabilityHelper();

            instance = new ManageableConfigurationSourceImplementation(testConfigurationFile,
                                                                       manageabilityHelper,
                                                                       groupPolicyWatcher,
                                                                       watcherCoordinator,
                                                                       notificationCoordinator);

            instance.AddSectionChangeHandler(localSection1, OnConfigurationSectionChange);
            instance.AddSectionChangeHandler(externalSection1, OnConfigurationSectionChange);

            instance.GetSection(localSection1);
            instance.GetSection(externalSection1);
            watcherCoordinator.OnConfigurationChanged(null,
                                                      new ConfigurationChangedEventArgs(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));

            Assert.AreEqual(2, valuesForNotifiedSections.Count);
            Assert.AreEqual("value1", valuesForNotifiedSections[localSection1]);
            Assert.AreEqual("value1", valuesForNotifiedSections[externalSection1]);

            instance.RemoveSectionChangeHandler(localSection1, OnConfigurationSectionChange);
            valuesForNotifiedSections.Clear();

            watcherCoordinator.OnConfigurationChanged(null,
                                                      new ConfigurationChangedEventArgs(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));

            Assert.AreEqual(1, valuesForNotifiedSections.Count);
            Assert.AreEqual("value1", valuesForNotifiedSections[externalSection1]);
        }

        [TestMethod]
        [DeploymentItem("test.external.config")]
        public void ChangeOnExternalConfigurationFileNotifiesHandlersForTheExternalSectionOnlyIfNoOtherSectionsWereRemoved()
        {
            // if the file for a section's config source changes only that section can be affected.
            // even though the configuration object is reloaded entirely, changes for other sections are not notified
            // if there were any changes in other sections, then other configuration files would have changed and
            // notifications for these changes would be issued before of after the section's configuration source file
            // i.e. the appropriate notifications will be issued, eventually
            // the only exception is for removed or restored sections, tested elsewhere

            MockManageabilityHelper manageabilityHelper = new MockManageabilityHelper();

            instance = new ManageableConfigurationSourceImplementation(testConfigurationFile,
                                                                       manageabilityHelper,
                                                                       groupPolicyWatcher,
                                                                       watcherCoordinator,
                                                                       notificationCoordinator);

            instance.AddSectionChangeHandler(localSection1, OnConfigurationSectionChange);
            instance.AddSectionChangeHandler(externalSection1, OnConfigurationSectionChange);

            instance.GetSection(localSection1);
            instance.GetSection(externalSection1);
            watcherCoordinator.OnConfigurationChanged(null,
                                                      new ConfigurationChangedEventArgs("test.external.config"));

            Assert.AreEqual(1, valuesForNotifiedSections.Count);
            Assert.AreEqual("value1", valuesForNotifiedSections[externalSection1]);
        }

        [TestMethod]
        [DeploymentItem("test.external.config")]
        public void ChangeOnExternalConfigurationFileNotifiesHandlersForOtherRemovedSections()
        {
            // if a removed section is detected it should be notified when the external file changes
            // as processing the main change file later will break the logic that deals with deletions

            MockManageabilityHelper manageabilityHelper = new MockManageabilityHelper();

            instance = new ManageableConfigurationSourceImplementation(testConfigurationFile,
                                                                       manageabilityHelper,
                                                                       groupPolicyWatcher,
                                                                       watcherCoordinator,
                                                                       notificationCoordinator);

            instance.AddSectionChangeHandler(localSection1, OnConfigurationSectionChange);
            instance.AddSectionChangeHandler(externalSection1, OnConfigurationSectionChange);

            instance.GetSection(localSection1);
            instance.GetSection(externalSection1);

            rwConfiguration.Sections.Remove(localSection1);
            rwConfiguration.Save();

            watcherCoordinator.OnConfigurationChanged(null,
                                                      new ConfigurationChangedEventArgs("test.external.config"));

            Assert.AreEqual(2, valuesForNotifiedSections.Count);
            Assert.AreEqual(null, valuesForNotifiedSections[localSection1]);
            Assert.AreEqual("value1", valuesForNotifiedSections[externalSection1]);
        }

        [TestMethod]
        [DeploymentItem("test.external.config")]
        public void ChangeOnExternalConfigurationFileNotifiesHandlersForOtherRestoredSections()
        {
            // if a removed section is detected it should be notified when the external file changes
            // as processing the main change file later will break the logic that deals with deletions

            MockManageabilityHelper manageabilityHelper = new MockManageabilityHelper();

            instance = new ManageableConfigurationSourceImplementation(testConfigurationFile,
                                                                       manageabilityHelper,
                                                                       groupPolicyWatcher,
                                                                       watcherCoordinator,
                                                                       notificationCoordinator);

            instance.AddSectionChangeHandler(localSection1, OnConfigurationSectionChange);
            instance.AddSectionChangeHandler(externalSection1, OnConfigurationSectionChange);

            instance.GetSection(localSection1);
            instance.GetSection(externalSection1);

            rwConfiguration.Sections.Remove(localSection1);
            rwConfiguration.Save();

            watcherCoordinator.OnConfigurationChanged(null,
                                                      new ConfigurationChangedEventArgs("test.external.config"));

            rwConfiguration.Sections.Add(localSection1, new TestsConfigurationSection("valuerestored"));
            rwConfiguration.Save();

            valuesForNotifiedSections.Clear();
            watcherCoordinator.OnConfigurationChanged(null,
                                                      new ConfigurationChangedEventArgs("test.external.config"));

            Assert.AreEqual(2, valuesForNotifiedSections.Count);
            Assert.AreEqual("valuerestored", valuesForNotifiedSections[localSection1]);
            Assert.AreEqual("value1", valuesForNotifiedSections[externalSection1]);
        }

        [TestMethod]
        [DeploymentItem("test.external.config")]
        public void ChangeOnMainConfigurationFileDoesNotNotifyHandlerForExistingSectionNeverRequested()
        {
            // if the section has never been requested, then there's no point in notifying
            // for its (possible) changes.

            MockManageabilityHelper manageabilityHelper = new MockManageabilityHelper(); // make sure it's not requested

            instance = new ManageableConfigurationSourceImplementation(testConfigurationFile,
                                                                       manageabilityHelper,
                                                                       groupPolicyWatcher,
                                                                       watcherCoordinator,
                                                                       notificationCoordinator);

            instance.AddSectionChangeHandler(localSection1, OnConfigurationSectionChange);

            watcherCoordinator.OnConfigurationChanged(null,
                                                      new ConfigurationChangedEventArgs(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));

            Assert.AreEqual(0, valuesForNotifiedSections.Count);
        }

        [TestMethod]
        [DeploymentItem("test.external.config")]
        public void ChangeOnMainConfigurationFileDoesNotNotifyHandlerForNonExistingSection()
        {
            MockManageabilityHelper manageabilityHelper = new MockManageabilityHelper();

            instance = new ManageableConfigurationSourceImplementation(testConfigurationFile,
                                                                       manageabilityHelper,
                                                                       groupPolicyWatcher,
                                                                       watcherCoordinator,
                                                                       notificationCoordinator);

            instance.AddSectionChangeHandler("non existing", OnConfigurationSectionChange);
            ConfigurationSection section = instance.GetSection("non existing");
            Assert.IsNull(section);

            watcherCoordinator.OnConfigurationChanged(null,
                                                      new ConfigurationChangedEventArgs(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));

            Assert.AreEqual(0, valuesForNotifiedSections.Count);
        }

        [TestMethod]
        [DeploymentItem("test.external.config")]
        public void ChangeOnMainConfigurationFileNotifiesHandlerForRemovedSectionOnlyTheFirstTime()
        {
            // if the section existed once, then registered handlers must be notified when the removal
            // is detected, but after that they shouldn't be notified while the section is still deleted

            MockManageabilityHelper manageabilityHelper = new MockManageabilityHelper();

            instance = new ManageableConfigurationSourceImplementation(testConfigurationFile,
                                                                       manageabilityHelper,
                                                                       groupPolicyWatcher,
                                                                       watcherCoordinator,
                                                                       notificationCoordinator);

            instance.AddSectionChangeHandler(localSection1, OnConfigurationSectionChange);
            TestsConfigurationSection section = (TestsConfigurationSection)instance.GetSection(localSection1);
            Assert.IsNotNull(section);

            watcherCoordinator.OnConfigurationChanged(null,
                                                      new ConfigurationChangedEventArgs(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));

            Assert.AreEqual(1, valuesForNotifiedSections.Count);
            Assert.AreEqual("value1", valuesForNotifiedSections[localSection1]);

            rwConfiguration.Sections.Remove(localSection1);
            rwConfiguration.Save();

            watcherCoordinator.OnConfigurationChanged(null,
                                                      new ConfigurationChangedEventArgs(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));

            Assert.AreEqual(1, valuesForNotifiedSections.Count);
            Assert.AreEqual(null, valuesForNotifiedSections[localSection1]);

            valuesForNotifiedSections.Clear();

            watcherCoordinator.OnConfigurationChanged(null,
                                                      new ConfigurationChangedEventArgs(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));

            Assert.AreEqual(0, valuesForNotifiedSections.Count); // second time, no notification
        }

        [TestMethod]
        [DeploymentItem("test.external.config")]
        public void ChangeOnMainConfigurationFileNotifiesHandlerForRestoredSection()
        {
            // if the section existed once, then registered handlers must be notified when the removal
            // is detected, but after that they shouldn't be notified while the section is still deleted
            // and they should be notified if the configuration section shows

            MockManageabilityHelper manageabilityHelper = new MockManageabilityHelper();

            instance = new ManageableConfigurationSourceImplementation(testConfigurationFile,
                                                                       manageabilityHelper,
                                                                       groupPolicyWatcher,
                                                                       watcherCoordinator,
                                                                       notificationCoordinator);

            instance.AddSectionChangeHandler(localSection1, OnConfigurationSectionChange);
            TestsConfigurationSection section = (TestsConfigurationSection)instance.GetSection(localSection1);
            Assert.IsNotNull(section);

            watcherCoordinator.OnConfigurationChanged(null,
                                                      new ConfigurationChangedEventArgs(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));

            Assert.AreEqual(1, valuesForNotifiedSections.Count);
            Assert.AreEqual("value1", valuesForNotifiedSections[localSection1]);

            rwConfiguration.Sections.Remove(localSection1);
            rwConfiguration.Save();

            watcherCoordinator.OnConfigurationChanged(null,
                                                      new ConfigurationChangedEventArgs(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));

            Assert.AreEqual(1, valuesForNotifiedSections.Count);
            Assert.AreEqual(null, valuesForNotifiedSections[localSection1]);

            valuesForNotifiedSections.Clear();

            watcherCoordinator.OnConfigurationChanged(null,
                                                      new ConfigurationChangedEventArgs(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));

            Assert.AreEqual(0, valuesForNotifiedSections.Count); // second time, no notification

            // restore the section and notify again
            rwConfiguration.Sections.Add(localSection1, new TestsConfigurationSection("valuenew"));
            rwConfiguration.Save();

            watcherCoordinator.OnConfigurationChanged(null,
                                                      new ConfigurationChangedEventArgs(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));

            Assert.AreEqual(1, valuesForNotifiedSections.Count);
            Assert.AreEqual("valuenew", valuesForNotifiedSections[localSection1]);
        }

        [TestMethod]
        [DeploymentItem("test.external.config")]
        public void ChangeOnMainConfigurationFileForRemovedInternalSectionDoesNotAffectWatchers()
        {
            // the main file watcher is never removed, so removed internal sections don't affect the watchers

            MockManageabilityHelper manageabilityHelper = new MockManageabilityHelper();

            instance = new ManageableConfigurationSourceImplementation(testConfigurationFile,
                                                                       manageabilityHelper,
                                                                       groupPolicyWatcher,
                                                                       watcherCoordinator,
                                                                       notificationCoordinator);

            instance.GetSection(localSection1);
            instance.GetSection(externalSection1);

            Assert.IsTrue(watcherCoordinator.WatchedConfigSources.Contains(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));
            Assert.IsTrue(watcherCoordinator.WatchedConfigSources.Contains("test.external.config"));

            rwConfiguration.Sections.Remove(localSection1);
            rwConfiguration.Save();

            watcherCoordinator.OnConfigurationChanged(null,
                                                      new ConfigurationChangedEventArgs(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));

            Assert.AreEqual(2, watcherCoordinator.WatchedConfigSources.Count);
            Assert.IsTrue(watcherCoordinator.WatchedConfigSources.Contains(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));
            Assert.IsTrue(watcherCoordinator.WatchedConfigSources.Contains("test.external.config"));
        }

        [TestMethod]
        [DeploymentItem("test.external.config")]
        public void ChangeOnMainConfigurationFileForRemovedExternalSectionRemovesWatcherOnExternalFile()
        {
            MockManageabilityHelper manageabilityHelper = new MockManageabilityHelper();

            instance = new ManageableConfigurationSourceImplementation(testConfigurationFile,
                                                                       manageabilityHelper,
                                                                       groupPolicyWatcher,
                                                                       watcherCoordinator,
                                                                       notificationCoordinator);

            instance.GetSection(localSection1);
            instance.GetSection(externalSection1);

            Assert.IsTrue(watcherCoordinator.WatchedConfigSources.Contains(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));
            Assert.IsTrue(watcherCoordinator.WatchedConfigSources.Contains("test.external.config"));

            rwConfiguration.Sections.Remove(externalSection1);
            rwConfiguration.Save();

            watcherCoordinator.OnConfigurationChanged(null,
                                                      new ConfigurationChangedEventArgs(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));

            Assert.AreEqual(1, watcherCoordinator.WatchedConfigSources.Count);
            Assert.IsTrue(watcherCoordinator.WatchedConfigSources.Contains(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));
        }

        void OnConfigurationSectionChange(object sender,
                                          ConfigurationChangedEventArgs e)
        {
            TestsConfigurationSection section
                = (TestsConfigurationSection)instance.GetSection(e.SectionName);

            valuesForNotifiedSections[e.SectionName] = section != null ? section.Value : null;
        }
    }
}
