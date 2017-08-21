/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using System.Management;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Manageability.Tests.WMI
{
    [TestClass]
    public class ConnectionStringSettingFixture
    {
        [TestInitialize]
        public void SetUp()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(ConnectionStringSetting));
            NamedConfigurationSetting.ClearPublishedInstances();
        }
        [TestCleanup]
        public void TearDown()
        {
			ManagementEntityTypesRegistrar.UnregisterAll();
			NamedConfigurationSetting.ClearPublishedInstances();
        }
        [TestMethod]
        public void WmiQueryReturnsEmptyResultIfNoPublishedInstances()
        {
            using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM ConnectionStringSetting")
                        .Get().GetEnumerator())
            {
                Assert.IsFalse(resultEnumerator.MoveNext());
            }
        }
        [TestMethod]
        public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
        {
            ConnectionStringSetting setting = new ConnectionStringSetting(null, "name", "ConnectionString", "ProviderName");
            setting.ApplicationName = "app";
            setting.SectionName = InstrumentationConfigurationSection.SectionName;
            setting.Publish();
            using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM ConnectionStringSetting")
                        .Get().GetEnumerator())
            {
                Assert.IsTrue(resultEnumerator.MoveNext());
                Assert.AreEqual("name", resultEnumerator.Current.Properties["Name"].Value);
                Assert.AreEqual("ConnectionString", resultEnumerator.Current.Properties["ConnectionString"].Value);
                Assert.AreEqual("ProviderName", resultEnumerator.Current.Properties["ProviderName"].Value);
                Assert.AreEqual("ConnectionStringSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
                Assert.IsFalse(resultEnumerator.MoveNext());
            }
        }
        [TestMethod]
        public void CanBindObject()
        {
            ConnectionStringSetting setting = new ConnectionStringSetting(null, "name", "ConnectionString", "ProviderName");
            setting.ApplicationName = "app";
            setting.SectionName = InstrumentationConfigurationSection.SectionName;
            setting.Publish();
            using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM ConnectionStringSetting")
                    .Get().GetEnumerator())
            {
                Assert.IsTrue(resultEnumerator.MoveNext());
                Assert.AreEqual("ConnectionStringSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
                ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
                Assert.IsNotNull(managementObject);
                managementObject.Put();
            }
        }
		[TestMethod]
		public void SavesChangesToConfigurationObject()
		{
			ConnectionStringSettings sourceElement = new ConnectionStringSettings("name", "connection string", "provider name");
			List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
			ConnectionStringsWmiMapper.GenerateConnectionStringWmiObjects(sourceElement, settings);
			Assert.AreEqual(1, settings.Count);
			ConnectionStringSetting setting = settings[0] as ConnectionStringSetting;
			Assert.IsNotNull(setting);
			setting.ConnectionString = "updated connection string";
			setting.ProviderName = "updated provider name";
			setting.Commit();
			Assert.AreEqual("updated connection string", sourceElement.ConnectionString);
			Assert.AreEqual("updated provider name", sourceElement.ProviderName);
		}
	}
}
