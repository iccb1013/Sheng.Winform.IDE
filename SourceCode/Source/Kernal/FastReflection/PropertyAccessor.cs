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
using System.Linq.Expressions;
using System.Reflection;
namespace Sheng.SailingEase.Kernal
{
    public interface IPropertyAccessor
    {
        object GetValue(object instance);
        void SetValue(object instance, object value);
    }
    public class PropertyAccessor : IPropertyAccessor
    {
        private Func<object, object> m_getter;
        private MethodInvoker m_setMethodInvoker;
        public PropertyInfo PropertyInfo { get; private set; }
        public PropertyAccessor(PropertyInfo propertyInfo)
        {
            this.PropertyInfo = propertyInfo;
            this.InitializeGet(propertyInfo);
            this.InitializeSet(propertyInfo);
        }
        private void InitializeGet(PropertyInfo propertyInfo)
        {
            if (!propertyInfo.CanRead) return;
            var instance = Expression.Parameter(typeof(object), "instance");
            var instanceCast = propertyInfo.GetGetMethod(true).IsStatic ? null :
                Expression.Convert(instance, propertyInfo.ReflectedType);
            var propertyAccess = Expression.Property(instanceCast, propertyInfo);
            var castPropertyValue = Expression.Convert(propertyAccess, typeof(object));
            var lambda = Expression.Lambda<Func<object, object>>(castPropertyValue, instance);
            this.m_getter = lambda.Compile();
        }
        private void InitializeSet(PropertyInfo propertyInfo)
        {
            if (!propertyInfo.CanWrite) return;
            this.m_setMethodInvoker = new MethodInvoker(propertyInfo.GetSetMethod(true));
        }
        public object GetValue(object o)
        {
            if (this.m_getter == null)
            {
                throw new NotSupportedException("Get method is not defined for this property.");
            }
            return this.m_getter(o);
        }
        public void SetValue(object o, object value)
        {
            if (this.m_setMethodInvoker == null)
            {
                throw new NotSupportedException("Set method is not defined for this property.");
            }
            this.m_setMethodInvoker.Invoke(o, new object[] { value });
        }
        object IPropertyAccessor.GetValue(object instance)
        {
            return this.GetValue(instance);
        }
        void IPropertyAccessor.SetValue(object instance, object value)
        {
            this.SetValue(instance, value);
        }
    }
}
