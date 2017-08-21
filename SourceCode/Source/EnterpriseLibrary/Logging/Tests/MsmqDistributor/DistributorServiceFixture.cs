/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Tests.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Tests
{
    [TestClass]
    public class DistributorServiceFixture
    {
        DistributorServiceTestFacade distributorServiceTestFacade;
        MockMsmqListener mockListener;
        const int sleepTimer = 300;
        [TestInitialize]
        public void Setup()
        {
            distributorServiceTestFacade = new DistributorServiceTestFacade();
            mockListener = new MockMsmqListener(distributorServiceTestFacade, 1000, CommonUtil.MessageQueuePath);
            distributorServiceTestFacade.QueueListener = mockListener;
        }
        [TestCleanup]
        public void Teardown()
        {
        }
        [TestMethod]
        public void Initialization()
        {
            DistributorServiceTestFacade distributor = new DistributorServiceTestFacade();
            distributor.InitializeComponent();
            Assert.IsNotNull(distributor);
            Assert.AreEqual(ServiceStatus.OK, distributor.Status);
            MsmqDistributorSettings settings = MsmqDistributorSettings.GetSettings(new SystemConfigurationSource());
            Assert.AreEqual(settings.ServiceName, distributor.ApplicationName);
            distributor.EventLogger.LogServiceFailure(string.Empty, new Exception("simulated exception - forced event logger flush"), TraceEventType.Error);
            Assert.IsTrue(CommonUtil.LogEntryExists(Resources.InitializeComponentStarted), "init begin");
            Assert.IsTrue(CommonUtil.LogEntryExists(Resources.InitializeComponentCompleted), "init end");
        }
        [TestMethod]
        public void StartAndStopService()
        {
            distributorServiceTestFacade.OnStart();
            Assert.IsTrue(mockListener.StartCalled, "mock start called");
            Assert.AreEqual(ServiceStatus.OK, distributorServiceTestFacade.Status, "status");
            Assert.IsTrue(CommonUtil.LogEntryExists(Resources.ValidationStarted), "validate start");
            Assert.IsTrue(CommonUtil.LogEntryExists(Resources.ValidationComplete), "validate complete");
            Assert.IsTrue(CommonUtil.LogEntryExists(string.Format(Resources.ServiceStartComplete, distributorServiceTestFacade.ApplicationName)), "start complete");
            distributorServiceTestFacade.OnStop();
            Assert.IsTrue(mockListener.StopCalled, "mock stop called");
            Assert.IsTrue(CommonUtil.LogEntryExists(string.Format(Resources.ServiceStopComplete, distributorServiceTestFacade.ApplicationName)), "stop complete");
            Assert.AreEqual(ServiceStatus.OK, distributorServiceTestFacade.Status);
        }
        [TestMethod]
        public void StartServiceWithError()
        {
            mockListener.ExceptionOnStart = true;
            distributorServiceTestFacade.OnStart();
            Assert.IsTrue(CommonUtil.LogEntryExists(string.Format(Resources.ServiceStartError, distributorServiceTestFacade.ApplicationName)), "start error");
            Assert.IsTrue(CommonUtil.LogEntryExists(Resources.ValidationError), "validate error");
            Assert.AreEqual(ServiceStatus.Shutdown, distributorServiceTestFacade.Status);
        }
        [TestMethod]
        public void StopServiceWithError()
        {
            mockListener.ExceptionOnStop = true;
            distributorServiceTestFacade.OnStart();
            distributorServiceTestFacade.OnStop();
            Assert.IsTrue(CommonUtil.LogEntryExists(string.Format(Resources.ServiceStopError, distributorServiceTestFacade.ApplicationName)), "stop error");
            Assert.AreEqual(ServiceStatus.Shutdown, distributorServiceTestFacade.Status);
        }
        [TestMethod]
        public void StopServiceWithWarning()
        {
            mockListener.StopReturnsFalse = true;
            distributorServiceTestFacade.OnStart();
            distributorServiceTestFacade.OnStop();
            Assert.IsTrue(CommonUtil.LogEntryExists(string.Format(Resources.ServiceStopWarning, distributorServiceTestFacade.ApplicationName)), "stop warning");
        }
        [TestMethod]
        public void PauseAndContinueService()
        {
            distributorServiceTestFacade.OnStart();
            distributorServiceTestFacade.OnPause();
            Assert.AreEqual(ServiceStatus.OK, distributorServiceTestFacade.Status, "status");
            Assert.IsTrue(mockListener.StopCalled, "mock stop called");
            Assert.IsTrue(CommonUtil.LogEntryExists(string.Format(Resources.ServicePausedSuccess, distributorServiceTestFacade.ApplicationName)), "start complete");
            distributorServiceTestFacade.OnContinue();
            Assert.AreEqual(ServiceStatus.OK, distributorServiceTestFacade.Status);
            Assert.IsTrue(mockListener.StartCalled, "mock start called");
            Assert.IsTrue(CommonUtil.LogEntryExists(string.Format(Resources.ServiceResumeComplete, distributorServiceTestFacade.ApplicationName)), "stop complete");
        }
        [TestMethod]
        public void PauseServiceWithWarning()
        {
            mockListener.StopReturnsFalse = true;
            distributorServiceTestFacade.OnStart();
            distributorServiceTestFacade.OnPause();
            Assert.IsTrue(CommonUtil.LogEntryExists(string.Format(Resources.ServicePauseWarning, distributorServiceTestFacade.ApplicationName)), "stop warning");
            Assert.AreEqual(ServiceStatus.OK, distributorServiceTestFacade.Status);
        }
        [TestMethod]
        public void PauseServiceWithError()
        {
            mockListener.ExceptionOnStop = true;
            distributorServiceTestFacade.OnStart();
            distributorServiceTestFacade.OnPause();
            Assert.IsTrue(CommonUtil.LogEntryExists(string.Format(Resources.ServicePauseError, distributorServiceTestFacade.ApplicationName)), "stop warning");
            Assert.AreEqual(ServiceStatus.Shutdown, distributorServiceTestFacade.Status);
        }
        [TestMethod]
        public void ContinueServiceWithError()
        {
            distributorServiceTestFacade.OnStart();
            distributorServiceTestFacade.OnPause();
            mockListener.ExceptionOnStart = true;
            distributorServiceTestFacade.OnContinue();
            Assert.IsTrue(CommonUtil.LogEntryExists(string.Format(Resources.ServiceResumeError, distributorServiceTestFacade.ApplicationName)), "continue error");
            Assert.AreEqual(ServiceStatus.Shutdown, distributorServiceTestFacade.Status);
        }
    }
}
