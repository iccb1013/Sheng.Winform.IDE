/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
    [AttributeUsage(AttributeTargets.Class, Inherited=true)]
	public sealed class InstrumentationListenerAttribute : Attribute
    {
        Type listenerType;
		Type listenerBinderType;
        public Type ListenerType
        {
            get { return listenerType; }
        }
		public Type ListenerBinderType
		{
			get { return listenerBinderType; }
		}
        public InstrumentationListenerAttribute(Type listenerType)
        {
            this.listenerType = listenerType;  
			this.listenerBinderType = null;
        }
		public InstrumentationListenerAttribute(Type listenerType, Type listenerBinderType)
		{
			this.listenerType = listenerType;
			this.listenerBinderType = listenerBinderType;
		}
    }
}
