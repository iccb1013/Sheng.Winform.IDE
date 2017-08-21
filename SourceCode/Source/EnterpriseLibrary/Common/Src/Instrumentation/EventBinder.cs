/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
    public class EventBinder
    {
        object source;
        object listener;
        public EventBinder(object source, object listener)
        {
            this.source = source;
            this.listener = listener;
        }
        public virtual void Bind(EventInfo sourceEvent, MethodInfo listenerMethod)
        {
            Delegate methodToBindTo =
                Delegate.CreateDelegate(sourceEvent.EventHandlerType, listener, listenerMethod);
            sourceEvent.AddEventHandler(source, methodToBindTo);            
        }
    }
}
