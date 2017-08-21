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
    public class EmailTraceListenerDataManageabilityProviderFixture
    {
        EmailTraceListenerDataManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        IList<ConfigurationSetting> wmiSettings;
        EmailTraceListenerData configurationObject;

        [TestInitialize]
        public void SetUp()
        {
            provider = new EmailTraceListenerDataManageabilityProvider();
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            wmiSettings = new List<ConfigurationSetting>();
            configurationObject = new EmailTraceListenerData();
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

            Assembly assembly = typeof(EmailTraceListenerDataManageabilityProvider).Assembly;
            foreach (ConfigurationElementManageabilityProviderAttribute providerAttribute
                in assembly.GetCustomAttributes(typeof(ConfigurationElementManageabilityProviderAttribute), false))
            {
                if (providerAttribute.ManageabilityProviderType.Equals(typeof(EmailTraceListenerDataManageabilityProvider)))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(LoggingSettingsManageabilityProvider), selectedAttribute.SectionManageabilityProviderType);
            Assert.AreSame(typeof(EmailTraceListenerData), selectedAttribute.TargetType);
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
            configurationObject.FromAddress = "from";
            configurationObject.SmtpPort = 25;
            configurationObject.SmtpServer = "smtp server";
            configurationObject.SubjectLineEnder = "subject line ender";
            configurationObject.SubjectLineStarter = "subject line starter";
            configurationObject.ToAddress = "to";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, null, true, wmiSettings);

            Assert.AreEqual("formatter", configurationObject.Formatter);
            Assert.AreEqual("from", configurationObject.FromAddress);
            Assert.AreEqual(25, configurationObject.SmtpPort);
            Assert.AreEqual("smtp server", configurationObject.SmtpServer);
            Assert.AreEqual("subject line ender", configurationObject.SubjectLineEnder);
            Assert.AreEqual("subject line starter", configurationObject.SubjectLineStarter);
            Assert.AreEqual("to", configurationObject.ToAddress);
            Assert.AreEqual(TraceOptions.None, configurationObject.TraceOutputOptions);
			Assert.AreEqual(SourceLevels.Error, configurationObject.Filter);
		}

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreMachinePolicyOverrides()
        {
            configurationObject.Formatter = "formatter";
            configurationObject.FromAddress = "from";
            configurationObject.SmtpPort = 25;
            configurationObject.SmtpServer = "smtp server";
            configurationObject.SubjectLineEnder = "subject line ender";
            configurationObject.SubjectLineStarter = "subject line starter";
            configurationObject.ToAddress = "to";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.FormatterPropertyName, "overriden formatter");
            machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.FromAddressPropertyName, "overriden from");
            machineKey.AddIntValue(EmailTraceListenerDataManageabilityProvider.SmtpPortPropertyName, 26);
            machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.SmtpServerPropertyName, "overriden smtp server");
            machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.SubjectLineEnderPropertyName, "overriden subject line ender");
            machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.SubjectLineStarterPropertyName, "overriden subject line starter");
            machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.ToAddressPropertyName, "overriden to");
            machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, "ProcessId, ThreadId");
			machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.FilterPropertyName, "Critical");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, null, true, wmiSettings);

            Assert.AreEqual("overriden formatter", configurationObject.Formatter);
            Assert.AreEqual("overriden from", configurationObject.FromAddress);
            Assert.AreEqual(26, configurationObject.SmtpPort);
            Assert.AreEqual("overriden smtp server", configurationObject.SmtpServer);
            Assert.AreEqual("overriden subject line ender", configurationObject.SubjectLineEnder);
            Assert.AreEqual("overriden subject line starter", configurationObject.SubjectLineStarter);
            Assert.AreEqual("overriden to", configurationObject.ToAddress);
            Assert.AreEqual(TraceOptions.ProcessId | TraceOptions.ThreadId, configurationObject.TraceOutputOptions);
			Assert.AreEqual(SourceLevels.Critical, configurationObject.Filter);
		}

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreUserPolicyOverrides()
        {
            configurationObject.Formatter = "formatter";
            configurationObject.FromAddress = "from";
            configurationObject.SmtpPort = 25;
            configurationObject.SmtpServer = "smtp server";
            configurationObject.SubjectLineEnder = "subject line ender";
            configurationObject.SubjectLineStarter = "subject line starter";
            configurationObject.ToAddress = "to";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            userKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.FormatterPropertyName, "overriden formatter");
            userKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.FromAddressPropertyName, "overriden from");
            userKey.AddIntValue(EmailTraceListenerDataManageabilityProvider.SmtpPortPropertyName, 26);
            userKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.SmtpServerPropertyName, "overriden smtp server");
            userKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.SubjectLineEnderPropertyName, "overriden subject line ender");
            userKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.SubjectLineStarterPropertyName, "overriden subject line starter");
            userKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.ToAddressPropertyName, "overriden to");
            userKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, "ProcessId, ThreadId");
			userKey.AddStringValue(CustomTraceListenerDataManageabilityProvider.FilterPropertyName, "Critical");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, userKey, true, wmiSettings);

            Assert.AreEqual("overriden formatter", configurationObject.Formatter);
            Assert.AreEqual("overriden from", configurationObject.FromAddress);
            Assert.AreEqual(26, configurationObject.SmtpPort);
            Assert.AreEqual("overriden smtp server", configurationObject.SmtpServer);
            Assert.AreEqual("overriden subject line ender", configurationObject.SubjectLineEnder);
            Assert.AreEqual("overriden subject line starter", configurationObject.SubjectLineStarter);
            Assert.AreEqual("overriden to", configurationObject.ToAddress);
            Assert.AreEqual(TraceOptions.ProcessId | TraceOptions.ThreadId, configurationObject.TraceOutputOptions);
			Assert.AreEqual(SourceLevels.Critical, configurationObject.Filter);
		}

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfThereArePolicyOverridesButGroupPoliciesAreDisabled()
        {
            configurationObject.Formatter = "formatter";
            configurationObject.FromAddress = "from";
            configurationObject.SmtpPort = 25;
            configurationObject.SmtpServer = "smtp server";
            configurationObject.SubjectLineEnder = "subject line ender";
            configurationObject.SubjectLineStarter = "subject line starter";
            configurationObject.ToAddress = "to";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.FormatterPropertyName, "overriden formatter");
            machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.FromAddressPropertyName, "overriden from");
            machineKey.AddIntValue(EmailTraceListenerDataManageabilityProvider.SmtpPortPropertyName, 26);
            machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.SmtpServerPropertyName, "overriden smtp server");
            machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.SubjectLineEnderPropertyName, "overriden subject line ender");
            machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.SubjectLineStarterPropertyName, "overriden subject line starter");
            machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.ToAddressPropertyName, "overriden to");
            machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, "ProcessId, ThreadId");
			machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.FilterPropertyName, "Critical");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, null, true, wmiSettings);

            Assert.AreEqual("formatter", configurationObject.Formatter);
            Assert.AreEqual("from", configurationObject.FromAddress);
            Assert.AreEqual(25, configurationObject.SmtpPort);
            Assert.AreEqual("smtp server", configurationObject.SmtpServer);
            Assert.AreEqual("subject line ender", configurationObject.SubjectLineEnder);
            Assert.AreEqual("subject line starter", configurationObject.SubjectLineStarter);
            Assert.AreEqual("to", configurationObject.ToAddress);
            Assert.AreEqual(TraceOptions.None, configurationObject.TraceOutputOptions);
			Assert.AreEqual(SourceLevels.Error, configurationObject.Filter);
		}

        [TestMethod]
        public void ConfigurationObjectIsModifiedWithFormatterOverrideWithListItemNone()
        {
            configurationObject.Formatter = "formatter";

            machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.FormatterPropertyName, AdmContentBuilder.NoneListItem);
            machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.FromAddressPropertyName, "overriden from");
            machineKey.AddIntValue(EmailTraceListenerDataManageabilityProvider.SmtpPortPropertyName, 26);
            machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.SmtpServerPropertyName, "overriden smtp server");
            machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.SubjectLineEnderPropertyName, "overriden subject line ender");
            machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.SubjectLineStarterPropertyName, "overriden subject line starter");
            machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.ToAddressPropertyName, "overriden to");
            machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, "ProcessId, ThreadId");
			machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.FilterPropertyName, "Critical");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual("", configurationObject.Formatter);
        }

        [TestMethod]
        public void WmiSettingsAreNotGeneratedIfWmiIsDisabled()
        {
            configurationObject.Formatter = "formatter";
            configurationObject.FromAddress = "from";
            configurationObject.SmtpPort = 25;
            configurationObject.SmtpServer = "smtp server";
            configurationObject.SubjectLineEnder = "subject line ender";
            configurationObject.SubjectLineStarter = "subject line starter";
            configurationObject.ToAddress = "to";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, userKey, false, wmiSettings);

            Assert.AreEqual(0, wmiSettings.Count);
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedIfWmiIsEnabled()
        {
            configurationObject.Formatter = "formatter";
            configurationObject.FromAddress = "from";
            configurationObject.SmtpPort = 25;
            configurationObject.SmtpServer = "smtp server";
            configurationObject.SubjectLineEnder = "subject line ender";
            configurationObject.SubjectLineStarter = "subject line starter";
            configurationObject.ToAddress = "to";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Critical;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(EmailTraceListenerSetting), wmiSettings[0].GetType());
            Assert.AreEqual(configurationObject.Formatter, ((EmailTraceListenerSetting)wmiSettings[0]).Formatter);
            Assert.AreEqual(configurationObject.FromAddress, ((EmailTraceListenerSetting)wmiSettings[0]).FromAddress);
            Assert.AreEqual(configurationObject.SmtpPort, ((EmailTraceListenerSetting)wmiSettings[0]).SmtpPort);
            Assert.AreEqual(configurationObject.SmtpServer, ((EmailTraceListenerSetting)wmiSettings[0]).SmtpServer);
            Assert.AreEqual(configurationObject.SubjectLineEnder, ((EmailTraceListenerSetting)wmiSettings[0]).SubjectLineEnder);
            Assert.AreEqual(configurationObject.SubjectLineStarter, ((EmailTraceListenerSetting)wmiSettings[0]).SubjectLineStarter);
            Assert.AreEqual(configurationObject.ToAddress, ((EmailTraceListenerSetting)wmiSettings[0]).ToAddress);
            Assert.AreEqual(configurationObject.TraceOutputOptions.ToString(), ((EmailTraceListenerSetting)wmiSettings[0]).TraceOutputOptions);
			Assert.AreEqual(configurationObject.Filter.ToString(), ((EmailTraceListenerSetting)wmiSettings[0]).Filter);
		}

        [TestMethod]
        public void WmiSettingsAreGeneratedWithPolicyOverridesIfWmiIsEnabled()
        {
            configurationObject.Formatter = "formatter";
            configurationObject.FromAddress = "from";
            configurationObject.SmtpPort = 25;
            configurationObject.SmtpServer = "smtp server";
            configurationObject.SubjectLineEnder = "subject line ender";
            configurationObject.SubjectLineStarter = "subject line starter";
            configurationObject.ToAddress = "to";
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;

            machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.FormatterPropertyName, "overriden formatter");
            machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.FromAddressPropertyName, "overriden from");
            machineKey.AddIntValue(EmailTraceListenerDataManageabilityProvider.SmtpPortPropertyName, 26);
            machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.SmtpServerPropertyName, "overriden smtp server");
            machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.SubjectLineEnderPropertyName, "overriden subject line ender");
            machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.SubjectLineStarterPropertyName, "overriden subject line starter");
            machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.ToAddressPropertyName, "overriden to");
            machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, "ProcessId, ThreadId");
			machineKey.AddStringValue(EmailTraceListenerDataManageabilityProvider.FilterPropertyName, "Critical");

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(EmailTraceListenerSetting), wmiSettings[0].GetType());
            Assert.AreEqual(configurationObject.Formatter, ((EmailTraceListenerSetting)wmiSettings[0]).Formatter);
            Assert.AreEqual(configurationObject.FromAddress, ((EmailTraceListenerSetting)wmiSettings[0]).FromAddress);
            Assert.AreEqual(configurationObject.SmtpPort, ((EmailTraceListenerSetting)wmiSettings[0]).SmtpPort);
            Assert.AreEqual(configurationObject.SmtpServer, ((EmailTraceListenerSetting)wmiSettings[0]).SmtpServer);
            Assert.AreEqual(configurationObject.SubjectLineEnder, ((EmailTraceListenerSetting)wmiSettings[0]).SubjectLineEnder);
            Assert.AreEqual(configurationObject.SubjectLineStarter, ((EmailTraceListenerSetting)wmiSettings[0]).SubjectLineStarter);
            Assert.AreEqual(configurationObject.ToAddress, ((EmailTraceListenerSetting)wmiSettings[0]).ToAddress);
            Assert.AreEqual(configurationObject.TraceOutputOptions.ToString(), ((EmailTraceListenerSetting)wmiSettings[0]).TraceOutputOptions);
			Assert.AreEqual(configurationObject.Filter.ToString(), ((EmailTraceListenerSetting)wmiSettings[0]).Filter);
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
            Assert.AreEqual(EmailTraceListenerDataManageabilityProvider.FromAddressPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(EmailTraceListenerDataManageabilityProvider.ToAddressPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmNumericPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(EmailTraceListenerDataManageabilityProvider.SmtpPortPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(EmailTraceListenerDataManageabilityProvider.SmtpServerPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(EmailTraceListenerDataManageabilityProvider.SubjectLineStarterPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(EmailTraceListenerDataManageabilityProvider.SubjectLineEnderPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(EmailTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName,
                            partsEnumerator.Current.ValueName);

			Assert.IsTrue(partsEnumerator.MoveNext());
			Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
			Assert.IsNull(partsEnumerator.Current.KeyName);
			Assert.AreEqual(EmailTraceListenerDataManageabilityProvider.FilterPropertyName,
							partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(EmailTraceListenerDataManageabilityProvider.FormatterPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsFalse(partsEnumerator.MoveNext());
            Assert.IsFalse(policiesEnumerator.MoveNext());
        }
    }
}
