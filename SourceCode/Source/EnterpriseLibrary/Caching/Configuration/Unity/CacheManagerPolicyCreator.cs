/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Unity
{
	public class CacheManagerPolicyCreator : IContainerPolicyCreator
	{
		void IContainerPolicyCreator.CreatePolicies(
			IPolicyList policyList,
			string instanceName,
			ConfigurationElement configurationObject,
			IConfigurationSource configurationSource)
		{
			CacheManagerData castConfigurationObject = (CacheManagerData)configurationObject;
			var cacheManagerName = instanceName;
			var cacheStorageName = castConfigurationObject.CacheStorage;
			var maximumElementsInCacheBeforeScavenging = castConfigurationObject.MaximumElementsInCacheBeforeScavenging;
			var numberToRemoveWhenScavenging = castConfigurationObject.NumberToRemoveWhenScavenging;
			var expirationPollFrequencyInSeconds = castConfigurationObject.ExpirationPollFrequencyInSeconds;
			policyList.Set<IBuildPlanPolicy>(
				new DelegateBuildPlanPolicy(
					context =>
					{
						IBuilderContext backingStoreContext 
							= context.CloneForNewBuild(NamedTypeBuildKey.Make<IBackingStore>(cacheStorageName), null);
						IBackingStore backingStore = (IBackingStore)context.Strategies.ExecuteBuildUp(backingStoreContext);
						CachingInstrumentationProvider instrumentationProvider
							= CreateInstrumentationProvider(cacheManagerName, configurationSource);
						return new CacheManagerFactoryHelper().BuildCacheManager(
							cacheManagerName,
							backingStore,
							maximumElementsInCacheBeforeScavenging,
							numberToRemoveWhenScavenging,
							expirationPollFrequencyInSeconds,
							instrumentationProvider);
					}),
				NamedTypeBuildKey.Make<CacheManager>(cacheManagerName));
		}
		private readonly static ConfigurationReflectionCache reflectionCache = new ConfigurationReflectionCache();
		private static CachingInstrumentationProvider CreateInstrumentationProvider(string name, IConfigurationSource configurationSource)
		{
			CachingInstrumentationProvider instrumentationProvider = new CachingInstrumentationProvider();
			new InstrumentationAttachmentStrategy()
				.AttachInstrumentation(name, instrumentationProvider, configurationSource, reflectionCache);
			return instrumentationProvider;
		}
	}
}
