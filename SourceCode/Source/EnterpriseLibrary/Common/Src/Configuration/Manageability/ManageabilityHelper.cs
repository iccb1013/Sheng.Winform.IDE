//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
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
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
    /// <summary>
    /// Represents a manageability configuration helper.
    /// </summary>
    public class ManageabilityHelper : IManageabilityHelper
    {
        readonly string applicationName;
        bool generateWmiObjects;
        readonly IDictionary<string, ConfigurationSectionManageabilityProvider> manageabilityProviders;
        readonly IDictionary<string, IEnumerable<ConfigurationSetting>> publishedSettingsMapping;
        readonly bool readGroupPolicies;
        readonly IRegistryAccessor registryAccessor;
        readonly IWmiPublisher wmiPublisher;

        ///<summary>
        /// Initialize a new instance of a <see cref="ManageabilityHelper"/> class.
        ///</summary>
        ///<param name="manageabilityProviders">The manageability propvodiers.</param>
        ///<param name="readGroupPolicies">true to read group policies; otherwise, false.</param>
        ///<param name="generateWmiObjects">true to generate wmi objects; othrewise, false.</param>
        ///<param name="applicationName">The application name.</param>
        public ManageabilityHelper(IDictionary<string, ConfigurationSectionManageabilityProvider> manageabilityProviders,
                                   bool readGroupPolicies,
                                   bool generateWmiObjects,
                                   string applicationName)
            : this(manageabilityProviders,
                   readGroupPolicies,
                   new RegistryAccessor(),
                   generateWmiObjects,
                   new InstrumentationWmiPublisher(),
                   applicationName) {}

        ///<summary>
        /// Initialize a new instance of the <see cref="ManageabilityHelper"/> class.
        ///</summary>
        ///<param name="manageabilityProviders">The manageability providers.</param>
        ///<param name="readGroupPolicies">true to read group policies; otherwise, false.</param>
        ///<param name="registryAccessor">A registry accessor.</param>
        ///<param name="generateWmiObjects">true to generate wmi objects; othrewise, false.</param>
        ///<param name="wmiPublisher">The wmi publisher.</param>
        ///<param name="applicationName">The application name.</param>
        public ManageabilityHelper(IDictionary<string, ConfigurationSectionManageabilityProvider> manageabilityProviders,
                                   bool readGroupPolicies,
                                   IRegistryAccessor registryAccessor,
                                   bool generateWmiObjects,
                                   IWmiPublisher wmiPublisher,
                                   string applicationName)
        {
            this.manageabilityProviders = manageabilityProviders;
            this.readGroupPolicies = readGroupPolicies;
            this.registryAccessor = registryAccessor;
            this.generateWmiObjects = generateWmiObjects;
            this.wmiPublisher = wmiPublisher;
            this.applicationName = applicationName;

            publishedSettingsMapping = new Dictionary<string, IEnumerable<ConfigurationSetting>>();
        }

        /// <summary>
        /// Gets the manageability providers.
        /// </summary>
        /// <value>
        /// The manageability providers.
        /// </value>
        public IDictionary<string, ConfigurationSectionManageabilityProvider> ManageabilityProviders
        {
            get { return manageabilityProviders; }
        }

        /// <summary>
        /// Builds the section key name.
        /// </summary>
        /// <param name="applicationName">
        /// The application name.
        /// </param>
        /// <param name="sectionName">
        /// The section name.
        /// </param>
        /// <returns>
        /// The section key name.
        /// </returns>
        public static string BuildSectionKeyName(String applicationName,
                                                   String sectionName)
        {
            return Path.Combine(Path.Combine(@"Software\Policies\", applicationName), sectionName);
        }

        /// <summary>
        /// The event that is notified when a configuration setting is changed.
        /// </summary>
        public event EventHandler<ConfigurationSettingChangedEventArgs> ConfigurationSettingChanged;

        void DoUpdateConfigurationSectionManageability(IConfigurationAccessor configurationAccessor,
                                                       String sectionName)
        {
            if (publishedSettingsMapping.ContainsKey(sectionName))
            {
                RevokeAll(publishedSettingsMapping[sectionName]);
            }

            ConfigurationSectionManageabilityProvider manageabilityProvider = manageabilityProviders[sectionName];

            ConfigurationSection section = configurationAccessor.GetSection(sectionName);
            if (section != null)
            {
                ICollection<ConfigurationSetting> wmiSettings = new List<ConfigurationSetting>();

                IRegistryKey machineKey = null;
                IRegistryKey userKey = null;

                try
                {
                    LoadPolicyRegistryKeys(sectionName, out machineKey, out userKey);

                    if (manageabilityProvider
                        .OverrideWithGroupPoliciesAndGenerateWmiObjects(section,
                                                                        readGroupPolicies, machineKey, userKey,
                                                                        generateWmiObjects, wmiSettings))
                    {
                        publishedSettingsMapping[sectionName] = wmiSettings;

                        PublishAll(wmiSettings, sectionName);
                    }
                    else
                    {
                        configurationAccessor.RemoveSection(sectionName);
                    }
                }
                catch (Exception e)
                {
                    ManageabilityExtensionsLogger.LogException(e, Resources.ExceptionUnexpectedErrorProcessingSection);
                }
                finally
                {
                    ReleasePolicyRegistryKeys(machineKey, userKey);
                }
            }
        }

        void LoadPolicyRegistryKeys(String sectionName,
                                    out IRegistryKey machineKey,
                                    out IRegistryKey userKey)
        {
            if (readGroupPolicies)
            {
                String sectionKeyName = BuildSectionKeyName(applicationName, sectionName);
                machineKey = registryAccessor.LocalMachine.OpenSubKey(sectionKeyName);
                userKey = registryAccessor.CurrentUser.OpenSubKey(sectionKeyName);
            }
            else
            {
                machineKey = null;
                userKey = null;
            }
        }

        void OnConfigurationSettingChanged(object source,
                                           EventArgs args)
        {
            ConfigurationSetting setting = source as ConfigurationSetting;

            if (ConfigurationSettingChanged != null && setting != null)
            {
                ConfigurationSettingChanged(this, new ConfigurationSettingChangedEventArgs(setting.SectionName));
            }
        }

        void PublishAll(IEnumerable<ConfigurationSetting> instances,
                        String sectionName)
        {
            foreach (ConfigurationSetting instance in instances)
            {
                instance.ApplicationName = applicationName;
                instance.SectionName = sectionName;
                instance.Changed += OnConfigurationSettingChanged;
                wmiPublisher.Publish(instance);
            }
        }

        static void ReleasePolicyRegistryKeys(IRegistryKey machineKey,
                                              IRegistryKey userKey)
        {
            if (machineKey != null)
            {
                try
                {
                    machineKey.Close();
                }
                catch (Exception) {}
            }

            if (userKey != null)
            {
                try
                {
                    userKey.Close();
                }
                catch (Exception) {}
            }
        }

        /// <summary>
        /// Revoke all instances from WMI.
        /// </summary>
        /// <param name="instances">
        /// The instances to revoke.
        /// </param>
        public void RevokeAll(IEnumerable<ConfigurationSetting> instances)
        {
            foreach (ConfigurationSetting instance in instances)
            {
                instance.Changed -= OnConfigurationSettingChanged;
                wmiPublisher.Revoke(instance);
            }
        }

        /// <summary>
        /// Updates configuration management from the given configuration.
        /// </summary>
        /// <param name="configurationAccessor">
        /// The accessor for the configuration.
        /// </param>
        public void UpdateConfigurationManageability(IConfigurationAccessor configurationAccessor)
        {
            using (new GroupPolicyLock())
            {
                foreach (String sectionName in manageabilityProviders.Keys)
                {
                    DoUpdateConfigurationSectionManageability(configurationAccessor, sectionName);
                }
            }
        }

        /// <summary>
        /// Updates configuration management from the given configuration in the given section.
        /// </summary>
        /// <param name="configurationAccessor">
        /// The accessor for the configuration.
        /// </param>
        /// <param name="sectionName">
        /// The section to update.
        /// </param>
        public void UpdateConfigurationSectionManageability(IConfigurationAccessor configurationAccessor,
                                                            string sectionName)
        {
            using (new GroupPolicyLock())
            {
                DoUpdateConfigurationSectionManageability(configurationAccessor, sectionName);
            }
        }

        class GroupPolicyLock : IDisposable
        {
            IntPtr machineCriticalSectionHandle;
            IntPtr userCriticalSectionHandle;

            public GroupPolicyLock()
            {
                // lock policy processing, user first
                // see http://msdn.microsoft.com/library/default.asp?url=/library/en-us/policy/policy/entercriticalpolicysection.asp for details

                userCriticalSectionHandle = NativeMethods.EnterCriticalPolicySection(false);
                if (IntPtr.Zero == userCriticalSectionHandle)
                {
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                }

                machineCriticalSectionHandle = NativeMethods.EnterCriticalPolicySection(true);
                if (IntPtr.Zero == machineCriticalSectionHandle)
                {
                    // save the current call's error
                    int hr = Marshal.GetHRForLastWin32Error();

                    // release the user policy section first - don't check for errors as an exception will be thrown
                    NativeMethods.LeaveCriticalPolicySection(userCriticalSectionHandle);

                    Marshal.ThrowExceptionForHR(hr);
                }
            }

            void IDisposable.Dispose()
            {
                // release locks in the reverse order
                // handles shouldn't be null here, as the constructor should have thrown if they were
                // exceptions are not thrown here; critical section locks will be timed out by the O.S.
                NativeMethods.LeaveCriticalPolicySection(machineCriticalSectionHandle);
                NativeMethods.LeaveCriticalPolicySection(userCriticalSectionHandle);
            }
        }
    }
}
