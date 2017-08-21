/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Management.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability
{
    [ManagementEntity]
    public class CustomCacheStorageSetting : CacheStorageSetting
    {
        string[] attributes;
        string providerType;
        public CustomCacheStorageSetting(string name,
                                         string providerType,
                                         string[] attributes)
            : base(name)
        {
            this.providerType = providerType;
            this.attributes = attributes;
        }
        [ManagementProbe]
        public string[] Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }
        [ManagementProbe]
        public string ProviderType
        {
            get { return providerType; }
            set { providerType = value; }
        }
        [ManagementBind]
        public static CustomCacheStorageSetting BindInstance(string ApplicationName,
                                                             string SectionName,
                                                             string Name)
        {
            return BindInstance<CustomCacheStorageSetting>(ApplicationName, SectionName, Name);
        }
        [ManagementEnumerator]
        public static IEnumerable<CustomCacheStorageSetting> GetInstances()
        {
            return GetInstances<CustomCacheStorageSetting>();
        }
    }
}
