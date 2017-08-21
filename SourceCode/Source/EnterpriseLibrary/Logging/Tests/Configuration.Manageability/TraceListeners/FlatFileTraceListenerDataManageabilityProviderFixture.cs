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
    public class FlatFileTraceListenerDataManageabilityProviderFixture
    {
        FlatFileTraceListenerDataManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        IList<ConfigurationSetting> wmiSettings;
        FlatFileTraceListenerData configurationObject;

        [TestInitialize]
        public void SetUp()
        {
            provider = new FlatFileTraceListenerDataManageabilityProvider();
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            wmiSettings = new List<ConfigurationSetting>();
            configurationObject = new FlatFileTraceListenerData();
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

            Assembly assembly = typeof(FlatFileTraceListenerDataManageabilityProvider).Assembly;
            foreach (ConfigurationElementManageabilityProviderAttribute providerAttribute
                in assembly.GetCustomAttributes(typeof(ConfigurationElementManageabilityProviderAttribute), false))
            {
                if (providerAttribute.ManageabilityProviderType.Equals(typeof(FlatFileTraceListenerDataManageabilityProvider)))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(LoggingSettingsManageabilityProvider), selectedAttribute.SectionManageabilityProviderType);
            Assert.AreSame(typeof(FlatFileTraceListenerData), selectedAttribute.TargetType);
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
            configurationObject.FileName = "file name";
            configurationObject.Footer = "footer";
            configurationObject.Formatter = "formatter";
            configurationObject.Header = "header";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, null, true, wmiSettings);

            Assert.AreEqual("file name", configurationObject.FileName);
            Assert.AreEqual("footer", configurationObject.Footer);
            Assert.AreEqual("formatter", configurationObject.Formatter);
            Assert.AreEqual("header", configurationObject.Header);
            Assert.AreEqual(TraceOptions.None, configurationObject.TraceOutputOptions);
			Assert.AreEqual(SourceLevels.Error, configurationObject.Filter);
		}

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreMachinePolicyOverrides()
        {
            configurationObject.FileName = "file name";
            configurationObject.Footer = "footer";
            configurationObject.Formatter = "formatter";
            configurationObject.Header = "header";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            machineKey.AddStringValue(FlatFileTraceListenerDataManageabilityProvider.FileNamePropertyName, "overriden file name");
            machineKey.AddStringValue(FlatFileTraceListenerDataManageabilityProvider.FooterPropertyName, "overriden footer");
            machineKey.AddStringValue(FlatFileTraceListenerDataManageabilityProvider.FormatterPropertyName, "overriden formatter");
            machineKey.AddStringValue(FlatFileTraceListenerDataManageabilityProvider.HeaderPropertyName, "overriden header");
            machineKey.AddStringValue(FlatFileTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, "ProcessId, ThreadId");
			machineKey.AddStringValue(FlatFileTraceListenerDataManageabilityProvider.FilterPropertyName, "Critical");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, null, true, wmiSettings);

            Assert.AreEqual("overriden file name", configurationObject.FileName);
            Assert.AreEqual("overriden footer", configurationObject.Footer);
            Assert.AreEqual("overriden formatter", configurationObject.Formatter);
            Assert.AreEqual("overriden header", configurationObject.Header);
            Assert.AreEqual(TraceOptions.ProcessId | TraceOptions.ThreadId, configurationObject.TraceOutputOptions);
			Assert.AreEqual(SourceLevels.Critical, configurationObject.Filter);
		}

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreUserPolicyOverrides()
        {
            configurationObject.FileName = "file name";
            configurationObject.Footer = "footer";
            configurationObject.Formatter = "formatter";
            configurationObject.Header = "header";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            userKey.AddStringValue(FlatFileTraceListenerDataManageabilityProvider.FileNamePropertyName, "overriden file name");
            userKey.AddStringValue(FlatFileTraceListenerDataManageabilityProvider.FooterPropertyName, "overriden footer");
            userKey.AddStringValue(FlatFileTraceListenerDataManageabilityProvider.FormatterPropertyName, "overriden formatter");
            userKey.AddStringValue(FlatFileTraceListenerDataManageabilityProvider.HeaderPropertyName, "overriden header");
            userKey.AddStringValue(FlatFileTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, "ProcessId, ThreadId");
			userKey.AddStringValue(FlatFileTraceListenerDataManageabilityProvider.FilterPropertyName, "Critical");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, userKey, true, wmiSettings);

            Assert.AreEqual("overriden file name", configurationObject.FileName);
            Assert.AreEqual("overriden footer", configurationObject.Footer);
            Assert.AreEqual("overriden formatter", configurationObject.Formatter);
            Assert.AreEqual("overriden header", configurationObject.Header);
            Assert.AreEqual(TraceOptions.ProcessId | TraceOptions.ThreadId, configurationObject.TraceOutputOptions);
			Assert.AreEqual(SourceLevels.Critical, configurationObject.Filter);
		}

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfThereArePolicyOverridesButGroupPoliciesAreDisabled()
        {
            configurationObject.FileName = "file name";
            configurationObject.Footer = "footer";
            configurationObject.Formatter = "formatter";
            configurationObject.Header = "header";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            machineKey.AddStringValue(FlatFileTraceListenerDataManageabilityProvider.FileNamePropertyName, "overriden file name");
            machineKey.AddStringValue(FlatFileTraceListenerDataManageabilityProvider.FooterPropertyName, "overriden footer");
            machineKey.AddStringValue(FlatFileTraceListenerDataManageabilityProvider.FormatterPropertyName, "overriden formatter");
            machineKey.AddStringValue(FlatFileTraceListenerDataManageabilityProvider.HeaderPropertyName, "overriden header");
            machineKey.AddStringValue(FlatFileTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, "ProcessId, ThreadId");
			machineKey.AddStringValue(FlatFileTraceListenerDataManageabilityProvider.FilterPropertyName, "Critical");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, null, true, wmiSettings);

            Assert.AreEqual("file name", configurationObject.FileName);
            Assert.AreEqual("footer", configurationObject.Footer);
            Assert.AreEqual("formatter", configurationObject.Formatter);
            Assert.AreEqual("header", configurationObject.Header);
            Assert.AreEqual(TraceOptions.None, configurationObject.TraceOutputOptions);
			Assert.AreEqual(SourceLevels.Error, configurationObject.Filter);
		}

        [TestMethod]
        public void ConfigurationObjectIsModifiedWithFormatterOverrideWithListItemNone()
        {
            configurationObject.Formatter = "formatter";

            machineKey.AddStringValue(FlatFileTraceListenerDataManageabilityProvider.FileNamePropertyName, "overriden file name");
            machineKey.AddStringValue(FlatFileTraceListenerDataManageabilityProvider.FooterPropertyName, "overriden footer");
            machineKey.AddStringValue(FlatFileTraceListenerDataManageabilityProvider.FormatterPropertyName, AdmContentBuilder.NoneListItem);
            machineKey.AddStringValue(FlatFileTraceListenerDataManageabilityProvider.HeaderPropertyName, "overriden header");
            machineKey.AddStringValue(FlatFileTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, "ProcessId, ThreadId");
			machineKey.AddStringValue(FlatFileTraceListenerDataManageabilityProvider.FilterPropertyName, "Critical");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual("", configurationObject.Formatter);
        }

        [TestMethod]
        public void WmiSettingsAreNotGeneratedIfWmiIsDisabled()
        {
            configurationObject.FileName = "file name";
            configurationObject.Footer = "footer";
            configurationObject.Formatter = "formatter";
            configurationObject.Header = "header";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, userKey, false, wmiSettings);

            Assert.AreEqual(0, wmiSettings.Count);
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedIfWmiIsEnabled()
        {
            configurationObject.FileName = "file name";
            configurationObject.Footer = "footer";
            configurationObject.Formatter = "formatter";
            configurationObject.Header = "header";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(FlatFileTraceListenerSetting), wmiSettings[0].GetType());
            Assert.AreEqual(configurationObject.FileName, ((FlatFileTraceListenerSetting)wmiSettings[0]).FileName);
            Assert.AreEqual(configurationObject.Footer, ((FlatFileTraceListenerSetting)wmiSettings[0]).Footer);
            Assert.AreEqual(configurationObject.Formatter, ((FlatFileTraceListenerSetting)wmiSettings[0]).Formatter);
            Assert.AreEqual(configurationObject.Header, ((FlatFileTraceListenerSetting)wmiSettings[0]).Header);
            Assert.AreEqual(configurationObject.TraceOutputOptions.ToString(), ((FlatFileTraceListenerSetting)wmiSettings[0]).TraceOutputOptions);
			Assert.AreEqual(configurationObject.Filter.ToString(), ((FlatFileTraceListenerSetting)wmiSettings[0]).Filter);
		}

        [TestMethod]
        public void WmiSettingsAreGeneratedWithPolicyOverridesIfWmiIsEnabled()
        {
            configurationObject.FileName = "file name";
            configurationObject.Footer = "footer";
            configurationObject.Formatter = "formatter";
            configurationObject.Header = "header";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            machineKey.AddStringValue(FlatFileTraceListenerDataManageabilityProvider.FileNamePropertyName, "overriden file name");
            machineKey.AddStringValue(FlatFileTraceListenerDataManageabilityProvider.FooterPropertyName, "overriden footer");
            machineKey.AddStringValue(FlatFileTraceListenerDataManageabilityProvider.FormatterPropertyName, "overriden formatter");
            machineKey.AddStringValue(FlatFileTraceListenerDataManageabilityProvider.HeaderPropertyName, "overriden header");
            machineKey.AddStringValue(FlatFileTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, "ProcessId, ThreadId");
			machineKey.AddStringValue(FlatFileTraceListenerDataManageabilityProvider.FilterPropertyName, "Critical");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(FlatFileTraceListenerSetting), wmiSettings[0].GetType());
            Assert.AreEqual(configurationObject.FileName, ((FlatFileTraceListenerSetting)wmiSettings[0]).FileName);
            Assert.AreEqual(configurationObject.Footer, ((FlatFileTraceListenerSetting)wmiSettings[0]).Footer);
            Assert.AreEqual(configurationObject.Formatter, ((FlatFileTraceListenerSetting)wmiSettings[0]).Formatter);
            Assert.AreEqual(configurationObject.Header, ((FlatFileTraceListenerSetting)wmiSettings[0]).Header);
            Assert.AreEqual(configurationObject.TraceOutputOptions.ToString(), ((FlatFileTraceListenerSetting)wmiSettings[0]).TraceOutputOptions);
			Assert.AreEqual(configurationObject.Filter.ToString(), ((FlatFileTraceListenerSetting)wmiSettings[0]).Filter);
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
            Assert.AreEqual(FlatFileTraceListenerDataManageabilityProvider.FileNamePropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(FlatFileTraceListenerDataManageabilityProvider.HeaderPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(FlatFileTraceListenerDataManageabilityProvider.FooterPropertyName,
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
