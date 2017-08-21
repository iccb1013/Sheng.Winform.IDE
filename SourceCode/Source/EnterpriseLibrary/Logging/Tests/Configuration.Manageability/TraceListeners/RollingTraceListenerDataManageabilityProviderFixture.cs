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
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Tests.TraceListeners
{
    [TestClass]
    public class RollingFlatFileTraceListenerDataManageabilityProviderFixture
    {
        RollingFlatFileTraceListenerDataManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        IList<ConfigurationSetting> wmiSettings;
        RollingFlatFileTraceListenerData configurationObject;

        [TestInitialize]
        public void SetUp()
        {
            provider = new RollingFlatFileTraceListenerDataManageabilityProvider();
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            wmiSettings = new List<ConfigurationSetting>();
            configurationObject = new RollingFlatFileTraceListenerData();
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

            Assembly assembly = typeof(RollingFlatFileTraceListenerDataManageabilityProvider).Assembly;
            foreach (ConfigurationElementManageabilityProviderAttribute providerAttribute
                in assembly.GetCustomAttributes(typeof(ConfigurationElementManageabilityProviderAttribute), false))
            {
                if (providerAttribute.ManageabilityProviderType.Equals(typeof(RollingFlatFileTraceListenerDataManageabilityProvider)))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(LoggingSettingsManageabilityProvider), selectedAttribute.SectionManageabilityProviderType);
            Assert.AreSame(typeof(RollingFlatFileTraceListenerData), selectedAttribute.TargetType);
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
            configurationObject.Formatter = "formatter";
            configurationObject.RollFileExistsBehavior = RollFileExistsBehavior.Increment;
            configurationObject.RollInterval = RollInterval.Month;
            configurationObject.RollSizeKB = 100;
            configurationObject.TimeStampPattern = "pattern";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;
			configurationObject.Header = "header";
            configurationObject.Footer = "footer";

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, null, true, wmiSettings);

            Assert.AreEqual("file name", configurationObject.FileName);
            Assert.AreEqual("formatter", configurationObject.Formatter);
            Assert.AreEqual(RollFileExistsBehavior.Increment, configurationObject.RollFileExistsBehavior);
            Assert.AreEqual(RollInterval.Month, configurationObject.RollInterval);
            Assert.AreEqual(100, configurationObject.RollSizeKB);
            Assert.AreEqual("pattern", configurationObject.TimeStampPattern);
            Assert.AreEqual(TraceOptions.None, configurationObject.TraceOutputOptions);
			Assert.AreEqual(SourceLevels.Error, configurationObject.Filter);
			Assert.AreEqual("header", configurationObject.Header);
            Assert.AreEqual("footer", configurationObject.Footer);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreMachinePolicyOverrides()
        {
            configurationObject.FileName = "file name";
            configurationObject.Formatter = "formatter";
            configurationObject.RollFileExistsBehavior = RollFileExistsBehavior.Increment;
            configurationObject.RollInterval = RollInterval.Month;
            configurationObject.RollSizeKB = 100;
            configurationObject.TimeStampPattern = "pattern";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;
			configurationObject.Header = "header";
            configurationObject.Footer = "footer";

            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.FileNamePropertyName, "overriden file name");
            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.FormatterPropertyName, "overriden formatter");
            machineKey.AddEnumValue<RollFileExistsBehavior>(RollingFlatFileTraceListenerDataManageabilityProvider.RollFileExistsBehaviorPropertyName, RollFileExistsBehavior.Overwrite);
            machineKey.AddEnumValue<RollInterval>(RollingFlatFileTraceListenerDataManageabilityProvider.RollIntervalPropertyName, RollInterval.Day);
            machineKey.AddIntValue(RollingFlatFileTraceListenerDataManageabilityProvider.RollSizeKBPropertyName, 200);
            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.TimeStampPatternPropertyName, "overriden pattern");
            machineKey.AddEnumValue<TraceOptions>(RollingFlatFileTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, TraceOptions.ProcessId | TraceOptions.ThreadId);
			machineKey.AddEnumValue<SourceLevels>(RollingFlatFileTraceListenerDataManageabilityProvider.FilterPropertyName, SourceLevels.Critical);
			machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.HeaderPropertyName, "overriden header");
            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.FooterPropertyName, "overriden footer");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, null, true, wmiSettings);

            Assert.AreEqual("overriden file name", configurationObject.FileName);
            Assert.AreEqual("overriden formatter", configurationObject.Formatter);
            Assert.AreEqual(RollFileExistsBehavior.Overwrite, configurationObject.RollFileExistsBehavior);
            Assert.AreEqual(RollInterval.Day, configurationObject.RollInterval);
            Assert.AreEqual(200, configurationObject.RollSizeKB);
            Assert.AreEqual("overriden pattern", configurationObject.TimeStampPattern);
            Assert.AreEqual(TraceOptions.ProcessId | TraceOptions.ThreadId, configurationObject.TraceOutputOptions);
			Assert.AreEqual(SourceLevels.Critical, configurationObject.Filter);
			Assert.AreEqual("overriden header", configurationObject.Header);
            Assert.AreEqual("overriden footer", configurationObject.Footer);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreUserPolicyOverrides()
        {
            configurationObject.FileName = "file name";
            configurationObject.Formatter = "formatter";
            configurationObject.RollFileExistsBehavior = RollFileExistsBehavior.Increment;
            configurationObject.RollInterval = RollInterval.Month;
            configurationObject.RollSizeKB = 100;
            configurationObject.TimeStampPattern = "pattern";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;
			configurationObject.Header = "header";
            configurationObject.Footer = "footer";

            userKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.FileNamePropertyName, "overriden file name");
            userKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.FormatterPropertyName, "overriden formatter");
            userKey.AddEnumValue<RollFileExistsBehavior>(RollingFlatFileTraceListenerDataManageabilityProvider.RollFileExistsBehaviorPropertyName, RollFileExistsBehavior.Overwrite);
            userKey.AddEnumValue<RollInterval>(RollingFlatFileTraceListenerDataManageabilityProvider.RollIntervalPropertyName, RollInterval.Day);
            userKey.AddIntValue(RollingFlatFileTraceListenerDataManageabilityProvider.RollSizeKBPropertyName, 200);
            userKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.TimeStampPatternPropertyName, "overriden pattern");
            userKey.AddEnumValue<TraceOptions>(RollingFlatFileTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, TraceOptions.ProcessId | TraceOptions.ThreadId);
			userKey.AddEnumValue<SourceLevels>(RollingFlatFileTraceListenerDataManageabilityProvider.FilterPropertyName, SourceLevels.Critical);
			userKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.HeaderPropertyName, "overriden header");
            userKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.FooterPropertyName, "overriden footer");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, userKey, true, wmiSettings);

            Assert.AreEqual("overriden file name", configurationObject.FileName);
            Assert.AreEqual("overriden formatter", configurationObject.Formatter);
            Assert.AreEqual(RollFileExistsBehavior.Overwrite, configurationObject.RollFileExistsBehavior);
            Assert.AreEqual(RollInterval.Day, configurationObject.RollInterval);
            Assert.AreEqual(200, configurationObject.RollSizeKB);
            Assert.AreEqual("overriden pattern", configurationObject.TimeStampPattern);
            Assert.AreEqual(TraceOptions.ProcessId | TraceOptions.ThreadId, configurationObject.TraceOutputOptions);
			Assert.AreEqual(SourceLevels.Critical, configurationObject.Filter);
			Assert.AreEqual("overriden header", configurationObject.Header);
            Assert.AreEqual("overriden footer", configurationObject.Footer);
        }

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfThereArePolicyOverridesButGroupPoliciesAreDisabled()
        {
            configurationObject.FileName = "file name";
            configurationObject.Formatter = "formatter";
            configurationObject.RollFileExistsBehavior = RollFileExistsBehavior.Increment;
            configurationObject.RollInterval = RollInterval.Month;
            configurationObject.RollSizeKB = 100;
            configurationObject.TimeStampPattern = "pattern";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;
			configurationObject.Header = "header";
            configurationObject.Footer = "footer";

            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.FileNamePropertyName, "overriden file name");
            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.FormatterPropertyName, "overriden formatter");
            machineKey.AddEnumValue<RollFileExistsBehavior>(RollingFlatFileTraceListenerDataManageabilityProvider.RollFileExistsBehaviorPropertyName, RollFileExistsBehavior.Overwrite);
            machineKey.AddEnumValue<RollInterval>(RollingFlatFileTraceListenerDataManageabilityProvider.RollIntervalPropertyName, RollInterval.Day);
            machineKey.AddIntValue(RollingFlatFileTraceListenerDataManageabilityProvider.RollSizeKBPropertyName, 200);
            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.TimeStampPatternPropertyName, "overriden pattern");
            machineKey.AddEnumValue<TraceOptions>(RollingFlatFileTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, TraceOptions.ProcessId | TraceOptions.ThreadId);
			machineKey.AddEnumValue<SourceLevels>(RollingFlatFileTraceListenerDataManageabilityProvider.FilterPropertyName, SourceLevels.Critical);
			machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.HeaderPropertyName, "overriden header");
            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.FooterPropertyName, "overriden footer");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, null, true, wmiSettings);

            Assert.AreEqual("file name", configurationObject.FileName);
            Assert.AreEqual("formatter", configurationObject.Formatter);
            Assert.AreEqual(RollFileExistsBehavior.Increment, configurationObject.RollFileExistsBehavior);
            Assert.AreEqual(RollInterval.Month, configurationObject.RollInterval);
            Assert.AreEqual(100, configurationObject.RollSizeKB);
            Assert.AreEqual("pattern", configurationObject.TimeStampPattern);
            Assert.AreEqual(TraceOptions.None, configurationObject.TraceOutputOptions);
			Assert.AreEqual(SourceLevels.Error, configurationObject.Filter);
			Assert.AreEqual("header", configurationObject.Header);
            Assert.AreEqual("footer", configurationObject.Footer);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedWithFormatterOverrideWithListItemNone()
        {
            configurationObject.Formatter = "formatter";

            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.FileNamePropertyName, "overriden file name");
            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.FormatterPropertyName, AdmContentBuilder.NoneListItem);
            machineKey.AddEnumValue<RollFileExistsBehavior>(RollingFlatFileTraceListenerDataManageabilityProvider.RollFileExistsBehaviorPropertyName, RollFileExistsBehavior.Overwrite);
            machineKey.AddEnumValue<RollInterval>(RollingFlatFileTraceListenerDataManageabilityProvider.RollIntervalPropertyName, RollInterval.Day);
            machineKey.AddIntValue(RollingFlatFileTraceListenerDataManageabilityProvider.RollSizeKBPropertyName, 200);
            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.TimeStampPatternPropertyName, "overriden pattern");
            machineKey.AddEnumValue<TraceOptions>(RollingFlatFileTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, TraceOptions.ProcessId | TraceOptions.ThreadId);
			machineKey.AddEnumValue<SourceLevels>(RollingFlatFileTraceListenerDataManageabilityProvider.FilterPropertyName, SourceLevels.Critical);
			machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.HeaderPropertyName, "overriden header");
            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.FooterPropertyName, "overriden footer");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual("", configurationObject.Formatter);
        }

        [TestMethod]
        public void WmiSettingsAreNotGeneratedIfWmiIsDisabled()
        {
            configurationObject.FileName = "file name";
            configurationObject.Formatter = "formatter";
            configurationObject.RollFileExistsBehavior = RollFileExistsBehavior.Increment;
            configurationObject.RollInterval = RollInterval.Month;
            configurationObject.RollSizeKB = 100;
            configurationObject.TimeStampPattern = "pattern";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;
			configurationObject.Header = "header";
            configurationObject.Footer = "footer";

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, userKey, false, wmiSettings);

            Assert.AreEqual(0, wmiSettings.Count);
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedIfWmiIsEnabled()
        {
            configurationObject.FileName = "file name";
            configurationObject.Formatter = "formatter";
            configurationObject.RollFileExistsBehavior = RollFileExistsBehavior.Increment;
            configurationObject.RollInterval = RollInterval.Month;
            configurationObject.RollSizeKB = 100;
            configurationObject.TimeStampPattern = "pattern";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;
			configurationObject.Header = "header";
            configurationObject.Footer = "footer";

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(RollingFlatFileTraceListenerSetting), wmiSettings[0].GetType());
            Assert.AreEqual(configurationObject.FileName, ((RollingFlatFileTraceListenerSetting)wmiSettings[0]).FileName);
            Assert.AreEqual(configurationObject.Formatter, ((RollingFlatFileTraceListenerSetting)wmiSettings[0]).Formatter);
            Assert.AreEqual(configurationObject.RollFileExistsBehavior.ToString(), ((RollingFlatFileTraceListenerSetting)wmiSettings[0]).RollFileExistsBehavior);
            Assert.AreEqual(configurationObject.RollInterval.ToString(), ((RollingFlatFileTraceListenerSetting)wmiSettings[0]).RollInterval);
            Assert.AreEqual(configurationObject.RollSizeKB, ((RollingFlatFileTraceListenerSetting)wmiSettings[0]).RollSizeKB);
            Assert.AreEqual(configurationObject.TimeStampPattern, ((RollingFlatFileTraceListenerSetting)wmiSettings[0]).TimeStampPattern);
            Assert.AreEqual(configurationObject.TraceOutputOptions.ToString(), ((RollingFlatFileTraceListenerSetting)wmiSettings[0]).TraceOutputOptions);
			Assert.AreEqual(configurationObject.Filter.ToString(), ((RollingFlatFileTraceListenerSetting)wmiSettings[0]).Filter);
			Assert.AreEqual(configurationObject.Header, ((RollingFlatFileTraceListenerSetting)wmiSettings[0]).Header);
            Assert.AreEqual(configurationObject.Footer, ((RollingFlatFileTraceListenerSetting)wmiSettings[0]).Footer);
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedWithPolicyOverridesIfWmiIsEnabled()
        {
            configurationObject.FileName = "file name";
            configurationObject.Formatter = "formatter";
            configurationObject.RollFileExistsBehavior = RollFileExistsBehavior.Increment;
            configurationObject.RollInterval = RollInterval.Month;
            configurationObject.RollSizeKB = 100;
            configurationObject.TimeStampPattern = "pattern";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;
			configurationObject.Header = "header";
            configurationObject.Footer = "footer";

            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.FileNamePropertyName, "overriden file name");
            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.FormatterPropertyName, AdmContentBuilder.NoneListItem);
            machineKey.AddEnumValue<RollFileExistsBehavior>(RollingFlatFileTraceListenerDataManageabilityProvider.RollFileExistsBehaviorPropertyName, RollFileExistsBehavior.Overwrite);
            machineKey.AddEnumValue<RollInterval>(RollingFlatFileTraceListenerDataManageabilityProvider.RollIntervalPropertyName, RollInterval.Day);
            machineKey.AddIntValue(RollingFlatFileTraceListenerDataManageabilityProvider.RollSizeKBPropertyName, 200);
            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.TimeStampPatternPropertyName, "overriden pattern");
            machineKey.AddEnumValue<TraceOptions>(RollingFlatFileTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, TraceOptions.ProcessId | TraceOptions.ThreadId);
			machineKey.AddEnumValue<SourceLevels>(RollingFlatFileTraceListenerDataManageabilityProvider.FilterPropertyName, SourceLevels.Critical);
			machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.HeaderPropertyName, "overriden header");
            machineKey.AddStringValue(RollingFlatFileTraceListenerDataManageabilityProvider.FooterPropertyName, "overriden footer");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(RollingFlatFileTraceListenerSetting), wmiSettings[0].GetType());
            Assert.AreEqual(configurationObject.FileName, ((RollingFlatFileTraceListenerSetting)wmiSettings[0]).FileName);
            Assert.AreEqual(configurationObject.Formatter, ((RollingFlatFileTraceListenerSetting)wmiSettings[0]).Formatter);
            Assert.AreEqual(configurationObject.RollFileExistsBehavior.ToString(), ((RollingFlatFileTraceListenerSetting)wmiSettings[0]).RollFileExistsBehavior);
            Assert.AreEqual(configurationObject.RollInterval.ToString(), ((RollingFlatFileTraceListenerSetting)wmiSettings[0]).RollInterval);
            Assert.AreEqual(configurationObject.RollSizeKB, ((RollingFlatFileTraceListenerSetting)wmiSettings[0]).RollSizeKB);
            Assert.AreEqual(configurationObject.TimeStampPattern, ((RollingFlatFileTraceListenerSetting)wmiSettings[0]).TimeStampPattern);
            Assert.AreEqual(configurationObject.TraceOutputOptions.ToString(), ((RollingFlatFileTraceListenerSetting)wmiSettings[0]).TraceOutputOptions);
			Assert.AreEqual(configurationObject.Filter.ToString(), ((RollingFlatFileTraceListenerSetting)wmiSettings[0]).Filter);
			Assert.AreEqual(configurationObject.Header, ((RollingFlatFileTraceListenerSetting)wmiSettings[0]).Header);
            Assert.AreEqual(configurationObject.Footer, ((RollingFlatFileTraceListenerSetting)wmiSettings[0]).Footer);
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
            Assert.AreEqual(RollingFlatFileTraceListenerDataManageabilityProvider.FileNamePropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(RollingFlatFileTraceListenerDataManageabilityProvider.HeaderPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(RollingFlatFileTraceListenerDataManageabilityProvider.FooterPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(RollingFlatFileTraceListenerDataManageabilityProvider.RollFileExistsBehaviorPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(RollingFlatFileTraceListenerDataManageabilityProvider.RollIntervalPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmNumericPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(RollingFlatFileTraceListenerDataManageabilityProvider.RollSizeKBPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(RollingFlatFileTraceListenerDataManageabilityProvider.TimeStampPatternPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(RollingFlatFileTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName,
                            partsEnumerator.Current.ValueName);

			Assert.IsTrue(partsEnumerator.MoveNext());
			Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
			Assert.IsNull(partsEnumerator.Current.KeyName);
			Assert.AreEqual(RollingFlatFileTraceListenerDataManageabilityProvider.FilterPropertyName,
							partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(RollingFlatFileTraceListenerDataManageabilityProvider.FormatterPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsFalse(partsEnumerator.MoveNext());
            Assert.IsFalse(policiesEnumerator.MoveNext());
        }
    }
}
