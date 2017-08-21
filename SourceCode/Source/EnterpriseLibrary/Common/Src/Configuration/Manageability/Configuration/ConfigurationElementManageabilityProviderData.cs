/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Configuration
{
	public class ConfigurationElementManageabilityProviderData : NameTypeConfigurationElement
	{
		private const string targetTypePropertyName = "targetType";
		public ConfigurationElementManageabilityProviderData()
		{ }
		public ConfigurationElementManageabilityProviderData(String name, Type providerType, Type targetType)
			: base(name, providerType)
		{
			this.TargetType = targetType;
		}
		[ConfigurationProperty(targetTypePropertyName, IsRequired = true)]
		[TypeConverter(typeof(AssemblyQualifiedTypeNameConverter))]
		public Type TargetType
		{
			get { return (Type)base[targetTypePropertyName]; }
			set { base[targetTypePropertyName] = value; }
		}
		internal ConfigurationElementManageabilityProvider CreateManageabilityProvider()
		{
			return (ConfigurationElementManageabilityProvider)Activator.CreateInstance(this.Type);
		}
	}
}
