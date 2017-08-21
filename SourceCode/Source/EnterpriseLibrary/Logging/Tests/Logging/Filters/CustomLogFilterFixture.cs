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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Filters.Tests
{
    [TestClass]
    public class CustomLogFilterFixture
    {
        [TestInitialize]
        public void TestInitialize()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
        }
        [TestMethod]
        public void CanBuildCustomLogFilterFromGivenConfiguration()
        {
            CustomLogFilterData filterData
                = new CustomLogFilterData("custom", typeof(MockCustomLogFilter));
            filterData.SetAttributeValue(MockCustomProviderBase.AttributeKey, "value1");
            MockLogObjectsHelper helper = new MockLogObjectsHelper();
			ILogFilter filter
				= LogFilterCustomFactory.Instance.Create(
					new BuilderContext(new StrategyChain(), null, null, new PolicyList(), null, null),
					filterData,
					helper.configurationSource,
					new ConfigurationReflectionCache());
            Assert.IsNotNull(filter);
            Assert.AreSame(typeof(MockCustomLogFilter), filter.GetType());
            Assert.AreEqual("value1", ((MockCustomLogFilter)filter).customValue);
        }
        [TestMethod]
        public void CanDeserializeSerializedConfiguration()
        {
            LoggingSettings rwLoggingSettings = new LoggingSettings();
            rwLoggingSettings.LogFilters.Add(new CustomLogFilterData("filter1", typeof(MockCustomLogFilter)));
            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[LoggingSettings.SectionName] = rwLoggingSettings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);
            LoggingSettings roLoggingSettings = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);
            Assert.AreEqual(1, roLoggingSettings.LogFilters.Count);
            Assert.IsNotNull(roLoggingSettings.LogFilters.Get("filter1"));
            Assert.AreSame(typeof(CustomLogFilterData), roLoggingSettings.LogFilters.Get("filter1").GetType());
            Assert.AreSame(typeof(MockCustomLogFilter), roLoggingSettings.LogFilters.Get("filter1").Type);
        }
    }
}
