/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
[assembly : ConfigurationSectionManageabilityProvider(CacheManagerSettings.SectionName, typeof(CacheManagerSettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(CacheStorageDataManageabilityProvider), typeof(CacheStorageData), typeof(CacheManagerSettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(CustomCacheStorageDataManageabilityProvider), typeof(CustomCacheStorageData), typeof(CacheManagerSettingsManageabilityProvider))]
[assembly : ConfigurationElementManageabilityProvider(typeof(IsolatedStorageCacheStorageDataManageabilityProvider), typeof(IsolatedStorageCacheStorageData), typeof(CacheManagerSettingsManageabilityProvider))]
