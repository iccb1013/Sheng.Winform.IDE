/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability
{
    [ManagementEntity]
    public class CacheManagerSetting : CacheManagerBaseSetting
    {
        string cacheStorage;
        int expirationPollFrequencyInSeconds;
        int maximumElementsInCacheBeforeScavenging;
        int numberToRemoveWhenScavenging;
        public CacheManagerSetting(string name,
                                   string cacheStorage,
                                   int expirationPollFrequencyInSeconds,
                                   int maximumElementsInCacheBeforeScavenging,
                                   int numberToRemoveWhenScavenging)
            : base(name)
        {
            this.cacheStorage = cacheStorage;
            this.expirationPollFrequencyInSeconds = expirationPollFrequencyInSeconds;
            this.maximumElementsInCacheBeforeScavenging = maximumElementsInCacheBeforeScavenging;
            this.numberToRemoveWhenScavenging = numberToRemoveWhenScavenging;
        }
        [ManagementProbe]
        public string CacheStorage
        {
            get { return cacheStorage; }
            set { cacheStorage = value; }
        }
        [ManagementProbe]
        public int ExpirationPollFrequencyInSeconds
        {
            get { return expirationPollFrequencyInSeconds; }
            set { expirationPollFrequencyInSeconds = value; }
        }
        [ManagementProbe]
        public int MaximumElementsInCacheBeforeScavenging
        {
            get { return maximumElementsInCacheBeforeScavenging; }
            set { maximumElementsInCacheBeforeScavenging = value; }
        }
        [ManagementProbe]
        public int NumberToRemoveWhenScavenging
        {
            get { return numberToRemoveWhenScavenging; }
            set { numberToRemoveWhenScavenging = value; }
        }
        [ManagementBind]
        public static CacheManagerSetting BindInstance(string ApplicationName,
                                                       string SectionName,
                                                       string Name)
        {
            return BindInstance<CacheManagerSetting>(ApplicationName, SectionName, Name);
        }
        [ManagementEnumerator]
        public static IEnumerable<CacheManagerSetting> GetInstances()
        {
            return GetInstances<CacheManagerSetting>();
        }
    }
}
