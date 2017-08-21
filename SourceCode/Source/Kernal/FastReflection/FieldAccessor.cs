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
using System.Linq.Expressions;
namespace Sheng.SailingEase.Kernal
{
    public interface IFieldAccessor
    {
        object GetValue(object instance);
    }
    public class FieldAccessor : IFieldAccessor
    {
        private Func<object, object> m_getter;
        public FieldInfo FieldInfo { get; private set; }
        public FieldAccessor(FieldInfo fieldInfo)
        {
            this.FieldInfo = fieldInfo;
        }
        private void InitializeGet(FieldInfo fieldInfo)
        {
            var instance = Expression.Parameter(typeof(object), "instance");
            var instanceCast = fieldInfo.IsStatic ? null :
                Expression.Convert(instance, fieldInfo.ReflectedType);
            var fieldAccess = Expression.Field(instanceCast, fieldInfo);
            var castFieldValue = Expression.Convert(fieldAccess, typeof(object));
            var lambda = Expression.Lambda<Func<object, object>>(castFieldValue, instance);
            this.m_getter = lambda.Compile();
        }
        public object GetValue(object instance)
        {
            return this.m_getter(instance);
        }
        object IFieldAccessor.GetValue(object instance)
        {
            return this.GetValue(instance);
        }
    }
}
