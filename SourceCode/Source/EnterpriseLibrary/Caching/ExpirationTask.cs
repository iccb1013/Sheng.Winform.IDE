/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    public class ExpirationTask
    {
        private ICacheOperations cacheOperations;
		private CachingInstrumentationProvider instrumentationProvider;
		public ExpirationTask(ICacheOperations cacheOperations, CachingInstrumentationProvider instrumentationProvider)
        {
            this.cacheOperations = cacheOperations;
			this.instrumentationProvider = instrumentationProvider;
        }
        public void DoExpirations()
        {
            Hashtable liveCacheRepresentation = cacheOperations.CurrentCacheState;
            MarkAsExpired(liveCacheRepresentation);
            PrepareForSweep();
            int expiredItemsCount = SweepExpiredItemsFromCache(liveCacheRepresentation);
			if(expiredItemsCount > 0) instrumentationProvider.FireCacheExpired(expiredItemsCount);
        }
        public virtual int MarkAsExpired(Hashtable liveCacheRepresentation)
        {
            int markedCount = 0;
            foreach (CacheItem cacheItem in liveCacheRepresentation.Values)
            {
                lock (cacheItem)
                {
                    if (cacheItem.HasExpired())
                    {
                        markedCount++;
                        cacheItem.WillBeExpired = true;
                    }
                }
            }
            return markedCount;
        }
		public virtual int SweepExpiredItemsFromCache(Hashtable liveCacheRepresentation)
        {
			int expiredItems = 0;
            foreach (CacheItem cacheItem in liveCacheRepresentation.Values)
            {
				if (RemoveItemFromCache(cacheItem))
					expiredItems++;
            }
			return expiredItems;
        }
        public virtual void PrepareForSweep()
        {
        }
		private bool RemoveItemFromCache(CacheItem itemToRemove)
        {
			bool expired = false;
            lock (itemToRemove)
            {
                if (itemToRemove.WillBeExpired)
                {
					try
					{
						expired = true;
						cacheOperations.RemoveItemFromCache(itemToRemove.Key, CacheItemRemovedReason.Expired);
					}
					catch (Exception e)
					{
						instrumentationProvider.FireCacheFailed(Resources.FailureToRemoveCacheItemInBackground, e);
					}                    
                }
            }
			return expired;
        }
    }
}
