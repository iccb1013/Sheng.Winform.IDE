/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability
{
	public static class CacheManagerSettingsWmiMapper
	{
		public static void GenerateWmiObjects(CacheManagerSettings configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(new CachingBlockSetting(configurationObject, configurationObject.DefaultCacheManager));
		}
		public static void GenerateCacheManagerWmiObjects(CacheManagerDataBase data,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			if (data is CacheManagerData)
			{
				CacheManagerData cacheManagerData = (CacheManagerData)data;
				wmiSettings.Add(
					new CacheManagerSetting(data.Name,
								cacheManagerData.CacheStorage,
								cacheManagerData.ExpirationPollFrequencyInSeconds,
								cacheManagerData.MaximumElementsInCacheBeforeScavenging,
								cacheManagerData.NumberToRemoveWhenScavenging));
			}
			else
			{
				wmiSettings.Add(new UnknownCacheManagerSetting(data.Name));
			}
		}
		public static bool SaveChanges(CachingBlockSetting cachingBlockSetting, ConfigurationElement sourceElement)
		{
			CacheManagerSettings settings = (CacheManagerSettings)sourceElement;
			settings.DefaultCacheManager = cachingBlockSetting.DefaultCacheManager;
			return true;
		}
		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(CachingBlockSetting),
				typeof(CacheManagerSetting),
				typeof(UnknownCacheManagerSetting));
		}
	}
}
