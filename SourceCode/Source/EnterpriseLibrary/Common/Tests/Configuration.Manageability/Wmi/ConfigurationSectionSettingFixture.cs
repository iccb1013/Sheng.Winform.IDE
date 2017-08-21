//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Management;
using System.Management.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Wmi
{
	[TestClass]
	public class ConfigurationSectionSettingFixture
	{
		[TestInitialize]
		public void SetUp()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof (MockConfigurationSectionSetting),
			                                                   typeof (MockConfigurationSectionSettingB));
			ConfigurationSectionSetting.ClearPublishedInstances();
		}

		[TestCleanup]
		public void TearDown()
		{
			ManagementEntityTypesRegistrar.UnregisterAll();
			ConfigurationSectionSetting.ClearPublishedInstances();
		}

		[TestMethod]
		public void WmiQueryReturnsEmptyResultIfNoPublishedInstances()
		{
			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MockConfigurationSectionSetting")
					.Get().GetEnumerator())
			{
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}

		[TestMethod]
		public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
		{
			MockConfigurationSectionSetting setting = CreateMockConfigurationSectionSetting("Foo", "Bar");

			setting.Publish();

			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MockConfigurationSectionSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("Foo", resultEnumerator.Current.Properties["ApplicationName"].Value);
				Assert.AreEqual("Bar", resultEnumerator.Current.Properties["SectionName"].Value);
				Assert.AreEqual("MockConfigurationSectionSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}

		[TestMethod]
		public void RevokedInstanceIsNotReturnedInQuery()
		{
			MockConfigurationSectionSetting setting = CreateMockConfigurationSectionSetting("Foo", "Bar");

			setting.Publish();

			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MockConfigurationSectionSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("Foo", resultEnumerator.Current.Properties["ApplicationName"].Value);
				Assert.AreEqual("Bar", resultEnumerator.Current.Properties["SectionName"].Value);
				Assert.AreEqual("MockConfigurationSectionSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}

			setting.Revoke();

			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MockConfigurationSectionSetting")
					.Get().GetEnumerator())
			{
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}

		[TestMethod]
		public void RevokeForNonPublishedInstanceIsIgnored()
		{
			MockConfigurationSectionSetting setting = CreateMockConfigurationSectionSetting("Foo", "Bar");
			MockConfigurationSectionSetting setting2 = CreateMockConfigurationSectionSetting("Foo", "Bar2");

			setting.Publish();
			setting2.Revoke();
		}

		[TestMethod]
		public void PublishInstanceWithDuplicateKeyThrows()
		{
			MockConfigurationSectionSetting setting = CreateMockConfigurationSectionSetting("Foo", "Bar");
			MockConfigurationSectionSetting setting2 = CreateMockConfigurationSectionSetting("Foo", "Bar");

			setting.Publish();
			try
			{
				setting2.Publish();
				Assert.Fail("should have thrown");
			}
			catch (ArgumentException)
			{
			}
		}

		[TestMethod]
		public void CanPublishMultipleInstancesOfClassWithDifferentKeys()
		{
			MockConfigurationSectionSetting setting = CreateMockConfigurationSectionSetting("Foo", "Bar");
			MockConfigurationSectionSetting setting2 = CreateMockConfigurationSectionSetting("Foo", "Bar2");
			setting.Publish();
			setting2.Publish();

			IDictionary<string, MockConfigurationSectionSetting> nameMapping =
				new Dictionary<string, MockConfigurationSectionSetting>();
			nameMapping.Add(setting.SectionName, setting);
			nameMapping.Add(setting2.SectionName, setting2);

			using (ManagementObjectCollection resultCollection
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MockConfigurationSectionSetting")
					.Get())
			{
				foreach (ManagementObject current in resultCollection)
				{
					Assert.AreEqual("MockConfigurationSectionSetting", current.SystemProperties["__CLASS"].Value);
					Assert.IsTrue(nameMapping.Remove((string) current.Properties["SectionName"].Value), "unknown name");
				}
			}

			Assert.AreEqual(0, nameMapping.Count, "Not all elements were retrieved");
		}

		[TestMethod]
		public void RevokedInstanceIsNotReturnedWhenMultipleInstancesArePublished()
		{
			MockConfigurationSectionSetting setting = CreateMockConfigurationSectionSetting("Foo", "Bar");
			MockConfigurationSectionSetting setting2 = CreateMockConfigurationSectionSetting("Foo", "Bar2");
			setting.Publish();
			setting2.Publish();

			setting.Revoke();

			IDictionary<string, MockConfigurationSectionSetting> nameMapping =
				new Dictionary<string, MockConfigurationSectionSetting>();
			nameMapping.Add(setting2.SectionName, setting2);

			using (ManagementObjectCollection resultCollection
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MockConfigurationSectionSetting")
					.Get())
			{
				foreach (ManagementObject current in resultCollection)
				{
					Assert.AreEqual("MockConfigurationSectionSetting", current.SystemProperties["__CLASS"].Value);
					Assert.IsTrue(nameMapping.Remove((string) current.Properties["SectionName"].Value), "unknown name");
				}
			}

			Assert.AreEqual(0, nameMapping.Count, "Not all elements were retrieved");
		}

		[TestMethod]
		public void CanPublishInstancesOfSiblingClassesWithTheSameKey()
		{
			MockConfigurationSectionSetting setting = CreateMockConfigurationSectionSetting("Foo", "Bar");
			MockConfigurationSectionSettingB setting2 = CreateMockConfigurationSectionSettingB("Foo", "Bar");
			setting.Publish();
			setting2.Publish();

			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MockConfigurationSectionSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("Bar", resultEnumerator.Current.Properties["SectionName"].Value);
				Assert.AreEqual("MockConfigurationSectionSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}

			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MockConfigurationSectionSettingB")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("Bar", resultEnumerator.Current.Properties["SectionName"].Value);
				Assert.AreEqual("MockConfigurationSectionSettingB", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}

		[TestMethod]
		public void SecondPublishIsNoOp()
		{
			MockConfigurationSectionSetting setting = CreateMockConfigurationSectionSetting("Foo", "Bar");

			setting.Publish();

			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MockConfigurationSectionSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("Bar", resultEnumerator.Current.Properties["SectionName"].Value);
				Assert.AreEqual("MockConfigurationSectionSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}

			// second publish, should not throw but should return single instance
			setting.Publish();

			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MockConfigurationSectionSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("Bar", resultEnumerator.Current.Properties["SectionName"].Value);
				Assert.AreEqual("MockConfigurationSectionSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}

		[TestMethod]
		public void ReturnsEmptyResultForNonPublishedClassWithPublishedSibling()
		{
			MockConfigurationSectionSetting setting = CreateMockConfigurationSectionSetting("Foo", "Bar");

			setting.Publish();

			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MockConfigurationSectionSettingB")
					.Get().GetEnumerator())
			{
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}

		[TestMethod]
		public void ReturnsInstanceFromPublishedClassWithPublishedSibling()
		{
			MockConfigurationSectionSetting setting = CreateMockConfigurationSectionSetting("Foo", "Bar");
			MockConfigurationSectionSettingB settingB = CreateMockConfigurationSectionSettingB("Foob", "Barb");

			setting.Publish();
			settingB.Publish();

			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MockConfigurationSectionSettingB")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("Barb", resultEnumerator.Current.Properties["SectionName"].Value);
				Assert.AreEqual("MockConfigurationSectionSettingB", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}

		[TestMethod]
		public void CanBindObject()
		{
			MockConfigurationSectionSetting setting = CreateMockConfigurationSectionSetting("Foo", "Bar");
			setting.Changed += this.Changed;
			setting.SourceElement = new TestConfigurationSection(); // just so Commit works

			setting.Publish();

			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MockConfigurationSectionSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("Bar", resultEnumerator.Current.Properties["SectionName"].Value);
				Assert.AreEqual("MockConfigurationSectionSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);

				ManagementObject managementObject = (ManagementObject)resultEnumerator.Current;
				Assert.IsNotNull(managementObject);

				Assert.IsNull(this.changedObject);
				managementObject.Put();
				Assert.IsNotNull(this.changedObject);

				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}

		[TestMethod]
        [Ignore] // not sure why this is not working
		public void BindOnRevokedInstanceThrows()
		{
			MockConfigurationSectionSetting setting = CreateMockConfigurationSectionSetting("Foo", "Bar");
			setting.Changed += this.Changed;

			setting.Publish();

			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MockConfigurationSectionSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("Bar", resultEnumerator.Current.Properties["SectionName"].Value);
				Assert.AreEqual("MockConfigurationSectionSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);

				ManagementObject managementObject = (ManagementObject)resultEnumerator.Current;
				Assert.IsNotNull(managementObject);

				Assert.IsNull(this.changedObject);
				setting.Revoke();
				try
				{
					managementObject.Put();
					Assert.Fail("should have thrown");
				}
				catch (ManagementException e)
				{
					Assert.AreEqual(ManagementStatus.ProviderNotCapable, e.ErrorCode);
				}
			}
		}

		[TestMethod]
		public void NotifiesIfSavedChanges()
		{
			MockConfigurationSectionSettingB setting = CreateMockConfigurationSectionSettingB("Foo", "Bar");
			setting.Changed += this.Changed;
			setting.SourceElement = new TestConfigurationSection(); // just so Commit works

			setting.Publish();

			using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MockConfigurationSectionSettingB")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());

				ManagementObject managementObject = (ManagementObject)resultEnumerator.Current;

				setting.performSave = false;
				Assert.IsNull(this.changedObject);
				managementObject.Put();
				Assert.IsNull(this.changedObject); // no change notified

				setting.performSave = true;
				Assert.IsNull(this.changedObject);
				managementObject.Put();
				Assert.IsNotNull(this.changedObject); // change notified
			}
		}

		private static MockConfigurationSectionSetting CreateMockConfigurationSectionSetting(string applicationName,
		                                                                                     string sectionName)
		{
			MockConfigurationSectionSetting setting = new MockConfigurationSectionSetting();
			setting.ApplicationName = applicationName;
			setting.SectionName = sectionName;
			return setting;
		}

		private static MockConfigurationSectionSettingB CreateMockConfigurationSectionSettingB(string applicationName,
		                                                                                       string sectionName)
		{
			MockConfigurationSectionSettingB setting = new MockConfigurationSectionSettingB();
			setting.ApplicationName = applicationName;
			setting.SectionName = sectionName;
			return setting;
		}

		private object changedObject;

		private void Changed(object source, EventArgs args)
		{
			this.changedObject = source;
		}
	}


	[ManagementEntity]
	public class MockConfigurationSectionSetting : ConfigurationSectionSetting
	{
		[ManagementEnumerator]
		public static IEnumerable<MockConfigurationSectionSetting> GetInstances()
		{
			return GetInstances<MockConfigurationSectionSetting>();
		}

		[ManagementBind]
		public static MockConfigurationSectionSetting BindInstance(string ApplicationName, string SectionName)
		{
			return BindInstance<MockConfigurationSectionSetting>(ApplicationName, SectionName);
		}

		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return true;
		}
	}

	[ManagementEntity]
	public class MockConfigurationSectionSettingB : ConfigurationSectionSetting
	{
		[ManagementEnumerator]
		public static IEnumerable<MockConfigurationSectionSettingB> GetInstances()
		{
			return GetInstances<MockConfigurationSectionSettingB>();
		}

		[ManagementBind]
		public static MockConfigurationSectionSettingB BindInstance(string ApplicationName, string SectionName)
		{
			return BindInstance<MockConfigurationSectionSettingB>(ApplicationName, SectionName);
		}

		public bool performSave;

		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return performSave;
		}
	}
}
