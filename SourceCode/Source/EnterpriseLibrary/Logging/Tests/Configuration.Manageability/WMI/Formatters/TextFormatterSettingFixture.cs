/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Management;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Formatters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Tests.WMI.Formatters
{
    [TestClass]
    public class TextFormatterSettingFixture
    {
        [TestInitialize]
        public void SetUp()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(TextFormatterSetting));
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
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM TextFormatterSetting")
                        .Get().GetEnumerator())
            {
                Assert.IsFalse(resultEnumerator.MoveNext());
            }
        }
        [TestMethod]
        public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
        {
            TextFormatterSetting setting = new TextFormatterSetting(null, "name", "Template");
            setting.ApplicationName = "app";
            setting.SectionName = InstrumentationConfigurationSection.SectionName;
            setting.Publish();
            using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM TextFormatterSetting")
                        .Get().GetEnumerator())
            {
                Assert.IsTrue(resultEnumerator.MoveNext());
                Assert.AreEqual("name", resultEnumerator.Current.Properties["Name"].Value);
                Assert.AreEqual("Template", resultEnumerator.Current.Properties["Template"].Value);
                Assert.AreEqual("TextFormatterSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
                Assert.IsFalse(resultEnumerator.MoveNext());
            }
        }
        [TestMethod]
        public void CanBindObject()
        {
            TextFormatterSetting setting = new TextFormatterSetting(null, "name", "Template");
            setting.ApplicationName = "app";
            setting.SectionName = InstrumentationConfigurationSection.SectionName;
            setting.Publish();
            using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM TextFormatterSetting")
                    .Get().GetEnumerator())
            {
                Assert.IsTrue(resultEnumerator.MoveNext());
                Assert.AreEqual("TextFormatterSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
                ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
                Assert.IsNotNull(managementObject);
                managementObject.Put();
            }
        }
		[TestMethod]
		public void SavesChangesToConfigurationObject()
		{
			TextFormatterData sourceElement = new TextFormatterData();
			sourceElement.Template = "template";
			List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
			TextFormatterDataWmiMapper.GenerateWmiObjects(sourceElement, settings);
			Assert.AreEqual(1, settings.Count);
			TextFormatterSetting setting = settings[0] as TextFormatterSetting;
			Assert.IsNotNull(setting);
			setting.Template = "updated template";
			setting.Commit();
			Assert.AreEqual("updated template", sourceElement.Template);
		}
	}
}
