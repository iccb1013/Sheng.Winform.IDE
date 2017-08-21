/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	public abstract class AssemblerBasedCustomFactory<TObject, TConfiguration> : AssemblerBasedObjectFactory<TObject, TConfiguration>, ICustomFactory
		where TObject : class
		where TConfiguration : class
	{
		public TObject Create(IBuilderContext context, string name, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "name");
			}
			TConfiguration objectConfiguration = GetConfiguration(name, configurationSource);
			if (objectConfiguration == null)
			{
				throw new ConfigurationErrorsException(
					string.Format(
						Resources.Culture,
						Resources.ExceptionNamedConfigurationNotFound,
						name,
						GetType().FullName));
			}
			TObject createdObject = Create(context, objectConfiguration, configurationSource, reflectionCache);
			return createdObject;
		}
		public object CreateObject(IBuilderContext context, string name, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			return Create(context, name, configurationSource, reflectionCache);
		}
		protected abstract TConfiguration GetConfiguration(string name, IConfigurationSource configurationSource);
	}
}
