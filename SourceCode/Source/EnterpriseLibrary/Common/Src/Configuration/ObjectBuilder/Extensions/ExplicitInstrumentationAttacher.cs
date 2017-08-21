/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	public class ExplicitInstrumentationAttacher : IInstrumentationAttacher
	{
		object source;
		Type listenerType;
		object[] listenerConstructorArguments;
		Type explicitBinderType;
		public ExplicitInstrumentationAttacher(object source, Type listenerType, object [] listenerConstructorArguments, Type explicitBinderType)
		{
			this.source = source;
			this.listenerType = listenerType;
			this.listenerConstructorArguments = listenerConstructorArguments;
			this.explicitBinderType = explicitBinderType;
		}
		public void BindInstrumentation()
		{
			IExplicitInstrumentationBinder binder = (IExplicitInstrumentationBinder) Activator.CreateInstance(explicitBinderType);
			object listener = Activator.CreateInstance(listenerType, listenerConstructorArguments);
			binder.Bind(source, listener);
		}
	}
}
