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
    public static class FastReflectionFactories
    {
        static FastReflectionFactories()
        {
            MethodInvokerFactory = new MethodInvokerFactory();
            PropertyAccessorFactory = new PropertyAccessorFactory();
            FieldAccessorFactory = new FieldAccessorFactory();
            ConstructorInvokerFactory = new ConstructorInvokerFactory();
        }
        public static IFastReflectionFactory<MethodInfo, IMethodInvoker> MethodInvokerFactory { get; set; }
        public static IFastReflectionFactory<PropertyInfo, IPropertyAccessor> PropertyAccessorFactory { get; set; }
        public static IFastReflectionFactory<FieldInfo, IFieldAccessor> FieldAccessorFactory { get; set; }
        public static IFastReflectionFactory<ConstructorInfo, IConstructorInvoker> ConstructorInvokerFactory { get; set; }
    }
}
