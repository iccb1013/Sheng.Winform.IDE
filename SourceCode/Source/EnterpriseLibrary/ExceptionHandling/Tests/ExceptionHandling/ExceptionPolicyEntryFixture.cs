/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    [TestClass]
    public class ExceptionPolicyEntryFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ExceptionHandlingException))]
        public void ExceptionHandlerInChainReturnsNullThrows()
        {
            List<IExceptionHandler> handlers = new List<IExceptionHandler>();
            handlers.Add(new MockReturnNullExceptionHandler());
            ExceptionPolicyEntry entry = new ExceptionPolicyEntry(
                PostHandlingAction.ThrowNewException,
                handlers);
            entry.Handle(new ApplicationException());
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructExceptionPolicyEntryWithNullHandlersThrows()
        {
            ExceptionPolicyEntry entry = new ExceptionPolicyEntry(
                PostHandlingAction.ThrowNewException,
                null);
        }
        [TestMethod]
        public void HandleExceptionWithNoPostHandling()
        {
            List<IExceptionHandler> handlers = new List<IExceptionHandler>();
            handlers.Add(new WrapHandler("message", typeof(ArgumentException)));
            ExceptionPolicyEntry entry = new ExceptionPolicyEntry(
                PostHandlingAction.None,
                handlers);
            bool handled = entry.Handle(new ApplicationException());
            Assert.IsFalse(handled);
        }
        [TestMethod]
        public void HandleExceptionWithNotifyRethrow()
        {
            List<IExceptionHandler> handlers = new List<IExceptionHandler>();
            handlers.Add(new WrapHandler("message", typeof(ArgumentException)));
            ExceptionPolicyEntry entry = new ExceptionPolicyEntry(
                PostHandlingAction.NotifyRethrow,
                handlers);
            bool handled = entry.Handle(new ApplicationException());
            Assert.IsTrue(handled);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HandleExceptionWithThrowNewException()
        {
            List<IExceptionHandler> handlers = new List<IExceptionHandler>();
            handlers.Add(new WrapHandler("message", typeof(ArgumentException)));
            ExceptionPolicyEntry entry = new ExceptionPolicyEntry(
                PostHandlingAction.ThrowNewException,
                handlers);
            entry.Handle(new InvalidOperationException());
        }
        [TestMethod]
        [ExpectedException(typeof(ExceptionHandlingException))]
        public void HandleExceptionWithBadExceptionHandlerThatThrows()
        {
            List<IExceptionHandler> handlers = new List<IExceptionHandler>();
            handlers.Add(new MockThrowingExceptionHandler());
            ExceptionPolicyEntry entry = new ExceptionPolicyEntry(
                PostHandlingAction.None,
                handlers);
            entry.Handle(new ApplicationException());
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HandleExceptionWithNullExceptionThrows()
        {
            List<IExceptionHandler> handlers = new List<IExceptionHandler>();
            handlers.Add(new WrapHandler("message", typeof(ArgumentException)));
            ExceptionPolicyEntry entry = new ExceptionPolicyEntry(
                PostHandlingAction.ThrowNewException,
                handlers);
            entry.Handle(null);
        }
    }
}
