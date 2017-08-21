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
	public class ReplaceHandlerSettingFixture
	{
		[TestInitialize]
		public void SetUp()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof (ReplaceHandlerSetting));
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
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM ReplaceHandlerSetting")
					.Get().GetEnumerator())
			{
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
		{
			ReplaceHandlerSetting setting = new ReplaceHandlerSetting(null, "name", "ExceptionMessage", "ReplaceExceptionType");
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();
			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM ReplaceHandlerSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("name", resultEnumerator.Current.Properties["Name"].Value);
				Assert.AreEqual("ExceptionMessage", resultEnumerator.Current.Properties["ExceptionMessage"].Value);
				Assert.AreEqual("ReplaceExceptionType", resultEnumerator.Current.Properties["ReplaceExceptionType"].Value);
				Assert.AreEqual("ReplaceHandlerSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void CanBindObject()
		{
			ReplaceHandlerSetting setting = new ReplaceHandlerSetting(null, "name", "ExceptionMessage", "ReplaceExceptionType");
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Policy = "Policy";
			setting.ExceptionType = typeof (Exception).AssemblyQualifiedName;
			setting.Publish();
			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM ReplaceHandlerSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("ReplaceHandlerSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
				Assert.IsNotNull(managementObject);
				managementObject.Put();
			}
		}
		[TestMethod]
		public void SavesChangesToConfigurationObject()
		{
			ReplaceHandlerData sourceElement = new ReplaceHandlerData("name",
			                                                          "ExcpetionMessage",
			                                                          "System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
			List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
			ReplaceHandlerDataWmiMapper.GenerateWmiObjects(sourceElement, settings);
			Assert.AreEqual(1, settings.Count);
			ReplaceHandlerSetting setting = settings[0] as ReplaceHandlerSetting;
			Assert.IsNotNull(setting);
			setting.ExceptionMessage = "Changed";
			setting.Commit();
			Assert.AreEqual(setting.ExceptionMessage, sourceElement.ExceptionMessage);
			Assert.AreEqual(setting.ReplaceExceptionType, sourceElement.ReplaceExceptionTypeName);
		}
	}
}
