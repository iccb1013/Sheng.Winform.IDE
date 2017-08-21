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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Manageability.Tests
{
    [TestClass]
    public class OracleConnectionSettingsManageabilityProviderFixture
    {
        ConfigurationSectionManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        IList<ConfigurationSetting> wmiSettings;
        OracleConnectionSettings section;

        [TestInitialize]
        public void SetUp()
        {
            provider = new OracleConnectionSettingsManageabilityProvider(new Dictionary<Type, ConfigurationElementManageabilityProvider>(0));
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            wmiSettings = new List<ConfigurationSetting>();
            section = new OracleConnectionSettings();
        }

        [TestCleanup]
        public void TearDown()
        {
            // preventive unregister to work around WMI.NET 2.0 issues with appdomain unloading
            ManagementEntityTypesRegistrar.UnregisterAll();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ProviderThrowsWithsectionOfWrongType()
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
        public void ManageabilityProviderIsProperlyRegistered()
        {
            ConfigurationSectionManageabilityProviderAttribute selectedAttribute = null;

            Assembly assembly = typeof(OracleConnectionSettingsManageabilityProvider).Assembly;
            foreach (ConfigurationSectionManageabilityProviderAttribute providerAttribute in assembly.GetCustomAttributes(typeof(ConfigurationSectionManageabilityProviderAttribute), false))
            {
                if (providerAttribute.SectionName.Equals(OracleConnectionSettings.SectionName))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(OracleConnectionSettingsManageabilityProvider), selectedAttribute.ManageabilityProviderType);
        }

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfThereAreNoPolicyOverrides()
        {
            OracleConnectionData connectionData = new OracleConnectionData();
            connectionData.Name = "data1";
            connectionData.Packages.Add(new OraclePackageData("package11", "prefix11"));
            connectionData.Packages.Add(new OraclePackageData("package12", "prefix12"));
            section.OracleConnectionsData.Add(connectionData);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(2, connectionData.Packages.Count);
            Assert.IsNotNull(connectionData.Packages.Get("package11"));
            Assert.AreEqual("prefix11", connectionData.Packages.Get("package11").Prefix);
            Assert.IsNotNull(connectionData.Packages.Get("package12"));
            Assert.AreEqual("prefix12", connectionData.Packages.Get("package12").Prefix);
        }

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfPolicyOverridesAreNull()
        {
            OracleConnectionData connectionData = new OracleConnectionData();
            connectionData.Name = "data1";
            connectionData.Packages.Add(new OraclePackageData("package11", "prefix11"));
            connectionData.Packages.Add(new OraclePackageData("package12", "prefix12"));
            section.OracleConnectionsData.Add(connectionData);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, null, null, true, wmiSettings);

            Assert.AreEqual(2, connectionData.Packages.Count);
            Assert.IsNotNull(connectionData.Packages.Get("package11"));
            Assert.AreEqual("prefix11", connectionData.Packages.Get("package11").Prefix);
            Assert.IsNotNull(connectionData.Packages.Get("package12"));
            Assert.AreEqual("prefix12", connectionData.Packages.Get("package12").Prefix);
        }

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfThereArePolicyOverridesForDifferentName()
        {
            OracleConnectionData connectionData = new OracleConnectionData();
            connectionData.Name = "data1";
            connectionData.Packages.Add(new OraclePackageData("package11", "prefix11"));
            connectionData.Packages.Add(new OraclePackageData("package12", "prefix12"));
            section.OracleConnectionsData.Add(connectionData);

            MockRegistryKey machinePackageKey = new MockRegistryKey(false);
            machineKey.AddSubKey("data2", machinePackageKey);
            machinePackageKey.AddStringValue(OracleConnectionSettingsManageabilityProvider.PackagesPropertyName,
                                             "package23=prefix23; package24=prefix24; package25=prefix25");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(2, connectionData.Packages.Count);
            Assert.IsNotNull(connectionData.Packages.Get("package11"));
            Assert.AreEqual("prefix11", connectionData.Packages.Get("package11").Prefix);
            Assert.IsNotNull(connectionData.Packages.Get("package12"));
            Assert.AreEqual("prefix12", connectionData.Packages.Get("package12").Prefix);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machinePackageKey));
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereArePolicyOverrides()
        {
            OracleConnectionData connectionData = new OracleConnectionData();
            connectionData.Name = "data1";
            connectionData.Packages.Add(new OraclePackageData("package11", "prefix11"));
            connectionData.Packages.Add(new OraclePackageData("package12", "prefix12"));
            section.OracleConnectionsData.Add(connectionData);

            MockRegistryKey machinePackageKey = new MockRegistryKey(false);
            machineKey.AddSubKey("data1", machinePackageKey);
            machinePackageKey.AddStringValue(OracleConnectionSettingsManageabilityProvider.PackagesPropertyName,
                                             "package13=prefix13; package14=prefix14; package15=prefix15");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(3, connectionData.Packages.Count);
            Assert.IsNotNull(connectionData.Packages.Get("package13"));
            Assert.AreEqual("prefix13", connectionData.Packages.Get("package13").Prefix);
            Assert.IsNotNull(connectionData.Packages.Get("package14"));
            Assert.AreEqual("prefix14", connectionData.Packages.Get("package14").Prefix);
            Assert.IsNotNull(connectionData.Packages.Get("package15"));
            Assert.AreEqual("prefix15", connectionData.Packages.Get("package15").Prefix);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machinePackageKey));
        }

        [TestMethod]
        public void MultipleConfigurationObjectsAreModifiedIfThereArePolicyOverrides()
        {
            OracleConnectionData connection1Data = new OracleConnectionData();
            connection1Data.Name = "data1";
            connection1Data.Packages.Add(new OraclePackageData("package11", "prefix11"));
            connection1Data.Packages.Add(new OraclePackageData("package12", "prefix12"));
            section.OracleConnectionsData.Add(connection1Data);

            OracleConnectionData connection2Data = new OracleConnectionData();
            connection2Data.Name = "data2";
            connection2Data.Packages.Add(new OraclePackageData("package21", "prefix21"));
            connection2Data.Packages.Add(new OraclePackageData("package22", "prefix22"));
            connection2Data.Packages.Add(new OraclePackageData("package23", "prefix23"));
            section.OracleConnectionsData.Add(connection2Data);

            OracleConnectionData connection3Data = new OracleConnectionData();
            connection3Data.Name = "data3";
            connection3Data.Packages.Add(new OraclePackageData("package31", "prefix31"));
            section.OracleConnectionsData.Add(connection3Data);

            MockRegistryKey machinePackage1Key = new MockRegistryKey(false);
            machineKey.AddSubKey("data1", machinePackage1Key);
            machinePackage1Key.AddStringValue(OracleConnectionSettingsManageabilityProvider.PackagesPropertyName,
                                              "package13=prefix13; package14=prefix14; package15=prefix15");
            MockRegistryKey machinePackage2Key = new MockRegistryKey(false);
            machineKey.AddSubKey("data2", machinePackage2Key);
            machinePackage2Key.AddStringValue(OracleConnectionSettingsManageabilityProvider.PackagesPropertyName,
                                              "package24=prefix24");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(3, connection1Data.Packages.Count);
            Assert.IsNotNull(connection1Data.Packages.Get("package13"));
            Assert.AreEqual("prefix13", connection1Data.Packages.Get("package13").Prefix);
            Assert.IsNotNull(connection1Data.Packages.Get("package14"));
            Assert.AreEqual("prefix14", connection1Data.Packages.Get("package14").Prefix);
            Assert.IsNotNull(connection1Data.Packages.Get("package15"));
            Assert.AreEqual("prefix15", connection1Data.Packages.Get("package15").Prefix);

            Assert.AreEqual(1, connection2Data.Packages.Count);
            Assert.IsNotNull(connection2Data.Packages.Get("package24"));
            Assert.AreEqual("prefix24", connection2Data.Packages.Get("package24").Prefix);

            Assert.AreEqual(1, connection3Data.Packages.Count);
            Assert.IsNotNull(connection3Data.Packages.Get("package31"));
            Assert.AreEqual("prefix31", connection3Data.Packages.Get("package31").Prefix);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machinePackage1Key, machinePackage2Key));
        }

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfThereArePolicyOverridesButGroupPoliciesAreDisabled()
        {
            OracleConnectionData connectionData = new OracleConnectionData();
            connectionData.Name = "data1";
            connectionData.Packages.Add(new OraclePackageData("package11", "prefix11"));
            connectionData.Packages.Add(new OraclePackageData("package12", "prefix12"));
            section.OracleConnectionsData.Add(connectionData);

            MockRegistryKey machinePackageKey = new MockRegistryKey(false);
            machineKey.AddSubKey("data1", machinePackageKey);
            machinePackageKey.AddStringValue(OracleConnectionSettingsManageabilityProvider.PackagesPropertyName,
                                             "package13=prefix13; package14=prefix14; package15=prefix15");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(2, connectionData.Packages.Count);
            Assert.IsNotNull(connectionData.Packages.Get("package11"));
            Assert.AreEqual("prefix11", connectionData.Packages.Get("package11").Prefix);
            Assert.IsNotNull(connectionData.Packages.Get("package12"));
            Assert.AreEqual("prefix12", connectionData.Packages.Get("package12").Prefix);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machinePackageKey));
        }

        [TestMethod]
        public void OracleConnectionWithDisabledPolicyIsRemoved()
        {
            OracleConnectionData connectionData1 = new OracleConnectionData();
            connectionData1.Name = "data1";
            section.OracleConnectionsData.Add(connectionData1);
            OracleConnectionData connectionData2 = new OracleConnectionData();
            connectionData2.Name = "data2";
            section.OracleConnectionsData.Add(connectionData2);

            MockRegistryKey machinePackage1Key = new MockRegistryKey(false);
            machineKey.AddSubKey("data1", machinePackage1Key);
            machinePackage1Key.AddBooleanValue(AdmContentBuilder.AvailableValueName, false);
            MockRegistryKey machinePackage2Key = new MockRegistryKey(false);
            machineKey.AddSubKey("data2", machinePackage2Key);
            machinePackage2Key.AddBooleanValue(AdmContentBuilder.AvailableValueName, true);
            machinePackage2Key.AddStringValue(OracleConnectionSettingsManageabilityProvider.PackagesPropertyName,
                                              "package24=prefix24");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, section.OracleConnectionsData.Count);
            Assert.IsNotNull(section.OracleConnectionsData.Get("data2"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machinePackage1Key, machinePackage2Key));
        }

        [TestMethod]
        public void OracleConnectionWithDisabledPolicyIsNotRemovedIfGroupPoliciesAreDisabled()
        {
            OracleConnectionData connectionData1 = new OracleConnectionData();
            connectionData1.Name = "data1";
            section.OracleConnectionsData.Add(connectionData1);
            OracleConnectionData connectionData2 = new OracleConnectionData();
            connectionData2.Name = "data2";
            section.OracleConnectionsData.Add(connectionData2);

            MockRegistryKey machinePackage1Key = new MockRegistryKey(false);
            machineKey.AddSubKey("data1", machinePackage1Key);
            machinePackage1Key.AddBooleanValue(AdmContentBuilder.AvailableValueName, false);
            MockRegistryKey machinePackage2Key = new MockRegistryKey(false);
            machineKey.AddSubKey("data2", machinePackage2Key);
            machinePackage2Key.AddBooleanValue(AdmContentBuilder.AvailableValueName, true);
            machinePackage2Key.AddStringValue(OracleConnectionSettingsManageabilityProvider.PackagesPropertyName,
                                              "package24=prefix24");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(2, section.OracleConnectionsData.Count);
            Assert.IsNotNull(section.OracleConnectionsData.Get("data1"));
            Assert.IsNotNull(section.OracleConnectionsData.Get("data2"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machinePackage1Key, machinePackage2Key));
        }

        [TestMethod]
        public void WmiSettingsAreNotGeneratedIfWmiIsDisabled()
        {
            OracleConnectionData connection1Data = new OracleConnectionData();
            connection1Data.Name = "data1";
            connection1Data.Packages.Add(new OraclePackageData("package11", "prefix11"));
            connection1Data.Packages.Add(new OraclePackageData("package12", "prefix12"));
            section.OracleConnectionsData.Add(connection1Data);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, false, wmiSettings);

            Assert.AreEqual(0, wmiSettings.Count);
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedIfWmiIsEnabled()
        {
            OracleConnectionData connection1Data = new OracleConnectionData();
            connection1Data.Name = "data1";
            connection1Data.Packages.Add(new OraclePackageData("package11", "prefix11"));
            connection1Data.Packages.Add(new OraclePackageData("package12", "prefix12"));
            section.OracleConnectionsData.Add(connection1Data);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(OracleConnectionSetting), wmiSettings[0].GetType());

            Dictionary<String, String> packagesDictionary = new Dictionary<string, string>();
            foreach (String entry in ((OracleConnectionSetting)wmiSettings[0]).Packages)
            {
                KeyValuePairParsingTestHelper.ExtractKeyValueEntries(entry, packagesDictionary);
            }
            Assert.AreEqual(2, packagesDictionary.Count);
            Assert.AreEqual("prefix11", packagesDictionary["package11"]);
            Assert.AreEqual("prefix12", packagesDictionary["package12"]);
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedWithPolicyOverridesIfWmiIsEnabled()
        {
            OracleConnectionData connection1Data = new OracleConnectionData();
            connection1Data.Name = "data1";
            connection1Data.Packages.Add(new OraclePackageData("package11", "prefix11"));
            connection1Data.Packages.Add(new OraclePackageData("package12", "prefix12"));
            section.OracleConnectionsData.Add(connection1Data);

            MockRegistryKey machinePackageKey = new MockRegistryKey(false);
            machineKey.AddSubKey("data1", machinePackageKey);
            machinePackageKey.AddStringValue(OracleConnectionSettingsManageabilityProvider.PackagesPropertyName,
                                             "package13=prefix13; package14=prefix14; package15=prefix15");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(OracleConnectionSetting), wmiSettings[0].GetType());

            Dictionary<String, String> packagesDictionary = new Dictionary<string, string>();
            foreach (String entry in ((OracleConnectionSetting)wmiSettings[0]).Packages)
            {
                KeyValuePairParsingTestHelper.ExtractKeyValueEntries(entry, packagesDictionary);
            }
            Assert.AreEqual(3, packagesDictionary.Count);
            Assert.AreEqual("prefix13", packagesDictionary["package13"]);
            Assert.AreEqual("prefix14", packagesDictionary["package14"]);
            Assert.AreEqual("prefix15", packagesDictionary["package15"]);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machinePackageKey));
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContent()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();

            OracleConnectionData data1 = new OracleConnectionData();
            section.OracleConnectionsData.Add(data1);
            data1.Name = "data1";
            data1.Packages.Add(new OraclePackageData("name1", "prefix1"));
            data1.Packages.Add(new OraclePackageData("name2", "pre;fix2"));
            OracleConnectionData data2 = new OracleConnectionData();
            section.OracleConnectionsData.Add(data2);
            data2.Name = "data2";

            MockAdmContentBuilder contentBuilder = new MockAdmContentBuilder();

            provider.AddAdministrativeTemplateDirectives(contentBuilder, section, configurationSource, "TestApp");

            MockAdmContent content = contentBuilder.GetMockContent();
            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            Assert.IsTrue(categoriesEnumerator.MoveNext());
            IEnumerator<AdmCategory> subCategoriesEnumerator = categoriesEnumerator.Current.Categories.GetEnumerator();
            Assert.IsTrue(subCategoriesEnumerator.MoveNext());
            IEnumerator<AdmPolicy> policiesEnumerator = subCategoriesEnumerator.Current.Policies.GetEnumerator();

            Assert.IsTrue(policiesEnumerator.MoveNext());
            IEnumerator<AdmPart> oracleDataPartsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(oracleDataPartsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), oracleDataPartsEnumerator.Current.GetType());
            Assert.IsNull(oracleDataPartsEnumerator.Current.KeyName);
            Assert.AreEqual(OracleConnectionSettingsManageabilityProvider.PackagesPropertyName,
                            oracleDataPartsEnumerator.Current.ValueName);
            IDictionary<String, String> packages = new Dictionary<String, String>();
            KeyValuePairParser.ExtractKeyValueEntries(((AdmEditTextPart)oracleDataPartsEnumerator.Current).DefaultValue, packages);
            Assert.AreEqual(2, packages.Count);
            Assert.AreEqual("prefix1", packages["name1"]);
            Assert.AreEqual("pre;fix2", packages["name2"]);

            Assert.IsFalse(oracleDataPartsEnumerator.MoveNext());
            Assert.IsTrue(policiesEnumerator.MoveNext());
            Assert.IsFalse(policiesEnumerator.MoveNext());

            Assert.IsFalse(subCategoriesEnumerator.MoveNext());
            IEnumerator<AdmPolicy> sectionPoliciesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            Assert.IsFalse(sectionPoliciesEnumerator.MoveNext());
            Assert.IsFalse(categoriesEnumerator.MoveNext());
        }
    }
}
