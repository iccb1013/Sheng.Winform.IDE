/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
	[ConfigurationElementType(typeof(CacheManagerData))]
	public class CacheManager : IDisposable, ICacheManager
	{
		private Cache realCache;
		private BackgroundScheduler scheduler;
		private ExpirationPollTimer pollTimer;
		internal CacheManager(Cache realCache, BackgroundScheduler scheduler, ExpirationPollTimer pollTimer)
		{
			this.realCache = realCache;
			this.scheduler = scheduler;
			this.pollTimer = pollTimer;
		}
		public int Count
		{
			get { return realCache.Count; }
		}
		public bool Contains(string key)
		{
			return realCache.Contains(key);
		}
		public object this[string key]
		{
			get { return realCache.GetData(key); }
		}
		public void Add(string key, object value)
		{
			Add(key, value, CacheItemPriority.Normal, null);
		}
		public void Add(string key, object value, CacheItemPriority scavengingPriority, ICacheItemRefreshAction refreshAction, params ICacheItemExpiration[] expirations)
		{
			realCache.Add(key, value, scavengingPriority, refreshAction, expirations);
		}
		public void Remove(string key)
		{
			realCache.Remove(key);
		}
		public object GetData(string key)
		{
			return realCache.GetData(key);
		}
		public void Flush()
		{
			realCache.Flush();
		}
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			if (scheduler != null)
			{
				scheduler.Stop();
				scheduler = null;
			}
			if (pollTimer != null)
			{
				pollTimer.StopPolling();
				pollTimer = null;
			}
			if (realCache != null)
			{
				realCache.Dispose();
				realCache = null;
			}
		}
	}
}
