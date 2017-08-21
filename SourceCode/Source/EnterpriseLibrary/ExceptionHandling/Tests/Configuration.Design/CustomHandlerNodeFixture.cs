/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestClass]
    public class CustomHandlerNodeFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInCustomHandlerNodeThrows()
        {
            new CustomHandlerNode(null);
        }
        [TestMethod]
        public void CustomHandlerNodeDefaults()
        {
            CustomHandlerNode customHandler = new CustomHandlerNode();
            Assert.AreEqual(0, customHandler.Attributes.Count);
            Assert.AreEqual("Custom Handler", customHandler.Name);
            Assert.IsTrue(string.IsNullOrEmpty(customHandler.Type));
        }
        [TestMethod]
        public void CustomHandlerDataTest()
        {
            string attributeKey = "attKey";
            string attributeValue = "attValue";
            string name = "some name";
            Type type = typeof(WrapHandlerNode);
            CustomHandlerData data = new CustomHandlerData();
            data.Name = name;
            data.Type = type;
            data.Attributes.Add(attributeKey, attributeValue);
            CustomHandlerNode node = new CustomHandlerNode(data);
            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(type.AssemblyQualifiedName, node.Type);
            Assert.AreEqual(attributeKey, node.Attributes[0].Key);
            Assert.AreEqual(attributeValue, node.Attributes[0].Value);
        }
        [TestMethod]
        public void CustomHandlerNodeDataTest()
        {
            string attributeKey = "attKey";
            string attributeValue = "attValue";
            string name = "some name";
            Type type = typeof(WrapHandlerNode);
            CustomHandlerData customHandlerData = new CustomHandlerData();
            customHandlerData.Attributes.Add(attributeKey, attributeValue);
            customHandlerData.Name = name;
            customHandlerData.Type = type;
            CustomHandlerNode customHandlerNode = new CustomHandlerNode(customHandlerData);
            CustomHandlerData nodeData = (CustomHandlerData)customHandlerNode.ExceptionHandlerData;
            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(type, nodeData.Type);
            Assert.AreEqual(attributeKey, nodeData.Attributes.AllKeys[0]);
            Assert.AreEqual(attributeValue, nodeData.Attributes[attributeKey]);
        }
    }
}
