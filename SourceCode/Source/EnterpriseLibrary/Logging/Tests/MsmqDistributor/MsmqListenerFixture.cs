/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Tests
{
    [TestClass]
    public class MsmqListenerFixture
    {
        MsmqListener listener;
        DistributorServiceTestFacade distributorServiceTestFacade;
        MockMsmqLogDistributor mockQ;
        [TestInitialize]
        public void Setup()
        {
            distributorServiceTestFacade = new DistributorServiceTestFacade();
            distributorServiceTestFacade.Initialize();
            listener = new MsmqListener(distributorServiceTestFacade, 1000, CommonUtil.MessageQueuePath);
            mockQ = new MockMsmqLogDistributor(new LogWriter(new List<ILogFilter>(), new List<LogSource>(), new LogSource("errors"), "default"), CommonUtil.MessageQueuePath);
        }
        [TestCleanup]
        public void Teardown() {}
        [TestMethod]
        public void StartListener()
        {
            listener.QueueTimerInterval = 10;
            listener.SetMsmqLogDistributor(mockQ);
            listener.StartListener();
            Thread.Sleep(listener.QueueTimerInterval + 300);
            Assert.IsTrue(mockQ.ReceiveMsgCalled, "receive initiated");
            listener.StopListener();
        }
        [TestMethod]
        public void StopListener()
        {
            listener.QueueTimerInterval = 10;
            listener.SetMsmqLogDistributor(mockQ);
            listener.StartListener();
            Thread.Sleep(listener.QueueTimerInterval + 300);
            bool result = listener.StopListener();
            Assert.IsTrue(result, "stopListener result");
            try
            {
                throw new Exception("simulated exception - forced event logger flush");
            }
            catch (Exception /* e */)
            {
            }
        }
        [TestMethod]
        public void StopListenerAndExceedStopRetries()
        {
            listener.QueueTimerInterval = 10;
            listener.QueueListenerRetries = 1;
            mockQ.SetIsCompleted(false);
            listener.SetMsmqLogDistributor(mockQ);
            listener.StartListener();
            Thread.Sleep(listener.QueueTimerInterval + 300);
            bool result = listener.StopListener();
            Assert.IsFalse(mockQ.StopReceiving, "stop receiving");
            Assert.IsFalse(result, "stopListener result");
            try
            {
                throw new Exception("simulated exception - forced event logger flush");
            }
            catch (Exception /* e */)
            {
            }
        }
        [TestMethod]
        public void StopListenerError()
        {
            listener.QueueTimerInterval = 10000;
            mockQ.ExceptionOnGetIsCompleted = true;
            listener.SetMsmqLogDistributor(mockQ);
            listener.StartListener();
            try
            {
                listener.StopListener();
            }
            catch (Exception /* e */)
            {
                return;
            }
            Assert.Fail("exception not raised");
        }
        [TestMethod]
        public void RevertToDefaultTimerInterval()
        {
            listener.QueueTimerInterval = 0;
            Assert.AreEqual(20000, listener.QueueTimerInterval);
        }
    }
}
