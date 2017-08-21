/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	public class TypeInstantiationAssembler<TObject, TConfiguration> : IAssembler<TObject, TConfiguration>
		where TObject : class
		where TConfiguration : NameTypeConfigurationElement
	{
		public TObject Assemble(IBuilderContext context, TConfiguration objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			return (TObject)Activator.CreateInstance(objectConfiguration.Type);
		}
	}
}
