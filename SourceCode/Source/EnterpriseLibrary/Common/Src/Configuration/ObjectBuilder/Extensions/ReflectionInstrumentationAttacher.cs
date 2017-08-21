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
	public class ReflectionInstrumentationAttacher : IInstrumentationAttacher
	{
		object source;
		Type listenerType;
		object[] listenerConstructorArgs;
		public ReflectionInstrumentationAttacher(object source, Type listenerType, object[] listenerConstructorArgs)
		{
			this.source = source;
			this.listenerType = listenerType;
			this.listenerConstructorArgs = listenerConstructorArgs;
		}
		public void BindInstrumentation()
		{
			object listener = CreateListener();
			BindSourceToListener(source, listener);
		}
		private object CreateListener()
		{
			return Activator.CreateInstance(listenerType, listenerConstructorArgs);
		}
		private void BindSourceToListener(object createdObject, object listener)
		{
			ReflectionInstrumentationBinder binder = new ReflectionInstrumentationBinder();
			binder.Bind(createdObject, listener);
		}
	}
}
