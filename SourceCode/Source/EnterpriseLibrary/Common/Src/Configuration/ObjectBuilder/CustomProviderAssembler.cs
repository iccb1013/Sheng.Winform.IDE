/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	public class CustomProviderAssembler<TObject, TConfiguration, TConcreteConfiguration> : IAssembler<TObject, TConfiguration>
		where TObject : class
		where TConfiguration : class, IObjectWithNameAndType
		where TConcreteConfiguration : class, TConfiguration, ICustomProviderData
	{
		public TObject Assemble(IBuilderContext context, TConfiguration objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			TConcreteConfiguration castedObjectConfiguration = (TConcreteConfiguration)objectConfiguration;
			TObject provider
				= (TObject)Activator.CreateInstance(objectConfiguration.Type, castedObjectConfiguration.Attributes);
			return provider;
		}
	}
}
