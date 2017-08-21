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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Tests.TraceListeners
{
    [TestClass]
    public class CustomTraceListenerDataManageabilityProviderFixture
    {
        CustomTraceListenerDataManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        IList<ConfigurationSetting> wmiSettings;
        CustomTraceListenerData configurationObject;

        [TestInitialize]
        public void SetUp()
        {
            provider = new CustomTraceListenerDataManageabilityProvider();
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            wmiSettings = new List<ConfigurationSetting>();
            configurationObject = new CustomTraceListenerData();
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

            Assembly assembly = typeof(CustomTraceListenerDataManageabilityProvider).Assembly;
            foreach (ConfigurationElementManageabilityProviderAttribute providerAttribute
                in assembly.GetCustomAttributes(typeof(ConfigurationElementManageabilityProviderAttribute), false))
            {
                if (providerAttribute.ManageabilityProviderType.Equals(typeof(CustomTraceListenerDataManageabilityProvider)))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(LoggingSettingsManageabilityProvider), selectedAttribute.SectionManageabilityProviderType);
            Assert.AreSame(typeof(CustomTraceListenerData), selectedAttribute.TargetType);
        }

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfThereAreNoPolicyOverrides()
        {
            // no need to test for attributes, it's tested for parent class
            configurationObject.Type = typeof(Object);
            configurationObject.Formatter = "formatter";
            configurationObject.InitData = "init data";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, null, true, wmiSettings);

            Assert.AreSame(typeof(Object), configurationObject.Type);
            Assert.AreEqual("formatter", configurationObject.Formatter);
            Assert.AreEqual("init data", configurationObject.InitData);
            Assert.AreEqual(TraceOptions.None, configurationObject.TraceOutputOptions);
			Assert.AreEqual(SourceLevels.Error, configurationObject.Filter);
		}

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreMachinePolicyOverrides()
        {
            // no need to test for attributes, it's tested for parent class
            configurationObject.Type = typeof(Object);
            configurationObject.Formatter = "formatter";
            configurationObject.InitData = "init data";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            machineKey.AddStringValue(CustomTraceListenerDataManageabilityProvider.ProviderTypePropertyName, typeof(Object).AssemblyQualifiedName);
            machineKey.AddStringValue(CustomTraceListenerDataManageabilityProvider.AttributesPropertyName, "");
            machineKey.AddStringValue(CustomTraceListenerDataManageabilityProvider.FormatterPropertyName, "overriden formatter");
            machineKey.AddStringValue(CustomTraceListenerDataManageabilityProvider.InitDataPropertyName, "overriden init data");
            machineKey.AddStringValue(CustomTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, "ProcessId, ThreadId");
			machineKey.AddStringValue(CustomTraceListenerDataManageabilityProvider.FilterPropertyName, "Critical");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, null, true, wmiSettings);

            Assert.AreEqual("overriden formatter", configurationObject.Formatter);
            Assert.AreEqual("overriden init data", configurationObject.InitData);
            Assert.AreEqual(TraceOptions.ProcessId | TraceOptions.ThreadId, configurationObject.TraceOutputOptions);
			Assert.AreEqual(SourceLevels.Critical, configurationObject.Filter);
		}

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreUserPolicyOverrides()
        {
            // no need to test for attributes, it's tested for parent class
            configurationObject.Type = typeof(Object);
            configurationObject.Formatter = "formatter";
            configurationObject.InitData = "init data";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            userKey.AddStringValue(CustomTraceListenerDataManageabilityProvider.ProviderTypePropertyName, typeof(Object).AssemblyQualifiedName);
            userKey.AddStringValue(CustomTraceListenerDataManageabilityProvider.AttributesPropertyName, "");
            userKey.AddStringValue(CustomTraceListenerDataManageabilityProvider.FormatterPropertyName, "overriden formatter");
            userKey.AddStringValue(CustomTraceListenerDataManageabilityProvider.InitDataPropertyName, "overriden init data");
            userKey.AddStringValue(CustomTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, "ProcessId, ThreadId");
			userKey.AddStringValue(CustomTraceListenerDataManageabilityProvider.FilterPropertyName, "Critical");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, userKey, true, wmiSettings);

            Assert.AreEqual("overriden formatter", configurationObject.Formatter);
            Assert.AreEqual("overriden init data", configurationObject.InitData);
            Assert.AreEqual(TraceOptions.ProcessId | TraceOptions.ThreadId, configurationObject.TraceOutputOptions);
			Assert.AreEqual(SourceLevels.Critical, configurationObject.Filter);
		}

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfThereArePolicyOverridesButGroupPoliciesAreDisabled()
        {
            // no need to test for attributes, it's tested for parent class
            configurationObject.Type = typeof(Object);
            configurationObject.Formatter = "formatter";
            configurationObject.InitData = "init data";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            machineKey.AddStringValue(CustomTraceListenerDataManageabilityProvider.FormatterPropertyName, "overriden formatter");
            machineKey.AddStringValue(CustomTraceListenerDataManageabilityProvider.InitDataPropertyName, "overriden init data");
            machineKey.AddStringValue(CustomTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, "ProcessId, ThreadId");
			machineKey.AddStringValue(CustomTraceListenerDataManageabilityProvider.FilterPropertyName, "Critical");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, userKey, true, wmiSettings);

            Assert.AreSame(typeof(Object), configurationObject.Type);
            Assert.AreEqual("formatter", configurationObject.Formatter);
            Assert.AreEqual("init data", configurationObject.InitData);
            Assert.AreEqual(TraceOptions.None, configurationObject.TraceOutputOptions);
			Assert.AreEqual(SourceLevels.Error, configurationObject.Filter);
		}

        [TestMethod]
        public void ConfigurationObjectIsModifiedWithFormatterOverrideWithListItemNone()
        {
            configurationObject.Type = typeof(Object);
            configurationObject.Formatter = "formatter";

            machineKey.AddStringValue(CustomTraceListenerDataManageabilityProvider.ProviderTypePropertyName, typeof(Object).AssemblyQualifiedName);
            machineKey.AddStringValue(CustomTraceListenerDataManageabilityProvider.AttributesPropertyName, "");
            machineKey.AddStringValue(CustomTraceListenerDataManageabilityProvider.InitDataPropertyName, "overriden init data");
            machineKey.AddStringValue(CustomTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, "ProcessId, ThreadId");
			machineKey.AddStringValue(CustomTraceListenerDataManageabilityProvider.FilterPropertyName, "Critical");
			machineKey.AddStringValue(CustomTraceListenerDataManageabilityProvider.FormatterPropertyName, AdmContentBuilder.NoneListItem);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual("", configurationObject.Formatter);
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedIfWmiIsEnabled()
        {
            configurationObject.Type = typeof(Object);
            configurationObject.Attributes.Add("name1", "value1");
            configurationObject.Attributes.Add("name2", "value2");
            configurationObject.Formatter = "formatter";
            configurationObject.InitData = "init data";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Critical;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(CustomTraceListenerSetting), wmiSettings[0].GetType());
            Assert.AreEqual(typeof(Object).AssemblyQualifiedName, ((CustomTraceListenerSetting)wmiSettings[0]).ListenerType);
            Assert.AreEqual("init data", ((CustomTraceListenerSetting)wmiSettings[0]).InitData);
            Assert.AreEqual("formatter", ((CustomTraceListenerSetting)wmiSettings[0]).Formatter);
            Assert.AreEqual(TraceOptions.None.ToString(), ((CustomTraceListenerSetting)wmiSettings[0]).TraceOutputOptions);
			Assert.AreEqual(SourceLevels.Critical.ToString(), ((CustomTraceListenerSetting)wmiSettings[0]).Filter);

            Dictionary<String, String> attributesDictionary = new Dictionary<string, string>();
            foreach (String entry in ((CustomTraceListenerSetting)wmiSettings[0]).Attributes)
            {
                KeyValuePairParsingTestHelper.ExtractKeyValueEntries(entry, attributesDictionary);
            }
            Assert.AreEqual(2, attributesDictionary.Count);
            Assert.AreEqual("value1", attributesDictionary["name1"]);
            Assert.AreEqual("value2", attributesDictionary["name2"]);
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContent()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            LoggingSettings section = new LoggingSettings();
            configurationSource.Add(LoggingSettings.SectionName, section);

            configurationObject.Type = typeof(object);
            configurationObject.Attributes.Add("name1", "valu;e1");
            configurationObject.Attributes.Add("name2", "value2");

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
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.AreEqual(CustomTraceListenerDataManageabilityProvider.ProviderTypePropertyName,
                            partsEnumerator.Current.ValueName);
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(typeof(object).AssemblyQualifiedName, ((AdmEditTextPart)partsEnumerator.Current).DefaultValue);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.AreEqual(CustomTraceListenerDataManageabilityProvider.AttributesPropertyName,
                            partsEnumerator.Current.ValueName);
            Assert.IsNull(partsEnumerator.Current.KeyName);
            IDictionary<String, String> attributes = new Dictionary<String, String>();
            KeyValuePairParser.ExtractKeyValueEntries(((AdmEditTextPart)partsEnumerator.Current).DefaultValue, attributes);
            Assert.AreEqual(2, attributes.Count);
            Assert.AreEqual("valu;e1", attributes["name1"]);
            Assert.AreEqual("value2", attributes["name2"]);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(CustomTraceListenerDataManageabilityProvider.InitDataPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(CustomTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName,
                            partsEnumerator.Current.ValueName);

			Assert.IsTrue(partsEnumerator.MoveNext());
			Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
			Assert.IsNull(partsEnumerator.Current.KeyName);
			Assert.AreEqual(CustomTraceListenerDataManageabilityProvider.FilterPropertyName,
							partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(CustomTraceListenerDataManageabilityProvider.FormatterPropertyName,
                            partsEnumerator.Current.ValueName);

			Assert.IsFalse(partsEnumerator.MoveNext());
            Assert.IsFalse(policiesEnumerator.MoveNext());
        }
    }
}
