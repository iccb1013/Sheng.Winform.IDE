/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Design.Tests
{
    [TestClass]
    public class FaultContractExceptionHandlerNodeFixture
    {
        [TestMethod]
        public void CreatedNodeHasAppropriateDefaultValues()
        {
            FaultContractExceptionHandlerNode node = new FaultContractExceptionHandlerNode();
            Assert.AreEqual(string.Empty, node.FaultContractType);
            Assert.AreEqual(string.Empty, node.ExceptionMessage);
        }
        [TestMethod]
        public void CreatedNodeWithValidatorDataHasAppropriateValuesFromData()
        {
            FaultContractExceptionHandlerData data = new FaultContractExceptionHandlerData("name");
            data.FaultContractType = typeof(object).AssemblyQualifiedName;
            data.ExceptionMessage = "my exception message";
            data.PropertyMappings.Add(new FaultContractExceptionHandlerMappingData("property", "source"));
            FaultContractExceptionHandlerNode node = new FaultContractExceptionHandlerNode(data);
            Assert.AreEqual(typeof(object).AssemblyQualifiedName, node.FaultContractType);
            Assert.AreEqual("my exception message", node.ExceptionMessage);
            Assert.AreEqual(1, node.PropertyMappings.Count);
            Assert.AreEqual("property", node.PropertyMappings[0].Name);
            Assert.AreEqual("source", node.PropertyMappings[0].Source);
        }
        [TestMethod]
        public void NodeCreatesValidatorDataWithValues()
        {
            FaultContractExceptionHandlerNode node = new FaultContractExceptionHandlerNode();
            node.Name = "faultContract";
            node.FaultContractType = typeof(object).AssemblyQualifiedName;
            node.ExceptionMessage = "my exception message";
            FaultContractPropertyMapping mapping1 = new FaultContractPropertyMapping();
            mapping1.Name = "property1";
            mapping1.Source = "source1";
            node.PropertyMappings.Add(mapping1);
            FaultContractPropertyMapping mapping2 = new FaultContractPropertyMapping();
            mapping2.Name = "property2";
            mapping2.Source = "source2";
            node.PropertyMappings.Add(mapping2);
            FaultContractExceptionHandlerData data = node.ExceptionHandlerData as FaultContractExceptionHandlerData;
            Assert.IsNotNull(data);
            Assert.AreEqual("faultContract", data.Name);
            Assert.AreEqual(typeof(object).AssemblyQualifiedName, data.FaultContractType);
            Assert.AreEqual("my exception message", data.ExceptionMessage);
            Assert.AreEqual(2, data.PropertyMappings.Count);
            Assert.AreEqual("source1", data.PropertyMappings.Get("property1").Source);
            Assert.AreEqual("source2", data.PropertyMappings.Get("property2").Source);
        }
    }
}
