/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sheng.SailingEase.Infrastructure;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.Composite.Events;
namespace Sheng.SailingEase.Shell
{
    class CachingService : ICachingService
    {
        private ICacheManager _primitivesCache;
        private IEventAggregator _eventAggregator = ServiceUnity.EventAggregator;
        public CachingService()
        {
            _primitivesCache = CacheFactory.GetCacheManager();
            _eventAggregator.GetEvent<ProjectPreCloseEvent>().Subscribe((e) =>
            {
                _primitivesCache.Flush();
            });
        }
        public void Add(string key, object value)
        {
            _primitivesCache.Add(key, value);
        }
        public void Remove(string key)
        {
            _primitivesCache.Remove(key);
        }
        public object GetData(string key)
        {
            return _primitivesCache.GetData(key);
        }
    }
}
