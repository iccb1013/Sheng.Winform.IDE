/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design.Tests
{
    [TestClass]
    public class LoggingExceptionHandlerNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInLoggingExceptionHandlerNodehrows()
        {
            new LoggingExceptionHandlerNode(null);
        }
        [TestMethod]
        public void LoggingExceptionHandlerNodeDefaults()
        {
            LoggingExceptionHandlerNode policyNode = new LoggingExceptionHandlerNode();
            Assert.AreEqual("Logging Handler", policyNode.Name);
        }
        [TestMethod]
        public void CanCreateLoggingHandlerNodeFromData()
        {
            ConfigurationNode createdNode = ServiceHelper.GetNodeCreationService(ServiceProvider).CreateNodeByDataType(typeof(LoggingExceptionHandlerData), new object[] { new LoggingExceptionHandlerData() });
            Assert.IsNotNull(createdNode);
        }
        [TestMethod]
        public void LoggingHandlerNodeDataTest()
        {
            string name = "some name";
            int eventId = 3234;
            int priority = -1;
            Type formatterType = typeof(Guid);
            TraceEventType severity = TraceEventType.Resume;
            LoggingExceptionHandlerData data = new LoggingExceptionHandlerData();
            data.Name = name;
            data.EventId = eventId;
            data.FormatterType = formatterType;
            data.Priority = priority;
            data.Severity = severity;
            data.Title = "Title";
            data.LogCategory = "Category1";
            data.UseDefaultLogger = false;
            LoggingExceptionHandlerNode node = new LoggingExceptionHandlerNode(data);
            node.LogCategory = new CategoryTraceSourceNode();
            node.LogCategory.Name = "Category1";
            Assert.AreEqual(data.Name, node.Name);
            Assert.AreEqual(data.EventId, node.EventId);
            Assert.AreEqual(formatterType.AssemblyQualifiedName, node.FormatterType);
            Assert.AreEqual(data.Priority, node.Priority);
            Assert.AreEqual(data.Severity, node.Severity);
            Assert.AreEqual(data.Title, node.Title);
            Assert.AreEqual(data.LogCategory, node.LogCategory.Name);
            Assert.AreEqual(data.UseDefaultLogger, node.UseDefaultLogger);
        }
        [TestMethod]
        public void LoggingHandlerNodeTest()
        {
            string name = "some name";
            int eventId = 3234;
            int priority = -1;
            Type formatterType = typeof(Guid);
            TraceEventType severity = TraceEventType.Resume;
            bool useDefaultLogger = true;
            LoggingExceptionHandlerData loggingHandlerData = new LoggingExceptionHandlerData();
            loggingHandlerData.Name = name;
            loggingHandlerData.EventId = eventId;
            loggingHandlerData.Priority = priority;
            loggingHandlerData.FormatterType = formatterType;
            loggingHandlerData.Severity = severity;
            loggingHandlerData.UseDefaultLogger = useDefaultLogger;
            LoggingExceptionHandlerNode exceptionHandlerNode = new LoggingExceptionHandlerNode(loggingHandlerData);
            ApplicationNode.AddNode(exceptionHandlerNode);
            LoggingExceptionHandlerData nodeData = (LoggingExceptionHandlerData)exceptionHandlerNode.ExceptionHandlerData;
            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(eventId, nodeData.EventId);
            Assert.AreEqual(formatterType, nodeData.FormatterType);
            Assert.AreEqual(priority, nodeData.Priority);
            Assert.AreEqual(severity, nodeData.Severity);
            Assert.AreEqual(useDefaultLogger, nodeData.UseDefaultLogger);
        }
    }
}
