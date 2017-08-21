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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Tests.WMI.Filters
{
	[TestClass]
	public class CategoryFilterSettingFixture
	{
		[TestInitialize]
		public void SetUp()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(CategoryFilterSetting));
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
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM CategoryFilterSetting")
						.Get().GetEnumerator())
			{
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
		{
			string[] categoryFilters = new string[] { "cf1", "cf2" };
			CategoryFilterSetting setting = new CategoryFilterSetting(null, "name", "CategoryFilterMode", categoryFilters);
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();
			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM CategoryFilterSetting")
						.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("name", resultEnumerator.Current.Properties["Name"].Value);
				Assert.AreEqual("CategoryFilterMode", resultEnumerator.Current.Properties["CategoryFilterMode"].Value);
				Assert.AreEqual("CategoryFilterSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void CanBindObject()
		{
			string[] categoryFilters = new string[] { "cf1", "cf2" };
			CategoryFilterSetting setting = new CategoryFilterSetting(null, "name", "CategoryFilterMode", categoryFilters);
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();
			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM CategoryFilterSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("CategoryFilterSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
				Assert.IsNotNull(managementObject);
				managementObject.Put();
			}
		}
		[TestMethod]
		public void SavesChangesToConfigurationObject()
		{
			CategoryFilterData sourceElement = new CategoryFilterData();
			sourceElement.CategoryFilterMode = CategoryFilterMode.AllowAllExceptDenied;
			sourceElement.CategoryFilters.Add(new CategoryFilterEntry("foo"));
			sourceElement.CategoryFilters.Add(new CategoryFilterEntry("bar"));
			List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
			CategoryFilterDataWmiMapper.GenerateWmiObjects(sourceElement, settings);
			Assert.AreEqual(1, settings.Count);
			CategoryFilterSetting setting = settings[0] as CategoryFilterSetting;
			Assert.IsNotNull(setting);
			setting.CategoryFilterMode = CategoryFilterMode.DenyAllExceptAllowed.ToString();
			setting.CategoryFilters = new string[] { "foobar" };
			setting.Commit();
			Assert.AreEqual(CategoryFilterMode.DenyAllExceptAllowed, sourceElement.CategoryFilterMode);
			Assert.AreEqual(1, sourceElement.CategoryFilters.Count);
			Assert.IsTrue(sourceElement.CategoryFilters.Contains("foobar"));
		}
	}
}
