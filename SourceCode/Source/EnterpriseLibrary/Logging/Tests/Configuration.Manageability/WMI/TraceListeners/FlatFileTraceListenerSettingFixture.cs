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
	public class FlatFileTraceListenerSettingFixture
	{
		[TestInitialize]
		public void SetUp()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(FlatFileTraceListenerSetting));
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
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM FlatFileTraceListenerSetting")
						.Get().GetEnumerator())
			{
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
		{
			FlatFileTraceListenerSetting setting
				= new FlatFileTraceListenerSetting(null, "name", "FileName", "Header", "Footer", "Formatter", "TraceOutputOptions", System.Diagnostics.SourceLevels.Critical.ToString());
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();
			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM FlatFileTraceListenerSetting")
						.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("name", resultEnumerator.Current.Properties["Name"].Value);
				Assert.AreEqual("FileName", resultEnumerator.Current.Properties["FileName"].Value);
				Assert.AreEqual("Header", resultEnumerator.Current.Properties["Header"].Value);
				Assert.AreEqual("Footer", resultEnumerator.Current.Properties["Footer"].Value);
				Assert.AreEqual("Formatter", resultEnumerator.Current.Properties["Formatter"].Value);
				Assert.AreEqual("TraceOutputOptions", resultEnumerator.Current.Properties["TraceOutputOptions"].Value);
				Assert.AreEqual(SourceLevels.Critical.ToString(), resultEnumerator.Current.Properties["Filter"].Value);
				Assert.AreEqual("FlatFileTraceListenerSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void CanBindObject()
		{
			FlatFileTraceListenerSetting setting
				= new FlatFileTraceListenerSetting(null, "name", "FileName", "Header", "Footer", "Formatter", "TraceOutputOptions", System.Diagnostics.SourceLevels.Critical.ToString());
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();
			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM FlatFileTraceListenerSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("FlatFileTraceListenerSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
				Assert.IsNotNull(managementObject);
				managementObject.Put();
			}
		}
		[TestMethod]
		public void SavesChangesToConfigurationObject()
		{
			FlatFileTraceListenerData sourceElement = new FlatFileTraceListenerData();
			sourceElement.FileName = "file name";
			sourceElement.Footer = "footer";
			sourceElement.Formatter = "formatter";
			sourceElement.Header = "header";
		    sourceElement.Filter = SourceLevels.Information;
			sourceElement.TraceOutputOptions = TraceOptions.ProcessId;
			List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
			FlatFileTraceListenerDataWmiMapper.GenerateWmiObjects(sourceElement, settings);
			Assert.AreEqual(1, settings.Count);
			FlatFileTraceListenerSetting setting = settings[0] as FlatFileTraceListenerSetting;
			Assert.IsNotNull(setting);
			setting.FileName = "updated file name";
			setting.Footer = "updated footer";
			setting.Formatter = "updated formatter";
			setting.Header = "updated header";
		    setting.Filter = SourceLevels.All.ToString();
			setting.TraceOutputOptions = TraceOptions.ThreadId.ToString();
			setting.Commit();
			Assert.AreEqual("updated file name", sourceElement.FileName);
			Assert.AreEqual("updated footer", sourceElement.Footer);
			Assert.AreEqual("updated formatter", sourceElement.Formatter);
			Assert.AreEqual("updated header", sourceElement.Header);
		    Assert.AreEqual(SourceLevels.All, sourceElement.Filter);
			Assert.AreEqual(TraceOptions.ThreadId, sourceElement.TraceOutputOptions);
		}
	}
}
