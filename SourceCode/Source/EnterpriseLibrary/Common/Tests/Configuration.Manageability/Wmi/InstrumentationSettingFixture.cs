/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Management;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Wmi
{
	[TestClass]
	public class InstrumentationSettingFixture
	{
		[TestInitialize]
		public void SetUp()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof (InstrumentationSetting));
			ConfigurationSectionSetting.ClearPublishedInstances();
		}
		[TestCleanup]
		public void TearDown()
		{
			ManagementEntityTypesRegistrar.UnregisterAll();
			ConfigurationSectionSetting.ClearPublishedInstances();
		}
		[TestMethod]
		public void WmiQueryReturnsEmptyResultIfNoPublishedInstances()
		{
			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM InstrumentationSetting")
					.Get().GetEnumerator())
			{
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
		{
			InstrumentationSetting setting = new InstrumentationSetting(null, true, false, true);
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();
			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM InstrumentationSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual(true, resultEnumerator.Current.Properties["EventLoggingEnabled"].Value);
				Assert.AreEqual(false, resultEnumerator.Current.Properties["PerformanceCountersEnabled"].Value);
				Assert.AreEqual(true, resultEnumerator.Current.Properties["WmiEnabled"].Value);
				Assert.AreEqual("InstrumentationSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void CanBindObject()
		{
			InstrumentationSetting setting = new InstrumentationSetting(null, true, false, true);
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();
			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM InstrumentationSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("InstrumentationSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				ManagementObject managementObject = (ManagementObject) resultEnumerator.Current;
				Assert.IsNotNull(managementObject);
				managementObject.Put();
			}
		}
		[TestMethod]
		public void SavesChangesToConfigurationObject()
		{
			InstrumentationConfigurationSection sourceElement = new InstrumentationConfigurationSection(false, true, false);
			List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
			InstrumentationConfigurationSectionWmiMapper.GenerateWmiObjects(sourceElement, settings);
			Assert.AreEqual(1, settings.Count);
			InstrumentationSetting setting = (InstrumentationSetting) settings[0];
			Assert.IsNotNull(setting);
			setting.EventLoggingEnabled = true;
			setting.PerformanceCountersEnabled = false;
			setting.WmiEnabled = true;
			setting.Commit();
			Assert.AreEqual(true, sourceElement.EventLoggingEnabled);
			Assert.AreEqual(false, sourceElement.PerformanceCountersEnabled);
			Assert.AreEqual(true, sourceElement.WmiEnabled);
		}
	}
}
