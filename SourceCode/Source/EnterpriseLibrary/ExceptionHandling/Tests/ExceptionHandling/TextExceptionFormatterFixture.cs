/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    [TestClass]
    public class TextExceptionFormatterFixture
    {
        const string message = "message";
        const string innerException = "Inner Exception";
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstrutingWithNullWriterThrows()
        {
            new TextExceptionFormatter(null, new Exception());
        }
        [TestMethod]
        public void CreateTest()
        {
            TextWriter writer = new StringWriter();
            Exception exception = new Exception();
            TextExceptionFormatter formatter = new TextExceptionFormatter(writer, exception);
            Assert.AreSame(writer, formatter.Writer);
            Assert.AreSame(exception, formatter.Exception);
        }
        [TestMethod]
        public void VerifyInnerExceptionGetsFormatted()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            Exception exception = new MockException(message, new MockException());
            TextExceptionFormatter formatter = new TextExceptionFormatter(writer, exception);
            Assert.IsTrue(sb.Length == 0);
            formatter.Format();
            Assert.IsTrue(sb.ToString().Contains(innerException));
        }
        [TestMethod]
        public void SimpleFormatterTest()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            Exception exception = new MockException();
            TextExceptionFormatter formatter = new TextExceptionFormatter(writer, exception);
            Assert.IsTrue(sb.Length == 0);
            formatter.Format();
            Assert.IsTrue(sb.Length > 0);
        }
        [TestMethod]
        public void WritesHandlingInstanceIdIfNotEmpty()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            Guid testGuid = Guid.NewGuid();
            Exception exception = new MockException();
            TextExceptionFormatter formatter = new TextExceptionFormatter(writer, exception, testGuid);
            formatter.Format();
            string[] lines = sb.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            string line = lines.First(l => l.StartsWith("HandlingInstanceID", StringComparison.Ordinal));
            Assert.IsNotNull(line);
            Assert.IsTrue(line.IndexOf(testGuid.ToString("D", CultureInfo.InvariantCulture)) >= 0);
        }
        [TestMethod]
        public void SkipsHandlingInstanceIdIfEmpty()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            Exception exception = new MockException();
            TextExceptionFormatter formatter = new TextExceptionFormatter(writer, exception, Guid.Empty);
            formatter.Format();
            string[] lines = sb.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            Assert.IsFalse(lines.Any(l => l.StartsWith("HandlingInstanceID", StringComparison.Ordinal)));
        }
    }
}
