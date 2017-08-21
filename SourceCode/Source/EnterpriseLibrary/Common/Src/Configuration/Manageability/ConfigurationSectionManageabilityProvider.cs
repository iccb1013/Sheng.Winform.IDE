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
using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
    /// <summary>
    /// Represents the behavior required to provide Group Policy updates and to publish the 
    /// <see cref="ConfigurationSetting"/> instances associated to a <see cref="ConfigurationSection"/>.
    /// </summary>
    /// <remarks>
    /// Subclasses define the implementation necessary to provide manageability for a specific type of configuration
    /// section.
    /// Section providers delegate the manageability support for internal configuration elements to registered instances
    /// of <see cref="ConfigurationElementManageabilityProvider"/> when collections of heterogeneous elements are involved 
    /// and the concrete type of the configuration elements is unknown in advance. 
    /// Section providers are registered with the configuration section name they provide manageability to using 
    /// the <see cref="ConfigurationSectionManageabilityProviderAttribute"/> attribute, which is bound to assemblies.
    /// Section providers are also responsible for generating the ADM instructions that describe the policies that can be
    /// used to override the values for all the configuration settings in the section. Usually the ADM instructions generated 
    /// for a section consist of a policy for block-wide settings and one policy for each configuration element in a collection; 
    /// however some sections might require a different structure. Manageability providers for elements in a section must be 
    /// consistent with the ADM structure defined by the section's manageability provider.
    /// </remarks>
    /// <seealso cref="ConfigurationElementManageabilityProvider"/>
    /// <seealso cref="ConfigurationSectionManageabilityProviderAttribute"/>
    public abstract class ConfigurationSectionManageabilityProvider
    {
        /// <summary>
        /// The name of the value used to hold policy enablement status.
        /// </summary>
        public const String PolicyValueName = RegistryKeyBase.PolicyValueName;

        readonly IDictionary<Type, ConfigurationElementManageabilityProvider> providers;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationSectionManageabilityProvider"/> class with a 
        /// given set of manageability providers for the elements in the section's collections.
        /// </summary>
        /// <param name="providers">The mapping from configuration element type to
        /// <see cref="ConfigurationElementManageabilityProvider"/>.</param>
        protected ConfigurationSectionManageabilityProvider(IDictionary<Type, ConfigurationElementManageabilityProvider> providers)
        {
            this.providers = providers;
        }

        /// <summary>
        /// Gets the mapping from configuration element type to
        /// <see cref="ConfigurationElementManageabilityProvider"/>
        /// </summary>
        /// <value>
        /// The mapping from configuration element type to
        /// <see cref="ConfigurationElementManageabilityProvider"/>
        /// </value>
        public IDictionary<Type, ConfigurationElementManageabilityProvider> Providers
        {
            get { return providers; }
        }

        /// <summary>
        /// Adds the ADM instructions that describe the policies that can be used to override the configuration
        /// information represented by a configuration section.
        /// </summary>
        /// <param name="contentBuilder">The <see cref="AdmContentBuilder"/> to which the Adm instructions are to be appended.</param>
        /// <param name="configurationObject">The configuration section instance.</param>
        /// <param name="configurationSource">The configuration source from where to get additional configuration
        /// information, if necessary.</param>
        /// <param name="applicationName">The key path for which the generated instructions' keys must be sub keys of.</param>
        public abstract void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder,
                                                                 ConfigurationSection configurationObject,
                                                                 IConfigurationSource configurationSource,
                                                                 String applicationName);

        /// <summary>
        /// Adds the ADM instructions that describe the policies that can be used to override the configuration
        /// information for the element using the supplied element manageability provider.
        /// </summary>
        /// <typeparam name="T">The base type for the configuration element.</typeparam>
        /// <param name="contentBuilder">The <see cref="AdmContentBuilder"/> to which the Adm instructions are to be appended.</param>
        /// <param name="element">The configuration element.</param>
        /// <param name="subProvider">The <see cref="ConfigurationElementManageabilityProvider"/> used to append the ADM instructions
        /// for the element.</param>
        /// <param name="configurationSource">The configuration source from where to get additional configuration
        /// information, if necessary.</param>
        /// <param name="parentKey">The key path for which the generated instructions' keys must be sub keys of.</param>
        protected static void AddAdministrativeTemplateDirectivesForElement<T>(AdmContentBuilder contentBuilder,
                                                                               T element,
                                                                               ConfigurationElementManageabilityProvider subProvider,
                                                                               IConfigurationSource configurationSource,
                                                                               String parentKey)
            where T : NamedConfigurationElement, new()
        {
            subProvider.AddAdministrativeTemplateDirectives(contentBuilder,
                                                            element,
                                                            configurationSource,
                                                            parentKey);
        }

        /// <summary>
        /// Adds the ADM instructions that describe the policies that can be used to override the configuration
        /// information for the elements in a collection of configuration elements, using the registered element 
        /// manageability providers for each element.
        /// </summary>
        /// <remarks>
        /// A new category and one policy for each element in the collection are generated; the element manageability
        /// providers are responsible for generating the policies.
        /// Elements for which no manageability provider is registered are ignored.
        /// </remarks>
        /// <typeparam name="T">The base type for the configuration elements collection.</typeparam>
        /// <param name="contentBuilder">The <see cref="AdmContentBuilder"/> to which the Adm instructions are to be appended.</param>
        /// <param name="elements">The collection of configuration elements.</param>
        /// <param name="configurationSource">The configuration source from where to get additional configuration
        /// information, if necessary.</param>
        /// <param name="parentKey">The key path for which the generated instructions' keys must be sub keys of.</param>
        /// <param name="categoryName">The name for the category where the generated policies will be created.</param>
        /// <devdoc>
        /// FxCop message CA1004 is supressed because it seems like the rule does not detect the
        /// existing 'elements' method parameter that uses the generic parameter T.
        /// </devdoc>
        [SuppressMessage("Microsoft.Design", "CA1004")]
        protected void AddElementsPolicies<T>(AdmContentBuilder contentBuilder,
                                              NamedElementCollection<T> elements,
                                              IConfigurationSource configurationSource,
                                              String parentKey,
                                              String categoryName)
            where T : NamedConfigurationElement, new()
        {
            contentBuilder.StartCategory(categoryName);

            foreach (T element in elements)
            {
                ConfigurationElementManageabilityProvider subProvider = GetSubProvider(element.GetType());

                if (subProvider != null)
                {
                    AddAdministrativeTemplateDirectivesForElement<T>(contentBuilder,
                                                                     element, subProvider,
                                                                     configurationSource,
                                                                     parentKey);
                }
            }

            contentBuilder.EndCategory();
        }

        /// <summary>
        /// Returns the registry key that represents a policy.
        /// </summary>
        /// <param name="machineKey">The key for the policy on the machine tree.</param>
        /// <param name="userKey">The key for the policy on the user tree.</param>
        /// <returns>The <paramref name="machineKey"/> if it is not <see langword="null"/> and
        /// it represents a policy; otherwise the <paramref name="machineKey"/> if it is not 
        /// <see langword="null"/> and it represents a policy, otherwise <see langword="null"/>.</returns>
        /// <seealso cref="IRegistryKey.IsPolicyKey">IRegistryKey.IsPolicyKey</seealso>
        protected static IRegistryKey GetPolicyKey(IRegistryKey machineKey,
                                                   IRegistryKey userKey)
        {
            if (machineKey != null && machineKey.IsPolicyKey)
                return machineKey;

            if (userKey != null && userKey.IsPolicyKey)
                return userKey;

            return null;
        }

        /// <summary>
        /// Returns the <see cref="ConfigurationElementManageabilityProvider"/> instance registered 
        /// for type <paramref name="configurationObjectType"/>.
        /// </summary>
        /// <param name="configurationObjectType">The configuration element type of the instance needing management.</param>
        /// <returns>The manageability provider registered to manage the type, or <see langword="null"/> 
        /// if no provider is registered for the type.</returns>
        protected ConfigurationElementManageabilityProvider GetSubProvider(Type configurationObjectType)
        {
            if (providers.ContainsKey(configurationObjectType))
            {
                return providers[configurationObjectType];
            }

            return null;
        }

        /// <summary>
        /// Utility method that loads sub keys at the machine and user level.
        /// </summary>
        /// <param name="subKeyName">The name of the required sub key.</param>
        /// <param name="machineKey">The parent key at the machine level, or <see langword="null"/> 
        /// if there is no registry key.</param>
        /// <param name="userKey">The parent key at the user level, or <see langword="null"/> 
        /// if there is no registry key.</param>
        /// <param name="machineSubKey">When this method returns, contains a reference to the sub key of
        /// <paramref name="machineKey"/> named <paramref name="subKeyName"/>, or <see langword="null"/> 
        /// if either machineKey is <see langword="null"/> or it does not have a sub key with
        /// the requested name.</param>
        /// <param name="userSubKey">When this method returns, contains a reference to the sub key of
        /// <paramref name="userKey"/> named <paramref name="subKeyName"/>, or <see langword="null"/> 
        /// if either userKey is <see langword="null"/> or it does not have a sub key with
        /// the requested name.</param>
        protected static void LoadRegistrySubKeys(String subKeyName,
                                                  IRegistryKey machineKey,
                                                  IRegistryKey userKey,
                                                  out IRegistryKey machineSubKey,
                                                  out IRegistryKey userSubKey)
        {
            machineSubKey = machineKey != null ? machineKey.OpenSubKey(subKeyName) : null;
            userSubKey = userKey != null ? userKey.OpenSubKey(subKeyName) : null;
        }

        /// <summary>
        /// Logs an error detected while overriding a configuration object with policy values.
        /// </summary>
        /// <param name="exception">The exception representing the error.</param>
        protected virtual void LogExceptionWhileOverriding(Exception exception)
        {
            ManageabilityExtensionsLogger.LogExceptionWhileOverriding(exception);
        }

        /// <summary>
        /// Overrides the <paramref name="configurationObject"/>'s and its internal configuration elements' properties 
        /// with the Group Policy values from the registry, if any, and creates the <see cref="ConfigurationSetting"/> 
        /// instances that describe the configuration.
        /// </summary>
        /// <param name="configurationObject">The configuration section that must be managed.</param>
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
        /// <returns><see langword="true"/> if the policy settings do not disable the configuration section, otherwise
        /// <see langword="false"/>.</returns>
        /// <exception cref="ArgumentException">when the type of <paramref name="configurationObject"/> is not 
        /// the type <typeparamref name="T"/>.</exception>
        public abstract bool OverrideWithGroupPoliciesAndGenerateWmiObjects(
            ConfigurationSection configurationObject,
            bool readGroupPolicies,
            IRegistryKey machineKey,
            IRegistryKey userKey,
            bool generateWmiObjects,
            ICollection<ConfigurationSetting> wmiSettings);

        /// <summary>
        /// Overrides the properties for the configuration element and creates the 
        /// <see cref="ConfigurationSetting"/> instances that describe it.
        /// </summary>
        /// <typeparam name="T">The base type for the configuration elements collection.</typeparam>
        /// <param name="element">The configuration element.</param>
        /// <param name="subProvider">The <see cref="ConfigurationElementManageabilityProvider"/> used to override the element's
        /// properties and create the wmi objects.</param>
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
        /// <returns><see langword="true"/> if the policy settings do not disable the configuration element, otherwise
        /// <see langword="false"/>.</returns>
        /// <remarks>
        /// This method assumes a specific layout for the policy values: there is a registry key representing the collection
        /// of elements, and a sub key with the policy values for each element. An element's sub key may also contains a value
        /// stating whether the policy for an element is disabled; in that case the element is removed from the collection.
        /// Such a layout for the policy values can be constructed manually, or method 
        /// <see cref="ConfigurationSectionManageabilityProvider.AddElementsPolicies{T}"/> can be invoked during the construction
        /// of the ADM template to generate it.
        /// </remarks>
        protected static bool OverrideWithGroupPoliciesAndGenerateWmiObjectsForElement<T>(T element,
                                                                                          ConfigurationElementManageabilityProvider subProvider,
                                                                                          bool readGroupPolicies,
                                                                                          IRegistryKey machineKey,
                                                                                          IRegistryKey userKey,
                                                                                          bool generateWmiObjects,
                                                                                          ICollection<ConfigurationSetting> wmiSettings)
            where T : NamedConfigurationElement, new()
        {
            return subProvider.OverrideWithGroupPoliciesAndGenerateWmiObjects(element,
                                                                              readGroupPolicies, machineKey, userKey,
                                                                              generateWmiObjects, wmiSettings);
        }

        /// <summary>
        /// Overrides the properties for the configuration elements in the given collection, and creates the 
        /// <see cref="ConfigurationSetting"/> instances that describe each element.
        /// </summary>
        /// <typeparam name="T">The base type for the configuration elements collection.</typeparam>
        /// <param name="elements">The collection of configuration elements.</param>
        /// <param name="keyName">The name of the sub key where the policy values for the elements in the collection
        /// reside.</param>
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
        /// <remarks>
        /// This method assumes a specific layout for the policy values: there is a registry key representing the collection
        /// of elements, and a sub key with the policy values for each element. An element's sub key may also contains a value
        /// stating whether the policy for an element is disabled; in that case the element is removed from the collection.
        /// Such a layout for the policy values can be constructed manually, or method 
        /// <see cref="ConfigurationSectionManageabilityProvider.AddElementsPolicies{T}"/> can be invoked during the construction
        /// of the ADM template to generate it.
        /// </remarks>
        /// <devdoc>
        /// FxCop message CA1004 is supressed because it seems like the rule does not detect the
        /// existing 'elements' method parameter that uses the generic parameter T.
        /// </devdoc>
        [SuppressMessage("Microsoft.Design", "CA1004")]
        protected void OverrideWithGroupPoliciesAndGenerateWmiObjectsForElementCollection<T>(NamedElementCollection<T> elements,
                                                                                             String keyName,
                                                                                             bool readGroupPolicies,
                                                                                             IRegistryKey machineKey,
                                                                                             IRegistryKey userKey,
                                                                                             bool generateWmiObjects,
                                                                                             ICollection<ConfigurationSetting> wmiSettings)
            where T : NamedConfigurationElement, new()
        {
            List<T> elementsToRemove = new List<T>();

            IRegistryKey machineElementsKey = null;
            IRegistryKey userElementsKey = null;

            try
            {
                LoadRegistrySubKeys(keyName,
                                    machineKey, userKey,
                                    out machineElementsKey, out userElementsKey);

                foreach (T element in elements)
                {
                    IRegistryKey machineElementKey = null;
                    IRegistryKey userElementKey = null;

                    try
                    {
                        LoadRegistrySubKeys(element.Name,
                                            machineElementsKey, userElementsKey,
                                            out machineElementKey, out userElementKey);

                        ConfigurationElementManageabilityProvider subProvider = GetSubProvider(element.GetType());

                        if (subProvider != null && !OverrideWithGroupPoliciesAndGenerateWmiObjectsForElement<T>(element,
                                                                                                                subProvider,
                                                                                                                readGroupPolicies, machineElementKey, userElementKey,
                                                                                                                generateWmiObjects, wmiSettings))
                        {
                            elementsToRemove.Add(element);
                        }
                    }
                    finally
                    {
                        ReleaseRegistryKeys(machineElementKey, userElementKey);
                    }
                }
            }
            finally
            {
                ReleaseRegistryKeys(machineElementsKey, userElementsKey);
            }

            // remove disabled elements
            foreach (T element in elementsToRemove)
            {
                elements.Remove(element.Name);
            }
        }

        /// <summary>
        /// Utility method that closes registry keys.
        /// </summary>
        /// <param name="keys">The registry keys to close.</param>
        protected static void ReleaseRegistryKeys(params IRegistryKey[] keys)
        {
            foreach (IRegistryKey key in keys)
            {
                if (key != null)
                {
                    try
                    {
                        key.Close();
                    }
                    catch (Exception) {}
                }
            }
        }
    }
}
