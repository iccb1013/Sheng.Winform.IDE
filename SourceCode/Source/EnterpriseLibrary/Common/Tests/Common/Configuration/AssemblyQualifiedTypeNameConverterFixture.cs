/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.ComponentModel;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SysConfig = System.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests
{
    [TestClass]
    public class AssemblyQualifiedTypeNameConverterFixture
    {
        const string sectionName = "assemblyConverter";
        const string badSectionName = "badAssemblyConverter";
        [TestInitialize]
        public void TestInitialize()
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.Sections.Remove(sectionName);
            config.Save();
        }
        [TestMethod]
        public void SerializeAndDeserializeAType()
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConverterSection section = new ConverterSection();
            section.Type = typeof(Exception);
            config.Sections.Add(sectionName, section);
            config.Save();
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            section = config.Sections[sectionName] as ConverterSection;
            Assert.IsNotNull(section);
            Assert.AreEqual(section.Type, typeof(Exception));
        }
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void SerializeABadTypeThrows()
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            BadConverterSection section = new BadConverterSection();
            section.BadConverter = new Exception();
            config.Sections.Add(sectionName, section);
            config.Save();
        }
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void DeserialzieABadTypeThrows()
        {
            ConfigurationManager.RefreshSection(badSectionName);
            BadNameSection section = (BadNameSection)ConfigurationManager.GetSection(badSectionName);
            Type t = section.Type;
        }
        public class ConverterSection : SerializableConfigurationSection
        {
            const string typeProperty = "type";
            [ConfigurationProperty(typeProperty)]
            [TypeConverter(typeof(AssemblyQualifiedTypeNameConverter))]
            public Type Type
            {
                get { return (Type)base[typeProperty]; }
                set { base[typeProperty] = value; }
            }
        }
        public class BadConverterSection : SerializableConfigurationSection
        {
            const string typeProperty = "type";
            [ConfigurationProperty(typeProperty)]
            [TypeConverter(typeof(AssemblyQualifiedTypeNameConverter))]
            public Exception BadConverter
            {
                get { return (Exception)base[typeProperty]; }
                set { base[typeProperty] = value; }
            }
        }
    }
    public class BadNameSection : SerializableConfigurationSection
    {
        const string typeProperty = "type";
        [ConfigurationProperty(typeProperty, IsRequired= true)]
        [TypeConverter(typeof(AssemblyQualifiedTypeNameConverter))]
        public Type Type
        {
            get { return (Type)base[typeProperty]; }
            set { base[typeProperty] = value; }
        }
    }
}
