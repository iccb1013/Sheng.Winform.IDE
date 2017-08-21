/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests.Sources
{
    [TestClass]
    public class ErrorsTraceSourceNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInErrorsTraceSourceNodeThrows()
        {
            new ErrorsTraceSourceNode(null);
        }
        [TestMethod]
        public void ErrorsTraceSourcNamePropertyIsReadOnly()
        {
            Assert.AreEqual(true, CommonUtil.IsPropertyReadOnly(typeof(ErrorsTraceSourceNode), "Name"));
        }
        [TestMethod]
        public void ErrorsTraceSourceDefaultDataTest()
        {
            ErrorsTraceSourceNode errorsTraceSourcesNode = new ErrorsTraceSourceNode(new TraceSourceData());
            ApplicationNode.AddNode(errorsTraceSourcesNode);
            Assert.AreEqual("Logging Errors & Warnings", errorsTraceSourcesNode.Name);
            Assert.AreEqual(0, errorsTraceSourcesNode.Nodes.Count);
        }
    }
}
