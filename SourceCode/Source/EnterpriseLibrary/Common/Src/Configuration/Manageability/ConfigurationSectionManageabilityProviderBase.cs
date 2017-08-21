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
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	/// <summary>
	/// Provides a default base implementation for <see cref="ConfigurationSectionManageabilityProvider"/> that splits
	/// policy override processing and WMI object generation for the section, performing appropriate logging of
	/// policy processing errors, from policy override processing and WMI object generation for configuration objects
	/// contained by the section.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class ConfigurationSectionManageabilityProviderBase<T>
		: ConfigurationSectionManageabilityProvider
		where T : ConfigurationSection
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationSectionManageabilityProviderBase{T}"/> class with a 
		/// given set of manageability providers for the elements in the section's collections.
		/// </summary>
		/// <param name="subProviders">The mapping from configuration element type to
		/// <see cref="ConfigurationElementManageabilityProvider"/>.</param>
		protected ConfigurationSectionManageabilityProviderBase(IDictionary<Type, ConfigurationElementManageabilityProvider> subProviders)
			: base(subProviders)
		{ }

		/// <summary>
		/// Adds the ADM instructions that describe the policies that can be used to override the configuration
		/// information represented by a configuration section.
		/// </summary>
		/// <param name="contentBuilder">The <see cref="AdmContentBuilder"/> to which the Adm instructions are to be appended.</param>
		/// <param name="configurationObject">The configuration section instance.</param>
		/// <param name="configurationSource">The configuration source from where to get additional configuration
		/// information, if necessary.</param>
		/// <param name="applicationName">The key path for which the generated instructions' keys must be sub keys of.</param>
		public sealed override void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder,
			ConfigurationSection configurationObject,
			IConfigurationSource configurationSource,
			String applicationName)
		{
			T configurationSection = (T)configurationObject;
			String sectionKey = applicationName + @"\" + SectionName;

			contentBuilder.StartCategory(SectionCategoryName);
			{
				AddAdministrativeTemplateDirectives(contentBuilder, configurationSection, configurationSource, sectionKey);
			}
			contentBuilder.EndCategory();
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
		protected abstract void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder,
			T configurationSection,
			IConfigurationSource configurationSource,
			String sectionKey);

		/// <summary>
		/// Gets the name of the category that represents the whole configuration section.
		/// </summary>
		protected abstract String SectionCategoryName
		{
			get;
		}

		/// <summary>
		/// Gets the name of the managed configuration section.
		/// </summary>
		protected abstract String SectionName
		{
			get;
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
		public sealed override bool OverrideWithGroupPoliciesAndGenerateWmiObjects(ConfigurationSection configurationObject,
			bool readGroupPolicies, IRegistryKey machineKey, IRegistryKey userKey,
			bool generateWmiObjects, ICollection<ConfigurationSetting> wmiSettings)
		{
			T configurationSection = configurationObject as T;
			if (configurationSection == null)
			{
				throw new ArgumentException(
					String.Format(CultureInfo.CurrentUICulture, Resources.ConfigurationElementOfWrongType, typeof(T).FullName, configurationObject.GetType().FullName),
					"configurationObject");
			}

			if (!OverrideWithGroupPoliciesAndGenerateWmiObjectsForConfigurationSection(configurationSection,
				readGroupPolicies, machineKey, userKey,
				generateWmiObjects, wmiSettings))
			{
				return false;
			}

			OverrideWithGroupPoliciesAndGenerateWmiObjectsForConfigurationElements(configurationSection,
				readGroupPolicies, machineKey, userKey,
				generateWmiObjects, wmiSettings);

			return true;
		}

		private bool OverrideWithGroupPoliciesAndGenerateWmiObjectsForConfigurationSection(T configurationSection,
			bool readGroupPolicies, IRegistryKey machineKey, IRegistryKey userKey,
			bool generateWmiObjects, ICollection<ConfigurationSetting> wmiSettings)
		{
			if (readGroupPolicies)
			{
				IRegistryKey policyKey = GetPolicyKey(machineKey, userKey);
				if (policyKey != null)
				{
					if (!policyKey.GetBoolValue(PolicyValueName).Value)
					{
						return false;
					}
					try
					{
						OverrideWithGroupPoliciesForConfigurationSection(configurationSection, policyKey);
					}
					catch (Exception ex)
					{
						LogExceptionWhileOverriding(ex);
					}
				}
			}
			if (generateWmiObjects)
			{
				GenerateWmiObjectsForConfigurationSection(configurationSection, wmiSettings);
			}

			return true;
		}

		/// <summary>
		/// Overrides the <paramref name="configurationSection"/>'s properties with the Group Policy values from 
		/// the registry.
		/// </summary>
		/// <param name="configurationSection">The configuration section that must be managed.</param>
		/// <param name="policyKey">The <see cref="IRegistryKey"/> which holds the Group Policy overrides.</param>
		/// <remarks>Subclasses implementing this method must retrieve all the override values from the registry
		/// before making modifications to the <paramref name="configurationSection"/> so any error retrieving
		/// the override values will cancel policy processing.</remarks>
		protected abstract void OverrideWithGroupPoliciesForConfigurationSection(T configurationSection, IRegistryKey policyKey);

		/// <summary>
		/// Creates the <see cref="ConfigurationSetting"/> instances that describe the <paramref name="configurationSection"/>.
		/// </summary>
		/// <param name="configurationSection">The configuration section that must be managed.</param>
		/// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		protected abstract void GenerateWmiObjectsForConfigurationSection(T configurationSection, ICollection<ConfigurationSetting> wmiSettings);

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
		/// <remarks>Errors detected while processing policy overrides for the configuration elements in the section 
		/// must be logged but processing for other objects must not be interrupted.</remarks>
		protected abstract void OverrideWithGroupPoliciesAndGenerateWmiObjectsForConfigurationElements(T configurationSection,
			bool readGroupPolicies, IRegistryKey machineKey, IRegistryKey userKey,
			bool generateWmiObjects, ICollection<ConfigurationSetting> wmiSettings);
	}
}
