/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Management;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability.Tests.Wmi
{
	[TestClass]
	public class DuplicateNamesFixture
	{
		[TestInitialize]
		public void SetUp()
		{
			AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
		}
		[TestCleanup]
		public void TearDown()
		{
			ManagementEntityTypesRegistrar.UnregisterAll();
			NamedConfigurationSetting.ClearPublishedInstances();
			ExceptionHandlerSetting.ClearPublishedInstances();
			ExceptionTypeSetting.ClearPublishedInstances();
			ManageableConfigurationSource.ResetAllImplementations();
		}
		[TestMethod]
		public void CanPublishConfigurationWithDuplicateHandlerNamesInDifferentPolicies()
		{
			IConfigurationSource configurationSource = ConfigurationSourceFactory.Create("Manageable");
			using (ManagementObjectCollection resultCollection
				=
				new ManagementObjectSearcher("root\\enterpriselibrary",
				                             "SELECT * FROM ExceptionHandlerSetting WHERE ApplicationName = \"UNITTEST\"")
					.Get())
			{
				List<ManagementObject> managementObjects = new List<ManagementObject>();
				foreach (ManagementObject managementObject in resultCollection)
				{
					managementObjects.Add(managementObject);
				}
				Assert.AreEqual(8, managementObjects.Count);
			}
		}
		[TestMethod]
		public void CanPublishConfigurationWithDuplicateTypeNamesInDifferentPolicies()
		{
			IConfigurationSource configurationSource = ConfigurationSourceFactory.Create("Manageable");
			using (ManagementObjectCollection resultCollection
				=
				new ManagementObjectSearcher("root\\enterpriselibrary",
				                             "SELECT * FROM ExceptionTypeSetting WHERE ApplicationName = \"UNITTEST\"")
					.Get())
			{
				List<ManagementObject> managementObjects = new List<ManagementObject>();
				foreach (ManagementObject managementObject in resultCollection)
				{
					managementObjects.Add(managementObject);
				}
				Assert.AreEqual(4, managementObjects.Count);
			}
		}
	}
}
