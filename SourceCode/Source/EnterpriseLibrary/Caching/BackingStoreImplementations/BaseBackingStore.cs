/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations
{
    public abstract class BaseBackingStore : IBackingStore, IDisposable
    {
        protected BaseBackingStore()
        {
        }
        ~BaseBackingStore()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
        }
        public abstract int Count { get; }
        public void Remove(string key)
        {
            Remove(key.GetHashCode());
        }
        protected abstract void Remove(int storageKey);
        public void UpdateLastAccessedTime(string key, DateTime timestamp)
        {
            UpdateLastAccessedTime(key.GetHashCode(), timestamp);
        }
        protected abstract void UpdateLastAccessedTime(int storageKey, DateTime timestamp);
        public abstract void Flush();
        public virtual void Add(CacheItem newCacheItem)
        {
            try
            {
                RemoveOldItem(newCacheItem.Key.GetHashCode());
                AddNewItem(newCacheItem.Key.GetHashCode(), newCacheItem);
            }
            catch
            {
                Remove(newCacheItem.Key.GetHashCode());
                throw;
            }
        }
        public virtual Hashtable Load()
        {
            return LoadDataFromStore();
        }
        protected abstract void RemoveOldItem(int storageKey);
        protected abstract void AddNewItem(int storageKey, CacheItem newItem);
        protected abstract Hashtable LoadDataFromStore();
    }
}
