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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests
{
    [TestClass()]
    public class ManageabilityHelperFixture
    {
        const string ApplicationName = "TestApp";
        const string SectionName = "TestSection";
        const string AltSectionName = "AltTestSection";
        const String originalValue = "original value";

        DictionaryConfigurationSource configurationSource;
        DictionaryConfigurationSourceConfigurationAccessor configurationAccessor;
        MockRegistryAccessor registryAccessor;
        MockRegistryKey currentUser;
        MockRegistryKey localMachine;
        MockWmiPublisher wmiPublisher;
        IDictionary<string, ConfigurationSectionManageabilityProvider> manageabilityProviders;
        IDictionary<Type, ConfigurationElementManageabilityProvider> subProviders;

        [TestInitialize()]
        public void SetUp()
        {
            configurationSource = new DictionaryConfigurationSource();
            configurationAccessor = new DictionaryConfigurationSourceConfigurationAccessor(configurationSource);
            currentUser = new MockRegistryKey(true);
            localMachine = new MockRegistryKey(true);
            registryAccessor = new MockRegistryAccessor(currentUser, localMachine);
            wmiPublisher = new MockWmiPublisher();

            manageabilityProviders = new Dictionary<string, ConfigurationSectionManageabilityProvider>();
            subProviders = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
        }

        [TestMethod()]
        public void HelperWithEmtpyManageabilityProvidersDoesNothing()
        {
            TestsConfigurationSection section = new TestsConfigurationSection(originalValue);
            configurationSource.Add(SectionName, section);

            MockRegistryKey userKey = new MockRegistryKey(false);
            currentUser.AddSubKey(ManageabilityHelper.BuildSectionKeyName(ApplicationName, SectionName), userKey);
            MockRegistryKey machineKey = new MockRegistryKey(false);
            localMachine.AddSubKey(ManageabilityHelper.BuildSectionKeyName(ApplicationName, SectionName), machineKey);

            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, true, registryAccessor, true, wmiPublisher, ApplicationName);
            helper.UpdateConfigurationManageability(configurationAccessor);

            Assert.AreEqual(0, wmiPublisher.GetPublishedInstances().Count);
            Assert.AreEqual(0, currentUser.GetRequests().Count);
            Assert.AreEqual(0, localMachine.GetRequests().Count);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(userKey, machineKey));
        }

        [TestMethod()]
        public void HelperWithManageabilityProviderForMissingSectionDoesNothing()
        {
            TestsConfigurationSection section = new TestsConfigurationSection(originalValue);
            configurationSource.Add(SectionName, section);

            MockRegistryKey userKey = new MockRegistryKey(false);
            currentUser.AddSubKey(ManageabilityHelper.BuildSectionKeyName(ApplicationName, SectionName), userKey);
            MockRegistryKey machineKey = new MockRegistryKey(false);
            localMachine.AddSubKey(ManageabilityHelper.BuildSectionKeyName(ApplicationName, SectionName), machineKey);

            MockConfigurationSectionManageabilityProvider manageabilityProvider = new MockConfigurationSectionManageabilityProvider(subProviders);
            manageabilityProviders.Add(AltSectionName, manageabilityProvider);

            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, true, registryAccessor, true, wmiPublisher, ApplicationName);
            helper.UpdateConfigurationManageability(configurationAccessor);

            Assert.IsFalse(manageabilityProvider.called);
            Assert.AreEqual(0, wmiPublisher.GetPublishedInstances().Count);
            Assert.AreEqual(0, currentUser.GetRequests().Count);
            Assert.AreEqual(0, localMachine.GetRequests().Count);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(userKey, machineKey));
        }

        [TestMethod()]
        public void HelperWithManageabilityProviderForExistingSectionDoesInvokeWithApproriateParameters()
        {
            TestsConfigurationSection section = new TestsConfigurationSection(originalValue);
            configurationSource.Add(SectionName, section);

            MockRegistryKey userKey = new MockRegistryKey(false);
            currentUser.AddSubKey(ManageabilityHelper.BuildSectionKeyName(ApplicationName, SectionName), userKey);
            MockRegistryKey machineKey = new MockRegistryKey(false);
            localMachine.AddSubKey(ManageabilityHelper.BuildSectionKeyName(ApplicationName, SectionName), machineKey);

            MockConfigurationSectionManageabilityProvider manageabilityProvider = new MockConfigurationSectionManageabilityProvider(subProviders);
            manageabilityProviders.Add(SectionName, manageabilityProvider);

            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, true, registryAccessor, true, wmiPublisher, ApplicationName);
            helper.UpdateConfigurationManageability(configurationAccessor);

            Assert.IsTrue(manageabilityProvider.called);
            Assert.AreEqual(1, wmiPublisher.GetPublishedInstances().Count);
            Assert.AreEqual(1, currentUser.GetRequests().Count);
            Assert.AreSame(userKey, manageabilityProvider.userKey);
            Assert.AreEqual(1, localMachine.GetRequests().Count);
            Assert.AreSame(machineKey, manageabilityProvider.machineKey);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(userKey, machineKey));
        }

        [TestMethod()]
        public void HelperWillNotSendRegistryKeysIfNotReadingGroupPolicies()
        {
            TestsConfigurationSection section = new TestsConfigurationSection(originalValue);
            configurationSource.Add(SectionName, section);

            MockRegistryKey userKey = new MockRegistryKey(false);
            currentUser.AddSubKey(ManageabilityHelper.BuildSectionKeyName(ApplicationName, SectionName), userKey);
            MockRegistryKey machineKey = new MockRegistryKey(false);
            localMachine.AddSubKey(ManageabilityHelper.BuildSectionKeyName(ApplicationName, SectionName), machineKey);

            MockConfigurationSectionManageabilityProvider manageabilityProvider = new MockConfigurationSectionManageabilityProvider(subProviders);
            manageabilityProviders.Add(SectionName, manageabilityProvider);

            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, false, registryAccessor, true, wmiPublisher, ApplicationName);
            helper.UpdateConfigurationManageability(configurationAccessor);

            Assert.IsTrue(manageabilityProvider.called);
            Assert.IsFalse(manageabilityProvider.readGroupPolicies);
            Assert.AreEqual(0, currentUser.GetRequests().Count);
            Assert.IsNull(manageabilityProvider.userKey);
            Assert.AreEqual(0, localMachine.GetRequests().Count);
            Assert.IsNull(manageabilityProvider.machineKey);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(userKey, machineKey));
        }

        [TestMethod()]
        public void HelperWillNotSendRegistryKeysIfNotAvailable()
        {
            TestsConfigurationSection section = new TestsConfigurationSection(originalValue);
            configurationSource.Add(SectionName, section);

            MockRegistryKey machineKey = new MockRegistryKey(false);
            localMachine.AddSubKey(ManageabilityHelper.BuildSectionKeyName(ApplicationName, SectionName), machineKey);

            MockConfigurationSectionManageabilityProvider manageabilityProvider = new MockConfigurationSectionManageabilityProvider(subProviders);
            manageabilityProviders.Add(SectionName, manageabilityProvider);

            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, true, registryAccessor, true, wmiPublisher, ApplicationName);
            helper.UpdateConfigurationManageability(configurationAccessor);

            Assert.IsTrue(manageabilityProvider.called);
            Assert.IsTrue(manageabilityProvider.readGroupPolicies);
            Assert.AreEqual(1, currentUser.GetRequests().Count);
            Assert.IsNull(manageabilityProvider.userKey);
            Assert.AreEqual(1, localMachine.GetRequests().Count);
            Assert.AreSame(machineKey, manageabilityProvider.machineKey);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineKey));
        }

        [TestMethod]
        public void HelperWillSendAppropriateParameterIfWmiIsDisabled()
        {
            TestsConfigurationSection section = new TestsConfigurationSection(originalValue);
            configurationSource.Add(SectionName, section);

            MockConfigurationSectionManageabilityProvider manageabilityProvider = new MockConfigurationSectionManageabilityProvider(subProviders);
            manageabilityProviders.Add(SectionName, manageabilityProvider);

            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, true, registryAccessor, false, wmiPublisher, ApplicationName);
            helper.UpdateConfigurationManageability(configurationAccessor);

            Assert.IsTrue(manageabilityProvider.called);
            Assert.IsTrue(manageabilityProvider.readGroupPolicies);
            Assert.IsFalse(manageabilityProvider.generateWmiObjects);
            Assert.AreEqual(0, wmiPublisher.GetPublishedInstances().Count);
        }

        [TestMethod]
        public void HelperWillPublishIfWmiIsEnabled()
        {
            TestsConfigurationSection section = new TestsConfigurationSection(originalValue);
            configurationSource.Add(SectionName, section);

            MockConfigurationSectionManageabilityProvider manageabilityProvider
                = new MockConfigurationSectionManageabilityProvider(subProviders);
            manageabilityProviders.Add(SectionName, manageabilityProvider);

            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, true, registryAccessor, true, wmiPublisher, ApplicationName);
            helper.UpdateConfigurationManageability(configurationAccessor);

            Assert.IsTrue(manageabilityProvider.called);
            Assert.IsTrue(manageabilityProvider.readGroupPolicies);
            Assert.IsTrue(manageabilityProvider.generateWmiObjects);
            Assert.AreEqual(1, wmiPublisher.GetPublishedInstances().Count);
        }

        [TestMethod]
        public void HelperWillRevokePublishedSettingsOnReprocess()
        {
            TestsConfigurationSection section = new TestsConfigurationSection(originalValue);
            configurationSource.Add(SectionName, section);

            MockConfigurationSectionManageabilityProvider manageabilityProvider
                = new MockConfigurationSectionManageabilityProvider(subProviders);
            manageabilityProviders.Add(SectionName, manageabilityProvider);

            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, true, registryAccessor, true, wmiPublisher, ApplicationName);
            helper.UpdateConfigurationManageability(configurationAccessor);

            Assert.AreEqual(1, wmiPublisher.GetPublishedInstances().Count);
            ConfigurationSetting publishedSetting = new List<ConfigurationSetting>(wmiPublisher.GetPublishedInstances())[0];

            helper.UpdateConfigurationManageability(configurationAccessor);

            Assert.AreEqual(1, wmiPublisher.GetPublishedInstances().Count);
            ConfigurationSetting rePublishedSetting = new List<ConfigurationSetting>(wmiPublisher.GetPublishedInstances())[0];
            Assert.AreNotSame(publishedSetting, rePublishedSetting);
        }

        [TestMethod]
        public void HelperIgnoresUpdateEmptySectionsList()
        {
            MockConfigurationSectionManageabilityProvider manageabilityProvider
                = new MockConfigurationSectionManageabilityProvider(subProviders);
            manageabilityProviders.Add(SectionName, manageabilityProvider);

            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, true, registryAccessor, true, wmiPublisher, ApplicationName);
            helper.UpdateConfigurationManageability(configurationAccessor);

            Assert.AreEqual(0, wmiPublisher.GetPublishedInstances().Count);
            Assert.AreEqual(0, currentUser.GetRequests().Count);
            Assert.AreEqual(0, localMachine.GetRequests().Count);
        }

        [TestMethod]
        public void HelperIgnoresUpdateSectionsListWithoutMappedProvider()
        {
            MockConfigurationSectionManageabilityProvider manageabilityProvider
                = new MockConfigurationSectionManageabilityProvider(subProviders);
            manageabilityProviders.Add(SectionName, manageabilityProvider);

            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, true, registryAccessor, true, wmiPublisher, ApplicationName);
            helper.UpdateConfigurationManageability(configurationAccessor);

            Assert.AreEqual(0, wmiPublisher.GetPublishedInstances().Count);
            Assert.AreEqual(0, currentUser.GetRequests().Count);
            Assert.AreEqual(0, localMachine.GetRequests().Count);
        }

        [TestMethod]
        public void HelperIgnoresUpdateSectionsListWithMappedProviderForMissingSection()
        {
            MockConfigurationSectionManageabilityProvider manageabilityProvider
                = new MockConfigurationSectionManageabilityProvider(subProviders);
            manageabilityProviders.Add(SectionName, manageabilityProvider);

            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, true, registryAccessor, true, wmiPublisher, ApplicationName);
            helper.UpdateConfigurationManageability(configurationAccessor);

            Assert.IsFalse(manageabilityProvider.called);
        }

        [TestMethod]
        public void HelperPerformsUpdateForSectionNotPreviouslyProcessed()
        {
            TestsConfigurationSection section = new TestsConfigurationSection(originalValue);
            configurationSource.Add(SectionName, section);

            MockConfigurationSectionManageabilityProvider manageabilityProvider
                = new MockConfigurationSectionManageabilityProvider(subProviders);
            manageabilityProviders.Add(SectionName, manageabilityProvider);

            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, true, registryAccessor, true, wmiPublisher, ApplicationName);
            helper.UpdateConfigurationManageability(configurationAccessor);

            Assert.IsTrue(manageabilityProvider.called);
        }

        [TestMethod]
        public void HelperRevokesPublishedObjectsForRemovedSection()
        {
            TestsConfigurationSection section = new TestsConfigurationSection(originalValue);
            configurationSource.Add(SectionName, section);

            MockConfigurationSectionManageabilityProvider manageabilityProvider
                = new MockConfigurationSectionManageabilityProvider(subProviders);
            manageabilityProviders.Add(SectionName, manageabilityProvider);

            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, true, registryAccessor, true, wmiPublisher, ApplicationName);
            helper.UpdateConfigurationManageability(configurationAccessor);

            Assert.AreEqual(1, wmiPublisher.GetPublishedInstances().Count);

            configurationSource.Remove(SectionName);
            helper.UpdateConfigurationManageability(configurationAccessor);

            Assert.AreEqual(0, wmiPublisher.GetPublishedInstances().Count);
        }

        [TestMethod]
        public void WmiSettingsArePublishedWithApplicationAndSectionName()
        {
            TestsConfigurationSection section = new TestsConfigurationSection(originalValue);
            configurationSource.Add(SectionName, section);

            MockConfigurationSectionManageabilityProvider manageabilityProvider
                = new MockConfigurationSectionManageabilityProvider(subProviders);
            manageabilityProviders.Add(SectionName, manageabilityProvider);

            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, false, registryAccessor, true, wmiPublisher, ApplicationName);
            helper.UpdateConfigurationManageability(configurationAccessor);

            Assert.AreEqual(1, wmiPublisher.GetPublishedInstances().Count);
            ConfigurationSetting publishedSetting = new List<ConfigurationSetting>(wmiPublisher.GetPublishedInstances())[0];
            Assert.AreEqual(ApplicationName, publishedSetting.ApplicationName);
            Assert.AreEqual(SectionName, publishedSetting.SectionName);
        }

        [TestMethod]
        public void WillRemoveRegisteredSectionWithDisabledPolicyIfPolicyOverridesAreEnabled()
        {
            TestsConfigurationSection section = new TestsConfigurationSection(originalValue);
            configurationSource.Add(SectionName, section);

            MockRegistryKey machineKey = new MockRegistryKey(false);
            localMachine.AddSubKey(ManageabilityHelper.BuildSectionKeyName(ApplicationName, SectionName), machineKey);
            machineKey.AddBooleanValue(AdmContentBuilder.AvailableValueName, false);

            MockConfigurationSectionManageabilityProvider manageabilityProvider
                = new MockConfigurationSectionManageabilityProvider(subProviders);
            manageabilityProviders.Add(SectionName, manageabilityProvider);
            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, true, registryAccessor, true, wmiPublisher, ApplicationName);

            Assert.IsTrue(configurationSource.Contains(SectionName));

            helper.UpdateConfigurationManageability(configurationAccessor);

            Assert.IsFalse(configurationSource.Contains(SectionName));
        }

        [TestMethod]
        public void WillNotRemoveRegisteredSectionWithDisabledPolicyIfPolicyOverridesAreDisabled()
        {
            TestsConfigurationSection section = new TestsConfigurationSection(originalValue);
            configurationSource.Add(SectionName, section);

            MockRegistryKey machineKey = new MockRegistryKey(false);
            localMachine.AddSubKey(ManageabilityHelper.BuildSectionKeyName(ApplicationName, SectionName), machineKey);
            machineKey.AddBooleanValue(AdmContentBuilder.AvailableValueName, false);

            MockConfigurationSectionManageabilityProvider manageabilityProvider
                = new MockConfigurationSectionManageabilityProvider(subProviders);
            manageabilityProviders.Add(SectionName, manageabilityProvider);
            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, false, registryAccessor, true, wmiPublisher, ApplicationName);

            Assert.IsTrue(configurationSource.Contains(SectionName));

            helper.UpdateConfigurationManageability(configurationAccessor);

            Assert.IsTrue(configurationSource.Contains(SectionName));
        }

        [TestMethod]
        public void WillForwardChangeEventInConfigurationSetting()
        {
            TestsConfigurationSection section = new TestsConfigurationSection(originalValue);
            configurationSource.Add(SectionName, section);

            MockConfigurationSectionManageabilityProvider manageabilityProvider
                = new MockConfigurationSectionManageabilityProvider(subProviders);
            manageabilityProviders.Add(SectionName, manageabilityProvider);

            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, true, registryAccessor, true, wmiPublisher, ApplicationName);
            helper.ConfigurationSettingChanged += OnConfigurationSettingChanged;

            helper.UpdateConfigurationManageability(configurationAccessor);

            IEnumerator<ConfigurationSetting> publishedSettings = wmiPublisher.GetPublishedInstances().GetEnumerator();
            Assert.IsTrue(publishedSettings.MoveNext());

            Assert.IsNull(notifiedChangeSource);
            ((ConfigurationSetting)publishedSettings.Current).Commit(); // commit changes
            Assert.AreEqual(SectionName, notifiedChangeSource);
        }

        [TestMethod]
        public void WillRemoveHandlerForChangeEventInConfigurationSettingWhenRevoked()
        {
            TestsConfigurationSection section = new TestsConfigurationSection(originalValue);
            configurationSource.Add(SectionName, section);

            MockConfigurationSectionManageabilityProvider manageabilityProvider
                = new MockConfigurationSectionManageabilityProvider(subProviders);
            manageabilityProviders.Add(SectionName, manageabilityProvider);

            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, true, registryAccessor, true, wmiPublisher, ApplicationName);
            helper.ConfigurationSettingChanged += OnConfigurationSettingChanged;

            helper.UpdateConfigurationManageability(configurationAccessor);

            IEnumerator<ConfigurationSetting> publishedSettings = wmiPublisher.GetPublishedInstances().GetEnumerator();
            Assert.IsTrue(publishedSettings.MoveNext());

            // same as before, check that the setting's commit will trigger the event
            Assert.IsNull(notifiedChangeSource);
            ((ConfigurationSetting)publishedSettings.Current).Commit(); // commit changes
            Assert.AreEqual(SectionName, notifiedChangeSource);

            // remove the section and update the manageability
            configurationSource.Remove(SectionName);
            helper.UpdateConfigurationManageability(configurationAccessor);

            // check that *the original* setting's commit will not trigger changes now
            notifiedChangeSource = null;
            ((ConfigurationSetting)publishedSettings.Current).Commit(); // commit changes
            Assert.IsNull(notifiedChangeSource);
        }

        [TestMethod]
        public void UpdateOnSectionRepublishesWmiObjects()
        {
            TestsConfigurationSection section = new TestsConfigurationSection(originalValue);
            configurationSource.Add(SectionName, section);

            MockRegistryKey userKey = new MockRegistryKey(false);
            MockRegistryKey machineKey = new MockRegistryKey(false);

            MockConfigurationSectionManageabilityProvider manageabilityProvider = new MockConfigurationSectionManageabilityProvider(subProviders);
            manageabilityProviders.Add(SectionName, manageabilityProvider);

            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, true, registryAccessor, true, wmiPublisher, ApplicationName);
            helper.ConfigurationSettingChanged += OnConfigurationSettingChangedTriggerUpdate;
            helper.UpdateConfigurationManageability(configurationAccessor);

            // check the original setting was published alright.
            IEnumerator<ConfigurationSetting> publishedInstances = wmiPublisher.GetPublishedInstances().GetEnumerator();
            Assert.IsTrue(publishedInstances.MoveNext());
            TestConfigurationSettings originalSetting = publishedInstances.Current as TestConfigurationSettings;
            Assert.IsNotNull(originalSetting);
            Assert.AreEqual(originalValue, originalSetting.Value);
            Assert.IsFalse(publishedInstances.MoveNext());

            // change and notify
            originalSetting.Value = "Foo";
            originalSetting.Commit();

            // check the updated setting is indeed updated and a new instance
            publishedInstances = wmiPublisher.GetPublishedInstances().GetEnumerator();
            Assert.IsTrue(publishedInstances.MoveNext());
            TestConfigurationSettings updatedSetting = publishedInstances.Current as TestConfigurationSettings;
            Assert.IsNotNull(updatedSetting);
            Assert.AreNotSame(originalSetting, updatedSetting);
            Assert.AreEqual(originalSetting.Value, updatedSetting.Value);
            Assert.IsFalse(publishedInstances.MoveNext());
        }

        [TestMethod]
        public void UpdateOnSectionReappliesPoliciesAndRepublishesWmiObjects()
        {
            TestsConfigurationSection section = new TestsConfigurationSection(originalValue);
            configurationSource.Add(SectionName, section);

            MockRegistryKey userKey = new MockRegistryKey(false);
            currentUser.AddSubKey(ManageabilityHelper.BuildSectionKeyName(ApplicationName, SectionName), userKey);
            userKey.AddBooleanValue(RegistryKeyBase.PolicyValueName, true);
            userKey.AddStringValue(MockConfigurationSectionManageabilityProvider.ValuePropertyName, "Overriden");
            MockRegistryKey machineKey = new MockRegistryKey(false);
            localMachine.AddSubKey(ManageabilityHelper.BuildSectionKeyName(ApplicationName, SectionName), machineKey);

            MockConfigurationSectionManageabilityProvider manageabilityProvider = new MockConfigurationSectionManageabilityProvider(subProviders);
            manageabilityProviders.Add(SectionName, manageabilityProvider);

            ManageabilityHelper helper
                = new ManageabilityHelper(manageabilityProviders, true, registryAccessor, true, wmiPublisher, ApplicationName);
            helper.ConfigurationSettingChanged += OnConfigurationSettingChangedTriggerUpdate;
            helper.UpdateConfigurationManageability(configurationAccessor);

            // check the original setting was published alright.
            IEnumerator<ConfigurationSetting> publishedInstances = wmiPublisher.GetPublishedInstances().GetEnumerator();
            Assert.IsTrue(publishedInstances.MoveNext());
            TestConfigurationSettings originalSetting = publishedInstances.Current as TestConfigurationSettings;
            Assert.IsNotNull(originalSetting);
            Assert.AreEqual("Overriden", originalSetting.Value);
            Assert.IsFalse(publishedInstances.MoveNext());

            // change and notify
            originalSetting.Value = "Foo";
            originalSetting.Commit();

            // check the updated setting is indeed updated and a new instance
            // the gp override should still take precedence
            publishedInstances = wmiPublisher.GetPublishedInstances().GetEnumerator();
            Assert.IsTrue(publishedInstances.MoveNext());
            TestConfigurationSettings updatedSetting = publishedInstances.Current as TestConfigurationSettings;
            Assert.IsNotNull(updatedSetting);
            Assert.AreNotSame(originalSetting, updatedSetting);
            Assert.AreEqual("Overriden", updatedSetting.Value);
            Assert.IsFalse(publishedInstances.MoveNext());
        }

        void OnConfigurationSettingChangedTriggerUpdate(object source,
                                                        ConfigurationSettingChangedEventArgs args)
        {
            ((ManageabilityHelper)source).UpdateConfigurationSectionManageability(configurationAccessor, args.SectionName);
        }

        string notifiedChangeSource;

        void OnConfigurationSettingChanged(object source,
                                           ConfigurationSettingChangedEventArgs args)
        {
            notifiedChangeSource = args.SectionName;
        }
    }
}
