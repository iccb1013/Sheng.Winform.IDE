/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Storage;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    [TestClass]
    public class LogWriterRefreshFixture
    {
        [TestCleanup]
        public void TearDown()
        {
            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            LoggingSettings rwSettings = rwConfiguration.GetSection(LoggingSettings.SectionName) as LoggingSettings;
            ((CategoryFilterData)rwSettings.LogFilters.Get("Category")).CategoryFilters.Remove("MockCategoryOne");
            rwConfiguration.Save();
            ConfigurationManager.RefreshSection(LoggingSettings.SectionName);
            ConfigurationChangeWatcher.ResetDefaultPollDelay();
        }
        [TestMethod]
        public void ConfigurationChangeNotificationRefreshesLogger()
        {
            SystemConfigurationSource.ResetImplementation(false);
            Logger.Reset();
            MockTraceListener.Reset();
            Logger.Write("test", "MockCategoryOne");
            Assert.AreEqual(1, MockTraceListener.Entries.Count);
            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            LoggingSettings rwSettings = rwConfiguration.GetSection(LoggingSettings.SectionName) as LoggingSettings;
            ((CategoryFilterData)rwSettings.LogFilters.Get("Category")).CategoryFilters.Add(new CategoryFilterEntry("MockCategoryOne"));
            rwConfiguration.Save();
            SystemConfigurationSource.Implementation.ConfigSourceChanged(string.Empty);
            MockTraceListener.Reset();
            Logger.Write("test", "MockCategoryOne");
            Assert.AreEqual(0, MockTraceListener.Entries.Count, "should have been filtered out by the new category filter");
        }
        [TestMethod]
        [Ignore] 
        public void ConfigurationChangeNotificationRefreshesLoggerAutomatically()
        {
            ConfigurationChangeWatcher.SetDefaultPollDelayInMilliseconds(100);
            SystemConfigurationSource.ResetImplementation(true);
            Logger.Reset();
            MockTraceListener.Reset();
            Logger.Write("test", "MockCategoryOne");
            Assert.AreEqual(1, MockTraceListener.Entries.Count);
            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            LoggingSettings rwSettings = rwConfiguration.GetSection(LoggingSettings.SectionName) as LoggingSettings;
            ((CategoryFilterData)rwSettings.LogFilters.Get("Category")).CategoryFilters.Add(new CategoryFilterEntry("MockCategoryOne"));
            rwConfiguration.Save();
            Thread.Sleep(400);
            MockTraceListener.Reset();
            Logger.Write("test", "MockCategoryOne");
            Assert.AreEqual(0, MockTraceListener.Entries.Count);
        }
    }
}
