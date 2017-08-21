/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
	[CustomFactory(typeof(BackingStoreCustomFactory))]
	public interface IBackingStore : IDisposable
	{
		int Count { get; }
		void Add(CacheItem newCacheItem);
		void Remove(string key);
		void UpdateLastAccessedTime(string key, DateTime timestamp);
		void Flush();
		Hashtable Load();
	}
}
