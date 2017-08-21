/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Management.Instrumentation;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters.Tests
{
    [TestClass]
    public class XmlLogFormatterFixture
    {
        LogFormatter xmlLogFormatter;
        [TestInitialize]
        public void SetUp()
        {
            xmlLogFormatter = new XmlLogFormatter();
        }
        [TestMethod]
        public void FormatLogEntry()
        {
            LogEntry logEntry = CommonUtil.CreateLogEntry();
            string xml = xmlLogFormatter.Format(logEntry);
            Assert.IsFalse(string.IsNullOrEmpty(xml));
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            Assert.IsNotNull(xmlDocument.FirstChild);
        }
        [TestMethod]
        public void FormatXmlLogEntry()
        {
            XmlLogEntry logEntry = CommonUtil.CreateXmlLogEntry();
            string xml = xmlLogFormatter.Format(logEntry);
            Assert.IsFalse(string.IsNullOrEmpty(xml));
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            Assert.IsNotNull(xmlDocument.FirstChild);
        }
        [TestMethod]
        public void CanFormatLogEntryWithExtendedProperties()
        {
            LogEntry logEntry = new LogEntry();
            logEntry.Message = "Hello";
            logEntry.Categories.Add("General");
            logEntry.ExtendedProperties.Add("test", "test");
            string xml = xmlLogFormatter.Format(logEntry);
            XmlDocument dom = new XmlDocument();
            dom.LoadXml(xml);
        }
        [TestMethod]
        public void CanFormatLogEntryWithIndexerProperties()
        {
            LogEntry logEntry = new LogEntryWithIndexer();
            string xml = xmlLogFormatter.Format(logEntry);
            Assert.IsFalse(string.IsNullOrEmpty(xml));
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            Assert.IsNotNull(xmlDocument.FirstChild);
        }
        [TestMethod]
        public void CanFormatLogEntryWithWriteOnlyProperties()
        {
            LogEntry logEntry = new LogEntryWithWriteOnlyProperty();
            string xml = xmlLogFormatter.Format(logEntry);
            Assert.IsFalse(string.IsNullOrEmpty(xml));
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            Assert.IsNotNull(xmlDocument.FirstChild);
        }
        [TestMethod]
        public void CanFormatLogEntryWithTextThatNeedsEscaping()
        {
            LogEntry logEntry = CommonUtil.CreateLogEntry();
            logEntry.Message = "some <text> that needs escaping &";
            string xml = xmlLogFormatter.Format(logEntry);
            Assert.IsFalse(string.IsNullOrEmpty(xml));
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            Assert.IsNotNull(xmlDocument.FirstChild);
            Assert.AreEqual("Message", xmlDocument.FirstChild.ChildNodes[0].Name);
            Assert.AreEqual(logEntry.Message, xmlDocument.FirstChild.ChildNodes[0].InnerText);
        }
    }
    public class LogEntryWithIndexer : LogEntry
    {
        [IgnoreMember]
        public string this[int index]
        {
            get { return null; }
        }
    }
    public class LogEntryWithWriteOnlyProperty : LogEntry
    {
        [IgnoreMember]
        public string WriteOnly
        {
            set { ; }
        }
    }
}
