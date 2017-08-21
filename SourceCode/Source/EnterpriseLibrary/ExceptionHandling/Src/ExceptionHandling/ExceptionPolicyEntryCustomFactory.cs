/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
	public class ExceptionPolicyEntryCustomFactory
	{
		public static ExceptionPolicyEntryCustomFactory Instance = new ExceptionPolicyEntryCustomFactory();
		public ExceptionPolicyEntry Create(IBuilderContext context, ExceptionTypeData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			List<IExceptionHandler> handlers = new List<IExceptionHandler>();
			foreach (ExceptionHandlerData handlerData in objectConfiguration.ExceptionHandlers)
			{
				IExceptionHandler handler
					= ExceptionHandlerCustomFactory.Instance.Create(context, handlerData, configurationSource, reflectionCache);
				handlers.Add(handler);
			}
			ExceptionPolicyEntry createdObject
				= new ExceptionPolicyEntry(
					objectConfiguration.PostHandlingAction,
					handlers);
			return createdObject;
		}
	}
}
