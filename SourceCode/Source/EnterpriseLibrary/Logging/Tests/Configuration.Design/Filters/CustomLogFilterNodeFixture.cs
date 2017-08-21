/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestClass]
    public class CustomLogFilterNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInCustomLogFilterNodeThrows()
        {
            new CustomLogFilterNode(null);
        }
        [TestMethod]
        public void CustomLogFilterNodeDefaults()
        {
            CustomLogFilterNode customLogFilter = new CustomLogFilterNode();
            Assert.AreEqual("Custom Filter", customLogFilter.Name);
            Assert.IsTrue(String.IsNullOrEmpty(customLogFilter.Type));
        }
        [TestMethod]
        public void CustomLogFilterDataTest()
        {
            string attributeKey = "attKey";
            string attributeValue = "attValue";
            string name = "some name";
            Type type = typeof(LogEnabledFilter);
            CustomLogFilterData data = new CustomLogFilterData();
            data.Name = name;
            data.Type = type;
            data.Attributes.Add(attributeKey, attributeValue);
            CustomLogFilterNode node = new CustomLogFilterNode(data);
            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(type.AssemblyQualifiedName, node.Type);
            Assert.AreEqual(attributeKey, node.Attributes[0].Key);
            Assert.AreEqual(attributeValue, node.Attributes[0].Value);
        }
        [TestMethod]
        public void CustomLogFilterNodeTest()
        {
            string attributeKey = "attKey";
            string attributeValue = "attValue";
            string name = "some name";
            Type type = typeof(LogEnabledFilter);
            CustomLogFilterNode node = new CustomLogFilterNode();
            node.Name = name;
            node.Attributes.Add(new EditableKeyValue(attributeKey, attributeValue));
            node.Type = type.AssemblyQualifiedName;
            CustomLogFilterData nodeData = (CustomLogFilterData)node.LogFilterData;
            Assert.AreEqual(type, nodeData.Type);
            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(attributeKey, nodeData.Attributes.AllKeys[0]);
            Assert.AreEqual(attributeValue, nodeData.Attributes[attributeKey]);
        }
    }
}
