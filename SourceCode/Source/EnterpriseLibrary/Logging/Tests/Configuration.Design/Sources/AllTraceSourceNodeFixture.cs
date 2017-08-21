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
    public class AllTraceSourceNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInAllSourcesNodeThrows()
        {
            new AllTraceSourceNode(null);
        }
        [TestMethod]
        public void AllTraceSourceNamePropertyIsReadOnly()
        {
            Assert.AreEqual(true, CommonUtil.IsPropertyReadOnly(typeof(AllTraceSourceNode), "Name"));
        }
        [TestMethod]
        public void AllTraceSourcesDefaultDataTest()
        {
            AllTraceSourceNode allTraceSourcesNode = new AllTraceSourceNode(new TraceSourceData());
            ApplicationNode.AddNode(allTraceSourcesNode);
            Assert.AreEqual("All Events", allTraceSourcesNode.Name);
            Assert.AreEqual(0, allTraceSourcesNode.Nodes.Count);
        }
    }
}
