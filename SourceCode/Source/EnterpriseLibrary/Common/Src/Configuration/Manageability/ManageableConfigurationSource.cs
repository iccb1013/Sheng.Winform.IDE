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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
    /// <summary>
    /// Represents a configuration source that retrieves configuration information from an arbitrary file, overrides 
    /// the configuration information with values from the registry's group policy keys, and publishes WMI objects
    /// that represent the configuration information.
    /// </summary>
    /// <remarks>
    /// This configuration source uses a <see cref="System.Configuration.Configuration"/> object to deserialize configuration, so 
    /// the configuration file must be a valid .NET Framework configuration file.
    /// Multiple instances of <see cref="ManageableConfigurationSource"/> can be created with a given configuration; however 
    /// instances with the same configuration will share the same configuration objects, and WMI objects will be published 
    /// only once regardless of how many instances there are.
    /// </remarks>
    /// <seealso cref="FileConfigurationSource"/>
    [ConfigurationElementType(typeof(ManageableConfigurationSourceElement))]
    public class ManageableConfigurationSource : IConfigurationSource
    {
        static ManageableConfigurationSourceSingletonHelper singletonHelper = new ManageableConfigurationSourceSingletonHelper();
        readonly ManageableConfigurationSourceImplementation implementation;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageableConfigurationSource"/> class.
        /// </summary>
        /// <param name="configurationFilePath">The configuration file path. The path can be absolute or relative.</param>
        /// <param name="manageabilityProviders">The <see cref="ConfigurationSectionManageabilityProvider"/> that will
        /// provide manageability for each configuration section.</param>
        /// <param name="readGroupPolicies"><see langword="true"/> if Group Policy overrides must be applied; otherwise, 
        /// <see langword="false"/>.</param>
        /// <param name="generateWmiObjects"><see langword="true"/> if WMI objects must be generated; otherwise, 
        /// <see langword="false"/>.</param>
        /// <param name="applicationName">The name of the running application. This name is used to look for policy overrides
        /// and to identify the published WMI objects.</param>
        public ManageableConfigurationSource(
            string configurationFilePath,
            IDictionary<string, ConfigurationSectionManageabilityProvider> manageabilityProviders,
            bool readGroupPolicies,
            bool generateWmiObjects,
            string applicationName)
            : this(GetManageableConfigurationSourceImplementation(configurationFilePath, manageabilityProviders, readGroupPolicies, generateWmiObjects, applicationName)) {}

        /// <summary>
        /// Initialize a new instance of the <see cref="ManageableConfigurationSource"/> class with the implementation.
        /// </summary>
        /// <param name="implementation">
        /// A <see cref="ManageableConfigurationSourceImplementation"/> to use.
        /// </param>
        public ManageableConfigurationSource(ManageableConfigurationSourceImplementation implementation)
        {
            this.implementation = implementation;
        }

        /// <summary>
        /// Gets the implementation for configuraiton source.
        /// </summary>
        public ManageableConfigurationSourceImplementation Implementation
        {
            get { return implementation; }
        }

        /// <summary>
        /// Adds a <see cref="ConfigurationSection"/> to the configuration source location specified by 
        /// <paramref name="saveParameter"/> and saves the configuration source.
        /// </summary>
        /// <remarks>
        /// This operation is not implemented.
        /// </remarks>
        public void Add(IConfigurationParameter saveParameter,
                        string sectionName,
                        ConfigurationSection configurationSection)
        {
            throw new NotImplementedException(Resources.ManageableConfigurationSourceUpdateNotAvailable);
        }

        /// <summary>
        /// Adds a handler to be called when changes to section <code>sectionName</code> are detected.
        /// This call should always be followed by a <see cref="RemoveSectionChangeHandler"/>. Failure to remove change
        /// handlers will result in .Net resource leaks.
        /// </summary>
        /// <param name="sectionName">The name of the section to watch for.</param>
        /// <param name="handler">The handler.</param>
        public void AddSectionChangeHandler(string sectionName,
                                            ConfigurationChangedEventHandler handler)
        {
            CheckSectionName(sectionName);
            CheckHandler(handler);

            implementation.AddSectionChangeHandler(sectionName, handler);
        }

        static void CheckApplicationName(string applicationName)
        {
            if (String.IsNullOrEmpty(applicationName))
            {
                throw new ArgumentNullException("applicationName");
            }
            if (applicationName.Length > 255)
            {
                throw new ArgumentException(Resources.ExceptionApplicationNameTooLong, "applicationName");
            }
        }

        static void CheckFilePath(string configurationFilePath)
        {
            if (String.IsNullOrEmpty(configurationFilePath))
            {
                throw new ArgumentNullException("configurationFilePath");
            }
        }

        static void CheckHandler(MulticastDelegate handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }
        }

        static void CheckProvidersMapping(IDictionary<string, ConfigurationSectionManageabilityProvider> manageabilityProviders)
        {
            if (manageabilityProviders == null)
            {
                throw new ArgumentNullException("manageabilityProviders");
            }
        }

        static void CheckSectionName(string sectionName)
        {
            if (String.IsNullOrEmpty(sectionName))
            {
                throw new ArgumentNullException("sectionName");
            }
        }

        static ManageableConfigurationSourceImplementation GetManageableConfigurationSourceImplementation(string configurationFilePath,
                                                                                                          IDictionary<string, ConfigurationSectionManageabilityProvider> manageabilityProviders,
                                                                                                          bool readGroupPolicies,
                                                                                                          bool generateWmiObjects,
                                                                                                          string applicationName)
        {
            CheckFilePath(configurationFilePath);
            CheckApplicationName(applicationName);
            CheckProvidersMapping(manageabilityProviders);

            return singletonHelper.GetInstance(configurationFilePath, manageabilityProviders, readGroupPolicies, generateWmiObjects, applicationName);
        }

        /// <summary>
        /// Retrieves the specified <see cref="ConfigurationSection"/>.
        /// </summary>
        /// <param name="sectionName">The name of the section to be retrieved.</param>
        /// <returns>The specified <see cref="ConfigurationSection"/>, or <see langword="null"/>
        /// if a section by that name is not found.</returns>
        public ConfigurationSection GetSection(string sectionName)
        {
            CheckSectionName(sectionName);

            return implementation.GetSection(sectionName);
        }

        /// <summary>
        /// Removes a <see cref="ConfigurationSection"/> from the configuration source location specified by 
        /// <paramref name="removeParameter"/> and saves the configuration source.
        /// </summary>
        /// <remarks>
        /// This operation is not implemented.
        /// </remarks>
        public void Remove(IConfigurationParameter removeParameter,
                           string sectionName)
        {
            throw new NotImplementedException(Resources.ManageableConfigurationSourceUpdateNotAvailable);
        }

        /// <summary>
        /// Remove a handler to be called when changes to section <code>sectionName</code> are detected.
        /// This class should always follow a call to <see cref="AddSectionChangeHandler"/>. Failure
        /// to call these methods in pairs will result in .Net resource leaks.
        /// </summary>
        /// <param name="sectionName">The name of the section to watch for.</param>
        /// <param name="handler">The handler.</param>
        public void RemoveSectionChangeHandler(string sectionName,
                                               ConfigurationChangedEventHandler handler)
        {
            CheckSectionName(sectionName);
            CheckHandler(handler);

            implementation.RemoveSectionChangeHandler(sectionName, handler);
        }

		/// <summary>
		/// Public for testing purposes.
		/// </summary>
        public static void ResetAllImplementations()
        {
            ManageableConfigurationSourceSingletonHelper oldSingletonHelper = singletonHelper;
            singletonHelper = new ManageableConfigurationSourceSingletonHelper();
            oldSingletonHelper.Dispose();
        }
    }
}
