/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;
using Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    public class ScavengerTask
    {
        private CacheCapacityScavengingPolicy scavengingPolicy;
        private readonly int numberToRemoveWhenScavenging;
        private ICacheOperations cacheOperations;
        private CachingInstrumentationProvider instrumentationProvider;
        public ScavengerTask(int numberToRemoveWhenScavenging,
                               CacheCapacityScavengingPolicy scavengingPolicy,
                               ICacheOperations cacheOperations,
                               CachingInstrumentationProvider instrumentationProvider)
        {
            this.numberToRemoveWhenScavenging = numberToRemoveWhenScavenging;
            this.scavengingPolicy = scavengingPolicy;
            this.cacheOperations = cacheOperations;
            this.instrumentationProvider = instrumentationProvider;
        }
        public void DoScavenging()
        {
            if (NumberOfItemsToBeScavenged == 0) return;
            Hashtable liveCacheRepresentation = cacheOperations.CurrentCacheState;
            int currentNumberItemsInCache = liveCacheRepresentation.Count;
            if (scavengingPolicy.IsScavengingNeeded(currentNumberItemsInCache))
            {
                ResetScavengingFlagInCacheItems(liveCacheRepresentation);
                SortedList scavengableItems = SortItemsForScavenging(liveCacheRepresentation);
                RemoveScavengableItems(scavengableItems);
            }
        }
        private static void ResetScavengingFlagInCacheItems(Hashtable liveCacheRepresentation)
        {
            foreach (CacheItem cacheItem in liveCacheRepresentation.Values)
            {
                lock (cacheItem)
                {
                    cacheItem.MakeEligibleForScavenging();
                }
            }
        }
        private static SortedList SortItemsForScavenging(Hashtable unsortedItemsInCache)
        {
            return new SortedList(unsortedItemsInCache, new PriorityDateComparer(unsortedItemsInCache));
        }
        internal int NumberOfItemsToBeScavenged
        {
            get { return this.numberToRemoveWhenScavenging; }
        }
        private void RemoveScavengableItems(SortedList scavengableItems)
        {
            int scavengedItemCount = 0;
            foreach (CacheItem scavengableItem in scavengableItems.Values)
            {
                bool wasRemoved = RemoveItemFromCache(scavengableItem);
                if (wasRemoved)
                {
                    scavengedItemCount++;
                    if (scavengedItemCount == NumberOfItemsToBeScavenged)
                    {
                        break;
                    }
                }
            }
            instrumentationProvider.FireCacheScavenged(scavengedItemCount);
        }
        private bool RemoveItemFromCache(CacheItem itemToRemove)
        {
            lock (itemToRemove)
            {
                if (itemToRemove.EligibleForScavenging)
                {
                    try
                    {
                        cacheOperations.RemoveItemFromCache(itemToRemove.Key, CacheItemRemovedReason.Scavenged);
                        return true;
                    }
                    catch (Exception e)
                    {
                        instrumentationProvider.FireCacheFailed(Resources.FailureToRemoveCacheItemInBackground, e);
                    }
                }
            }
            return false;
        }
    }
}
