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
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Manageability.Tests.Wmi
{
	[TestClass]
	public class FaultContractExceptionHandlerSettingFixture
	{
		[TestInitialize]
		public void SetUp()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof (FaultContractExceptionHandlerSetting));
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
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM FaultContractExceptionHandlerSetting")
					.Get().GetEnumerator())
			{
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
		{
			string[] attributes = new string[] {"att1=val1", "att2=val1"};
			FaultContractExceptionHandlerSetting setting =
				new FaultContractExceptionHandlerSetting(null, "name", "ExceptionMessage", "FaultContract", attributes);
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();
			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM FaultContractExceptionHandlerSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("name", resultEnumerator.Current.Properties["Name"].Value);
				Assert.AreEqual("FaultContract", resultEnumerator.Current.Properties["FaultContractType"].Value);
				Assert.AreEqual("ExceptionMessage", resultEnumerator.Current.Properties["ExceptionMessage"].Value);
				Assert.AreEqual("FaultContractExceptionHandlerSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void CanBindObject()
		{
			string[] attributes = new string[] {"att1=val1", "att2=val1"};
			FaultContractExceptionHandlerSetting setting
				= new FaultContractExceptionHandlerSetting(null, "name", "ExceptionMessage", "FaultContract", attributes);
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Policy = "policy";
			setting.ExceptionType = typeof (Exception).AssemblyQualifiedName;
			setting.Publish();
			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM FaultContractExceptionHandlerSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("FaultContractExceptionHandlerSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
				Assert.IsNotNull(managementObject);
				managementObject.Put();
			}
		}
		[TestMethod]
		public void SavesChangesToConfigurationObject()
		{
			FaultContractExceptionHandlerData sourceElement = new FaultContractExceptionHandlerData("name",
			                                                                                        "System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
			sourceElement.Attributes.Add("att3", "val3");
			sourceElement.Attributes.Add("att4", "val4");
			sourceElement.Attributes.Add("att5", "val5");
			List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
			FaultContractExceptionHandlerDataWmiMapper.GenerateWmiObjects(sourceElement, settings);
			Assert.AreEqual(1, settings.Count);
			FaultContractExceptionHandlerSetting setting = settings[0] as FaultContractExceptionHandlerSetting;
			Assert.IsNotNull(setting);
			setting.Attributes = new string[] {"att1=val1", "att2=val2"};
			setting.Commit();
			Assert.AreEqual(2, sourceElement.Attributes.Count);
			Assert.AreEqual("val1", sourceElement.Attributes["att1"]);
			Assert.AreEqual("val2", sourceElement.Attributes["att2"]);
		}
	}
}
