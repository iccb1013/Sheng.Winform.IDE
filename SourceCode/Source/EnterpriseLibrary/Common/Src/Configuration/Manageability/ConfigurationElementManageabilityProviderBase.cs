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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	/// <summary>
	/// Provides a default implementation for <see cref="ConfigurationElementManageabilityProvider"/> that
	/// splits policy overrides processing and WMI objects generation, performing approriate logging of 
	/// policy processing errors.
	/// </summary>
	/// <typeparam name="T">The managed configuration element type. Must inherit from <see cref="NamedConfigurationElement"/>.
	/// </typeparam>
	public abstract class ConfigurationElementManageabilityProviderBase<T>
		: ConfigurationElementManageabilityProvider
		where T : NamedConfigurationElement
	{
		/// <summary>
		/// 
		/// </summary>
		/// <remarks>Default to ReadOnly</remarks>
		protected ConfigurationElementManageabilityProviderBase()
		{ }

		/// <summary>
		/// Adds the ADM instructions that describe the policies that can be used to override the properties of
		/// a specific instance of the configuration element type managed by the receiver.
		/// </summary>
		/// <param name="contentBuilder">The <see cref="AdmContentBuilder"/> to which the Adm instructions are to be appended.</param>
		/// <param name="configurationObject">The configuration object instance.</param>
		/// <param name="configurationSource">The configuration source from where to get additional configuration
		/// information, if necessary.</param>
		/// <param name="parentKey">The key path for which the generated instructions' keys must be subKeys of.</param>
		/// <remarks>
		/// Class <see cref="ConfigurationElementManageabilityProviderBase{T}"/> provides a default implementation for this method that
		/// calls the strongly typed 
		/// <see cref="ConfigurationElementManageabilityProviderBase{T}.AddAdministrativeTemplateDirectives(AdmContentBuilder, T, IConfigurationSource, String)"/>
		/// method.
		/// </remarks>
		public sealed override void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder,
			ConfigurationElement configurationObject,
			IConfigurationSource configurationSource,
			string parentKey)
		{
			T data = (T)configurationObject;
			String elementPolicyKeyName = parentKey + @"\" + data.Name;

			AddAdministrativeTemplateDirectives(contentBuilder, data, configurationSource, elementPolicyKeyName);
		}

		/// <summary>
		/// Adds the ADM instructions that describe the policies that can be used to override the properties of
		/// a specific instance of the configuration element type managed by the receiver.
		/// </summary>
		/// <param name="contentBuilder">The <see cref="AdmContentBuilder"/> to which the Adm instructions are to be appended.</param>
		/// <param name="configurationObject">The configuration object instance.</param>
		/// <param name="configurationSource">The configuration source from where to get additional configuration
		/// information, if necessary.</param>
		/// <param name="elementPolicyKeyName">The key for the element's policies.</param>
		/// <remarks>
		/// The default implementation for this method creates a policy, using 
		/// <see cref="ConfigurationElementManageabilityProviderBase{T}.ElementPolicyNameTemplate"/> to create the policy name and invoking
		/// <see cref="ConfigurationElementManageabilityProviderBase{T}.AddElementAdministrativeTemplateParts(AdmContentBuilder, T, IConfigurationSource, String)"/>
		/// to add the policy parts.
		/// Subclasses managing objects that must not create a policy must override this method to just add the parts.
		/// </remarks>
		protected virtual void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder,
			T configurationObject,
			IConfigurationSource configurationSource,
			String elementPolicyKeyName)
		{
			contentBuilder.StartPolicy(String.Format(CultureInfo.InvariantCulture,
										ElementPolicyNameTemplate,
										configurationObject.Name),
				elementPolicyKeyName);
			{
				AddElementAdministrativeTemplateParts(contentBuilder,
					configurationObject,
					configurationSource,
					elementPolicyKeyName);
			}
			contentBuilder.EndPolicy();
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
		protected abstract void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder,
			T configurationObject,
			IConfigurationSource configurationSource,
			String elementPolicyKeyName);

		/// <summary>
		/// Gets the template for the name of the policy associated to the object.
		/// </summary>
		/// <remarks>
		/// Elements that override 
		/// <see cref="ConfigurationElementManageabilityProviderBase{T}.AddAdministrativeTemplateDirectives(AdmContentBuilder, T, IConfigurationSource, String)"/>
		/// to avoid creating a policy must still override this property.
		/// </remarks>
		protected abstract String ElementPolicyNameTemplate
		{
			get;
		}

		/// <summary>
		/// Overrides the <paramref name="configurationObject"/>'s properties with the Group Policy values from the 
		/// registry, if any, and creates the <see cref="ConfigurationSetting"/> instances that describe the 
		/// configurationObject.
		/// </summary>
		/// <param name="configurationObject">The configuration object for instances that must be managed.</param>
		/// <param name="readGroupPolicies"><see langword="true"/> if Group Policy overrides must be applied; otherwise, 
		/// <see langword="false"/>.</param>
		/// <param name="machineKey">The <see cref="IRegistryKey"/> which holds the Group Policy overrides for the 
		/// configuration element at the machine level, or <see langword="null"/> 
		/// if there is no such registry key.</param>
		/// <param name="userKey">The <see cref="IRegistryKey"/> which holds the Group Policy overrides for the 
		/// configuration element at the user level, or <see langword="null"/> 
		/// if there is no such registry key.</param>
		/// <param name="generateWmiObjects"><see langword="true"/> if WMI objects must be generated; otherwise, 
		/// <see langword="false"/>.</param>
		/// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		/// <returns><see langword="true"/> if the policy settings do not disable the configuration element, otherwise
		/// <see langword="false"/>.</returns>
		/// <exception cref="ArgumentException">when the type of <paramref name="configurationObject"/> is not 
		/// the type <typeparamref name="T"/>.</exception>
		/// <remarks>
		/// Provides a default implementation that performs appropriate logging of errors when processing
		/// policy overrides.
		/// </remarks>
		/// <seealso cref="ConfigurationElementManageabilityProvider.OverrideWithGroupPoliciesAndGenerateWmiObjects">ConfigurationElementManageabilityProvider.OverrideWithGroupPoliciesAndGenerateWmiObjects</seealso>
		/// <seealso cref="ConfigurationElementManageabilityProviderBase{T}.OverrideWithGroupPolicies"/>
		/// <seealso cref="ConfigurationElementManageabilityProviderBase{T}.GenerateWmiObjects"/>
		public sealed override bool OverrideWithGroupPoliciesAndGenerateWmiObjects(ConfigurationElement configurationObject,
			bool readGroupPolicies, IRegistryKey machineKey, IRegistryKey userKey,
			bool generateWmiObjects, ICollection<ConfigurationSetting> wmiSettings)
		{
			T data = configurationObject as T;
			if (data == null)
			{
				throw new ArgumentException(
					String.Format(CultureInfo.CurrentUICulture, Resources.ConfigurationElementOfWrongType,
						typeof(T).FullName, configurationObject.GetType().FullName),
					"configurationObject");
			}

			if (readGroupPolicies)
			{
				IRegistryKey policyKey = machineKey != null ? machineKey : userKey;
				if (policyKey != null)
				{
					// the keys for some elements might not be the keys associated to a policy,
					// but hold the policy values for the element when policies for multiple
					// elements are combined.
					if (policyKey.IsPolicyKey && !policyKey.GetBoolValue(PolicyValueName).Value)
					{
						return false;
					}
					try
					{
						OverrideWithGroupPolicies(data, policyKey);
					}
					catch (Exception ex)
					{
						LogExceptionWhileOverriding(ex);
					}
				}
			}
			if (generateWmiObjects)
			{
				GenerateWmiObjects(data, wmiSettings);
			}

			return true;
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
		protected abstract void OverrideWithGroupPolicies(T configurationObject, IRegistryKey policyKey);

		/// <summary>
		/// Creates the <see cref="ConfigurationSetting"/> instances that describe the 
		/// configurationObject.
		/// </summary>
		/// <param name="configurationObject">The configuration object for instances that must be managed.</param>
		/// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		protected abstract void GenerateWmiObjects(T configurationObject, ICollection<ConfigurationSetting> wmiSettings);
	}
}
