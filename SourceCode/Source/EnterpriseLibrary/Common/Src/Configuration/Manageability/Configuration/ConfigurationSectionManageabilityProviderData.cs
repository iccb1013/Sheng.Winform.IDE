/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Configuration
{
	public class ConfigurationSectionManageabilityProviderData : NameTypeConfigurationElement
	{
		private const String manageabilityProvidersCollectionPropertyName = "manageabilityProviders";
		public ConfigurationSectionManageabilityProviderData()
		{ }
		public ConfigurationSectionManageabilityProviderData(String sectionName, Type providerType)
			: base(sectionName, providerType)
		{ }
		[ConfigurationProperty(manageabilityProvidersCollectionPropertyName)]
		public NamedElementCollection<ConfigurationElementManageabilityProviderData> ManageabilityProviders
		{
			get { return (NamedElementCollection<ConfigurationElementManageabilityProviderData>)base[manageabilityProvidersCollectionPropertyName]; }
		}
		internal ConfigurationSectionManageabilityProvider CreateManageabilityProvider()
		{
			IDictionary<Type, ConfigurationElementManageabilityProvider> subProviders = new Dictionary<Type, ConfigurationElementManageabilityProvider>();
			foreach (ConfigurationElementManageabilityProviderData data in this.ManageabilityProviders)
			{
				ConfigurationElementManageabilityProvider subManageabilityProvider = data.CreateManageabilityProvider();
				subProviders.Add(data.TargetType, subManageabilityProvider);
			}
			return (ConfigurationSectionManageabilityProvider)Activator.CreateInstance(this.Type, subProviders);
		}
	}
}
