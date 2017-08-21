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
    public class NotProcessedTraceSourceNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInNotProcessedNodeThrows()
        {
            new NotProcessedTraceSourceNode(null);
        }
        [TestMethod]
        public void NotProcessedTraceSourcNamePropertyIsReadOnly()
        {
            Assert.AreEqual(true, CommonUtil.IsPropertyReadOnly(typeof(NotProcessedTraceSourceNode), "Name"));
        }
        [TestMethod]
        public void NotProcessedTraceSourceDefaultDataTest()
        {
            NotProcessedTraceSourceNode notProcessedTraceSourcesNode = new NotProcessedTraceSourceNode(new TraceSourceData());
            ApplicationNode.AddNode(notProcessedTraceSourcesNode);
            Assert.AreEqual("Unprocessed Category", notProcessedTraceSourcesNode.Name);
            Assert.AreEqual(0, notProcessedTraceSourcesNode.Nodes.Count);
        }
    }
}
