/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Tests.WMI
{
	[TestClass]
	public class TraceSourceSettingFixture
	{
		[TestInitialize]
		public void SetUp()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(TraceSourceSetting));
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
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM TraceSourceSetting")
						.Get().GetEnumerator())
			{
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
		{
			string[] traceListeners = new string[] { "tl1", "tl2" };
			TraceSourceSetting setting = new TraceSourceSetting(null, "name", "DefaultLevel", traceListeners, "Kind");
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();
			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM TraceSourceSetting")
						.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("name", resultEnumerator.Current.Properties["Name"].Value);
				Assert.AreEqual("DefaultLevel", resultEnumerator.Current.Properties["DefaultLevel"].Value);
				Assert.ReferenceEquals(traceListeners, resultEnumerator.Current.Properties["TraceListeners"].Value);
				Assert.AreEqual("Kind", resultEnumerator.Current.Properties["Kind"].Value);
				Assert.AreEqual("TraceSourceSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void CanBindObject()
		{
			string[] traceListeners = new string[] { "tl1", "tl2" };
			TraceSourceSetting setting = new TraceSourceSetting(null, "name", "DefaultLevel", traceListeners, "Kind");
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();
			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM TraceSourceSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("TraceSourceSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
				Assert.IsNotNull(managementObject);
				managementObject.Put();
			}
		}
		[TestMethod]
		public void SavesChangesToConfigurationObject()
		{
			TraceSourceData sourceElement = new TraceSourceData();
			sourceElement.Name = "source";
			sourceElement.DefaultLevel = SourceLevels.Error;
			sourceElement.TraceListeners.Add(new TraceListenerReferenceData("listener1"));
			sourceElement.TraceListeners.Add(new TraceListenerReferenceData("listener2"));
			List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
			LoggingSettingsWmiMapper.GenerateTraceSourceDataWmiObjects(sourceElement, settings, "Category");
			Assert.AreEqual(1, settings.Count);
			TraceSourceSetting setting = settings[0] as TraceSourceSetting;
			Assert.IsNotNull(setting);
			setting.DefaultLevel = SourceLevels.All.ToString();
			setting.TraceListeners = new string[] { "listener1", "listener3", "listener4" };
			setting.Commit();
			Assert.AreEqual(SourceLevels.All, sourceElement.DefaultLevel);
			Assert.AreEqual(3, sourceElement.TraceListeners.Count);
			Assert.IsTrue(sourceElement.TraceListeners.Contains("listener1"));
			Assert.IsTrue(sourceElement.TraceListeners.Contains("listener3"));
			Assert.IsTrue(sourceElement.TraceListeners.Contains("listener4"));
		}
	}
}
