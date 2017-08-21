/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
	[InstrumentationListener(typeof(CachingInstrumentationListener))]
	public class CachingInstrumentationProvider
	{
		[InstrumentationProvider("CacheUpdated")]
		public event EventHandler<CacheUpdatedEventArgs> cacheUpdated;
		[InstrumentationProvider("CacheAccessed")]
		public event EventHandler<CacheAccessedEventArgs> cacheAccessed;
		[InstrumentationProvider("CacheExpired")]
		public event EventHandler<CacheExpiredEventArgs> cacheExpired;
		[InstrumentationProvider("CacheScavenged")]
		public event EventHandler<CacheScavengedEventArgs> cacheScavenged;
		[InstrumentationProvider("CacheCallbackFailed")]
		public event EventHandler<CacheCallbackFailureEventArgs> cacheCallbackFailed;
		[InstrumentationProvider("CacheFailed")]
		public event EventHandler<CacheFailureEventArgs> cacheFailed;
		public void FireCacheUpdated(long updatedEntriesCount, long totalEntriesCount)
		{
			if (cacheUpdated != null) cacheUpdated(this, new CacheUpdatedEventArgs(updatedEntriesCount, totalEntriesCount));
		}
		public void FireCacheAccessed(string key, bool hit)
		{
			if (cacheAccessed != null) cacheAccessed(this, new CacheAccessedEventArgs(key, hit));
		}
		public void FireCacheExpired(long itemsExpired)
		{
			if (cacheExpired != null) cacheExpired(this, new CacheExpiredEventArgs(itemsExpired));
		}
		public void FireCacheScavenged(long itemsScavenged)
		{
			if (cacheScavenged != null) cacheScavenged(this, new CacheScavengedEventArgs(itemsScavenged));
		}
		public void FireCacheCallbackFailed(string key, Exception exception)
		{
			if (cacheCallbackFailed != null) cacheCallbackFailed(this, new CacheCallbackFailureEventArgs(key, exception));
		}
		public void FireCacheFailed(string errorMessage, Exception exception)
		{
			if (cacheFailed != null) cacheFailed(this, new CacheFailureEventArgs(errorMessage, exception));
		}
	}
}
