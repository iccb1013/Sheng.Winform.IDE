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
using System.Reflection;
namespace Sheng.SailingEase.Kernal
{
   
    public abstract class FastReflectionPool<TKeyType, TAccessor> : IFastReflectionPool<TKeyType,TAccessor>
    {
        private object _mutex = new object();
        private Dictionary<Type, Dictionary<TKeyType, TAccessor>> _cache =
            new Dictionary<Type, Dictionary<TKeyType, TAccessor>>();
        private bool _customCompare = false;
        public bool CustomCompare { get { return _customCompare; } set { _customCompare = value; } }
        protected virtual bool Compare(TKeyType key1, TKeyType key2)
        {
            return false;
        }
        public TAccessor Get(Type type, TKeyType key)
        {
            TAccessor accessor;
            Dictionary<TKeyType, TAccessor> accessorCache;
            if (this._cache.TryGetValue(type, out accessorCache))
            {
                TKeyType accessorKey;
                if (_customCompare)
                {
                    accessorKey = accessorCache.Keys.Single((k) => { return Compare(k, key); });
                }
                else
                {
                    accessorKey = key;
                }
                if (accessorCache.TryGetValue(key, out accessor))
                {
                    return accessor;
                }
            }
            lock (_mutex)
            {
                if (this._cache.ContainsKey(type) == false)
                {
                    this._cache[type] = new Dictionary<TKeyType, TAccessor>();
                }
                accessor = Create(type, key);
                this._cache[type][key] = accessor;
                return accessor;
            }
        }
        protected abstract TAccessor Create(Type type, TKeyType key);
    }
}
