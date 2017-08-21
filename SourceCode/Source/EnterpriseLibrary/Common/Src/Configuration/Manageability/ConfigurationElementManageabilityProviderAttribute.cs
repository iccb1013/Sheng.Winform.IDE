/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public sealed class ConfigurationElementManageabilityProviderAttribute : Attribute
	{
		private Type manageabilityProviderType;
		private Type targetType;
		private Type sectionManageabilityProviderType;
		public ConfigurationElementManageabilityProviderAttribute(Type manageabilityProviderType, Type targetType, Type sectionManageabilityProviderType)
		{
			this.manageabilityProviderType = manageabilityProviderType;
			this.targetType = targetType;
			this.sectionManageabilityProviderType = sectionManageabilityProviderType;
		}
		public Type ManageabilityProviderType
		{
			get { return manageabilityProviderType; }
		}
		public Type TargetType
		{
			get { return targetType; }
		}
		public Type SectionManageabilityProviderType
		{
			get { return sectionManageabilityProviderType; }
		}
	}
}
