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
namespace Sheng.SailingEase.Kernal.FastReflection
{
    public static class FastReflectionExtensions
    {
        public static object FastInvoke(this MethodInfo methodInfo, object instance, params object[] parameters)
        {
            return FastReflectionCaches.MethodInvokerCache.Get(methodInfo).Invoke(instance, parameters);
        }
        public static void FastSetValue(this PropertyInfo propertyInfo, object instance, object value)
        {
            FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo).SetValue(instance, value);
        }
        public static object FastGetValue(this PropertyInfo propertyInfo, object instance)
        {
            return FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo).GetValue(instance);
        }
        public static object FastGetValue(this FieldInfo fieldInfo, object instance)
        {
            return FastReflectionCaches.FieldAccessorCache.Get(fieldInfo).GetValue(instance);
        }
        public static object FastInvoke(this ConstructorInfo constructorInfo, params object[] parameters)
        {
            return FastReflectionCaches.ConstructorInvokerCache.Get(constructorInfo).Invoke(parameters);
        }
    }
}
