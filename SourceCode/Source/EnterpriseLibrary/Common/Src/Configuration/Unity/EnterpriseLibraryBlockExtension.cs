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
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
	/// <summary>
	/// Base class for Enterprise Library Blocks' container extensions.
	/// </summary>
	public abstract class EnterpriseLibraryBlockExtension : UnityContainerExtension
	{
		/// <summary>
		/// Creates the policies to create the providers specified by a <see cref="PolymorphicConfigurationElementCollection{T}"/>
		/// using the appropriate <see cref="IContainerPolicyCreator"/> classes specified with the <see cref="ContainerPolicyCreatorAttribute"/>
		/// class.
		/// </summary>
		/// <typeparam name="TProviderBase">The root of the provider hierarchy.</typeparam>
		/// <typeparam name="TProviderConfigurationBase">The root of the configuration object hierarchy.</typeparam>
		/// <param name="policyList">The destination of the new policies.</param>
		/// <param name="defaultProviderName">The name of the default provider, if any.</param>
		/// <param name="configurationObjects">The collection of configuration objects.</param>
		/// <param name="configurationSource">The <see cref="IConfigurationSource"/> from where additional configuration information
		/// can be retrieved.</param>
		/// <exception cref="ConfigurationErrorsException">when the specified default name does not correspond to an element
		/// in the collection of provider configuration objects.</exception>
		/// <exception cref="ArgumentException">when the type of a configuration object in <paramref name="configurationObjects"/>
		/// does not have the required <see cref="ContainerPolicyCreatorAttribute"/> specifying an <see cref="IContainerPolicyCreator"/>
		/// class.</exception>
		/// <remarks>
		/// Two policies are usually created for each element in <paramref name="configurationObjects"/>: a key mapping policy 
		/// to map to the concrete provider type, and a constructor selector policy with properly configured parameters
		/// to create the provider.
		/// If a valid provider name is supplied for <paramref name="defaultProviderName"/>, an additional key mapping policy
		/// is created to map the null name to the default provider name.
		/// </remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "The purpose of this method is to create policies to describe instances of TProviderBase, "
				+ "so it is not possible to supply such a parameter")]
		protected void CreateProvidersPolicies<TProviderBase, TProviderConfigurationBase>(
			IPolicyList policyList,
			string defaultProviderName,
			PolymorphicConfigurationElementCollection<TProviderConfigurationBase> configurationObjects,
			IConfigurationSource configurationSource)
			where TProviderBase : class
			where TProviderConfigurationBase : NameTypeConfigurationElement, new()
		{
			CreateProvidersPolicies<TProviderBase, TProviderConfigurationBase>(
				policyList,
				defaultProviderName,
				configurationObjects,
				configurationSource,
				c => c.Name);
		}

		/// <summary>
		/// Creates the policies to create the providers specified by a <see cref="PolymorphicConfigurationElementCollection{T}"/>
		/// using the appropriate <see cref="IContainerPolicyCreator"/> classes specified with the <see cref="ContainerPolicyCreatorAttribute"/>
		/// class.
		/// </summary>
		/// <typeparam name="TProviderBase">The root of the provider hierarchy.</typeparam>
		/// <typeparam name="TProviderConfigurationBase">The root of the configuration object hierarchy.</typeparam>
		/// <param name="policyList">The destination of the new policies.</param>
		/// <param name="defaultProviderName">The name of the default provider, if any.</param>
		/// <param name="configurationObjects">The collection of configuration objects.</param>
		/// <param name="configurationSource">The <see cref="IConfigurationSource"/> from where additional configuration information
		/// can be retrieved.</param>
		/// <param name="getName">A delegate to determine the name to use for policies.</param>
		/// <exception cref="ConfigurationErrorsException">when the specified default name does not correspond to an element
		/// in the collection of provider configuration objects.</exception>
		/// <exception cref="ArgumentException">when the type of a configuration object in <paramref name="configurationObjects"/>
		/// does not have the required <see cref="ContainerPolicyCreatorAttribute"/> specifying an <see cref="IContainerPolicyCreator"/>
		/// class.</exception>
		/// <remarks>
		/// Two policies are usually created for each element in <paramref name="configurationObjects"/>: a key mapping policy 
		/// to map to the concrete provider type, and a constructor selector policy with properly configured parameters
		/// to create the provider.
		/// If a valid provider name is supplied for <paramref name="defaultProviderName"/>, an additional key mapping policy
		/// is created to map the null name to the default provider name.
		/// </remarks>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "The purpose of this method is to create policies to describe instances of TProviderBase, "
				+ "so it is not possible to supply such a parameter")]
		protected void CreateProvidersPolicies<TProviderBase, TProviderConfigurationBase>(
			IPolicyList policyList,
			string defaultProviderName,
			PolymorphicConfigurationElementCollection<TProviderConfigurationBase> configurationObjects,
			IConfigurationSource configurationSource,
			Func<TProviderConfigurationBase, string> getName)
			where TProviderBase : class
			where TProviderConfigurationBase : NameTypeConfigurationElement, new()
		{
			// set the key mapping for the default, if any
			if (!string.IsNullOrEmpty(defaultProviderName))
			{
				TProviderConfigurationBase defaultData = configurationObjects.Get(defaultProviderName);
				if (defaultData == null)
				{
					throw new ConfigurationErrorsException(
						string.Format(
							CultureInfo.CurrentCulture,
							Resources.ExceptionTheSpecifiedDefaultProviderDoesNotExistInConfiguration,
							defaultProviderName,
							typeof(TProviderBase).FullName));
				}

				policyList.Set<IBuildKeyMappingPolicy>(
					new BuildKeyMappingPolicy(new NamedTypeBuildKey(defaultData.Type, getName(defaultData))),
					new NamedTypeBuildKey(typeof(TProviderBase)));
			}

			// set the policies for the providers
			foreach (TProviderConfigurationBase data in configurationObjects)
			{
				string instanceName = getName(data);

				// key mapping from the provider abstraction to the implementation
				policyList.Set<IBuildKeyMappingPolicy>(
					new BuildKeyMappingPolicy(new NamedTypeBuildKey(data.Type, instanceName)),
					new NamedTypeBuildKey(typeof(TProviderBase), instanceName));

				IContainerPolicyCreator policyCreator = GetContainerPolicyCreator(data.GetType(), data.Type);
				policyCreator.CreatePolicies(policyList, instanceName, data, configurationSource);
			}
		}

		/// <summary>
		/// Returns the <see cref="IContainerPolicyCreator"/> associated to <paramref name="type"/> with the
		/// <see cref="ContainerPolicyCreatorAttribute"/>.
		/// </summary>
		/// <param name="sourceType">The <see cref="Type"/> from which the policy creator must be created. Usually a configuration object type.</param>
		/// <param name="targetType">The <see cref="Type"/> for which the policies will be created.</param>
		/// <returns>An instance of <see cref="IContainerPolicyCreator"/>.</returns>
		/// <exception cref="ArgumentException">when there is no <see cref="ContainerPolicyCreatorAttribute"/> 
		/// on <paramref name="type"/>.</exception>
		protected virtual IContainerPolicyCreator GetContainerPolicyCreator(Type sourceType, Type targetType)
		{
			Guard.ArgumentNotNull(sourceType, "sourceType");

			ContainerPolicyCreatorAttribute policyCreatorAttribute
				= (ContainerPolicyCreatorAttribute)Attribute.GetCustomAttribute(
					sourceType,
					typeof(ContainerPolicyCreatorAttribute),
					false);

			if (policyCreatorAttribute != null)
			{
				// ContainerPolicyCreatorAttribute requires the policy creator type to have a zero-args .ctor
				return (IContainerPolicyCreator)Activator.CreateInstance(policyCreatorAttribute.PolicyCreatorType);
			}
			if (targetType != null)
			{
				return GetDefaultContainerPolicyCreator(targetType);
			}

			throw new ArgumentException(
				string.Format(
					CultureInfo.CurrentCulture,
					Resources.ExceptionContainerPolicyCreatorAttributeNotPresent,
					sourceType.AssemblyQualifiedName));
		}

		/// <summary>
		/// Returns a default <see cref="IContainerPolicyCreator"/> implementation for <paramref name="targetType"/>.
		/// </summary>
		/// <param name="targetType">The type for which policies must be built.</param>
		/// <returns>An instance of <see cref="ConstructorArgumentMatchingPolicyCreator"/>.</returns>
		protected virtual IContainerPolicyCreator GetDefaultContainerPolicyCreator(Type targetType)
		{
			return new ConstructorArgumentMatchingPolicyCreator(targetType);
		}

		/// <summary>
		/// Gets the <see cref="IConfigurationSource"/> for the extension.
		/// </summary>
		protected IConfigurationSource ConfigurationSource
		{
			get
			{
				IConfigurationObjectPolicy policy
					= Context.Policies.Get<IConfigurationObjectPolicy>(typeof(IConfigurationSource));
				if (policy == null)
				{
					throw new InvalidOperationException(Resources.ExceptionNoConfigurationObjectPolicySet);
				}

				return policy.ConfigurationSource;
			}
		}
	}
}
