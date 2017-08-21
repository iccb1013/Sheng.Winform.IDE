/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
	public sealed class ConfigurationNameMapperAttribute : Attribute
	{
		public Type NameMappingObjectType;
		public ConfigurationNameMapperAttribute(Type nameMappingObjectType)
		{
			if (nameMappingObjectType == null) 
				throw new ArgumentNullException("nameMappingObjectType");
			if (!typeof(IConfigurationNameMapper).IsAssignableFrom(nameMappingObjectType))
				throw new ArgumentException(string.Format(Resources.Culture, Resources.ExceptionTypeNotNameMapper, nameMappingObjectType), "nameMappingObjectType");
			NameMappingObjectType = nameMappingObjectType;
		}
	}
}
