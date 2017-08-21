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
    public class UnknownCacheManagerSetting : CacheManagerBaseSetting
    {
        public UnknownCacheManagerSetting(string name)
            : base(name) {}
        [ManagementBind]
        public static UnknownCacheManagerSetting BindInstance(string ApplicationName,
                                                              string SectionName,
                                                              string Name)
        {
            return BindInstance<UnknownCacheManagerSetting>(ApplicationName, SectionName, Name);
        }
        [ManagementEnumerator]
        public static IEnumerable<UnknownCacheManagerSetting> GetInstances()
        {
            return GetInstances<UnknownCacheManagerSetting>();
        }
    }
}
