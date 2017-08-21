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
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Manageability.Tests.WMI
{
	[TestClass]
	public class OracleConnectionSettingFixture
	{
		[TestInitialize]
		public void SetUp()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(OracleConnectionSetting));
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
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM OracleConnectionSetting")
						.Get().GetEnumerator())
			{
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
		{
			string[] packages = new string[] { "pkg1", "pkg2" };
			OracleConnectionSetting setting = new OracleConnectionSetting(null, "name", packages);
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();
			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM OracleConnectionSetting")
						.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("name", resultEnumerator.Current.Properties["Name"].Value);
				Assert.ReferenceEquals(packages, resultEnumerator.Current.Properties["Packages"].Value);
				Assert.AreEqual("OracleConnectionSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void CanBindObject()
		{
			string[] packages = new string[] { "pkg1", "pkg2" };
			OracleConnectionSetting setting = new OracleConnectionSetting(null, "name", packages);
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();
			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM OracleConnectionSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("OracleConnectionSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
				Assert.IsNotNull(managementObject);
				managementObject.Put();
			}
		}
		[TestMethod]
		public void SavesChangesToConfigurationObject()
		{
			OracleConnectionData sourceElement = new OracleConnectionData();
			sourceElement.Packages.Add(new OraclePackageData("foo", "bar"));
			sourceElement.Packages.Add(new OraclePackageData("foo2", "bar2"));
			List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
			OracleConnectionSettingsWmiMapper.GenerateOracleConnectionSettingWmiObjects(sourceElement, settings);
			Assert.AreEqual(1, settings.Count);
			OracleConnectionSetting setting = settings[0] as OracleConnectionSetting;
			Assert.IsNotNull(setting);
			setting.Packages = new string[] { KeyValuePairEncoder.EncodeKeyValuePair("foo3", "bar3") };
			setting.Commit();
			Assert.AreEqual(1, sourceElement.Packages.Count);
			Assert.AreEqual("bar3", sourceElement.Packages.Get("foo3").Prefix);
		}
	}
}
