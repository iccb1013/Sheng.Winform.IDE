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
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Manageability.Tests.WMI
{
	[TestClass]
	public class DatabaseBlockSettingFixture
	{
		[TestInitialize]
		public void SetUp()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(DatabaseBlockSetting));
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
			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM DatabaseBlockSetting")
						.Get().GetEnumerator())
			{
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
		{
			DatabaseBlockSetting setting = new DatabaseBlockSetting(null, "defaultDataBase");
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();
			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM DatabaseBlockSetting")
						.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("defaultDataBase", resultEnumerator.Current.Properties["DefaultDatabase"].Value);
				Assert.AreEqual("DatabaseBlockSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void CanBindObject()
		{
			DatabaseBlockSetting setting = new DatabaseBlockSetting(null, "defaultDataBase");
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();
			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM DatabaseBlockSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("DatabaseBlockSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
				Assert.IsNotNull(managementObject);
				managementObject.Put();
			}
		}
		[TestMethod]
		public void SavesChangesToConfigurationObject()
		{
			DatabaseSettings sourceElement = new DatabaseSettings();
			sourceElement.DefaultDatabase = "foo database";
			List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
			DatabaseSettingsWmiMapper.GenerateWmiObjects(sourceElement, settings);
			Assert.AreEqual(1, settings.Count);
			DatabaseBlockSetting setting = settings[0] as DatabaseBlockSetting;
			Assert.IsNotNull(setting);
			setting.DefaultDatabase = "bar database";
			setting.Commit();
			Assert.AreEqual("bar database", sourceElement.DefaultDatabase);
		}
	}
}
