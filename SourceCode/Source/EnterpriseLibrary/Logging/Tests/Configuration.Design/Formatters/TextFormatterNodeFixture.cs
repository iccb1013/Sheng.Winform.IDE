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
    public class TextFormatterNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInTextFormatterNodeThrows()
        {
            new TextFormatterNode(null);
        }
        [TestMethod]
        public void TextFormatterDataTest()
        {
            string name = "some name";
            string template = "template";
            TextFormatterData data = new TextFormatterData();
            data.Name = name;
            data.Template = template;
            TextFormatterNode node = new TextFormatterNode(data);
            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(template, node.Template.Text);
        }
        [TestMethod]
        public void TextFormatterNodeTest()
        {
            string name = "some name";
            string template = "template";
            TextFormatterNode formatterNode = new TextFormatterNode();
            formatterNode.Name = name;
            formatterNode.Template = new Template(template);
            TextFormatterData nodeData = (TextFormatterData)formatterNode.FormatterData;
            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(template, nodeData.Template);
        }
    }
}
