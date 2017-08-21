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
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestClass]
    public class RollingFlatFileTraceListenerNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInRollingFlatFileTraceListenerNodeThrows()
        {
            new RollingTraceListenerNode(null);
        }
        [TestMethod]
        public void RollingFlatFileTraceListenerNodeTest()
        {
            string name = "some name";
            string fileName = "some filename";
            string timesTampPattern = "yyyy-MM-dd";
            int rollSizeKB = 10;
            RollFileExistsBehavior rollFileExistsBehavior = RollFileExistsBehavior.Increment;
            RollInterval rollInterval = RollInterval.Hour;
            TraceOptions traceOutputOptions = TraceOptions.Callstack;
            SourceLevels filter = SourceLevels.Critical;
            string header = "header";
            string footer = "footer";
            RollingTraceListenerNode rollingFlatFileTraceListenerNode = new RollingTraceListenerNode();
            rollingFlatFileTraceListenerNode.Name = name;
            rollingFlatFileTraceListenerNode.FileName = fileName;
            rollingFlatFileTraceListenerNode.TimeStampPattern = timesTampPattern;
            rollingFlatFileTraceListenerNode.RollSizeKB = rollSizeKB;
            rollingFlatFileTraceListenerNode.RollFileExistsBehavior = rollFileExistsBehavior;
            rollingFlatFileTraceListenerNode.RollInterval = rollInterval;
            rollingFlatFileTraceListenerNode.TraceOutputOptions = traceOutputOptions;
            rollingFlatFileTraceListenerNode.Filter = filter;
            rollingFlatFileTraceListenerNode.Header = header;
            rollingFlatFileTraceListenerNode.Footer = footer;
            ApplicationNode.AddNode(rollingFlatFileTraceListenerNode);
            RollingFlatFileTraceListenerData nodeData = (RollingFlatFileTraceListenerData)rollingFlatFileTraceListenerNode.TraceListenerData;
            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(fileName, nodeData.FileName);
            Assert.AreEqual(timesTampPattern, nodeData.TimeStampPattern);
            Assert.AreEqual(rollSizeKB, nodeData.RollSizeKB);
            Assert.AreEqual(rollFileExistsBehavior, nodeData.RollFileExistsBehavior);
            Assert.AreEqual(rollInterval, nodeData.RollInterval);
            Assert.AreEqual(traceOutputOptions, nodeData.TraceOutputOptions);
            Assert.AreEqual(filter, nodeData.Filter);
            Assert.AreEqual(header, nodeData.Header);
            Assert.AreEqual(footer, nodeData.Footer);
        }
        [TestMethod]
        public void RollingFlatFileTraceListenerNodeDataTest()
        {
            string name = "some name";
            string fileName = "some filename";
            string timesTampPattern = "yyyy-MM-dd";
            int rollSizeKB = 10;
            RollFileExistsBehavior rollFileExistsBehavior = RollFileExistsBehavior.Increment;
            RollInterval rollInterval = RollInterval.Hour;
            TraceOptions traceOutputOptions = TraceOptions.Callstack;
            SourceLevels filter = SourceLevels.Critical;
            string header = "header";
            string footer = "footer";
            RollingFlatFileTraceListenerData rollingFlatFileTraceListenerData = new RollingFlatFileTraceListenerData();
            rollingFlatFileTraceListenerData.Name = name;
            rollingFlatFileTraceListenerData.FileName = fileName;
            rollingFlatFileTraceListenerData.TimeStampPattern = timesTampPattern;
            rollingFlatFileTraceListenerData.RollSizeKB = rollSizeKB;
            rollingFlatFileTraceListenerData.RollFileExistsBehavior = rollFileExistsBehavior;
            rollingFlatFileTraceListenerData.RollInterval = rollInterval;
            rollingFlatFileTraceListenerData.TraceOutputOptions = traceOutputOptions;
            rollingFlatFileTraceListenerData.Filter = filter;
            rollingFlatFileTraceListenerData.Header = header;
            rollingFlatFileTraceListenerData.Footer = footer;
            RollingTraceListenerNode rollingFlatFileTraceListenerNode = new RollingTraceListenerNode(rollingFlatFileTraceListenerData);
            ApplicationNode.AddNode(rollingFlatFileTraceListenerNode);
            Assert.AreEqual(name, rollingFlatFileTraceListenerNode.Name);
            Assert.AreEqual(fileName, rollingFlatFileTraceListenerNode.FileName);
            Assert.AreEqual(timesTampPattern, rollingFlatFileTraceListenerNode.TimeStampPattern);
            Assert.AreEqual(rollSizeKB, rollingFlatFileTraceListenerNode.RollSizeKB);
            Assert.AreEqual(rollFileExistsBehavior, rollingFlatFileTraceListenerNode.RollFileExistsBehavior);
            Assert.AreEqual(rollInterval, rollingFlatFileTraceListenerNode.RollInterval);
            Assert.AreEqual(traceOutputOptions, rollingFlatFileTraceListenerNode.TraceOutputOptions);
            Assert.AreEqual(filter, rollingFlatFileTraceListenerNode.Filter);
            Assert.AreEqual(header, rollingFlatFileTraceListenerNode.Header);
            Assert.AreEqual(footer, rollingFlatFileTraceListenerNode.Footer);
        }
    }
}
