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
	public sealed class ConfigurationSectionManageabilityProviderAttribute : Attribute
	{
		private string sectionName;
		private Type manageabilityProviderType;
		public ConfigurationSectionManageabilityProviderAttribute(string sectionName, Type manageabilityProviderType)
		{
			this.sectionName = sectionName;
			this.manageabilityProviderType = manageabilityProviderType;
		}
		public string SectionName
		{
			get { return sectionName; }
		}
		public Type ManageabilityProviderType
		{
			get { return manageabilityProviderType; }
		}
	}
}
