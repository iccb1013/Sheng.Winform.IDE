/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Configuration
{
	public class ManageableConfigurationSourceElement : ConfigurationSourceElement
	{
		private const String filePathPropertyName = "filePath";
		private const String applicationNamePropertyName = "applicationName";
		private const String enableWmiPropertyName = "enableWmi";
		private const String enableGroupPoliciesPropertyName = "enableGroupPolicies";
		private const String manageabilityProvidersCollectionPropertyName = "manageabilityProviders";
		public const int MinimumApplicationNameLength = 1;
		public const int MaximumApplicationNameLength = 255;		
		public ManageableConfigurationSourceElement()
			: base(Resources.ManageableConfigurationSourceName, typeof(ManageableConfigurationSource))
		{ }
		public ManageableConfigurationSourceElement(String name, String filePath, String applicationName)
			: this(name, filePath, applicationName, true, true)
		{ }
		public ManageableConfigurationSourceElement(String name, String filePath, String applicationName,
			Boolean enableGroupPolicies, Boolean enableWmi)
			: base(name, typeof(ManageableConfigurationSource))
		{
			this.FilePath = filePath;
			this.ApplicationName = applicationName;
			this.EnableGroupPolicies = enableGroupPolicies;
			this.EnableWmi = enableWmi;
		}
		[ConfigurationProperty(filePathPropertyName, IsRequired = true)]
		public String FilePath
		{
			get { return (String)this[filePathPropertyName]; }
			set { this[filePathPropertyName] = value; }
		}
		[ConfigurationProperty(applicationNamePropertyName, IsRequired = true, DefaultValue = "Application")]
		[StringValidator(MinLength = MinimumApplicationNameLength, MaxLength = MaximumApplicationNameLength)]
		public String ApplicationName
		{
			get { return (String)this[applicationNamePropertyName]; }
			set { this[applicationNamePropertyName] = value; }
		}
		[ConfigurationProperty(enableWmiPropertyName, DefaultValue = true)]
		public bool EnableWmi
		{
			get { return (bool)this[enableWmiPropertyName]; }
			set { this[enableWmiPropertyName] = value; }
		}
		[ConfigurationProperty(enableGroupPoliciesPropertyName, DefaultValue = true)]
		public bool EnableGroupPolicies
		{
			get { return (bool)this[enableGroupPoliciesPropertyName]; }
			set { this[enableGroupPoliciesPropertyName] = value; }
		}
		[ConfigurationProperty(manageabilityProvidersCollectionPropertyName)]
		public NamedElementCollection<ConfigurationSectionManageabilityProviderData> ConfigurationManageabilityProviders
		{
			get
			{
				return (NamedElementCollection<ConfigurationSectionManageabilityProviderData>)this[manageabilityProvidersCollectionPropertyName];
			}
		}
		public override IConfigurationSource CreateSource()
		{
			IDictionary<String, ConfigurationSectionManageabilityProvider> manageabilityProviders = new Dictionary<String, ConfigurationSectionManageabilityProvider>(this.ConfigurationManageabilityProviders.Count);
			ManageabilityProviderBuilder providerBuilder = new ManageabilityProviderBuilder();
			foreach (ConfigurationSectionManageabilityProviderData data in this.ConfigurationManageabilityProviders)
			{
				ConfigurationSectionManageabilityProvider provider
					= providerBuilder.CreateConfigurationSectionManageabilityProvider(data);
				manageabilityProviders.Add(data.Name, provider);
			}
			return new ManageableConfigurationSource(this.FilePath, manageabilityProviders, this.EnableGroupPolicies, this.EnableWmi, this.ApplicationName);
		}
	}
}
