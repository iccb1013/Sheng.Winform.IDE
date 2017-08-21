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
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Manageability.Tests
{
    [TestClass]
    public class FormattedDatabaseTraceListenerDataManageabilityProviderFixture
    {
        FormattedDatabaseTraceListenerDataManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        IList<ConfigurationSetting> wmiSettings;
        FormattedDatabaseTraceListenerData configurationObject;

        [TestInitialize]
        public void SetUp()
        {
            provider = new FormattedDatabaseTraceListenerDataManageabilityProvider();
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            wmiSettings = new List<ConfigurationSetting>();
            configurationObject = new FormattedDatabaseTraceListenerData();
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

            Assembly assembly = typeof(FormattedDatabaseTraceListenerDataManageabilityProvider).Assembly;
            foreach (ConfigurationElementManageabilityProviderAttribute providerAttribute
                in assembly.GetCustomAttributes(typeof(ConfigurationElementManageabilityProviderAttribute), false))
            {
                if (providerAttribute.ManageabilityProviderType.Equals(typeof(FormattedDatabaseTraceListenerDataManageabilityProvider)))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(LoggingSettingsManageabilityProvider), selectedAttribute.SectionManageabilityProviderType);
            Assert.AreSame(typeof(FormattedDatabaseTraceListenerData), selectedAttribute.TargetType);
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
            configurationObject.AddCategoryStoredProcName = "add category";
            configurationObject.DatabaseInstanceName = "database";
            configurationObject.Formatter = "formatter";
            configurationObject.WriteLogStoredProcName = "write";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, null, true, wmiSettings);

            Assert.AreEqual("add category", configurationObject.AddCategoryStoredProcName);
            Assert.AreEqual("database", configurationObject.DatabaseInstanceName);
            Assert.AreEqual("formatter", configurationObject.Formatter);
            Assert.AreEqual("write", configurationObject.WriteLogStoredProcName);
            Assert.AreEqual(TraceOptions.None, configurationObject.TraceOutputOptions);
			Assert.AreEqual(SourceLevels.Error, configurationObject.Filter);
		}

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreMachinePolicyOverrides()
        {
            configurationObject.AddCategoryStoredProcName = "add category";
            configurationObject.DatabaseInstanceName = "database";
            configurationObject.Formatter = "formatter";
            configurationObject.WriteLogStoredProcName = "write";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            machineKey.AddStringValue(FormattedDatabaseTraceListenerDataManageabilityProvider.AddCategoryStoredProcNamePropertyName, "overriden add category");
            machineKey.AddStringValue(FormattedDatabaseTraceListenerDataManageabilityProvider.DatabaseInstanceNamePropertyName, "overriden database");
            machineKey.AddStringValue(FormattedDatabaseTraceListenerDataManageabilityProvider.FormatterPropertyName, "overriden formatter");
            machineKey.AddStringValue(FormattedDatabaseTraceListenerDataManageabilityProvider.WriteLogStoredProcNamePropertyName, "overriden write");
            machineKey.AddStringValue(FormattedDatabaseTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, "ProcessId, ThreadId");
			machineKey.AddStringValue(FormattedDatabaseTraceListenerDataManageabilityProvider.FilterPropertyName, "Critical");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, null, true, wmiSettings);

            Assert.AreEqual("overriden add category", configurationObject.AddCategoryStoredProcName);
            Assert.AreEqual("overriden database", configurationObject.DatabaseInstanceName);
            Assert.AreEqual("overriden formatter", configurationObject.Formatter);
            Assert.AreEqual("overriden write", configurationObject.WriteLogStoredProcName);
            Assert.AreEqual(TraceOptions.ProcessId | TraceOptions.ThreadId, configurationObject.TraceOutputOptions);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreUserPolicyOverrides()
        {
            configurationObject.AddCategoryStoredProcName = "add category";
            configurationObject.DatabaseInstanceName = "database";
            configurationObject.Formatter = "formatter";
            configurationObject.WriteLogStoredProcName = "write";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            userKey.AddStringValue(FormattedDatabaseTraceListenerDataManageabilityProvider.AddCategoryStoredProcNamePropertyName, "overriden add category");
            userKey.AddStringValue(FormattedDatabaseTraceListenerDataManageabilityProvider.DatabaseInstanceNamePropertyName, "overriden database");
            userKey.AddStringValue(FormattedDatabaseTraceListenerDataManageabilityProvider.FormatterPropertyName, "overriden formatter");
            userKey.AddStringValue(FormattedDatabaseTraceListenerDataManageabilityProvider.WriteLogStoredProcNamePropertyName, "overriden write");
            userKey.AddStringValue(FormattedDatabaseTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, "ProcessId, ThreadId");
			userKey.AddStringValue(FormattedDatabaseTraceListenerDataManageabilityProvider.FilterPropertyName, "Critical");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, userKey, true, wmiSettings);

            Assert.AreEqual("overriden add category", configurationObject.AddCategoryStoredProcName);
            Assert.AreEqual("overriden database", configurationObject.DatabaseInstanceName);
            Assert.AreEqual("overriden formatter", configurationObject.Formatter);
            Assert.AreEqual("overriden write", configurationObject.WriteLogStoredProcName);
            Assert.AreEqual(TraceOptions.ProcessId | TraceOptions.ThreadId, configurationObject.TraceOutputOptions);
        }

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfThereArePolicyOverridesButGroupPoliciesAreDisabled()
        {
            configurationObject.AddCategoryStoredProcName = "add category";
            configurationObject.DatabaseInstanceName = "database";
            configurationObject.Formatter = "formatter";
            configurationObject.WriteLogStoredProcName = "write";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            machineKey.AddStringValue(FormattedDatabaseTraceListenerDataManageabilityProvider.AddCategoryStoredProcNamePropertyName, "overriden add category");
            machineKey.AddStringValue(FormattedDatabaseTraceListenerDataManageabilityProvider.DatabaseInstanceNamePropertyName, "overriden database");
            machineKey.AddStringValue(FormattedDatabaseTraceListenerDataManageabilityProvider.FormatterPropertyName, "overriden formatter");
            machineKey.AddStringValue(FormattedDatabaseTraceListenerDataManageabilityProvider.WriteLogStoredProcNamePropertyName, "overriden write");
            machineKey.AddStringValue(FormattedDatabaseTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, "ProcessId, ThreadId");
			machineKey.AddStringValue(FormattedDatabaseTraceListenerDataManageabilityProvider.FilterPropertyName, "Critical");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, null, true, wmiSettings);

            Assert.AreEqual("add category", configurationObject.AddCategoryStoredProcName);
            Assert.AreEqual("database", configurationObject.DatabaseInstanceName);
            Assert.AreEqual("formatter", configurationObject.Formatter);
            Assert.AreEqual("write", configurationObject.WriteLogStoredProcName);
            Assert.AreEqual(TraceOptions.None, configurationObject.TraceOutputOptions);
			Assert.AreEqual(SourceLevels.Error, configurationObject.Filter);
		}

        [TestMethod]
        public void ConfigurationObjectIsModifiedWithFormatterOverrideWithListItemNone()
        {
            configurationObject.Formatter = "formatter";

            machineKey.AddStringValue(FormattedDatabaseTraceListenerDataManageabilityProvider.AddCategoryStoredProcNamePropertyName, "overriden add category");
            machineKey.AddStringValue(FormattedDatabaseTraceListenerDataManageabilityProvider.DatabaseInstanceNamePropertyName, "overriden database");
            machineKey.AddStringValue(FormattedDatabaseTraceListenerDataManageabilityProvider.FormatterPropertyName, AdmContentBuilder.NoneListItem);
            machineKey.AddStringValue(FormattedDatabaseTraceListenerDataManageabilityProvider.WriteLogStoredProcNamePropertyName, "overriden write");
            machineKey.AddStringValue(FormattedDatabaseTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, "ProcessId, ThreadId");
			machineKey.AddStringValue(FormattedDatabaseTraceListenerDataManageabilityProvider.FilterPropertyName, "Critical");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual("", configurationObject.Formatter);
        }

        [TestMethod]
        public void WmiSettingsAreNotGeneratedIfWmiIsDisabled()
        {
            configurationObject.AddCategoryStoredProcName = "add category";
            configurationObject.DatabaseInstanceName = "database";
            configurationObject.Formatter = "formatter";
            configurationObject.WriteLogStoredProcName = "write";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, userKey, false, wmiSettings);

            Assert.AreEqual(0, wmiSettings.Count);
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedIfWmiIsEnabled()
        {
            configurationObject.AddCategoryStoredProcName = "add category";
            configurationObject.DatabaseInstanceName = "database";
            configurationObject.Formatter = "formatter";
            configurationObject.WriteLogStoredProcName = "write";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(FormattedDatabaseTraceListenerSetting), wmiSettings[0].GetType());
            Assert.AreEqual(configurationObject.AddCategoryStoredProcName, ((FormattedDatabaseTraceListenerSetting)wmiSettings[0]).AddCategoryStoredProcName);
            Assert.AreEqual(configurationObject.DatabaseInstanceName, ((FormattedDatabaseTraceListenerSetting)wmiSettings[0]).DatabaseInstanceName);
            Assert.AreEqual(configurationObject.Formatter, ((FormattedDatabaseTraceListenerSetting)wmiSettings[0]).Formatter);
            Assert.AreEqual(configurationObject.TraceOutputOptions.ToString(), ((FormattedDatabaseTraceListenerSetting)wmiSettings[0]).TraceOutputOptions);
			Assert.AreEqual(configurationObject.Filter.ToString(), ((FormattedDatabaseTraceListenerSetting)wmiSettings[0]).Filter);
			Assert.AreEqual(configurationObject.WriteLogStoredProcName, ((FormattedDatabaseTraceListenerSetting)wmiSettings[0]).WriteLogStoredProcName);
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedWithPolicyOverridesIfWmiIsEnabled()
        {
            configurationObject.AddCategoryStoredProcName = "add category";
            configurationObject.DatabaseInstanceName = "database";
            configurationObject.Formatter = "formatter";
            configurationObject.WriteLogStoredProcName = "write";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            machineKey.AddStringValue(FormattedDatabaseTraceListenerDataManageabilityProvider.AddCategoryStoredProcNamePropertyName, "overriden add category");
            machineKey.AddStringValue(FormattedDatabaseTraceListenerDataManageabilityProvider.DatabaseInstanceNamePropertyName, "overriden database");
            machineKey.AddStringValue(FormattedDatabaseTraceListenerDataManageabilityProvider.FormatterPropertyName, "overriden formatter");
            machineKey.AddStringValue(FormattedDatabaseTraceListenerDataManageabilityProvider.WriteLogStoredProcNamePropertyName, "overriden write");
            machineKey.AddStringValue(FormattedDatabaseTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, "ProcessId, ThreadId");
			machineKey.AddStringValue(FormattedDatabaseTraceListenerDataManageabilityProvider.FilterPropertyName, "Critical");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, null, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(FormattedDatabaseTraceListenerSetting), wmiSettings[0].GetType());
            Assert.AreEqual(configurationObject.AddCategoryStoredProcName, ((FormattedDatabaseTraceListenerSetting)wmiSettings[0]).AddCategoryStoredProcName);
            Assert.AreEqual(configurationObject.DatabaseInstanceName, ((FormattedDatabaseTraceListenerSetting)wmiSettings[0]).DatabaseInstanceName);
            Assert.AreEqual(configurationObject.Formatter, ((FormattedDatabaseTraceListenerSetting)wmiSettings[0]).Formatter);
            Assert.AreEqual(configurationObject.TraceOutputOptions.ToString(), ((FormattedDatabaseTraceListenerSetting)wmiSettings[0]).TraceOutputOptions);
			Assert.AreEqual(configurationObject.Filter.ToString(), ((FormattedDatabaseTraceListenerSetting)wmiSettings[0]).Filter);
			Assert.AreEqual(configurationObject.WriteLogStoredProcName, ((FormattedDatabaseTraceListenerSetting)wmiSettings[0]).WriteLogStoredProcName);
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContent()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            LoggingSettings section = new LoggingSettings();
            configurationSource.Add(LoggingSettings.SectionName, section);
            ConnectionStringsSection connectionStringsSection = new ConnectionStringsSection();
            configurationSource.Add("connectionStrings", connectionStringsSection);
            connectionStringsSection.ConnectionStrings.Add(new ConnectionStringSettings("cs1", "cs1"));

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
            Assert.AreEqual(FormattedDatabaseTraceListenerDataManageabilityProvider.DatabaseInstanceNamePropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(FormattedDatabaseTraceListenerDataManageabilityProvider.WriteLogStoredProcNamePropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(FormattedDatabaseTraceListenerDataManageabilityProvider.AddCategoryStoredProcNamePropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(FormattedDatabaseTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName,
                            partsEnumerator.Current.ValueName);

			Assert.IsTrue(partsEnumerator.MoveNext());
			Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
			Assert.IsNull(partsEnumerator.Current.KeyName);
			Assert.AreEqual(FormattedDatabaseTraceListenerDataManageabilityProvider.FilterPropertyName,
							partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(FormattedDatabaseTraceListenerDataManageabilityProvider.FormatterPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsFalse(partsEnumerator.MoveNext());
            Assert.IsFalse(policiesEnumerator.MoveNext());
        }
    }
}
