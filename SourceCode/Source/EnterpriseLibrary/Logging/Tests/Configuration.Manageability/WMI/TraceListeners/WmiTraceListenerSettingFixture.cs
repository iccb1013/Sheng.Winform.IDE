/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Diagnostics;
using System.Management;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Tests.WMI.TraceListeners
{
    [TestClass]
    public class WmiTraceListenerSettingFixture
    {
        [TestInitialize]
        public void SetUp()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(WmiTraceListenerSetting));
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
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM WmiTraceListenerSetting")
                        .Get().GetEnumerator())
            {
                Assert.IsFalse(resultEnumerator.MoveNext());
            }
        }
        [TestMethod]
        public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
        {
			WmiTraceListenerSetting setting = new WmiTraceListenerSetting(null, "name", "TraceOutputOptions", System.Diagnostics.SourceLevels.Critical.ToString());
            setting.ApplicationName = "app";
            setting.SectionName = InstrumentationConfigurationSection.SectionName;
            setting.Publish();
            using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM WmiTraceListenerSetting")
                        .Get().GetEnumerator())
            {
                Assert.IsTrue(resultEnumerator.MoveNext());
                Assert.AreEqual("name", resultEnumerator.Current.Properties["Name"].Value);
                Assert.AreEqual("TraceOutputOptions", resultEnumerator.Current.Properties["TraceOutputOptions"].Value);
				Assert.AreEqual(SourceLevels.Critical.ToString(), resultEnumerator.Current.Properties["Filter"].Value);
				Assert.AreEqual("WmiTraceListenerSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
                Assert.IsFalse(resultEnumerator.MoveNext());
            }
        }
        [TestMethod]
        public void CanBindObject()
        {
			WmiTraceListenerSetting setting = new WmiTraceListenerSetting(null, "name", "TraceOutputOptions", System.Diagnostics.SourceLevels.Critical.ToString());
            setting.ApplicationName = "app";
            setting.SectionName = InstrumentationConfigurationSection.SectionName;
            setting.Publish();
            using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM WmiTraceListenerSetting")
                    .Get().GetEnumerator())
            {
                Assert.IsTrue(resultEnumerator.MoveNext());
                Assert.AreEqual("WmiTraceListenerSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
                ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
                Assert.IsNotNull(managementObject);
                managementObject.Put();
            }
        }
    }
}
