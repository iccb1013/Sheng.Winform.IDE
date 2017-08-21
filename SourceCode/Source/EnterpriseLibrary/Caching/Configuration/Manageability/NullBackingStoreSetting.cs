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
    public class NullBackingStoreSetting : CacheStorageSetting
    {
        string storageEncryption;
        public NullBackingStoreSetting(string name,
                                       string storageEncryption)
            : base(name)
        {
            this.storageEncryption = storageEncryption;
        }
        [ManagementProbe]
        public string StorageEncryption
        {
            get { return storageEncryption; }
            set { storageEncryption = value; }
        }
        [ManagementBind]
        public static NullBackingStoreSetting BindInstance(string ApplicationName,
                                                           string SectionName,
                                                           string Name)
        {
            return BindInstance<NullBackingStoreSetting>(ApplicationName, SectionName, Name);
        }
        [ManagementEnumerator]
        public static IEnumerable<NullBackingStoreSetting> GetInstances()
        {
            return GetInstances<NullBackingStoreSetting>();
        }
    }
}
