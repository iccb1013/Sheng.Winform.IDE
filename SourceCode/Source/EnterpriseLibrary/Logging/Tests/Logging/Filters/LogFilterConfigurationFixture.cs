/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SysConf = System.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Filters.Tests
{
    [TestClass]
    public class LogFilterConfigurationFixture
    {
        IBuilderContext context;
        ConfigurationReflectionCache reflectionCache;
        [TestInitialize]
        public void SetUp()
        {
			context = new BuilderContext(new StrategyChain(), null, null, new PolicyList(), null, null);
            reflectionCache = new ConfigurationReflectionCache();
        }
        [TestMethod]
        public void CanReadWrittenFilterConfiguration()
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = "test.exe.config";
            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            rwConfiguration.Sections.Remove(LoggingSettings.SectionName);
            LoggingSettings rwLoggingSettings = new LoggingSettings();
            rwConfiguration.Sections.Add(LoggingSettings.SectionName, rwLoggingSettings);
            rwLoggingSettings.LogFilters.Add(new LogEnabledFilterData("enabled", true));
            NamedElementCollection<CategoryFilterEntry> categoryEntries = new NamedElementCollection<CategoryFilterEntry>();
            categoryEntries.Add(new CategoryFilterEntry("foo"));
            categoryEntries.Add(new CategoryFilterEntry("bar"));
            categoryEntries.Add(new CategoryFilterEntry("baz"));
            rwLoggingSettings.LogFilters.Add(new CategoryFilterData("category", categoryEntries, CategoryFilterMode.DenyAllExceptAllowed));
            rwLoggingSettings.LogFilters.Add(new PriorityFilterData("priority", 5));
            File.SetAttributes(fileMap.ExeConfigFilename, FileAttributes.Normal);
            rwConfiguration.Save();
            System.Configuration.Configuration roConfiguration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            LoggingSettings roLoggingSettings = (LoggingSettings)roConfiguration.Sections[LoggingSettings.SectionName];
            Assert.AreEqual(3, roLoggingSettings.LogFilters.Count);
            Assert.AreEqual(roLoggingSettings.LogFilters.Get("enabled").GetType(), typeof(LogEnabledFilterData));
            Assert.AreEqual(true, ((LogEnabledFilterData)roLoggingSettings.LogFilters.Get("enabled")).Enabled);
            Assert.AreEqual(roLoggingSettings.LogFilters.Get("category").GetType(), typeof(CategoryFilterData));
            Assert.AreEqual(3, ((CategoryFilterData)roLoggingSettings.LogFilters.Get("category")).CategoryFilters.Count);
            Assert.IsNotNull(((CategoryFilterData)roLoggingSettings.LogFilters.Get("category")).CategoryFilters.Get("foo"));
            Assert.IsNotNull(((CategoryFilterData)roLoggingSettings.LogFilters.Get("category")).CategoryFilters.Get("bar"));
            Assert.IsNotNull(((CategoryFilterData)roLoggingSettings.LogFilters.Get("category")).CategoryFilters.Get("baz"));
            Assert.AreEqual(CategoryFilterMode.DenyAllExceptAllowed, ((CategoryFilterData)roLoggingSettings.LogFilters.Get("category")).CategoryFilterMode);
            Assert.AreEqual(roLoggingSettings.LogFilters.Get("priority").GetType(), typeof(PriorityFilterData));
            Assert.AreEqual(5, ((PriorityFilterData)roLoggingSettings.LogFilters.Get("priority")).MinimumPriority);
        }
        [TestMethod]
        public void CanCreateCategoryFilterFromEmptyCategoryConfiguration()
        {
            NamedElementCollection<CategoryFilterEntry> categoryEntries = new NamedElementCollection<CategoryFilterEntry>();
            CategoryFilterData filterData = new CategoryFilterData("category", categoryEntries, CategoryFilterMode.DenyAllExceptAllowed);
            MockLogObjectsHelper helper = new MockLogObjectsHelper();
            ILogFilter filter = LogFilterCustomFactory.Instance.Create(context, filterData, helper.configurationSource, reflectionCache);
            ;
            Assert.IsNotNull(filter);
            Assert.AreEqual(filter.GetType(), typeof(CategoryFilter));
            Assert.AreEqual(0, ((CategoryFilter)filter).CategoryFilters.Count);
            Assert.AreEqual(CategoryFilterMode.DenyAllExceptAllowed, ((CategoryFilter)filter).CategoryFilterMode);
        }
        [TestMethod]
        public void CanCreateCategoryFilterFromNonEmptyCategoryConfiguration()
        {
            NamedElementCollection<CategoryFilterEntry> categoryEntries = new NamedElementCollection<CategoryFilterEntry>();
            categoryEntries.Add(new CategoryFilterEntry("foo"));
            categoryEntries.Add(new CategoryFilterEntry("bar"));
            categoryEntries.Add(new CategoryFilterEntry("baz"));
            CategoryFilterData filterData = new CategoryFilterData("category", categoryEntries, CategoryFilterMode.AllowAllExceptDenied);
            MockLogObjectsHelper helper = new MockLogObjectsHelper();
            ILogFilter filter = LogFilterCustomFactory.Instance.Create(context, filterData, helper.configurationSource, reflectionCache);
            ;
            Assert.IsNotNull(filter);
            Assert.AreEqual(filter.GetType(), typeof(CategoryFilter));
            Assert.AreEqual(3, ((CategoryFilter)filter).CategoryFilters.Count);
            Assert.IsTrue(((CategoryFilter)filter).CategoryFilters.Contains("foo"));
            Assert.IsTrue(((CategoryFilter)filter).CategoryFilters.Contains("bar"));
            Assert.IsTrue(((CategoryFilter)filter).CategoryFilters.Contains("baz"));
            Assert.IsFalse(((CategoryFilter)filter).CategoryFilters.Contains("foobar"));
            Assert.AreEqual(CategoryFilterMode.AllowAllExceptDenied, ((CategoryFilter)filter).CategoryFilterMode);
        }
        [TestMethod]
        public void CanCreatePriorityFilterFromConfiguration()
        {
            PriorityFilterData filterData = new PriorityFilterData(1000);
            MockLogObjectsHelper helper = new MockLogObjectsHelper();
            ILogFilter filter = LogFilterCustomFactory.Instance.Create(context, filterData, helper.configurationSource, reflectionCache);
            ;
            Assert.IsNotNull(filter);
            Assert.AreEqual(filter.GetType(), typeof(PriorityFilter));
            Assert.AreEqual(1000, ((PriorityFilter)filter).MinimumPriority);
        }
        [TestMethod]
        public void PriorityFilterMaximumPriotDefaultsToMaxIntWhenNotSpecified()
        {
            PriorityFilterData filterData = new PriorityFilterData(1000);
            MockLogObjectsHelper helper = new MockLogObjectsHelper();
            ILogFilter filter = LogFilterCustomFactory.Instance.Create(context, filterData, helper.configurationSource, reflectionCache);
            ;
            Assert.IsNotNull(filter);
            Assert.AreEqual(filter.GetType(), typeof(PriorityFilter));
            Assert.AreEqual(int.MaxValue, ((PriorityFilter)filter).MaximumPriority);
        }
        [TestMethod]
        public void PriorityFilterShouldNotLogWhenPriotityIsAboveMaxPriority()
        {
            PriorityFilterData filterData = new PriorityFilterData(0);
            filterData.MaximumPriority = 100;
            MockLogObjectsHelper helper = new MockLogObjectsHelper();
            ILogFilter filter = LogFilterCustomFactory.Instance.Create(context, filterData, helper.configurationSource, reflectionCache);
            ;
            Assert.IsNotNull(filter);
            Assert.AreEqual(filter.GetType(), typeof(PriorityFilter));
            Assert.IsTrue(((PriorityFilter)filter).ShouldLog(100));
            Assert.IsFalse(((PriorityFilter)filter).ShouldLog(101));
        }
        [TestMethod]
        public void CanCreateLogEnabledFilterFromConfiguration()
        {
            LogEnabledFilterData filterData = new LogEnabledFilterData(true);
            MockLogObjectsHelper helper = new MockLogObjectsHelper();
            ILogFilter filter = LogFilterCustomFactory.Instance.Create(context, filterData, helper.configurationSource, reflectionCache);
            ;
            Assert.IsNotNull(filter);
            Assert.AreEqual(filter.GetType(), typeof(LogEnabledFilter));
            Assert.AreEqual(true, ((LogEnabledFilter)filter).Enabled);
        }
    }
}
