/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
	public class CacheManagerAssembler : IAssembler<ICacheManager, CacheManagerDataBase>
	{
		public ICacheManager Assemble(IBuilderContext context, CacheManagerDataBase objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			CacheManagerData cacheManagerData = (CacheManagerData)objectConfiguration;
			IBackingStore backingStore
				= BackingStoreCustomFactory.Instance.Create(context, cacheManagerData.CacheStorage, configurationSource, reflectionCache);
			CachingInstrumentationProvider instrumentationProvider = CreateInstrumentationProvider(cacheManagerData.Name, configurationSource, reflectionCache);
			CacheManager createdObject
				= new CacheManagerFactoryHelper().BuildCacheManager(
					cacheManagerData.Name,
					backingStore,
					cacheManagerData.MaximumElementsInCacheBeforeScavenging,
					cacheManagerData.NumberToRemoveWhenScavenging,
					cacheManagerData.ExpirationPollFrequencyInSeconds,
					instrumentationProvider);
			return createdObject;
		}
		private static CachingInstrumentationProvider CreateInstrumentationProvider(string name, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			CachingInstrumentationProvider instrumentationProvider = new CachingInstrumentationProvider();
			new InstrumentationAttachmentStrategy().AttachInstrumentation(name, instrumentationProvider, configurationSource, reflectionCache);
			return instrumentationProvider;
		}
	}
	public class CacheManagerFactoryHelper
	{
		public CacheManager BuildCacheManager(
			string cacheManagerName,
			IBackingStore backingStore,
			int maximumElementsInCacheBeforeScavenging,
			int numberToRemoveWhenScavenging,
			int expirationPollFrequencyInSeconds,
			CachingInstrumentationProvider instrumentationProvider)
		{
			CacheCapacityScavengingPolicy scavengingPolicy =
				new CacheCapacityScavengingPolicy(maximumElementsInCacheBeforeScavenging);
			Cache cache = new Cache(backingStore, scavengingPolicy, instrumentationProvider);
			ExpirationPollTimer timer = new ExpirationPollTimer();
			ExpirationTask expirationTask = CreateExpirationTask(cache, instrumentationProvider);
			ScavengerTask scavengerTask = new ScavengerTask(numberToRemoveWhenScavenging, scavengingPolicy, cache, instrumentationProvider);
			BackgroundScheduler scheduler = new BackgroundScheduler(expirationTask, scavengerTask, instrumentationProvider);
			cache.Initialize(scheduler);
			scheduler.Start();
			timer.StartPolling(new TimerCallback(scheduler.ExpirationTimeoutExpired), expirationPollFrequencyInSeconds * 1000);
			return new CacheManager(cache, scheduler, timer);
		}
		public virtual ExpirationTask CreateExpirationTask(ICacheOperations cacheOperations, CachingInstrumentationProvider instrumentationProvider)
		{
			return new ExpirationTask(cacheOperations, instrumentationProvider);
		}
	}
}
