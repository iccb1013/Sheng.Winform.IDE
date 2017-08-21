/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.Unity
{
	[TestClass]
	public class EnterpriseLibraryCoreExtensionFixture
	{
		private IUnityContainer container;
		private DictionaryConfigurationSource configurationSource;
		[TestInitialize]
		public void SetUp()
		{
			container = new UnityContainer();
			configurationSource = new DictionaryConfigurationSource();
		}
		[TestCleanup]
		public void TearDown()
		{
			container.Dispose();
		}
		[TestMethod]
		public void ExtensionPopulatesTheConfigurationSourcePolicy()
		{
			IConfigurationSource theAvailableConfigurationSource = null;
			container.AddExtension(new EnterpriseLibraryCoreExtension(configurationSource));
			container.AddExtension(new TestHelperExtension(
				c =>
				{
					theAvailableConfigurationSource
						= c.Policies.Get<IConfigurationObjectPolicy>(typeof(IConfigurationSource)).ConfigurationSource;
				}));
			Assert.AreSame(configurationSource, theAvailableConfigurationSource);
		}
		[TestMethod]
		public void ExtensionAddsInstrumentationStrategy()
		{
			configurationSource.Add(
				InstrumentationConfigurationSection.SectionName, 
				new InstrumentationConfigurationSection(false, true, false));
			InstrumentationAttachmentStrategyFixture.ApplicationClass createdObject
				= container.Resolve<InstrumentationAttachmentStrategyFixture.ApplicationClass>("foo");
			Assert.IsFalse(((InstrumentationAttachmentStrategyFixture.InstrumentationProvider)createdObject.GetInstrumentationEventProvider()).IsWired);
			container.AddExtension(new EnterpriseLibraryCoreExtension(configurationSource));
			createdObject
				= container.Resolve<InstrumentationAttachmentStrategyFixture.ApplicationClass>("foo");
			Assert.IsTrue(((InstrumentationAttachmentStrategyFixture.InstrumentationProvider)createdObject.GetInstrumentationEventProvider()).IsWired);
		}
	}
}
