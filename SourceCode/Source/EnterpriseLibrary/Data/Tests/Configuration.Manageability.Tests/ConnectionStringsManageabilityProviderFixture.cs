//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Manageability.Tests
{
    [TestClass]
    public class ConnectionStringsManageabilityProviderFixture
    {
        ConnectionStringsManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        IList<ConfigurationSetting> wmiSettings;
        ConnectionStringsSection section;

        [TestInitialize]
        public void SetUp()
        {
            provider = new ConnectionStringsManageabilityProvider(new Dictionary<Type, ConfigurationElementManageabilityProvider>(0));
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            wmiSettings = new List<ConfigurationSetting>();
            section = new ConnectionStringsSection();
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
        public void EmptySectionIsIgnored()
        {
            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);
            Assert.AreEqual(0, wmiSettings.Count);
        }

        [TestMethod]
        public void ConnectionStringWithoutPolicyOverridesIsNotModified()
        {
            ConnectionStringSettings connectionString = new ConnectionStringSettings("cs1", "connectionString", "providerName");
            section.ConnectionStrings.Add(connectionString);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, false, wmiSettings);

            Assert.AreEqual("connectionString", connectionString.ConnectionString);
            Assert.AreEqual("providerName", connectionString.ProviderName);
        }

        [TestMethod]
        public void ConnectionStringWithNullRegistryKeysOverridesIsNotModified()
        {
            ConnectionStringSettings connectionString = new ConnectionStringSettings("cs1", "connectionString", "providerName");
            section.ConnectionStrings.Add(connectionString);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, null, null, false, wmiSettings);

            Assert.AreEqual("connectionString", connectionString.ConnectionString);
            Assert.AreEqual("providerName", connectionString.ProviderName);
        }

        [TestMethod]
        public void ConnectionStringWithPolicyOverridesForOtherNameIsNotModified()
        {
            ConnectionStringSettings connectionString = new ConnectionStringSettings("cs1", "connectionString", "providerName");
            section.ConnectionStrings.Add(connectionString);

            MockRegistryKey overrideKey = new MockRegistryKey(false);
            overrideKey.AddStringValue(ConnectionStringsManageabilityProvider.ConnectionStringPropertyName, "overridenConnectionString");
            overrideKey.AddStringValue(ConnectionStringsManageabilityProvider.ProviderNamePropertyName, "overridenProviderName");
            machineKey.AddSubKey("cs2", overrideKey);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, false, wmiSettings);

            Assert.AreEqual("connectionString", connectionString.ConnectionString);
            Assert.AreEqual("providerName", connectionString.ProviderName);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(overrideKey));
        }

        // machine key overrides
        [TestMethod]
        public void ConnectionStringWithMachinePolicyOverrideIsModified()
        {
            ConnectionStringSettings connectionString = new ConnectionStringSettings("cs1", "connectionString", "providerName");
            section.ConnectionStrings.Add(connectionString);

            MockRegistryKey overrideKey = new MockRegistryKey(false);
            overrideKey.AddStringValue(ConnectionStringsManageabilityProvider.ConnectionStringPropertyName, "overridenConnectionString");
            overrideKey.AddStringValue(ConnectionStringsManageabilityProvider.ProviderNamePropertyName, "overridenProviderName");
            machineKey.AddSubKey("cs1", overrideKey);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, false, wmiSettings);

            Assert.AreEqual("overridenConnectionString", connectionString.ConnectionString);
            Assert.AreEqual("overridenProviderName", connectionString.ProviderName);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(overrideKey));
        }

        [TestMethod]
        public void ConnectionStringWithMachinePolicyOverrideIsNotModifiedIfGroupPoliciesAreDisabled()
        {
            ConnectionStringSettings connectionString = new ConnectionStringSettings("cs1", "connectionString", "providerName");
            section.ConnectionStrings.Add(connectionString);

            MockRegistryKey overrideKey = new MockRegistryKey(false);
            overrideKey.AddStringValue(ConnectionStringsManageabilityProvider.ConnectionStringPropertyName, "overridenConnectionString");
            overrideKey.AddStringValue(ConnectionStringsManageabilityProvider.ProviderNamePropertyName, "overridenProviderName");
            machineKey.AddSubKey("cs1", overrideKey);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, false, wmiSettings);

            Assert.AreEqual("connectionString", connectionString.ConnectionString);
            Assert.AreEqual("providerName", connectionString.ProviderName);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(overrideKey));
        }

        // user key overrides
        [TestMethod]
        public void ConnectionStringWithUserPolicyOverrideIsModified()
        {
            ConnectionStringSettings connectionString = new ConnectionStringSettings("cs1", "connectionString", "providerName");
            section.ConnectionStrings.Add(connectionString);

            MockRegistryKey overrideKey = new MockRegistryKey(false);
            overrideKey.AddStringValue(ConnectionStringsManageabilityProvider.ConnectionStringPropertyName, "overridenConnectionString");
            overrideKey.AddStringValue(ConnectionStringsManageabilityProvider.ProviderNamePropertyName, "overridenProviderName");
            userKey.AddSubKey("cs1", overrideKey);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, false, wmiSettings);

            Assert.AreEqual("overridenConnectionString", connectionString.ConnectionString);
            Assert.AreEqual("overridenProviderName", connectionString.ProviderName);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(overrideKey));
        }

        [TestMethod]
        public void ConnectionStringWithUserPolicyOverrideIsNotModifiedIfGroupPoliciesAreDisabled()
        {
            ConnectionStringSettings connectionString = new ConnectionStringSettings("cs1", "connectionString", "providerName");
            section.ConnectionStrings.Add(connectionString);

            MockRegistryKey overrideKey = new MockRegistryKey(false);
            overrideKey.AddStringValue(ConnectionStringsManageabilityProvider.ConnectionStringPropertyName, "overridenConnectionString");
            overrideKey.AddStringValue(ConnectionStringsManageabilityProvider.ProviderNamePropertyName, "overridenProviderName");
            userKey.AddSubKey("cs1", overrideKey);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, false, wmiSettings);

            Assert.AreEqual("connectionString", connectionString.ConnectionString);
            Assert.AreEqual("providerName", connectionString.ProviderName);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(overrideKey));
        }

        // mixed key overrides
        [TestMethod]
        public void ConnectionStringMachinePolicyOverrideTakesPrecedenceOverUserPolicyOverride()
        {
            ConnectionStringSettings connectionString = new ConnectionStringSettings("cs1", "connectionString", "providerName");
            section.ConnectionStrings.Add(connectionString);

            MockRegistryKey machineOverrideKey = new MockRegistryKey(false);
            machineOverrideKey.AddStringValue(ConnectionStringsManageabilityProvider.ConnectionStringPropertyName, "machineOverridenConnectionString");
            machineOverrideKey.AddStringValue(ConnectionStringsManageabilityProvider.ProviderNamePropertyName, "machineOverridenProviderName");
            machineKey.AddSubKey("cs1", machineOverrideKey);
            MockRegistryKey userOverrideKey = new MockRegistryKey(false);
            userOverrideKey.AddStringValue(ConnectionStringsManageabilityProvider.ConnectionStringPropertyName, "userOverridenConnectionString");
            userOverrideKey.AddStringValue(ConnectionStringsManageabilityProvider.ProviderNamePropertyName, "userOverridenProviderName");
            userKey.AddSubKey("cs1", userOverrideKey);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, false, wmiSettings);

            Assert.AreEqual("machineOverridenConnectionString", connectionString.ConnectionString);
            Assert.AreEqual("machineOverridenProviderName", connectionString.ProviderName);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineOverrideKey, userOverrideKey));
        }

        [TestMethod]
        public void ConnectionStringWithDisabledPolicyIsRemoved()
        {
            ConnectionStringSettings connectionString1 = new ConnectionStringSettings("cs1", "connectionString", "providerName");
            section.ConnectionStrings.Add(connectionString1);
            ConnectionStringSettings connectionString2 = new ConnectionStringSettings("cs2", "connectionString", "providerName");
            section.ConnectionStrings.Add(connectionString2);

            MockRegistryKey machineConnectionString1Key = new MockRegistryKey(false);
            machineKey.AddSubKey("cs1", machineConnectionString1Key);
            machineConnectionString1Key.AddBooleanValue(AdmContentBuilder.AvailableValueName, false);
            MockRegistryKey machineConnectionString2Key = new MockRegistryKey(false);
            machineKey.AddSubKey("cs2", machineConnectionString2Key);
            machineConnectionString2Key.AddBooleanValue(AdmContentBuilder.AvailableValueName, true);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, section.ConnectionStrings.Count);
            Assert.IsNotNull(section.ConnectionStrings["cs2"]);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineConnectionString1Key, machineConnectionString2Key));
        }

        [TestMethod]
        public void LogFormatterWithDisabledPolicyIsNotRemovedIfGroupPoliciesAreDisabled()
        {
            ConnectionStringSettings connectionString1 = new ConnectionStringSettings("cs1", "connectionString", "providerName");
            section.ConnectionStrings.Add(connectionString1);
            ConnectionStringSettings connectionString2 = new ConnectionStringSettings("cs2", "connectionString", "providerName");
            section.ConnectionStrings.Add(connectionString2);

            MockRegistryKey machineConnectionString1Key = new MockRegistryKey(false);
            machineKey.AddSubKey("cs1", machineConnectionString1Key);
            machineConnectionString1Key.AddBooleanValue(AdmContentBuilder.AvailableValueName, false);
            MockRegistryKey machineConnectionString2Key = new MockRegistryKey(false);
            machineKey.AddSubKey("cs2", machineConnectionString2Key);
            machineConnectionString2Key.AddBooleanValue(AdmContentBuilder.AvailableValueName, true);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(2, section.ConnectionStrings.Count);
            Assert.IsNotNull(section.ConnectionStrings["cs1"]);
            Assert.IsNotNull(section.ConnectionStrings["cs2"]);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineConnectionString1Key, machineConnectionString2Key));
        }

        // wmi
        [TestMethod]
        public void SettingsAreNotCreatedWhenWmiIsDisabled()
        {
            ConnectionStringSettings connectionString = new ConnectionStringSettings("cs1", "connectionString", "providerName");
            section.ConnectionStrings.Add(connectionString);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, false, wmiSettings);

            Assert.AreEqual(0, wmiSettings.Count);
        }

        [TestMethod]
        public void SettingsAreCreatedForSingleConnectionStringWhenWmiIsEnabled()
        {
            ConnectionStringSettings connectionString = new ConnectionStringSettings("cs1", "connectionString", "providerName");
            section.ConnectionStrings.Add(connectionString);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);

            ConnectionStringSetting setting = GetSetting(wmiSettings, "cs1");
            Assert.IsNotNull(setting);
            Assert.IsNull(setting.ApplicationName);
            Assert.IsNull(setting.SectionName);
            Assert.AreEqual("connectionString", setting.ConnectionString);
            Assert.AreEqual("providerName", setting.ProviderName);
        }

        [TestMethod]
        public void SettingsAreCreatedForManyConnectionStringsWhenWmiIsEnabled()
        {
            ConnectionStringSettings connectionString = new ConnectionStringSettings("cs1", "connectionString", "providerName");
            section.ConnectionStrings.Add(connectionString);
            connectionString = new ConnectionStringSettings("cs2", "connectionString2", "providerName2");
            section.ConnectionStrings.Add(connectionString);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, true, wmiSettings);

            ConnectionStringSetting setting = GetSetting(wmiSettings, "cs1");
            Assert.IsNotNull(setting);
            Assert.IsNull(setting.ApplicationName);
            Assert.IsNull(setting.SectionName);
            Assert.AreEqual("connectionString", setting.ConnectionString);
            Assert.AreEqual("providerName", setting.ProviderName);
            setting = GetSetting(wmiSettings, "cs2");
            Assert.IsNotNull(setting);
            Assert.IsNull(setting.ApplicationName);
            Assert.IsNull(setting.SectionName);
            Assert.AreEqual("connectionString2", setting.ConnectionString);
            Assert.AreEqual("providerName2", setting.ProviderName);
        }

        [TestMethod]
        public void ManageabilityProviderIsProperlyRegistered()
        {
            ConfigurationSectionManageabilityProviderAttribute selectedAttribute = null;

            Assembly assembly = typeof(ConnectionStringsManageabilityProvider).Assembly;
            foreach (ConfigurationSectionManageabilityProviderAttribute providerAttribute in assembly.GetCustomAttributes(typeof(ConfigurationSectionManageabilityProviderAttribute), false))
            {
                if (providerAttribute.SectionName.Equals("connectionStrings"))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(ConnectionStringsManageabilityProvider), selectedAttribute.ManageabilityProviderType);
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContent()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add("connectionStrings", section);

            section.ConnectionStrings.Add(new ConnectionStringSettings("cs1", "cs1"));
            section.ConnectionStrings.Add(new ConnectionStringSettings("cs2", "cs2"));

            MockAdmContentBuilder contentBuilder = new MockAdmContentBuilder();

            provider.AddAdministrativeTemplateDirectives(contentBuilder, section, configurationSource, "TestApp");

            MockAdmContent content = contentBuilder.GetMockContent();
            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            Assert.IsTrue(categoriesEnumerator.MoveNext());
            IEnumerator<AdmCategory> subCategoriesEnumerator = categoriesEnumerator.Current.Categories.GetEnumerator();
            Assert.IsTrue(subCategoriesEnumerator.MoveNext());
            IEnumerator<AdmPolicy> policiesEnumerator = subCategoriesEnumerator.Current.Policies.GetEnumerator();

            Assert.IsTrue(policiesEnumerator.MoveNext());
            IEnumerator<AdmPart> connectionStringPartsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();

            Assert.IsTrue(connectionStringPartsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), connectionStringPartsEnumerator.Current.GetType());
            Assert.IsNull(connectionStringPartsEnumerator.Current.KeyName);
            Assert.AreEqual(ConnectionStringsManageabilityProvider.ConnectionStringPropertyName,
                            connectionStringPartsEnumerator.Current.ValueName);

            Assert.IsTrue(connectionStringPartsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmComboBoxPart), connectionStringPartsEnumerator.Current.GetType());
            Assert.IsNull(connectionStringPartsEnumerator.Current.KeyName);
            Assert.AreEqual(ConnectionStringsManageabilityProvider.ProviderNamePropertyName,
                            connectionStringPartsEnumerator.Current.ValueName);

            Assert.IsFalse(connectionStringPartsEnumerator.MoveNext());
            Assert.IsTrue(policiesEnumerator.MoveNext());
            Assert.IsFalse(policiesEnumerator.MoveNext()); // 2 connection strings -> 2 policies

            Assert.IsFalse(subCategoriesEnumerator.MoveNext());
            IEnumerator<AdmPolicy> sectionPoliciesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            Assert.IsFalse(sectionPoliciesEnumerator.MoveNext());
            Assert.IsFalse(categoriesEnumerator.MoveNext());
        }

        ConnectionStringSetting GetSetting(IEnumerable<ConfigurationSetting> wmiSettings,
                                           String name)
        {
            foreach (ConfigurationSetting setting in wmiSettings)
            {
                ConnectionStringSetting connectionStringSetting = setting as ConnectionStringSetting;
                if (connectionStringSetting != null && connectionStringSetting.Name.Equals(name))
                    return connectionStringSetting;
            }

            return null;
        }
    }
}
