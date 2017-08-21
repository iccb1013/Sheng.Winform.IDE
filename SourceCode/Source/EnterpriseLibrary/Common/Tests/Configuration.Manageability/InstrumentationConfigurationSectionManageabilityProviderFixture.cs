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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests
{
    [TestClass]
    public class InstrumentationConfigurationSectionManageabilityProviderFixture
    {
        InstrumentationConfigurationSectionManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        IList<ConfigurationSetting> wmiSettings;
        InstrumentationConfigurationSection section;

        [TestInitialize]
        public void SetUp()
        {
            provider = new InstrumentationConfigurationSectionManageabilityProvider(new Dictionary<Type, ConfigurationElementManageabilityProvider>(0));
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            wmiSettings = new List<ConfigurationSetting>();
            section = new InstrumentationConfigurationSection();
        }

        [TestCleanup]
        public void TearDown()
        {
            // preventive unregister to work around WMI.NET 2.0 issues with appdomain unloading
            ManagementEntityTypesRegistrar.UnregisterAll();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ProviderThrowsWithConfigurationObjectOfWrongType()
        {
            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(new TestsConfigurationSection(), true, machineKey, userKey, true, wmiSettings);
        }

        [TestMethod]
        public void SectionWithoutPolicyOverridesIsNotModified()
        {
            section.EventLoggingEnabled = true;
            section.PerformanceCountersEnabled = true;
            section.WmiEnabled = true;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, null, null, false, wmiSettings);

            Assert.AreEqual(true, section.EventLoggingEnabled);
            Assert.AreEqual(true, section.PerformanceCountersEnabled);
            Assert.AreEqual(true, section.WmiEnabled);
        }

        [TestMethod]
        public void SectionWithNullRegistryKeysOverridesIsNotModified()
        {
            section.EventLoggingEnabled = true;
            section.PerformanceCountersEnabled = true;
            section.WmiEnabled = true;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, null, null, false, wmiSettings);

            Assert.AreEqual(true, section.EventLoggingEnabled);
            Assert.AreEqual(true, section.PerformanceCountersEnabled);
            Assert.AreEqual(true, section.WmiEnabled);
        }

        [TestMethod]
        public void SectionWithMachineOverrideForEventLoggingIsModified()
        {
            section.EventLoggingEnabled = true;
            section.PerformanceCountersEnabled = true;
            section.WmiEnabled = true;

            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.PolicyValueName, true);
            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.EventLoggingEnabledPropertyName, false);
            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.PerformanceCountersEnabledPropertyName, true);
            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.WmiEnabledPropertyName, true);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, null, false, wmiSettings);

            Assert.AreEqual(false, section.EventLoggingEnabled);
            Assert.AreEqual(true, section.PerformanceCountersEnabled);
            Assert.AreEqual(true, section.WmiEnabled);
        }

        [TestMethod]
        public void SectionWithMachineOverrideForPerformanceCountersIsModified()
        {
            section.EventLoggingEnabled = true;
            section.PerformanceCountersEnabled = true;
            section.WmiEnabled = true;

            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.PolicyValueName, true);
            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.EventLoggingEnabledPropertyName, true);
            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.PerformanceCountersEnabledPropertyName, false);
            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.WmiEnabledPropertyName, true);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, null, false, wmiSettings);

            Assert.AreEqual(true, section.EventLoggingEnabled);
            Assert.AreEqual(false, section.PerformanceCountersEnabled);
            Assert.AreEqual(true, section.WmiEnabled);
        }

        [TestMethod]
        public void SectionWithMachineOverrideForWmiIsModified()
        {
            section.EventLoggingEnabled = true;
            section.PerformanceCountersEnabled = true;
            section.WmiEnabled = true;

            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.PolicyValueName, true);
            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.EventLoggingEnabledPropertyName, true);
            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.PerformanceCountersEnabledPropertyName, true);
            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.WmiEnabledPropertyName, false);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, null, false, wmiSettings);

            Assert.AreEqual(true, section.EventLoggingEnabled);
            Assert.AreEqual(true, section.PerformanceCountersEnabled);
            Assert.AreEqual(false, section.WmiEnabled);
        }

        [TestMethod]
        public void MachinePolicyOverrideTakesPrecedenceOverUserPolicyOverride()
        {
            section.EventLoggingEnabled = true;
            section.PerformanceCountersEnabled = true;
            section.WmiEnabled = true;

            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.PolicyValueName, true);
            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.EventLoggingEnabledPropertyName, false);
            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.PerformanceCountersEnabledPropertyName, true);
            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.WmiEnabledPropertyName, false);
            userKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.PolicyValueName, true);
            userKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.EventLoggingEnabledPropertyName, true);
            userKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.PerformanceCountersEnabledPropertyName, false);
            userKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.WmiEnabledPropertyName, true);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, false, wmiSettings);

            Assert.AreEqual(false, section.EventLoggingEnabled);
            Assert.AreEqual(true, section.PerformanceCountersEnabled);
            Assert.AreEqual(false, section.WmiEnabled);
        }

        [TestMethod]
        public void OverridesAreIgnoredIfGroupPoliciesAreDisabled()
        {
            section.EventLoggingEnabled = true;
            section.PerformanceCountersEnabled = true;
            section.WmiEnabled = true;

            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.PolicyValueName, true);
            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.EventLoggingEnabledPropertyName, false);
            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.PerformanceCountersEnabledPropertyName, true);
            machineKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.WmiEnabledPropertyName, false);
            userKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.PolicyValueName, true);
            userKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.EventLoggingEnabledPropertyName, true);
            userKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.PerformanceCountersEnabledPropertyName, false);
            userKey.AddBooleanValue(InstrumentationConfigurationSectionManageabilityProvider.WmiEnabledPropertyName, true);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, false, wmiSettings);

            Assert.AreEqual(true, section.EventLoggingEnabled);
            Assert.AreEqual(true, section.PerformanceCountersEnabled);
            Assert.AreEqual(true, section.WmiEnabled);
        }

        [TestMethod]
        public void SettingsAreNotCreatedWhenWmiIsDisabled()
        {
            section.EventLoggingEnabled = true;
            section.PerformanceCountersEnabled = true;
            section.WmiEnabled = true;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, false, wmiSettings);

            Assert.AreEqual(0, wmiSettings.Count);
        }

        [TestMethod]
        public void SettingsAreCreatedWhenWmiIsEnabled()
        {
            section.EventLoggingEnabled = false;
            section.PerformanceCountersEnabled = true;
            section.WmiEnabled = false;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreEqual(false, section.EventLoggingEnabled);
            Assert.AreEqual(true, section.PerformanceCountersEnabled);
            Assert.AreEqual(false, section.WmiEnabled);
        }

        [TestMethod]
        public void ManageabilityProviderIsProperlyRegistered()
        {
            ConfigurationSectionManageabilityProviderAttribute selectedAttribute = null;

            Assembly assembly = typeof(InstrumentationConfigurationSectionManageabilityProvider).Assembly;
            foreach (ConfigurationSectionManageabilityProviderAttribute providerAttribute in assembly.GetCustomAttributes(typeof(ConfigurationSectionManageabilityProviderAttribute), false))
            {
                if (providerAttribute.SectionName.Equals(InstrumentationConfigurationSection.SectionName))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(InstrumentationConfigurationSectionManageabilityProvider), selectedAttribute.ManageabilityProviderType);
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContent()
        {
            IConfigurationSource configurationSource = new DictionaryConfigurationSource();

            section.EventLoggingEnabled = false;
            section.PerformanceCountersEnabled = true;
            section.WmiEnabled = false;

            MockAdmContentBuilder contentBuilder = new MockAdmContentBuilder();

            provider.AddAdministrativeTemplateDirectives(contentBuilder, section, configurationSource, "TestApp");
        }
    }
}
