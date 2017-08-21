/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestClass]
    public class LoggingSettingsNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void LoggingSettingsNamePropertyIsReadOnly()
        {
            Assert.AreEqual(true, CommonUtil.IsPropertyReadOnly(typeof(LoggingSettingsNode), "Name"));
        }
        [TestMethod]
        public void LoggingSettingsNodeDefaultDataTest()
        {
            LoggingSettingsNode loggingSettings = new LoggingSettingsNode();
            ApplicationNode.AddNode(loggingSettings);
            Assert.AreEqual(false, loggingSettings.LogWarningWhenNoCategoriesMatch);
            Assert.AreEqual(false, loggingSettings.TracingEnabled);
            Assert.AreEqual("Logging Application Block", loggingSettings.Name);
            Assert.IsNull(loggingSettings.DefaultCategory);
        }
        [TestMethod]
        public void LoggingSettingsNodeTest()
        {
            LoggingSettingsNode node = new LoggingSettingsNode();
            ApplicationNode.AddNode(node);
            bool logWarningWhenNoCategoriesMatch = true;
            bool tracingEnabled = true;
            node.LogWarningWhenNoCategoriesMatch = logWarningWhenNoCategoriesMatch;
            Assert.AreEqual(logWarningWhenNoCategoriesMatch, node.LogWarningWhenNoCategoriesMatch);
            node.TracingEnabled = tracingEnabled;
            Assert.AreEqual(tracingEnabled, node.TracingEnabled);
            LoggingSettingsBuilder builder = new LoggingSettingsBuilder(ServiceProvider, node);
            LoggingSettings data = builder.Build();
            Assert.AreEqual("Logging Application Block", data.Name);
            Assert.AreEqual(logWarningWhenNoCategoriesMatch, data.LogWarningWhenNoCategoriesMatch);
            Assert.AreEqual(tracingEnabled, data.TracingEnabled);
        }
        [TestMethod]
        public void LogCategoryOnlyLinksWithCateogrySources()
        {
            Type defaultCategoryType = typeof(LoggingSettingsNode).GetProperty("DefaultCategory").PropertyType;
            Assert.AreEqual(typeof(CategoryTraceSourceNode), defaultCategoryType);
        }
    }
}
