/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections;
namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    public interface ICacheOperations
    {
        Hashtable CurrentCacheState { get; }
        void RemoveItemFromCache(string key, CacheItemRemovedReason removalReason);
    }
}
