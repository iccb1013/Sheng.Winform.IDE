/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.IO;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners
{
    [TestClass]
    public class TallyKeepingFileStreamWriterFixture
    {
        string fileName = Guid.NewGuid().ToString();
        [TestCleanup]
        public void TearDown()
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
        [TestMethod]
        public void InitializesTallyForNewFile()
        {
            using (RollingFlatFileTraceListener.TallyKeepingFileStreamWriter writer
                = new RollingFlatFileTraceListener.TallyKeepingFileStreamWriter(File.Open(fileName, FileMode.Append, FileAccess.Write, FileShare.Read)))
            {
                Assert.AreEqual(0L, writer.Tally);
            }
        }
        [TestMethod]
        public void InitializesTallyForExistingFile()
        {
            File.WriteAllText(fileName, "12345");
            using (RollingFlatFileTraceListener.TallyKeepingFileStreamWriter writer
                = new RollingFlatFileTraceListener.TallyKeepingFileStreamWriter(File.Open(fileName, FileMode.Append, FileAccess.Write, FileShare.Read)))
            {
                Assert.AreEqual(5L, writer.Tally);
            }
            File.WriteAllText(fileName, "12345");
            using (RollingFlatFileTraceListener.TallyKeepingFileStreamWriter writer
                = new RollingFlatFileTraceListener.TallyKeepingFileStreamWriter(File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.Read)))
            {
                Assert.AreEqual(0L, writer.Tally);
            }
            File.WriteAllText(fileName, "12345");
            using (RollingFlatFileTraceListener.TallyKeepingFileStreamWriter writer
                = new RollingFlatFileTraceListener.TallyKeepingFileStreamWriter(File.Open(fileName, FileMode.Truncate, FileAccess.Write, FileShare.Read)))
            {
                Assert.AreEqual(0L, writer.Tally);
            }
        }
        [TestMethod]
        public void WritingToFileUpdatesTally()
        {
            using (RollingFlatFileTraceListener.TallyKeepingFileStreamWriter writer
                = new RollingFlatFileTraceListener.TallyKeepingFileStreamWriter(File.Open(fileName, FileMode.Append, FileAccess.Write, FileShare.Read)))
            {
                writer.Write("12345");
                writer.Flush();
                Assert.AreEqual(5L, writer.Tally);
            }
        }
        [TestMethod]
        public void WritingToFileWithEncodingUpdatesTally()
        {
            using (RollingFlatFileTraceListener.TallyKeepingFileStreamWriter writer
                = new RollingFlatFileTraceListener.TallyKeepingFileStreamWriter(File.Open(fileName, FileMode.Append, FileAccess.Write, FileShare.Read),
                                                                                Encoding.UTF32))
            {
                writer.Write("12345");
                writer.Flush();
                Assert.AreEqual(20L, writer.Tally); 
            }
        }
    }
}
