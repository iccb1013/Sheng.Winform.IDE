/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Management;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Formatters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Tests.WMI.Formatters
{
    [TestClass]
    public class BinaryFormatterSettingFixture
    {
        [TestInitialize]
        public void SetUp()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(BinaryFormatterSetting));
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
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM BinaryFormatterSetting")
                        .Get().GetEnumerator())
            {
                Assert.IsFalse(resultEnumerator.MoveNext());
            }
        }
        [TestMethod]
        public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
        {
            BinaryFormatterSetting setting = new BinaryFormatterSetting(null, "name");
            setting.ApplicationName = "app";
            setting.SectionName = InstrumentationConfigurationSection.SectionName;
            setting.Publish();
            using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM BinaryFormatterSetting")
                        .Get().GetEnumerator())
            {
                Assert.IsTrue(resultEnumerator.MoveNext());
                Assert.AreEqual("name", resultEnumerator.Current.Properties["Name"].Value);
                Assert.AreEqual("BinaryFormatterSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
                Assert.IsFalse(resultEnumerator.MoveNext());
            }
        }
        [TestMethod]
        public void CanBindObject()
        {
            BinaryFormatterSetting setting = new BinaryFormatterSetting(null, "name");
            setting.ApplicationName = "app";
            setting.SectionName = InstrumentationConfigurationSection.SectionName;
            setting.Publish();
            using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM BinaryFormatterSetting")
                    .Get().GetEnumerator())
            {
                Assert.IsTrue(resultEnumerator.MoveNext());
                Assert.AreEqual("BinaryFormatterSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
                ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
                Assert.IsNotNull(managementObject);
                managementObject.Put();
            }
        }
	}
}
