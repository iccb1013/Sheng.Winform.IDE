/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Tests
{
    [TestClass]
    public class FaultContractWrapperExceptionFixture
    {
        [TestMethod]
        public void CanCreateInstanceWithFaultContract()
        {
            MockFaultContract faultContract = new MockFaultContract();
            FaultContractWrapperException instance = new FaultContractWrapperException(faultContract);
            Assert.IsNotNull(instance);
            Assert.AreEqual(faultContract, instance.FaultContract);
        }
        [TestMethod]
        public void CanCreateInstanceWithFaultContractAndGuid()
        {
            MockFaultContract faultContract = new MockFaultContract();
            Guid guid = Guid.NewGuid();
            FaultContractWrapperException instance = new FaultContractWrapperException(faultContract, guid);
            Assert.AreEqual(faultContract, instance.FaultContract);
            Assert.IsTrue(instance.Message.Contains(guid.ToString()));
        }
        [TestMethod]
        public void CanCreateInstanceWithFaultContractAndGuidAndMessage()
        {
            MockFaultContract faultContract = new MockFaultContract();
            Guid guid = Guid.NewGuid();
            Exception innerException = new Exception();
            FaultContractWrapperException instance = new FaultContractWrapperException(faultContract, guid, "NewMessage");
            Assert.AreEqual(faultContract, instance.FaultContract);
            Assert.IsFalse(instance.Message.Contains(guid.ToString()));
            Assert.AreEqual("NewMessage", instance.Message);
        }
        [TestMethod]
        public void CanCreateInstanceWithFaultContractAndGuidAndNullMessage()
        {
            MockFaultContract faultContract = new MockFaultContract();
            Guid guid = Guid.NewGuid();
            Exception innerException = new Exception();
            FaultContractWrapperException instance = new FaultContractWrapperException(faultContract, guid, null);
            Assert.AreEqual(faultContract, instance.FaultContract);
            Assert.IsFalse(string.IsNullOrEmpty(instance.Message));
            Assert.IsTrue(instance.Message.Contains(guid.ToString()));
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowOnNullFaultContract()
        {
            FaultContractWrapperException instance = new FaultContractWrapperException(null);
        }
        [TestMethod]
        public void CanAssignFaultContractMessage()
        {
            MockFaultContract mock = new MockFaultContract("message");
            FaultContractWrapperException instance = new FaultContractWrapperException(mock);
            Assert.AreSame(mock, instance.FaultContract);
            MockFaultContract newMock = new MockFaultContract();
            instance.FaultContract = newMock;
            Assert.AreSame(newMock, instance.FaultContract);
        }
    }
}
