/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests
{
    [TestClass]
    public class SystemConfigurationSourceFixture
    {
        const string localSection = "dummy.local";
        const string localSectionSource = "";
        IDictionary<string, int> updatedSectionsTally;
        [TestInitialize]
        public void Setup()
        {
            SystemConfigurationSource.ResetImplementation(false);
            ConfigurationChangeWatcher.SetDefaultPollDelayInMilliseconds(50);
            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            DummySection rwSection;
            rwConfiguration.Sections.Remove(localSection);
            rwConfiguration.Sections.Add(localSection, rwSection = new DummySection());
            rwSection.Name = localSection;
            rwSection.Value = 10;
            rwSection.SectionInformation.ConfigSource = localSectionSource;
            updatedSectionsTally = new Dictionary<string, int>(0);
        }
        [TestCleanup]
        public void TearDown()
        {
            ConfigurationChangeWatcher.ResetDefaultPollDelay();
        }
        [TestMethod]
        public void SameImplementationSurvivesAcrossInstances()
        {
            SystemConfigurationSource source1 = new SystemConfigurationSource();
            object section = source1.GetSection(localSection);
            Assert.IsNotNull(section);
            Assert.AreEqual(1, SystemConfigurationSource.Implementation.WatchedSections.Count);
            Assert.IsTrue(SystemConfigurationSource.Implementation.WatchedSections.Contains(localSection));
            source1 = null;
            GC.Collect(3);
            Assert.AreEqual(1, SystemConfigurationSource.Implementation.WatchedSections.Count);
            Assert.IsTrue(SystemConfigurationSource.Implementation.WatchedSections.Contains("dummy.local"));
        }
        [TestMethod]
        public void CanReceiveNotificationsFromImplementation()
        {
            SystemConfigurationSource.ResetImplementation(false);
            SystemConfigurationSource source = new SystemConfigurationSource();
            source.GetSection(localSection);
            source.AddSectionChangeHandler(localSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            SystemConfigurationSource.Implementation.ConfigSourceChanged(localSectionSource);
            Assert.AreEqual(1, updatedSectionsTally[localSection]);
        }
        [TestMethod]
        public void SystemConfigurationSourceReturnsReadOnlySections()
        {
            SystemConfigurationSource.ResetImplementation(false);
            SystemConfigurationSource source = new SystemConfigurationSource();
            ConfigurationSection dummySection = source.GetSection(localSection);
            Assert.IsTrue(dummySection.IsReadOnly());
        }
        [TestMethod]
        public void CanStopReceivingNotificationsFromImplementation()
        {
            SystemConfigurationSource.ResetImplementation(false);
            SystemConfigurationSource source = new SystemConfigurationSource();
            source.GetSection(localSection);
            source.AddSectionChangeHandler(localSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            SystemConfigurationSource.Implementation.ConfigSourceChanged(localSectionSource);
            Assert.AreEqual(1, updatedSectionsTally[localSection]);
            source.RemoveSectionChangeHandler(localSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            SystemConfigurationSource.Implementation.ConfigSourceChanged(localSectionSource);
            Assert.AreEqual(1, updatedSectionsTally[localSection]);
            source.AddSectionChangeHandler(localSection, new ConfigurationChangedEventHandler(OnConfigurationChanged));
            SystemConfigurationSource.Implementation.ConfigSourceChanged(localSectionSource);
            Assert.AreEqual(2, updatedSectionsTally[localSection]);
        }
        [TestMethod]
        public void RemovingAndAddingSection()
        {
            SystemConfigurationSource.ResetImplementation(true);
            SystemConfigurationSource sysSource = new SystemConfigurationSource();
            DummySection dummySection = sysSource.GetSection(localSection) as DummySection;
            Assert.IsTrue(dummySection != null);
            Thread.Sleep(300);
            System.Configuration.Configuration rwConfiguration =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string fileName = rwConfiguration.FilePath;
            int numSections = rwConfiguration.Sections.Count;
            FileConfigurationParameter parameter = new FileConfigurationParameter(fileName);
            sysSource.Remove(parameter, localSection);
            Thread.Sleep(300);
            rwConfiguration =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            Assert.AreEqual(rwConfiguration.Sections.Count, numSections - 1);
            sysSource.Add(parameter, localSection, new DummySection()); 
            Thread.Sleep(300);
            rwConfiguration =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            Assert.AreEqual(rwConfiguration.Sections.Count, numSections);
        }
        void OnConfigurationChanged(object sender,
                                    ConfigurationChangedEventArgs args)
        {
            if (updatedSectionsTally.ContainsKey(args.SectionName))
            {
                updatedSectionsTally[args.SectionName] = updatedSectionsTally[args.SectionName] + 1;
            }
            else
            {
                updatedSectionsTally[args.SectionName] = 1;
            }
        }
    }
}
