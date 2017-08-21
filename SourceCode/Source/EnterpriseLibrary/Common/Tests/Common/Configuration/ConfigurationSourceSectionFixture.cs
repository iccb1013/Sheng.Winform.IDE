/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests
{
    [TestClass]
    public class ConfigurationSourceSectionFixture
    {
        const string configFileName = "test.exe.config";
        const string filePath = "myfile.config";
        const string fileSourceName = "fileSource";
        [TestMethod]
        public void CanReadAndWriteConfigurationSourceSectionInformation()
        {
            RemoveSection();
            SaveSection();
            System.Configuration.Configuration config = OpenFileConfig();
            ConfigurationSourceSection section = (ConfigurationSourceSection)config.GetSection(ConfigurationSourceSection.SectionName);
            FileConfigurationSourceElement elem = (FileConfigurationSourceElement)section.Sources.Get(fileSourceName);
            Assert.AreEqual(typeof(FileConfigurationSource), elem.Type);
            Assert.AreEqual(filePath, elem.FilePath);
            RemoveSection();
        }
        void SaveSection()
        {
            System.Configuration.Configuration config = OpenFileConfig();
            config.Sections.Add(ConfigurationSourceSection.SectionName, CreateConfigurationSourceSection());
            config.Save();
        }
        void RemoveSection()
        {
            System.Configuration.Configuration config = OpenFileConfig();
            config.Sections.Remove(ConfigurationSourceSection.SectionName);
            config.Save();
        }
        static System.Configuration.Configuration OpenFileConfig()
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = configFileName;
            System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            return config;
        }
        ConfigurationSourceSection CreateConfigurationSourceSection()
        {
            ConfigurationSourceSection section = new ConfigurationSourceSection();
            section.Sources.Add(new FileConfigurationSourceElement(fileSourceName, filePath));
            return section;
        }
        [TestMethod]
        public void CanDeserializeSerializedConfiguration()
        {
            string sourceName1 = "source1";
            string sourceName2 = "source2";
            string sourceFile1 = "file 1";
            ConfigurationSourceSection settings = new ConfigurationSourceSection();
            FileConfigurationSource.ResetImplementation(sourceFile1, false);
            ConfigurationSourceElement data1 = new FileConfigurationSourceElement(sourceName1, sourceFile1);
            ConfigurationSourceElement data2 = new SystemConfigurationSourceElement(sourceName2);
            settings.Sources.Add(data1);
            settings.Sources.Add(data2);
            settings.SelectedSource = sourceName1;
            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[ConfigurationSourceSection.SectionName] = settings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);
            ConfigurationSourceSection roSettigs = (ConfigurationSourceSection)configurationSource.GetSection(ConfigurationSourceSection.SectionName);
            Assert.IsNotNull(roSettigs);
            Assert.AreEqual(2, roSettigs.Sources.Count);
            Assert.AreEqual(sourceName1, roSettigs.SelectedSource);
            Assert.IsNotNull(roSettigs.Sources.Get(sourceName1));
            Assert.AreSame(typeof(FileConfigurationSourceElement), roSettigs.Sources.Get(sourceName1).GetType());
            Assert.AreEqual(sourceFile1, ((FileConfigurationSourceElement)roSettigs.Sources.Get(sourceName1)).FilePath);
            Assert.IsNotNull(roSettigs.Sources.Get(sourceName2));
            Assert.AreSame(typeof(SystemConfigurationSourceElement), roSettigs.Sources.Get(sourceName2).GetType());
        }
    }
}
