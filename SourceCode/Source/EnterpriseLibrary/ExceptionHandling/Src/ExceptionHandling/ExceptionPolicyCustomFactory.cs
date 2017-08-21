/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.ObjectBuilder2;
using System.Collections.Generic;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
	public class ExceptionPolicyCustomFactory : ICustomFactory
	{
        public object CreateObject(IBuilderContext context, string name, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			ExceptionPolicyData objectConfiguration = GetConfiguration(name, configurationSource);
			Dictionary<Type, ExceptionPolicyEntry> policyEntries = new Dictionary<Type,ExceptionPolicyEntry>();
			foreach (ExceptionTypeData exceptionTypeData in objectConfiguration.ExceptionTypes)
			{
				ExceptionPolicyEntry entry
					= ExceptionPolicyEntryCustomFactory.Instance.Create(context, exceptionTypeData, configurationSource, reflectionCache);
				policyEntries.Add(exceptionTypeData.Type, entry);
			}
			ExceptionPolicyImpl createdObject
				= new ExceptionPolicyImpl(
					objectConfiguration.Name,
					policyEntries);
			return createdObject;
		}
        private ExceptionPolicyData GetConfiguration(string id, IConfigurationSource configurationSource)
		{
			return new ExceptionHandlingConfigurationView(configurationSource).GetExceptionPolicyData(id);
		}
	}
}
