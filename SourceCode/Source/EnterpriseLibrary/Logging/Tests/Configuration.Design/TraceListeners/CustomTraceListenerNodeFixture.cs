/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestClass]
    public class CustomTraceListenerNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInCustomTraceListenerNodeThrows()
        {
            new CustomTraceListenerNode(null);
        }
        [TestMethod]
        public void CustomTraceListenerNodeDefaults()
        {
            CustomTraceListenerNode customListener = new CustomTraceListenerNode();
            Assert.AreEqual("Custom Trace Listener", customListener.Name);
            Assert.IsNull(customListener.Type);
        }
        [TestMethod]
        public void CustomTraceListenerDataTest()
        {
            string attributeKey = "attKey";
            string attributeValue = "attValue";
            string name = "some name";
            Type type = typeof(FormattedEventLogTraceListener);
            CustomTraceListenerData data = new CustomTraceListenerData();
            data.Name = name;
            data.Type = type;
            data.Attributes.Add(attributeKey, attributeValue);
            CustomTraceListenerNode node = new CustomTraceListenerNode(data);
            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(type.AssemblyQualifiedName, node.Type);
            Assert.AreEqual(attributeKey, node.Attributes[0].Key);
            Assert.AreEqual(attributeValue, node.Attributes[0].Value);
        }
        [TestMethod]
        public void CustomTraceListenerNodeTest()
        {
            string attributeKey = "attKey";
            string attributeValue = "attValue";
            string name = "some name";
            Type type = typeof(FormattedEventLogTraceListener);
            CustomTraceListenerNode customTraceListenerNode = new CustomTraceListenerNode();
            customTraceListenerNode.Attributes.Add(new EditableKeyValue(attributeKey, attributeValue));
            customTraceListenerNode.Name = name;
            customTraceListenerNode.Type = type.AssemblyQualifiedName;
            ApplicationNode.AddNode(customTraceListenerNode);
            CustomTraceListenerData nodeData = (CustomTraceListenerData)customTraceListenerNode.TraceListenerData;
            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(type, nodeData.Type);
            Assert.AreEqual(attributeKey, nodeData.Attributes.AllKeys[0]);
            Assert.AreEqual(attributeValue, nodeData.Attributes[attributeKey]);
        }
        [TestMethod]
        public void CustomTraceListenerNodeDataTest()
        {
            string attributeKey = "attKey";
            string attributeValue = "attValue";
            string name = "some name";
            Type type = typeof(FormattedEventLogTraceListener);
            CustomTraceListenerData customTraceListenerData = new CustomTraceListenerData();
            customTraceListenerData.Attributes.Add(attributeKey, attributeValue);
            customTraceListenerData.Name = name;
            customTraceListenerData.Type = type;
            CustomTraceListenerNode customTraceListenerNode = new CustomTraceListenerNode(customTraceListenerData);
            ApplicationNode.AddNode(customTraceListenerNode);
            CustomTraceListenerData nodeData = (CustomTraceListenerData)customTraceListenerNode.TraceListenerData;
            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(type, nodeData.Type);
            Assert.AreEqual(attributeKey, nodeData.Attributes.AllKeys[0]);
            Assert.AreEqual(attributeValue, nodeData.Attributes[attributeKey]);
        }
    }
}
