/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Configuration.Tests
{
    [TestClass]
    public class MsmqDistributorSettingsFixture
    {
        [TestMethod]
        public void CanDeserializeSerializedSettings() {}
        [TestMethod]
        public void CanReadSettingsFromConfigurationFile()
        {
            MsmqDistributorSettings settings = MsmqDistributorSettings.GetSettings(new SystemConfigurationSource());
            Assert.IsNotNull(settings);
            Assert.AreEqual(CommonUtil.MessageQueuePath, settings.MsmqPath);
            Assert.AreEqual(1000, settings.QueueTimerInterval);
            Assert.AreEqual("Msmq Distributor", settings.ServiceName);
        }
    }
}
