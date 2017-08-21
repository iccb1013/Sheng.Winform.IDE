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
using System.Globalization;
using System.Messaging;
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
    public class MsmqTraceListenerDataManageabilityProviderFixture
    {
        MsmqTraceListenerDataManageabilityProvider provider;
        MockRegistryKey machineKey;
        MockRegistryKey userKey;
        IList<ConfigurationSetting> wmiSettings;
        MsmqTraceListenerData configurationObject;

        [TestInitialize]
        public void SetUp()
        {
            provider = new MsmqTraceListenerDataManageabilityProvider();
            machineKey = new MockRegistryKey(true);
            userKey = new MockRegistryKey(true);
            wmiSettings = new List<ConfigurationSetting>();
            configurationObject = new MsmqTraceListenerData();
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

            Assembly assembly = typeof(MsmqTraceListenerDataManageabilityProvider).Assembly;
            foreach (ConfigurationElementManageabilityProviderAttribute providerAttribute
                in assembly.GetCustomAttributes(typeof(ConfigurationElementManageabilityProviderAttribute), false))
            {
                if (providerAttribute.ManageabilityProviderType.Equals(typeof(MsmqTraceListenerDataManageabilityProvider)))
                {
                    selectedAttribute = providerAttribute;
                    break;
                }
            }

            Assert.IsNotNull(selectedAttribute);
            Assert.AreSame(typeof(LoggingSettingsManageabilityProvider), selectedAttribute.SectionManageabilityProviderType);
            Assert.AreSame(typeof(MsmqTraceListenerData), selectedAttribute.TargetType);
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
            configurationObject.MessagePriority = MessagePriority.Normal;
            configurationObject.QueuePath = "queue";
            configurationObject.Recoverable = true;
            configurationObject.TimeToBeReceived = TimeSpan.FromSeconds(500);
            configurationObject.TimeToReachQueue = TimeSpan.FromSeconds(1000);
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;
			configurationObject.TransactionType = MessageQueueTransactionType.None;
            configurationObject.UseAuthentication = false;
            configurationObject.UseDeadLetterQueue = true;
            configurationObject.UseEncryption = false;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, null, true, wmiSettings);

            Assert.AreEqual("formatter", configurationObject.Formatter);
            Assert.AreEqual(MessagePriority.Normal, configurationObject.MessagePriority);
            Assert.AreEqual("queue", configurationObject.QueuePath);
            Assert.AreEqual(true, configurationObject.Recoverable);
            Assert.AreEqual(TimeSpan.FromSeconds(500), configurationObject.TimeToBeReceived);
            Assert.AreEqual(TimeSpan.FromSeconds(1000), configurationObject.TimeToReachQueue);
            Assert.AreEqual(TraceOptions.None, configurationObject.TraceOutputOptions);
			Assert.AreEqual(SourceLevels.Error, configurationObject.Filter);
			Assert.AreEqual(MessageQueueTransactionType.None, configurationObject.TransactionType);
            Assert.AreEqual(false, configurationObject.UseAuthentication);
            Assert.AreEqual(true, configurationObject.UseDeadLetterQueue);
            Assert.AreEqual(false, configurationObject.UseEncryption);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreMachinePolicyOverrides()
        {
            configurationObject.Formatter = "formatter";
            configurationObject.MessagePriority = MessagePriority.Normal;
            configurationObject.QueuePath = "queue";
            configurationObject.Recoverable = true;
            configurationObject.TimeToBeReceived = TimeSpan.FromSeconds(500);
            configurationObject.TimeToReachQueue = TimeSpan.FromSeconds(1000);
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;
			configurationObject.TransactionType = MessageQueueTransactionType.None;
            configurationObject.UseAuthentication = false;
            configurationObject.UseDeadLetterQueue = true;
            configurationObject.UseEncryption = false;

            machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.FormatterPropertyName, "overriden formatter");
            machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.MessagePriorityPropertyName, MessagePriority.High.ToString());
            machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.QueuePathPropertyName, "overriden queue");
            machineKey.AddBooleanValue(MsmqTraceListenerDataManageabilityProvider.RecoverablePropertyName, false);
            machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.TimeToBeReceivedPropertyName, Convert.ToString(TimeSpan.FromSeconds(100), CultureInfo.CurrentCulture));
            machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.TimeToReachQueuePropertyName, Convert.ToString(TimeSpan.FromSeconds(200), CultureInfo.CurrentCulture));
            machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, "ProcessId, ThreadId");
			machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.FilterPropertyName, "Critical");
			machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.TransactionTypePropertyName, MessageQueueTransactionType.Single.ToString());
            machineKey.AddBooleanValue(MsmqTraceListenerDataManageabilityProvider.UseAuthenticationPropertyName, true);
            machineKey.AddBooleanValue(MsmqTraceListenerDataManageabilityProvider.UseDeadLetterQueuePropertyName, false);
            machineKey.AddBooleanValue(MsmqTraceListenerDataManageabilityProvider.UseEncryptionPropertyName, true);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, null, true, wmiSettings);

            Assert.AreEqual("overriden formatter", configurationObject.Formatter);
            Assert.AreEqual(MessagePriority.High, configurationObject.MessagePriority);
            Assert.AreEqual("overriden queue", configurationObject.QueuePath);
            Assert.AreEqual(false, configurationObject.Recoverable);
            Assert.AreEqual(TimeSpan.FromSeconds(100), configurationObject.TimeToBeReceived);
            Assert.AreEqual(TimeSpan.FromSeconds(200), configurationObject.TimeToReachQueue);
            Assert.AreEqual(TraceOptions.ProcessId | TraceOptions.ThreadId, configurationObject.TraceOutputOptions);
			Assert.AreEqual(SourceLevels.Critical, configurationObject.Filter);
			Assert.AreEqual(MessageQueueTransactionType.Single, configurationObject.TransactionType);
            Assert.AreEqual(true, configurationObject.UseAuthentication);
            Assert.AreEqual(false, configurationObject.UseDeadLetterQueue);
            Assert.AreEqual(true, configurationObject.UseEncryption);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedIfThereAreUserPolicyOverrides()
        {
            configurationObject.Formatter = "formatter";
            configurationObject.MessagePriority = MessagePriority.Normal;
            configurationObject.QueuePath = "queue";
            configurationObject.Recoverable = true;
            configurationObject.TimeToBeReceived = TimeSpan.MaxValue;
            configurationObject.TimeToReachQueue = TimeSpan.MinValue;
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;
			configurationObject.TransactionType = MessageQueueTransactionType.None;
            configurationObject.UseAuthentication = false;
            configurationObject.UseDeadLetterQueue = true;
            configurationObject.UseEncryption = false;

            userKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.FormatterPropertyName, "overriden formatter");
            userKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.MessagePriorityPropertyName, MessagePriority.High.ToString());
            userKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.QueuePathPropertyName, "overriden queue");
            userKey.AddBooleanValue(MsmqTraceListenerDataManageabilityProvider.RecoverablePropertyName, false);
            userKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.TimeToBeReceivedPropertyName, Convert.ToString(TimeSpan.FromSeconds(100), CultureInfo.CurrentCulture));
            userKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.TimeToReachQueuePropertyName, Convert.ToString(TimeSpan.FromSeconds(200), CultureInfo.CurrentCulture));
            userKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, "ProcessId, ThreadId");
			userKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.FilterPropertyName, "Critical");
			userKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.TransactionTypePropertyName, MessageQueueTransactionType.Single.ToString());
            userKey.AddBooleanValue(MsmqTraceListenerDataManageabilityProvider.UseAuthenticationPropertyName, true);
            userKey.AddBooleanValue(MsmqTraceListenerDataManageabilityProvider.UseDeadLetterQueuePropertyName, false);
            userKey.AddBooleanValue(MsmqTraceListenerDataManageabilityProvider.UseEncryptionPropertyName, true);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, null, userKey, true, wmiSettings);

            Assert.AreEqual("overriden formatter", configurationObject.Formatter);
            Assert.AreEqual(MessagePriority.High, configurationObject.MessagePriority);
            Assert.AreEqual("overriden queue", configurationObject.QueuePath);
            Assert.AreEqual(false, configurationObject.Recoverable);
            Assert.AreEqual(TimeSpan.FromSeconds(100), configurationObject.TimeToBeReceived);
            Assert.AreEqual(TimeSpan.FromSeconds(200), configurationObject.TimeToReachQueue);
            Assert.AreEqual(TraceOptions.ProcessId | TraceOptions.ThreadId, configurationObject.TraceOutputOptions);
			Assert.AreEqual(SourceLevels.Critical, configurationObject.Filter);
			Assert.AreEqual(MessageQueueTransactionType.Single, configurationObject.TransactionType);
            Assert.AreEqual(true, configurationObject.UseAuthentication);
            Assert.AreEqual(false, configurationObject.UseDeadLetterQueue);
            Assert.AreEqual(true, configurationObject.UseEncryption);
        }

        [TestMethod]
        public void ConfigurationObjectIsNotModifiedIfThereArePolicyOverridesButGroupPoliciesAreDisabled()
        {
            configurationObject.Formatter = "formatter";
            configurationObject.MessagePriority = MessagePriority.Normal;
            configurationObject.QueuePath = "queue";
            configurationObject.Recoverable = true;
            configurationObject.TimeToBeReceived = TimeSpan.FromSeconds(500);
            configurationObject.TimeToReachQueue = TimeSpan.FromSeconds(1000);
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;
			configurationObject.TransactionType = MessageQueueTransactionType.None;
            configurationObject.UseAuthentication = false;
            configurationObject.UseDeadLetterQueue = true;
            configurationObject.UseEncryption = false;

            machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.FormatterPropertyName, "overriden formatter");
            machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.MessagePriorityPropertyName, MessagePriority.High.ToString());
            machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.QueuePathPropertyName, "overriden queue");
            machineKey.AddBooleanValue(MsmqTraceListenerDataManageabilityProvider.RecoverablePropertyName, false);
            machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.TimeToBeReceivedPropertyName, Convert.ToString(TimeSpan.FromSeconds(100), CultureInfo.CurrentCulture));
            machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.TimeToReachQueuePropertyName, Convert.ToString(TimeSpan.FromSeconds(200), CultureInfo.CurrentCulture));
            machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, "ProcessId, ThreadId");
			machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.FilterPropertyName, "Critical");
			machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.TransactionTypePropertyName, MessageQueueTransactionType.Single.ToString());
            machineKey.AddBooleanValue(MsmqTraceListenerDataManageabilityProvider.UseAuthenticationPropertyName, true);
            machineKey.AddBooleanValue(MsmqTraceListenerDataManageabilityProvider.UseDeadLetterQueuePropertyName, false);
            machineKey.AddBooleanValue(MsmqTraceListenerDataManageabilityProvider.UseEncryptionPropertyName, true);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, null, true, wmiSettings);

            Assert.AreEqual("formatter", configurationObject.Formatter);
            Assert.AreEqual(MessagePriority.Normal, configurationObject.MessagePriority);
            Assert.AreEqual("queue", configurationObject.QueuePath);
            Assert.AreEqual(true, configurationObject.Recoverable);
            Assert.AreEqual(TimeSpan.FromSeconds(500), configurationObject.TimeToBeReceived);
            Assert.AreEqual(TimeSpan.FromSeconds(1000), configurationObject.TimeToReachQueue);
            Assert.AreEqual(TraceOptions.None, configurationObject.TraceOutputOptions);
			Assert.AreEqual(SourceLevels.Error, configurationObject.Filter);
			Assert.AreEqual(MessageQueueTransactionType.None, configurationObject.TransactionType);
            Assert.AreEqual(false, configurationObject.UseAuthentication);
            Assert.AreEqual(true, configurationObject.UseDeadLetterQueue);
            Assert.AreEqual(false, configurationObject.UseEncryption);
        }

        [TestMethod]
        public void OverridesForTimeSpansAreIgnoredIfFormatIsWrong()
        {
            configurationObject.Formatter = "formatter";
            configurationObject.MessagePriority = MessagePriority.Normal;
            configurationObject.QueuePath = "queue";
            configurationObject.Recoverable = true;
            configurationObject.TimeToBeReceived = TimeSpan.FromSeconds(500);
            configurationObject.TimeToReachQueue = TimeSpan.FromSeconds(1000);
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;
			configurationObject.TransactionType = MessageQueueTransactionType.None;
            configurationObject.UseAuthentication = false;
            configurationObject.UseDeadLetterQueue = true;
            configurationObject.UseEncryption = false;

            machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.FormatterPropertyName, "overriden formatter");
            machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.MessagePriorityPropertyName, MessagePriority.High.ToString());
            machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.QueuePathPropertyName, "overriden queue");
            machineKey.AddBooleanValue(MsmqTraceListenerDataManageabilityProvider.RecoverablePropertyName, false);
            machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.TimeToBeReceivedPropertyName, "invalid");
            machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.TimeToReachQueuePropertyName, "invalid");
            machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, "ProcessId, ThreadId");
			machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.FilterPropertyName, "Critical");
			machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.TransactionTypePropertyName, MessageQueueTransactionType.Single.ToString());
            machineKey.AddBooleanValue(MsmqTraceListenerDataManageabilityProvider.UseAuthenticationPropertyName, true);
            machineKey.AddBooleanValue(MsmqTraceListenerDataManageabilityProvider.UseDeadLetterQueuePropertyName, false);
            machineKey.AddBooleanValue(MsmqTraceListenerDataManageabilityProvider.UseEncryptionPropertyName, true);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, null, true, wmiSettings);

            Assert.AreEqual("formatter", configurationObject.Formatter);
            Assert.AreEqual(MessagePriority.Normal, configurationObject.MessagePriority);
            Assert.AreEqual("queue", configurationObject.QueuePath);
            Assert.AreEqual(true, configurationObject.Recoverable);
            Assert.AreEqual(TimeSpan.FromSeconds(500), configurationObject.TimeToBeReceived);
            Assert.AreEqual(TimeSpan.FromSeconds(1000), configurationObject.TimeToReachQueue);
            Assert.AreEqual(TraceOptions.None, configurationObject.TraceOutputOptions);
			Assert.AreEqual(SourceLevels.Error, configurationObject.Filter);
			Assert.AreEqual(MessageQueueTransactionType.None, configurationObject.TransactionType);
            Assert.AreEqual(false, configurationObject.UseAuthentication);
            Assert.AreEqual(true, configurationObject.UseDeadLetterQueue);
            Assert.AreEqual(false, configurationObject.UseEncryption);
        }

        [TestMethod]
        public void ConfigurationObjectIsModifiedWithFormatterOverrideWithListItemNone()
        {
            configurationObject.Formatter = "formatter";

            machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.FormatterPropertyName, AdmContentBuilder.NoneListItem);
            machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.MessagePriorityPropertyName, MessagePriority.High.ToString());
            machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.QueuePathPropertyName, "overriden queue");
            machineKey.AddBooleanValue(MsmqTraceListenerDataManageabilityProvider.RecoverablePropertyName, false);
            machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.TimeToBeReceivedPropertyName, Convert.ToString(TimeSpan.FromSeconds(100), CultureInfo.CurrentCulture));
            machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.TimeToReachQueuePropertyName, Convert.ToString(TimeSpan.FromSeconds(200), CultureInfo.CurrentCulture));
            machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, "ProcessId, ThreadId");
			machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.FilterPropertyName, "Critical");
			machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.TransactionTypePropertyName, MessageQueueTransactionType.Single.ToString());
            machineKey.AddBooleanValue(MsmqTraceListenerDataManageabilityProvider.UseAuthenticationPropertyName, true);
            machineKey.AddBooleanValue(MsmqTraceListenerDataManageabilityProvider.UseDeadLetterQueuePropertyName, false);
            machineKey.AddBooleanValue(MsmqTraceListenerDataManageabilityProvider.UseEncryptionPropertyName, true);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, true, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual("", configurationObject.Formatter);
        }

        [TestMethod]
        public void WmiSettingsAreNotGeneratedIfWmiIsDisabled()
        {
            configurationObject.Formatter = "formatter";
            configurationObject.MessagePriority = MessagePriority.Normal;
            configurationObject.QueuePath = "queue";
            configurationObject.Recoverable = true;
            configurationObject.TimeToBeReceived = TimeSpan.MaxValue;
            configurationObject.TimeToReachQueue = TimeSpan.MinValue;
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;
			configurationObject.TransactionType = MessageQueueTransactionType.None;
            configurationObject.UseAuthentication = false;
            configurationObject.UseDeadLetterQueue = true;
            configurationObject.UseEncryption = false;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, userKey, false, wmiSettings);

            Assert.AreEqual(0, wmiSettings.Count);
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedIfWmiIsEnabled()
        {
            configurationObject.Formatter = "formatter";
            configurationObject.MessagePriority = MessagePriority.Normal;
            configurationObject.QueuePath = "queue";
            configurationObject.Recoverable = true;
            configurationObject.TimeToBeReceived = TimeSpan.FromSeconds(500);
            configurationObject.TimeToReachQueue = TimeSpan.FromSeconds(1000);
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;
			configurationObject.TransactionType = MessageQueueTransactionType.None;
            configurationObject.UseAuthentication = false;
            configurationObject.UseDeadLetterQueue = true;
            configurationObject.UseEncryption = false;

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(MsmqTraceListenerSetting), wmiSettings[0].GetType());
            Assert.AreEqual(configurationObject.Formatter, ((MsmqTraceListenerSetting)wmiSettings[0]).Formatter);
            Assert.AreEqual(configurationObject.MessagePriority.ToString(), ((MsmqTraceListenerSetting)wmiSettings[0]).MessagePriority);
            Assert.AreEqual(configurationObject.QueuePath, ((MsmqTraceListenerSetting)wmiSettings[0]).QueuePath);
            Assert.AreEqual(configurationObject.Recoverable, ((MsmqTraceListenerSetting)wmiSettings[0]).Recoverable);
            Assert.AreEqual(configurationObject.TimeToBeReceived, TimeSpan.Parse(((MsmqTraceListenerSetting)wmiSettings[0]).TimeToBeReceived));
            Assert.AreEqual(configurationObject.TimeToReachQueue, TimeSpan.Parse(((MsmqTraceListenerSetting)wmiSettings[0]).TimeToReachQueue));
            Assert.AreEqual(configurationObject.TraceOutputOptions.ToString(), ((MsmqTraceListenerSetting)wmiSettings[0]).TraceOutputOptions);
			Assert.AreEqual(configurationObject.Filter.ToString(), ((MsmqTraceListenerSetting)wmiSettings[0]).Filter);
			Assert.AreEqual(configurationObject.TransactionType.ToString(), ((MsmqTraceListenerSetting)wmiSettings[0]).TransactionType);
            Assert.AreEqual(configurationObject.UseAuthentication, ((MsmqTraceListenerSetting)wmiSettings[0]).UseAuthentication);
            Assert.AreEqual(configurationObject.UseDeadLetterQueue, ((MsmqTraceListenerSetting)wmiSettings[0]).UseDeadLetterQueue);
            Assert.AreEqual(configurationObject.UseEncryption, ((MsmqTraceListenerSetting)wmiSettings[0]).UseEncryption);
        }

        [TestMethod]
        public void WmiSettingsAreGeneratedWithPolicyOverridesIfWmiIsEnabled()
        {
            configurationObject.Formatter = "formatter";
            configurationObject.MessagePriority = MessagePriority.Normal;
            configurationObject.QueuePath = "queue";
            configurationObject.Recoverable = true;
            configurationObject.TimeToBeReceived = TimeSpan.FromSeconds(500);
            configurationObject.TimeToReachQueue = TimeSpan.FromSeconds(1000);
            configurationObject.TraceOutputOptions = TraceOptions.None;
			configurationObject.Filter = SourceLevels.Error;
			configurationObject.TransactionType = MessageQueueTransactionType.None;
            configurationObject.UseAuthentication = false;
            configurationObject.UseDeadLetterQueue = true;
            configurationObject.UseEncryption = false;

            machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.FormatterPropertyName, "overriden formatter");
            machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.MessagePriorityPropertyName, MessagePriority.High.ToString());
            machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.QueuePathPropertyName, "overriden queue");
            machineKey.AddBooleanValue(MsmqTraceListenerDataManageabilityProvider.RecoverablePropertyName, false);
            machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.TimeToBeReceivedPropertyName, Convert.ToString(TimeSpan.FromSeconds(100), CultureInfo.CurrentCulture));
            machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.TimeToReachQueuePropertyName, Convert.ToString(TimeSpan.FromSeconds(200), CultureInfo.CurrentCulture));
            machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName, "ProcessId, ThreadId");
			machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.FilterPropertyName, "Critical");
			machineKey.AddStringValue(MsmqTraceListenerDataManageabilityProvider.TransactionTypePropertyName, MessageQueueTransactionType.Single.ToString());
            machineKey.AddBooleanValue(MsmqTraceListenerDataManageabilityProvider.UseAuthenticationPropertyName, true);
            machineKey.AddBooleanValue(MsmqTraceListenerDataManageabilityProvider.UseDeadLetterQueuePropertyName, false);
            machineKey.AddBooleanValue(MsmqTraceListenerDataManageabilityProvider.UseEncryptionPropertyName, true);

            provider.OverrideWithGroupPoliciesAndGenerateWmiObjects(configurationObject, false, machineKey, userKey, true, wmiSettings);

            Assert.AreEqual(1, wmiSettings.Count);
            Assert.AreSame(typeof(MsmqTraceListenerSetting), wmiSettings[0].GetType());
            Assert.AreEqual(configurationObject.Formatter, ((MsmqTraceListenerSetting)wmiSettings[0]).Formatter);
            Assert.AreEqual(configurationObject.MessagePriority.ToString(), ((MsmqTraceListenerSetting)wmiSettings[0]).MessagePriority);
            Assert.AreEqual(configurationObject.QueuePath, ((MsmqTraceListenerSetting)wmiSettings[0]).QueuePath);
            Assert.AreEqual(configurationObject.Recoverable, ((MsmqTraceListenerSetting)wmiSettings[0]).Recoverable);
            Assert.AreEqual(configurationObject.TimeToBeReceived, TimeSpan.Parse(((MsmqTraceListenerSetting)wmiSettings[0]).TimeToBeReceived));
            Assert.AreEqual(configurationObject.TimeToReachQueue, TimeSpan.Parse(((MsmqTraceListenerSetting)wmiSettings[0]).TimeToReachQueue));
            Assert.AreEqual(configurationObject.TraceOutputOptions.ToString(), ((MsmqTraceListenerSetting)wmiSettings[0]).TraceOutputOptions);
			Assert.AreEqual(configurationObject.Filter.ToString(), ((MsmqTraceListenerSetting)wmiSettings[0]).Filter);
			Assert.AreEqual(configurationObject.TransactionType.ToString(), ((MsmqTraceListenerSetting)wmiSettings[0]).TransactionType);
            Assert.AreEqual(configurationObject.UseAuthentication, ((MsmqTraceListenerSetting)wmiSettings[0]).UseAuthentication);
            Assert.AreEqual(configurationObject.UseDeadLetterQueue, ((MsmqTraceListenerSetting)wmiSettings[0]).UseDeadLetterQueue);
            Assert.AreEqual(configurationObject.UseEncryption, ((MsmqTraceListenerSetting)wmiSettings[0]).UseEncryption);
        }

        [TestMethod]
        public void ManageabilityProviderGeneratesProperAdmContent()
        {
            // need to set these because their default values are null.
            configurationObject.Formatter = "formatter";
            configurationObject.QueuePath = "path";

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
            Assert.AreEqual(MsmqTraceListenerDataManageabilityProvider.QueuePathPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(MsmqTraceListenerDataManageabilityProvider.MessagePriorityPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(MsmqTraceListenerDataManageabilityProvider.TimeToBeReceivedPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmEditTextPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(MsmqTraceListenerDataManageabilityProvider.TimeToReachQueuePropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmCheckboxPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(MsmqTraceListenerDataManageabilityProvider.RecoverablePropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(MsmqTraceListenerDataManageabilityProvider.TransactionTypePropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmCheckboxPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(MsmqTraceListenerDataManageabilityProvider.UseAuthenticationPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmCheckboxPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(MsmqTraceListenerDataManageabilityProvider.UseDeadLetterQueuePropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmCheckboxPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(MsmqTraceListenerDataManageabilityProvider.UseEncryptionPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(MsmqTraceListenerDataManageabilityProvider.TraceOutputOptionsPropertyName,
                            partsEnumerator.Current.ValueName);

			Assert.IsTrue(partsEnumerator.MoveNext());
			Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
			Assert.IsNull(partsEnumerator.Current.KeyName);
			Assert.AreEqual(MsmqTraceListenerDataManageabilityProvider.FilterPropertyName,
							partsEnumerator.Current.ValueName);

            Assert.IsTrue(partsEnumerator.MoveNext());
            Assert.AreSame(typeof(AdmDropDownListPart), partsEnumerator.Current.GetType());
            Assert.IsNull(partsEnumerator.Current.KeyName);
            Assert.AreEqual(MsmqTraceListenerDataManageabilityProvider.FormatterPropertyName,
                            partsEnumerator.Current.ValueName);

            Assert.IsFalse(partsEnumerator.MoveNext());
            Assert.IsFalse(policiesEnumerator.MoveNext());
        }
    }
}
