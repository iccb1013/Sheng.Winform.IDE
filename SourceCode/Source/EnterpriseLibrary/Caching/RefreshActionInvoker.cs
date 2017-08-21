/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    public static class RefreshActionInvoker
    {
		public static void InvokeRefreshAction(CacheItem removedCacheItem, CacheItemRemovedReason removalReason, CachingInstrumentationProvider instrumentationProvider)
        {
            if (removedCacheItem.RefreshAction == null)
            {
                return;
            }
			try
			{
                RefreshActionData refreshActionData =
                    new RefreshActionData(removedCacheItem.RefreshAction, removedCacheItem.Key, removedCacheItem.Value, removalReason, instrumentationProvider);
                refreshActionData.InvokeOnThreadPoolThread();
			}
			catch (Exception e)
			{
				instrumentationProvider.FireCacheFailed(Resources.FailureToSpawnUserSpecifiedRefreshAction, e);
			}            
        }
        private class RefreshActionData
        {
            private ICacheItemRefreshAction refreshAction;
            private string keyToRefresh;
            private object removedData;
            private CacheItemRemovedReason removalReason;
			private CachingInstrumentationProvider instrumentationProvider;
			public RefreshActionData(ICacheItemRefreshAction refreshAction, string keyToRefresh, object removedData, CacheItemRemovedReason removalReason, CachingInstrumentationProvider instrumentationProvider)
            {
                this.refreshAction = refreshAction;
                this.keyToRefresh = keyToRefresh;
                this.removalReason = removalReason;
                this.removedData = removedData;
				this.instrumentationProvider = instrumentationProvider;
            }
            public ICacheItemRefreshAction RefreshAction
            {
                get { return refreshAction; }
            }
            public string KeyToRefresh
            {
                get { return keyToRefresh; }
            }
            public CacheItemRemovedReason RemovalReason
            {
                get { return removalReason; }
            }
            public object RemovedData
            {
                get { return removedData; }
            }
			public CachingInstrumentationProvider InstrumentationProvider
			{
				get { return instrumentationProvider; }
			}
            public void InvokeOnThreadPoolThread()
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadPoolRefreshActionInvoker));
            }
            private void ThreadPoolRefreshActionInvoker(object notUsed)
            {
				try
				{
                    RefreshAction.Refresh(KeyToRefresh, RemovedData, RemovalReason);
				}
				catch (Exception e)
				{
					InstrumentationProvider.FireCacheCallbackFailed(KeyToRefresh, e);
				}
            }
        }
    }
}
