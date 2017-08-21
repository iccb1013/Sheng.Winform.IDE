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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Tests.TraceListeners
{
    [TestClass]
    public class FormattedEventLogTraceListenerDataManageabilityProviderFixture
    {
        FormattedEventLogTraceListenerDataManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        IList<ConfigurationSetting> wmiSettings;
        FormattedEventLogTraceListenerData configurationObject;

        [TestInitialize]
        public void SetUp()
        {
            provider = new FormattedEventLogTraceListenerDataManageabilityProvider();
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            wmiSettings = new List<ConfigurationSetting>();
            configurationObject = new FormattedEventLogTraceListenerData();
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

            Assembly assembly = typeof(FormattedEventLogTraceListenerDataManageabilityProvider).Assembly;
            foreach (ConfigurationElementManageabilityProviderAttribute providerAttribute
                in assembly.GetCustomAttributes(typeof(ConfigurationElementManageabilityProviderAttribute), false))
            {
                if (providerAttribute.ManageabilityProviderType.Equals(typeof(FormattedEventLogTraceListenerDataManageabilityProvider)))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(LoggingSettingsManageabilityProvider), selectedAttribute.SectionManageabilityProviderType);
            Assert.AreSame(typeof(FormattedEventLogTraceListenerData), selectedAttribute.TargetType);
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
            configurationObject.Formatter = "formatter";
            configurationObject.Log = "log";
            configurationObject.MachineName = "machine name";
            configurationObject.Source = "source";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, null, true, wmiSettings);

            Assert.AreEqual("formatter", configurationObject.Formatter);
            Assert.AreEqual("log", configurationObject.Log);
            Assert.AreEqual("machine name", configurationObject.MachineName);
            Assert.AreEqual("source", configurationObject.Source);
            Assert.AreEqual(TraceOptions.None, configurationObject.TraceOutputOptions);
			Assert.AreEqual(SourceLevels.Error, configurationObject.Filter);
		}

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreMachinePolicyOverrides()
        {
            configurationObject.Formatter = "formatter";
            configurationObject.Log = "log";
            configurationObject.MachineName = "machine name";
            configurationObject.Source = "source";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            machineKey.AddStringValue(FormattedEventLogTraceListenerDataManageabilityProvider.FormatterPropertyName, "overriden formatter");
            machineKey.AddStringValue(FormattedEventLogTraceListenerDataManageabilityProvider.LogPropertyName, "overriden log");
            machineKey.AddStringValue(FormattedEventLogTraceListenerDataManageabilityProvider.MachineNamePropertyName, "overriden machine name");
            machineKey.AddStringValue(FormattedEventLogTraceListenerDataManageabilityProvider.SourcePropertyName, "overriden source");
            machineKey.AddStringValue(FormattedEventLogTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, "ProcessId, ThreadId");
			machineKey.AddStringValue(FormattedEventLogTraceListenerDataManageabilityProvider.FilterPropertyName, "Critical");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, null, true, wmiSettings);

            Assert.AreEqual("overriden formatter", configurationObject.Formatter);
            Assert.AreEqual("overriden log", configurationObject.Log);
            Assert.AreEqual("overriden machine name", configurationObject.MachineName);
            Assert.AreEqual("overriden source", configurationObject.Source);
            Assert.AreEqual(TraceOptions.ProcessId | TraceOptions.ThreadId, configurationObject.TraceOutputOptions);
			Assert.AreEqual(SourceLevels.Critical, configurationObject.Filter);
		}

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreUserPolicyOverrides()
        {
            configurationObject.Formatter = "formatter";
            configurationObject.Log = "log";
            configurationObject.MachineName = "machine name";
            configurationObject.Source = "source";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            userKey.AddStringValue(FormattedEventLogTraceListenerDataManageabilityProvider.FormatterPropertyName, "overriden formatter");
            userKey.AddStringValue(FormattedEventLogTraceListenerDataManageabilityProvider.LogPropertyName, "overriden log");
            userKey.AddStringValue(FormattedEventLogTraceListenerDataManageabilityProvider.MachineNamePropertyName, "overriden machine name");
            userKey.AddStringValue(FormattedEventLogTraceListenerDataManageabilityProvider.SourcePropertyName, "overriden source");
            userKey.AddStringValue(FormattedEventLogTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, "ProcessId, ThreadId");
			userKey.AddStringValue(FormattedEventLogTraceListenerDataManageabilityProvider.FilterPropertyName, "Critical");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, userKey, true, wmiSettings);

            Assert.AreEqual("overriden formatter", configurationObject.Formatter);
            Assert.AreEqual("overriden log", configurationObject.Log);
            Assert.AreEqual("overriden machine name", configurationObject.MachineName);
            Assert.AreEqual("overriden source", configurationObject.Source);
            Assert.AreEqual(TraceOptions.ProcessId | TraceOptions.ThreadId, configurationObject.TraceOutputOptions);
			Assert.AreEqual(SourceLevels.Critical, configurationObject.Filter);
		}

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfThereArePolicyOverridesButGroupPoliciesAreDisabled()
        {
            configurationObject.Formatter = "formatter";
            configurationObject.Log = "log";
            configurationObject.MachineName = "machine name";
            configurationObject.Source = "source";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            machineKey.AddStringValue(FormattedEventLogTraceListenerDataManageabilityProvider.FormatterPropertyName, "overriden formatter");
            machineKey.AddStringValue(FormattedEventLogTraceListenerDataManageabilityProvider.LogPropertyName, "overriden log");
            machineKey.AddStringValue(FormattedEventLogTraceListenerDataManageabilityProvider.MachineNamePropertyName, "overriden machine name");
            machineKey.AddStringValue(FormattedEventLogTraceListenerDataManageabilityProvider.SourcePropertyName, "overriden source");
            machineKey.AddStringValue(FormattedEventLogTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, "ProcessId, ThreadId");
			machineKey.AddStringValue(FormattedEventLogTraceListenerDataManageabilityProvider.FilterPropertyName, "Critical");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, null, true, wmiSettings);

            Assert.AreEqual("formatter", configurationObject.Formatter);
            Assert.AreEqual("log", configurationObject.Log);
            Assert.AreEqual("machine name", configurationObject.MachineName);
            Assert.AreEqual("source", configurationObject.Source);
            Assert.AreEqual(TraceOptions.None, configurationObject.TraceOutputOptions);
			Assert.AreEqual(SourceLevels.Error, configurationObject.Filter);
		}

        [TestMethod]
        public void ConfigurationObjectIsModifiedWithFormatterOverrideWithListItemNone()
        {
            configurationObject.Formatter = "formatter";

            machineKey.AddStringValue(FormattedEventLogTraceListenerDataManageabilityProvider.FormatterPropertyName, AdmContentBuilder.NoneListItem);
            machineKey.AddStringValue(FormattedEventLogTraceListenerDataManageabilityProvider.LogPropertyName, "overriden log");
            machineKey.AddStringValue(FormattedEventLogTraceListenerDataManageabilityProvider.MachineNamePropertyName, "overriden machine name");
            machineKey.AddStringValue(FormattedEventLogTraceListenerDataManageabilityProvider.SourcePropertyName, "overriden source");
            machineKey.AddStringValue(FormattedEventLogTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, "ProcessId, ThreadId");
			machineKey.AddStringValue(FormattedEventLogTraceListenerDataManageabilityProvider.FilterPropertyName, "Critical");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual("", configurationObject.Formatter);
        }

        [TestMethod]
        public void WmiSettingsAreNotGeneratedIfWmiIsDisabled()
        {
            configurationObject.Formatter = "formatter";
            configurationObject.Log = "log";
            configurationObject.MachineName = "machine name";
            configurationObject.Source = "source";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, userKey, false, wmiSettings);

            Assert.AreEqual(0, wmiSettings.Count);
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedIfWmiIsEnabled()
        {
            configurationObject.Formatter = "formatter";
            configurationObject.Log = "log";
            configurationObject.MachineName = "machine name";
            configurationObject.Source = "source";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(FormattedEventLogTraceListenerSetting), wmiSettings[0].GetType());
            Assert.AreEqual(configurationObject.Formatter, ((FormattedEventLogTraceListenerSetting)wmiSettings[0]).Formatter);
            Assert.AreEqual(configurationObject.Log, ((FormattedEventLogTraceListenerSetting)wmiSettings[0]).Log);
            Assert.AreEqual(configurationObject.MachineName, ((FormattedEventLogTraceListenerSetting)wmiSettings[0]).MachineName);
            Assert.AreEqual(configurationObject.Source, ((FormattedEventLogTraceListenerSetting)wmiSettings[0]).Source);
            Assert.AreEqual(configurationObject.TraceOutputOptions.ToString(), ((FormattedEventLogTraceListenerSetting)wmiSettings[0]).TraceOutputOptions);
			Assert.AreEqual(configurationObject.Filter.ToString(), ((FormattedEventLogTraceListenerSetting)wmiSettings[0]).Filter);
		}

        [TestMethod]
        public void WmiSettingsAreGeneratedWithPolicyOverridesIfWmiIsEnabled()
        {
            configurationObject.Formatter = "formatter";
            configurationObject.Log = "log";
            configurationObject.MachineName = "machine name";
            configurationObject.Source = "source";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            machineKey.AddStringValue(FormattedEventLogTraceListenerDataManageabilityProvider.FormatterPropertyName, "overriden formatter");
            machineKey.AddStringValue(FormattedEventLogTraceListenerDataManageabilityProvider.LogPropertyName, "overriden log");
            machineKey.AddStringValue(FormattedEventLogTraceListenerDataManageabilityProvider.MachineNamePropertyName, "overriden machine name");
            machineKey.AddStringValue(FormattedEventLogTraceListenerDataManageabilityProvider.SourcePropertyName, "overriden source");
            machineKey.AddStringValue(FormattedEventLogTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, "ProcessId, ThreadId");
			machineKey.AddStringValue(FormattedEventLogTraceListenerDataManageabilityProvider.FilterPropertyName, "Critical");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(FormattedEventLogTraceListenerSetting), wmiSettings[0].GetType());
            Assert.AreEqual(configurationObject.Formatter, ((FormattedEventLogTraceListenerSetting)wmiSettings[0]).Formatter);
            Assert.AreEqual(configurationObject.Log, ((FormattedEventLogTraceListenerSetting)wmiSettings[0]).Log);
            Assert.AreEqual(configurationObject.MachineName, ((FormattedEventLogTraceListenerSetting)wmiSettings[0]).MachineName);
            Assert.AreEqual(configurationObject.Source, ((FormattedEventLogTraceListenerSetting)wmiSettings[0]).Source);
            Assert.AreEqual(configurationObject.TraceOutputOptions.ToString(), ((FormattedEventLogTraceListenerSetting)wmiSettings[0]).TraceOutputOptions);
			Assert.AreEqual(configurationObject.Filter.ToString(), ((FormattedEventLogTraceListenerSetting)wmiSettings[0]).Filter);
		}

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContent()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            LoggingSettings section = new LoggingSettings();
            configurationSource.Add(LoggingSettings.SectionName, section);

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
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(FormattedEventLogTraceListenerDataManageabilityProvider.SourcePropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(FormattedEventLogTraceListenerDataManageabilityProvider.LogPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(FormattedEventLogTraceListenerDataManageabilityProvider.MachineNamePropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(FlatFileTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName,
                            partsEnumerator.Current.ValueName);

			Assert.IsTrue(partsEnumerator.MoveNext());
			Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
			Assert.IsNull(partsEnumerator.Current.KeyName);
			Assert.AreEqual(FlatFileTraceListenerDataManageabilityProvider.FilterPropertyName,
							partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(FlatFileTraceListenerDataManageabilityProvider.FormatterPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsFalse(partsEnumerator.MoveNext());
            Assert.IsFalse(policiesEnumerator.MoveNext());
        }
    }
}
