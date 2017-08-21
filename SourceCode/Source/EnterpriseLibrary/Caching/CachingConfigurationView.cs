/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    public class CachingConfigurationView
    {
        private IConfigurationSource configurationSource;
        public CachingConfigurationView(IConfigurationSource configurationSource)
        {
            this.configurationSource = configurationSource;
        }
        public CacheStorageData GetCacheStorageData(string name)
        {
            CacheManagerSettings settings = this.CacheManagerSettings;
            if (!settings.BackingStores.Contains(name))
            {
                throw new ConfigurationErrorsException(string.Format(Resources.Culture, Resources.ExceptionBackingStoreNotDefined, name));
            }
            return settings.BackingStores.Get(name);
        }
        public string DefaultCacheManager
        {
            get
            {
                CacheManagerSettings configSettings = this.CacheManagerSettings;
                if (string.IsNullOrEmpty(configSettings.DefaultCacheManager))
                {
                    throw new ConfigurationErrorsException(Resources.NoDefaultCacheManager);
                }
                return configSettings.DefaultCacheManager;
            }
        }
        public CacheManagerSettings CacheManagerSettings
        {
            get
            {
                CacheManagerSettings settings
                    = (CacheManagerSettings)configurationSource.GetSection(CacheManagerSettings.SectionName);
                if (settings == null)
                {
                    throw new ConfigurationErrorsException(Resources.MissingSection);
                }
                return settings;
            }
        }
        public CacheManagerDataBase GetCacheManagerData(string cacheManagerName)
        {
            CacheManagerSettings configSettings = this.CacheManagerSettings;
            CacheManagerDataBase data = configSettings.CacheManagers.Get(cacheManagerName);
            if (data == null)
            {
                throw new ConfigurationErrorsException(string.Format(Resources.Culture, Resources.UnableToFindCacheManagerInstance, cacheManagerName));
            }
            return data;
        }
        public StorageEncryptionProviderData GetStorageEncryptionProviderData(string name)
        {
            CacheManagerSettings settings = this.CacheManagerSettings;
            if (!settings.EncryptionProviders.Contains(name))
            {
                throw new ConfigurationErrorsException(string.Format(Resources.Culture, Resources.ExceptionEncryptionProviderNotDefined, name));
            }
            return settings.EncryptionProviders.Get(name);
        }
    }
}
