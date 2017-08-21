/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations
{
	public class IsolatedStorageCacheItem
	{
		private IsolatedStorageCacheItemField keyField;
		private IsolatedStorageCacheItemField valueField;
		private IsolatedStorageCacheItemField scavengingPriorityField;
		private IsolatedStorageCacheItemField refreshActionField;
		private IsolatedStorageCacheItemField expirationsField;
		private IsolatedStorageCacheItemField lastAccessedField;
		private const int MaxRetries = 3;
		private const int RetryDelayInMilliseconds = 50;
		public IsolatedStorageCacheItem(IsolatedStorageFile storage, string itemDirectoryRoot, IStorageEncryptionProvider encryptionProvider)
		{
			int retriesLeft = MaxRetries;
			while (true)
			{
				try
				{
					storage.CreateDirectory(itemDirectoryRoot);
					using (IsolatedStorageFileStream fileStream =
						new IsolatedStorageFileStream(itemDirectoryRoot + @"\sanity-check.txt", FileMode.Create, FileAccess.Write, FileShare.None, storage))
					{ }
					break;
				}
				catch (UnauthorizedAccessException)
				{
					if (retriesLeft-- > 0)
					{
						Thread.Sleep(RetryDelayInMilliseconds);
						continue;
					}
					throw;
				}
				catch (DirectoryNotFoundException)
				{
					if (retriesLeft-- > 0)
					{
						Thread.Sleep(RetryDelayInMilliseconds);
						continue;
					}
					throw;
				}
			}
			keyField = new IsolatedStorageCacheItemField(storage, "Key", itemDirectoryRoot, encryptionProvider);
			valueField = new IsolatedStorageCacheItemField(storage, "Val", itemDirectoryRoot, encryptionProvider);
			scavengingPriorityField = new IsolatedStorageCacheItemField(storage, "ScPr", itemDirectoryRoot, encryptionProvider);
			refreshActionField = new IsolatedStorageCacheItemField(storage, "RA", itemDirectoryRoot, encryptionProvider);
			expirationsField = new IsolatedStorageCacheItemField(storage, "Exp", itemDirectoryRoot, encryptionProvider);
			lastAccessedField = new IsolatedStorageCacheItemField(storage, "LA", itemDirectoryRoot, encryptionProvider);
		}
		public void Store(CacheItem itemToStore)
		{
			keyField.Write(itemToStore.Key, false);
			valueField.Write(itemToStore.Value, true);
			scavengingPriorityField.Write(itemToStore.ScavengingPriority, false);
			refreshActionField.Write(itemToStore.RefreshAction, false);
			expirationsField.Write(itemToStore.GetExpirations(), false);
			lastAccessedField.Write(itemToStore.LastAccessedTime, false);
		}
		public CacheItem Load()
		{
			string key = (string)keyField.Read(false);
			object value = valueField.Read(true);
			CacheItemPriority scavengingPriority = (CacheItemPriority)scavengingPriorityField.Read(false);
			ICacheItemRefreshAction refreshAction = (ICacheItemRefreshAction)refreshActionField.Read(false);
			ICacheItemExpiration[] expirations = (ICacheItemExpiration[])expirationsField.Read(false);
			DateTime lastAccessedTime = (DateTime)lastAccessedField.Read(false);
			return new CacheItem(lastAccessedTime, key, value, scavengingPriority, refreshAction, expirations);
		}
		public void UpdateLastAccessedTime(DateTime newTimestamp)
		{
			lastAccessedField.Overwrite(newTimestamp);
		}
	}
}
