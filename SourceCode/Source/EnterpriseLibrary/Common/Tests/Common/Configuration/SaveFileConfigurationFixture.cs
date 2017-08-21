/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using System.IO;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration
{
    [TestClass]
    public class SaveFileConfigurationFixture
    {
        string file;
        [TestInitialize]
        public void TestInitialize()
        {
            file = CreateFile();
        }
        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists(file)) File.Delete(file);
        }
        [TestMethod]
        public void CanSaveConfigurationSectionToFile()
        {
            SystemConfigurationSource source = new SystemConfigurationSource();
            source.Save(file, InstrumentationConfigurationSection.SectionName, CreateInstrumentationSection());
            ValidateConfiguration(file);
        }
        string CreateFile()
        {
            string tempFile = Path.Combine(Directory.GetCurrentDirectory(), @"app.config");
            XmlDocument doc = new XmlDocument();
            XmlElement elem = doc.CreateElement("configuration");
            doc.AppendChild(elem);
            doc.Save(tempFile);
            return tempFile;
            ;
        }
        void ValidateConfiguration(string configFile)
        {
            InstrumentationConfigurationSection section = GetSection(configFile);
            Assert.IsTrue(section.PerformanceCountersEnabled);
            Assert.IsTrue(section.WmiEnabled);
            Assert.IsTrue(section.EventLoggingEnabled);
        }
        InstrumentationConfigurationSection GetSection(string configFile)
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = configFile;
            System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            InstrumentationConfigurationSection section = (InstrumentationConfigurationSection)config.GetSection(InstrumentationConfigurationSection.SectionName);
            return section;
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TryToSaveWithNullOrEmptyFileNameThrows()
        {
            SystemConfigurationSource source = new SystemConfigurationSource();
            source.Save((string)null, InstrumentationConfigurationSection.SectionName, CreateInstrumentationSection());
        }
        [TestMethod]
        public void TryToSaveWithAFileConfigurationSaveParameter()
        {
            SystemConfigurationSource source = new SystemConfigurationSource();
            source.Add(new FileConfigurationParameter(file), InstrumentationConfigurationSection.SectionName, CreateInstrumentationSection());
            ValidateConfiguration(file);
        }
        [TestMethod]
        public void TryToSaveWithConfigurationMultipleTimes()
        {
            string tempFile = CreateFile();
            try
            {
                FileConfigurationSource.ResetImplementation(tempFile, false);
                IConfigurationSource source = new FileConfigurationSource(tempFile);
                source.Add(new FileConfigurationParameter(tempFile), InstrumentationConfigurationSection.SectionName, CreateInstrumentationSection());
                ValidateConfiguration(tempFile);
                source.Add(new FileConfigurationParameter(tempFile), InstrumentationConfigurationSection.SectionName, CreateInstrumentationSection());
                ValidateConfiguration(tempFile);
                source.Add(new FileConfigurationParameter(tempFile), InstrumentationConfigurationSection.SectionName, CreateInstrumentationSection());
                ValidateConfiguration(tempFile);
            }
            finally
            {
                if (File.Exists(tempFile)) File.Delete(tempFile);
            }
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TryToSaveWithTheWrongConfigurationSaveParameterTypeThrows()
        {
            SystemConfigurationSource source = new SystemConfigurationSource();
            source.Add(new WrongConfigurationSaveParameter(), InstrumentationConfigurationSection.SectionName, CreateInstrumentationSection());
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TryToSaveWithNullOrEmptySectionNameThrows()
        {
            SystemConfigurationSource source = new SystemConfigurationSource();
            source.Save(file, null, CreateInstrumentationSection());
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TryToSaveWithNullSectionThrows()
        {
            SystemConfigurationSource source = new SystemConfigurationSource();
            source.Save(file, InstrumentationConfigurationSection.SectionName, null);
        }
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void SaveConfigurationSectionWithNoConfigurationFileThrows()
        {
            SystemConfigurationSource source = new SystemConfigurationSource();
            source.Save("foo.exe.cofig", InstrumentationConfigurationSection.SectionName, CreateInstrumentationSection());
        }
        InstrumentationConfigurationSection CreateInstrumentationSection()
        {
            return new InstrumentationConfigurationSection(true, true, true, "fooApplicationName");
        }
        class WrongConfigurationSaveParameter : IConfigurationParameter {}
    }
}
