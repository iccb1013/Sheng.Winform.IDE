/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    [TestClass]
    public class ExceptionUtilityFixture
    {
        const string EventLogName = "Application";
        static readonly string EventLogSource = "Enterprise Library Exception Handling";
        const string idMessage = "ID : {handlingInstanceID}";
        const string exceptionMessage = "Unable to handle exception";
        const string policy = "policy";
        [TestMethod]
        public void FormatTokenInMessage()
        {
            Guid id = Guid.NewGuid();
            string formattedMessage = ExceptionUtility.FormatExceptionMessage(idMessage, id);
            Assert.AreEqual("ID : " + id.ToString(), formattedMessage);
        }
        [TestMethod]
        public void LogHandlingError()
        {
            if (!EventLog.SourceExists(EventLogSource)) return;
            Exception ex = new Exception(exceptionMessage);
            ExceptionUtility.FormatExceptionHandlingExceptionMessage(policy, null, null, ex);
            StringBuilder message = new StringBuilder();
            StringWriter writer = null;
            try
            {
                writer = new StringWriter(message);
                TextExceptionFormatter formatter = new TextExceptionFormatter(writer, ex);
                formatter.Format();
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
            using (EventLog log = new EventLog(EventLogName))
            {
                EventLogEntry entry = log.Entries[log.Entries.Count - 1];
                Assert.AreEqual(EventLogEntryType.Error, entry.EntryType);
                Assert.AreEqual(EventLogSource, entry.Source);
            }
        }
    }
}
