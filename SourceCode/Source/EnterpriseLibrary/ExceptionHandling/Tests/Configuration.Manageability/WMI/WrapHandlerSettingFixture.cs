/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Management;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability.Tests.Wmi
{
	[TestClass]
	public class WrapHandlerSettingFixture
	{
		[TestInitialize]
		public void SetUp()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof (WrapHandlerSetting));
			ExceptionHandlerSetting.ClearPublishedInstances();
		}
		[TestCleanup]
		public void TearDown()
		{
			ManagementEntityTypesRegistrar.UnregisterAll();
			ExceptionHandlerSetting.ClearPublishedInstances();
		}
		[TestMethod]
		public void WmiQueryReturnsEmptyResultIfNoPublishedInstances()
		{
			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM WrapHandlerSetting")
					.Get().GetEnumerator())
			{
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
		{
			WrapHandlerSetting setting = new WrapHandlerSetting(null, "name", "ExceptionMessage", "WrapExceptionType");
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();
			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\EnterpriseLibrary", "SELECT * FROM WrapHandlerSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("name", resultEnumerator.Current.Properties["Name"].Value);
				Assert.AreEqual("ExceptionMessage", resultEnumerator.Current.Properties["ExceptionMessage"].Value);
				Assert.AreEqual("WrapExceptionType", resultEnumerator.Current.Properties["WrapExceptionType"].Value);
				Assert.AreEqual("WrapHandlerSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void CanBindObject()
		{
			WrapHandlerSetting setting = new WrapHandlerSetting(null, "name", "ExceptionMessage", "WrapExceptionType");
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Policy = "Policy";
			setting.ExceptionType = typeof (Exception).AssemblyQualifiedName;
			setting.Publish();
			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM WrapHandlerSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("WrapHandlerSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
				Assert.IsNotNull(managementObject);
				managementObject.Put();
			}
		}
		[TestMethod]
		public void SavesChangesToConfigurationObject()
		{
			WrapHandlerData sourceElement = new WrapHandlerData("name",
			                                                    "ExceptionMessage",
			                                                    "System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
			List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
			WrapHandlerDataWmiMapper.GenerateWmiObjects(sourceElement, settings);
			Assert.AreEqual(1, settings.Count);
			WrapHandlerSetting setting = settings[0] as WrapHandlerSetting;
			Assert.IsNotNull(setting);
			setting.Commit();
			Assert.AreEqual(setting.ExceptionMessage, sourceElement.ExceptionMessage);
			Assert.AreEqual(setting.WrapExceptionType, sourceElement.WrapExceptionTypeName);
		}
	}
}
