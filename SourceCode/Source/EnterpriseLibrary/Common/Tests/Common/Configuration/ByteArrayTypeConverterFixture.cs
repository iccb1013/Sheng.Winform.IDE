/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.ComponentModel;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SysConfig = System.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests
{
    [TestClass]
    public class ByteArrayConverterFixture
    {
        const string sectionName = "byteArrayConverter";
        [TestInitialize]
        public void TestInitialize()
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.Sections.Remove(sectionName);
            config.Save();
        }
        [TestMethod]
        public void SerializeAndDeserializeAByteArray()
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConverterSection section = new ConverterSection();
            section.ByteArray = new byte[] { 1, 2, 3, 4 };
            config.Sections.Add(sectionName, section);
            config.Save();
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            section = config.Sections[sectionName] as ConverterSection;
            Assert.IsNotNull(section);
            byte actual = 1;
            Assert.AreEqual(section.ByteArray[0], actual);
        }
        [TestMethod]
        public void SerializeAndDeserialzieANullByteArray()
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConverterSection section = new ConverterSection();
            section.ByteArray = null;
            config.Sections.Add(sectionName, section);
            config.Save();
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            section = config.Sections[sectionName] as ConverterSection;
            Assert.IsNotNull(section);
            Assert.IsNull(section.ByteArray);
        }
        public class ConverterSection : SerializableConfigurationSection
        {
            const string propertyName = "property";
            [ConfigurationProperty(propertyName)]
            [TypeConverter(typeof(ByteArrayTypeConverter))]
            public byte[] ByteArray
            {
                get { return (byte[])base[propertyName]; }
                set { base[propertyName] = value; }
            }
        }
    }
}
