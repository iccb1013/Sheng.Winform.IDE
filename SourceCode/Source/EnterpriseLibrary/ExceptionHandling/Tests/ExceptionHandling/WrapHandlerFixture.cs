/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Globalization;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    [TestClass]
    public class WrapHandlerFixture
    {
        const string message = "message";
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HandlerThrowsWhenNotWrappingAnException()
        {
            WrapHandler handler = new WrapHandler(message, typeof(object));
            handler.HandleException(new ApplicationException(), Guid.NewGuid());
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructingWithNullExceptionTypeThrows()
        {
            WrapHandler handler = new WrapHandler(message, null);
        }
        [TestMethod]
        public void CanWrapException()
        {
            WrapHandler handler = new WrapHandler(message, typeof(ApplicationException));
            Exception ex = handler.HandleException(new InvalidOperationException(), Guid.NewGuid());
            Assert.AreEqual(typeof(ApplicationException), ex.GetType());
            Assert.AreEqual(typeof(ApplicationException), handler.WrapExceptionType);
            Assert.AreEqual(message, ex.Message);
            Assert.AreEqual(typeof(InvalidOperationException), ex.InnerException.GetType());
        }
        [TestMethod]
        public void WrapExceptionReturnsLocalizedMessage()
        {
            Exception exceptionToWrap = new Exception();
            Exception thrownException;
            ExceptionPolicy.HandleException(exceptionToWrap, "LocalizedWrapPolicy", out thrownException);
            Assert.AreEqual(Resources.ExceptionMessage, thrownException.Message);
        }
        [TestMethod]
        public void WrapExceptionReturnsLocalizedMessageBasedOnCurentUICulture()
        {
            CultureInfo originalCultureInfo = Thread.CurrentThread.CurrentUICulture;
            try
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("nl-nl");
                Exception exceptionToWrap = new Exception();
                Exception thrownException;
                ExceptionPolicy.HandleException(exceptionToWrap, "LocalizedWrapPolicy", out thrownException);
                Assert.AreEqual(Resources.ExceptionMessage, thrownException.Message);
            }
            finally
            {
                Thread.CurrentThread.CurrentUICulture = originalCultureInfo;
            }
        }
    }
}
