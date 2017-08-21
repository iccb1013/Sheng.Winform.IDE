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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Manageability.Tests.Wmi
{
	[TestClass]
	public class FormattedDatabaseTraceListenerSettingFixture
	{
		[TestInitialize]
		public void SetUp()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(FormattedDatabaseTraceListenerSetting));
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
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM FormattedDatabaseTraceListenerSetting")
						.Get().GetEnumerator())
			{
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
		{
			FormattedDatabaseTraceListenerSetting setting
				= new FormattedDatabaseTraceListenerSetting(null, "name", "database",
					"write", "addCategory", "formatter", TraceOptions.Callstack.ToString(),
					SourceLevels.Critical.ToString());
			setting.ApplicationName = "app";
			setting.SectionName = LoggingSettings.SectionName;
			setting.Publish();
			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM FormattedDatabaseTraceListenerSetting")
						.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("name", resultEnumerator.Current.Properties["Name"].Value);
				Assert.AreEqual("formatter", resultEnumerator.Current.Properties["Formatter"].Value);
				Assert.AreEqual("database", resultEnumerator.Current.Properties["DatabaseInstanceName"].Value);
				Assert.AreEqual("write", resultEnumerator.Current.Properties["WriteLogStoredProcName"].Value);
				Assert.AreEqual("addCategory", resultEnumerator.Current.Properties["AddCategoryStoredProcName"].Value);
				Assert.AreEqual(TraceOptions.Callstack.ToString(), resultEnumerator.Current.Properties["TraceOutputOptions"].Value);
				Assert.AreEqual(SourceLevels.Critical.ToString(), resultEnumerator.Current.Properties["Filter"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void CanBindObject()
		{
			FormattedDatabaseTraceListenerSetting setting
				= new FormattedDatabaseTraceListenerSetting(null, "name", "database",
					"write", "addCategory", "formatter", TraceOptions.Callstack.ToString(),
					SourceLevels.Critical.ToString());
			setting.ApplicationName = "app";
			setting.SectionName = LoggingSettings.SectionName;
			setting.Publish();
			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM FormattedDatabaseTraceListenerSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("FormattedDatabaseTraceListenerSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
				Assert.IsNotNull(managementObject);
				managementObject.Put();
			}
		}
        [TestMethod]
        public void SavesChangesToConfigurationObject()
        {
            FormattedDatabaseTraceListenerData sourceElement = new FormattedDatabaseTraceListenerData();
            sourceElement.Filter = SourceLevels.Information;
            sourceElement.Formatter = "formatter";
            sourceElement.AddCategoryStoredProcName = "add sproc";
            sourceElement.DatabaseInstanceName = "instance";
            sourceElement.WriteLogStoredProcName = "write sproc";
            sourceElement.TraceOutputOptions = TraceOptions.ThreadId;
            List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
            FormattedDatabaseTraceListenerDataWmiMapper.GenerateWmiObjects(sourceElement, settings);
            Assert.AreEqual(1, settings.Count);
            FormattedDatabaseTraceListenerSetting setting = settings[0] as FormattedDatabaseTraceListenerSetting;
            Assert.IsNotNull(setting);
            setting.Filter = SourceLevels.All.ToString();
            setting.Formatter = "updated formatter";
            setting.AddCategoryStoredProcName = "new add sproc";
            setting.DatabaseInstanceName = "updated instance";
            setting.WriteLogStoredProcName = "new write sproc";
            setting.TraceOutputOptions = TraceOptions.ProcessId.ToString();
            setting.Commit();
            Assert.AreEqual("updated formatter", sourceElement.Formatter);
            Assert.AreEqual("new add sproc", sourceElement.AddCategoryStoredProcName);
            Assert.AreEqual("updated instance", sourceElement.DatabaseInstanceName);
            Assert.AreEqual("new write sproc", sourceElement.WriteLogStoredProcName);
            Assert.AreEqual(SourceLevels.All, sourceElement.Filter);
            Assert.AreEqual(TraceOptions.ProcessId, sourceElement.TraceOutputOptions);
        }
	}
}
