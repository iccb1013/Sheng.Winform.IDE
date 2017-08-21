/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestClass]
    public class CustomFormatterNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInCustomFormatterNodeThrows()
        {
            new CustomFormatterNode(null);
        }
        [TestMethod]
        public void CustomFormatterNodeDefaults()
        {
            CustomFormatterNode customFormatter = new CustomFormatterNode();
            Assert.AreEqual("Custom Formatter", customFormatter.Name);
            Assert.IsTrue(String.IsNullOrEmpty(customFormatter.Type));
        }
        [TestMethod]
        public void CustomFormatterDataTest()
        {
            string attributeKey = "attKey";
            string attributeValue = "attValue";
            string name = "some name";
            Type type = typeof(BinaryLogFormatter);
            CustomFormatterData data = new CustomFormatterData();
            data.Name = name;
            data.Type = type;
            data.Attributes.Add(attributeKey, attributeValue);
            CustomFormatterNode node = new CustomFormatterNode(data);
            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(type.AssemblyQualifiedName, node.Type);
            Assert.AreEqual(attributeKey, node.Attributes[0].Key);
            Assert.AreEqual(attributeValue, node.Attributes[0].Value);
        }
        [TestMethod]
        public void CustomFormatterNodeTest()
        {
            string attributeKey = "attKey";
            string attributeValue = "attValue";
            string name = "some name";
            Type type = typeof(BinaryLogFormatter);
            CustomFormatterNode formatterNode = new CustomFormatterNode();
            formatterNode.Attributes.Add(new EditableKeyValue(attributeKey, attributeValue));
            formatterNode.Name = name;
            formatterNode.Type = type.AssemblyQualifiedName;
            CustomFormatterData nodeData = (CustomFormatterData)formatterNode.FormatterData;
            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(type, nodeData.Type);
            Assert.AreEqual(attributeKey, nodeData.Attributes.AllKeys[0]);
            Assert.AreEqual(attributeValue, nodeData.Attributes[attributeKey]);
        }
    }
}
