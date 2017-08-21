/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Management;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Tests.WMI.Filters
{
    [TestClass]
    public class CustomFilterSettingFixture
    {
        [TestInitialize]
        public void SetUp()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(CustomFilterSetting));
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
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM CustomFilterSetting")
                    .Get().GetEnumerator())
            {
                Assert.IsFalse(resultEnumerator.MoveNext());
            }
        }
        [TestMethod]
        public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
        {
            string[] attributes = new string[] { "att1", "att2" };
            CustomFilterSetting setting = new CustomFilterSetting(null, "name", "FilterType", attributes);
            setting.ApplicationName = "app";
            setting.SectionName = InstrumentationConfigurationSection.SectionName;
            setting.Publish();
            using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM CustomFilterSetting")
                    .Get().GetEnumerator())
            {
                Assert.IsTrue(resultEnumerator.MoveNext());
                Assert.AreEqual("name", resultEnumerator.Current.Properties["Name"].Value);
                Assert.AreEqual("FilterType", resultEnumerator.Current.Properties["FilterType"].Value);
                ReferenceEquals(attributes, resultEnumerator.Current.Properties["Attributes"].Value);
                Assert.AreEqual("CustomFilterSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
                Assert.IsFalse(resultEnumerator.MoveNext());
            }
        }
        [TestMethod]
        public void CanBindObject()
        {
            string[] attributes = new string[] { "att1", "att2" };
            CustomFilterSetting setting = new CustomFilterSetting(null, "name", "FilterType", attributes);
            setting.ApplicationName = "app";
            setting.SectionName = InstrumentationConfigurationSection.SectionName;
            setting.Publish();
            using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM CustomFilterSetting")
                    .Get().GetEnumerator())
            {
                Assert.IsTrue(resultEnumerator.MoveNext());
                Assert.AreEqual("CustomFilterSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
                ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
                Assert.IsNotNull(managementObject);
                managementObject.Put();
            }
        }
        [TestMethod]
        public void SavesChangesToConfigurationObject()
        {
            CustomLogFilterData sourceElement = new CustomLogFilterData("name", typeof(bool));
            sourceElement.Attributes.Add("att3", "val3");
            sourceElement.Attributes.Add("att4", "val4");
            sourceElement.Attributes.Add("att5", "val5");
            List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
            CustomLogFilterDataWmiMapper.GenerateWmiObjects(sourceElement, settings);
            Assert.AreEqual(1, settings.Count);
            CustomFilterSetting setting = settings[0] as CustomFilterSetting;
            Assert.IsNotNull(setting);
            setting.Attributes = new string[] { "att1=val1", "att2=val2" };
            setting.FilterType = typeof(int).AssemblyQualifiedName;
            setting.Commit();
            Assert.AreEqual(2, sourceElement.Attributes.Count);
            Assert.AreEqual("val1", sourceElement.Attributes["att1"]);
            Assert.AreEqual("val2", sourceElement.Attributes["att2"]);
            Assert.AreEqual(typeof(int), sourceElement.Type);
        }
    }
}
