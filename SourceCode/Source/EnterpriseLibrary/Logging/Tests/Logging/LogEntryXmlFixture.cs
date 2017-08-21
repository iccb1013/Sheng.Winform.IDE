/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    [TestClass]
    public class LogEntryXmlFixture
    {
        [TestMethod]
        public void CanDeserializeLogEntryXmlUsingBinaryFormatter()
        {
            XmlLogEntry entry = CommonUtil.CreateXmlLogEntry();
            string serializedLogEntryXmlText = new BinaryLogFormatter().Format(entry);
            XmlLogEntry desiaralizedLogEntryXml = (XmlLogEntry)BinaryLogFormatter.Deserialize(serializedLogEntryXmlText);
            Assert.IsNotNull(desiaralizedLogEntryXml);
            CommonUtil.AssertXmlLogEntries(entry, desiaralizedLogEntryXml);
        }
    }
}
