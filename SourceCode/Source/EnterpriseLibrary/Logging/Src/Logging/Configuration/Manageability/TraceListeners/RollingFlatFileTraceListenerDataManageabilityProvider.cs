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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
    /// <summary>
    /// Provides an implementation for <see cref="RollingFlatFileTraceListenerData"/> that
    /// splits policy overrides processing and WMI objects generation, performing approriate logging of 
    /// policy processing errors.
    /// </summary>
    public class RollingFlatFileTraceListenerDataManageabilityProvider
        : TraceListenerDataManageabilityProvider<RollingFlatFileTraceListenerData>
    {
        /// <summary>
        /// The name of the file name property.
        /// </summary>
        public const String FileNamePropertyName = "fileName";

        /// <summary>
        /// The name of the footer property.
        /// </summary>
        public const String FooterPropertyName = "footer";

        /// <summary>
        /// The name of the header property.
        /// </summary>
        public const String HeaderPropertyName = "header";

        /// <summary>
        /// The name of the file exists behaviour property.
        /// </summary>
        public const String RollFileExistsBehaviorPropertyName = "rollFileExistsBehavior";

        /// <summary>
        /// The name of the roll interval property.
        /// </summary>
        public const String RollIntervalPropertyName = "rollInterval";

        /// <summary>
        /// The name of the roll file size in kilobytes property.
        /// </summary>
        public const String RollSizeKBPropertyName = "rollSizeKB";

        /// <summary>
        /// The name of the time stamp property.
        /// </summary>
        public const String TimeStampPatternPropertyName = "timeStampPattern";

        /// <summary>
        /// Initialize a new instance of the <see cref="RollingFlatFileTraceListenerDataManageabilityProvider"/> class.
        /// </summary>
        public RollingFlatFileTraceListenerDataManageabilityProvider()
        {
            RollingFlatFileTraceListenerDataWmiMapper.RegisterWmiTypes();
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
                                                                      RollingFlatFileTraceListenerData configurationObject,
                                                                      IConfigurationSource configurationSource,
                                                                      String elementPolicyKeyName)
        {
            contentBuilder.AddEditTextPart(Resources.RollingFlatFileTraceListenerFileNamePartName,
                                           FileNamePropertyName,
                                           configurationObject.FileName,
                                           255,
                                           true);

            contentBuilder.AddEditTextPart(Resources.FlatFileTraceListenerHeaderPartName,
                                           HeaderPropertyName,
                                           configurationObject.Header,
                                           512,
                                           false);

            contentBuilder.AddEditTextPart(Resources.FlatFileTraceListenerFooterPartName,
                                           FooterPropertyName,
                                           configurationObject.Footer,
                                           512,
                                           false);

            contentBuilder.AddDropDownListPartForEnumeration<RollFileExistsBehavior>(Resources.RollingFlatFileTraceListenerRollFileExistsBehaviorPartName,
                                                                                     RollFileExistsBehaviorPropertyName,
                                                                                     configurationObject.RollFileExistsBehavior);

            contentBuilder.AddDropDownListPartForEnumeration<RollInterval>(Resources.RollingFlatFileTraceListenerRollIntervalPartName,
                                                                           RollIntervalPropertyName,
                                                                           configurationObject.RollInterval);

            contentBuilder.AddNumericPart(Resources.RollingFlatFileTraceListenerRollSizeKBPartName,
                                          RollSizeKBPropertyName,
                                          configurationObject.RollSizeKB);

            contentBuilder.AddEditTextPart(Resources.RollingFlatFileTraceListenerTimeStampPatternPartName,
                                           TimeStampPatternPropertyName,
                                           configurationObject.TimeStampPattern,
                                           512,
                                           true);

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
        protected override void GenerateWmiObjects(RollingFlatFileTraceListenerData configurationObject,
                                                   ICollection<ConfigurationSetting> wmiSettings)
        {
            RollingFlatFileTraceListenerDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
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
        protected override void OverrideWithGroupPolicies(RollingFlatFileTraceListenerData configurationObject,
                                                          IRegistryKey policyKey)
        {
            String fileNameOverride = policyKey.GetStringValue(FileNamePropertyName);
            String formatterOverride = GetFormatterPolicyOverride(policyKey);
            RollFileExistsBehavior? rollFileExistsBehaviorOverride = policyKey.GetEnumValue<RollFileExistsBehavior>(RollFileExistsBehaviorPropertyName);
            RollInterval? rollIntervalOverride = policyKey.GetEnumValue<RollInterval>(RollIntervalPropertyName);
            int? rollSizeKBOverride = policyKey.GetIntValue(RollSizeKBPropertyName);
            string timeStampPatternOverride = policyKey.GetStringValue(TimeStampPatternPropertyName);
            TraceOptions? traceOutputOptionsOverride = policyKey.GetEnumValue<TraceOptions>(TraceOutputOptionsPropertyName);
			SourceLevels? filterOverride = policyKey.GetEnumValue<SourceLevels>(FilterPropertyName);
			string headerOverride = policyKey.GetStringValue(HeaderPropertyName);
            string footerOverride = policyKey.GetStringValue(FooterPropertyName);

            configurationObject.FileName = fileNameOverride;
            configurationObject.Header = headerOverride;
            configurationObject.Footer = footerOverride;
            configurationObject.Formatter = formatterOverride;
            configurationObject.RollFileExistsBehavior = rollFileExistsBehaviorOverride.Value;
            configurationObject.RollInterval = rollIntervalOverride.Value;
            configurationObject.RollSizeKB = rollSizeKBOverride.Value;
            configurationObject.TimeStampPattern = timeStampPatternOverride;
            configurationObject.TraceOutputOptions = traceOutputOptionsOverride.Value;
			configurationObject.Filter = filterOverride.Value;
		}
    }
}
