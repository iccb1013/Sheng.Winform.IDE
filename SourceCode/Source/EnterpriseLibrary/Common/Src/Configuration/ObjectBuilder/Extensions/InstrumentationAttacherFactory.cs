/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
    public class InstrumentationAttacherFactory
    {
        public IInstrumentationAttacher CreateBinder(object createdObject,
                                                     object[] constructorArgs,
                                                     ConfigurationReflectionCache reflectionCache)
        {
            InstrumentationListenerAttribute listenerAttribute = GetInstrumentationListenerAttribute(createdObject, reflectionCache);
            if (listenerAttribute == null) return new NoBindingInstrumentationAttacher();
            Type listenerType = listenerAttribute.ListenerType;
            Type listenerBinderType = listenerAttribute.ListenerBinderType;
            if (listenerBinderType == null) return new ReflectionInstrumentationAttacher(createdObject, listenerType, constructorArgs);
            return new ExplicitInstrumentationAttacher(createdObject, listenerType, constructorArgs, listenerBinderType);
        }
        static InstrumentationListenerAttribute GetInstrumentationListenerAttribute(object createdObject,
                                                                             ConfigurationReflectionCache reflectionCache)
        {
            Type createdObjectType = createdObject.GetType();
            InstrumentationListenerAttribute listenerAttribute
                = reflectionCache.GetCustomAttribute<InstrumentationListenerAttribute>(createdObjectType, true);
            return listenerAttribute;
        }
    }
}
