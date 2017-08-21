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
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Tests.WMI.TraceListeners
{
	[TestClass]
	public class RollingFlatFileTraceListenerSettingFixture
	{
		[TestInitialize]
		public void SetUp()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(RollingFlatFileTraceListenerSetting));
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
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM RollingFlatFileTraceListenerSetting")
						.Get().GetEnumerator())
			{
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
		{
			RollingFlatFileTraceListenerSetting setting
				= new RollingFlatFileTraceListenerSetting(null, "name", "FileName", "Header", "Footer", "Formatter",
								"RollFileExistsBehavior", "RollInterval", 256, "TimeStampPattern", "TraceOutputOptions", System.Diagnostics.SourceLevels.Critical.ToString());
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();
			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM RollingFlatFileTraceListenerSetting")
						.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("name", resultEnumerator.Current.Properties["Name"].Value);
				Assert.AreEqual("FileName", resultEnumerator.Current.Properties["FileName"].Value);
				Assert.AreEqual("Header", resultEnumerator.Current.Properties["Header"].Value);
				Assert.AreEqual("Footer", resultEnumerator.Current.Properties["Footer"].Value);
				Assert.AreEqual("Formatter", resultEnumerator.Current.Properties["Formatter"].Value);
				Assert.AreEqual("RollFileExistsBehavior", resultEnumerator.Current.Properties["RollFileExistsBehavior"].Value);
				Assert.AreEqual("RollInterval", resultEnumerator.Current.Properties["RollInterval"].Value);
				Assert.AreEqual(256, resultEnumerator.Current.Properties["RollSizeKB"].Value);
				Assert.AreEqual("TimeStampPattern", resultEnumerator.Current.Properties["TimeStampPattern"].Value);
				Assert.AreEqual("TraceOutputOptions", resultEnumerator.Current.Properties["TraceOutputOptions"].Value);
				Assert.AreEqual(SourceLevels.Critical.ToString(), resultEnumerator.Current.Properties["Filter"].Value);
				Assert.AreEqual("RollingFlatFileTraceListenerSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void CanBindObject()
		{
			RollingFlatFileTraceListenerSetting setting
				= new RollingFlatFileTraceListenerSetting(null, "name", "FileName", "Header", "Footer", "Formatter",
								"RollFileExistsBehavior", "RollInterval", 256, "TimeStampPattern", "TraceOutputOptions", System.Diagnostics.SourceLevels.Critical.ToString());
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();
			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM RollingFlatFileTraceListenerSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("RollingFlatFileTraceListenerSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
				Assert.IsNotNull(managementObject);
				managementObject.Put();
			}
		}
		[TestMethod]
		public void SavesChangesToConfigurationObject()
		{
			RollingFlatFileTraceListenerData sourceElement = new RollingFlatFileTraceListenerData();
			sourceElement.FileName = "file name";
			sourceElement.Footer = "footer";
			sourceElement.Formatter = "formatter";
			sourceElement.Header = "header";
			sourceElement.RollFileExistsBehavior = RollFileExistsBehavior.Increment;
			sourceElement.RollInterval = RollInterval.Day;
			sourceElement.RollSizeKB = 100;
			sourceElement.TimeStampPattern = "XXXXX";
			sourceElement.TraceOutputOptions = TraceOptions.ProcessId;
		    sourceElement.Filter = SourceLevels.Information;
			List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
			RollingFlatFileTraceListenerDataWmiMapper.GenerateWmiObjects(sourceElement, settings);
			Assert.AreEqual(1, settings.Count);
			RollingFlatFileTraceListenerSetting setting = settings[0] as RollingFlatFileTraceListenerSetting;
			Assert.IsNotNull(setting);
			setting.FileName = "updated file name";
			setting.Footer = "updated footer";
			setting.Formatter = "updated formatter";
			setting.Header = "updated header";
			setting.RollFileExistsBehavior = RollFileExistsBehavior.Overwrite.ToString();
			setting.RollInterval = RollInterval.Hour.ToString();
			setting.RollSizeKB = 200;
			setting.TimeStampPattern = "ZZZZZZ";
		    setting.Filter = SourceLevels.All.ToString();
			setting.TraceOutputOptions = TraceOptions.ThreadId.ToString();
			setting.Commit();
			Assert.AreEqual("updated file name", sourceElement.FileName);
			Assert.AreEqual("updated footer", sourceElement.Footer);
			Assert.AreEqual("updated formatter", sourceElement.Formatter);
			Assert.AreEqual("updated header", sourceElement.Header);
			Assert.AreEqual(RollFileExistsBehavior.Overwrite, sourceElement.RollFileExistsBehavior);
			Assert.AreEqual(RollInterval.Hour, sourceElement.RollInterval);
			Assert.AreEqual(200, sourceElement.RollSizeKB);
			Assert.AreEqual("ZZZZZZ", sourceElement.TimeStampPattern);
		    Assert.AreEqual(SourceLevels.All, sourceElement.Filter);
			Assert.AreEqual(TraceOptions.ThreadId, sourceElement.TraceOutputOptions);
		}
	}
}
