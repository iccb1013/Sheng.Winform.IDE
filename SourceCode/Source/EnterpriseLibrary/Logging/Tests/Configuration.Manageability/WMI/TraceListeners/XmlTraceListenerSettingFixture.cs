/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Tests.WMI.TraceListeners
{
    [TestClass]
    public class XmlTraceListenerSettingFixture
    {
        [TestInitialize]
        public void SetUp()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(XmlTraceListenerSetting));
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
            using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM XmlTraceListenerSetting")
                    .Get().GetEnumerator())
            {
                Assert.IsFalse(resultEnumerator.MoveNext());
            }
        }
        [TestMethod]
        public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
        {
			XmlTraceListenerSetting setting = new XmlTraceListenerSetting(null, "name", "FileName", "TraceOutputOptions", System.Diagnostics.SourceLevels.Critical.ToString());
            setting.ApplicationName = "app";
            setting.SectionName = InstrumentationConfigurationSection.SectionName;
            setting.Publish();
            using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM XmlTraceListenerSetting")
                    .Get().GetEnumerator())
            {
                Assert.IsTrue(resultEnumerator.MoveNext());
                Assert.AreEqual("name", resultEnumerator.Current.Properties["Name"].Value);
                Assert.AreEqual("FileName", resultEnumerator.Current.Properties["FileName"].Value);
                Assert.AreEqual("TraceOutputOptions", resultEnumerator.Current.Properties["TraceOutputOptions"].Value);
				Assert.AreEqual(SourceLevels.Critical.ToString(), resultEnumerator.Current.Properties["Filter"].Value);
				Assert.AreEqual("XmlTraceListenerSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
                Assert.IsFalse(resultEnumerator.MoveNext());
            }
        }
        [TestMethod]
        public void CanBindObject()
        {
			XmlTraceListenerSetting setting = new XmlTraceListenerSetting(null, "name", "FileName", "TraceOutputOptions", System.Diagnostics.SourceLevels.Critical.ToString());
            setting.ApplicationName = "app";
            setting.SectionName = InstrumentationConfigurationSection.SectionName;
            setting.Publish();
            using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM XmlTraceListenerSetting")
                    .Get().GetEnumerator())
            {
                Assert.IsTrue(resultEnumerator.MoveNext());
                Assert.AreEqual("XmlTraceListenerSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
                ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
                Assert.IsNotNull(managementObject);
                managementObject.Put();
            }
        }
        [TestMethod]
        public void SavesChangesToConfigurationObject()
        {
            XmlTraceListenerData sourceElement = new XmlTraceListenerData();
            sourceElement.FileName = "file name";
            sourceElement.Filter = SourceLevels.Information;
            sourceElement.TraceOutputOptions = TraceOptions.ProcessId;
            List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
            XmlTraceListenerDataWmiMapper.GenerateWmiObjects(sourceElement, settings);
            Assert.AreEqual(1, settings.Count);
            XmlTraceListenerSetting setting = settings[0] as XmlTraceListenerSetting;
            Assert.IsNotNull(setting);
            setting.FileName = "updated file name";
            setting.Filter = SourceLevels.All.ToString();
            setting.TraceOutputOptions = TraceOptions.ThreadId.ToString();
            setting.Commit();
            Assert.AreEqual("updated file name", sourceElement.FileName);
            Assert.AreEqual(SourceLevels.All, sourceElement.Filter);
            Assert.AreEqual(TraceOptions.ThreadId, sourceElement.TraceOutputOptions);
        }
    }
}
