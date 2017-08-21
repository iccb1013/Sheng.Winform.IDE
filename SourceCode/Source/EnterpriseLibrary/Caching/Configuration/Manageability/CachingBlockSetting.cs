/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability
{
    [ManagementEntity]
    public class CachingBlockSetting : ConfigurationSectionSetting
    {
        string defaultCacheManager;
        public CachingBlockSetting(CacheManagerSettings settings,
                                   string defaultCacheManager)
            : base(settings)
        {
            this.defaultCacheManager = defaultCacheManager;
        }
        [ManagementConfiguration]
        public string DefaultCacheManager
        {
            get { return defaultCacheManager; }
            set { defaultCacheManager = value; }
        }
        [ManagementBind]
        public static CachingBlockSetting BindInstance(string ApplicationName,
                                                       string SectionName)
        {
            return BindInstance<CachingBlockSetting>(ApplicationName, SectionName);
        }
        [ManagementEnumerator]
        public static IEnumerable<CachingBlockSetting> GetInstances()
        {
            return GetInstances<CachingBlockSetting>();
        }
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return CacheManagerSettingsWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}
