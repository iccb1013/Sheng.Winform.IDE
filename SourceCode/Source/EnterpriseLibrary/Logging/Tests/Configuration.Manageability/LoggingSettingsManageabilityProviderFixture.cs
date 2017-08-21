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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Tests
{
    [TestClass]
    public class LoggingSettingsManageabilityProviderFixture
    {
        LoggingSettingsManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        IList<ConfigurationSetting> wmiSettings;
        LoggingSettings section;
        DictionaryConfigurationSource configurationSource;

        [TestInitialize]
        public void SetUp()
        {
            provider = new LoggingSettingsManageabilityProvider(new Dictionary<Type, ConfigurationElementManageabilityProvider>(0));
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            wmiSettings = new List<ConfigurationSetting>();
            section = new LoggingSettings();
            configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(LoggingSettings.SectionName, section);
        }

        [TestCleanup]
        public void TearDown()
        {
            // preventive unregister to work around WMI.NET 2.0 issues with appdomain unloading
            ManagementEntityTypesRegistrar.UnregisterAll();
        }

        [TestMethod]
        public void ManageabilityProviderIsProperlyRegistered()
        {
            ConfigurationSectionManageabilityProviderAttribute selectedAttribute = null;

            Assembly assembly = typeof(LoggingSettingsManageabilityProvider).Assembly;
            foreach (ConfigurationSectionManageabilityProviderAttribute providerAttribute
                in assembly.GetCustomAttributes(typeof(ConfigurationSectionManageabilityProviderAttribute), false))
            {
                if (providerAttribute.SectionName.Equals(LoggingSettings.SectionName))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(LoggingSettingsManageabilityProvider), selectedAttribute.ManageabilityProviderType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ProviderThrowsWithConfigurationObjectOfWrongType()
        {
            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(new TestsConfigurationSection(), true, machineKey, userKey, true, wmiSettings);
        }

        [TestMethod]
        public void SectionIsNotModifiedIfThereAreNoPolicyOverrides()
        {
            section.DefaultCategory = "defaultCategory";
            section.TracingEnabled = true;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual("defaultCategory", section.DefaultCategory);
            Assert.AreEqual(true, section.TracingEnabled);
        }

        [TestMethod]
        public void NoExceptionsAreThrownIfMachineKeyIsNull()
        {
            section.DefaultCategory = "defaultCategory";
            section.TracingEnabled = true;
            section.LogWarningWhenNoCategoriesMatch = true;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, null, userKey, true, wmiSettings);
        }

        [TestMethod]
        public void NoExceptionsAreThrownIfUserKeyIsNull()
        {
            section.DefaultCategory = "defaultCategory";
            section.TracingEnabled = true;
            section.LogWarningWhenNoCategoriesMatch = true;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, null, true, wmiSettings);
        }

        [TestMethod]
        public void SectionPropertiesAreOverridenFromMachineKey()
        {
            section.DefaultCategory = "defaultCategory";
            section.TracingEnabled = true;
            section.LogWarningWhenNoCategoriesMatch = false;
            section.RevertImpersonation = true;

            machineKey.AddBooleanValue(LoggingSettingsManageabilityProvider.PolicyValueName, true);
            machineKey.AddStringValue(LoggingSettingsManageabilityProvider.DefaultCategoryPropertyName, "machineOverridenCategory");
            machineKey.AddBooleanValue(LoggingSettingsManageabilityProvider.TracingEnabledPropertyName, false);
            machineKey.AddBooleanValue(LoggingSettingsManageabilityProvider.LogWarningOnNoMatchPropertyName, true);
            machineKey.AddBooleanValue(LoggingSettingsManageabilityProvider.RevertImpersonationPropertyName, false);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual("machineOverridenCategory", section.DefaultCategory);
            Assert.AreEqual(false, section.TracingEnabled);
            Assert.AreEqual(true, section.LogWarningWhenNoCategoriesMatch);
            Assert.AreEqual(false, section.RevertImpersonation);
        }

        [TestMethod]
        public void SectionPropertiesAreOverridenFromUserKey()
        {
            section.DefaultCategory = "defaultCategory";
            section.TracingEnabled = false;
            section.LogWarningWhenNoCategoriesMatch = true;
            section.RevertImpersonation = false;

            userKey.AddBooleanValue(LoggingSettingsManageabilityProvider.PolicyValueName, true);
            userKey.AddStringValue(LoggingSettingsManageabilityProvider.DefaultCategoryPropertyName, "userOverridenCategory");
            userKey.AddBooleanValue(LoggingSettingsManageabilityProvider.TracingEnabledPropertyName, true);
            userKey.AddBooleanValue(LoggingSettingsManageabilityProvider.LogWarningOnNoMatchPropertyName, false);
            userKey.AddBooleanValue(LoggingSettingsManageabilityProvider.RevertImpersonationPropertyName, true);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual("userOverridenCategory", section.DefaultCategory);
            Assert.AreEqual(true, section.TracingEnabled);
            Assert.AreEqual(false, section.LogWarningWhenNoCategoriesMatch);
            Assert.AreEqual(true, section.RevertImpersonation);
        }

        [TestMethod]
        public void TraceSourceDefaultLevelIsOverridenFromMachineKey()
        {
            TraceSourceData sourceData = new TraceSourceData("source1", SourceLevels.Critical);
            section.TraceSources.Add(sourceData);

            MockRegistryKey machineTraceSourcesKey = new MockRegistryKey(false);
            machineKey.AddSubKey(LoggingSettingsManageabilityProvider.CategorySourcesKeyName, machineTraceSourcesKey);
            MockRegistryKey machineSource1Key = new MockRegistryKey(false);
            machineTraceSourcesKey.AddSubKey("source1", machineSource1Key);
            machineSource1Key.AddStringValue(LoggingSettingsManageabilityProvider.SourceDefaultLevelPropertyName, SourceLevels.Error.ToString());

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(SourceLevels.Error, sourceData.DefaultLevel);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineTraceSourcesKey, machineSource1Key));
        }

        [TestMethod]
        public void TraceSourceDefaultLevelIsOverridenFromUserKey()
        {
            TraceSourceData sourceData = new TraceSourceData("source1", SourceLevels.Critical);
            section.TraceSources.Add(sourceData);

            MockRegistryKey userTraceSourcesKey = new MockRegistryKey(false);
            userKey.AddSubKey(LoggingSettingsManageabilityProvider.CategorySourcesKeyName, userTraceSourcesKey);
            MockRegistryKey userSource1Key = new MockRegistryKey(false);
            userTraceSourcesKey.AddSubKey("source1", userSource1Key);
            userSource1Key.AddStringValue(LoggingSettingsManageabilityProvider.SourceDefaultLevelPropertyName, SourceLevels.Error.ToString());

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(SourceLevels.Error, sourceData.DefaultLevel);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(userTraceSourcesKey, userSource1Key));
        }

        // the listeners key is set by the trace source policy, so the override of trace listeners must be performed
        // every time the source key exists, even if the listeners sub key is not present
        [TestMethod]
        public void TraceSourceTraceListenersAreEmptiedIfMachineKeyIsPresentForSourceButKeyForListenersIsNot()
        {
            TraceSourceData sourceData = new TraceSourceData("source1", SourceLevels.Critical);
            section.TraceSources.Add(sourceData);
            sourceData.TraceListeners.Add(new TraceListenerReferenceData("listener1"));
            sourceData.TraceListeners.Add(new TraceListenerReferenceData("listener2"));

            MockRegistryKey machineTraceSourcesKey = new MockRegistryKey(false);
            machineKey.AddSubKey(LoggingSettingsManageabilityProvider.CategorySourcesKeyName, machineTraceSourcesKey);
            MockRegistryKey machineSource1Key = new MockRegistryKey(false);
            machineTraceSourcesKey.AddSubKey("source1", machineSource1Key);
            machineSource1Key.AddEnumValue<SourceLevels>(LoggingSettingsManageabilityProvider.SourceDefaultLevelPropertyName, SourceLevels.ActivityTracing);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(0, sourceData.TraceListeners.Count);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineTraceSourcesKey, machineSource1Key));
        }

        [TestMethod]
        public void TraceSourceTraceListenersAreEmptiedIfListenerKeyIsEmptyInMachineKey()
        {
            TraceSourceData sourceData = new TraceSourceData("source1", SourceLevels.Critical);
            section.TraceSources.Add(sourceData);
            sourceData.TraceListeners.Add(new TraceListenerReferenceData("listener1"));
            sourceData.TraceListeners.Add(new TraceListenerReferenceData("listener2"));

            MockRegistryKey machineTraceSourcesKey = new MockRegistryKey(false);
            machineKey.AddSubKey(LoggingSettingsManageabilityProvider.CategorySourcesKeyName, machineTraceSourcesKey);
            MockRegistryKey machineSource1Key = new MockRegistryKey(false);
            machineTraceSourcesKey.AddSubKey("source1", machineSource1Key);
            machineSource1Key.AddEnumValue<SourceLevels>(LoggingSettingsManageabilityProvider.SourceDefaultLevelPropertyName, SourceLevels.ActivityTracing);
            MockRegistryKey machineSource1ListenersKey = new MockRegistryKey(false);
            machineSource1Key.AddSubKey(LoggingSettingsManageabilityProvider.SourceTraceListenersPropertyName, machineSource1ListenersKey);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(0, sourceData.TraceListeners.Count);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineTraceSourcesKey, machineSource1Key, machineSource1ListenersKey));
        }

        [TestMethod]
        public void TraceSourceTraceListenersAreOverridenIfListenerKeyIsPresentInMachineKey()
        {
            TraceSourceData sourceData = new TraceSourceData("source1", SourceLevels.Critical);
            section.TraceSources.Add(sourceData);
            sourceData.TraceListeners.Add(new TraceListenerReferenceData("listener1"));
            sourceData.TraceListeners.Add(new TraceListenerReferenceData("listener2"));

            MockRegistryKey machineTraceSourcesKey = new MockRegistryKey(false);
            machineKey.AddSubKey(LoggingSettingsManageabilityProvider.CategorySourcesKeyName, machineTraceSourcesKey);
            MockRegistryKey machineSource1Key = new MockRegistryKey(false);
            machineTraceSourcesKey.AddSubKey("source1", machineSource1Key);
            machineSource1Key.AddEnumValue<SourceLevels>(LoggingSettingsManageabilityProvider.SourceDefaultLevelPropertyName, SourceLevels.ActivityTracing);
            MockRegistryKey machineSource1ListenersKey = new MockRegistryKey(false);
            machineSource1Key.AddSubKey(LoggingSettingsManageabilityProvider.SourceTraceListenersPropertyName, machineSource1ListenersKey);
            machineSource1ListenersKey.AddBooleanValue("listener3", true);
            machineSource1ListenersKey.AddBooleanValue("listener4", true);
            machineSource1ListenersKey.AddBooleanValue("listener5", true);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(3, sourceData.TraceListeners.Count);
            Assert.IsNotNull(sourceData.TraceListeners.Get("listener3"));
            Assert.IsNotNull(sourceData.TraceListeners.Get("listener4"));
            Assert.IsNotNull(sourceData.TraceListeners.Get("listener5"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineTraceSourcesKey, machineSource1Key, machineSource1ListenersKey));
        }

        // the listeners key is set by the trace source policy, so the override of trace listeners must be performed
        // every time the source key exists, even if the listeners sub key is not present
        [TestMethod]
        public void TraceSourceTraceListenersAreEmptiedIfUserKeyIsPresentForSourceButKeyForListenersIsNot()
        {
            TraceSourceData sourceData = new TraceSourceData("source1", SourceLevels.Critical);
            section.TraceSources.Add(sourceData);
            sourceData.TraceListeners.Add(new TraceListenerReferenceData("listener1"));
            sourceData.TraceListeners.Add(new TraceListenerReferenceData("listener2"));

            MockRegistryKey userTraceSourcesKey = new MockRegistryKey(false);
            userKey.AddSubKey(LoggingSettingsManageabilityProvider.CategorySourcesKeyName, userTraceSourcesKey);
            MockRegistryKey userSource1Key = new MockRegistryKey(false);
            userTraceSourcesKey.AddSubKey("source1", userSource1Key);
            userSource1Key.AddEnumValue<SourceLevels>(LoggingSettingsManageabilityProvider.SourceDefaultLevelPropertyName, SourceLevels.ActivityTracing);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(0, sourceData.TraceListeners.Count);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(userTraceSourcesKey, userSource1Key));
        }

        [TestMethod]
        public void TraceSourceTraceListenersAreEmptiedIfListenerKeyIsEmptyInUserKey()
        {
            TraceSourceData sourceData = new TraceSourceData("source1", SourceLevels.Critical);
            section.TraceSources.Add(sourceData);
            sourceData.TraceListeners.Add(new TraceListenerReferenceData("listener1"));
            sourceData.TraceListeners.Add(new TraceListenerReferenceData("listener2"));

            MockRegistryKey userTraceSourcesKey = new MockRegistryKey(false);
            userKey.AddSubKey(LoggingSettingsManageabilityProvider.CategorySourcesKeyName, userTraceSourcesKey);
            MockRegistryKey userSource1Key = new MockRegistryKey(false);
            userTraceSourcesKey.AddSubKey("source1", userSource1Key);
            MockRegistryKey userSource1ListenersKey = new MockRegistryKey(false);
            userSource1Key.AddEnumValue<SourceLevels>(LoggingSettingsManageabilityProvider.SourceDefaultLevelPropertyName, SourceLevels.ActivityTracing);
            userSource1Key.AddSubKey(LoggingSettingsManageabilityProvider.SourceTraceListenersPropertyName, userSource1ListenersKey);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(0, sourceData.TraceListeners.Count);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(userTraceSourcesKey, userSource1Key, userSource1ListenersKey));
        }

        [TestMethod]
        public void TraceSourceTraceListenersAreOverridenIfListenerKeyIsPresentInUserKey()
        {
            TraceSourceData sourceData = new TraceSourceData("source1", SourceLevels.Critical);
            section.TraceSources.Add(sourceData);
            sourceData.TraceListeners.Add(new TraceListenerReferenceData("listener1"));
            sourceData.TraceListeners.Add(new TraceListenerReferenceData("listener2"));

            MockRegistryKey userTraceSourcesKey = new MockRegistryKey(false);
            userKey.AddSubKey(LoggingSettingsManageabilityProvider.CategorySourcesKeyName, userTraceSourcesKey);
            MockRegistryKey userSource1Key = new MockRegistryKey(false);
            userTraceSourcesKey.AddSubKey("source1", userSource1Key);
            userSource1Key.AddEnumValue<SourceLevels>(LoggingSettingsManageabilityProvider.SourceDefaultLevelPropertyName, SourceLevels.ActivityTracing);
            MockRegistryKey userSource1ListenersKey = new MockRegistryKey(false);
            userSource1Key.AddSubKey(LoggingSettingsManageabilityProvider.SourceTraceListenersPropertyName, userSource1ListenersKey);
            userSource1ListenersKey.AddBooleanValue("listener3", true);
            userSource1ListenersKey.AddBooleanValue("listener4", true);
            userSource1ListenersKey.AddBooleanValue("listener5", true);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(3, sourceData.TraceListeners.Count);
            Assert.IsNotNull(sourceData.TraceListeners.Get("listener3"));
            Assert.IsNotNull(sourceData.TraceListeners.Get("listener4"));
            Assert.IsNotNull(sourceData.TraceListeners.Get("listener5"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(userTraceSourcesKey, userSource1Key, userSource1ListenersKey));
        }

        [TestMethod]
        public void MachineKeyOverridesTakePrecedenceOverUserKeyOverrides()
        {
            TraceSourceData sourceData = new TraceSourceData("source1", SourceLevels.Critical);
            section.TraceSources.Add(sourceData);
            sourceData.TraceListeners.Add(new TraceListenerReferenceData("listener1"));
            sourceData.TraceListeners.Add(new TraceListenerReferenceData("listener2"));

            MockRegistryKey machineTraceSourcesKey = new MockRegistryKey(false);
            machineKey.AddSubKey(LoggingSettingsManageabilityProvider.CategorySourcesKeyName, machineTraceSourcesKey);
            MockRegistryKey machineSource1Key = new MockRegistryKey(false);
            machineTraceSourcesKey.AddSubKey("source1", machineSource1Key);
            machineSource1Key.AddEnumValue<SourceLevels>(LoggingSettingsManageabilityProvider.SourceDefaultLevelPropertyName, SourceLevels.ActivityTracing);
            MockRegistryKey machineSource1ListenersKey = new MockRegistryKey(false);
            machineSource1Key.AddSubKey(LoggingSettingsManageabilityProvider.SourceTraceListenersPropertyName, machineSource1ListenersKey);
            machineSource1ListenersKey.AddBooleanValue("listener3", true);
            machineSource1ListenersKey.AddBooleanValue("listener4", true);
            machineSource1ListenersKey.AddBooleanValue("listener5", true);
            MockRegistryKey userTraceSourcesKey = new MockRegistryKey(false);
            userKey.AddSubKey(LoggingSettingsManageabilityProvider.CategorySourcesKeyName, userTraceSourcesKey);
            MockRegistryKey userSource1Key = new MockRegistryKey(false);
            userTraceSourcesKey.AddSubKey("source1", userSource1Key);
            userSource1Key.AddEnumValue<SourceLevels>(LoggingSettingsManageabilityProvider.SourceDefaultLevelPropertyName, SourceLevels.ActivityTracing);
            MockRegistryKey userSource1ListenersKey = new MockRegistryKey(false);
            userSource1Key.AddSubKey(LoggingSettingsManageabilityProvider.SourceTraceListenersPropertyName, userSource1ListenersKey);
            userSource1ListenersKey.AddBooleanValue("listener6", true);
            userSource1ListenersKey.AddBooleanValue("listener7", true);
            userSource1ListenersKey.AddBooleanValue("listener8", true);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(3, sourceData.TraceListeners.Count);
            Assert.IsNotNull(sourceData.TraceListeners.Get("listener3"));
            Assert.IsNotNull(sourceData.TraceListeners.Get("listener4"));
            Assert.IsNotNull(sourceData.TraceListeners.Get("listener5"));

            Assert.IsTrue(
                MockRegistryKey.CheckAllClosed(
                    machineTraceSourcesKey, machineSource1Key, machineSource1ListenersKey,
                    userTraceSourcesKey, userSource1Key, userSource1ListenersKey));
        }

        [TestMethod]
        public void MachineKeySourceOverrideWithoutListenersSubKeyTakesPrecedenceOverUserKeyOverrides()
        {
            TraceSourceData sourceData = new TraceSourceData("source1", SourceLevels.Critical);
            section.TraceSources.Add(sourceData);
            sourceData.TraceListeners.Add(new TraceListenerReferenceData("listener1"));
            sourceData.TraceListeners.Add(new TraceListenerReferenceData("listener2"));

            MockRegistryKey machineTraceSourcesKey = new MockRegistryKey(false);
            machineKey.AddSubKey(LoggingSettingsManageabilityProvider.CategorySourcesKeyName, machineTraceSourcesKey);
            MockRegistryKey machineSource1Key = new MockRegistryKey(false);
            machineTraceSourcesKey.AddSubKey("source1", machineSource1Key);
            machineSource1Key.AddEnumValue<SourceLevels>(LoggingSettingsManageabilityProvider.SourceDefaultLevelPropertyName, SourceLevels.ActivityTracing);
            MockRegistryKey userTraceSourcesKey = new MockRegistryKey(false);
            userKey.AddSubKey(LoggingSettingsManageabilityProvider.CategorySourcesKeyName, userTraceSourcesKey);
            MockRegistryKey userSource1Key = new MockRegistryKey(false);
            userTraceSourcesKey.AddSubKey("source1", userSource1Key);
            userSource1Key.AddEnumValue<SourceLevels>(LoggingSettingsManageabilityProvider.SourceDefaultLevelPropertyName, SourceLevels.ActivityTracing);
            MockRegistryKey userSource1ListenersKey = new MockRegistryKey(false);
            userSource1Key.AddSubKey(LoggingSettingsManageabilityProvider.SourceTraceListenersPropertyName, userSource1ListenersKey);
            userSource1ListenersKey.AddBooleanValue("listener6", true);
            userSource1ListenersKey.AddBooleanValue("listener7", true);
            userSource1ListenersKey.AddBooleanValue("listener8", true);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(0, sourceData.TraceListeners.Count);

            Assert.IsTrue(
                MockRegistryKey.CheckAllClosed(
                    machineTraceSourcesKey, machineSource1Key,
                    userTraceSourcesKey, userSource1Key, userSource1ListenersKey));
        }

        [TestMethod]
        public void MachineOverridesAreAppliedToSpecialSources()
        {
            section.SpecialTraceSources.AllEventsTraceSource.DefaultLevel = SourceLevels.Critical;
            section.SpecialTraceSources.ErrorsTraceSource.DefaultLevel = SourceLevels.Critical;
            section.SpecialTraceSources.NotProcessedTraceSource.DefaultLevel = SourceLevels.Critical;

            MockRegistryKey machineSpecialSourcesKey = new MockRegistryKey(false);
            machineKey.AddSubKey(LoggingSettingsManageabilityProvider.SpecialSourcesKeyName, machineSpecialSourcesKey);
            MockRegistryKey machineAllEventsSourceKey = new MockRegistryKey(false);
            machineSpecialSourcesKey.AddSubKey(LoggingSettingsManageabilityProvider.SpecialSourcesAllEventsKeyName, machineAllEventsSourceKey);
            machineAllEventsSourceKey.AddStringValue(LoggingSettingsManageabilityProvider.SourceDefaultLevelPropertyName, SourceLevels.Error.ToString());
            MockRegistryKey machineErrorsSourceKey = new MockRegistryKey(false);
            machineSpecialSourcesKey.AddSubKey(LoggingSettingsManageabilityProvider.SpecialSourcesErrorsKeyName, machineErrorsSourceKey);
            machineErrorsSourceKey.AddStringValue(LoggingSettingsManageabilityProvider.SourceDefaultLevelPropertyName, SourceLevels.Information.ToString());
            MockRegistryKey machineNotProcessedSourceKey = new MockRegistryKey(false);
            machineSpecialSourcesKey.AddSubKey(LoggingSettingsManageabilityProvider.SpecialSourcesNotProcessedKeyName, machineNotProcessedSourceKey);
            machineNotProcessedSourceKey.AddStringValue(LoggingSettingsManageabilityProvider.SourceDefaultLevelPropertyName, SourceLevels.Warning.ToString());

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(SourceLevels.Error, section.SpecialTraceSources.AllEventsTraceSource.DefaultLevel);
            Assert.AreEqual(SourceLevels.Information, section.SpecialTraceSources.ErrorsTraceSource.DefaultLevel);
            Assert.AreEqual(SourceLevels.Warning, section.SpecialTraceSources.NotProcessedTraceSource.DefaultLevel);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineSpecialSourcesKey,
                                                         machineAllEventsSourceKey, machineErrorsSourceKey, machineNotProcessedSourceKey));
        }

        [TestMethod]
        public void UserOverridesAreAppliedToSpecialSources()
        {
            section.SpecialTraceSources.AllEventsTraceSource.DefaultLevel = SourceLevels.Critical;
            section.SpecialTraceSources.ErrorsTraceSource.DefaultLevel = SourceLevels.Critical;
            section.SpecialTraceSources.NotProcessedTraceSource.DefaultLevel = SourceLevels.Critical;

            MockRegistryKey machineSpecialSourcesKey = new MockRegistryKey(false);
            machineKey.AddSubKey(LoggingSettingsManageabilityProvider.SpecialSourcesKeyName, machineSpecialSourcesKey);
            MockRegistryKey machineAllEventsSourceKey = new MockRegistryKey(false);
            machineSpecialSourcesKey.AddSubKey(LoggingSettingsManageabilityProvider.SpecialSourcesAllEventsKeyName, machineAllEventsSourceKey);
            machineAllEventsSourceKey.AddStringValue(LoggingSettingsManageabilityProvider.SourceDefaultLevelPropertyName, SourceLevels.Error.ToString());
            MockRegistryKey userSpecialSourcesKey = new MockRegistryKey(false);
            userKey.AddSubKey(LoggingSettingsManageabilityProvider.SpecialSourcesKeyName, userSpecialSourcesKey);
            MockRegistryKey userAllEventsSourceKey = new MockRegistryKey(false);
            userSpecialSourcesKey.AddSubKey(LoggingSettingsManageabilityProvider.SpecialSourcesAllEventsKeyName, userAllEventsSourceKey);
            userAllEventsSourceKey.AddStringValue(LoggingSettingsManageabilityProvider.SourceDefaultLevelPropertyName, SourceLevels.Information.ToString());
            MockRegistryKey userErrorsSourceKey = new MockRegistryKey(false);
            userSpecialSourcesKey.AddSubKey(LoggingSettingsManageabilityProvider.SpecialSourcesErrorsKeyName, userErrorsSourceKey);
            userErrorsSourceKey.AddStringValue(LoggingSettingsManageabilityProvider.SourceDefaultLevelPropertyName, SourceLevels.Information.ToString());

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(SourceLevels.Error, section.SpecialTraceSources.AllEventsTraceSource.DefaultLevel);
            Assert.AreEqual(SourceLevels.Information, section.SpecialTraceSources.ErrorsTraceSource.DefaultLevel);
            Assert.AreEqual(SourceLevels.Critical, section.SpecialTraceSources.NotProcessedTraceSource.DefaultLevel);

            Assert.IsTrue(
                MockRegistryKey.CheckAllClosed(machineSpecialSourcesKey, machineAllEventsSourceKey,
                                               userSpecialSourcesKey, userAllEventsSourceKey, userErrorsSourceKey));
        }

        [TestMethod]
        public void MachineOverridesTakePrecedenceOverUserOverrides()
        {
            section.SpecialTraceSources.AllEventsTraceSource.DefaultLevel = SourceLevels.Critical;
            section.SpecialTraceSources.ErrorsTraceSource.DefaultLevel = SourceLevels.Critical;
            section.SpecialTraceSources.NotProcessedTraceSource.DefaultLevel = SourceLevels.Critical;

            MockRegistryKey userSpecialSourcesKey = new MockRegistryKey(false);
            userKey.AddSubKey(LoggingSettingsManageabilityProvider.SpecialSourcesKeyName, userSpecialSourcesKey);
            MockRegistryKey userAllEventsSourceKey = new MockRegistryKey(false);
            userSpecialSourcesKey.AddSubKey(LoggingSettingsManageabilityProvider.SpecialSourcesAllEventsKeyName, userAllEventsSourceKey);
            userAllEventsSourceKey.AddStringValue(LoggingSettingsManageabilityProvider.SourceDefaultLevelPropertyName, SourceLevels.Error.ToString());
            MockRegistryKey userErrorsSourceKey = new MockRegistryKey(false);
            userSpecialSourcesKey.AddSubKey(LoggingSettingsManageabilityProvider.SpecialSourcesErrorsKeyName, userErrorsSourceKey);
            userErrorsSourceKey.AddStringValue(LoggingSettingsManageabilityProvider.SourceDefaultLevelPropertyName, SourceLevels.Information.ToString());
            MockRegistryKey userNotProcessedSourceKey = new MockRegistryKey(false);
            userSpecialSourcesKey.AddSubKey(LoggingSettingsManageabilityProvider.SpecialSourcesNotProcessedKeyName, userNotProcessedSourceKey);
            userNotProcessedSourceKey.AddStringValue(LoggingSettingsManageabilityProvider.SourceDefaultLevelPropertyName, SourceLevels.Warning.ToString());

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(SourceLevels.Error, section.SpecialTraceSources.AllEventsTraceSource.DefaultLevel);
            Assert.AreEqual(SourceLevels.Information, section.SpecialTraceSources.ErrorsTraceSource.DefaultLevel);
            Assert.AreEqual(SourceLevels.Warning, section.SpecialTraceSources.NotProcessedTraceSource.DefaultLevel);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(userSpecialSourcesKey,
                                                         userAllEventsSourceKey, userErrorsSourceKey, userNotProcessedSourceKey));
        }

        [TestMethod]
        public void LogFilterWithDisabledPolicyIsRemoved()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(LogFilterData), registeredProvider);
            provider = new LoggingSettingsManageabilityProvider(subProviders);

            LogFilterData logFilter1Data = new LogFilterData("logFilter1", typeof(Object));
            section.LogFilters.Add(logFilter1Data);
            LogFilterData logFilter2Data = new LogFilterData("logFilter2", typeof(Object));
            section.LogFilters.Add(logFilter2Data);

            MockRegistryKey machineLogFiltersKey = new MockRegistryKey(false);
            machineKey.AddSubKey(LoggingSettingsManageabilityProvider.LogFiltersKeyName, machineLogFiltersKey);
            MockRegistryKey machineLogFilter2Key = new MockRegistryKey(false);
            machineLogFiltersKey.AddSubKey("logFilter2", machineLogFilter2Key);
            machineLogFilter2Key.AddBooleanValue(AdmContentBuilder.AvailableValueName, false);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, section.LogFilters.Count);
            Assert.IsNotNull(section.LogFilters.Get("logFilter1"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineLogFiltersKey, machineLogFilter2Key));
        }

        [TestMethod]
        public void LogFilterWithDisabledPolicyIsNotRemovedIfGroupPoliciesAreDisabled()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(LogFilterData), registeredProvider);
            provider = new LoggingSettingsManageabilityProvider(subProviders);

            LogFilterData logFilter1Data = new LogFilterData("logFilter1", typeof(Object));
            section.LogFilters.Add(logFilter1Data);
            LogFilterData logFilter2Data = new LogFilterData("logFilter2", typeof(Object));
            section.LogFilters.Add(logFilter2Data);

            MockRegistryKey machineLogFiltersKey = new MockRegistryKey(false);
            machineKey.AddSubKey(LoggingSettingsManageabilityProvider.LogFiltersKeyName, machineLogFiltersKey);
            MockRegistryKey machineLogFilter2Key = new MockRegistryKey(false);
            machineLogFiltersKey.AddSubKey("logFilter2", machineLogFilter2Key);
            machineLogFilter2Key.AddBooleanValue(AdmContentBuilder.AvailableValueName, false);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(2, section.LogFilters.Count);
            Assert.IsNotNull(section.LogFilters.Get("logFilter1"));
            Assert.IsNotNull(section.LogFilters.Get("logFilter2"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineLogFiltersKey, machineLogFilter2Key));
        }

        [TestMethod]
        public void LogFormatterWithDisabledPolicyIsRemoved()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(FormatterData), registeredProvider);
            provider = new LoggingSettingsManageabilityProvider(subProviders);

            FormatterData logFormatter1Data = new FormatterData("logFormatter1", typeof(Object));
            section.Formatters.Add(logFormatter1Data);
            FormatterData logFormatter2Data = new FormatterData("logFormatter2", typeof(Object));
            section.Formatters.Add(logFormatter2Data);

            MockRegistryKey machineLogFormattersKey = new MockRegistryKey(false);
            machineKey.AddSubKey(LoggingSettingsManageabilityProvider.LogFormattersKeyName, machineLogFormattersKey);
            MockRegistryKey machineLogFormatter2Key = new MockRegistryKey(false);
            machineLogFormattersKey.AddSubKey("logFormatter2", machineLogFormatter2Key);
            machineLogFormatter2Key.AddBooleanValue(AdmContentBuilder.AvailableValueName, false);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, section.Formatters.Count);
            Assert.IsNotNull(section.Formatters.Get("logFormatter1"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineLogFormattersKey, machineLogFormatter2Key));
        }

        [TestMethod]
        public void LogFormatterWithDisabledPolicyIsNotRemovedIfGroupPoliciesAreDisabled()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(FormatterData), registeredProvider);
            provider = new LoggingSettingsManageabilityProvider(subProviders);

            FormatterData logFormatter1Data = new FormatterData("logFormatter1", typeof(Object));
            section.Formatters.Add(logFormatter1Data);
            FormatterData logFormatter2Data = new FormatterData("logFormatter2", typeof(Object));
            section.Formatters.Add(logFormatter2Data);

            MockRegistryKey machineLogFormattersKey = new MockRegistryKey(false);
            machineKey.AddSubKey(LoggingSettingsManageabilityProvider.LogFormattersKeyName, machineLogFormattersKey);
            MockRegistryKey machineLogFormatter2Key = new MockRegistryKey(false);
            machineLogFormattersKey.AddSubKey("logFormatter2", machineLogFormatter2Key);
            machineLogFormatter2Key.AddBooleanValue(AdmContentBuilder.AvailableValueName, false);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(2, section.Formatters.Count);
            Assert.IsNotNull(section.Formatters.Get("logFormatter1"));
            Assert.IsNotNull(section.Formatters.Get("logFormatter2"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineLogFormattersKey, machineLogFormatter2Key));
        }

        [TestMethod]
        public void TraceListenerWithDisabledPolicyIsRemoved()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(TraceListenerData), registeredProvider);
            provider = new LoggingSettingsManageabilityProvider(subProviders);

            TraceListenerData traceListener1Data = new TraceListenerData();
            traceListener1Data.Name = "traceListener1";
            section.TraceListeners.Add(traceListener1Data);
            TraceListenerData traceListener2Data = new TraceListenerData();
            traceListener2Data.Name = "traceListener2";
            section.TraceListeners.Add(traceListener2Data);

            MockRegistryKey machineTraceListenersKey = new MockRegistryKey(false);
            machineKey.AddSubKey(LoggingSettingsManageabilityProvider.TraceListenersKeyName, machineTraceListenersKey);
            MockRegistryKey machineTraceListener2Key = new MockRegistryKey(false);
            machineTraceListenersKey.AddSubKey("traceListener2", machineTraceListener2Key);
            machineTraceListener2Key.AddBooleanValue(AdmContentBuilder.AvailableValueName, false);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, section.TraceListeners.Count);
            Assert.IsNotNull(section.TraceListeners.Get("traceListener1"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineTraceListenersKey, machineTraceListener2Key));
        }

        [TestMethod]
        public void TraceListenerWithDisabledPolicyIsNotRemovedIfGroupPoliciesAreDisabled()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(TraceListenerData), registeredProvider);
            provider = new LoggingSettingsManageabilityProvider(subProviders);

            TraceListenerData traceListener1Data = new TraceListenerData();
            traceListener1Data.Name = "traceListener1";
            section.TraceListeners.Add(traceListener1Data);
            TraceListenerData traceListener2Data = new TraceListenerData();
            traceListener2Data.Name = "traceListener2";
            section.TraceListeners.Add(traceListener2Data);

            MockRegistryKey machineTraceListenersKey = new MockRegistryKey(false);
            machineKey.AddSubKey(LoggingSettingsManageabilityProvider.TraceListenersKeyName, machineTraceListenersKey);
            MockRegistryKey machineTraceListener2Key = new MockRegistryKey(false);
            machineTraceListenersKey.AddSubKey("traceListener2", machineTraceListener2Key);
            machineTraceListener2Key.AddBooleanValue(AdmContentBuilder.AvailableValueName, false);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(2, section.TraceListeners.Count);
            Assert.IsNotNull(section.TraceListeners.Get("traceListener1"));
            Assert.IsNotNull(section.TraceListeners.Get("traceListener2"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineTraceListenersKey, machineTraceListener2Key));
        }

        [TestMethod]
        public void TraceSourceWithDisabledPolicyIsRemoved()
        {
            TraceSourceData traceSource1Data = new TraceSourceData("traceSource1", SourceLevels.Error);
            section.TraceSources.Add(traceSource1Data);
            TraceSourceData traceSource2Data = new TraceSourceData("traceSource2", SourceLevels.Error);
            section.TraceSources.Add(traceSource2Data);

            MockRegistryKey machineTraceSourcesKey = new MockRegistryKey(false);
            machineKey.AddSubKey(LoggingSettingsManageabilityProvider.CategorySourcesKeyName, machineTraceSourcesKey);
            MockRegistryKey machineTraceSource2Key = new MockRegistryKey(false);
            machineTraceSourcesKey.AddSubKey("traceSource2", machineTraceSource2Key);
            machineTraceSource2Key.AddBooleanValue(AdmContentBuilder.AvailableValueName, false);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, section.TraceSources.Count);
            Assert.IsNotNull(section.TraceSources.Get("traceSource1"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineTraceSourcesKey, machineTraceSource2Key));
        }

        [TestMethod]
        public void TraceSourceWithDisabledPolicyIsNotRemovedIfGroupPoliciesAreDisabled()
        {
            TraceSourceData traceSource1Data = new TraceSourceData("traceSource1", SourceLevels.Error);
            section.TraceSources.Add(traceSource1Data);
            TraceSourceData traceSource2Data = new TraceSourceData("traceSource2", SourceLevels.Error);
            section.TraceSources.Add(traceSource2Data);

            MockRegistryKey machineTraceSourcesKey = new MockRegistryKey(false);
            machineKey.AddSubKey(LoggingSettingsManageabilityProvider.CategorySourcesKeyName, machineTraceSourcesKey);
            MockRegistryKey machineTraceSource2Key = new MockRegistryKey(false);
            machineTraceSourcesKey.AddSubKey("traceSource2", machineTraceSource2Key);
            machineTraceSource2Key.AddBooleanValue(AdmContentBuilder.AvailableValueName, false);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(2, section.TraceSources.Count);
            Assert.IsNotNull(section.TraceSources.Get("traceSource1"));
            Assert.IsNotNull(section.TraceSources.Get("traceSource2"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineTraceSourcesKey, machineTraceSource2Key));
        }

        [TestMethod]
        public void SpecialTraceSourceWithDisabledPolicyIsInvalidated()
        {
            section.SpecialTraceSources.AllEventsTraceSource.TraceListeners.Add(new TraceListenerReferenceData("listener"));

            MockRegistryKey machineTraceSpecialSourcesKey = new MockRegistryKey(false);
            machineKey.AddSubKey(LoggingSettingsManageabilityProvider.SpecialSourcesKeyName, machineTraceSpecialSourcesKey);
            MockRegistryKey machineAllEventsTraceSourceKey = new MockRegistryKey(false);
            machineTraceSpecialSourcesKey.AddSubKey(LoggingSettingsManageabilityProvider.SpecialSourcesAllEventsKeyName, machineAllEventsTraceSourceKey);
            machineAllEventsTraceSourceKey.AddBooleanValue(AdmContentBuilder.AvailableValueName, false);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(0, section.SpecialTraceSources.AllEventsTraceSource.TraceListeners.Count);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineTraceSpecialSourcesKey, machineAllEventsTraceSourceKey));
        }

        [TestMethod]
        public void SpecialTraceSourceWithDisabledPolicyIsNotInvalidatedIfGroupPoliciesAreDisabled()
        {
            section.SpecialTraceSources.AllEventsTraceSource.TraceListeners.Add(new TraceListenerReferenceData("listener"));

            MockRegistryKey machineTraceSpecialSourcesKey = new MockRegistryKey(false);
            machineKey.AddSubKey(LoggingSettingsManageabilityProvider.SpecialSourcesKeyName, machineTraceSpecialSourcesKey);
            MockRegistryKey machineAllEventsTraceSourceKey = new MockRegistryKey(false);
            machineTraceSpecialSourcesKey.AddSubKey(LoggingSettingsManageabilityProvider.SpecialSourcesAllEventsKeyName, machineAllEventsTraceSourceKey);
            machineAllEventsTraceSourceKey.AddBooleanValue(AdmContentBuilder.AvailableValueName, false);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, section.SpecialTraceSources.AllEventsTraceSource.TraceListeners.Count);
            Assert.IsNotNull(section.SpecialTraceSources.AllEventsTraceSource.TraceListeners.Get("listener"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineTraceSpecialSourcesKey, machineAllEventsTraceSourceKey));
        }

        [TestMethod]
        public void RegisteredLogFilterDataProviderIsCalledWithNoOverrides()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(PriorityFilterData), registeredProvider);
            provider = new LoggingSettingsManageabilityProvider(subProviders);

            PriorityFilterData filterData = new PriorityFilterData(10);
            section.LogFilters.Add(filterData);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.IsTrue(registeredProvider.called);
            Assert.AreSame(filterData, registeredProvider.LastConfigurationObject);
            Assert.AreEqual(null, registeredProvider.machineKey);
            Assert.AreEqual(null, registeredProvider.userKey);
        }

        [TestMethod]
        public void RegisteredLogFilterDataProviderIsCalledWithCorrectOverrides()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(PriorityFilterData), registeredProvider);
            provider = new LoggingSettingsManageabilityProvider(subProviders);

            PriorityFilterData filterData = new PriorityFilterData("filter1", 10);
            section.LogFilters.Add(filterData);

            MockRegistryKey machineFiltersKey = new MockRegistryKey(false);
            machineKey.AddSubKey(LoggingSettingsManageabilityProvider.LogFiltersKeyName, machineFiltersKey);
            MockRegistryKey machineFilterKey = new MockRegistryKey(false);
            machineFiltersKey.AddSubKey("filter1", machineFilterKey);
            MockRegistryKey machineOtherFilterKey = new MockRegistryKey(false);
            machineFiltersKey.AddSubKey("filter2", machineOtherFilterKey);

            MockRegistryKey userFiltersKey = new MockRegistryKey(false);
            userKey.AddSubKey(LoggingSettingsManageabilityProvider.LogFiltersKeyName, userFiltersKey);
            MockRegistryKey userFilterKey = new MockRegistryKey(false);
            userFiltersKey.AddSubKey("filter1", userFilterKey);
            MockRegistryKey userOtherFilterKey = new MockRegistryKey(false);
            userFiltersKey.AddSubKey("filter2", userOtherFilterKey);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.IsTrue(registeredProvider.called);
            Assert.AreSame(filterData, registeredProvider.LastConfigurationObject);
            Assert.AreSame(machineFilterKey, registeredProvider.machineKey);
            Assert.AreSame(userFilterKey, registeredProvider.userKey);

            Assert.IsTrue(
                MockRegistryKey.CheckAllClosed(machineFiltersKey, machineFilterKey, machineOtherFilterKey,
                                               userFiltersKey, userFilterKey, userOtherFilterKey));
        }

        [TestMethod]
        public void RegisteredFormatterDataProviderIsCalledWithNoOverrides()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(TextFormatterData), registeredProvider);
            provider = new LoggingSettingsManageabilityProvider(subProviders);

            TextFormatterData formatterData = new TextFormatterData("name", "template");
            section.Formatters.Add(formatterData);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.IsTrue(registeredProvider.called);
            Assert.AreSame(formatterData, registeredProvider.LastConfigurationObject);
            Assert.AreEqual(null, registeredProvider.machineKey);
            Assert.AreEqual(null, registeredProvider.userKey);
        }

        [TestMethod]
        public void RegisteredFormatterDataProviderIsCalledWithCorrectOverrides()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(TextFormatterData), registeredProvider);
            provider = new LoggingSettingsManageabilityProvider(subProviders);

            TextFormatterData formatterData = new TextFormatterData("formatter1", "template");
            section.Formatters.Add(formatterData);

            MockRegistryKey machineFormattersKey = new MockRegistryKey(false);
            machineKey.AddSubKey(LoggingSettingsManageabilityProvider.LogFormattersKeyName, machineFormattersKey);
            MockRegistryKey machineFormatterKey = new MockRegistryKey(false);
            machineFormattersKey.AddSubKey("formatter1", machineFormatterKey);
            MockRegistryKey machineOtherFormatterKey = new MockRegistryKey(false);
            machineFormattersKey.AddSubKey("formatter2", machineOtherFormatterKey);

            MockRegistryKey userFormattersKey = new MockRegistryKey(false);
            userKey.AddSubKey(LoggingSettingsManageabilityProvider.LogFormattersKeyName, userFormattersKey);
            MockRegistryKey userFormatterKey = new MockRegistryKey(false);
            userFormattersKey.AddSubKey("formatter1", userFormatterKey);
            MockRegistryKey userOtherFormatterKey = new MockRegistryKey(false);
            userFormattersKey.AddSubKey("formatter2", userOtherFormatterKey);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.IsTrue(registeredProvider.called);
            Assert.AreSame(formatterData, registeredProvider.LastConfigurationObject);
            Assert.AreSame(machineFormatterKey, registeredProvider.machineKey);
            Assert.AreSame(userFormatterKey, registeredProvider.userKey);

            Assert.IsTrue(
                MockRegistryKey.CheckAllClosed(machineFormattersKey, machineFormatterKey, machineOtherFormatterKey,
                                               userFormattersKey, userFormatterKey, userOtherFormatterKey));
        }

        [TestMethod]
        public void RegisteredTraceListenerDataProviderIsCalledWithNoOverrides()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(FormattedEventLogTraceListenerData), registeredProvider);
            provider = new LoggingSettingsManageabilityProvider(subProviders);

            FormattedEventLogTraceListenerData listenerData
                = new FormattedEventLogTraceListenerData("name", "source", "formatter");
            section.TraceListeners.Add(listenerData);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.IsTrue(registeredProvider.called);
            Assert.AreSame(listenerData, registeredProvider.LastConfigurationObject);
            Assert.AreEqual(null, registeredProvider.machineKey);
            Assert.AreEqual(null, registeredProvider.userKey);
        }

        [TestMethod]
        public void RegisteredListenerDataProviderIsCalledWithCorrectOverrides()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(FormattedEventLogTraceListenerData), registeredProvider);
            provider = new LoggingSettingsManageabilityProvider(subProviders);

            FormattedEventLogTraceListenerData listenerData
                = new FormattedEventLogTraceListenerData("listener1", "source", "formatter");
            section.TraceListeners.Add(listenerData);

            MockRegistryKey machineListenersKey = new MockRegistryKey(false);
            machineKey.AddSubKey(LoggingSettingsManageabilityProvider.TraceListenersKeyName, machineListenersKey);
            MockRegistryKey machineListenerKey = new MockRegistryKey(false);
            machineListenersKey.AddSubKey("listener1", machineListenerKey);
            MockRegistryKey machineOtherListenerKey = new MockRegistryKey(false);
            machineListenersKey.AddSubKey("listener2", machineOtherListenerKey);

            MockRegistryKey userListenersKey = new MockRegistryKey(false);
            userKey.AddSubKey(LoggingSettingsManageabilityProvider.TraceListenersKeyName, userListenersKey);
            MockRegistryKey userListenerKey = new MockRegistryKey(false);
            userListenersKey.AddSubKey("listener1", userListenerKey);
            MockRegistryKey userOtherListenerKey = new MockRegistryKey(false);
            userListenersKey.AddSubKey("listener2", userOtherListenerKey);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.IsTrue(registeredProvider.called);
            Assert.AreSame(listenerData, registeredProvider.LastConfigurationObject);
            Assert.AreSame(machineListenerKey, registeredProvider.machineKey);
            Assert.AreSame(userListenerKey, registeredProvider.userKey);

            Assert.IsTrue(
                MockRegistryKey.CheckAllClosed(machineListenersKey, machineListenerKey, machineOtherListenerKey,
                                               userListenersKey, userListenerKey, userOtherListenerKey));
        }

        [TestMethod]
        public void WmiSettingsAreNotGeneratedIfWmiIsDisabled()
        {
            section.DefaultCategory = "source1";
            section.LogWarningWhenNoCategoriesMatch = false;
            section.TracingEnabled = true;
            TraceSourceData sourceData = new TraceSourceData("source1", SourceLevels.Critical);
            section.TraceSources.Add(sourceData);
            sourceData.TraceListeners.Add(new TraceListenerReferenceData("listener1"));
            sourceData.TraceListeners.Add(new TraceListenerReferenceData("listener2"));

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, false, wmiSettings);

            Assert.AreEqual(0, wmiSettings.Count);
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedIfWmiIsEnabled()
        {
            section.DefaultCategory = "source1";
            section.LogWarningWhenNoCategoriesMatch = false;
            section.TracingEnabled = true;
            section.RevertImpersonation = false;
            TraceSourceData sourceData = new TraceSourceData("source1", SourceLevels.Critical);
            section.TraceSources.Add(sourceData);
            sourceData.TraceListeners.Add(new TraceListenerReferenceData("listener1"));
            sourceData.TraceListeners.Add(new TraceListenerReferenceData("listener2"));

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(5, wmiSettings.Count); // 3 wmi settings created for special trace sources
            Assert.AreSame(typeof(LoggingBlockSetting), wmiSettings[0].GetType());
            Assert.AreEqual("source1", ((LoggingBlockSetting)wmiSettings[0]).DefaultCategory);
            Assert.AreEqual(false, ((LoggingBlockSetting)wmiSettings[0]).LogWarningWhenNoCategoriesMatch);
            Assert.AreEqual(true, ((LoggingBlockSetting)wmiSettings[0]).TracingEnabled);
            Assert.AreEqual(false, ((LoggingBlockSetting)wmiSettings[0]).RevertImpersonation);
            Assert.AreSame(typeof(TraceSourceSetting), wmiSettings[1].GetType());
            Assert.AreEqual(SourceLevels.Critical.ToString(), ((TraceSourceSetting)wmiSettings[1]).DefaultLevel);
            Assert.AreEqual(2, ((TraceSourceSetting)wmiSettings[1]).TraceListeners.Length);
            Assert.AreEqual(LoggingSettingsManageabilityProvider.SourceKindCategory, ((TraceSourceSetting)wmiSettings[1]).Kind);
            Assert.AreSame(typeof(TraceSourceSetting), wmiSettings[2].GetType());
            Assert.AreEqual(LoggingSettingsManageabilityProvider.SourceKindAllEvents, ((TraceSourceSetting)wmiSettings[2]).Kind);
            Assert.AreSame(typeof(TraceSourceSetting), wmiSettings[3].GetType());
            Assert.AreEqual(LoggingSettingsManageabilityProvider.SourceKindNotProcessed, ((TraceSourceSetting)wmiSettings[3]).Kind);
            Assert.AreSame(typeof(TraceSourceSetting), wmiSettings[4].GetType());
            Assert.AreEqual(LoggingSettingsManageabilityProvider.SourceKindErrors, ((TraceSourceSetting)wmiSettings[4]).Kind);
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedWithPolicyOverridesIfWmiIsEnabled()
        {
            section.DefaultCategory = "source1";
            section.LogWarningWhenNoCategoriesMatch = false;
            section.TracingEnabled = true;
            section.RevertImpersonation = false;
            TraceSourceData sourceData = new TraceSourceData("source1", SourceLevels.Critical);
            section.TraceSources.Add(sourceData);
            sourceData.TraceListeners.Add(new TraceListenerReferenceData("listener1"));
            sourceData.TraceListeners.Add(new TraceListenerReferenceData("listener2"));

            machineKey.AddBooleanValue(LoggingSettingsManageabilityProvider.PolicyValueName, true);
            machineKey.AddStringValue(LoggingSettingsManageabilityProvider.DefaultCategoryPropertyName, "overrideSource1");
            machineKey.AddBooleanValue(LoggingSettingsManageabilityProvider.LogWarningOnNoMatchPropertyName, true);
            machineKey.AddBooleanValue(LoggingSettingsManageabilityProvider.TracingEnabledPropertyName, false);
            machineKey.AddBooleanValue(LoggingSettingsManageabilityProvider.RevertImpersonationPropertyName, true);
            MockRegistryKey machineTraceSourcesKey = new MockRegistryKey(false);
            machineKey.AddSubKey(LoggingSettingsManageabilityProvider.CategorySourcesKeyName, machineTraceSourcesKey);
            MockRegistryKey machineSource1Key = new MockRegistryKey(false);
            machineTraceSourcesKey.AddSubKey("source1", machineSource1Key);
            machineSource1Key.AddEnumValue<SourceLevels>(LoggingSettingsManageabilityProvider.SourceDefaultLevelPropertyName, SourceLevels.Error);
            MockRegistryKey machineSource1ListenersKey = new MockRegistryKey(false);
            machineSource1Key.AddSubKey(LoggingSettingsManageabilityProvider.SourceTraceListenersPropertyName, machineSource1ListenersKey);
            machineSource1ListenersKey.AddBooleanValue("listener3", true);
            machineSource1ListenersKey.AddBooleanValue("listener4", true);
            machineSource1ListenersKey.AddBooleanValue("listener5", true);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(5, wmiSettings.Count); // 3 wmi settings created for special trace sources
            Assert.AreSame(typeof(LoggingBlockSetting), wmiSettings[0].GetType());
            Assert.AreEqual("overrideSource1", ((LoggingBlockSetting)wmiSettings[0]).DefaultCategory);
            Assert.AreEqual(true, ((LoggingBlockSetting)wmiSettings[0]).LogWarningWhenNoCategoriesMatch);
            Assert.AreEqual(false, ((LoggingBlockSetting)wmiSettings[0]).TracingEnabled);
            Assert.AreEqual(true, ((LoggingBlockSetting)wmiSettings[0]).RevertImpersonation);
            Assert.AreSame(typeof(TraceSourceSetting), wmiSettings[1].GetType());
            Assert.AreEqual(SourceLevels.Error.ToString(), ((TraceSourceSetting)wmiSettings[1]).DefaultLevel);
            Assert.AreEqual(3, ((TraceSourceSetting)wmiSettings[1]).TraceListeners.Length);
            Assert.AreEqual(LoggingSettingsManageabilityProvider.SourceKindCategory, ((TraceSourceSetting)wmiSettings[1]).Kind);
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContentForEmptySection()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(LoggingSettings.SectionName, section);

            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            provider = new LoggingSettingsManageabilityProvider(subProviders);

            MockAdmContentBuilder contentBuilder = new MockAdmContentBuilder();

            provider.AddAdministrativeTemplateDirectives(contentBuilder, section, configurationSource, "TestApp");

            MockAdmContent content = contentBuilder.GetMockContent();
            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            Assert.IsTrue(categoriesEnumerator.MoveNext());
            IEnumerator<AdmCategory> subCategoriesEnumerator = categoriesEnumerator.Current.Categories.GetEnumerator();
            Assert.IsTrue(subCategoriesEnumerator.MoveNext());
            Assert.IsTrue(subCategoriesEnumerator.MoveNext());
            Assert.IsTrue(subCategoriesEnumerator.MoveNext());
            Assert.IsTrue(subCategoriesEnumerator.MoveNext());
            Assert.IsTrue(subCategoriesEnumerator.MoveNext());
            Assert.IsFalse(subCategoriesEnumerator.MoveNext());
            IEnumerator<AdmPolicy> sectionPoliciesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            Assert.IsTrue(sectionPoliciesEnumerator.MoveNext());
            IEnumerator<AdmPart> sectionPartsEnumerator = sectionPoliciesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(sectionPartsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), sectionPartsEnumerator.Current.GetType());
            Assert.AreEqual(LoggingSettingsManageabilityProvider.DefaultCategoryPropertyName,
                            ((AdmDropDownListPart)sectionPartsEnumerator.Current).ValueName);
            Assert.IsTrue(sectionPartsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmCheckboxPart), sectionPartsEnumerator.Current.GetType());
            Assert.AreEqual(LoggingSettingsManageabilityProvider.LogWarningOnNoMatchPropertyName,
                            ((AdmCheckboxPart)sectionPartsEnumerator.Current).ValueName);
            Assert.IsTrue(sectionPartsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmCheckboxPart), sectionPartsEnumerator.Current.GetType());
            Assert.AreEqual(LoggingSettingsManageabilityProvider.TracingEnabledPropertyName,
                            ((AdmCheckboxPart)sectionPartsEnumerator.Current).ValueName);
            Assert.IsTrue(sectionPartsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmCheckboxPart), sectionPartsEnumerator.Current.GetType());
            Assert.AreEqual(LoggingSettingsManageabilityProvider.RevertImpersonationPropertyName,
                            ((AdmCheckboxPart)sectionPartsEnumerator.Current).ValueName);
            Assert.IsFalse(sectionPartsEnumerator.MoveNext());
            Assert.IsFalse(sectionPoliciesEnumerator.MoveNext());
            Assert.IsFalse(categoriesEnumerator.MoveNext());
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContentForTraceSources()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(LoggingSettings.SectionName, section);
            section.TraceSources.Add(new TraceSourceData("source1", SourceLevels.Error));
            section.TraceSources.Add(new TraceSourceData("source2", SourceLevels.Error));
            section.TraceListeners.Add(new FlatFileTraceListenerData("listener1", "file", ""));

            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            provider = new LoggingSettingsManageabilityProvider(subProviders);

            MockAdmContentBuilder contentBuilder = new MockAdmContentBuilder();

            provider.AddAdministrativeTemplateDirectives(contentBuilder, section, configurationSource, "TestApp");

            MockAdmContent content = contentBuilder.GetMockContent();
            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            categoriesEnumerator.MoveNext();
            IEnumerator<AdmCategory> subCategoriesEnumerator = categoriesEnumerator.Current.Categories.GetEnumerator();
            subCategoriesEnumerator.MoveNext(); // first is trace sources category
            IEnumerator<AdmPolicy> sourcePoliciesEnumerator = subCategoriesEnumerator.Current.Policies.GetEnumerator();
            Assert.IsTrue(sourcePoliciesEnumerator.MoveNext());
            IEnumerator<AdmPart> sourcePartsEnumerator = sourcePoliciesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(sourcePartsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), sourcePartsEnumerator.Current.GetType());
            Assert.AreEqual(LoggingSettingsManageabilityProvider.SourceDefaultLevelPropertyName,
                            sourcePartsEnumerator.Current.ValueName);
            Assert.IsTrue(sourcePartsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmTextPart), sourcePartsEnumerator.Current.GetType());
            Assert.IsTrue(sourcePartsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmCheckboxPart), sourcePartsEnumerator.Current.GetType());
            Assert.AreEqual("listener1", sourcePartsEnumerator.Current.ValueName);
            Assert.AreEqual("listener1", sourcePartsEnumerator.Current.PartName);
            Assert.IsTrue(sourcePartsEnumerator.Current.KeyName.EndsWith(LoggingSettingsManageabilityProvider.SourceTraceListenersPropertyName));
            Assert.IsFalse(sourcePartsEnumerator.MoveNext());
            Assert.IsTrue(sourcePoliciesEnumerator.MoveNext()); // there are 2 sources
            Assert.IsFalse(sourcePoliciesEnumerator.MoveNext());
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContentForSpecialTraceSources()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(LoggingSettings.SectionName, section);
            section.TraceListeners.Add(new FlatFileTraceListenerData("listener1", "file", ""));

            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            provider = new LoggingSettingsManageabilityProvider(subProviders);

            MockAdmContentBuilder contentBuilder = new MockAdmContentBuilder();

            provider.AddAdministrativeTemplateDirectives(contentBuilder, section, configurationSource, "TestApp");

            MockAdmContent content = contentBuilder.GetMockContent();
            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            categoriesEnumerator.MoveNext();
            IEnumerator<AdmCategory> subCategoriesEnumerator = categoriesEnumerator.Current.Categories.GetEnumerator();
            subCategoriesEnumerator.MoveNext(); // first is trace sources category
            subCategoriesEnumerator.MoveNext(); // second is special sources category
            IEnumerator<AdmPolicy> specialSourcePoliciesEnumerator = subCategoriesEnumerator.Current.Policies.GetEnumerator();
            Assert.IsTrue(specialSourcePoliciesEnumerator.MoveNext());
            IEnumerator<AdmPart> sourcePartsEnumerator = specialSourcePoliciesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(sourcePartsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), sourcePartsEnumerator.Current.GetType());
            Assert.AreEqual(LoggingSettingsManageabilityProvider.SourceDefaultLevelPropertyName,
                            sourcePartsEnumerator.Current.ValueName);
            Assert.IsTrue(sourcePartsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmTextPart), sourcePartsEnumerator.Current.GetType());
            Assert.IsTrue(sourcePartsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmCheckboxPart), sourcePartsEnumerator.Current.GetType());
            Assert.AreEqual("listener1", sourcePartsEnumerator.Current.ValueName);
            Assert.AreEqual("listener1", sourcePartsEnumerator.Current.PartName);
            Assert.IsTrue(sourcePartsEnumerator.Current.KeyName.EndsWith(LoggingSettingsManageabilityProvider.SourceTraceListenersPropertyName));
            Assert.IsFalse(sourcePartsEnumerator.MoveNext());
            Assert.IsTrue(specialSourcePoliciesEnumerator.MoveNext());
            Assert.IsTrue(specialSourcePoliciesEnumerator.MoveNext()); // there are 3 special sources
            Assert.IsFalse(specialSourcePoliciesEnumerator.MoveNext());
        }

        [TestMethod]
        public void ManageabilityProviderInvokesRegisteredElementProvidersWhileGeneratingAdmContent()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(LoggingSettings.SectionName, section);

            section.LogFilters.Add(new LogFilterData("filter", typeof(object)));
            section.Formatters.Add(new FormatterData("formatter", typeof(object)));
            section.TraceListeners.Add(new FlatFileTraceListenerData("listener", "file", "formatter"));

            MockConfigurationElementManageabilityProvider subProvider = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(LogFilterData), subProvider);
            subProviders.Add(typeof(FormatterData), subProvider);
            subProviders.Add(typeof(FlatFileTraceListenerData), subProvider);
            provider = new LoggingSettingsManageabilityProvider(subProviders);

            MockAdmContentBuilder contentBuilder = new MockAdmContentBuilder();

            provider.AddAdministrativeTemplateDirectives(contentBuilder, section, configurationSource, "TestApp");

            Assert.AreEqual(3, subProvider.configurationObjects.Count);
            Assert.AreSame(typeof(LogFilterData), subProvider.configurationObjects[0].GetType());
            Assert.AreSame(typeof(FormatterData), subProvider.configurationObjects[1].GetType());
            Assert.AreSame(typeof(FlatFileTraceListenerData), subProvider.configurationObjects[2].GetType());
        }
    }
}
