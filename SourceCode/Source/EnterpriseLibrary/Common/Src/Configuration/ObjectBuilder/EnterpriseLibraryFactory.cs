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
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	/// <summary>
	/// Static facade for the generic building mechanism based on ObjectBuilder.
	/// </summary>
	/// <remarks>
	/// The facade uses a shared stateless <see cref="IBuilder"/> instance configured with the strategies 
	/// that perform the creation of objects in the Enterprise Library.
	/// <para>
	/// The strategies used by the <see cref="EnterpriseLibraryFactory"/> are:
	/// <list type="bullet">
	/// <item><term><see cref="ConfigurationNameMappingStrategy"/> to deal with default instances.</term></item>
	/// <item><term><see cref="ConfiguredObjectStrategy"/> to perform the actual creation of the objects based on the available configuration.</term></item>
	/// <item><term><see cref="InstrumentationStrategy"/> to attach instrumentation to the created objects.</term></item>
	/// </list>
	/// </para>
	/// <para>
	/// The creation request can provide an <see cref="IConfigurationSource"/> to be used by the strategies that need access to 
	/// configuration. If such a configuration source is not provided, a default configuration source will be requested 
	/// to the <see cref="ConfigurationSourceFactory"/>.
	/// In any case, the configuration source is made available to the strategies through a transient <see cref="IConfigurationObjectPolicy"/>.
	/// </para>
	/// <para>
	/// The facade keeps a shared <see cref="ConfigurationReflectionCache"/> that is made available to the strategies through a transient 
	/// <see cref="IReflectionCachePolicy"/>.
	/// </para>
	/// </remarks>
	/// <seealso cref="ConfiguredObjectStrategy"/>
	public static class EnterpriseLibraryFactory
	{
		private static readonly IBuilder builder;
		private static readonly IStrategyChain strategyChain;
		private static readonly ConfigurationReflectionCache reflectionCache = new ConfigurationReflectionCache();

		static EnterpriseLibraryFactory()
		{
			builder = new Builder();
			StagedStrategyChain<BuilderStage> stagedStrategyChain = new StagedStrategyChain<BuilderStage>();
			stagedStrategyChain.AddNew<ConfigurationNameMappingStrategy>(BuilderStage.PreCreation);
			stagedStrategyChain.AddNew<LocatorLookupStrategy>(BuilderStage.PreCreation);
			stagedStrategyChain.AddNew<ConfiguredObjectStrategy>(BuilderStage.PreCreation);
			stagedStrategyChain.AddNew<InstrumentationStrategy>(BuilderStage.PostInitialization);
			strategyChain = stagedStrategyChain.MakeStrategyChain();
		}

		/// <overloads>
		/// Returns an instance of type <typeparamref name="T"/>.
		/// </overloads>
		/// <summary>
		/// Returns a new default instance of type <typeparamref name="T"/> based on configuration information 
		/// from the default configuration source.
		/// </summary>
		/// <typeparam name="T">The type to build.</typeparam>
		/// <returns>A new instance of <typeparamref name="T"/> or any of it subtypes.</returns>
		public static T BuildUp<T>()
		{
			return BuildUp<T>(ConfigurationSource);
		}

		/// <summary>
		/// Returns a default instance of type <typeparamref name="T"/> based on configuration information 
		/// from the default configuration source.
		/// </summary>
		/// <typeparam name="T">The type to build.</typeparam>
		/// <param name="locator">The locator to be used for this build operation.</param>
		/// <param name="lifetimeContainer">The lifetime container to be used for this build operation.</param>
		/// <returns>A new instance of <typeparamref name="T"/> or any of it subtypes, or an existing instance
		/// if type <typeparamref name="T"/> is a singleton that is already present in the <paramref name="locator"/>.
		/// </returns>
		public static T BuildUp<T>(IReadWriteLocator locator, ILifetimeContainer lifetimeContainer)
		{
			return BuildUp<T>(locator, lifetimeContainer, ConfigurationSource);
		}

		/// <summary>
		/// Returns a new default instance of type <typeparamref name="T"/> based on configuration information 
		/// from <paramref name="configurationSource"/>.
		/// </summary>
		/// <typeparam name="T">The type to build.</typeparam>
		/// <param name="configurationSource">The source for configuration information.</param>
		/// <returns>A new instance of <typeparamref name="T"/> or any of it subtypes.</returns>
		public static T BuildUp<T>(IConfigurationSource configurationSource)
		{
			return BuildUp<T>(null, null, configurationSource);
		}

		/// <summary>
		/// Returns a default instance of type <typeparamref name="T"/> based on configuration information 
		/// from <paramref name="configurationSource"/>.
		/// </summary>
		/// <typeparam name="T">The type to build.</typeparam>
		/// <param name="locator">The locator to be used for this build operation.</param>
		/// <param name="lifetimeContainer">The lifetime container to be used for this build operation.</param>
		/// <param name="configurationSource">The source for configuration information.</param>
		/// <returns>A new instance of <typeparamref name="T"/> or any of it subtypes, or an existing instance
		/// if type <typeparamref name="T"/> is a singleton that is already present in the <paramref name="locator"/>.
		/// </returns>
		public static T BuildUp<T>(IReadWriteLocator locator,
								   ILifetimeContainer lifetimeContainer,
								   IConfigurationSource configurationSource)
		{
			if (configurationSource == null)
				throw new ArgumentNullException("configurationSource");

			try
			{
				return GetObjectBuilder()
					.BuildUp<T>(locator,
								lifetimeContainer,
								GetPolicies(configurationSource),
								strategyChain,
								NamedTypeBuildKey.Make<T>(),
								null);

			}
			catch (BuildFailedException e)
			{
				// look for the wrapped ConfigurationErrorsException, if any, and throw it
				ConfigurationErrorsException cee = GetConfigurationErrorsException(e);
				if (cee != null)
				{
					throw cee;
				}

				// unknown exception, bubble it up
				throw;
			}
		}

		/// <summary>
		/// Returns a new default instance of type <typeparamref name="T"/> based on configuration information 
		/// from the default configuration source for <paramref name="id"/>.
		/// </summary>
		/// <typeparam name="T">The type to build.</typeparam>
		/// <param name="id">The id of the object to build.</param>
		/// <returns>A new instance of <typeparamref name="T"/> or any of it subtypes.</returns>
		public static T BuildUp<T>(string id)
		{
			return BuildUp<T>(id, ConfigurationSource);
		}

		/// <summary>
		/// Returns a new default instance of type <typeparamref name="T"/> based on configuration information 
		/// from <paramref name="configurationSource"/> for <paramref name="id"/>.
		/// </summary>
		/// <typeparam name="T">The type to build.</typeparam>
		/// <param name="id">The id of the object to build.</param>
		/// <param name="configurationSource">The source for configuration information.</param>
		/// <returns>A new instance of <typeparamref name="T"/> or any of it subtypes.</returns>
		public static T BuildUp<T>(string id, IConfigurationSource configurationSource)
		{
			return BuildUp<T>(null, null, id, configurationSource);
		}

		/// <summary>
		/// Returns a default instance of type <typeparamref name="T"/> based on configuration information 
		/// from the default configuration source for <paramref name="id"/>.
		/// </summary>
		/// <typeparam name="T">The type to build.</typeparam>
		/// <param name="lifetimeContainer">The lifetime container to be used for this build operation.</param>
		/// <param name="id">The id of the object to build.</param>
		/// <param name="locator">The locator to be used for this build operation.</param>
		/// <returns>A new instance of <typeparamref name="T"/> or any of it subtypes, or an existing instance
		/// if type <typeparamref name="T"/> is a singleton that is already present in the <paramref name="locator"/>.
		/// </returns>
		public static T BuildUp<T>(IReadWriteLocator locator, ILifetimeContainer lifetimeContainer, string id)
		{
			return BuildUp<T>(locator, lifetimeContainer, id, ConfigurationSource);
		}

		/// <summary>
		/// Returns a default instance of type <typeparamref name="T"/> based on configuration information 
		/// from <paramref name="configurationSource"/> for <paramref name="id"/>.
		/// </summary>
		/// <typeparam name="T">The type to build.</typeparam>
		/// <param name="id">The id of the object to build.</param>
		/// <param name="locator">The locator to be used for this build operation.</param>
		/// <param name="lifetimeContainer">The lifetime container to be used for this build operation.</param>
		/// <param name="configurationSource">The source for configuration information.</param>
		/// <returns>A new instance of <typeparamref name="T"/> or any of it subtypes, or an existing instance
		/// if type <typeparamref name="T"/> is a singleton that is already present in the <paramref name="locator"/>.
		/// </returns>
		public static T BuildUp<T>(IReadWriteLocator locator,
								   ILifetimeContainer lifetimeContainer,
								   string id,
								   IConfigurationSource configurationSource)
		{
			if (string.IsNullOrEmpty(id))
				throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "id");
			if (configurationSource == null)
				throw new ArgumentNullException("configurationSource");

			try
			{
				return GetObjectBuilder()
					.BuildUp<T>(locator,
								lifetimeContainer,
								GetPolicies(configurationSource),
								strategyChain,
								NamedTypeBuildKey.Make<T>(id),
								null);
			}
			catch (BuildFailedException e)
			{
				// look for the wrapped ConfigurationErrorsException, if any, and throw it
				ConfigurationErrorsException cee = GetConfigurationErrorsException(e);
				if (cee != null)
				{
					throw cee;
				}

				// unknown exception, bubble it up
				throw;
			}
		}

		private static ConfigurationErrorsException GetConfigurationErrorsException(BuildFailedException e)
		{
			Exception currentException = e;

			while ((currentException = currentException.InnerException) != null)
			{
				if (currentException is ConfigurationErrorsException)
				{
					return currentException as ConfigurationErrorsException;
				}
			}

			return null;
		}

		private static PolicyList GetPolicies(IConfigurationSource configurationSource)
		{
			PolicyList policyList = new PolicyList();
			policyList.Set<IConfigurationObjectPolicy>(new ConfigurationObjectPolicy(configurationSource),
													   typeof(IConfigurationSource));
			policyList.Set<IReflectionCachePolicy>(new ReflectionCachePolicy(reflectionCache),
												   typeof(IReflectionCachePolicy));

			return policyList;
		}

		private static IBuilder GetObjectBuilder()
		{
			return builder;
		}

		private static IConfigurationSource ConfigurationSource
		{
			get { return ConfigurationSourceFactory.Create(); }
		}
	}
}
