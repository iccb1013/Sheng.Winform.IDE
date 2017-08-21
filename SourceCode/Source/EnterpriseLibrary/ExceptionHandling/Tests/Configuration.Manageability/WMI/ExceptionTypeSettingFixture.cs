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
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability.Tests.Wmi
{
	[TestClass]
	public class ExceptionTypeSettingFixture
	{
		[TestInitialize]
		public void SetUp()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof (ExceptionTypeSetting));
			ExceptionTypeSetting.ClearPublishedInstances();
		}
		[TestCleanup]
		public void TearDown()
		{
			ManagementEntityTypesRegistrar.UnregisterAll();
			ExceptionTypeSetting.ClearPublishedInstances();
		}
		[TestMethod]
		public void WmiQueryReturnsEmptyResultIfNoPublishedInstances()
		{
			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM ExceptionTypeSetting")
					.Get().GetEnumerator())
			{
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
		{
			ExceptionTypeSetting setting = new ExceptionTypeSetting(null, "name", "System.String", "PostHandlingAction");
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();
			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM ExceptionTypeSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("name", resultEnumerator.Current.Properties["Name"].Value);
				Assert.AreEqual("PostHandlingAction", resultEnumerator.Current.Properties["PostHandlingAction"].Value);
				Assert.AreEqual("ExceptionTypeSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void CanBindObject()
		{
			ExceptionTypeSetting setting = new ExceptionTypeSetting(null, "name", "System.String", "PostHandlingAction");
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Policy = "policy";
			setting.Publish();
			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM ExceptionTypeSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("ExceptionTypeSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
				Assert.IsNotNull(managementObject);
				managementObject.Put();
			}
		}
		[TestMethod]
		public void SavesChangesToConfigurationObject()
		{
			ExceptionPolicyData policyData = new ExceptionPolicyData("name");
			ExceptionTypeData sourceElement = new ExceptionTypeData("name", "System.String", PostHandlingAction.None);
			List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
			ExceptionHandlingSettingsWmiMapper.GenerateExceptionTypeWmiObjects(sourceElement, policyData, settings);
			Assert.AreEqual(1, settings.Count);
			ExceptionTypeSetting setting = settings[0] as ExceptionTypeSetting;
			Assert.IsNotNull(setting);
			setting.Commit();
		}
	}
}
