/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Tests
{
    [TestClass]
    public class LoggingExceptionHandlerFixture
    {
        [TestMethod]
        public void ExceptionHandledThroughLoggingBlock()
        {
            MockTraceListener.Reset();
            Assert.IsFalse(ExceptionPolicy.HandleException(new Exception("TEST EXCEPTION"), "Logging Policy"));
            Assert.AreEqual(1, MockTraceListener.LastEntry.Categories.Count);
            Assert.IsTrue(MockTraceListener.LastEntry.Categories.Contains("TestCat"));
            Assert.AreEqual(5, MockTraceListener.LastEntry.EventId);
            Assert.AreEqual(TraceEventType.Error, MockTraceListener.LastEntry.Severity);
            Assert.AreEqual("TestTitle", MockTraceListener.LastEntry.Title);
        }
        [TestMethod]
        [ExpectedException(typeof(ExceptionHandlingException))]
        public void BadFormatterThrowsExceptionWhenHandlingExceptionWithLoggingBlock()
        {
            ExceptionPolicy.HandleException(new Exception("TEST EXCEPTION"), "Bad Formatter Logging Policy");
        }
        [TestMethod]
        public void MultipleRequestsUseSameLogWriterInstance()
        {
            MockTraceListener.Reset();
            Assert.IsFalse(ExceptionPolicy.HandleException(new Exception("TEST EXCEPTION"), "Logging Policy"));
            Assert.IsFalse(ExceptionPolicy.HandleException(new Exception("TEST EXCEPTION"), "Logging Policy"));
            Assert.IsFalse(ExceptionPolicy.HandleException(new Exception("TEST EXCEPTION"), "Logging Policy"));
            Assert.AreEqual(3, MockTraceListener.Entries.Count);
            Assert.AreEqual(3, MockTraceListener.Instances.Count);
            Assert.AreSame(MockTraceListener.Instances[0], MockTraceListener.Instances[1]);
            Assert.AreSame(MockTraceListener.Instances[0], MockTraceListener.Instances[2]);
        }
        [TestMethod]
        public void LoggedExceptionCopiesExceptionDataForStringKeys()
        {
            MockTraceListener.Reset();
            Exception exception = new Exception("TEST EXCEPTION");
            object value = new object();
            object key4 = new object();
            exception.Data.Add("key1", value);
            exception.Data.Add("key2", "value");
            exception.Data.Add("key3", 3);
            exception.Data.Add(key4, "value");
            Assert.IsFalse(ExceptionPolicy.HandleException(exception, "Logging Policy"));
            Assert.AreEqual(1, MockTraceListener.Entries.Count);
            Assert.AreEqual(3, MockTraceListener.Entries[0].ExtendedProperties.Count);
            Assert.AreEqual(value, MockTraceListener.Entries[0].ExtendedProperties["key1"]);
            Assert.AreEqual("value", MockTraceListener.Entries[0].ExtendedProperties["key2"]);
            Assert.AreEqual(3, MockTraceListener.Entries[0].ExtendedProperties["key3"]);
        }
        [TestMethod]
        public void LoggedExceptionWithoutExceptionDataWorks()
        {
            MockTraceListener.Reset();
            Exception exception = new Exception("TEST EXCEPTION");
            Assert.IsFalse(ExceptionPolicy.HandleException(exception, "Logging Policy"));
            Assert.AreEqual(1, MockTraceListener.Entries.Count);
            Assert.AreEqual(0, MockTraceListener.Entries[0].ExtendedProperties.Count);
        }
        [TestMethod]
        public void LoggingHandlerUsesNewLoggingStackByDefault()
        {
            MockTraceListener.Reset();
            Assert.IsFalse(ExceptionPolicy.HandleException(new Exception("TEST EXCEPTION"), "Logging Policy"));
            Logger.Write("Test");
            Assert.AreEqual(2, MockTraceListener.Entries.Count);
            Assert.AreEqual(2, MockTraceListener.Instances.Count);
            Assert.AreNotSame(MockTraceListener.Instances[0], MockTraceListener.Instances[1]);
        }
        [TestMethod]
        public void LoggingHandlerUsesDefaultLoggingStackWhenIndicated()
        {
            MockTraceListener.Reset();
            Assert.IsFalse(ExceptionPolicy.HandleException(new Exception("TEST EXCEPTION"), "Logging Policy - use default logger"));
            Logger.Write("Test");
            Assert.AreEqual(2, MockTraceListener.Entries.Count);
            Assert.AreEqual(2, MockTraceListener.Instances.Count);
            Assert.AreSame(MockTraceListener.Instances[0], MockTraceListener.Instances[1]);
        }
        [TestMethod]
        public void LoggingHandlerCreatesFormatterWithTheDocumentedConstructor()
        {
            MockTraceListener.Reset();
            ConstructorSensingExceptionFormatter.Instances.Clear();
            Exception exception = new Exception("test exception");
            Guid testGuid = Guid.NewGuid();
            LoggingExceptionHandler handler
                = new LoggingExceptionHandler(
                    "TestCat",
                    0,
                    TraceEventType.Warning,
                    "test",
                    0,
                    typeof(ConstructorSensingExceptionFormatter),
                    Logger.Writer);
            handler.HandleException(exception, testGuid);
            Assert.AreEqual(1, ConstructorSensingExceptionFormatter.Instances.Count);
            Assert.AreSame(exception, ConstructorSensingExceptionFormatter.Instances[0].Exception);
            Assert.AreEqual(testGuid, ConstructorSensingExceptionFormatter.Instances[0].HandlingInstanceId);
        }
        [TestMethod]
        public void CanUseLoggingHandlerWithXmlExceptionFormatter()
        {
            MockTraceListener.Reset();
            Exception exception = new Exception("test exception");
            Guid testGuid = Guid.NewGuid();
            LoggingExceptionHandler handler
                = new LoggingExceptionHandler(
                    "TestCat",
                    0,
                    TraceEventType.Warning,
                    "test",
                    10,
                    typeof(XmlExceptionFormatter),
                    Logger.Writer);
            handler.HandleException(exception, testGuid);
            Assert.AreEqual(1, MockTraceListener.Entries.Count);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(MockTraceListener.Entries[0].Message);
            XmlNode element = doc.DocumentElement.SelectSingleNode("/Exception/@handlingInstanceId");
            Assert.IsNotNull(element);
            Assert.AreEqual(testGuid.ToString("D", CultureInfo.InvariantCulture), element.InnerText);
        }
        public class ConstructorSensingExceptionFormatter : ExceptionFormatter
        {
            TextWriter writer;
            internal static readonly List<ConstructorSensingExceptionFormatter> Instances
                = new List<ConstructorSensingExceptionFormatter>();
            public ConstructorSensingExceptionFormatter(TextWriter writer, Exception exception, Guid handlingInstanceId)
                : base(exception, handlingInstanceId)
            {
                this.writer = writer;
                Instances.Add(this);
            }
            public override void Format()
            {
            }
            protected override void WriteDescription()
            {
                throw new NotImplementedException();
            }
            protected override void WriteDateTime(DateTime utcNow)
            {
                throw new NotImplementedException();
            }
            protected override void WriteExceptionType(Type exceptionType)
            {
                throw new NotImplementedException();
            }
            protected override void WriteMessage(string message)
            {
                throw new NotImplementedException();
            }
            protected override void WriteSource(string source)
            {
                throw new NotImplementedException();
            }
            protected override void WriteHelpLink(string helpLink)
            {
                throw new NotImplementedException();
            }
            protected override void WriteStackTrace(string stackTrace)
            {
                throw new NotImplementedException();
            }
            protected override void WritePropertyInfo(System.Reflection.PropertyInfo propertyInfo, object value)
            {
                throw new NotImplementedException();
            }
            protected override void WriteFieldInfo(System.Reflection.FieldInfo fieldInfo, object value)
            {
                throw new NotImplementedException();
            }
            protected override void WriteAdditionalInfo(System.Collections.Specialized.NameValueCollection additionalInformation)
            {
                throw new NotImplementedException();
            }
        }
    }
}
