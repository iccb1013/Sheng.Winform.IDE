/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    [TestClass]
    public class ContextItemsFixture
    {
        [TestInitialize]
        public void Setup()
        {
            Logger.Reset();
            Logger.FlushContextItems();
        }
        [TestCleanup]
        public void Teardown()
        {
            Logger.FlushContextItems();
            MockTraceListener.Reset();
        }
        [TestMethod]
        public void AddItemToCallContext()
        {
            ContextItems item = new ContextItems();
            item.SetContextItem("AppVersion", "1234");
            Hashtable hash = (Hashtable)CallContext.GetData(ContextItems.CallContextSlotName);
            Assert.IsNotNull(hash);
            Assert.AreEqual(1, hash.Count);
            Assert.AreEqual("1234", hash["AppVersion"]);
        }
        [TestMethod]
        public void FlushItemsFromCallContext()
        {
            ContextItems item = new ContextItems();
            item.SetContextItem("AppVersion", "1234");
            Hashtable hash = (Hashtable)CallContext.GetData(ContextItems.CallContextSlotName);
            Assert.IsNotNull(hash);
            Assert.AreEqual(1, hash.Count);
            item.FlushContextItems();
            Hashtable hash2 = (Hashtable)CallContext.GetData(ContextItems.CallContextSlotName);
            Assert.IsNull(hash2);
        }
        [TestMethod]
        public void AddItemThenLog()
        {
            Logger.SetContextItem("AppVersion", "1234");
            LogEntry log = new LogEntry();
            log.Categories = new string[] { "MockCategoryOne" };
            Logger.Write(log);
            Assert.AreEqual(1, ((LogEntry)MockTraceListener.LastEntry).ExtendedProperties.Count);
            Assert.AreEqual("1234", ((LogEntry)MockTraceListener.LastEntry).ExtendedProperties["AppVersion"]);
        }
        [TestMethod]
        public void AddTwoItemsThenLog()
        {
            Logger.SetContextItem("AppVersion", "1234");
            Logger.SetContextItem("BuildNumber", "5678");
            LogEntry log = new LogEntry();
            log.Categories = new string[] { "MockCategoryOne" };
            Logger.Write(log);
            Assert.AreEqual(2, ((LogEntry)MockTraceListener.LastEntry).ExtendedProperties.Count);
            Assert.AreEqual("1234", ((LogEntry)MockTraceListener.LastEntry).ExtendedProperties["AppVersion"]);
            Assert.AreEqual("5678", ((LogEntry)MockTraceListener.LastEntry).ExtendedProperties["BuildNumber"]);
        }
        [TestMethod]
        public void AddItemsAndDictionaryThenLog()
        {
            Logger.SetContextItem("AppVersion", "1234");
            Logger.SetContextItem("BuildNumber", "5678");
            LogEntry log = new LogEntry();
            log.Categories = new string[] { "MockCategoryOne" };
            log.ExtendedProperties = CommonUtil.GetPropertiesDictionary();
            Logger.Write(log);
            Assert.AreEqual(5, ((LogEntry)MockTraceListener.LastEntry).ExtendedProperties.Count);
            Assert.AreEqual("1234", ((LogEntry)MockTraceListener.LastEntry).ExtendedProperties["AppVersion"]);
            Assert.AreEqual("5678", ((LogEntry)MockTraceListener.LastEntry).ExtendedProperties["BuildNumber"]);
        }
        [TestMethod]
        public void FlushItems()
        {
            Logger.SetContextItem("AppVersion", "1234");
            Logger.SetContextItem("BuildNumber", "5678");
            LogEntry log = new LogEntry();
            log.Categories = new string[] { "MockCategoryOne" };
            log.ExtendedProperties = CommonUtil.GetPropertiesDictionary();
            Logger.FlushContextItems();
            Logger.Write(log);
            Assert.AreEqual(3, ((LogEntry)MockTraceListener.LastEntry).ExtendedProperties.Count);
            Assert.IsFalse(((LogEntry)MockTraceListener.LastEntry).ExtendedProperties.ContainsKey("AppVersion"));
            Assert.IsFalse(((LogEntry)MockTraceListener.LastEntry).ExtendedProperties.ContainsKey("BuildNumber"));
        }
        [TestMethod]
        public void AddObjectAsContextItem()
        {
            ContextObject obj = new ContextObject();
            Logger.SetContextItem("object", obj);
            LogEntry log = CommonUtil.GetDefaultLogEntry();
            log.Categories = new string[] { "MockCategoryOne" };
            log.Severity = TraceEventType.Error;
            Logger.Write(log);
            string result = ((LogEntry)MockTraceListener.LastEntry).ExtendedProperties["object"].ToString();
            string expected = obj.ToString();
            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void CustomContextByteArrayObject()
        {
            Guid expectedGuid = Guid.NewGuid();
            byte[] byteArray = expectedGuid.ToByteArray();
            Logger.SetContextItem("bytes", byteArray);
            LogEntry log = CommonUtil.GetDefaultLogEntry();
            log.Categories = new string[] { "MockCategoryOne" };
            log.Severity = TraceEventType.Error;
            Logger.Write(log);
            string guidArray = ((LogEntry)MockTraceListener.LastEntry).ExtendedProperties["bytes"].ToString();
            byteArray = Convert.FromBase64String(guidArray);
            Guid resultGuid = new Guid(byteArray);
            Assert.AreEqual(expectedGuid, resultGuid);
        }
        internal class ContextObject
        {
            public override string ToString()
            {
                return "Scooby Doo Loves You";
            }
        }
    }
}
