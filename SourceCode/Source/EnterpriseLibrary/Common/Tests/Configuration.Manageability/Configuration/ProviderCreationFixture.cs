/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Configuration
{
	[TestClass]
	public class ProviderCreationFixture
	{
		[TestMethod]
		public void CanCreateSectionProviderWithNoSubProviders()
		{
			ConfigurationSectionManageabilityProviderData section1Data
				= new ConfigurationSectionManageabilityProviderData("section1", typeof(TestConfigurationSectionManageabilityProviderWithValidWmiImplementations));
			TestConfigurationSectionManageabilityProviderWithValidWmiImplementations provider
			   = (TestConfigurationSectionManageabilityProviderWithValidWmiImplementations)new ManageabilityProviderBuilder().CreateConfigurationSectionManageabilityProvider(section1Data);
			Assert.IsNotNull(provider);
			Assert.AreEqual(0, provider.Providers.Count);
		}
		[TestMethod]
		public void CanCreateSectionProviderWithSubProviders()
		{
			ConfigurationSectionManageabilityProviderData section1Data
				= new ConfigurationSectionManageabilityProviderData("section1", typeof(TestConfigurationSectionManageabilityProviderWithValidWmiImplementations));
			section1Data.ManageabilityProviders.Add(new ConfigurationElementManageabilityProviderData("2", typeof(TestConfigurationElementManageabilityProviderWithValidWmiImplementations), typeof(Boolean)));
			section1Data.ManageabilityProviders.Add(new ConfigurationElementManageabilityProviderData("3", typeof(TestConfigurationElementManageabilityProviderWithValidWmiImplementations2), typeof(Int32)));
			TestConfigurationSectionManageabilityProviderWithValidWmiImplementations provider
			   = new ManageabilityProviderBuilder().CreateConfigurationSectionManageabilityProvider(section1Data)
					as TestConfigurationSectionManageabilityProviderWithValidWmiImplementations;
			Assert.IsNotNull(provider);
			Assert.AreEqual(2, provider.Providers.Count);
			TestConfigurationElementManageabilityProviderWithValidWmiImplementations subProvider1
				= provider.Providers[typeof(bool)] as TestConfigurationElementManageabilityProviderWithValidWmiImplementations;
			Assert.IsNotNull(subProvider1);
			TestConfigurationElementManageabilityProviderWithValidWmiImplementations2 subProvider2
				= provider.Providers[typeof(int)] as TestConfigurationElementManageabilityProviderWithValidWmiImplementations2;
			Assert.IsNotNull(subProvider2);
		}
	}
	public class TestConfigurationSectionManageabilityProviderWithValidWmiImplementations
		: ConfigurationSectionManageabilityProviderBase<TestConfigurationSection>
	{
		public TestConfigurationSectionManageabilityProviderWithValidWmiImplementations(IDictionary<Type, ConfigurationElementManageabilityProvider> subProviders)
			: base(subProviders)
		{ }
		protected override void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder,
			TestConfigurationSection configurationSection,
			IConfigurationSource configurationSource,
			string sectionKey)
		{
			throw new Exception("The method or operation is not implemented.");
		}
		protected override string SectionCategoryName
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}
		protected override string SectionName
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}
		protected override void OverrideWithGroupPoliciesForConfigurationSection(TestConfigurationSection configurationSection,
			IRegistryKey policyKey)
		{
			throw new Exception("The method or operation is not implemented.");
		}
		protected override void OverrideWithGroupPoliciesAndGenerateWmiObjectsForConfigurationElements(TestConfigurationSection configurationSection,
			bool readGroupPolicies,
			IRegistryKey machineKey,
			IRegistryKey userKey,
			bool generateWmiObjects,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			throw new Exception("The method or operation is not implemented.");
		}
		protected override void GenerateWmiObjectsForConfigurationSection(TestConfigurationSection configurationSection, ICollection<ConfigurationSetting> wmiSettings)
		{
			throw new NotImplementedException();
		}
	}
	public class TestConfigurationElementManageabilityProviderWithValidWmiImplementations
		: ConfigurationElementManageabilityProviderBase<TestConfigurationElement>
	{
		public TestConfigurationElementManageabilityProviderWithValidWmiImplementations()
		{ }
		protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder, TestConfigurationElement configurationObject, IConfigurationSource configurationSource, string elementPolicyKeyName)
		{
			throw new Exception("The method or operation is not implemented.");
		}
		protected override string ElementPolicyNameTemplate
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}
		protected override void OverrideWithGroupPolicies(TestConfigurationElement configurationObject, IRegistryKey policyKey)
		{
			throw new Exception("The method or operation is not implemented.");
		}
		protected override void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder, TestConfigurationElement configurationObject, IConfigurationSource configurationSource, string elementPolicyKeyName)
		{
		}
		protected override void GenerateWmiObjects(TestConfigurationElement configurationObject, ICollection<ConfigurationSetting> wmiSettings)
		{
		}
	}
	public class TestConfigurationElementManageabilityProviderWithValidWmiImplementations2
		: TestConfigurationElementManageabilityProviderWithValidWmiImplementations
	{
		public TestConfigurationElementManageabilityProviderWithValidWmiImplementations2()
		{ }
	}
	public class TestConfigurationElement : NamedConfigurationElement
	{ }
}
