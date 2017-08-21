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
using Sheng.SailingEase.Kernal.FastReflection;
using System.Diagnostics;
namespace Sheng.SailingEase.Kernal
{
    public class PropertyAccessorPool
    {
        private object _mutex = new object();
        private Dictionary<Type, Dictionary<string, IPropertyAccessor>> _cache =
            new Dictionary<Type, Dictionary<string, IPropertyAccessor>>();
        private IPropertyAccessor GetAccessor(Type type, string propertyName)
        {
            if (type == null || String.IsNullOrEmpty(propertyName))
            {
                Debug.Assert(false, "type 或 propertyName 为空");
                throw new ArgumentNullException();
            }
            IPropertyAccessor accessor;
            Dictionary<string, IPropertyAccessor> typeCache;
            if (this._cache.TryGetValue(type, out typeCache))
            {
                if (typeCache.TryGetValue(propertyName, out accessor))
                {
                    return accessor;
                }
            }
            lock (_mutex)
            {
                if (this._cache.ContainsKey(type) == false)
                {
                    this._cache[type] = new Dictionary<string, IPropertyAccessor>();
                }
                var propertyInfo = type.GetProperty(propertyName);
                if (propertyInfo == null)
                {
                    Debug.Assert(false, propertyName + "不存在");
                    throw new MissingMemberException(propertyName);
                }
                accessor = new PropertyAccessor(propertyInfo);
                this._cache[type][propertyName] = accessor;
                return accessor;
            }
        }
        public object GetValue(object obj, string propertyName)
        {
            return GetAccessor(obj.GetType(), propertyName).GetValue(obj);
        }
        public T GetValue<T>(object obj, string propertyName)
        {
            object value = GetValue(obj, propertyName);
            if (value is T)
            {
                return (T)value;
            }
            else
            {
                Debug.Assert(false, "propertyName 的值 value 不是类型 " + typeof(T).ToString());
                throw new InvalidCastException();
            }
        }
    }
}
