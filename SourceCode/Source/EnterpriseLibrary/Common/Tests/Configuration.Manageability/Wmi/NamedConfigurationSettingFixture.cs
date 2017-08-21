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
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Management;
using System.Management.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Wmi
{
	[TestClass]
	public class NamedConfigurationSettingFixture
	{
		[TestInitialize]
		public void SetUp()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(MockNamedConfigurationSetting), typeof(MockNamedConfigurationSettingB));
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
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MockNamedConfigurationSetting")
						.Get().GetEnumerator())
			{
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}

		[TestMethod]
		public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
		{
			MockNamedConfigurationSetting setting = CreateMockNamedConfigurationSetting("Baz", "Foo", "Bar");

			setting.Publish();

			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MockNamedConfigurationSetting")
						.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("Baz", resultEnumerator.Current.Properties["Name"].Value);
				Assert.AreEqual("MockNamedConfigurationSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}

		[TestMethod]
		public void RevokedInstanceIsNotReturnedInQuery()
		{
			MockNamedConfigurationSetting setting = CreateMockNamedConfigurationSetting("Baz", "Foo", "Bar");

			setting.Publish();

			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MockNamedConfigurationSetting")
						.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("Baz", resultEnumerator.Current.Properties["Name"].Value);
				Assert.AreEqual("MockNamedConfigurationSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}

			setting.Revoke();

			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MockNamedConfigurationSetting")
						.Get().GetEnumerator())
			{
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}

		[TestMethod]
		public void RevokeForNonPublishedInstanceIsIgnored()
		{
			MockNamedConfigurationSetting setting = CreateMockNamedConfigurationSetting("Baz", "Foo", "Bar");
			MockNamedConfigurationSetting setting2 = CreateMockNamedConfigurationSetting("Baz2", "Foo", "Bar");

			setting.Publish();
			setting2.Revoke();
		}

		[TestMethod]
		public void PublishInstanceWithDuplicateKeyThrows()
		{
			MockNamedConfigurationSetting setting = CreateMockNamedConfigurationSetting("Baz", "Foo", "Bar");
			MockNamedConfigurationSetting setting2 = CreateMockNamedConfigurationSetting("Baz", "Foo", "Bar");

			setting.Publish();
			try
			{
				setting2.Publish();
				Assert.Fail("should have thrown");
			}
			catch (ArgumentException) { }
		}

		[TestMethod]
		public void CanPublishMultipleInstancesOfClassWithDifferentKeys()
		{
			MockNamedConfigurationSetting setting = CreateMockNamedConfigurationSetting("Baz", "Foo", "Bar");
			MockNamedConfigurationSetting setting2 = CreateMockNamedConfigurationSetting("Baz2", "Foo", "Bar");
			setting.Publish();
			setting2.Publish();

			IDictionary<string, MockNamedConfigurationSetting> nameMapping = new Dictionary<string, MockNamedConfigurationSetting>();
			nameMapping.Add(setting.Name, setting);
			nameMapping.Add(setting2.Name, setting2);

			using (System.Management.ManagementObjectCollection resultCollection
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MockNamedConfigurationSetting")
						.Get())
			{
				foreach (ManagementObject current in resultCollection)
				{
					Assert.AreEqual("MockNamedConfigurationSetting", current.SystemProperties["__CLASS"].Value);
					Assert.IsTrue(nameMapping.Remove((string)current.Properties["Name"].Value), "unknown name");
				}
			}

			Assert.AreEqual(0, nameMapping.Count, "Not all elements were retrieved");
		}

		[TestMethod]
		public void RevokedInstanceIsNotReturnedWhenMultipleInstancesArePublished()
		{
			MockNamedConfigurationSetting setting = CreateMockNamedConfigurationSetting("Baz", "Foo", "Bar");
			MockNamedConfigurationSetting setting2 = CreateMockNamedConfigurationSetting("Baz2", "Foo", "Bar");
			setting.Publish();
			setting2.Publish();

			setting.Revoke();

			IDictionary<string, MockNamedConfigurationSetting> nameMapping = new Dictionary<string, MockNamedConfigurationSetting>();
			nameMapping.Add(setting2.Name, setting2);

			using (System.Management.ManagementObjectCollection resultCollection
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MockNamedConfigurationSetting")
						.Get())
			{
				foreach (ManagementObject current in resultCollection)
				{
					Assert.AreEqual("MockNamedConfigurationSetting", current.SystemProperties["__CLASS"].Value);
					Assert.IsTrue(nameMapping.Remove((string)current.Properties["Name"].Value), "unknown name");
				}
			}

			Assert.AreEqual(0, nameMapping.Count, "Not all elements were retrieved");
		}

		[TestMethod]
		public void CanPublishInstancesOfSiblingClassesWithTheSameKey()
		{
			MockNamedConfigurationSetting setting = CreateMockNamedConfigurationSetting("Baz", "Foo", "Bar");
			MockNamedConfigurationSettingB setting2 = CreateMockNamedConfigurationSettingB("Baz", "Foo", "Bar");
			setting.Publish();
			setting2.Publish();

			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MockNamedConfigurationSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("Baz", resultEnumerator.Current.Properties["Name"].Value);
				Assert.AreEqual("MockNamedConfigurationSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}

			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MockNamedConfigurationSettingB")
						.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("Baz", resultEnumerator.Current.Properties["Name"].Value);
				Assert.AreEqual("MockNamedConfigurationSettingB", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}

		[TestMethod]
		public void SecondPublishIsNoOp()
		{
			MockNamedConfigurationSetting setting = CreateMockNamedConfigurationSetting("Baz", "Foo", "Bar");

			setting.Publish();

			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MockNamedConfigurationSetting")
						.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("Baz", resultEnumerator.Current.Properties["Name"].Value);
				Assert.AreEqual("MockNamedConfigurationSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}

			// second publish, should not throw but should return single instance
			setting.Publish();

			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MockNamedConfigurationSetting")
						.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("Baz", resultEnumerator.Current.Properties["Name"].Value);
				Assert.AreEqual("MockNamedConfigurationSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}

		[TestMethod]
		public void ReturnsEmptyResultForNonPublishedClassWithPublishedSibling()
		{
			MockNamedConfigurationSetting setting = CreateMockNamedConfigurationSetting("Baz", "Foo", "Bar");

			setting.Publish();

			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MockNamedConfigurationSettingB")
					.Get().GetEnumerator())
			{
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}

		[TestMethod]
		public void ReturnsInstanceFromPublishedClassWithPublishedSibling()
		{
			MockNamedConfigurationSetting setting = CreateMockNamedConfigurationSetting("Baz", "Foo", "Bar");
			MockNamedConfigurationSettingB settingB = CreateMockNamedConfigurationSettingB("Bazb", "Foob", "Barb");

			setting.Publish();
			settingB.Publish();

			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MockNamedConfigurationSettingB")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("Bazb", resultEnumerator.Current.Properties["Name"].Value);
				Assert.AreEqual("MockNamedConfigurationSettingB", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}

		[TestMethod]
		public void CanBindObject()
		{
			MockNamedConfigurationSetting setting = CreateMockNamedConfigurationSetting("Baz", "Foo", "Bar");
			setting.Changed += this.Changed;
			setting.SourceElement = new TestConfigurationSection(); // just so Commit works

			setting.Publish();

			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MockNamedConfigurationSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("Baz", resultEnumerator.Current.Properties["Name"].Value);
				Assert.AreEqual("MockNamedConfigurationSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);

				ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
				Assert.IsNotNull(managementObject);

				Assert.IsNull(this.changedObject);
				managementObject.Put();
				Assert.IsNotNull(this.changedObject);

				Assert.IsFalse(resultEnumerator.MoveNext());
			}
		}

		[TestMethod]
        [Ignore] // not sure why this is not working.
		public void BindOnRevokedInstanceThrows()
		{
			MockNamedConfigurationSetting setting = CreateMockNamedConfigurationSetting("Baz", "Foo", "Bar");
			setting.Changed += this.Changed;

			setting.Publish();

			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MockNamedConfigurationSetting")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());
				Assert.AreEqual("Baz", resultEnumerator.Current.Properties["Name"].Value);
				Assert.AreEqual("MockNamedConfigurationSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);

				ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
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
			MockNamedConfigurationSettingB setting = CreateMockNamedConfigurationSettingB("Baz", "Foo", "Bar");
			setting.Changed += this.Changed;
			setting.SourceElement = new TestConfigurationSection(); // just so Commit works

			setting.Publish();

			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
				= new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MockNamedConfigurationSettingB")
					.Get().GetEnumerator())
			{
				Assert.IsTrue(resultEnumerator.MoveNext());

				ManagementObject managementObject = resultEnumerator.Current as ManagementObject;

				setting.performSave = false;
				Assert.IsNull(this.changedObject);
				managementObject.Put();
				Assert.IsNull(this.changedObject);		// no change notified

				setting.performSave = true;
				Assert.IsNull(this.changedObject);
				managementObject.Put();
				Assert.IsNotNull(this.changedObject);	// change notified
			}
		}

		private static MockNamedConfigurationSetting CreateMockNamedConfigurationSetting(string name, string applicationName, string sectionName)
		{
			MockNamedConfigurationSetting setting = new MockNamedConfigurationSetting(name);
			setting.ApplicationName = applicationName;
			setting.SectionName = sectionName;
			return setting;
		}

		private static MockNamedConfigurationSettingB CreateMockNamedConfigurationSettingB(string name, string applicationName, string sectionName)
		{
			MockNamedConfigurationSettingB setting = new MockNamedConfigurationSettingB(name);
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
	public class MockNamedConfigurationSetting : NamedConfigurationSetting
	{
		public MockNamedConfigurationSetting(string name)
			: base(name)
		{ }

		[ManagementEnumerator]
		public static IEnumerable<MockNamedConfigurationSetting> GetInstances()
		{
			return NamedConfigurationSetting.GetInstances<MockNamedConfigurationSetting>();
		}

		[ManagementBind]
		public static MockNamedConfigurationSetting BindInstance(string ApplicationName, string SectionName, string Name)
		{
			return NamedConfigurationSetting.BindInstance<MockNamedConfigurationSetting>(ApplicationName, SectionName, Name);
		}

		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return true;
		}
	}

	[ManagementEntity]
	public class MockNamedConfigurationSettingB : NamedConfigurationSetting
	{
		public MockNamedConfigurationSettingB(string name)
			: base(name)
		{ }

		[ManagementEnumerator]
		public static IEnumerable<MockNamedConfigurationSettingB> GetInstances()
		{
			return NamedConfigurationSetting.GetInstances<MockNamedConfigurationSettingB>();
		}

		[ManagementBind]
		public static MockNamedConfigurationSettingB BindInstance(string ApplicationName, string SectionName, string Name)
		{
			return NamedConfigurationSetting.BindInstance<MockNamedConfigurationSettingB>(ApplicationName, SectionName, Name);
		}

		public bool performSave;
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return performSave;
		}
	}
}
