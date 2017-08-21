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
    public class WrapHandlerNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInWrapHandlerNodeThrows()
        {
            new WrapHandlerNode(null);
        }
        [TestMethod]
        public void WrapHandlerNodeDefaults()
        {
            WrapHandlerNode wrapHandler = new WrapHandlerNode();
            Assert.AreEqual(string.Empty, wrapHandler.ExceptionMessage);
            Assert.IsTrue(string.IsNullOrEmpty(wrapHandler.WrapExceptionType));
            Assert.AreEqual("Wrap Handler", wrapHandler.Name);
            Assert.AreEqual(string.Empty, wrapHandler.ExceptionMessageResourceName);
            Assert.AreEqual(string.Empty, wrapHandler.ExceptionMessageResourceType);
        }
        [TestMethod]
        public void WrapHandlerDataTest()
        {
            string name = "some name";
            string message = "some message";
            Type exceptionType = typeof(AppDomainUnloadedException);
            Type wrapExceptionType = typeof(ApplicationException);
            string resourceName = "resource name";
            string resourceType = "resource type";
            WrapHandlerData data = new WrapHandlerData();
            data.Name = name;
            data.ExceptionMessage = message;
            data.Type = exceptionType;
            data.WrapExceptionType = wrapExceptionType;
            data.ExceptionMessageResourceName = resourceName;
            data.ExceptionMessageResourceType = resourceType;
            WrapHandlerNode node = new WrapHandlerNode(data);
            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(message, node.ExceptionMessage);
            Assert.AreEqual(wrapExceptionType.AssemblyQualifiedName, node.WrapExceptionType);
            Assert.AreEqual(resourceName, node.ExceptionMessageResourceName);
            Assert.AreEqual(resourceType, node.ExceptionMessageResourceType);
        }
        [TestMethod]
        public void WrapHandlerNodeDataTest()
        {
            string name = "some name";
            string message = "some message";
            Type wrapExceptionType = typeof(ApplicationException);
            string resourceName = "resource name";
            string resourceType = "resource type";
            WrapHandlerData wrapHandlerData = new WrapHandlerData();
            wrapHandlerData.Name = name;
            wrapHandlerData.ExceptionMessage = message;
            wrapHandlerData.WrapExceptionType = wrapExceptionType;
            wrapHandlerData.ExceptionMessageResourceName = resourceName;
            wrapHandlerData.ExceptionMessageResourceType = resourceType;
            WrapHandlerNode wrapHandlerNode = new WrapHandlerNode(wrapHandlerData);
            WrapHandlerData nodeData = (WrapHandlerData)wrapHandlerNode.ExceptionHandlerData;
            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(message, nodeData.ExceptionMessage);
            Assert.AreEqual(wrapExceptionType, nodeData.WrapExceptionType);
            Assert.AreEqual(resourceName, nodeData.ExceptionMessageResourceName);
            Assert.AreEqual(resourceType, nodeData.ExceptionMessageResourceType);
        }
    }
}
