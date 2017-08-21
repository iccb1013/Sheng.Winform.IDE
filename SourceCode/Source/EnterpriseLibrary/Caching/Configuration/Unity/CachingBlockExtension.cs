/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Unity
{
	public class CachingBlockExtension : EnterpriseLibraryBlockExtension
	{
		protected override void Initialize()
		{
			CacheManagerSettings settings
				= (CacheManagerSettings)ConfigurationSource.GetSection(CacheManagerSettings.SectionName);
			if (settings == null)
			{
				return;
			}
			CreateProvidersPolicies<IBackingStore, CacheStorageData>(
				Context.Policies,
				null,
				settings.BackingStores,
				ConfigurationSource);
			CreateProvidersPolicies<IStorageEncryptionProvider, StorageEncryptionProviderData>(
				Context.Policies,
				null,
				settings.EncryptionProviders,
				ConfigurationSource);
			CreateProvidersPolicies<ICacheManager, CacheManagerDataBase>(
				Context.Policies,
				settings.DefaultCacheManager,
				settings.CacheManagers,
				ConfigurationSource);
            CreateCacheManagerLifetimePolicies(
                Context.Policies,
                Context.Container,
                settings.CacheManagers);
        }
        private static void CreateCacheManagerLifetimePolicies(IPolicyList policyList, IUnityContainer container, IEnumerable<CacheManagerDataBase> cacheManagers)
        {
            foreach (var cacheManagerData in cacheManagers)
            {
                container.RegisterType(cacheManagerData.Type, cacheManagerData.Name, new ContainerControlledLifetimeManager());
            }
        }
	}
}
