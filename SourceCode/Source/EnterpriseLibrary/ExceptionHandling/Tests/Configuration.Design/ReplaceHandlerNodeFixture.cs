/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Tests
{
    [TestClass]
    public class ReplaceHandlerNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInReplaceHandlerNodeThrows()
        {
            new ReplaceHandlerNode(null);
        }
        [TestMethod]
        public void ReplaceHandlerNodeDefaults()
        {
            ReplaceHandlerNode replaceHandler = new ReplaceHandlerNode();
            Assert.AreEqual(string.Empty, replaceHandler.ExceptionMessage);
            Assert.AreEqual(string.Empty, replaceHandler.ReplaceExceptionType);
            Assert.AreEqual("Replace Handler", replaceHandler.Name);
            Assert.AreEqual(string.Empty, replaceHandler.ExceptionMessageResourceName);
            Assert.AreEqual(string.Empty, replaceHandler.ExceptionMessageResourceType);
        }
        [TestMethod]
        public void ReplaceHandlerDataTest()
        {
            string name = "some name";
            string message = "some message";
            Type replaceExceptionType = typeof(ApplicationException);
            string resourceName = "resource name";
            string resourceType = "resource type";
            ReplaceHandlerData data = new ReplaceHandlerData();
            data.Name = name;
            data.ExceptionMessage = message;
            data.ReplaceExceptionType = replaceExceptionType;
            data.ExceptionMessageResourceName = resourceName;
            data.ExceptionMessageResourceType = resourceType;
            ReplaceHandlerNode node = new ReplaceHandlerNode(data);
            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(message, node.ExceptionMessage);
            Assert.AreEqual(replaceExceptionType.AssemblyQualifiedName, node.ReplaceExceptionType);
            Assert.AreEqual(resourceName, node.ExceptionMessageResourceName);
            Assert.AreEqual(resourceType, node.ExceptionMessageResourceType);
        }
        [TestMethod]
        public void ReplaceHandlerNodeDataTest()
        {
            string name = "some name";
            string message = "some message";
            Type replaceExceptionType = typeof(ApplicationException);
            string resourceName = "resource name";
            string resourceType = "resource type";
            ReplaceHandlerData replaceHandlerData = new ReplaceHandlerData();
            replaceHandlerData.Name = name;
            replaceHandlerData.ExceptionMessage = message;
            replaceHandlerData.ReplaceExceptionType = replaceExceptionType;
            replaceHandlerData.ExceptionMessageResourceName = resourceName;
            replaceHandlerData.ExceptionMessageResourceType = resourceType;
            ReplaceHandlerNode replaceHandlerNode = new ReplaceHandlerNode(replaceHandlerData);
            ReplaceHandlerData nodeData = (ReplaceHandlerData)replaceHandlerNode.ExceptionHandlerData;
            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(message, nodeData.ExceptionMessage);
            Assert.AreEqual(replaceExceptionType, nodeData.ReplaceExceptionType);
            Assert.AreEqual(resourceName, nodeData.ExceptionMessageResourceName);
            Assert.AreEqual(resourceType, nodeData.ExceptionMessageResourceType);
        }
    }
}
