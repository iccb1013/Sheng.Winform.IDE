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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Tests.Filters
{
    [TestClass]
    public class CategoryFilterDataManageabilityProviderFixture
    {
        CategoryFilterDataManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        IList<ConfigurationSetting> wmiSettings;
        CategoryFilterData configurationObject;

        [TestInitialize]
        public void SetUp()
        {
            provider = new CategoryFilterDataManageabilityProvider();
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            wmiSettings = new List<ConfigurationSetting>();
            configurationObject = new CategoryFilterData();
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
            ConfigurationElementManageabilityProviderAttribute selectedAttribute = null;

            Assembly assembly = typeof(CategoryFilterDataManageabilityProvider).Assembly;
            foreach (ConfigurationElementManageabilityProviderAttribute providerAttribute
                in assembly.GetCustomAttributes(typeof(ConfigurationElementManageabilityProviderAttribute), false))
            {
                if (providerAttribute.ManageabilityProviderType.Equals(typeof(CategoryFilterDataManageabilityProvider)))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(LoggingSettingsManageabilityProvider), selectedAttribute.SectionManageabilityProviderType);
            Assert.AreSame(typeof(CategoryFilterData), selectedAttribute.TargetType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ProviderThrowsWithConfigurationObjectOfWrongType()
        {
            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(new TestsConfigurationSection(), true, machineKey, userKey, true, wmiSettings);
        }

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfThereAreNoPolicyOverrides()
        {
            configurationObject.CategoryFilterMode = CategoryFilterMode.AllowAllExceptDenied;
            configurationObject.CategoryFilters.Add(new CategoryFilterEntry("cat1"));
            configurationObject.CategoryFilters.Add(new CategoryFilterEntry("cat2"));

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, null, true, wmiSettings);

            Assert.AreEqual(CategoryFilterMode.AllowAllExceptDenied, configurationObject.CategoryFilterMode);
            Assert.AreEqual(2, configurationObject.CategoryFilters.Count);
            Assert.IsNotNull(configurationObject.CategoryFilters.Get("cat1"));
            Assert.IsNotNull(configurationObject.CategoryFilters.Get("cat2"));
        }

        [TestMethod]
        public void NoExceptionsAreThrownIfMachineKeyIsNull()
        {
            configurationObject.CategoryFilterMode = CategoryFilterMode.AllowAllExceptDenied;
            configurationObject.CategoryFilters.Add(new CategoryFilterEntry("cat1"));
            configurationObject.CategoryFilters.Add(new CategoryFilterEntry("cat2"));

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, userKey, true, wmiSettings);
        }

        [TestMethod]
        public void NoExceptionsAreThrownIfUserKeyIsNull()
        {
            configurationObject.CategoryFilterMode = CategoryFilterMode.AllowAllExceptDenied;
            configurationObject.CategoryFilters.Add(new CategoryFilterEntry("cat1"));
            configurationObject.CategoryFilters.Add(new CategoryFilterEntry("cat2"));

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, userKey, true, wmiSettings);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreMachinePolicyOverrides()
        {
            configurationObject.CategoryFilterMode = CategoryFilterMode.AllowAllExceptDenied;
            configurationObject.CategoryFilters.Add(new CategoryFilterEntry("cat1"));
            configurationObject.CategoryFilters.Add(new CategoryFilterEntry("cat2"));

            machineKey.AddStringValue(CategoryFilterDataManageabilityProvider.CategoryFilterModePropertyName, CategoryFilterMode.DenyAllExceptAllowed.ToString());
            MockRegistryKey machineCategoriesKey = new MockRegistryKey(false);
            machineKey.AddSubKey(CategoryFilterDataManageabilityProvider.CategoryFiltersKeyName, machineCategoriesKey);
            machineCategoriesKey.AddBooleanValue("cat3", true);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(CategoryFilterMode.DenyAllExceptAllowed, configurationObject.CategoryFilterMode);
            Assert.AreEqual(1, configurationObject.CategoryFilters.Count);
            Assert.IsNotNull(configurationObject.CategoryFilters.Get("cat3"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineCategoriesKey));
        }

        [TestMethod]
        public void CategoryFiltersCollectionIsEmptiedIfMachineKeyIsNotNullButCategoryFiltersSubkeyIsMissing()
        {
            configurationObject.CategoryFilterMode = CategoryFilterMode.AllowAllExceptDenied;
            configurationObject.CategoryFilters.Add(new CategoryFilterEntry("cat1"));
            configurationObject.CategoryFilters.Add(new CategoryFilterEntry("cat2"));

            machineKey.AddStringValue(CategoryFilterDataManageabilityProvider.CategoryFilterModePropertyName, CategoryFilterMode.DenyAllExceptAllowed.ToString());

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(CategoryFilterMode.DenyAllExceptAllowed, configurationObject.CategoryFilterMode);
            Assert.AreEqual(0, configurationObject.CategoryFilters.Count);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreUserPolicyOverrides()
        {
            configurationObject.CategoryFilterMode = CategoryFilterMode.AllowAllExceptDenied;
            configurationObject.CategoryFilters.Add(new CategoryFilterEntry("cat1"));
            configurationObject.CategoryFilters.Add(new CategoryFilterEntry("cat2"));

            userKey.AddStringValue(CategoryFilterDataManageabilityProvider.CategoryFilterModePropertyName, CategoryFilterMode.DenyAllExceptAllowed.ToString());
            MockRegistryKey userCategoriesKey = new MockRegistryKey(false);
            userKey.AddSubKey(CategoryFilterDataManageabilityProvider.CategoryFiltersKeyName, userCategoriesKey);
            userCategoriesKey.AddBooleanValue("cat3", true);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, userKey, true, wmiSettings);

            Assert.AreEqual(CategoryFilterMode.DenyAllExceptAllowed, configurationObject.CategoryFilterMode);
            Assert.AreEqual(1, configurationObject.CategoryFilters.Count);
            Assert.IsNotNull(configurationObject.CategoryFilters.Get("cat3"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(userCategoriesKey));
        }

        [TestMethod]
        public void MachineKeyOverridesTakePrecedenceOverUserKeyOverrides()
        {
            configurationObject.CategoryFilterMode = CategoryFilterMode.AllowAllExceptDenied;
            configurationObject.CategoryFilters.Add(new CategoryFilterEntry("cat1"));
            configurationObject.CategoryFilters.Add(new CategoryFilterEntry("cat2"));

            machineKey.AddStringValue(CategoryFilterDataManageabilityProvider.CategoryFilterModePropertyName, CategoryFilterMode.DenyAllExceptAllowed.ToString());
            userKey.AddStringValue(CategoryFilterDataManageabilityProvider.CategoryFilterModePropertyName, CategoryFilterMode.AllowAllExceptDenied.ToString());
            MockRegistryKey userCategoriesKey = new MockRegistryKey(false);
            userKey.AddSubKey(CategoryFilterDataManageabilityProvider.CategoryFiltersKeyName, userCategoriesKey);
            userCategoriesKey.AddBooleanValue("cat3", true);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(CategoryFilterMode.DenyAllExceptAllowed, configurationObject.CategoryFilterMode);
            Assert.AreEqual(0, configurationObject.CategoryFilters.Count);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(userCategoriesKey));
        }

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfThereArePolicyOverridesButGroupPoliciesAreDisabled()
        {
            configurationObject.CategoryFilterMode = CategoryFilterMode.AllowAllExceptDenied;
            configurationObject.CategoryFilters.Add(new CategoryFilterEntry("cat1"));
            configurationObject.CategoryFilters.Add(new CategoryFilterEntry("cat2"));

            machineKey.AddStringValue(CategoryFilterDataManageabilityProvider.CategoryFilterModePropertyName, CategoryFilterMode.DenyAllExceptAllowed.ToString());
            MockRegistryKey machineCategoriesKey = new MockRegistryKey(false);
            machineKey.AddSubKey(CategoryFilterDataManageabilityProvider.CategoryFiltersKeyName, machineCategoriesKey);
            machineCategoriesKey.AddBooleanValue("cat3", true);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(CategoryFilterMode.AllowAllExceptDenied, configurationObject.CategoryFilterMode);
            Assert.AreEqual(2, configurationObject.CategoryFilters.Count);
            Assert.IsNotNull(configurationObject.CategoryFilters.Get("cat1"));
            Assert.IsNotNull(configurationObject.CategoryFilters.Get("cat2"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machineCategoriesKey));
        }

        [TestMethod]
        public void WmiSettingsAreNotGeneratedIfWmiIsDisabled()
        {
            configurationObject.CategoryFilterMode = CategoryFilterMode.AllowAllExceptDenied;
            configurationObject.CategoryFilters.Add(new CategoryFilterEntry("cat1"));
            configurationObject.CategoryFilters.Add(new CategoryFilterEntry("cat2"));

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, userKey, false, wmiSettings);

            Assert.AreEqual(0, wmiSettings.Count);
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedIfWmiIsEnabled()
        {
            configurationObject.CategoryFilterMode = CategoryFilterMode.AllowAllExceptDenied;
            configurationObject.CategoryFilters.Add(new CategoryFilterEntry("cat1"));
            configurationObject.CategoryFilters.Add(new CategoryFilterEntry("cat2"));

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(CategoryFilterSetting), wmiSettings[0].GetType());
            Assert.AreEqual(configurationObject.CategoryFilterMode.ToString(), ((CategoryFilterSetting)wmiSettings[0]).CategoryFilterMode);
            Assert.AreEqual(configurationObject.CategoryFilters.Count, ((CategoryFilterSetting)wmiSettings[0]).CategoryFilters.Length);
            foreach (String name in ((CategoryFilterSetting)wmiSettings[0]).CategoryFilters)
            {
                Assert.IsNotNull(configurationObject.CategoryFilters.Get(name));
            }
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedWithPolicyOverridesIfWmiIsEnabled()
        {
            configurationObject.CategoryFilterMode = CategoryFilterMode.AllowAllExceptDenied;
            configurationObject.CategoryFilters.Add(new CategoryFilterEntry("cat1"));
            configurationObject.CategoryFilters.Add(new CategoryFilterEntry("cat2"));

            machineKey.AddStringValue(CategoryFilterDataManageabilityProvider.CategoryFilterModePropertyName, CategoryFilterMode.DenyAllExceptAllowed.ToString());
            MockRegistryKey machineCategoriesKey = new MockRegistryKey(false);
            machineKey.AddSubKey(CategoryFilterDataManageabilityProvider.CategoryFiltersKeyName, machineCategoriesKey);
            machineCategoriesKey.AddBooleanValue("cat3", true);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(CategoryFilterSetting), wmiSettings[0].GetType());
            Assert.AreEqual(configurationObject.CategoryFilterMode.ToString(), ((CategoryFilterSetting)wmiSettings[0]).CategoryFilterMode);
            Assert.AreEqual(configurationObject.CategoryFilters.Count, ((CategoryFilterSetting)wmiSettings[0]).CategoryFilters.Length);
            foreach (String name in ((CategoryFilterSetting)wmiSettings[0]).CategoryFilters)
            {
                Assert.IsNotNull(configurationObject.CategoryFilters.Get(name));
            }
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContent()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            LoggingSettings section = new LoggingSettings();
            configurationSource.Add(LoggingSettings.SectionName, section);
            section.TraceSources.Add(new TraceSourceData("source1", SourceLevels.Off));
            section.TraceSources.Add(new TraceSourceData("source2", SourceLevels.Off));

            MockAdmContentBuilder contentBuilder = new MockAdmContentBuilder();

            contentBuilder.StartCategory("category");
            provider.AddAdministrativeTemplateDirectives(contentBuilder, configurationObject, configurationSource, "TestApp");
            contentBuilder.EndCategory();

            MockAdmContent content = contentBuilder.GetMockContent();
            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            categoriesEnumerator.MoveNext();
            IEnumerator<AdmPolicy> policiesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            Assert.IsTrue(policiesEnumerator.MoveNext());
            IEnumerator<AdmPart> partsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(CategoryFilterDataManageabilityProvider.CategoryFilterModePropertyName,
                            partsEnumerator.Current.ValueName);
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmTextPart), partsEnumerator.Current.GetType());
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmCheckboxPart), partsEnumerator.Current.GetType());
            Assert.IsTrue(partsEnumerator.Current.KeyName.EndsWith(CategoryFilterDataManageabilityProvider.CategoryFiltersKeyName));
            Assert.AreEqual("source1", partsEnumerator.Current.ValueName);
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmCheckboxPart), partsEnumerator.Current.GetType());
            Assert.IsTrue(partsEnumerator.Current.KeyName.EndsWith(CategoryFilterDataManageabilityProvider.CategoryFiltersKeyName));
            Assert.AreEqual("source2", partsEnumerator.Current.ValueName);
            Assert.IsFalse(partsEnumerator.MoveNext());
            Assert.IsFalse(policiesEnumerator.MoveNext());
        }
    }
}
