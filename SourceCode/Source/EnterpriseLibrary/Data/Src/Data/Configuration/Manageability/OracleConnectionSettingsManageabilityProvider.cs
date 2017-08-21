//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Manageability.Properties;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Manageability
{
    /// <summary>
    /// Provides an implementation for <see cref="OracleConnectionSettings"/> that splits
    /// policy override processing and WMI object generation for the section, performing appropriate logging of
    /// policy processing errors, from policy override processing and WMI object generation for configuration objects
    /// contained by the section.
    /// </summary>
    public class OracleConnectionSettingsManageabilityProvider
        : ConfigurationSectionManageabilityProviderBase<OracleConnectionSettings>
    {
        /// <summary>
        /// The name of the packages property.
        /// </summary>
        public const String PackagesPropertyName = "packages";

        /// <summary>
        /// Initialize a new instance of the <see cref="OracleConnectionSettingsManageabilityProvider"/> class witha a set of sub providers.
        /// </summary>
        /// <param name="subProviders">A set of sub providers.</param>
        public OracleConnectionSettingsManageabilityProvider(IDictionary<Type, ConfigurationElementManageabilityProvider> subProviders)
            : base(subProviders)
        {
            OracleConnectionSettingsWmiMapper.RegisterWmiTypes();
        }

        /// <summary>
        /// Gets the name of the category that represents the whole configuration section.
        /// </summary>
        protected override string SectionCategoryName
        {
            get { return Resources.DatabaseCategoryName; }
        }

        /// <summary>
        /// Gets the name of the managed configuration section.
        /// </summary>
        protected override string SectionName
        {
            get { return OracleConnectionSettings.SectionName; }
        }

        /// <summary>
        /// Adds the ADM instructions that describe the policies that can be used to override the configuration
        /// information represented by a configuration section.
        /// </summary>
        /// <param name="contentBuilder">The <see cref="AdmContentBuilder"/> to which the Adm instructions are to be appended.</param>
        /// <param name="configurationSection">The configuration section instance.</param>
        /// <param name="configurationSource">The configuration source from where to get additional configuration
        /// information, if necessary.</param>
        /// <param name="sectionKey">The root key for the section's policies.</param>
        protected override void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder,
                                                                    OracleConnectionSettings configurationSection,
                                                                    IConfigurationSource configurationSource,
                                                                    String sectionKey)
        {
            contentBuilder.StartCategory(Resources.OracleConnectionsCategoryName);
            {
                foreach (OracleConnectionData data in configurationSection.OracleConnectionsData)
                {
                    String connectionPolicyKey = sectionKey + @"\" + data.Name;

                    contentBuilder.StartPolicy(String.Format(CultureInfo.InvariantCulture,
                                                             Resources.OracleConnectionPolicyNameTemplate,
                                                             data.Name),
                                               connectionPolicyKey);
                    {
                        contentBuilder.AddEditTextPart(Resources.OracleConnectionPackagesPartName,
                                                       PackagesPropertyName,
                                                       GenerateRulesString(data.Packages),
                                                       1024,
                                                       true);
                    }
                    contentBuilder.EndPolicy();
                }
            }
            contentBuilder.EndCategory();
        }

        static String GenerateRulesString(IEnumerable<OraclePackageData> packages)
        {
            KeyValuePairEncoder encoder = new KeyValuePairEncoder();

            foreach (OraclePackageData packageData in packages)
            {
                encoder.AppendKeyValuePair(packageData.Name, packageData.Prefix);
            }

            return encoder.GetEncodedKeyValuePairs();
        }

        /// <summary>
        /// Creates the <see cref="ConfigurationSetting"/> instances that describe the <paramref name="configurationSection"/>.
        /// </summary>
        /// <param name="configurationSection">The configuration section that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
        protected override void GenerateWmiObjectsForConfigurationSection(OracleConnectionSettings configurationSection,
                                                                          ICollection<ConfigurationSetting> wmiSettings)
        {
            OracleConnectionSettingsWmiMapper.GenerateWmiObjects(configurationSection, wmiSettings);
        }

        /// <summary>
        /// Overrides the <paramref name="configurationSection"/>'s configuration elements' properties 
        /// with the Group Policy values from the registry, if any, and creates the <see cref="ConfigurationSetting"/> 
        /// instances that describe these configuration elements.
        /// </summary>
        /// <param name="configurationSection">The configuration section that must be managed.</param>
        /// <param name="readGroupPolicies"><see langword="true"/> if Group Policy overrides must be applied; otherwise, 
        /// <see langword="false"/>.</param>
        /// <param name="machineKey">The <see cref="IRegistryKey"/> which holds the Group Policy overrides for the 
        /// configuration section at the machine level, or <see langword="null"/> 
        /// if there is no such registry key.</param>
        /// <param name="userKey">The <see cref="IRegistryKey"/> which holds the Group Policy overrides for the 
        /// configuration section at the user level, or <see langword="null"/> 
        /// if there is no such registry key.</param>
        /// <param name="generateWmiObjects"><see langword="true"/> if WMI objects must be generated; otherwise, 
        /// <see langword="false"/>.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
        protected override void OverrideWithGroupPoliciesAndGenerateWmiObjectsForConfigurationElements(OracleConnectionSettings configurationSection,
                                                                                                       bool readGroupPolicies,
                                                                                                       IRegistryKey machineKey,
                                                                                                       IRegistryKey userKey,
                                                                                                       bool generateWmiObjects,
                                                                                                       ICollection<ConfigurationSetting> wmiSettings)
        {
            List<OracleConnectionData> connectionsToRemove = new List<OracleConnectionData>();

            foreach (OracleConnectionData connectionData in configurationSection.OracleConnectionsData)
            {
                IRegistryKey machineConnectionKey = null;
                IRegistryKey userConnectionKey = null;

                try
                {
                    LoadRegistrySubKeys(connectionData.Name,
                                        machineKey, userKey,
                                        out machineConnectionKey, out userConnectionKey);

                    if (!OverrideWithGroupPoliciesAndGenerateWmiObjectsForOracleConnection(connectionData,
                                                                                           readGroupPolicies, machineConnectionKey, userConnectionKey,
                                                                                           generateWmiObjects, wmiSettings))
                    {
                        connectionsToRemove.Add(connectionData);
                    }
                }
                finally
                {
                    ReleaseRegistryKeys(machineConnectionKey, userConnectionKey);
                }
            }

            foreach (OracleConnectionData connectionData in connectionsToRemove)
            {
                configurationSection.OracleConnectionsData.Remove(connectionData.Name);
            }
        }

        bool OverrideWithGroupPoliciesAndGenerateWmiObjectsForOracleConnection(OracleConnectionData connectionData,
                                                                               bool readGroupPolicies,
                                                                               IRegistryKey machineKey,
                                                                               IRegistryKey userKey,
                                                                               bool generateWmiObjects,
                                                                               ICollection<ConfigurationSetting> wmiSettings)
        {
            if (readGroupPolicies)
            {
                IRegistryKey policyKey = machineKey ?? userKey;
                if (policyKey != null)
                {
                    if (policyKey.IsPolicyKey && !policyKey.GetBoolValue(PolicyValueName).Value)
                    {
                        return false;
                    }
                    try
                    {
                        String packagesOverride = policyKey.GetStringValue(PackagesPropertyName);

                        connectionData.Packages.Clear();
                        Dictionary<String, String> packagesDictionary = new Dictionary<string, string>();
                        KeyValuePairParser.ExtractKeyValueEntries(packagesOverride, packagesDictionary);
                        foreach (KeyValuePair<String, String> kvp in packagesDictionary)
                        {
                            connectionData.Packages.Add(new OraclePackageData(kvp.Key, kvp.Value));
                        }
                    }
                    catch (RegistryAccessException ex)
                    {
                        LogExceptionWhileOverriding(ex);
                    }
                }
            }
            if (generateWmiObjects)
            {
                OracleConnectionSettingsWmiMapper.GenerateOracleConnectionSettingWmiObjects(connectionData, wmiSettings);
            }

            return true;
        }

        /// <summary>
        /// Overrides the <paramref name="configurationSection"/>'s properties with the Group Policy values from 
        /// the registry.
        /// </summary>
        /// <param name="configurationSection">The configuration section that must be managed.</param>
        /// <param name="policyKey">The <see cref="IRegistryKey"/> which holds the Group Policy overrides.</param>
        protected override void OverrideWithGroupPoliciesForConfigurationSection(OracleConnectionSettings configurationSection,
                                                                                 IRegistryKey policyKey)
        {
            // no section values to override
        }
    }
}
