//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability
{
	/// <summary>
	/// <para>This type supports the Enterprise Library Manageability Extensions infrastructure and is not intended to 
	/// be used directly from your code.</para>
	/// Represents the behavior required to provide Group Policy updates and to publish the <see cref="ConfigurationSetting"/> 
	/// instances associated to the configuration information for the Exception Handling Application Block, and it also manages
	/// the creation of the ADM template categories and policies required to edit Group Policy Objects for the block.
	/// </summary>
	/// <remarks>
	/// This class performs the actual Group Policy update and Wmi object generation for the <see cref="ExceptionHandlingSettings"/>
	/// configuration section and its <see cref="ExceptionPolicyData"/> instances together with the <see cref="ExceptionTypeData"/>
	/// objects they contain. Processing for <see cref="ExceptionHandlerData"/> instances is delegated to 
	/// <see cref="ConfigurationElementManageabilityProvider"/> objects registered to the handler data types.
	/// </remarks>
	public sealed class ExceptionHandlingSettingsManageabilityProvider
		: ConfigurationSectionManageabilityProviderBase<ExceptionHandlingSettings>
	{
        /// <summary>
        /// The name of the policies property.
        /// </summary>
		public const String PoliciesKeyName = "exceptionPolicies";

        /// <summary>
        /// The name of the policy types property.
        /// </summary>
		public const String PolicyTypesPropertyName = "exceptionTypes";
        
        /// <summary>
        /// The name of the policy post handling action property.
        /// </summary>
		public const String PolicyTypePostHandlingActionPropertyName = "postHandlingAction";

        /// <summary>
        /// The name of the exception handlers property.
        /// </summary>
		public const String PolicyTypeHandlersPropertyName = "exceptionHandlers";

		/// <summary>
		/// <para>This method supports the Enterprise Library Manageability Extensions infrastructure and is not intended to 
		/// be used directly from your code.</para>
		/// Initializes a new instance of the <see cref="ExceptionHandlingSettingsManageabilityProvider"/> class with a 
		/// given set of manageability providers to use when dealing with the configuration for exception handlers.
		/// </summary>
		/// <param name="subProviders">The mapping from configuration element type to
		/// <see cref="ConfigurationElementManageabilityProvider"/>.</param>
		public ExceptionHandlingSettingsManageabilityProvider(IDictionary<Type, ConfigurationElementManageabilityProvider> subProviders)
			: base(subProviders)
		{
			ExceptionHandlingSettingsWmiMapper.RegisterWmiTypes();
		}

		/// <summary>
		/// <para>This method supports the Enterprise Library Manageability Extensions infrastructure and is not intended to 
		/// be used directly from your code.</para>
		/// Adds the ADM instructions that describe the policies that can be used to override the configuration
		/// information for the Exception Handling Application Block.
		/// </summary>
		/// <seealso cref="ConfigurationSectionManageabilityProvider.AddAdministrativeTemplateDirectives(AdmContentBuilder, ConfigurationSection, IConfigurationSource, String)"/>
		protected override void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder,
			ExceptionHandlingSettings configurationSection,
			IConfigurationSource configurationSource,
			String sectionKey)
		{
			contentBuilder.StartPolicy(Resources.SectionPolicyName,
				sectionKey);
			contentBuilder.EndPolicy();

			foreach (ExceptionPolicyData policy in configurationSection.ExceptionPolicies)
			{
				contentBuilder.StartCategory(policy.Name);
				{
					String exceptionTypesKey = sectionKey
						+ @"\" + PoliciesKeyName
						+ @"\" + policy.Name
						+ @"\" + PolicyTypesPropertyName;

					foreach (ExceptionTypeData exceptionType in policy.ExceptionTypes)
					{
						String exceptionTypeKey = exceptionTypesKey + @"\" + exceptionType.Name;

						contentBuilder.StartPolicy(String.Format(CultureInfo.CurrentCulture,
																Resources.ExceptionTypePolicyNameTemplate,
																exceptionType.Name),
							 exceptionTypeKey);
						{
							contentBuilder.AddDropDownListPartForEnumeration<PostHandlingAction>(Resources.ExceptionTypePostHandlingActionPartName,
								PolicyTypePostHandlingActionPropertyName,
								exceptionType.PostHandlingAction);

							contentBuilder.AddTextPart(Resources.ExceptionTypeHandlersPartName);

							String exceptionHandlersKey = exceptionTypeKey + @"\" + PolicyTypeHandlersPropertyName;
							foreach (ExceptionHandlerData handler in exceptionType.ExceptionHandlers)
							{
								ConfigurationElementManageabilityProvider subProvider = GetSubProvider(handler.GetType());

								if (subProvider != null)
								{
									AddAdministrativeTemplateDirectivesForElement<ExceptionHandlerData>(contentBuilder,
										handler, subProvider,
										configurationSource,
										exceptionHandlersKey);
								}
							}
						}
						contentBuilder.EndPolicy();
					}
				}
				contentBuilder.EndCategory();
			}
		}


		/// <summary>
		/// Gets the name of the category that represents the whole configuration section.
		/// </summary>
		protected override string SectionCategoryName
		{
			get { return Resources.SectionCategoryName; }
		}

		/// <summary>
		/// Gets the name of the managed configuration section.
		/// </summary>
		protected override string SectionName
		{
			get { return ExceptionHandlingSettings.SectionName; }
		}

		/// <summary>
		/// Overrides the <paramref name="configurationSection"/>'s properties with the Group Policy values from 
		/// the registry.
		/// </summary>
		/// <param name="configurationSection">The configuration section that must be managed.</param>
		/// <param name="policyKey">The <see cref="IRegistryKey"/> which holds the Group Policy overrides.</param>
		protected override void OverrideWithGroupPoliciesForConfigurationSection(ExceptionHandlingSettings configurationSection,
			IRegistryKey policyKey)
		{
			// no section values to override
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
		protected override void OverrideWithGroupPoliciesAndGenerateWmiObjectsForConfigurationElements(ExceptionHandlingSettings configurationSection,
			bool readGroupPolicies, IRegistryKey machineKey, IRegistryKey userKey,
			bool generateWmiObjects, ICollection<ConfigurationSetting> wmiSettings)
		{
			IRegistryKey machinePoliciesKey = null;
			IRegistryKey userPoliciesKey = null;

			try
			{
				LoadRegistrySubKeys(PoliciesKeyName,
					machineKey, userKey,
					out machinePoliciesKey, out userPoliciesKey);

				foreach (ExceptionPolicyData policy in configurationSection.ExceptionPolicies)
				{
					IRegistryKey machinePolicyKey = null;
					IRegistryKey userPolicyKey = null;

					try
					{
						LoadRegistrySubKeys(policy.Name,
							machinePoliciesKey, userPoliciesKey,
							out machinePolicyKey, out userPolicyKey);

						OverrideWithGroupPoliciesAndGenerateWmiObjectsForPolicy(policy,
							readGroupPolicies, machinePolicyKey, userPolicyKey,
							generateWmiObjects, wmiSettings);

						if (generateWmiObjects)
						{
							ExceptionHandlingSettingsWmiMapper.GenerateExceptionPolicyDataWmiObjects(policy, wmiSettings);
						}
					}
					finally
					{
						ReleaseRegistryKeys(machinePolicyKey, userPolicyKey);
					}
				}
			}
			finally
			{
				ReleaseRegistryKeys(machinePoliciesKey, userPoliciesKey);
			}
		}

		private void OverrideWithGroupPoliciesAndGenerateWmiObjectsForPolicy(ExceptionPolicyData policy,
			bool readGroupPolicies, IRegistryKey machineKey, IRegistryKey userKey,
			bool generateWmiObjects, ICollection<ConfigurationSetting> wmiSettings)
		{
			List<ExceptionTypeData> typesToRemove = new List<ExceptionTypeData>();

			IRegistryKey machinePolicyTypesKey = null;
			IRegistryKey userPolicyTypesKey = null;

			try
			{
				LoadRegistrySubKeys(PolicyTypesPropertyName,
					machineKey, userKey,
					out machinePolicyTypesKey, out userPolicyTypesKey);

				foreach (ExceptionTypeData exceptionType in policy.ExceptionTypes)
				{
					IRegistryKey machinePolicyTypeKey = null;
					IRegistryKey userPolicyTypeKey = null;

					try
					{
						LoadRegistrySubKeys(exceptionType.Name,
							machinePolicyTypesKey, userPolicyTypesKey,
							out machinePolicyTypeKey, out userPolicyTypeKey);

						if (!OverrideWithGroupPolicyTypeTypesAndGenerateWmiObjectsForPolicyType(exceptionType,
								policy,
								readGroupPolicies, machinePolicyTypeKey, userPolicyTypeKey,
								generateWmiObjects, wmiSettings))
						{
							typesToRemove.Add(exceptionType);
						}
					}
					finally
					{
						ReleaseRegistryKeys(machinePolicyTypeKey, userPolicyTypeKey);
					}
				}
			}
			finally
			{
				ReleaseRegistryKeys(machinePolicyTypesKey, userPolicyTypesKey);
			}

			foreach (ExceptionTypeData exceptionType in typesToRemove)
			{
				policy.ExceptionTypes.Remove(exceptionType.Name);
			}
		}

		private bool OverrideWithGroupPolicyTypeTypesAndGenerateWmiObjectsForPolicyType(ExceptionTypeData exceptionType,
			ExceptionPolicyData parentPolicy,
			bool readGroupPolicies, IRegistryKey machineKey, IRegistryKey userKey,
			bool generateWmiObjects, ICollection<ConfigurationSetting> wmiSettings)
		{
			if (readGroupPolicies)
			{
				IRegistryKey policyKey = machineKey != null ? machineKey : userKey;
				if (policyKey != null)
				{
					if (policyKey.IsPolicyKey && !policyKey.GetBoolValue(PolicyValueName).Value)
					{
						return false;
					}
					try
					{
						PostHandlingAction? postHandlingActionOverride
							= policyKey.GetEnumValue<PostHandlingAction>(PolicyTypePostHandlingActionPropertyName);

						exceptionType.PostHandlingAction = postHandlingActionOverride.Value;
					}
					catch (RegistryAccessException ex)
					{
						LogExceptionWhileOverriding(ex);
					}
				}
			}
			if (generateWmiObjects)
			{
				ExceptionHandlingSettingsWmiMapper.GenerateExceptionTypeWmiObjects(exceptionType, parentPolicy, wmiSettings);
			}

			// Note: store the handler settings to a temporary location to enable 
			// post processing. This forces the creation of a specific interface and
			// fixes the schema.
			List<ConfigurationSetting> handlerSettings = new List<ConfigurationSetting>(exceptionType.ExceptionHandlers.Count);
			OverrideWithGroupPoliciesAndGenerateWmiObjectsForElementCollection(exceptionType.ExceptionHandlers,
				PolicyTypeHandlersPropertyName,
				readGroupPolicies, machineKey, userKey,
				generateWmiObjects, handlerSettings);

			int i = 0;
			foreach (ExceptionHandlerSetting setting in handlerSettings)
			{
				setting.Policy = parentPolicy.Name;
				setting.ExceptionType = exceptionType.Name;
				setting.Order = i++;

				// add the post-processed handler settings to the main settings collection
				wmiSettings.Add(setting);
			}

			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="configurationSection"></param>
		/// <param name="wmiSettings"></param>
		protected override void GenerateWmiObjectsForConfigurationSection(ExceptionHandlingSettings configurationSection,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			ExceptionHandlingSettingsWmiMapper.GenerateWmiObjects(configurationSection, wmiSettings);
		}
	}
}
