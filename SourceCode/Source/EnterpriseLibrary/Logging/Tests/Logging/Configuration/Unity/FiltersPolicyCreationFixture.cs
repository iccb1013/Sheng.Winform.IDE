/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration.Unity
{
	[TestClass]
	public class FiltersPolicyCreationFixture
	{
		private IUnityContainer container;
		private LoggingSettings loggingSettings;
		private DictionaryConfigurationSource configurationSource;
		[TestInitialize]
		public void SetUp()
		{
			loggingSettings = new LoggingSettings();
			configurationSource = new DictionaryConfigurationSource();
			configurationSource.Add(LoggingSettings.SectionName, loggingSettings);
			container = new UnityContainer();
			container.AddExtension(new EnterpriseLibraryCoreExtension(configurationSource));
		}
		[TestCleanup]
		public void TearDown()
		{
			container.Dispose();
		}
		[TestMethod]
		public void CanCreatePoliciesForCategoryFilter()
		{
			CategoryFilterData data = new CategoryFilterData();
			data.Type = typeof(CategoryFilter);
			data.Name = "name";
			data.CategoryFilterMode = CategoryFilterMode.DenyAllExceptAllowed;
			data.CategoryFilters.Add(new CategoryFilterEntry("foo"));
			data.CategoryFilters.Add(new CategoryFilterEntry("bar"));
			loggingSettings.LogFilters.Add(data);
			container.AddExtension(new LoggingBlockExtension());
			CategoryFilter createdObject = (CategoryFilter)container.Resolve<ILogFilter>("name");
			Assert.IsNotNull(createdObject);
			Assert.AreEqual(CategoryFilterMode.DenyAllExceptAllowed, createdObject.CategoryFilterMode);
			Assert.AreEqual(2, createdObject.CategoryFilters.Count);
			Assert.IsTrue(createdObject.CategoryFilters.Contains("foo"));
			Assert.IsTrue(createdObject.CategoryFilters.Contains("bar"));
		}
		[TestMethod]
		public void CanCreatePoliciesForPriorityFilter()
		{
			PriorityFilterData data = new PriorityFilterData("provider name", 10);
			data.MaximumPriority = 100;
			loggingSettings.LogFilters.Add(data);
			container.AddExtension(new LoggingBlockExtension());
			PriorityFilter createdObject = (PriorityFilter)container.Resolve<ILogFilter>("provider name");
			Assert.IsNotNull(createdObject);
			Assert.AreEqual("provider name", createdObject.Name);
			Assert.AreEqual(10, createdObject.MinimumPriority);
			Assert.AreEqual(100, createdObject.MaximumPriority);
		}
		[TestMethod]
		public void CanCreatePoliciesForEnabledFilter()
		{
			LogEnabledFilterData data = new LogEnabledFilterData("provider name", true);
			loggingSettings.LogFilters.Add(data);
			container.AddExtension(new LoggingBlockExtension());
			LogEnabledFilter createdObject = (LogEnabledFilter)container.Resolve<ILogFilter>("provider name");
			Assert.IsNotNull(createdObject);
			Assert.AreEqual("provider name", createdObject.Name);
			Assert.AreEqual(true, createdObject.Enabled);
		}
	}
}
