/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestClass]
    public class WmiTraceListenerNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInWmiTraceListenerNodeThrows()
        {
            new WmiTraceListenerNode(null);
        }
        [TestMethod]
        public void WmiTraceListenerNodeDefaults()
        {
            WmiTraceListenerNode wmiListener = new WmiTraceListenerNode();
            Assert.AreEqual("WMI TraceListener", wmiListener.Name);
            Assert.AreEqual(SourceLevels.All, wmiListener.Filter);
        }
        [TestMethod]
        public void WmiTraceListenerNodeTest()
        {
            string name = "some name";
            SourceLevels filter = SourceLevels.Critical;
            WmiTraceListenerNode wmiTraceListenerNode = new WmiTraceListenerNode();
            wmiTraceListenerNode.Name = name;
            wmiTraceListenerNode.Filter = filter;
            ApplicationNode.AddNode(wmiTraceListenerNode);
            WmiTraceListenerData nodeData = (WmiTraceListenerData)wmiTraceListenerNode.TraceListenerData;
            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(filter, nodeData.Filter);
        }
        [TestMethod]
        public void WmiTraceListenerNodeDataTest()
        {
            string name = "some name";
            SourceLevels filter = SourceLevels.Critical;
            WmiTraceListenerData wmiTraceListenerData = new WmiTraceListenerData();
            wmiTraceListenerData.Name = name;
            wmiTraceListenerData.Filter = filter;
            WmiTraceListenerNode wmiTraceListenerNode = new WmiTraceListenerNode(wmiTraceListenerData);
            ApplicationNode.AddNode(wmiTraceListenerNode);
            Assert.AreEqual(name, wmiTraceListenerNode.Name);
            Assert.AreEqual(filter, wmiTraceListenerNode.Filter);
        }
    }
}
