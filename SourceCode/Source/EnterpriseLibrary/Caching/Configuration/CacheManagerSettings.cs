/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
    public class CacheManagerSettings : SerializableConfigurationSection
    {
		public const string SectionName = "cachingConfiguration";
        private const string defaultCacheManagerProperty = "defaultCacheManager";
		private const string cacheManagersProperty = "cacheManagers";
		private const string backingStoresProperty = "backingStores";
		private const string encryptionProvidersProperty = "encryptionProviders";    
		[ConfigurationProperty(defaultCacheManagerProperty, IsRequired= true)]
        public string DefaultCacheManager
        {
			get { return (string)base[defaultCacheManagerProperty]; }
			set { base[defaultCacheManagerProperty] = value; }
        }
        [ConfigurationProperty(cacheManagersProperty, IsRequired= true)]
		public NameTypeConfigurationElementCollection<CacheManagerDataBase, CustomCacheManagerData> CacheManagers
		{
			get { return (NameTypeConfigurationElementCollection<CacheManagerDataBase, CustomCacheManagerData>)base[cacheManagersProperty]; }
		}
		[ConfigurationProperty(backingStoresProperty, IsRequired= false)]
		public NameTypeConfigurationElementCollection<CacheStorageData, CustomCacheStorageData> BackingStores
		{
            get { return (NameTypeConfigurationElementCollection<CacheStorageData, CustomCacheStorageData>)base[backingStoresProperty]; }
		}
		[ConfigurationProperty(encryptionProvidersProperty, IsRequired= false)]
        public NameTypeConfigurationElementCollection<StorageEncryptionProviderData, StorageEncryptionProviderData> EncryptionProviders
		{
            get { return (NameTypeConfigurationElementCollection<StorageEncryptionProviderData, StorageEncryptionProviderData>)base[encryptionProvidersProperty]; }
		}
    }
}
