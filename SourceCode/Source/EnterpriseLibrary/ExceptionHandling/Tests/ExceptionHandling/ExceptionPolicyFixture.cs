/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using System.Diagnostics;
using System.Security;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SysConfig = System.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    [TestClass]
    public class ExceptionPolicyFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HandleExceptionWithNullPolicyThrows()
        {
            ExceptionPolicy.HandleException(new UnauthorizedAccessException(), null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HandleExceptionWithNullPolicyThrows2()
        {
            Exception exceptionToThrow;
            ExceptionPolicy.HandleException(new UnauthorizedAccessException(), null, out exceptionToThrow);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HandleExceptionWithNullExceptionThrows()
        {
            ExceptionPolicy.HandleException(null, "Wrap Policy");
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HandleExceptionWithNullExceptionThrows2()
        {
            Exception exceptionToThrow;
            ExceptionPolicy.HandleException(null, "Wrap Policy", out exceptionToThrow);
        }
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void UndefinedPolicyRequestedThrows()
        {
            ExceptionPolicy.HandleException(new MockException(), "Undefined Policy");
        }
        [TestMethod]
        public void WrapHandlerTest()
        {
            Exception originalException = new ArgumentNullException();
            Exception wrappedException = null;
            try
            {
                ExceptionPolicy.HandleException(originalException, "Wrap Policy");
            }
            catch (Exception ex)
            {
                wrappedException = ex;
            }
            Assert.IsNotNull(wrappedException);
            Assert.AreEqual("Test Message", wrappedException.Message);
            Assert.AreEqual(typeof(ApplicationException), wrappedException.GetType());
            Assert.IsNotNull(wrappedException.InnerException);
        }
        [TestMethod]
        public void ReplaceHandlerTest()
        {
            Exception originalException = new ArgumentNullException();
            Exception replacedException = null;
            try
            {
                ExceptionPolicy.HandleException(originalException, "Replace Policy");
            }
            catch (Exception ex)
            {
                replacedException = ex;
            }
            Assert.IsNotNull(replacedException);
            Assert.AreEqual("Test Message", replacedException.Message);
            Assert.AreEqual(typeof(ApplicationException), replacedException.GetType());
            Assert.IsNull(replacedException.InnerException);
        }
        [TestMethod]
        public void CustomHandlerTest()
        {
            MockExceptionHandler.Clear();
            Exception originalException = new ArgumentNullException();
            Exception customException = null;
            try
            {
                ExceptionPolicy.HandleException(originalException, "Custom Policy");
            }
            catch (Exception ex)
            {
                customException = ex;
            }
            Assert.IsNotNull(customException);
            Assert.AreEqual(2, MockExceptionHandler.attributes.Count);
            Assert.AreEqual("32", MockExceptionHandler.attributes["Age"]);
            Assert.AreEqual(typeof(ArgumentNullException), customException.GetType());
            MockExceptionHandler.Clear();
        }
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void InvalidExceptionTypeInConfigurationTest()
        {
            try
            {
                throw new SecurityException("ExceptionType in Config File is not available");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "InvalidExceptionTypeInConfiguration");
                if (rethrow)
                {
                    throw;
                }
            }
        }
        [TestMethod]
        public void WmiEventFiredWhenCreatingUndefinedPolicy()
        {
            using (WmiEventWatcher eventListener = new WmiEventWatcher(1))
            {
                try
                {
                    ExceptionPolicy.HandleException(new Exception(), "ThisIsAnUnknownKey");
                }
                catch (ConfigurationErrorsException)
                {
                    eventListener.WaitForEvents();
                    Assert.AreEqual(1, eventListener.EventsReceived.Count);
                    Assert.AreEqual("ExceptionHandlingConfigurationFailureEvent", eventListener.EventsReceived[0].ClassPath.ClassName);
                    Assert.AreEqual("ThisIsAnUnknownKey", eventListener.EventsReceived[0].GetPropertyValue("PolicyName"));
                    string exceptionMessage = (string)eventListener.EventsReceived[0].GetPropertyValue("ExceptionMessage");
                    Assert.IsFalse(-1 == exceptionMessage.IndexOf("ThisIsAnUnknownKey"));
                }
            }
        }
        [TestMethod]
        public void EventLogWrittenWhenCreatingUndefinedPolicy()
        {
            try
            {
                ExceptionPolicy.HandleException(new Exception(), "ThisIsAnUnknownKey");
            }
            catch (ConfigurationErrorsException)
            {
                using (EventLog applicationLog = new EventLog("Application"))
                {
                    EventLogEntry lastEntry = applicationLog.Entries[applicationLog.Entries.Count - 1];
                    Assert.AreEqual("Enterprise Library ExceptionHandling", lastEntry.Source);
                    Assert.IsTrue(lastEntry.Message.Contains("ThisIsAnUnknownKey"));
                }
            }
        }
        [TestMethod]
        public void OutExceptionReturnsExceptionWhenThrowNewException()
        {
            Exception exceptionToThrow;
            ExceptionPolicy.HandleException(new Exception(), "ThrowNewExceptionPolicy", out exceptionToThrow);
            Assert.IsNotNull(exceptionToThrow);
            Assert.AreSame(typeof(ApplicationException), exceptionToThrow.GetType());
        }
    }
}
