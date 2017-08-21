/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Expirations
{
    [Serializable]
    public class NeverExpired : ICacheItemExpiration
    {
        public bool HasExpired()
        {
            return false;
        }
        public void Notify()
        {
        }
        public void Initialize(CacheItem owningCacheItem)
        {
        }
    }
}
