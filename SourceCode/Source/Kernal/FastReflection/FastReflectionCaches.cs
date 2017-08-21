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
    public static class FastReflectionCaches
    {
        static FastReflectionCaches()
        {
            MethodInvokerCache = new MethodInvokerCache();
            PropertyAccessorCache = new PropertyAccessorCache();
            FieldAccessorCache = new FieldAccessorCache();
            ConstructorInvokerCache = new ConstructorInvokerCache();
        }
        public static IFastReflectionCache<MethodInfo, IMethodInvoker> MethodInvokerCache { get; set; }
        public static IFastReflectionCache<PropertyInfo, IPropertyAccessor> PropertyAccessorCache { get; set; }
        public static IFastReflectionCache<FieldInfo, IFieldAccessor> FieldAccessorCache { get; set; }
        public static IFastReflectionCache<ConstructorInfo, IConstructorInvoker> ConstructorInvokerCache { get; set; }
    }
}
