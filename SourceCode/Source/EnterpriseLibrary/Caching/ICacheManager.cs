/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
	[ConfigurationNameMapper(typeof(CacheManagerDataRetriever))]
	[CustomFactory(typeof(CacheManagerCustomFactory))]
	public interface ICacheManager
	{
		void Add(string key, object value);
		void Add(string key, object value, CacheItemPriority scavengingPriority, ICacheItemRefreshAction refreshAction, params ICacheItemExpiration[] expirations);
		bool Contains(string key);
		int Count { get; }
		void Flush();
		object GetData(string key);
		void Remove(string key);
		object this[string key] { get; }
	}
}
