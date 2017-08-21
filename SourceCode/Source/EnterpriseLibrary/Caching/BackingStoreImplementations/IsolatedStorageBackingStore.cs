/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations
{
	[ConfigurationElementType(typeof(IsolatedStorageCacheStorageData))]
	public class IsolatedStorageBackingStore : BaseBackingStore
	{
		private string storageAreaName;
		private IsolatedStorageFile store;
		private IStorageEncryptionProvider encryptionProvider;
		[IsolatedStorageFilePermission(SecurityAction.Demand, UsageAllowed = IsolatedStorageContainment.DomainIsolationByUser)]
		public IsolatedStorageBackingStore(string storageAreaName)
			: this(storageAreaName, null)
		{
		}
		[IsolatedStorageFilePermission(SecurityAction.Demand, UsageAllowed = IsolatedStorageContainment.DomainIsolationByUser)]
		public IsolatedStorageBackingStore(string storageAreaName, IStorageEncryptionProvider encryptionProvider)
		{
			if (string.IsNullOrEmpty(storageAreaName)) throw new ArgumentException(Resources.ExceptionStorageAreaNullOrEmpty, "storageAreaName");
			this.storageAreaName = storageAreaName;
			this.encryptionProvider = encryptionProvider;
			Initialize();
		}
		public override int Count
		{
			get { return GetSize(); }
		}
		public override void Flush()
		{
			lock (store)
			{
				string[] directories = store.GetDirectoryNames(GenerateSearchString(storageAreaName));
				foreach (string itemLocation in directories)
				{
					string itemRoot = GenerateItemLocation(itemLocation);
					RemoveItem(itemRoot);
				}
			}
		}
		protected override void Remove(int storageKey)
		{
			lock (store)
			{
				if (ItemExists(storageKey))
				{
					RemoveItem(GenerateItemLocation(storageKey));
				}
			}
		}
		protected override void UpdateLastAccessedTime(int storageKey, DateTime timestamp)
		{
			lock (store)
			{
                try
                {
                    string itemLocation = GenerateItemLocation(storageKey);
                    IsolatedStorageCacheItem storageItem =
                        new IsolatedStorageCacheItem(store, itemLocation, this.encryptionProvider);
                    storageItem.UpdateLastAccessedTime(timestamp);
                }
                catch (IOException)
                { 
                }
			}
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (null != store)
				{
					store.Dispose();
				}
			}
		}
		protected override Hashtable LoadDataFromStore()
		{
			lock (store)
			{
				Hashtable itemsLoadedFromStore = new Hashtable();
				string[] itemNames = store.GetDirectoryNames(GenerateSearchString(storageAreaName));
				foreach (string itemLocation in itemNames)
				{
					string itemName = GenerateItemLocation(itemLocation);
                    try
                    {
                        IsolatedStorageCacheItem loadedItem =
                            new IsolatedStorageCacheItem(store, itemName, this.encryptionProvider);
                        CacheItem itemLoadedFromStore = loadedItem.Load();
                        itemsLoadedFromStore.Add(itemLoadedFromStore.Key, itemLoadedFromStore);
                    }
                    catch (IOException)
                    {
                    }
				}
				return itemsLoadedFromStore;
			}
		}
		protected override void RemoveOldItem(int storageKey)
		{
			Remove(storageKey);
		}
		protected override void AddNewItem(int storageKey, CacheItem newItem)
		{
            lock (store)
            {
                string storageLocation = GenerateItemLocation(storageKey);
                IsolatedStorageCacheItem cacheItem =
                    new IsolatedStorageCacheItem(store, storageLocation, this.encryptionProvider);
                cacheItem.Store(newItem);
            }
        }
		private bool ItemExists(int possibleItem)
		{
			string itemLocation = GenerateItemLocation(possibleItem);
			string[] d = store.GetDirectoryNames(itemLocation);
			return d.Length == 1;
		}
		private void RemoveItem(string itemLocation)
		{
			string[] files = store.GetFileNames(GenerateSearchString(itemLocation));
			foreach (string fileName in files)
			{
				string fileToDelete = Path.Combine(itemLocation, fileName);
				store.DeleteFile(fileToDelete);
			}
			store.DeleteDirectory(itemLocation);
		}
		private int GetSize()
		{
			string[] itemsInIsolatedStorage = store.GetDirectoryNames(GenerateSearchString(storageAreaName));
			return itemsInIsolatedStorage.Length;
		}
		private static string GenerateSearchString(string searchRoot)
		{
			return Path.Combine(searchRoot, "*");
		}
		private string GenerateItemLocation(string itemToLocate)
		{
			return Path.Combine(storageAreaName, itemToLocate);
		}
		private string GenerateItemLocation(int storageKey)
		{
			return GenerateItemLocation(storageKey.ToString(NumberFormatInfo.InvariantInfo));
		}
		private void Initialize()
		{
			store = IsolatedStorageFile.GetUserStoreForDomain();
			if (store.GetDirectoryNames(storageAreaName).Length == 0)
			{
				store.CreateDirectory(storageAreaName);
			}
		}
	}
}
