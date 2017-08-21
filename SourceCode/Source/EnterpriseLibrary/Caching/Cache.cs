//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;
using Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    /// <summary>
    /// The real worker of the block. The Cache class is the traffic cop that prevents 
    /// resource contention among the different threads in the system. It also will act
    /// as the remoting gateway when that feature is added to the cache.
    /// </summary>	
	public class Cache : ICacheOperations, IDisposable
    {
        private Hashtable inMemoryCache;
        private ICacheScavenger cacheScavenger;
        private IBackingStore backingStore;
        private CacheCapacityScavengingPolicy scavengingPolicy;
		private CachingInstrumentationProvider instrumentationProvider;

        private const string addInProgressFlag = "Dummy variable used to flag temp cache item added during Add";

		/// <summary>
		/// Initialzie a new instance of a <see cref="Cache"/> class with a backing store, and scavenging policy.
		/// </summary>
		/// <param name="backingStore">The cache backing store.</param>
		/// <param name="scavengingPolicy">The scavenging policy.</param>
		/// <param name="instrumentationProvider">The instrumentation provider.</param>
		public Cache(IBackingStore backingStore, CacheCapacityScavengingPolicy scavengingPolicy, CachingInstrumentationProvider instrumentationProvider)
        {
            this.backingStore = backingStore;
            this.scavengingPolicy = scavengingPolicy;
			this.instrumentationProvider = instrumentationProvider;

            Hashtable initialItems = backingStore.Load();
            inMemoryCache = Hashtable.Synchronized(initialItems);

			this.instrumentationProvider.FireCacheUpdated(initialItems.Count, initialItems.Count);
        }
		
        /// <summary>
        /// Gets the count of <see cref="CacheItem"/> objects.
        /// </summary>
		/// <value>
		/// The count of <see cref="CacheItem"/> objects.
		/// </value>
		public int Count
        {
            get { return inMemoryCache.Count; }
        }

		/// <summary>
		/// Gets the current cache.
		/// </summary>
		/// <returns>
		/// The current cache.
		/// </returns>
		public Hashtable CurrentCacheState
		{
			get { return (Hashtable)inMemoryCache.Clone(); }
		}

		/// <summary>
		/// Determines if a particular key is contained in the cache.
		/// </summary>
		/// <param name="key">The key to locate.</param>
		/// <returns>
		/// <see langword="true"/> if the key is contained in the cache; otherwise, <see langword="false"/>.
		/// </returns>
        public bool Contains(string key)
        {
            ValidateKey(key);

            return inMemoryCache.Contains(key);
        }

		/// <summary>
		/// Initialize the cache with a scavenger.
		/// </summary>
		/// <param name="cacheScavengerToUse">
		/// An <see cref="ICacheScavenger"/> object.
		/// </param>
        public void Initialize(ICacheScavenger cacheScavengerToUse)
        {
            this.cacheScavenger = cacheScavengerToUse;
        }

		/// <summary>
		/// Add a new keyed object to the cache.
		/// </summary>
		/// <param name="key">The key of the object.</param>
		/// <param name="value">The object to add.</param>
        public void Add(string key, object value)
        {
            Add(key, value, CacheItemPriority.Normal, null);
        }

		/// <summary>
		/// Add a new keyed object to the cache.
		/// </summary>
		/// <param name="key">The key of the object.</param>
		/// <param name="value">The object to add.</param>
		/// <param name="scavengingPriority">One of the <see cref="CacheItemPriority"/> values.</param>
		/// <param name="refreshAction">An <see cref="ICacheItemRefreshAction"/> object.</param>
		/// <param name="expirations">An array of <see cref="ICacheItemExpiration"/> objects.</param>
        public void Add(string key, object value, CacheItemPriority scavengingPriority, ICacheItemRefreshAction refreshAction, params ICacheItemExpiration[] expirations)
        {
            ValidateKey(key);
            EnsureCacheInitialized();

            CacheItem cacheItemBeforeLock = null;
            bool lockWasSuccessful = false;

            do
            {
                lock (inMemoryCache.SyncRoot)
                {
                    if (inMemoryCache.Contains(key) == false)
                    {
                        cacheItemBeforeLock = new CacheItem(key, addInProgressFlag, CacheItemPriority.NotRemovable, null);
                        inMemoryCache[key] = cacheItemBeforeLock;
                    }
                    else
                    {
                        cacheItemBeforeLock = (CacheItem)inMemoryCache[key];
                    }

                    lockWasSuccessful = Monitor.TryEnter(cacheItemBeforeLock);
                }

                if (lockWasSuccessful == false)
                {
                    Thread.Sleep(0);
                }
            } while (lockWasSuccessful == false);

            try
            {
                cacheItemBeforeLock.TouchedByUserAction(true);

                CacheItem newCacheItem = new CacheItem(key, value, scavengingPriority, refreshAction, expirations);
                try
                {
                    backingStore.Add(newCacheItem);
                    cacheItemBeforeLock.Replace(value, refreshAction, scavengingPriority, expirations);
                    inMemoryCache[key] = cacheItemBeforeLock;
                }
                catch
                {
                    backingStore.Remove(key);
                    inMemoryCache.Remove(key);
                    throw;
                }

                if (scavengingPolicy.IsScavengingNeeded(inMemoryCache.Count))
                {
                    cacheScavenger.StartScavenging();
                }

				instrumentationProvider.FireCacheUpdated(1, inMemoryCache.Count);
            }
            finally
            {
                Monitor.Exit(cacheItemBeforeLock);
            }
        }

		/// <summary>
		/// Remove an item from the cache by key.
		/// </summary>
		/// <param name="key">The key of the item to remove.</param>
        public void Remove(string key)
        {
            Remove(key, CacheItemRemovedReason.Removed);
        }

		/// <summary>
		/// Remove an item from the cache by key.
		/// </summary>
		/// <param name="key">The key of the item to remove.</param>
		/// <param name="removalReason">One of the <see cref="CacheItemRemovedReason"/> values.</param>
        public void Remove(string key, CacheItemRemovedReason removalReason)
        {
            ValidateKey(key);

            CacheItem cacheItemBeforeLock = null;
            bool lockWasSuccessful;
            do
            {
                lock (inMemoryCache.SyncRoot)
                {
                    cacheItemBeforeLock = (CacheItem)inMemoryCache[key];

                    if (IsObjectInCache(cacheItemBeforeLock))
                    {
                        return;
                    }

                    lockWasSuccessful = Monitor.TryEnter(cacheItemBeforeLock);
                }

                if (lockWasSuccessful == false)
                {
                    Thread.Sleep(0);
                }
            } while (lockWasSuccessful == false);

            try
            {
                cacheItemBeforeLock.TouchedByUserAction(true);

                backingStore.Remove(key); // Does exception safety matter here? We're removing it due to expiration or scavenging...
                inMemoryCache.Remove(key);

                RefreshActionInvoker.InvokeRefreshAction(cacheItemBeforeLock, removalReason, instrumentationProvider);

				instrumentationProvider.FireCacheUpdated(1, inMemoryCache.Count);
			}
            finally
            {
                Monitor.Exit(cacheItemBeforeLock);
            }

        }

		
        /// <summary>
        /// Removes an item from the cache.
        /// </summary>
        /// <param name="key">The key to remove.</param>
		/// <param name="removalReason">One of the <see cref="CacheItemRemovedReason"/> values.</param>
		/// <remarks>
		/// This seemingly redundant method is here to be called through the ICacheOperations 
		/// interface. I put this in place to break any dependency from any other class onto 
		/// the Cache class
		/// </remarks>
        public void RemoveItemFromCache(string key, CacheItemRemovedReason removalReason)
        {
            Remove(key, removalReason);
        }

        /// <summary>
        /// Get the object from the cache for the key.
        /// </summary>
        /// <param name="key">
		/// The key whose value to get.
		/// </param>
        /// <returns>
		/// The value associated with the specified key. 
		/// </returns>
		public object GetData(string key)
        {
            ValidateKey(key);
            CacheItem cacheItemBeforeLock = null;
            bool lockWasSuccessful = false;

            do
            {
                lock (inMemoryCache.SyncRoot)
                {
                    cacheItemBeforeLock = (CacheItem)inMemoryCache[key];
                    if (IsObjectInCache(cacheItemBeforeLock))
                    {
						instrumentationProvider.FireCacheAccessed(key, false);
                        return null;
                    }

                    lockWasSuccessful = Monitor.TryEnter(cacheItemBeforeLock);
                }

                if (lockWasSuccessful == false)
                {
                    Thread.Sleep(0);
                }
            } while (lockWasSuccessful == false);

            try
            {
                if (cacheItemBeforeLock.HasExpired())
                {
                    cacheItemBeforeLock.TouchedByUserAction(true);

                    backingStore.Remove(key); // Does exception safety matter here? We're removing it due to expiration or scavenging...
                    inMemoryCache.Remove(key);

                    RefreshActionInvoker.InvokeRefreshAction(cacheItemBeforeLock, CacheItemRemovedReason.Expired, instrumentationProvider);

					instrumentationProvider.FireCacheAccessed(key, false);
					instrumentationProvider.FireCacheUpdated(1, inMemoryCache.Count);
					instrumentationProvider.FireCacheExpired(1);
					return null;
                }

                backingStore.UpdateLastAccessedTime(cacheItemBeforeLock.Key, DateTime.Now); // Does exception safety matter here?
                cacheItemBeforeLock.TouchedByUserAction(false);

				instrumentationProvider.FireCacheAccessed(key, true);
				return cacheItemBeforeLock.Value;
            }
            finally
            {
                Monitor.Exit(cacheItemBeforeLock);
            }
        }

		/// <summary>
		/// Flush the cache.
		/// </summary>
		/// <remarks>
        /// There may still be thread safety issues in this class with respect to cacheItemExpirations
        /// and scavenging, but I really doubt that either of those will be happening while
        /// a Flush is in progress. It seems that the most likely scenario for a flush
        /// to be called is at the very start of a program, or when absolutely nothing else
        /// is going on. Calling flush in the middle of an application would seem to be
        /// an "interesting" thing to do in normal circumstances.
        /// </remarks>
		public void Flush()
        {
            RestartFlushAlgorithm:
            lock (inMemoryCache.SyncRoot)
            {
                foreach (string key in inMemoryCache.Keys)
                {
                    bool lockWasSuccessful = false;
                    CacheItem itemToRemove = (CacheItem)inMemoryCache[key];
                    try
                    {
                        if(lockWasSuccessful = Monitor.TryEnter(itemToRemove))
                        {
                            itemToRemove.TouchedByUserAction(true);
                        }
                        else
                        {
                           goto RestartFlushAlgorithm;
                        }
                    }
                    finally
                    {
                        if(lockWasSuccessful) Monitor.Exit(itemToRemove);
                    }
                }

                int countBeforeFlushing = inMemoryCache.Count;

                backingStore.Flush();
                inMemoryCache.Clear();

				instrumentationProvider.FireCacheUpdated(countBeforeFlushing, 0);
            }
        }

        private static void ValidateKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
				throw new ArgumentException(Resources.EmptyParameterName, "key");
            }            
        }

        private void EnsureCacheInitialized()
        {
            if (cacheScavenger == null)
            {
                throw new InvalidOperationException(Resources.CacheNotInitializedException);
            }
        }

        private static bool IsObjectInCache(CacheItem cacheItemBeforeLock)
        {
            return cacheItemBeforeLock == null || Object.ReferenceEquals(cacheItemBeforeLock.Value, addInProgressFlag);
        }


		/// <summary>
		/// Dispose of the backing store before garbage collection.
		/// </summary>
        ~Cache()
        {
            Dispose(false);
        }

		/// <summary>
		/// Dispose of the backing store before garbage collection.
		/// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

		/// <summary>
		/// Dispose of the backing store before garbage collection.
		/// </summary>
        /// <param name="disposing">
		/// <see langword="true"/> if disposing; otherwise, <see langword="false"/>.
		/// </param>
		protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                backingStore.Dispose();
                backingStore = null;
            }
        }
    }
}
