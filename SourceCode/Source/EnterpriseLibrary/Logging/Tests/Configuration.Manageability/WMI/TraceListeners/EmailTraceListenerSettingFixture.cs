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
	public class EmailTraceListenerSettingFixture
	{
		[TestInitialize]
		public void SetUp()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(EmailTraceListenerSetting));
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
			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM EmailTraceListenerSetting")
					.Get().GetEnumerator())
			{
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
		{
			EmailTraceListenerSetting setting
				= new EmailTraceListenerSetting(null, "name", "formatter", "from", 25, "server", "subjectEOL", "subjectSOL", "to", "traceOutputOptions", System.Diagnostics.SourceLevels.Critical.ToString());
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();
			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM EmailTraceListenerSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("name", resultEnumerator.Current.Properties["Name"].Value);
				Assert.AreEqual("formatter", resultEnumerator.Current.Properties["Formatter"].Value);
				Assert.AreEqual("from", resultEnumerator.Current.Properties["FromAddress"].Value);
				Assert.AreEqual("to", resultEnumerator.Current.Properties["ToAddress"].Value);
				Assert.AreEqual("subjectEOL", resultEnumerator.Current.Properties["SubjectLineEnder"].Value);
				Assert.AreEqual("subjectSOL", resultEnumerator.Current.Properties["SubjectLineStarter"].Value);
				Assert.AreEqual("traceOutputOptions", resultEnumerator.Current.Properties["TraceOutputOptions"].Value);
				Assert.AreEqual(SourceLevels.Critical.ToString(), resultEnumerator.Current.Properties["Filter"].Value);
				Assert.AreEqual(25, resultEnumerator.Current.Properties["SmtpPort"].Value);
				Assert.AreEqual("server", resultEnumerator.Current.Properties["SmtpServer"].Value);
				Assert.AreEqual("EmailTraceListenerSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void CanBindObject()
		{
			EmailTraceListenerSetting setting
				= new EmailTraceListenerSetting(null, "name", "formatter", "from", 25, "server", "subjectEOL", "subjectSOL", "to", "traceOutputOptions", System.Diagnostics.SourceLevels.Critical.ToString());
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();
			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM EmailTraceListenerSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("EmailTraceListenerSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
				Assert.IsNotNull(managementObject);
				managementObject.Put();
			}
		}
		[TestMethod]
		public void SavesChangesToConfigurationObject()
		{
			EmailTraceListenerData sourceElement = new EmailTraceListenerData();
			sourceElement.Filter = SourceLevels.Error;
			sourceElement.Formatter = "formatter";
			sourceElement.FromAddress = "from";
			sourceElement.SmtpPort = 25;
			sourceElement.SmtpServer = "server";
			sourceElement.SubjectLineEnder = "ender";
			sourceElement.SubjectLineStarter = "starter";
			sourceElement.ToAddress = "to";
		    sourceElement.Filter = SourceLevels.Information;
			sourceElement.TraceOutputOptions = TraceOptions.Callstack;
			List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
			EmailTraceListenerDataWmiMapper.GenerateWmiObjects(sourceElement, settings);
			Assert.AreEqual(1, settings.Count);
			EmailTraceListenerSetting setting = settings[0] as EmailTraceListenerSetting;
			Assert.IsNotNull(setting);
			setting.Formatter = "updated formatter";
			setting.FromAddress = "updated from";
			setting.SmtpPort = 26;
			setting.SmtpServer = "updated server";
			setting.SubjectLineEnder = "updated ender";
			setting.SubjectLineStarter = "updated starter";
			setting.ToAddress = "updated to";
		    setting.Filter = SourceLevels.All.ToString();
			setting.TraceOutputOptions = TraceOptions.LogicalOperationStack.ToString();
			;
			setting.Commit();
			Assert.AreEqual("updated formatter", sourceElement.Formatter);
			Assert.AreEqual("updated from", sourceElement.FromAddress);
			Assert.AreEqual(26, sourceElement.SmtpPort);
			Assert.AreEqual("updated server", sourceElement.SmtpServer);
			Assert.AreEqual("updated ender", sourceElement.SubjectLineEnder);
			Assert.AreEqual("updated starter", sourceElement.SubjectLineStarter);
			Assert.AreEqual("updated to", sourceElement.ToAddress);
		    Assert.AreEqual(SourceLevels.All, sourceElement.Filter);
			Assert.AreEqual(TraceOptions.LogicalOperationStack, sourceElement.TraceOutputOptions);
		}
	}
}
