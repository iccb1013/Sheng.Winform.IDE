/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations
{
	[ConfigurationElementType(typeof(CacheStorageData))]
	public class NullBackingStore : IBackingStore
    {   
        public int Count
        {
            get { return 0; }
        }
        public NullBackingStore()
        {
        }        
        public void Add(CacheItem newCacheItem)
        {
        }
        public void Remove(string key)
        {
        }
        public void UpdateLastAccessedTime(string key, DateTime timestamp)
        {
        }
        public void Flush()
        {
        }
        public Hashtable Load()
        {
            return new Hashtable();
        }
        public void Dispose()
        {
        }
    }
}
