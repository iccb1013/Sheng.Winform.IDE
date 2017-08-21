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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Tests.WMI.TraceListeners
{
	[TestClass]
	public class CustomTraceListenerSettingFixture
	{
		[TestInitialize]
		public void SetUp()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(CustomTraceListenerSetting));
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
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM CustomTraceListenerSetting")
						.Get().GetEnumerator())
			{
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
		{
			string[] attributes = new string[] { "att1", "att2" };
			CustomTraceListenerSetting setting
				= new CustomTraceListenerSetting(null, "name", "ListenerType", "InitData", attributes, "TraceOutputOptions", System.Diagnostics.SourceLevels.Critical.ToString(), "Formatter");
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();
			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM CustomTraceListenerSetting")
						.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("name", resultEnumerator.Current.Properties["Name"].Value);
				Assert.AreEqual("ListenerType", resultEnumerator.Current.Properties["ListenerType"].Value);
				Assert.AreEqual("InitData", resultEnumerator.Current.Properties["InitData"].Value);
				Assert.ReferenceEquals(attributes, resultEnumerator.Current.Properties["Attributes"].Value);
				Assert.AreEqual("TraceOutputOptions", resultEnumerator.Current.Properties["TraceOutputOptions"].Value);
				Assert.AreEqual(SourceLevels.Critical.ToString(), resultEnumerator.Current.Properties["Filter"].Value);
				Assert.AreEqual("Formatter", resultEnumerator.Current.Properties["Formatter"].Value);
				Assert.AreEqual("CustomTraceListenerSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void CanBindObject()
		{
			string[] attributes = new string[] { "att1", "att2" };
			CustomTraceListenerSetting setting
				= new CustomTraceListenerSetting(null, "name", "ListenerType", "InitData", attributes, "TraceOutputOptions", System.Diagnostics.SourceLevels.Critical.ToString(), "Formatter");
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();
			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM CustomTraceListenerSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("CustomTraceListenerSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
				Assert.IsNotNull(managementObject);
				managementObject.Put();
			}
		}
		[TestMethod]
		public void SavesChangesToConfigurationObject()
		{
			CustomTraceListenerData sourceElement = new CustomTraceListenerData("name", typeof(bool), "init");
			sourceElement.Attributes.Add("att3", "val3");
			sourceElement.Attributes.Add("att4", "val4");
			sourceElement.Attributes.Add("att5", "val5");
			sourceElement.Formatter = "formatter";
		    sourceElement.Filter = SourceLevels.Information;
			sourceElement.TraceOutputOptions = TraceOptions.Callstack;
			List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
			CustomTraceListenerDataWmiMapper.GenerateWmiObjects(sourceElement, settings);
			Assert.AreEqual(1, settings.Count);
			CustomTraceListenerSetting setting = settings[0] as CustomTraceListenerSetting;
			Assert.IsNotNull(setting);
			setting.Attributes = new string[] { "att1=val1", "att2=val2" };
			setting.Formatter = "formatter override";
			setting.InitData = "init override";
			setting.ListenerType = typeof(int).AssemblyQualifiedName;
		    setting.Filter = SourceLevels.All.ToString();
			setting.TraceOutputOptions = TraceOptions.DateTime.ToString();
			setting.Commit();
			Assert.AreEqual(2, sourceElement.Attributes.Count);
			Assert.AreEqual("val1", sourceElement.Attributes["att1"]);
			Assert.AreEqual("val2", sourceElement.Attributes["att2"]);
			Assert.AreEqual("formatter override", sourceElement.Formatter);
			Assert.AreEqual("init override", sourceElement.InitData);
			Assert.AreEqual(typeof(int), sourceElement.Type);
		    Assert.AreEqual(SourceLevels.All, sourceElement.Filter);
			Assert.AreEqual(TraceOptions.DateTime, sourceElement.TraceOutputOptions);
		}
	}
}
