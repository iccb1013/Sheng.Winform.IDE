/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Management;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Formatters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Tests.WMI.Formatters
{
	[TestClass]
	public class CustomFormatterSettingFixture
	{
		[TestInitialize]
		public void SetUp()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(CustomFormatterSetting));
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
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM CustomFormatterSetting")
						.Get().GetEnumerator())
			{
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
		{
			string[] attributes = new string[] { "att1", "att2" };
			CustomFormatterSetting setting = new CustomFormatterSetting(null, "name", "FormatterType", attributes);
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();
			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM CustomFormatterSetting")
						.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("name", resultEnumerator.Current.Properties["Name"].Value);
				Assert.AreEqual("FormatterType", resultEnumerator.Current.Properties["FormatterType"].Value);
				Assert.ReferenceEquals(attributes, resultEnumerator.Current.Properties["Attributes"].Value);
				Assert.AreEqual("CustomFormatterSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}
		[TestMethod]
		public void CanBindObject()
		{
			string[] attributes = new string[] { "att1", "att2" };
			CustomFormatterSetting setting = new CustomFormatterSetting(null, "name", "FormatterType", attributes);
			setting.ApplicationName = "app";
			setting.SectionName = InstrumentationConfigurationSection.SectionName;
			setting.Publish();
			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM CustomFormatterSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("CustomFormatterSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
				Assert.IsNotNull(managementObject);
				managementObject.Put();
			}
		}
		[TestMethod]
		public void SavesChangesToConfigurationObject()
		{
			CustomFormatterData sourceElement = new CustomFormatterData("name", typeof(bool));
			sourceElement.Attributes.Add("att3", "val3");
			sourceElement.Attributes.Add("att4", "val4");
			sourceElement.Attributes.Add("att5", "val5");
			List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
			CustomFormatterDataWmiMapper.GenerateWmiObjects(sourceElement, settings);
			Assert.AreEqual(1, settings.Count);
			CustomFormatterSetting setting = settings[0] as CustomFormatterSetting;
			Assert.IsNotNull(setting);
			setting.Attributes = new string[] { "att1=val1", "att2=val2" };
			setting.FormatterType = typeof(int).AssemblyQualifiedName;
			setting.Commit();
			Assert.AreEqual(2, sourceElement.Attributes.Count);
			Assert.AreEqual("val1", sourceElement.Attributes["att1"]);
			Assert.AreEqual("val2", sourceElement.Attributes["att2"]);
			Assert.AreEqual(typeof(int), sourceElement.Type);
		}
	}
}
