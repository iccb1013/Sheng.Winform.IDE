/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration
{
    [TestClass]
    public class NameTypeConfigurationElementCollectionFixture
    {
        [TestMethod]
        public void CanReadConfigurationElementsFromCollectionWithoutOverrides()
        {
            PolymorphicConfigurationElementCollectionTestSection section
                = ConfigurationManager.GetSection("withoutclear") as PolymorphicConfigurationElementCollectionTestSection;
            NameTypeConfigurationElementCollection<BasePolymorphicObjectData, CustomPolymorphicObjectData> elements = section.WithoutOverrides;
            Assert.AreEqual(3, elements.Count);
            Assert.AreSame(typeof(DerivedPolymorphicObject1Data), elements.Get("provider1a").GetType());
            Assert.AreSame(typeof(DerivedPolymorphicObject2Data), elements.Get("provider2").GetType());
            Assert.AreSame(typeof(DerivedPolymorphicObject1Data), elements.Get("provider1b").GetType());
        }
        [TestMethod]
        public void CanReadConfigurationElementsFromCollectionWithOverrides()
        {
            PolymorphicConfigurationElementCollectionTestSection section
                = ConfigurationManager.GetSection("withoutclear") as PolymorphicConfigurationElementCollectionTestSection;
            NameTypeConfigurationElementCollection<BasePolymorphicObjectData, CustomPolymorphicObjectData> elements = section.WithOverrides;
            Assert.AreEqual(3, elements.Count);
            Assert.AreSame(typeof(DerivedPolymorphicObject1Data), elements.Get("overrideprovider1a").GetType());
            Assert.AreSame(typeof(DerivedPolymorphicObject2Data), elements.Get("overrideprovider2").GetType());
            Assert.AreSame(typeof(DerivedPolymorphicObject1Data), elements.Get("overrideprovider1b").GetType());
        }
        [TestMethod]
        public void CanReadConfigurationElementsWithClearFromCollectionWithoutOverrides()
        {
            PolymorphicConfigurationElementCollectionTestSection section
                = ConfigurationManager.GetSection("withclear") as PolymorphicConfigurationElementCollectionTestSection;
            NameTypeConfigurationElementCollection<BasePolymorphicObjectData, CustomPolymorphicObjectData> elements = section.WithoutOverrides;
            Assert.AreEqual(2, elements.Count);
            Assert.AreSame(typeof(DerivedPolymorphicObject2Data), elements.Get("provider2").GetType());
            Assert.AreSame(typeof(DerivedPolymorphicObject1Data), elements.Get("provider1b").GetType());
        }
        [TestMethod]
        public void CanReadConfigurationElementsWithClearFromCollectionWithOverrides()
        {
            PolymorphicConfigurationElementCollectionTestSection section
                = ConfigurationManager.GetSection("withclear") as PolymorphicConfigurationElementCollectionTestSection;
            NameTypeConfigurationElementCollection<BasePolymorphicObjectData, CustomPolymorphicObjectData> elements = section.WithOverrides;
            Assert.AreEqual(2, elements.Count);
            Assert.AreSame(typeof(DerivedPolymorphicObject2Data), elements.Get("overrideprovider2").GetType());
            Assert.AreSame(typeof(DerivedPolymorphicObject1Data), elements.Get("overrideprovider1b").GetType());
        }
    }
}
