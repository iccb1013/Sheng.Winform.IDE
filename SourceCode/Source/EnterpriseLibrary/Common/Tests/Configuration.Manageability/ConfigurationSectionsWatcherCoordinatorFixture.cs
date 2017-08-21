/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests
{
    [TestClass]
    public class ConfigurationChangeWatcherCoordinatorFixture
    {
        System.Configuration.Configuration configuration;
        ConfigurationChangeWatcherCoordinator coordinator;
        List<ConfigurationChangedEventArgs> notifiedEvents;
        [TestInitialize]
        public void SetUp()
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = "test.config";
            configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            coordinator = new ConfigurationChangeWatcherCoordinator(configuration.FilePath, false);
            notifiedEvents = new List<ConfigurationChangedEventArgs>();
        }
        [TestMethod]
        public void MainConfigurationFileIsWatchedOnStartup()
        {
            Assert.IsTrue(coordinator.IsWatchingConfigSource(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));
        }
        [TestMethod]
        public void CanSetWatcherForLocalSection()
        {
            ConfigurationSection section1 = configuration.GetSection("local.section.1");
            coordinator.SetWatcherForConfigSource(section1.SectionInformation.ConfigSource);
            Assert.IsTrue(coordinator.IsWatchingConfigSource(section1.SectionInformation.ConfigSource));
        }
        [TestMethod]
        public void CanSetWatcherForExternalSection()
        {
            ConfigurationSection section1 = configuration.GetSection("external.section.1");
            Assert.AreEqual(1, coordinator.WatchedConfigSources.Count);
            Assert.IsFalse(coordinator.IsWatchingConfigSource(section1.SectionInformation.ConfigSource));
            coordinator.SetWatcherForConfigSource(section1.SectionInformation.ConfigSource);
            Assert.AreEqual(2, coordinator.WatchedConfigSources.Count);
            Assert.IsTrue(coordinator.WatchedConfigSources.Contains(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));
            Assert.IsTrue(coordinator.WatchedConfigSources.Contains(section1.SectionInformation.ConfigSource));
        }
        [TestMethod]
        public void ChangeInMainFileFiresTheAppropriateEvent()
        {
            ConfigurationSection section1 = configuration.GetSection("local.section.1");
            coordinator.SetWatcherForConfigSource(section1.SectionInformation.ConfigSource);
            coordinator.ConfigurationChanged += OnConfigurationChanged;
            coordinator.OnConfigurationChanged(null,
                                               new ConfigurationChangedEventArgs(section1.SectionInformation.ConfigSource));
            Assert.AreEqual(1, notifiedEvents.Count);
            Assert.AreEqual(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource, notifiedEvents[0].SectionName);
        }
        [TestMethod]
        public void ChangeInExternalFileFiresTheAppropriateEvent()
        {
            ConfigurationSection section1 = configuration.GetSection("external.section.1");
            coordinator.SetWatcherForConfigSource(section1.SectionInformation.ConfigSource);
            coordinator.ConfigurationChanged += OnConfigurationChanged;
            coordinator.OnConfigurationChanged(null,
                                               new ConfigurationChangedEventArgs(section1.SectionInformation.ConfigSource));
            Assert.AreEqual(1, notifiedEvents.Count);
            Assert.AreEqual(section1.SectionInformation.ConfigSource, notifiedEvents[0].SectionName);
        }
        [TestMethod]
        public void CanRemoveWatcher()
        {
            ConfigurationSection section1 = configuration.GetSection("external.section.1");
            coordinator.SetWatcherForConfigSource(section1.SectionInformation.ConfigSource);
            Assert.AreEqual(2, coordinator.WatchedConfigSources.Count);
            Assert.IsTrue(coordinator.WatchedConfigSources.Contains(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));
            Assert.IsTrue(coordinator.WatchedConfigSources.Contains(section1.SectionInformation.ConfigSource));
            coordinator.RemoveWatcherForConfigSource(section1.SectionInformation.ConfigSource);
            Assert.AreEqual(1, coordinator.WatchedConfigSources.Count);
            Assert.IsTrue(coordinator.WatchedConfigSources.Contains(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));
        }
        [TestMethod]
        public void CanRemoveWatcherForNonWatchedConfigSource()
        {
            ConfigurationSection section1 = configuration.GetSection("external.section.1");
            coordinator.RemoveWatcherForConfigSource(section1.SectionInformation.ConfigSource);
            Assert.AreEqual(1, coordinator.WatchedConfigSources.Count);
            Assert.IsTrue(coordinator.WatchedConfigSources.Contains(ConfigurationChangeWatcherCoordinator.MainConfigurationFileSource));
        }
        [TestMethod]
        public void CanDisposeWatcher()
        {
            ConfigurationSection section1 = configuration.GetSection("external.section.1");
            coordinator.SetWatcherForConfigSource(section1.SectionInformation.ConfigSource);
            coordinator.Dispose();
        }
        void OnConfigurationChanged(object sender,
                                    ConfigurationChangedEventArgs e)
        {
            notifiedEvents.Add(e);
        }
    }
}
