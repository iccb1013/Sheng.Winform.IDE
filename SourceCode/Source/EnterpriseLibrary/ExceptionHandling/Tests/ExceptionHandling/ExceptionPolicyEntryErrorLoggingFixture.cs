/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    [TestClass]
    public class ExceptionPolicyEntryErrorLoggingFixture
    {
        bool errorCalledBack;
        ExceptionHandlingInstrumentationProvider instrumentationProvider;
        [TestInitialize]
        public void SetUp()
        {
            errorCalledBack = false;
        }
        [TestMethod]
        public void HandlerThatReturnsNullAtEndOfChainWillCauseErrorEventToBeRaised()
        {
            ExceptionPolicyEntry policyEntry = CreatePolicyEntry(new MockReturnNullExceptionHandler(), PostHandlingAction.ThrowNewException);
            try
            {
                policyEntry.Handle(new ArgumentException());
            }
            catch (ExceptionHandlingException) {}
            Assert.IsTrue(errorCalledBack);
        }
        [TestMethod]
        public void HandlerThatThrowsExceptionCausesErrorEventToBeRaised()
        {
            ExceptionPolicyEntry policyEntry = CreatePolicyEntry(new MockThrowingExceptionHandler(), PostHandlingAction.None);
            try
            {
                policyEntry.Handle(new ArgumentException());
            }
            catch (ExceptionHandlingException) {}
            Assert.IsTrue(errorCalledBack);
        }
        ExceptionPolicyEntry CreatePolicyEntry(IExceptionHandler exceptionHandler,
                                               PostHandlingAction postHandlingAction)
        {
            List<IExceptionHandler> handlerList = new List<IExceptionHandler>();
            handlerList.Add(exceptionHandler);
            ExceptionPolicyEntry policyEntry = new ExceptionPolicyEntry(postHandlingAction, handlerList);
            instrumentationProvider = new ExceptionHandlingInstrumentationProvider();
            instrumentationProvider.exceptionHandlingErrorOccurred += new EventHandler<ExceptionHandlingErrorEventArgs>(ErrorCallback);
            policyEntry.SetInstrumentationProvider(instrumentationProvider);
            return policyEntry;
        }
        void ErrorCallback(object sender,
                           ExceptionHandlingErrorEventArgs e)
        {
            errorCalledBack = true;
        }
    }
}
