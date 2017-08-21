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
    public class ExceptionHandlingSettingsManageabilityProvideFixture
    {
        ExceptionHandlingSettingsManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        IList<ConfigurationSetting> wmiSettings;
        ExceptionHandlingSettings section;
        DictionaryConfigurationSource configurationSource;

        [TestInitialize]
        public void SetUp()
        {
            provider =
                new ExceptionHandlingSettingsManageabilityProvider(
                    new Dictionary<Type, ConfigurationElementManageabilityProvider>(0));
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            wmiSettings = new List<ConfigurationSetting>();
            section = new ExceptionHandlingSettings();
            configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(ExceptionHandlingSettings.SectionName, section);
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

            Assembly assembly = typeof(ExceptionHandlingSettingsManageabilityProvider).Assembly;
            foreach (ConfigurationSectionManageabilityProviderAttribute providerAttribute
                in assembly.GetCustomAttributes(typeof(ConfigurationSectionManageabilityProviderAttribute), false))
            {
                if (providerAttribute.SectionName.Equals(ExceptionHandlingSettings.SectionName))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(ExceptionHandlingSettingsManageabilityProvider), selectedAttribute.ManageabilityProviderType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ProviderThrowsWithConfigurationObjectOfWrongType()
        {
            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(new TestsConfigurationSection(), true, machineKey, userKey,
                                                                    true, wmiSettings);
        }

        [TestMethod]
        public void CanApplyMachinePolicyOverridesToExistingPolicyType()
        {
            ExceptionPolicyData policy1 = new ExceptionPolicyData("policy1");
            section.ExceptionPolicies.Add(policy1);
            ExceptionTypeData exceptionType1 = new ExceptionTypeData("type1", typeof(Exception), PostHandlingAction.None);
            policy1.ExceptionTypes.Add(exceptionType1);

            MockRegistryKey machinePoliciesKey = new MockRegistryKey(false);
            machineKey.AddSubKey(ExceptionHandlingSettingsManageabilityProvider.PoliciesKeyName, machinePoliciesKey);
            MockRegistryKey machinePolicy1Key = new MockRegistryKey(false);
            machinePoliciesKey.AddSubKey("policy1", machinePolicy1Key);
            MockRegistryKey machinePolicy1TypesKey = new MockRegistryKey(false);
            machinePolicy1Key.AddSubKey(ExceptionHandlingSettingsManageabilityProvider.PolicyTypesPropertyName,
                                        machinePolicy1TypesKey);
            MockRegistryKey machinePolicy1Type1Key = new MockRegistryKey(false);
            machinePolicy1TypesKey.AddSubKey("type1", machinePolicy1Type1Key);
            machinePolicy1Type1Key.AddStringValue(
                ExceptionHandlingSettingsManageabilityProvider.PolicyTypePostHandlingActionPropertyName,
                PostHandlingAction.NotifyRethrow.ToString());

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, null, false, wmiSettings);

            Assert.AreEqual(PostHandlingAction.NotifyRethrow, exceptionType1.PostHandlingAction);

            Assert.IsTrue(
                MockRegistryKey.CheckAllClosed(machinePoliciesKey, machinePolicy1Key, machinePolicy1TypesKey, machinePolicy1Type1Key));
        }

        [TestMethod]
        public void MachineOverridesForMissingPolicyCausesNoProblems()
        {
            ExceptionPolicyData policy1 = new ExceptionPolicyData("policy1");
            section.ExceptionPolicies.Add(policy1);
            ExceptionTypeData exceptionType1 = new ExceptionTypeData("type1", typeof(Exception), PostHandlingAction.None);
            policy1.ExceptionTypes.Add(exceptionType1);

            MockRegistryKey machinePoliciesKey = new MockRegistryKey(false);
            machineKey.AddSubKey(ExceptionHandlingSettingsManageabilityProvider.PoliciesKeyName, machinePoliciesKey);
            MockRegistryKey machinePolicy1Key = new MockRegistryKey(false);
            machinePoliciesKey.AddSubKey("policy2", machinePolicy1Key);
            MockRegistryKey machinePolicy1TypesKey = new MockRegistryKey(false);
            machinePolicy1Key.AddSubKey(ExceptionHandlingSettingsManageabilityProvider.PolicyTypesPropertyName,
                                        machinePolicy1TypesKey);
            MockRegistryKey machinePolicy1Type1Key = new MockRegistryKey(false);
            machinePolicy1TypesKey.AddSubKey("type1", machinePolicy1Type1Key);
            machinePolicy1Type1Key.AddStringValue(
                ExceptionHandlingSettingsManageabilityProvider.PolicyTypePostHandlingActionPropertyName,
                PostHandlingAction.NotifyRethrow.ToString());

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, null, false, wmiSettings);

            Assert.AreEqual(PostHandlingAction.None, exceptionType1.PostHandlingAction);

            Assert.IsTrue(
                MockRegistryKey.CheckAllClosed(machinePoliciesKey, machinePolicy1Key, machinePolicy1TypesKey, machinePolicy1Type1Key));
        }

        [TestMethod]
        public void CanApplyMachinePolicyOverridesToExistingPolicyTyqpeWhenMultiplePolicyOverridesExist()
        {
            ExceptionPolicyData policy1 = new ExceptionPolicyData("policy1");
            section.ExceptionPolicies.Add(policy1);
            ExceptionTypeData exceptionType1 = new ExceptionTypeData("type1", typeof(Exception), PostHandlingAction.None);
            policy1.ExceptionTypes.Add(exceptionType1);

            MockRegistryKey machinePoliciesKey = new MockRegistryKey(false);
            machineKey.AddSubKey(ExceptionHandlingSettingsManageabilityProvider.PoliciesKeyName, machinePoliciesKey);
            MockRegistryKey machinePolicy0Key = new MockRegistryKey(false);
            machinePoliciesKey.AddSubKey("policy0", machinePolicy0Key);
            MockRegistryKey machinePolicy0TypesKey = new MockRegistryKey(false);
            machinePolicy0Key.AddSubKey(ExceptionHandlingSettingsManageabilityProvider.PolicyTypesPropertyName,
                                        machinePolicy0TypesKey);
            MockRegistryKey machinePolicy0Type1Key = new MockRegistryKey(false);
            machinePolicy0TypesKey.AddSubKey("type1", machinePolicy0Type1Key);
            machinePolicy0Type1Key.AddStringValue(
                ExceptionHandlingSettingsManageabilityProvider.PolicyTypePostHandlingActionPropertyName,
                PostHandlingAction.ThrowNewException.ToString());
            MockRegistryKey machinePolicy1Key = new MockRegistryKey(false);
            machinePoliciesKey.AddSubKey("policy1", machinePolicy1Key);
            MockRegistryKey machinePolicy1TypesKey = new MockRegistryKey(false);
            machinePolicy1Key.AddSubKey(ExceptionHandlingSettingsManageabilityProvider.PolicyTypesPropertyName,
                                        machinePolicy1TypesKey);
            MockRegistryKey machinePolicy1Type1Key = new MockRegistryKey(false);
            machinePolicy1TypesKey.AddSubKey("type1", machinePolicy1Type1Key);
            machinePolicy1Type1Key.AddStringValue(
                ExceptionHandlingSettingsManageabilityProvider.PolicyTypePostHandlingActionPropertyName,
                PostHandlingAction.NotifyRethrow.ToString());

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, null, false, wmiSettings);

            Assert.AreEqual(PostHandlingAction.NotifyRethrow, exceptionType1.PostHandlingAction);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machinePoliciesKey,
                                                         machinePolicy0Key, machinePolicy0TypesKey, machinePolicy0Type1Key,
                                                         machinePolicy1Key, machinePolicy1TypesKey, machinePolicy1Type1Key));
        }

        [TestMethod]
        public void CanApplyUserPolicyOverridesToExistingPolicyType()
        {
            ExceptionPolicyData policy1 = new ExceptionPolicyData("policy1");
            section.ExceptionPolicies.Add(policy1);
            ExceptionTypeData exceptionType1 = new ExceptionTypeData("type1", typeof(Exception), PostHandlingAction.None);
            policy1.ExceptionTypes.Add(exceptionType1);

            MockRegistryKey userPoliciesKey = new MockRegistryKey(false);
            userKey.AddSubKey(ExceptionHandlingSettingsManageabilityProvider.PoliciesKeyName, userPoliciesKey);
            MockRegistryKey userPolicy1Key = new MockRegistryKey(false);
            userPoliciesKey.AddSubKey("policy1", userPolicy1Key);
            MockRegistryKey userPolicy1TypesKey = new MockRegistryKey(false);
            userPolicy1Key.AddSubKey(ExceptionHandlingSettingsManageabilityProvider.PolicyTypesPropertyName, userPolicy1TypesKey);
            MockRegistryKey userPolicy1Type1Key = new MockRegistryKey(false);
            userPolicy1TypesKey.AddSubKey("type1", userPolicy1Type1Key);
            userPolicy1Type1Key.AddStringValue(
                ExceptionHandlingSettingsManageabilityProvider.PolicyTypePostHandlingActionPropertyName,
                PostHandlingAction.NotifyRethrow.ToString());

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, null, userKey, false, wmiSettings);

            Assert.AreEqual(PostHandlingAction.NotifyRethrow, exceptionType1.PostHandlingAction);

            Assert.IsTrue(
                MockRegistryKey.CheckAllClosed(userPoliciesKey, userPolicy1Key, userPolicy1TypesKey, userPolicy1Type1Key));
        }

        [TestMethod]
        public void UserOverridesForMissingPolicyCausesNoProblems()
        {
            ExceptionPolicyData policy1 = new ExceptionPolicyData("policy1");
            section.ExceptionPolicies.Add(policy1);
            ExceptionTypeData exceptionType1 = new ExceptionTypeData("type1", typeof(Exception), PostHandlingAction.None);
            policy1.ExceptionTypes.Add(exceptionType1);

            MockRegistryKey userPoliciesKey = new MockRegistryKey(false);
            userKey.AddSubKey(ExceptionHandlingSettingsManageabilityProvider.PoliciesKeyName, userPoliciesKey);
            MockRegistryKey userPolicy1Key = new MockRegistryKey(false);
            userPoliciesKey.AddSubKey("policy2", userPolicy1Key);
            MockRegistryKey userPolicy1TypesKey = new MockRegistryKey(false);
            userPolicy1Key.AddSubKey(ExceptionHandlingSettingsManageabilityProvider.PolicyTypesPropertyName, userPolicy1TypesKey);
            MockRegistryKey userPolicy1Type1Key = new MockRegistryKey(false);
            userPolicy1TypesKey.AddSubKey("type1", userPolicy1Type1Key);
            userPolicy1Type1Key.AddStringValue(
                ExceptionHandlingSettingsManageabilityProvider.PolicyTypePostHandlingActionPropertyName,
                PostHandlingAction.NotifyRethrow.ToString());

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, null, userKey, false, wmiSettings);

            Assert.AreEqual(PostHandlingAction.None, exceptionType1.PostHandlingAction);

            Assert.IsTrue(
                MockRegistryKey.CheckAllClosed(userPoliciesKey, userPolicy1Key, userPolicy1TypesKey, userPolicy1Type1Key));
        }

        [TestMethod]
        public void CanApplyUserPolicyOverridesToExistingPolicyTyqpeWhenMultiplePolicyOverridesExist()
        {
            ExceptionPolicyData policy1 = new ExceptionPolicyData("policy1");
            section.ExceptionPolicies.Add(policy1);
            ExceptionTypeData exceptionType1 = new ExceptionTypeData("type1", typeof(Exception), PostHandlingAction.None);
            policy1.ExceptionTypes.Add(exceptionType1);

            MockRegistryKey userPoliciesKey = new MockRegistryKey(false);
            userKey.AddSubKey(ExceptionHandlingSettingsManageabilityProvider.PoliciesKeyName, userPoliciesKey);
            MockRegistryKey userPolicy0Key = new MockRegistryKey(false);
            userPoliciesKey.AddSubKey("policy0", userPolicy0Key);
            MockRegistryKey userPolicy0TypesKey = new MockRegistryKey(false);
            userPolicy0Key.AddSubKey(ExceptionHandlingSettingsManageabilityProvider.PolicyTypesPropertyName, userPolicy0TypesKey);
            MockRegistryKey userPolicy0Type1Key = new MockRegistryKey(false);
            userPolicy0TypesKey.AddSubKey("type1", userPolicy0Type1Key);
            userPolicy0Type1Key.AddStringValue(
                ExceptionHandlingSettingsManageabilityProvider.PolicyTypePostHandlingActionPropertyName,
                PostHandlingAction.ThrowNewException.ToString());
            MockRegistryKey userPolicy1Key = new MockRegistryKey(false);
            userPoliciesKey.AddSubKey("policy1", userPolicy1Key);
            MockRegistryKey userPolicy1TypesKey = new MockRegistryKey(false);
            userPolicy1Key.AddSubKey(ExceptionHandlingSettingsManageabilityProvider.PolicyTypesPropertyName, userPolicy1TypesKey);
            MockRegistryKey userPolicy1Type1Key = new MockRegistryKey(false);
            userPolicy1TypesKey.AddSubKey("type1", userPolicy1Type1Key);
            userPolicy1Type1Key.AddStringValue(
                ExceptionHandlingSettingsManageabilityProvider.PolicyTypePostHandlingActionPropertyName,
                PostHandlingAction.NotifyRethrow.ToString());

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, null, userKey, false, wmiSettings);

            Assert.AreEqual(PostHandlingAction.NotifyRethrow, exceptionType1.PostHandlingAction);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(userPoliciesKey,
                                                         userPolicy0Key, userPolicy0TypesKey, userPolicy0Type1Key,
                                                         userPolicy1Key, userPolicy1TypesKey, userPolicy1Type1Key));
        }

        [TestMethod]
        public void ExceptionTypeWithDisabledPolicyIsRemoved()
        {
            ExceptionPolicyData policy1 = new ExceptionPolicyData("policy1");
            section.ExceptionPolicies.Add(policy1);
            ExceptionTypeData exceptionType11 = new ExceptionTypeData("type11", typeof(Exception), PostHandlingAction.None);
            policy1.ExceptionTypes.Add(exceptionType11);
            ExceptionTypeData exceptionType12 = new ExceptionTypeData("type12", typeof(Exception), PostHandlingAction.None);
            policy1.ExceptionTypes.Add(exceptionType12);
            ExceptionPolicyData policy2 = new ExceptionPolicyData("policy2");
            section.ExceptionPolicies.Add(policy2);
            ExceptionTypeData exceptionType21 = new ExceptionTypeData("type21", typeof(Exception), PostHandlingAction.None);
            policy2.ExceptionTypes.Add(exceptionType21);

            MockRegistryKey userPoliciesKey = new MockRegistryKey(false);
            userKey.AddSubKey(ExceptionHandlingSettingsManageabilityProvider.PoliciesKeyName, userPoliciesKey);
            MockRegistryKey userPolicy1Key = new MockRegistryKey(false);
            userPoliciesKey.AddSubKey("policy1", userPolicy1Key);
            MockRegistryKey userPolicy1TypesKey = new MockRegistryKey(false);
            userPolicy1Key.AddSubKey(ExceptionHandlingSettingsManageabilityProvider.PolicyTypesPropertyName, userPolicy1TypesKey);
            MockRegistryKey userPolicy1Type11Key = new MockRegistryKey(false);
            userPolicy1TypesKey.AddSubKey("type11", userPolicy1Type11Key);
            userPolicy1Type11Key.AddBooleanValue(ExceptionHandlingSettingsManageabilityProvider.PolicyValueName, false);
            userPolicy1Type11Key.AddEnumValue(
                ExceptionHandlingSettingsManageabilityProvider.PolicyTypePostHandlingActionPropertyName, PostHandlingAction.None);
            MockRegistryKey userPolicy1Type12Key = new MockRegistryKey(false);
            userPolicy1TypesKey.AddSubKey("type12", userPolicy1Type12Key);
            userPolicy1Type12Key.AddBooleanValue(ExceptionHandlingSettingsManageabilityProvider.PolicyValueName, true);
            userPolicy1Type12Key.AddEnumValue(
                ExceptionHandlingSettingsManageabilityProvider.PolicyTypePostHandlingActionPropertyName, PostHandlingAction.None);
            MockRegistryKey userPolicy2Key = new MockRegistryKey(false);
            userPoliciesKey.AddSubKey("policy2", userPolicy2Key);
            MockRegistryKey userPolicy2TypesKey = new MockRegistryKey(false);
            userPolicy2Key.AddSubKey(ExceptionHandlingSettingsManageabilityProvider.PolicyTypesPropertyName, userPolicy2TypesKey);
            MockRegistryKey userPolicy2Type21Key = new MockRegistryKey(false);
            userPolicy2TypesKey.AddSubKey("type21", userPolicy2Type21Key);
            userPolicy2Type21Key.AddBooleanValue(ExceptionHandlingSettingsManageabilityProvider.PolicyValueName, false);
            userPolicy2Type21Key.AddEnumValue(
                ExceptionHandlingSettingsManageabilityProvider.PolicyTypePostHandlingActionPropertyName, PostHandlingAction.None);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(2, section.ExceptionPolicies.Count);
            Assert.IsNotNull(section.ExceptionPolicies.Get("policy1"));
            Assert.AreEqual(1, section.ExceptionPolicies.Get("policy1").ExceptionTypes.Count);
            Assert.IsNotNull(section.ExceptionPolicies.Get("policy1").ExceptionTypes.Get("type12"));
            Assert.IsNotNull(section.ExceptionPolicies.Get("policy2"));
            Assert.AreEqual(0, section.ExceptionPolicies.Get("policy2").ExceptionTypes.Count);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(userPoliciesKey,
                                                         userPolicy1Key, userPolicy1TypesKey, userPolicy1Type11Key,
                                                         userPolicy1Type12Key,
                                                         userPolicy2Key, userPolicy2TypesKey, userPolicy2Type21Key));
        }

        [TestMethod]
        public void ExceptionTypeWithDisabledPolicyIsNotRemovedIfGroupPoliciesAreDisabled()
        {
            ExceptionPolicyData policy1 = new ExceptionPolicyData("policy1");
            section.ExceptionPolicies.Add(policy1);
            ExceptionTypeData exceptionType11 = new ExceptionTypeData("type11", typeof(Exception), PostHandlingAction.None);
            policy1.ExceptionTypes.Add(exceptionType11);
            ExceptionTypeData exceptionType12 = new ExceptionTypeData("type12", typeof(Exception), PostHandlingAction.None);
            policy1.ExceptionTypes.Add(exceptionType12);
            ExceptionPolicyData policy2 = new ExceptionPolicyData("policy2");
            section.ExceptionPolicies.Add(policy2);
            ExceptionTypeData exceptionType21 = new ExceptionTypeData("type21", typeof(Exception), PostHandlingAction.None);
            policy2.ExceptionTypes.Add(exceptionType21);

            MockRegistryKey userPoliciesKey = new MockRegistryKey(false);
            userKey.AddSubKey(ExceptionHandlingSettingsManageabilityProvider.PoliciesKeyName, userPoliciesKey);
            MockRegistryKey userPolicy1Key = new MockRegistryKey(false);
            userPoliciesKey.AddSubKey("policy1", userPolicy1Key);
            MockRegistryKey userPolicy1TypesKey = new MockRegistryKey(false);
            userPolicy1Key.AddSubKey(ExceptionHandlingSettingsManageabilityProvider.PolicyTypesPropertyName, userPolicy1TypesKey);
            MockRegistryKey userPolicy1Type11Key = new MockRegistryKey(false);
            userPolicy1TypesKey.AddSubKey("type11", userPolicy1Type11Key);
            userPolicy1Type11Key.AddBooleanValue(ExceptionHandlingSettingsManageabilityProvider.PolicyValueName, false);
            MockRegistryKey userPolicy1Type12Key = new MockRegistryKey(false);
            userPolicy1TypesKey.AddSubKey("type12", userPolicy1Type12Key);
            userPolicy1Type12Key.AddBooleanValue(ExceptionHandlingSettingsManageabilityProvider.PolicyValueName, true);
            MockRegistryKey userPolicy2Key = new MockRegistryKey(false);
            userPoliciesKey.AddSubKey("policy2", userPolicy2Key);
            MockRegistryKey userPolicy2TypesKey = new MockRegistryKey(false);
            userPolicy2Key.AddSubKey(ExceptionHandlingSettingsManageabilityProvider.PolicyTypesPropertyName, userPolicy2TypesKey);
            MockRegistryKey userPolicy2Type21Key = new MockRegistryKey(false);
            userPolicy2TypesKey.AddSubKey("type21", userPolicy2Type21Key);
            userPolicy2Type21Key.AddBooleanValue(ExceptionHandlingSettingsManageabilityProvider.PolicyValueName, false);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(2, section.ExceptionPolicies.Count);
            Assert.IsNotNull(section.ExceptionPolicies.Get("policy1"));
            Assert.AreEqual(2, section.ExceptionPolicies.Get("policy1").ExceptionTypes.Count);
            Assert.IsNotNull(section.ExceptionPolicies.Get("policy1").ExceptionTypes.Get("type11"));
            Assert.IsNotNull(section.ExceptionPolicies.Get("policy1").ExceptionTypes.Get("type12"));
            Assert.IsNotNull(section.ExceptionPolicies.Get("policy2"));
            Assert.AreEqual(1, section.ExceptionPolicies.Get("policy2").ExceptionTypes.Count);
            Assert.IsNotNull(section.ExceptionPolicies.Get("policy2").ExceptionTypes.Get("type21"));

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(userPoliciesKey,
                                                         userPolicy1Key, userPolicy1TypesKey, userPolicy1Type11Key,
                                                         userPolicy1Type12Key,
                                                         userPolicy2Key, userPolicy2TypesKey, userPolicy2Type21Key));
        }

        [TestMethod]
        public void RegisteredHandlerDataProviderIsCalledWithNoOverrides()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(ReplaceHandlerData), registeredProvider);
            provider = new ExceptionHandlingSettingsManageabilityProvider(subProviders);

            ExceptionPolicyData policy1 = new ExceptionPolicyData("policy1");
            section.ExceptionPolicies.Add(policy1);
            ExceptionTypeData exceptionType1 = new ExceptionTypeData("type1", typeof(Exception), PostHandlingAction.None);
            policy1.ExceptionTypes.Add(exceptionType1);
            ExceptionHandlerData handlerData1 =
                new ReplaceHandlerData("handler1", "msg", typeof(ArgumentException).AssemblyQualifiedName);
            exceptionType1.ExceptionHandlers.Add(handlerData1);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, false, wmiSettings);

            Assert.IsTrue(registeredProvider.called);
            Assert.AreSame(handlerData1, registeredProvider.LastConfigurationObject);
            Assert.AreEqual(null, registeredProvider.machineKey);
            Assert.AreEqual(null, registeredProvider.userKey);
        }

        [TestMethod]
        public void RegisteredHandlerDataProviderIsCalledWithCorrectOverrides()
        {
            MockConfigurationElementManageabilityProvider registeredProvider
                = new MockConfigurationElementManageabilityProvider();
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(ReplaceHandlerData), registeredProvider);
            provider = new ExceptionHandlingSettingsManageabilityProvider(subProviders);

            ExceptionPolicyData policy1 = new ExceptionPolicyData("policy1");
            section.ExceptionPolicies.Add(policy1);
            ExceptionTypeData exceptionType1 = new ExceptionTypeData("type1", typeof(Exception), PostHandlingAction.None);
            policy1.ExceptionTypes.Add(exceptionType1);
            ExceptionHandlerData handlerData1 =
                new ReplaceHandlerData("handler1", "msg", typeof(ArgumentException).AssemblyQualifiedName);
            exceptionType1.ExceptionHandlers.Add(handlerData1);

            MockRegistryKey machinePoliciesKey = new MockRegistryKey(false);
            machineKey.AddSubKey(ExceptionHandlingSettingsManageabilityProvider.PoliciesKeyName, machinePoliciesKey);
            MockRegistryKey machinePolicy1Key = new MockRegistryKey(false);
            machinePoliciesKey.AddSubKey("policy1", machinePolicy1Key);
            MockRegistryKey machinePolicy1TypesKey = new MockRegistryKey(false);
            machinePolicy1Key.AddSubKey(ExceptionHandlingSettingsManageabilityProvider.PolicyTypesPropertyName,
                                        machinePolicy1TypesKey);
            MockRegistryKey machinePolicy1Type1Key = new MockRegistryKey(false);
            machinePolicy1TypesKey.AddSubKey("type1", machinePolicy1Type1Key);
            MockRegistryKey machineHandlersKey = new MockRegistryKey(false);
            machinePolicy1Type1Key.AddSubKey(ExceptionHandlingSettingsManageabilityProvider.PolicyTypeHandlersPropertyName,
                                             machineHandlersKey);
            machinePolicy1Type1Key.AddEnumValue(
                ExceptionHandlingSettingsManageabilityProvider.PolicyTypePostHandlingActionPropertyName, PostHandlingAction.None);
            MockRegistryKey machineHandlerKey = new MockRegistryKey(false);
            machineHandlersKey.AddSubKey("handler1", machineHandlerKey);

            MockRegistryKey userPoliciesKey = new MockRegistryKey(false);
            userKey.AddSubKey(ExceptionHandlingSettingsManageabilityProvider.PoliciesKeyName, userPoliciesKey);
            MockRegistryKey userPolicy1Key = new MockRegistryKey(false);
            userPoliciesKey.AddSubKey("policy1", userPolicy1Key);
            MockRegistryKey userPolicy1TypesKey = new MockRegistryKey(false);
            userPolicy1Key.AddSubKey(ExceptionHandlingSettingsManageabilityProvider.PolicyTypesPropertyName, userPolicy1TypesKey);
            MockRegistryKey userPolicy1Type1Key = new MockRegistryKey(false);
            userPolicy1TypesKey.AddSubKey("type1", userPolicy1Type1Key);
            MockRegistryKey userHandlersKey = new MockRegistryKey(false);
            userPolicy1Type1Key.AddSubKey(ExceptionHandlingSettingsManageabilityProvider.PolicyTypeHandlersPropertyName,
                                          userHandlersKey);
            userPolicy1Type1Key.AddEnumValue(
                ExceptionHandlingSettingsManageabilityProvider.PolicyTypePostHandlingActionPropertyName, PostHandlingAction.None);
            MockRegistryKey userHandlerKey = new MockRegistryKey(false);
            userHandlersKey.AddSubKey("handler1", userHandlerKey);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, true, machineKey, userKey, false, wmiSettings);

            Assert.IsTrue(registeredProvider.called);
            Assert.AreSame(handlerData1, registeredProvider.LastConfigurationObject);
            Assert.AreSame(machineHandlerKey, registeredProvider.machineKey);
            Assert.AreSame(userHandlerKey, registeredProvider.userKey);

            Assert.IsTrue(MockRegistryKey.CheckAllClosed(machinePoliciesKey,
                                                         machinePolicy1Key, machinePolicy1TypesKey, machinePolicy1Type1Key,
                                                         machineHandlersKey, machineHandlerKey,
                                                         userPolicy1Key, userPolicy1TypesKey, userPolicy1Type1Key,
                                                         userHandlersKey, userHandlerKey));
        }

        [TestMethod]
        public void WmiSettingsAreNotGeneratedIfWmiIsDisabled()
        {
            ExceptionPolicyData policy1 = new ExceptionPolicyData("policy1");
            section.ExceptionPolicies.Add(policy1);
            ExceptionTypeData exceptionType1 = new ExceptionTypeData("type1", typeof(Exception), PostHandlingAction.None);
            policy1.ExceptionTypes.Add(exceptionType1);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, false, wmiSettings);

            Assert.AreEqual(0, wmiSettings.Count);
        }

        [TestMethod]
        public void WmiSettingsForPolicyAreGeneratedIfWmiIsEnabled()
        {
            ExceptionPolicyData policy1 = new ExceptionPolicyData("policy1");
            section.ExceptionPolicies.Add(policy1);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(ExceptionPolicySetting), wmiSettings[0].GetType());
            Assert.AreEqual("policy1", ((ExceptionPolicySetting)wmiSettings[0]).Name);
        }

        [TestMethod]
        public void WmiSettingsForExceptionTypeAreGeneratedIfWmiIsEnabled()
        {
            ExceptionPolicyData policy1 = new ExceptionPolicyData("policy1");
            section.ExceptionPolicies.Add(policy1);
            ExceptionTypeData exceptionType1 = new ExceptionTypeData("type1", typeof(Exception), PostHandlingAction.None);
            policy1.ExceptionTypes.Add(exceptionType1);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, true, wmiSettings);

            // TODO change so that order is not important
            Assert.AreEqual(2, wmiSettings.Count);
            Assert.AreSame(typeof(ExceptionTypeSetting), wmiSettings[0].GetType());
            Assert.AreEqual("type1", ((ExceptionTypeSetting)wmiSettings[0]).Name);
            Assert.AreEqual(typeof(Exception).AssemblyQualifiedName, ((ExceptionTypeSetting)wmiSettings[0]).ExceptionTypeName);
            Assert.AreEqual(PostHandlingAction.None.ToString(), ((ExceptionTypeSetting)wmiSettings[0]).PostHandlingAction);
            Assert.AreEqual("policy1", ((ExceptionTypeSetting)wmiSettings[0]).Policy);
            Assert.AreSame(typeof(ExceptionPolicySetting), wmiSettings[1].GetType());
            Assert.AreEqual("policy1", ((ExceptionPolicySetting)wmiSettings[1]).Name);
        }

        [TestMethod]
        public void WmiSettingsForExceptionHandlerAreGeneratedWithCorrectPolicyAndTypeIfWmiIsEnabled()
        {
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(ReplaceHandlerData), new ReplaceHandlerDataManageabilityProvider());
            provider = new ExceptionHandlingSettingsManageabilityProvider(subProviders);

            ExceptionPolicyData policy1 = new ExceptionPolicyData("policy1");
            section.ExceptionPolicies.Add(policy1);
            ExceptionTypeData exceptionType1 = new ExceptionTypeData("type1", typeof(Exception), PostHandlingAction.None);
            policy1.ExceptionTypes.Add(exceptionType1);
            ReplaceHandlerData handler1 =
                new ReplaceHandlerData("handler1", "message", typeof(ArgumentNullException).AssemblyQualifiedName);
            exceptionType1.ExceptionHandlers.Add(handler1);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(3, wmiSettings.Count);
            IDictionary<String, ExceptionHandlerSetting> handlerSettings =
                SelectWmiSettings<ExceptionHandlerSetting>(wmiSettings);
            Assert.AreEqual(1, handlerSettings.Count);
            Assert.IsTrue(handlerSettings.ContainsKey("handler1"));
            Assert.AreEqual("message", ((ReplaceHandlerSetting)handlerSettings["handler1"]).ExceptionMessage);
            Assert.AreEqual(typeof(ArgumentNullException).AssemblyQualifiedName,
                            ((ReplaceHandlerSetting)handlerSettings["handler1"]).ReplaceExceptionType);
            Assert.AreEqual("policy1", handlerSettings["handler1"].Policy);
            Assert.AreEqual("type1", handlerSettings["handler1"].ExceptionType);
            Assert.AreEqual(0, handlerSettings["handler1"].Order);
        }

        [TestMethod]
        public void WmiSettingsForMultipleExceptionHandlersAreGeneratedWithCorrectPolicyAndTypeIfWmiIsEnabled()
        {
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(ReplaceHandlerData), new ReplaceHandlerDataManageabilityProvider());
            provider = new ExceptionHandlingSettingsManageabilityProvider(subProviders);

            ExceptionPolicyData policy1 = new ExceptionPolicyData("policy1");
            section.ExceptionPolicies.Add(policy1);
            ExceptionTypeData exceptionType1 = new ExceptionTypeData("type1", typeof(Exception), PostHandlingAction.None);
            policy1.ExceptionTypes.Add(exceptionType1);
            ReplaceHandlerData handler1 =
                new ReplaceHandlerData("handler1", "message", typeof(ArgumentNullException).AssemblyQualifiedName);
            exceptionType1.ExceptionHandlers.Add(handler1);
            ReplaceHandlerData handler2 =
                new ReplaceHandlerData("handler2", "message2", typeof(ArgumentException).AssemblyQualifiedName);
            exceptionType1.ExceptionHandlers.Add(handler2);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(4, wmiSettings.Count);
            IDictionary<String, ExceptionHandlerSetting> handlerSettings =
                SelectWmiSettings<ExceptionHandlerSetting>(wmiSettings);
            Assert.AreEqual(2, handlerSettings.Count);

            Assert.IsTrue(handlerSettings.ContainsKey("handler1"));
            Assert.AreEqual("message", ((ReplaceHandlerSetting)handlerSettings["handler1"]).ExceptionMessage);
            Assert.AreEqual(typeof(ArgumentNullException).AssemblyQualifiedName,
                            ((ReplaceHandlerSetting)handlerSettings["handler1"]).ReplaceExceptionType);
            Assert.AreEqual("policy1", handlerSettings["handler1"].Policy);
            Assert.AreEqual("type1", handlerSettings["handler1"].ExceptionType);
            Assert.AreEqual(0, handlerSettings["handler1"].Order);

            Assert.IsTrue(handlerSettings.ContainsKey("handler2"));
            Assert.AreEqual("message2", ((ReplaceHandlerSetting)handlerSettings["handler2"]).ExceptionMessage);
            Assert.AreEqual(typeof(ArgumentException).AssemblyQualifiedName,
                            ((ReplaceHandlerSetting)handlerSettings["handler2"]).ReplaceExceptionType);
            Assert.AreEqual("policy1", handlerSettings["handler2"].Policy);
            Assert.AreEqual("type1", handlerSettings["handler2"].ExceptionType);
            Assert.AreEqual(1, handlerSettings["handler2"].Order);
        }

        [TestMethod]
        public void WmiSettingsForMultipleExceptionHandlersOnDifferentTypesAreGeneratedWithCorrectPolicyAndTypeIfWmiIsEnabled()
        {
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(ReplaceHandlerData), new ReplaceHandlerDataManageabilityProvider());
            provider = new ExceptionHandlingSettingsManageabilityProvider(subProviders);

            ExceptionPolicyData policy1 = new ExceptionPolicyData("policy1");
            section.ExceptionPolicies.Add(policy1);
            ExceptionTypeData exceptionType1 = new ExceptionTypeData("type1", typeof(Exception), PostHandlingAction.None);
            policy1.ExceptionTypes.Add(exceptionType1);
            ReplaceHandlerData handler1 =
                new ReplaceHandlerData("handler1", "message", typeof(ArgumentNullException).AssemblyQualifiedName);
            exceptionType1.ExceptionHandlers.Add(handler1);
            ExceptionTypeData exceptionType2 =
                new ExceptionTypeData("type2", typeof(NullReferenceException), PostHandlingAction.None);
            policy1.ExceptionTypes.Add(exceptionType2);
            ReplaceHandlerData handler2 =
                new ReplaceHandlerData("handler2", "message2", typeof(ArgumentException).AssemblyQualifiedName);
            exceptionType2.ExceptionHandlers.Add(handler2);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(section, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(5, wmiSettings.Count);
            IDictionary<String, ExceptionHandlerSetting> handlerSettings =
                SelectWmiSettings<ExceptionHandlerSetting>(wmiSettings);
            Assert.AreEqual(2, handlerSettings.Count);

            Assert.IsTrue(handlerSettings.ContainsKey("handler1"));
            Assert.AreEqual("message", ((ReplaceHandlerSetting)handlerSettings["handler1"]).ExceptionMessage);
            Assert.AreEqual(typeof(ArgumentNullException).AssemblyQualifiedName,
                            ((ReplaceHandlerSetting)handlerSettings["handler1"]).ReplaceExceptionType);
            Assert.AreEqual("policy1", handlerSettings["handler1"].Policy);
            Assert.AreEqual("type1", handlerSettings["handler1"].ExceptionType);
            Assert.AreEqual(0, handlerSettings["handler1"].Order);

            Assert.IsTrue(handlerSettings.ContainsKey("handler2"));
            Assert.AreEqual("message2", ((ReplaceHandlerSetting)handlerSettings["handler2"]).ExceptionMessage);
            Assert.AreEqual(typeof(ArgumentException).AssemblyQualifiedName,
                            ((ReplaceHandlerSetting)handlerSettings["handler2"]).ReplaceExceptionType);
            Assert.AreEqual("policy1", handlerSettings["handler2"].Policy);
            Assert.AreEqual("type2", handlerSettings["handler2"].ExceptionType);
            Assert.AreEqual(0, handlerSettings["handler2"].Order);
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContent()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(ExceptionHandlingSettings.SectionName, section);

            ExceptionPolicyData policy1 = new ExceptionPolicyData("policy1");
            section.ExceptionPolicies.Add(policy1);
            ExceptionTypeData exceptionType11 = new ExceptionTypeData("type11", typeof(Exception), PostHandlingAction.None);
            policy1.ExceptionTypes.Add(exceptionType11);
            ExceptionTypeData exceptionType12 = new ExceptionTypeData("type12", typeof(Exception), PostHandlingAction.None);
            policy1.ExceptionTypes.Add(exceptionType12);
            ExceptionPolicyData policy2 = new ExceptionPolicyData("policy2");
            section.ExceptionPolicies.Add(policy2);
            ExceptionTypeData exceptionType21 = new ExceptionTypeData("type21", typeof(Exception), PostHandlingAction.None);
            policy2.ExceptionTypes.Add(exceptionType21);

            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            provider = new ExceptionHandlingSettingsManageabilityProvider(subProviders);

            MockAdmContentBuilder contentBuilder = new MockAdmContentBuilder();

            provider.AddAdministrativeTemplateDirectives(contentBuilder, section, configurationSource, "TestApp");

            MockAdmContent content = contentBuilder.GetMockContent();
            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            Assert.IsTrue(categoriesEnumerator.MoveNext());
            IEnumerator<AdmCategory> subCategoriesEnumerator = categoriesEnumerator.Current.Categories.GetEnumerator();

            Assert.IsTrue(subCategoriesEnumerator.MoveNext());
            Assert.AreEqual(policy1.Name, subCategoriesEnumerator.Current.Name);
            IEnumerator<AdmPolicy> policyPoliciesEnumerator = subCategoriesEnumerator.Current.Policies.GetEnumerator();
            Assert.IsTrue(policyPoliciesEnumerator.MoveNext());
            IEnumerator<AdmPart> partsEnumerator = policyPoliciesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(ExceptionHandlingSettingsManageabilityProvider.PolicyTypePostHandlingActionPropertyName,
                            partsEnumerator.Current.ValueName);
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmTextPart), partsEnumerator.Current.GetType());
            Assert.IsFalse(partsEnumerator.MoveNext());
            Assert.IsTrue(policyPoliciesEnumerator.MoveNext());
            Assert.IsFalse(policyPoliciesEnumerator.MoveNext());

            Assert.IsTrue(subCategoriesEnumerator.MoveNext());
            Assert.AreEqual(policy2.Name, subCategoriesEnumerator.Current.Name);
            policyPoliciesEnumerator = subCategoriesEnumerator.Current.Policies.GetEnumerator();
            Assert.IsTrue(policyPoliciesEnumerator.MoveNext());
            Assert.IsFalse(policyPoliciesEnumerator.MoveNext());

            Assert.IsFalse(subCategoriesEnumerator.MoveNext());
            IEnumerator<AdmPolicy> sectionPoliciesEnumerator = categoriesEnumerator.Current.Policies.GetEnumerator();
            Assert.IsTrue(sectionPoliciesEnumerator.MoveNext());
            partsEnumerator = sectionPoliciesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsFalse(partsEnumerator.MoveNext());
            Assert.IsFalse(sectionPoliciesEnumerator.MoveNext());
            Assert.IsFalse(categoriesEnumerator.MoveNext());
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContentWithRegisteredProviders()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(ExceptionHandlingSettings.SectionName, section);

            ExceptionPolicyData policy1 = new ExceptionPolicyData("policy1");
            section.ExceptionPolicies.Add(policy1);
            ExceptionTypeData exceptionType11 = new ExceptionTypeData("type11", typeof(Exception), PostHandlingAction.None);
            policy1.ExceptionTypes.Add(exceptionType11);
            ExceptionHandlerData handler11 = new ExceptionHandlerData("handler11", typeof(object));
            exceptionType11.ExceptionHandlers.Add(handler11);

            MockConfigurationElementManageabilityProvider subProvider =
                new MockConfigurationElementManageabilityProvider(false, true);
            Dictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
            subProviders.Add(typeof(ExceptionHandlerData), subProvider);
            provider = new ExceptionHandlingSettingsManageabilityProvider(subProviders);

            MockAdmContentBuilder contentBuilder = new MockAdmContentBuilder();

            provider.AddAdministrativeTemplateDirectives(contentBuilder, section, configurationSource, "TestApp");

            MockAdmContent content = contentBuilder.GetMockContent();
            IEnumerator<AdmCategory> categoriesEnumerator = content.Categories.GetEnumerator();
            Assert.IsTrue(categoriesEnumerator.MoveNext());
            IEnumerator<AdmCategory> subCategoriesEnumerator = categoriesEnumerator.Current.Categories.GetEnumerator();

            Assert.IsTrue(subCategoriesEnumerator.MoveNext());
            Assert.AreEqual(policy1.Name, subCategoriesEnumerator.Current.Name);
            IEnumerator<AdmPolicy> policyPoliciesEnumerator = subCategoriesEnumerator.Current.Policies.GetEnumerator();
            Assert.IsTrue(policyPoliciesEnumerator.MoveNext());
            IEnumerator<AdmPart> partsEnumerator = policyPoliciesEnumerator.Current.Parts.GetEnumerator();
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(ExceptionHandlingSettingsManageabilityProvider.PolicyTypePostHandlingActionPropertyName,
                            partsEnumerator.Current.ValueName);
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmTextPart), partsEnumerator.Current.GetType());
            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmTextPart), partsEnumerator.Current.GetType());
            Assert.AreEqual(MockConfigurationElementManageabilityProvider.Part, partsEnumerator.Current.PartName);

            Assert.IsFalse(partsEnumerator.MoveNext());
            Assert.IsFalse(policyPoliciesEnumerator.MoveNext());

            Assert.IsFalse(subCategoriesEnumerator.MoveNext());
            Assert.IsFalse(categoriesEnumerator.MoveNext());
        }

        static IDictionary<String, T> SelectWmiSettings<T>(IEnumerable<ConfigurationSetting> wmiSettings)
            where T : ExceptionHandlerSetting
        {
            Dictionary<String, T> result = new Dictionary<string, T>();

            foreach (ConfigurationSetting setting in wmiSettings)
            {
                T namedSetting = setting as T;
                if (namedSetting != null)
                {
                    result.Add(namedSetting.Name, namedSetting);
                }
            }

            return result;
        }
    }
}
