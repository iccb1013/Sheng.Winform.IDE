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
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Manageability.Tests
{
    [TestClass]
    public class DatabaseSettingsManageabilityProviderFixture
    {
        DatabaseSettingsManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        IList<ConfigurationSetting> wmiSettings;
        DatabaseSettings section;

        [TestInitialize]
        public void SetUp()
        {
            provider = new DatabaseSettingsManageabilityProvider(new Dictionary<Type, ConfigurationElementManageabilityProvider>(0));
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            wmiSettings = new List<ConfigurationSetting>();
            section = new DatabaseSettings();
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
            section.DefaultDatabase = "default";

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, false, wmiSettings);

            Assert.AreEqual("default", section.DefaultDatabase);
        }

        [TestMethod]
        public void SectionWithNullPolicyOverridesIsNotModified()
        {
            section.DefaultDatabase = "default";

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, null, null, false, wmiSettings);

            Assert.AreEqual("default", section.DefaultDatabase);
        }

        [TestMethod]
        public void SectionWithMachinePolicyOverridesIsModified()
        {
            section.DefaultDatabase = "default";

            machineKey.AddBooleanValue(DatabaseSettingsManageabilityProvider.PolicyValueName, true);
            machineKey.AddStringValue(DatabaseSettingsManageabilityProvider.DefaultDatabasePropertyName, "overridenDefault");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, false, wmiSettings);

            Assert.AreEqual("overridenDefault", section.DefaultDatabase);
        }

        [TestMethod]
        public void SectionWitUserPolicyOverridesIsModified()
        {
            section.DefaultDatabase = "default";

            userKey.AddBooleanValue(DatabaseSettingsManageabilityProvider.PolicyValueName, true);
            userKey.AddStringValue(DatabaseSettingsManageabilityProvider.DefaultDatabasePropertyName, "overridenDefault");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, false, wmiSettings);

            Assert.AreEqual("overridenDefault", section.DefaultDatabase);
        }

        [TestMethod]
        public void MachinePolicyOverrideTakesPrecedenceOverUserPolicyOverride()
        {
            section.DefaultDatabase = "default";

            machineKey.AddBooleanValue(DatabaseSettingsManageabilityProvider.PolicyValueName, true);
            machineKey.AddStringValue(DatabaseSettingsManageabilityProvider.DefaultDatabasePropertyName, "machineOverridenDefault");
            userKey.AddBooleanValue(DatabaseSettingsManageabilityProvider.PolicyValueName, true);
            userKey.AddStringValue(DatabaseSettingsManageabilityProvider.DefaultDatabasePropertyName, "userOverridenDefault");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, false, wmiSettings);

            Assert.AreEqual("machineOverridenDefault", section.DefaultDatabase);
        }

        [TestMethod]
        public void OverridesAreIgnoredIfGroupPoliciesAreDisabled()
        {
            section.DefaultDatabase = "default";

            userKey.AddBooleanValue(DatabaseSettingsManageabilityProvider.PolicyValueName, true);
            userKey.AddStringValue(DatabaseSettingsManageabilityProvider.DefaultDatabasePropertyName, "overridenDefault");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, false, wmiSettings);

            Assert.AreEqual("default", section.DefaultDatabase);
        }

        [TestMethod]
        public void ProviderMappingWithoutOverridesIsNotModified()
        {
            DbProviderMapping providerMapping = new DbProviderMapping("provider1", typeof(SqlDatabase));
            section.ProviderMappings.Add(providerMapping);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, false, wmiSettings);

            Assert.AreSame(typeof(SqlDatabase), providerMapping.DatabaseType);
        }

        [TestMethod]
        public void ProviderMappingWithMachineOverridesIsModified()
        {
            DbProviderMapping providerMapping = new DbProviderMapping("provider1", typeof(SqlDatabase));
            section.ProviderMappings.Add(providerMapping);

            MockRegistryKey providerMappingKey = new MockRegistryKey(false);
            providerMappingKey.AddStringValue(DatabaseSettingsManageabilityProvider.DatabaseTypePropertyName,
                                              typeof(OracleDatabase).AssemblyQualifiedName);
            MockRegistryKey providerMappingsKey = new MockRegistryKey(false);
            providerMappingsKey.AddSubKey("provider1",
                                          providerMappingKey);
            machineKey.AddSubKey(DatabaseSettingsManageabilityProvider.ProviderMappingsKeyName,
                                 providerMappingsKey);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, false, wmiSettings);

            Assert.AreSame(typeof(OracleDatabase), providerMapping.DatabaseType);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(providerMappingKey, providerMappingsKey));
        }

        [TestMethod]
        public void ProviderMappingWithUserOverridesIsModified()
        {
            DbProviderMapping providerMapping = new DbProviderMapping("provider1", typeof(SqlDatabase));
            section.ProviderMappings.Add(providerMapping);

            MockRegistryKey providerMappingKey = new MockRegistryKey(false);
            providerMappingKey.AddStringValue(DatabaseSettingsManageabilityProvider.DatabaseTypePropertyName,
                                              typeof(OracleDatabase).AssemblyQualifiedName);
            MockRegistryKey providerMappingsKey = new MockRegistryKey(false);
            providerMappingsKey.AddSubKey("provider1",
                                          providerMappingKey);
            userKey.AddSubKey(DatabaseSettingsManageabilityProvider.ProviderMappingsKeyName,
                              providerMappingsKey);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, false, wmiSettings);

            Assert.AreSame(typeof(OracleDatabase), providerMapping.DatabaseType);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(providerMappingKey, providerMappingsKey));
        }

        [TestMethod]
        public void MachineOverrideTakesPrecedenceOverUserOverride()
        {
            DbProviderMapping providerMapping = new DbProviderMapping("provider1", typeof(SqlDatabase));
            section.ProviderMappings.Add(providerMapping);

            MockRegistryKey providerMappingUserKey = new MockRegistryKey(false);
            providerMappingUserKey.AddStringValue(DatabaseSettingsManageabilityProvider.DatabaseTypePropertyName,
                                                  typeof(GenericDatabase).AssemblyQualifiedName);
            MockRegistryKey providerMappingsUserKey = new MockRegistryKey(false);
            providerMappingsUserKey.AddSubKey("provider1",
                                              providerMappingUserKey);
            userKey.AddSubKey(DatabaseSettingsManageabilityProvider.ProviderMappingsKeyName,
                              providerMappingsUserKey);

            MockRegistryKey providerMappingMachineKey = new MockRegistryKey(false);
            providerMappingMachineKey.AddStringValue(DatabaseSettingsManageabilityProvider.DatabaseTypePropertyName,
                                                     typeof(OracleDatabase).AssemblyQualifiedName);
            MockRegistryKey providerMappingsMachineKey = new MockRegistryKey(false);
            providerMappingsMachineKey.AddSubKey("provider1",
                                                 providerMappingMachineKey);
            machineKey.AddSubKey(DatabaseSettingsManageabilityProvider.ProviderMappingsKeyName,
                                 providerMappingsMachineKey);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, false, wmiSettings);

            Assert.AreSame(typeof(OracleDatabase), providerMapping.DatabaseType);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(providerMappingUserKey, providerMappingsUserKey, providerMappingMachineKey, providerMappingsMachineKey));
        }

        [TestMethod]
        public void OverrideForMissingProviderIsIgnored()
        {
            DbProviderMapping providerMapping = new DbProviderMapping("provider1", typeof(SqlDatabase));
            section.ProviderMappings.Add(providerMapping);

            MockRegistryKey providerMappingKey = new MockRegistryKey(false);
            providerMappingKey.AddStringValue(DatabaseSettingsManageabilityProvider.DatabaseTypePropertyName,
                                              typeof(OracleDatabase).AssemblyQualifiedName);
            MockRegistryKey providerMappingsKey = new MockRegistryKey(false);
            providerMappingsKey.AddSubKey("provider2",
                                          providerMappingKey);
            userKey.AddSubKey(DatabaseSettingsManageabilityProvider.ProviderMappingsKeyName,
                              providerMappingsKey);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, false, wmiSettings);

            Assert.AreSame(typeof(SqlDatabase), providerMapping.DatabaseType);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(providerMappingKey, providerMappingsKey));
        }

        [TestMethod]
        public void OverridesForProviderMappingsAreIndependent()
        {
            DbProviderMapping providerMapping1 = new DbProviderMapping("provider1", typeof(OracleDatabase));
            DbProviderMapping providerMapping2 = new DbProviderMapping("provider2", typeof(SqlDatabase));
            section.ProviderMappings.Add(providerMapping1);
            section.ProviderMappings.Add(providerMapping2);

            MockRegistryKey providerMappingUserKey = new MockRegistryKey(false);
            providerMappingUserKey.AddStringValue(DatabaseSettingsManageabilityProvider.DatabaseTypePropertyName,
                                                  typeof(GenericDatabase).AssemblyQualifiedName);
            MockRegistryKey providerMappingsUserKey = new MockRegistryKey(false);
            providerMappingsUserKey.AddSubKey("provider1",
                                              providerMappingUserKey);
            userKey.AddSubKey(DatabaseSettingsManageabilityProvider.ProviderMappingsKeyName,
                              providerMappingsUserKey);

            MockRegistryKey providerMappingMachineKey = new MockRegistryKey(false);
            providerMappingMachineKey.AddStringValue(DatabaseSettingsManageabilityProvider.DatabaseTypePropertyName,
                                                     typeof(OracleDatabase).AssemblyQualifiedName);
            MockRegistryKey providerMappingsMachineKey = new MockRegistryKey(false);
            providerMappingsMachineKey.AddSubKey("provider2",
                                                 providerMappingMachineKey);
            machineKey.AddSubKey(DatabaseSettingsManageabilityProvider.ProviderMappingsKeyName,
                                 providerMappingsMachineKey);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, false, wmiSettings);

            Assert.AreSame(typeof(GenericDatabase), providerMapping1.DatabaseType);
            Assert.AreSame(typeof(OracleDatabase), providerMapping2.DatabaseType);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(providerMappingUserKey, providerMappingsUserKey, providerMappingMachineKey, providerMappingsMachineKey));
        }

        [TestMethod]
        public void DatabaseTypeOverrideWithInvalidTypeNameIsIgnored()
        {
            DbProviderMapping providerMapping = new DbProviderMapping("provider1", typeof(SqlDatabase));
            section.ProviderMappings.Add(providerMapping);

            MockRegistryKey providerMappingsMachineKey = new MockRegistryKey(false);
            machineKey.AddSubKey(DatabaseSettingsManageabilityProvider.ProviderMappingsKeyName,
                                 providerMappingsMachineKey);
            MockRegistryKey providerMappingMachineKey = new MockRegistryKey(false);
            providerMappingsMachineKey.AddSubKey("provider1",
                                                 providerMappingMachineKey);
            providerMappingMachineKey.AddStringValue(DatabaseSettingsManageabilityProvider.DatabaseTypePropertyName,
                                                     "invalid type name");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, false, wmiSettings);

            Assert.AreSame(typeof(SqlDatabase), providerMapping.DatabaseType);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(providerMappingMachineKey, providerMappingsMachineKey));
        }

        [TestMethod]
        public void ProviderMappingWithDisabledPolicyIsRemoved()
        {
            DbProviderMapping providerMapping1 = new DbProviderMapping("provider1", typeof(SqlDatabase));
            section.ProviderMappings.Add(providerMapping1);
            DbProviderMapping providerMapping2 = new DbProviderMapping("provider2", typeof(SqlDatabase));
            section.ProviderMappings.Add(providerMapping2);

            MockRegistryKey providerMappingsMachineKey = new MockRegistryKey(false);
            machineKey.AddSubKey(DatabaseSettingsManageabilityProvider.ProviderMappingsKeyName,
                                 providerMappingsMachineKey);
            MockRegistryKey providerMapping1MachineKey = new MockRegistryKey(false);
            providerMappingsMachineKey.AddSubKey("provider1",
                                                 providerMapping1MachineKey);
            providerMapping1MachineKey.AddBooleanValue(DatabaseSettingsManageabilityProvider.PolicyValueName, false);
            MockRegistryKey providerMapping2MachineKey = new MockRegistryKey(false);
            providerMappingsMachineKey.AddSubKey("provider2",
                                                 providerMapping2MachineKey);
            providerMapping2MachineKey.AddBooleanValue(DatabaseSettingsManageabilityProvider.PolicyValueName, true);
            providerMapping2MachineKey.AddStringValue(DatabaseSettingsManageabilityProvider.DatabaseTypePropertyName,
                                                      typeof(GenericDatabase).AssemblyQualifiedName);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, section.ProviderMappings.Count);
            Assert.IsNotNull(section.ProviderMappings.Get("provider2"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(providerMapping1MachineKey, providerMapping2MachineKey, providerMappingsMachineKey));
        }

        [TestMethod]
        public void ProviderMappingWithDisabledPolicyIsNotRemovedIfGroupPoliciesAreDisabled()
        {
            DbProviderMapping providerMapping1 = new DbProviderMapping("provider1", typeof(SqlDatabase));
            section.ProviderMappings.Add(providerMapping1);
            DbProviderMapping providerMapping2 = new DbProviderMapping("provider2", typeof(SqlDatabase));
            section.ProviderMappings.Add(providerMapping2);

            MockRegistryKey providerMappingsMachineKey = new MockRegistryKey(false);
            machineKey.AddSubKey(DatabaseSettingsManageabilityProvider.ProviderMappingsKeyName,
                                 providerMappingsMachineKey);
            MockRegistryKey providerMapping1MachineKey = new MockRegistryKey(false);
            providerMappingsMachineKey.AddSubKey("provider1",
                                                 providerMapping1MachineKey);
            providerMapping1MachineKey.AddBooleanValue(AdmContentBuilder.AvailableValueName, false);
            MockRegistryKey providerMapping2MachineKey = new MockRegistryKey(false);
            providerMappingsMachineKey.AddSubKey("provider2",
                                                 providerMapping2MachineKey);
            providerMapping2MachineKey.AddBooleanValue(AdmContentBuilder.AvailableValueName, true);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(2, section.ProviderMappings.Count);
            Assert.IsNotNull(section.ProviderMappings.Get("provider1"));
            Assert.IsNotNull(section.ProviderMappings.Get("provider2"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(providerMapping1MachineKey, providerMapping2MachineKey, providerMappingsMachineKey));
        }

        [TestMethod]
        public void SettingsAreNotCreatedWhenWmiIsDisabled()
        {
            section.DefaultDatabase = "default";
            section.ProviderMappings.Add(new DbProviderMapping("test", typeof(SqlDatabase)));

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, false, wmiSettings);

            Assert.AreEqual(0, wmiSettings.Count);
        }

        [TestMethod]
        public void SettingsAreCreatedWhenWmiIsEnabled()
        {
            section.DefaultDatabase = "default";
            section.ProviderMappings.Add(new DbProviderMapping("test", typeof(SqlDatabase)));

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(2, wmiSettings.Count);
            Assert.AreSame(typeof(DatabaseBlockSetting), wmiSettings[0].GetType());
            Assert.IsNull(wmiSettings[0].ApplicationName);
            Assert.IsNull(wmiSettings[0].SectionName);
            Assert.AreEqual("default", ((DatabaseBlockSetting)wmiSettings[0]).DefaultDatabase);
            Assert.AreSame(typeof(ProviderMappingSetting), wmiSettings[1].GetType());
            Assert.AreEqual("test", ((ProviderMappingSetting)wmiSettings[1]).Name);
            Assert.AreEqual(typeof(SqlDatabase).AssemblyQualifiedName,
                            ((ProviderMappingSetting)wmiSettings[1]).DatabaseType);
        }

        [TestMethod]
        public void ManageabilityProviderIsProperlyRegistered()
        {
            ConfigurationSectionManageabilityProviderAttribute selectedAttribute = null;

            Assembly assembly = typeof(DatabaseSettingsManageabilityProvider).Assembly;
            foreach (ConfigurationSectionManageabilityProviderAttribute providerAttribute in assembly.GetCustomAttributes(typeof(ConfigurationSectionManageabilityProviderAttribute), false))
            {
                if (providerAttribute.SectionName.Equals(DatabaseSettings.SectionName))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(DatabaseSettingsManageabilityProvider), selectedAttribute.ManageabilityProviderType);
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContent()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            ConnectionStringsSection connectionStringsSection = new ConnectionStringsSection();
            connectionStringsSection.ConnectionStrings.Add(
                new ConnectionStringSettings("database1", "connection1", "system.provider1"));
            connectionStringsSection.ConnectionStrings.Add(
                new ConnectionStringSettings("database2", "connection2", "system.provider2"));
            configurationSource.Add("connectionStrings", connectionStringsSection);

            section.DefaultDatabase = "database1";
            section.ProviderMappings.Add(new DbProviderMapping("system.provider1", typeof(SqlDatabase)));

            MockAdmContentBuilder contentBuilder = new MockAdmContentBuilder();

            provider.AddAdministrativeTemplateDirectives(contentBuilder, section, configurationSource, "TestApp");

            MockAdmContent content = contentBuilder.GetMockContent();
            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            Assert.IsTrue(categoriesEnumerator.MoveNext());
            IEnumerator<AdmCategory> subCategoriesEnumerator = categoriesEnumerator.Current.Categories.GetEnumerator();
            Assert.IsTrue(subCategoriesEnumerator.MoveNext());
            IEnumerator<AdmPolicy> policiesEnumerator = subCategoriesEnumerator.Current.Policies.GetEnumerator();

            Assert.IsTrue(policiesEnumerator.MoveNext());
            IEnumerator<AdmPart> providerPartsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();

            Assert.IsTrue(providerPartsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmComboBoxPart), providerPartsEnumerator.Current.GetType());
            Assert.IsNull(providerPartsEnumerator.Current.KeyName);
            Assert.AreEqual(DatabaseSettingsManageabilityProvider.DatabaseTypePropertyName,
                            providerPartsEnumerator.Current.ValueName);
            Assert.IsFalse(providerPartsEnumerator.MoveNext());
            Assert.IsFalse(policiesEnumerator.MoveNext());

            Assert.IsFalse(subCategoriesEnumerator.MoveNext());
            IEnumerator<AdmPolicy> sectionPoliciesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            Assert.IsTrue(sectionPoliciesEnumerator.MoveNext());
            IEnumerator<AdmPart> sectionPartsEnumerator = sectionPoliciesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(sectionPartsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), sectionPartsEnumerator.Current.GetType());
            Assert.IsNull(sectionPartsEnumerator.Current.KeyName);
            Assert.AreEqual(DatabaseSettingsManageabilityProvider.DefaultDatabasePropertyName,
                            sectionPartsEnumerator.Current.ValueName);
            Assert.IsFalse(sectionPartsEnumerator.MoveNext());

            Assert.IsFalse(sectionPoliciesEnumerator.MoveNext());
            Assert.IsFalse(categoriesEnumerator.MoveNext());
        }
    }
}
