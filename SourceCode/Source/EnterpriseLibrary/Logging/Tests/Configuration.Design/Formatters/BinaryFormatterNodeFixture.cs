/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestClass]
    public class BinaryFormatterNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInBinaryFormatterNodeThrows()
        {
            new BinaryFormatterNode(null);
        }
        [TestMethod]
        public void BinaryFormatterNodeDefaults()
        {
            BinaryFormatterNode binaryFormatter = new BinaryFormatterNode();
            Assert.AreEqual("Binary Formatter", binaryFormatter.Name);
        }
        [TestMethod]
        public void BinaryFormatterDataTest()
        {
            string name = "some name";
            BinaryLogFormatterData data = new BinaryLogFormatterData();
            data.Name = name;
            BinaryFormatterNode node = new BinaryFormatterNode(data);
            Assert.AreEqual(name, node.Name);
        }
        [TestMethod]
        public void BinaryFormatterNodeTest()
        {
            string name = "some name";
            BinaryFormatterNode formatterNode = new BinaryFormatterNode();
            formatterNode.Name = name;
            FormatterData nodeData = formatterNode.FormatterData;
            Assert.AreEqual(name, nodeData.Name);
        }
    }
}
