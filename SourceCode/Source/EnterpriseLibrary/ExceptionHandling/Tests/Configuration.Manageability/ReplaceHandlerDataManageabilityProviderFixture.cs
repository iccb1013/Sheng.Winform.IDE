//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability.Tests
{
    [TestClass]
    public class ReplaceHandlerDataManageabilityProviderFixture
    {
        ReplaceHandlerDataManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        IList<ConfigurationSetting> wmiSettings;
        ReplaceHandlerData configurationObject;

        [TestInitialize]
        public void SetUp()
        {
            provider = new ReplaceHandlerDataManageabilityProvider();
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            wmiSettings = new List<ConfigurationSetting>();
            configurationObject = new ReplaceHandlerData();
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

            Assembly assembly = typeof(ReplaceHandlerDataManageabilityProvider).Assembly;
            foreach (ConfigurationElementManageabilityProviderAttribute providerAttribute
                in assembly.GetCustomAttributes(typeof(ConfigurationElementManageabilityProviderAttribute), false))
            {
                if (providerAttribute.ManageabilityProviderType.Equals(typeof(ReplaceHandlerDataManageabilityProvider)))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(ExceptionHandlingSettingsManageabilityProvider), selectedAttribute.SectionManageabilityProviderType);
            Assert.AreSame(typeof(ReplaceHandlerData), selectedAttribute.TargetType);
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
            configurationObject.ExceptionMessage = "message";
            configurationObject.ReplaceExceptionType = typeof(ArgumentException);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, null, true, wmiSettings);

            Assert.AreEqual("message", configurationObject.ExceptionMessage);
            Assert.AreSame(typeof(ArgumentException), configurationObject.ReplaceExceptionType);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreMachinePolicyOverrides()
        {
            configurationObject.ExceptionMessage = "message";
            configurationObject.ReplaceExceptionType = typeof(ArgumentException);

            machineKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ExceptionMessagePropertyName, "overriden message");
            machineKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ReplaceExceptionTypePropertyName, typeof(NullReferenceException).AssemblyQualifiedName);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, null, true, wmiSettings);

            Assert.AreEqual("overriden message", configurationObject.ExceptionMessage);
            Assert.AreSame(typeof(NullReferenceException), configurationObject.ReplaceExceptionType);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreUserPolicyOverrides()
        {
            configurationObject.ExceptionMessage = "message";
            configurationObject.ReplaceExceptionType = typeof(ArgumentException);

            userKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ExceptionMessagePropertyName, "overriden message");
            userKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ReplaceExceptionTypePropertyName, typeof(NullReferenceException).AssemblyQualifiedName);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, userKey, true, wmiSettings);

            Assert.AreEqual("overriden message", configurationObject.ExceptionMessage);
            Assert.AreSame(typeof(NullReferenceException), configurationObject.ReplaceExceptionType);
        }

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfThereArePolicyOverridesButGroupPoliciesAreDisabled()
        {
            configurationObject.ExceptionMessage = "message";
            configurationObject.ReplaceExceptionType = typeof(ArgumentException);

            machineKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ExceptionMessagePropertyName, "overriden message");
            machineKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ReplaceExceptionTypePropertyName, typeof(NullReferenceException).AssemblyQualifiedName);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, null, true, wmiSettings);

            Assert.AreEqual("message", configurationObject.ExceptionMessage);
            Assert.AreSame(typeof(ArgumentException), configurationObject.ReplaceExceptionType);
        }

        [TestMethod]
        public void ExceptionTypeIsOverridenIfValueIsValid()
        {
            configurationObject.ReplaceExceptionType = typeof(ArgumentException);

            machineKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ExceptionMessagePropertyName, "msg");
            machineKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ReplaceExceptionTypePropertyName, typeof(NullReferenceException).AssemblyQualifiedName);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, null, true, wmiSettings);

            Assert.AreSame(typeof(NullReferenceException), configurationObject.ReplaceExceptionType);
        }

        [TestMethod]
        public void TypeIsNotOverridenIfValueIsInvalid()
        {
            configurationObject.ReplaceExceptionType = typeof(ArgumentException);

            machineKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ExceptionMessagePropertyName, "msg");
            machineKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ReplaceExceptionTypePropertyName, "An invalid type name");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, null, true, wmiSettings);

            Assert.AreSame(typeof(ArgumentException), configurationObject.ReplaceExceptionType);
        }

        [TestMethod]
        public void WmiSettingsAreNotGeneratedIfWmiIsDisabled()
        {
            configurationObject.ExceptionMessage = "message";
            configurationObject.ReplaceExceptionType = typeof(ArgumentException);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, userKey, false, wmiSettings);

            Assert.AreEqual(0, wmiSettings.Count);
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedIfWmiIsEnabled()
        {
            configurationObject.ExceptionMessage = "message";
            configurationObject.ReplaceExceptionType = typeof(ArgumentException);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(ReplaceHandlerSetting), wmiSettings[0].GetType());
            Assert.AreEqual(configurationObject.ExceptionMessage, ((ReplaceHandlerSetting)wmiSettings[0]).ExceptionMessage);
            Assert.AreEqual(configurationObject.ReplaceExceptionType.AssemblyQualifiedName, ((ReplaceHandlerSetting)wmiSettings[0]).ReplaceExceptionType);
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedWithPolicyOverridesIfWmiIsEnabled()
        {
            configurationObject.ExceptionMessage = "message";
            configurationObject.ReplaceExceptionType = typeof(ArgumentException);

            machineKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ExceptionMessagePropertyName, "overriden message");
            machineKey.AddStringValue(ReplaceHandlerDataManageabilityProvider.ReplaceExceptionTypePropertyName, typeof(NullReferenceException).AssemblyQualifiedName);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(ReplaceHandlerSetting), wmiSettings[0].GetType());
            Assert.AreEqual(configurationObject.ExceptionMessage, ((ReplaceHandlerSetting)wmiSettings[0]).ExceptionMessage);
            Assert.AreEqual(configurationObject.ReplaceExceptionType.AssemblyQualifiedName, ((ReplaceHandlerSetting)wmiSettings[0]).ReplaceExceptionType);
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContent()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            configurationObject.ReplaceExceptionType = typeof(Exception);

            MockAdmContentBuilder contentBuilder = new MockAdmContentBuilder();

            contentBuilder.StartCategory("category");
            contentBuilder.StartPolicy("policy", "policy key");
            provider.AddAdministrativeTemplateDirectives(contentBuilder, configurationObject, configurationSource, "TestApp");
            contentBuilder.EndPolicy();
            contentBuilder.EndCategory();

            MockAdmContent content = contentBuilder.GetMockContent();
            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            categoriesEnumerator.MoveNext();
            IEnumerator<AdmPolicy> policiesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            policiesEnumerator.MoveNext();
            IEnumerator<AdmPart> partsEnumerator = policiesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmTextPart), partsEnumerator.Current.GetType());
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNotNull(((AdmEditTextPart)partsEnumerator.Current).KeyName);
            Assert.AreEqual(ReplaceHandlerDataManageabilityProvider.ExceptionMessagePropertyName,
                            partsEnumerator.Current.ValueName);
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNotNull(((AdmEditTextPart)partsEnumerator.Current).KeyName);
            Assert.AreEqual(ReplaceHandlerDataManageabilityProvider.ReplaceExceptionTypePropertyName,
                            partsEnumerator.Current.ValueName);
            Assert.IsFalse(partsEnumerator.MoveNext());
        }
    }
}
