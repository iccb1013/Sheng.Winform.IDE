/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestClass]
    public class LoggingConfigurationDesignManagerFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void OpenAndSaveConfiguration()
        {
            ApplicationNode.Hierarchy.Load();
            Assert.AreEqual(0, ServiceHelper.GetErrorService(ServiceProvider).ConfigurationErrorCount);
            ApplicationNode.Hierarchy.Open();
            Assert.AreEqual(0, ServiceHelper.GetErrorService(ServiceProvider).ConfigurationErrorCount);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(LoggingSettingsNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(CategoryFilterNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(LogEnabledFilterNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(PriorityFilterNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(CategoryTraceSourceNode)).Count);
            Assert.AreEqual(5, ApplicationNode.Hierarchy.FindNodesByType(typeof(TraceListenerReferenceNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(EmailTraceListenerNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(FlatFileTraceListenerNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(MsmqTraceListenerNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(SystemDiagnosticsTraceListenerNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(WmiTraceListenerNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(BinaryFormatterNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(TextFormatterNode)).Count);
            Assert.AreEqual(2, ApplicationNode.Hierarchy.FindNodesByType(typeof(CustomFormatterNode)).Count);
            ApplicationNode.Hierarchy.Save();
            Assert.AreEqual(0, ServiceHelper.GetErrorService(ServiceProvider).ConfigurationErrorCount);
            ApplicationNode.Hierarchy.Load();
            Assert.AreEqual(0, ServiceHelper.GetErrorService(ServiceProvider).ConfigurationErrorCount);
            ApplicationNode.Hierarchy.Open();
            Assert.AreEqual(0, ServiceHelper.GetErrorService(ServiceProvider).ConfigurationErrorCount);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(LoggingSettingsNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(CategoryFilterNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(LogEnabledFilterNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(PriorityFilterNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(CategoryTraceSourceNode)).Count);
            Assert.AreEqual(5, ApplicationNode.Hierarchy.FindNodesByType(typeof(TraceListenerReferenceNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(EmailTraceListenerNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(FlatFileTraceListenerNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(MsmqTraceListenerNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(SystemDiagnosticsTraceListenerNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(WmiTraceListenerNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(BinaryFormatterNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(TextFormatterNode)).Count);
            Assert.AreEqual(2, ApplicationNode.Hierarchy.FindNodesByType(typeof(CustomFormatterNode)).Count);
        }
        [TestMethod]
        public void BuildContextTest()
        {
            LoggingConfigurationDesignManager designManager = new LoggingConfigurationDesignManager();
            designManager.Register(ServiceProvider);
            designManager.Open(ServiceProvider);
            DictionaryConfigurationSource dictionarySource = new DictionaryConfigurationSource();
            designManager.BuildConfigurationSource(ServiceProvider, dictionarySource);
            Assert.IsTrue(dictionarySource.Contains("loggingConfiguration"));
        }
    }
    [ConfigurationElementType(typeof(CustomFormatterData))]
    public class CustomFormatter : LogFormatter
    {
        public override string Format(LogEntry log)
        {
            return string.Empty;
        }
    }
}
