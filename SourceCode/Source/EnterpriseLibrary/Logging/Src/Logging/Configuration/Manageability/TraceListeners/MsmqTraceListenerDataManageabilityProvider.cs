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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
    /// <summary>
    /// Provides an implementation for <see cref="FormattedEventLogTraceListenerData"/> that
    /// splits policy overrides processing and WMI objects generation, performing approriate logging of 
    /// policy processing errors.
    /// </summary>
    public class MsmqTraceListenerDataManageabilityProvider
        : TraceListenerDataManageabilityProvider<MsmqTraceListenerData>
    {
        /// <summary>
        /// The name of the message priority property.
        /// </summary>
        public const String MessagePriorityPropertyName = "messagePriority";

        /// <summary>
        /// The name of the queue path property.
        /// </summary>
        public const String QueuePathPropertyName = "queuePath";

        /// <summary>
        /// The name of the recoverable property.
        /// </summary>
        public const String RecoverablePropertyName = "recoverable";

        /// <summary>
        /// The name of the time to be received property.
        /// </summary>
        public const String TimeToBeReceivedPropertyName = "timeToBeReceived";
        
        /// <summary>
        /// The name of the time to reach the queue property.
        /// </summary>
        public const String TimeToReachQueuePropertyName = "timeToReachQueue";

        /// <summary>
        /// The name of the transaction type property.
        /// </summary>
        public const String TransactionTypePropertyName = "transactionType";

        /// <summary>
        /// The name of the use authentication property.
        /// </summary>
        public const String UseAuthenticationPropertyName = "useAuthentication";

        /// <summary>
        /// The name of the use dead letter queue property.
        /// </summary>
        public const String UseDeadLetterQueuePropertyName = "useDeadLetterQueue";

        /// <summary>
        /// The name of the use encryption property.
        /// </summary>
        public const String UseEncryptionPropertyName = "useEncryption";

        /// <summary>
        /// Initialize a new instance of the <see cref="MsmqTraceListenerDataManageabilityProvider"/> class.
        /// </summary>
        public MsmqTraceListenerDataManageabilityProvider()
        {
            MsmqTraceListenerDataWmiMapper.RegisterWmiTypes();
        }

        /// <summary>
        /// Adds the ADM parts that represent the properties of
        /// a specific instance of the configuration element type managed by the receiver.
        /// </summary>
        /// <param name="contentBuilder">The <see cref="AdmContentBuilder"/> to which the Adm instructions are to be appended.</param>
        /// <param name="configurationObject">The configuration object instance.</param>
        /// <param name="configurationSource">The configuration source from where to get additional configuration
        /// information, if necessary.</param>
        /// <param name="elementPolicyKeyName">The key for the element's policies.</param>
        /// <remarks>
        /// Subclasses managing objects that must not create a policy will likely need to include the elements' keys when creating the parts.
        /// </remarks>
        protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder,
                                                                      MsmqTraceListenerData configurationObject,
                                                                      IConfigurationSource configurationSource,
                                                                      String elementPolicyKeyName)
        {
            contentBuilder.AddEditTextPart(Resources.MsmqTraceListenerQueuePathPartName,
                                           QueuePathPropertyName,
                                           configurationObject.QueuePath,
                                           255,
                                           true);

            contentBuilder.AddDropDownListPartForEnumeration<MessagePriority>(Resources.MsmqTraceListenerPriorityPartName,
                                                                              MessagePriorityPropertyName,
                                                                              configurationObject.MessagePriority);

            contentBuilder.AddEditTextPart(Resources.MsmqTraceListenerTtbrPartName,
                                           TimeToBeReceivedPropertyName,
                                           Convert.ToString(configurationObject.TimeToBeReceived, CultureInfo.InvariantCulture),
                                           255,
                                           false);

            contentBuilder.AddEditTextPart(Resources.MsmqTraceListenerTtrqPartName,
                                           TimeToReachQueuePropertyName,
                                           Convert.ToString(configurationObject.TimeToReachQueue, CultureInfo.InvariantCulture),
                                           255,
                                           false);

            contentBuilder.AddCheckboxPart(Resources.MsmqTraceListenerRecoverablePartName,
                                           RecoverablePropertyName,
                                           configurationObject.Recoverable);

            contentBuilder.AddDropDownListPartForEnumeration<MessageQueueTransactionType>(Resources.MsmqTraceListenerTransactionTypePartName,
                                                                                          TransactionTypePropertyName,
                                                                                          configurationObject.TransactionType);

            contentBuilder.AddCheckboxPart(Resources.MsmqTraceListenerUseAuthenticationPartName,
                                           UseAuthenticationPropertyName,
                                           configurationObject.UseAuthentication);

            contentBuilder.AddCheckboxPart(Resources.MsmqTraceListenerUseDeadLetterQueuePartName,
                                           UseDeadLetterQueuePropertyName,
                                           configurationObject.UseDeadLetterQueue);

            contentBuilder.AddCheckboxPart(Resources.MsmqTraceListenerUseEncryptionPartName,
                                           UseEncryptionPropertyName,
                                           configurationObject.UseEncryption);

            AddTraceOptionsPart(contentBuilder, configurationObject.TraceOutputOptions);

			AddFilterPart(contentBuilder, configurationObject.Filter);

            AddFormattersPart(contentBuilder, configurationObject.Formatter, configurationSource);
        }

        /// <summary>
        /// Creates the <see cref="ConfigurationSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
        protected override void GenerateWmiObjects(MsmqTraceListenerData configurationObject,
                                                   ICollection<ConfigurationSetting> wmiSettings)
        {
            MsmqTraceListenerDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
        }

        TimeSpan GetTimeSpanOverride(IRegistryKey policyKey,
                                     String propertyName)
        {
            TimeSpan result;

            String overrideValue = policyKey.GetStringValue(propertyName);
            if (!TimeSpan.TryParse(overrideValue, out result))
            {
                throw new RegistryAccessException(
                    String.Format(Resources.Culture,
                                  Resources.ExceptionErrorValueNotTimeSpan,
                                  policyKey.Name,
                                  propertyName,
                                  overrideValue));
            }

            return result;
        }

        /// <summary>
        /// Overrides the <paramref name="configurationObject"/>'s properties with the Group Policy values from the 
        /// registry.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="policyKey">The <see cref="IRegistryKey"/> which holds the Group Policy overrides for the 
        /// configuration element.</param>
        /// <remarks>Subclasses implementing this method must retrieve all the override values from the registry
        /// before making modifications to the <paramref name="configurationObject"/> so any error retrieving
        /// the override values will cancel policy processing.</remarks>
        protected override void OverrideWithGroupPolicies(MsmqTraceListenerData configurationObject,
                                                          IRegistryKey policyKey)
        {
            String formatterOverride = GetFormatterPolicyOverride(policyKey);
            MessagePriority? messagePriorityOverride = policyKey.GetEnumValue<MessagePriority>(MessagePriorityPropertyName);
            String queuePathOverride = policyKey.GetStringValue(QueuePathPropertyName);
            bool? recoverableOverride = policyKey.GetBoolValue(RecoverablePropertyName);
            TimeSpan timeToBeReceivedOverride = GetTimeSpanOverride(policyKey, TimeToBeReceivedPropertyName);
            TimeSpan timeToReachQueueOverride = GetTimeSpanOverride(policyKey, TimeToReachQueuePropertyName);
            TraceOptions? traceOutputOptionsOverride = policyKey.GetEnumValue<TraceOptions>(TraceOutputOptionsPropertyName);
			SourceLevels? filterOverride = policyKey.GetEnumValue<SourceLevels>(FilterPropertyName);
			MessageQueueTransactionType? transactionTypeOverride
                = policyKey.GetEnumValue<MessageQueueTransactionType>(TransactionTypePropertyName);
            bool? usedAuthenticationOverride = policyKey.GetBoolValue(UseAuthenticationPropertyName);
            bool? useDeadLetterOverride = policyKey.GetBoolValue(UseDeadLetterQueuePropertyName);
            bool? useEncryptionOverride = policyKey.GetBoolValue(UseEncryptionPropertyName);

            configurationObject.Formatter = formatterOverride;
            configurationObject.MessagePriority = messagePriorityOverride.Value;
            configurationObject.QueuePath = queuePathOverride;
            configurationObject.Recoverable = recoverableOverride.Value;
            configurationObject.TimeToReachQueue = timeToReachQueueOverride;
            configurationObject.TimeToBeReceived = timeToBeReceivedOverride;
            configurationObject.TraceOutputOptions = traceOutputOptionsOverride.Value;
			configurationObject.Filter = filterOverride.Value;
			configurationObject.TransactionType = transactionTypeOverride.Value;
            configurationObject.UseAuthentication = usedAuthenticationOverride.Value;
            configurationObject.UseDeadLetterQueue = useDeadLetterOverride.Value;
            configurationObject.UseEncryption = useEncryptionOverride.Value;
        }
    }
}
