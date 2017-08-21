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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Tests.Filters
{
    [TestClass]
    public class PriorityFilterDataManageabilityProviderFixture
    {
        PriorityFilterDataManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        IList<ConfigurationSetting> wmiSettings;
        PriorityFilterData configurationObject;

        [TestInitialize]
        public void SetUp()
        {
            provider = new PriorityFilterDataManageabilityProvider();
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            wmiSettings = new List<ConfigurationSetting>();
            configurationObject = new PriorityFilterData();
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

            Assembly assembly = typeof(PriorityFilterDataManageabilityProvider).Assembly;
            foreach (ConfigurationElementManageabilityProviderAttribute providerAttribute
                in assembly.GetCustomAttributes(typeof(ConfigurationElementManageabilityProviderAttribute), false))
            {
                if (providerAttribute.ManageabilityProviderType.Equals(typeof(PriorityFilterDataManageabilityProvider)))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(LoggingSettingsManageabilityProvider), selectedAttribute.SectionManageabilityProviderType);
            Assert.AreSame(typeof(PriorityFilterData), selectedAttribute.TargetType);
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
            configurationObject.MaximumPriority = 10;
            configurationObject.MinimumPriority = 5;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(10, configurationObject.MaximumPriority);
            Assert.AreEqual(5, configurationObject.MinimumPriority);
        }

        [TestMethod]
        public void NoExceptionsAreThrownIfMachineKeyIsNull()
        {
            configurationObject.MaximumPriority = 10;
            configurationObject.MinimumPriority = 5;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, userKey, true, wmiSettings);
        }

        [TestMethod]
        public void NoExceptionsAreThrownIfUserKeyIsNull()
        {
            configurationObject.MaximumPriority = 10;
            configurationObject.MinimumPriority = 5;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, userKey, true, wmiSettings);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreMachinePolicyOverrides()
        {
            configurationObject.MaximumPriority = 10;
            configurationObject.MinimumPriority = 5;

            machineKey.AddIntValue(PriorityFilterDataManageabilityProvider.MaximumPriorityPropertyName, 9);
            machineKey.AddIntValue(PriorityFilterDataManageabilityProvider.MinimumPriorityPropertyName, 3);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(9, configurationObject.MaximumPriority);
            Assert.AreEqual(3, configurationObject.MinimumPriority);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreUserPolicyOverrides()
        {
            configurationObject.MaximumPriority = 10;
            configurationObject.MinimumPriority = 5;

            userKey.AddIntValue(PriorityFilterDataManageabilityProvider.MaximumPriorityPropertyName, 9);
            userKey.AddIntValue(PriorityFilterDataManageabilityProvider.MinimumPriorityPropertyName, 3);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, userKey, true, wmiSettings);

            Assert.AreEqual(9, configurationObject.MaximumPriority);
            Assert.AreEqual(3, configurationObject.MinimumPriority);
        }

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfThereArePolicyOverridesButGroupPoliciesAreDisabled()
        {
            configurationObject.MaximumPriority = 10;
            configurationObject.MinimumPriority = 5;

            userKey.AddIntValue(PriorityFilterDataManageabilityProvider.MaximumPriorityPropertyName, 9);
            userKey.AddIntValue(PriorityFilterDataManageabilityProvider.MinimumPriorityPropertyName, 3);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, null, true, wmiSettings);

            Assert.AreEqual(10, configurationObject.MaximumPriority);
            Assert.AreEqual(5, configurationObject.MinimumPriority);
        }

        [TestMethod]
        public void WmiSettingsAreNotGeneratedIfWmiIsDisabled()
        {
            configurationObject.MaximumPriority = 10;
            configurationObject.MinimumPriority = 5;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, null, null, false, wmiSettings);

            Assert.AreEqual(0, wmiSettings.Count);
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedIfWmiIsEnabled()
        {
            configurationObject.MaximumPriority = 10;
            configurationObject.MinimumPriority = 5;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, null, null, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(PriorityFilterSetting), wmiSettings[0].GetType());
            Assert.AreEqual(configurationObject.MaximumPriority, ((PriorityFilterSetting)wmiSettings[0]).MaximumPriority);
            Assert.AreEqual(configurationObject.MinimumPriority, ((PriorityFilterSetting)wmiSettings[0]).MinimumPriority);
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedWithPolicyOverridesIfWmiIsEnabled()
        {
            configurationObject.MaximumPriority = 10;
            configurationObject.MinimumPriority = 5;

            machineKey.AddIntValue(PriorityFilterDataManageabilityProvider.MaximumPriorityPropertyName, 9);
            machineKey.AddIntValue(PriorityFilterDataManageabilityProvider.MinimumPriorityPropertyName, 3);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, null, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(PriorityFilterSetting), wmiSettings[0].GetType());
            Assert.AreEqual(configurationObject.MaximumPriority, ((PriorityFilterSetting)wmiSettings[0]).MaximumPriority);
            Assert.AreEqual(configurationObject.MinimumPriority, ((PriorityFilterSetting)wmiSettings[0]).MinimumPriority);
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContent()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();

            configurationObject.MaximumPriority = 999999; // ADM templates do not support the default value for this property.

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
            Assert.AreSame(typeof(AdmNumericPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(PriorityFilterDataManageabilityProvider.MaximumPriorityPropertyName,
                            partsEnumerator.Current.ValueName);
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmNumericPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(PriorityFilterDataManageabilityProvider.MinimumPriorityPropertyName,
                            partsEnumerator.Current.ValueName);
            Assert.IsFalse(partsEnumerator.MoveNext());
            Assert.IsFalse(policiesEnumerator.MoveNext());
        }
    }
}
