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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
    /// <summary>
    /// Representa a manageable configuration source (like group policy).
    /// </summary>
    public class ManageableConfigurationSourceImplementation : IDisposable
    {
        readonly object configurationUpdateLock = new object();
        IConfigurationAccessor currentConfigurationAccessor;
        readonly ExeConfigurationFileMap fileMap;

        IGroupPolicyWatcher groupPolicyWatcher;
        IManageabilityHelper manageabilityHelper;
        readonly ConfigurationChangeNotificationCoordinator notificationCoordinator;
        ConfigurationChangeWatcherCoordinator watcherCoordinator;

        /// <summary>
        /// Initialize a new instance of the <see cref="ManageableConfigurationSourceImplementation"/> class.
        /// </summary>
        /// <param name="configurationFilePath">The path to the configuration file.</param>
        /// <param name="refresh">true to refresh configuration; otherwise, false.</param>
        /// <param name="manageabilityProviders">The providers used for managment.</param>
        /// <param name="readGroupPolicies">true to read group policy; otherwise, false.</param>
        /// <param name="generateWmiObjects">true to generate wmi objects; otherwise, false.</param>
        /// <param name="applicationName">The name of the application.</param>
        public ManageableConfigurationSourceImplementation(string configurationFilePath,
                                                           bool refresh,
                                                           IDictionary<string, ConfigurationSectionManageabilityProvider> manageabilityProviders,
                                                           bool readGroupPolicies,
                                                           bool generateWmiObjects,
                                                           string applicationName)
            : this(configurationFilePath,
                   new ManageabilityHelper(manageabilityProviders, readGroupPolicies, generateWmiObjects, applicationName),
                   new GroupPolicyWatcher(),
                   new ConfigurationChangeWatcherCoordinator(configurationFilePath, refresh),
                   new ConfigurationChangeNotificationCoordinator()) {}

        /// <summary>
        /// Initialize a new instance o the <see cref="ManageableConfigurationSourceImplementation"/> class.
        /// </summary>
        /// <param name="configurationFilePath">The configuration file path.</param>
        /// <param name="manageabilityHelper">The <see cref="IManageabilityHelper"/> to use.</param>
        /// <param name="groupPolicyWatcher">The <see cref="IGroupPolicyWatcher"/> to use.</param>
        /// <param name="watcherCoordinator">The <see cref="ConfigurationChangeWatcherCoordinator"/> to use.</param>
        /// <param name="notificationCoordinator">The <see cref="ConfigurationChangeNotificationCoordinator"/> to use.</param>
        public ManageableConfigurationSourceImplementation(string configurationFilePath,
                                                             IManageabilityHelper manageabilityHelper,
                                                             IGroupPolicyWatcher groupPolicyWatcher,
                                                             ConfigurationChangeWatcherCoordinator watcherCoordinator,
                                                             ConfigurationChangeNotificationCoordinator notificationCoordinator)
        {
            fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = configurationFilePath;

            this.notificationCoordinator = notificationCoordinator;
            AttachManageabilityHelper(manageabilityHelper);
            AttachGroupPolicyWatcher(groupPolicyWatcher);
            AttachWatcherCoordinator(watcherCoordinator);

            InitializeConfiguration();
        }
        
        /// <summary>
        /// Gets the manageability helper for the source.
        /// </summary>
        /// <value>
        /// The manageability helper for the source.
        /// </value>
        public IManageabilityHelper ManageabilityHelper
        {
            get { return manageabilityHelper; }
        }

        /// <summary>
        /// Adds a change handler for a section.
        /// </summary>
        /// <param name="sectionName">
        /// The section to add the handler.
        /// </param>
        /// <param name="handler">
        /// The handler to add.
        /// </param>
        public void AddSectionChangeHandler(string sectionName,
                                            ConfigurationChangedEventHandler handler)
        {
            notificationCoordinator.AddSectionChangeHandler(sectionName, handler);
        }

        void AttachGroupPolicyWatcher(IGroupPolicyWatcher groupPolicyWatcher)
        {
            this.groupPolicyWatcher = groupPolicyWatcher;
            this.groupPolicyWatcher.GroupPolicyUpdated += OnGroupPolicyUpdated;
            this.groupPolicyWatcher.StartWatching();
        }

        void AttachManageabilityHelper(IManageabilityHelper manageabilityHelper)
        {
            this.manageabilityHelper = manageabilityHelper;
            this.manageabilityHelper.ConfigurationSettingChanged += OnConfigurationSettingChanged;
        }

        void AttachWatcherCoordinator(ConfigurationChangeWatcherCoordinator watcherCoordinator)
        {
            this.watcherCoordinator = watcherCoordinator;
            this.watcherCoordinator.ConfigurationChanged += OnConfigurationChanged;
        }

        ///<summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        ///</summary>
        public void Dispose()
        {
            groupPolicyWatcher.Dispose();
            watcherCoordinator.Dispose();
        }

        ///<summary>
        /// Get the configuration section.
        ///</summary>
        ///<param name="sectionName">The section name to get.</param>
        ///<returns>A <see cref="ConfigurationSection"/> for the section name.</returns>
        public ConfigurationSection GetSection(string sectionName)
        {
            // always request to current accessor, no need for lock
            ConfigurationSection section = currentConfigurationAccessor.GetSection(sectionName);

            if (section != null)
            {
                lock (configurationUpdateLock)
                {
                    watcherCoordinator.SetWatcherForConfigSource(section.SectionInformation.ConfigSource);
                }
            }

            return section;
        }

        void InitializeConfiguration()
        {
            currentConfigurationAccessor
                = new ConfigurationInstanceConfigurationAccessor(
                    ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None));

            manageabilityHelper.UpdateConfigurationManageability(currentConfigurationAccessor);
            foreach (String managedSectionName in currentConfigurationAccessor.GetRequestedSectionNames())
            {
                ConfigurationSection configurationSection = currentConfigurationAccessor.GetSection(managedSectionName);
                if (configurationSection != null)
                {
                    watcherCoordinator.SetWatcherForConfigSource(configurationSection.SectionInformation.ConfigSource);
                }
            }
        }

        void OnConfigurationChanged(object sender,
                                    ConfigurationChangedEventArgs e)
        {
            UpdateConfiguration(e.SectionName);
        }

        void OnConfigurationSettingChanged(object sender,
                                           ConfigurationSettingChangedEventArgs e)
        {
            UpdateConfigurationSection(e.SectionName);
        }

        void OnGroupPolicyUpdated(bool machine)
        {
            UpdateConfiguration(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource);
        }

        ///<summary>
        /// Removes a change handler from a section.
        ///</summary>
        ///<param name="sectionName">
        /// The section to remove the handler.
        /// </param>
        ///<param name="handler">
        /// The handler to remove.
        /// </param>
        public void RemoveSectionChangeHandler(string sectionName,
                                               ConfigurationChangedEventHandler handler)
        {
            notificationCoordinator.RemoveSectionChangeHandler(sectionName, handler);
        }

        void UpdateConfiguration(String configSource)
        {
            lock (configurationUpdateLock)
            {
                ConfigurationInstanceConfigurationAccessor updatedConfigurationAccessor
                    = new ConfigurationInstanceConfigurationAccessor(ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None));

                manageabilityHelper.UpdateConfigurationManageability(updatedConfigurationAccessor);

                List<String> sectionsToNotify = new List<String>();
                bool notifyAll = ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource.Equals(configSource);

                foreach (String sectionName in currentConfigurationAccessor.GetRequestedSectionNames())
                {
                    ConfigurationSection currentSection = currentConfigurationAccessor.GetSection(sectionName);
                    ConfigurationSection updatedSection = updatedConfigurationAccessor.GetSection(sectionName);

                    if (currentSection != null || updatedSection != null)
                    {
                        UpdateWatchers(currentSection, updatedSection);

                        // notify if:
                        // - instructed to notify all
                        // - any of the versions is null, or its config source matches the changed source
                        if (notifyAll
                            || (updatedSection == null || configSource.Equals(updatedSection.SectionInformation.ConfigSource))
                            || (currentSection == null || configSource.Equals(currentSection.SectionInformation.ConfigSource)))
                        {
                            sectionsToNotify.Add(sectionName);
                        }
                    }
                }

                currentConfigurationAccessor = updatedConfigurationAccessor;
                notificationCoordinator.NotifyUpdatedSections(sectionsToNotify);
            }
        }

        void UpdateConfigurationSection(string sectionName)
        {
            lock (configurationUpdateLock)
            {
                manageabilityHelper.UpdateConfigurationSectionManageability(currentConfigurationAccessor, sectionName);

                notificationCoordinator.NotifyUpdatedSections(new string[] { sectionName });
            }
        }

        void UpdateWatchers(ConfigurationSection currentSection,
                            ConfigurationSection updatedSection)
        {
            if (currentSection != null)
            {
                // remove the watcher for the 'old' section if it's not the main and it's different from the new, or the section was removed
                if (updatedSection == null
                    || !currentSection.SectionInformation.ConfigSource.Equals(updatedSection.SectionInformation.ConfigSource))
                {
                    if (!ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource.Equals(currentSection.SectionInformation.ConfigSource))
                    {
                        watcherCoordinator.RemoveWatcherForConfigSource(currentSection.SectionInformation.ConfigSource);
                    }
                }

                // add the watcher for the new source, if exists
                if (updatedSection != null)
                {
                    watcherCoordinator.SetWatcherForConfigSource(updatedSection.SectionInformation.ConfigSource);
                }
            }
            else
            {
                // section restored, just add the watcher on the new source
                watcherCoordinator.SetWatcherForConfigSource(updatedSection.SectionInformation.ConfigSource);
            }
        }
    }
}
