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
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Manageability.Tests.Wmi
{
	[TestClass]
	public class LoggingExceptionHandlerSettingFixture
	{
		[TestInitialize]
		public void SetUp()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof (LoggingExceptionHandlerSetting));
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
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM LoggingExceptionHandlerSetting")
					.Get().GetEnumerator())
			{
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
		{
			LoggingExceptionHandlerSetting setting = new LoggingExceptionHandlerSetting(null,
			                                                                            "name",
			                                                                            1,
			                                                                            typeof (String).AssemblyQualifiedName,
			                                                                            "LogCategory",
			                                                                            1,
			                                                                            "Severity",
			                                                                            "Title");
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();
			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM LoggingExceptionHandlerSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("name", resultEnumerator.Current.Properties["Name"].Value);
				Assert.AreEqual("LogCategory", resultEnumerator.Current.Properties["LogCategory"].Value);
				Assert.AreEqual("LoggingExceptionHandlerSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void CanBindObject()
		{
			LoggingExceptionHandlerSetting setting
				= new LoggingExceptionHandlerSetting(null,
				                                     "name",
				                                     1,
				                                     typeof (String).AssemblyQualifiedName,
				                                     "LogCategory",
				                                     1,
				                                     "Severity",
				                                     "Title");
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Policy = "policy";
			setting.ExceptionType = typeof (Exception).AssemblyQualifiedName;
			setting.Publish();
			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM LoggingExceptionHandlerSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("LoggingExceptionHandlerSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
				Assert.IsNotNull(managementObject);
				managementObject.Put();
			}
		}
		[TestMethod]
		public void SavesChangesToConfigurationObject()
		{
			LoggingExceptionHandlerData sourceElement = new LoggingExceptionHandlerData("name",
			                                                                            "LogCategory",
			                                                                            0,
			                                                                            System.Diagnostics.TraceEventType.Error,
			                                                                            "Title",
			                                                                            typeof (String).AssemblyQualifiedName,
			                                                                            0);
			List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
			LoggingExceptionHandlerDataWmiMapper.GenerateWmiObjects(sourceElement, settings);
			Assert.AreEqual(1, settings.Count);
			LoggingExceptionHandlerSetting setting = settings[0] as LoggingExceptionHandlerSetting;
			Assert.IsNotNull(setting);
			setting.Title = "Changed";
			setting.FormatterType = typeof (bool).AssemblyQualifiedName;
			setting.Commit();
			Assert.AreEqual(setting.FormatterType, sourceElement.FormatterTypeName);
			Assert.AreEqual(setting.Title, sourceElement.Title);
		}
	}
}
