/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder.Tests
{
    [TestClass]
    public class InjectionAttributesFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConfigurationNameMapperAttributeWithNullTypeThrows()
        {
            ConfigurationNameMapperAttribute attribute = new ConfigurationNameMapperAttribute(null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConfigurationNameMapperAttributeWithIncompatibleTypeThrows()
        {
            ConfigurationNameMapperAttribute attribute = new ConfigurationNameMapperAttribute(typeof(string));
        }
        [TestMethod]
        public void ConfigurationNameMapperAttributeWithCompatibleTypeWorks()
        {
            ConfigurationNameMapperAttribute attribute = new ConfigurationNameMapperAttribute(typeof(TestConfigurationNameMapper));
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AssemblerAttributeWithNullTypeThrows()
        {
            AssemblerAttribute attribute = new AssemblerAttribute(null);
        }
        [TestMethod]
        public void AssemblerAttributeWithCompatibleTypeWorks()
        {
            AssemblerAttribute attribute = new AssemblerAttribute(typeof(MockAssembler));
        }
        [TestMethod]
        public void CustomFactoryWithCompatibleTypeWorks()
        {
            CustomFactoryAttribute attribute = new CustomFactoryAttribute(typeof(MockImplementor));
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CustomFactoryWithIncompatibleTypeThrows()
        {
            CustomFactoryAttribute attribute = new CustomFactoryAttribute(typeof(object));
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CustomFactoryWithNullTypeThrows()
        {
            CustomFactoryAttribute attribute = new CustomFactoryAttribute(null);
        }
    }
    public class TestConfigurationNameMapper : IConfigurationNameMapper
    {
        public string MapName(string name,
                              IConfigurationSource configSource)
        {
            return null;
        }
    }
}
