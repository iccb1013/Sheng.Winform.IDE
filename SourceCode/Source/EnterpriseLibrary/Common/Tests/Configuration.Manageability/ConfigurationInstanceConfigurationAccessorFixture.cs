/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests
{
    [TestClass]
    public class ConfigurationInstanceConfigurationAccessorFixture
    {
        System.Configuration.Configuration configuration;
        ConfigurationInstanceConfigurationAccessor configurationAccessor;
        [TestInitialize]
        public void SetUp()
        {
            configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configurationAccessor = new ConfigurationInstanceConfigurationAccessor(configuration);
        }
        [TestMethod]
        public void CanRequestNonExistingSection()
        {
            ConfigurationSection section = configurationAccessor.GetSection("section");
            Assert.IsNull(section);
        }
        [TestMethod]
        public void RequestForNonExistingSectionIsRecorded()
        {
            ConfigurationSection section = configurationAccessor.GetSection("section");
            List<string> sectionNames
                = CreateCollectionFromEnumerable(configurationAccessor.GetRequestedSectionNames());
            Assert.AreEqual(1, sectionNames.Count);
            Assert.IsTrue(sectionNames.Contains("section"));
        }
        [TestMethod]
        public void MultipleRequestsForNonExistingSectionAreRecordedOnce()
        {
            ConfigurationSection section = configurationAccessor.GetSection("section");
            section = configurationAccessor.GetSection("section");
            section = configurationAccessor.GetSection("section");
            section = configurationAccessor.GetSection("section");
            List<string> sectionNames
                = CreateCollectionFromEnumerable(configurationAccessor.GetRequestedSectionNames());
            Assert.AreEqual(1, sectionNames.Count);
            Assert.IsTrue(sectionNames.Contains("section"));
        }
        [TestMethod]
        public void CanRequestExistingSection()
        {
            TestsConfigurationSection existingSection = new TestsConfigurationSection();
            configuration.Sections.Add("section", existingSection);
            ConfigurationSection section = configurationAccessor.GetSection("section");
            Assert.AreSame(existingSection, section);
        }
        [TestMethod]
        public void RequestForExistingSectionIsRecorded()
        {
            TestsConfigurationSection existingSection = new TestsConfigurationSection();
            configuration.Sections.Add("section", existingSection);
            ConfigurationSection section = configurationAccessor.GetSection("section");
            List<string> sectionNames
                = CreateCollectionFromEnumerable(configurationAccessor.GetRequestedSectionNames());
            Assert.AreEqual(1, sectionNames.Count);
            Assert.IsTrue(sectionNames.Contains("section"));
        }
        [TestMethod]
        public void MultipleRequestsForExistingSectionAreRecordedOnce()
        {
            TestsConfigurationSection existingSection = new TestsConfigurationSection();
            configuration.Sections.Add("section", existingSection);
            ConfigurationSection section = configurationAccessor.GetSection("section");
            section = configurationAccessor.GetSection("section");
            section = configurationAccessor.GetSection("section");
            section = configurationAccessor.GetSection("section");
            List<string> sectionNames
                = CreateCollectionFromEnumerable(configurationAccessor.GetRequestedSectionNames());
            Assert.AreEqual(1, sectionNames.Count);
            Assert.IsTrue(sectionNames.Contains("section"));
        }
        [TestMethod]
        public void MultipleRequestsForExistingAndNonExistingSectionsAreRecordedOnce()
        {
            TestsConfigurationSection existingSection1 = new TestsConfigurationSection();
            configuration.Sections.Add("section1", existingSection1);
            TestsConfigurationSection existingSection4 = new TestsConfigurationSection();
            configuration.Sections.Add("section4", existingSection4);
            ConfigurationSection section = configurationAccessor.GetSection("section1");
            Assert.AreSame(existingSection1, section);
            section = configurationAccessor.GetSection("section2");
            Assert.IsNull(section);
            section = configurationAccessor.GetSection("section3");
            Assert.IsNull(section);
            section = configurationAccessor.GetSection("section4");
            Assert.AreSame(existingSection4, section);
            List<string> sectionNames
                = CreateCollectionFromEnumerable(configurationAccessor.GetRequestedSectionNames());
            Assert.AreEqual(4, sectionNames.Count);
            Assert.IsTrue(sectionNames.Contains("section1"));
            Assert.IsTrue(sectionNames.Contains("section2"));
            Assert.IsTrue(sectionNames.Contains("section3"));
            Assert.IsTrue(sectionNames.Contains("section4"));
        }
        [TestMethod]
        public void CanRemoveNonExistingSection()
        {
            Assert.IsNull(configurationAccessor.GetSection("section"));
            configurationAccessor.RemoveSection("section");
            Assert.IsNull(configurationAccessor.GetSection("section"));
        }
        [TestMethod]
        public void CanRemoveExistingSection()
        {
            TestsConfigurationSection existingSection = new TestsConfigurationSection();
            configuration.Sections.Add("section", existingSection);
            Assert.IsNotNull(configurationAccessor.GetSection("section"));
            configurationAccessor.RemoveSection("section");
            Assert.IsNull(configurationAccessor.GetSection("section"));
        }
        List<string> CreateCollectionFromEnumerable(IEnumerable<string> enumerable)
        {
            List<string> result = new List<string>();
            foreach (String element in enumerable)
            {
                result.Add(element);
            }
            return result;
        }
    }
}
