/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.Unity
{
	[TestClass]
	public class EnterpriseLibraryBlockExtensionFixture
	{
		private const string settingsKey = "settings";
		private IUnityContainer container;
		private TestSettings settings;
		private DictionaryConfigurationSource configurationSource;
		[TestInitialize]
		public void SetUp()
		{
			container = new UnityContainer();
			settings = new TestSettings();
			configurationSource = new DictionaryConfigurationSource();
			configurationSource.Add(settingsKey, settings);
			container.AddExtension(new EnterpriseLibraryCoreExtension(configurationSource));
		}
		[TestCleanup]
		public void TearDown()
		{
			container.Dispose();
		}
		[TestMethod]
		public void ConstructorMatchingIsUsedByDefault()
		{
			TestSettingWithNoPolicyCreatorAttribute setting = new TestSettingWithNoPolicyCreatorAttribute();
			setting.Name = "provider";
			setting.Property1 = 1;
			setting.Property2 = "value2";
			settings.Settings.Add(setting);
			container.AddExtension(new TestEnterpriseLibraryBlockExtension());
			TestClass provider
				= container.Resolve<TestClass>("provider");
			Assert.IsNotNull(provider);
			Assert.AreEqual(setting.Property1, provider.Property1);
			Assert.AreEqual(setting.Property2, provider.Property2);
		}
		[TestMethod]
		public void SpecifiedPolicyCreatorIsUsedIfExists()
		{
			TestSettingWithPolicyCreatorAttribute setting = new TestSettingWithPolicyCreatorAttribute();
			setting.Name = "provider";
			setting.Property1 = 1;
			setting.Property2 = "value2";
			settings.Settings.Add(setting);
			container.AddExtension(new TestEnterpriseLibraryBlockExtension());
			TestClass provider
				= container.Resolve<TestClass>("provider");
			Assert.IsNotNull(provider);
			Assert.AreEqual(10 + setting.Property1, provider.Property1);
			Assert.AreEqual("test " + setting.Property2, provider.Property2);
		}
		public class TestClass
		{
			public TestClass(int property1)
			{
				this.Property1 = property1;
				this.Property2 = "not set";
			}
			public TestClass(string property2, int property1)
			{
				this.Property1 = property1;
				this.Property2 = property2;
			}
			public int Property1 { get; set; }
			public string Property2 { get; set; }
		}
		public class TestEnterpriseLibraryBlockExtension : EnterpriseLibraryBlockExtension
		{
			protected override void Initialize()
			{
				TestSettings settings = (TestSettings)ConfigurationSource.GetSection(settingsKey);
				CreateProvidersPolicies<TestClass, TestSettingBase>(
					Context.Policies,
					null,
					settings.Settings,
					ConfigurationSource);
			}
		}
		class TestSettings : ConfigurationSection
		{
			[ConfigurationProperty("settings")]
			public NameTypeConfigurationElementCollection<TestSettingBase, TestSettingBase> Settings
			{
				get
				{
					return (NameTypeConfigurationElementCollection<TestSettingBase, TestSettingBase>)base["settings"];
				}
			}
		}
		public class TestSettingBase : NameTypeConfigurationElement
		{
			public TestSettingBase() : base("name", typeof(TestClass)) { }
			public int Property1 { get; set; }
			public string Property2 { get; set; }
		}
		class TestSettingWithNoPolicyCreatorAttribute : TestSettingBase
		{
		}
		[ContainerPolicyCreator(typeof(TestSettingWithPolicyCreatorAttributePolicyCreator))]
		class TestSettingWithPolicyCreatorAttribute : TestSettingBase
		{
		}
		class TestSettingWithPolicyCreatorAttributePolicyCreator : IContainerPolicyCreator
		{
			void IContainerPolicyCreator.CreatePolicies(
				IPolicyList policyList,
				string instanceName,
				ConfigurationElement configurationObject,
				IConfigurationSource configurationSource)
			{
				new PolicyBuilder<TestClass, TestSettingWithPolicyCreatorAttribute>(
					NamedTypeBuildKey.Make<TestClass>(instanceName),
					(TestSettingWithPolicyCreatorAttribute)configurationObject,
					c => new TestClass("test " + c.Property2, 10 + c.Property1))
					.AddPoliciesToPolicyList(policyList);
			}
		}
	}
}
