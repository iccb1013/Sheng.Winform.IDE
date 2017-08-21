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
    public class ExceptionTypeNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInExceptionTypeNodeThrows()
        {
            new ExceptionTypeNode(null);
        }
        [TestMethod]
        public void ExceptionTypeNodeHasReadonlyName()
        {
            Assert.IsTrue(CommonUtil.IsPropertyReadOnly(typeof(ExceptionTypeNode), "Name"));
        }
        [TestMethod]
        public void ExceptionTypeNodeDefaults()
        {
            ExceptionTypeNode exceptionTypeNode = new ExceptionTypeNode();
            Assert.AreEqual("Exception", exceptionTypeNode.Name);
            Assert.AreEqual(PostHandlingAction.NotifyRethrow, exceptionTypeNode.PostHandlingAction);
        }
        [TestMethod]
        public void ExceptionTypeNameBecomesName()
        {
            Type exceptionType = typeof(AppDomainUnloadedException);
            string name = "some name";
            ExceptionTypeData data = new ExceptionTypeData();
            data.Type = exceptionType;
            data.Name = name;
            ExceptionTypeNode node = new ExceptionTypeNode(data);
            Assert.AreEqual(exceptionType.Name, node.Name);
        }
        [TestMethod]
        public void ExceptionTypeDataTest()
        {
            Type exceptionType = typeof(AppDomainUnloadedException);
            PostHandlingAction action = PostHandlingAction.None;
            ExceptionTypeData data = new ExceptionTypeData();
            data.Type = exceptionType;
            data.PostHandlingAction = action;
            ExceptionTypeNode node = new ExceptionTypeNode(data);
            Assert.AreEqual(exceptionType.Name, node.Name);
            Assert.AreEqual(action, node.PostHandlingAction);
        }
    }
}
