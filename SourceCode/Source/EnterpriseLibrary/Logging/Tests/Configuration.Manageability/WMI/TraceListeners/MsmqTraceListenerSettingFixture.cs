/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Messaging;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Tests.WMI.TraceListeners
{
    [TestClass]
    public class MsmqTraceListenerSettingFixture
    {
        [TestInitialize]
        public void SetUp()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(MsmqTraceListenerSetting));
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
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MsmqTraceListenerSetting")
                    .Get().GetEnumerator())
            {
                Assert.IsFalse(resultEnumerator.MoveNext());
            }
        }
        [TestMethod]
        public void WmiQueryReturnsSingleResultIfSinglePublishedInstance()
        {
            MsmqTraceListenerSetting setting
                = new MsmqTraceListenerSetting(null, "name", "Formatter", "MessagePriority", "QueuePath", false,
                                               "TimeToBeReceived", "TimeToReachQueue", "TraceOutputOptions", "TransactionType",
                                               true, false, true, System.Diagnostics.SourceLevels.Critical.ToString());
            setting.ApplicationName = "app";
            setting.SectionName = InstrumentationConfigurationSection.SectionName;
            setting.Publish();
            using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MsmqTraceListenerSetting")
                    .Get().GetEnumerator())
            {
                Assert.IsTrue(resultEnumerator.MoveNext());
                Assert.AreEqual("name", resultEnumerator.Current.Properties["Name"].Value);
                Assert.AreEqual("Formatter", resultEnumerator.Current.Properties["Formatter"].Value);
                Assert.AreEqual("MessagePriority", resultEnumerator.Current.Properties["MessagePriority"].Value);
                Assert.AreEqual("QueuePath", resultEnumerator.Current.Properties["QueuePath"].Value);
                Assert.AreEqual(false, resultEnumerator.Current.Properties["Recoverable"].Value);
                Assert.AreEqual("TimeToBeReceived", resultEnumerator.Current.Properties["TimeToBeReceived"].Value);
                Assert.AreEqual("TimeToReachQueue", resultEnumerator.Current.Properties["TimeToReachQueue"].Value);
                Assert.AreEqual("TraceOutputOptions", resultEnumerator.Current.Properties["TraceOutputOptions"].Value);
				Assert.AreEqual(System.Diagnostics.SourceLevels.Critical.ToString(), resultEnumerator.Current.Properties["Filter"].Value);
				Assert.AreEqual("TransactionType", resultEnumerator.Current.Properties["TransactionType"].Value);
                Assert.AreEqual(true, resultEnumerator.Current.Properties["UseAuthentication"].Value);
                Assert.AreEqual(false, resultEnumerator.Current.Properties["UseDeadLetterQueue"].Value);
                Assert.AreEqual(true, resultEnumerator.Current.Properties["UseEncryption"].Value);
                Assert.AreEqual("MsmqTraceListenerSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
                Assert.IsFalse(resultEnumerator.MoveNext());
            }
        }
        [TestMethod]
        public void CanBindObject()
        {
            MsmqTraceListenerSetting setting
                = new MsmqTraceListenerSetting(null, "name", "Formatter", "MessagePriority", "QueuePath", false,
                                               "TimeToBeReceived", "TimeToReachQueue", "TraceOutputOptions", "TransactionType",
											   true, false, true, System.Diagnostics.SourceLevels.Critical.ToString());
            setting.ApplicationName = "app";
            setting.SectionName = InstrumentationConfigurationSection.SectionName;
            setting.Publish();
            using (ManagementObjectCollection.ManagementObjectEnumerator resultEnumerator
                = new ManagementObjectSearcher("root\\enterpriselibrary", "SELECT * FROM MsmqTraceListenerSetting")
                    .Get().GetEnumerator())
            {
                Assert.IsTrue(resultEnumerator.MoveNext());
                Assert.AreEqual("MsmqTraceListenerSetting", resultEnumerator.Current.SystemProperties["__CLASS"].Value);
                ManagementObject managementObject = resultEnumerator.Current as ManagementObject;
                Assert.IsNotNull(managementObject);
                managementObject.Put();
            }
        }
        [TestMethod]
        public void SavesChangesToConfigurationObject()
        {
            MsmqTraceListenerData sourceElement = new MsmqTraceListenerData();
            sourceElement.Formatter = "formatter";
            sourceElement.MessagePriority = MessagePriority.AboveNormal;
            sourceElement.QueuePath = "path";
            sourceElement.Recoverable = false;
            sourceElement.TimeToBeReceived = new TimeSpan(1000);
            sourceElement.TimeToReachQueue = new TimeSpan(2000);
            sourceElement.Filter = SourceLevels.Information;
            sourceElement.TraceOutputOptions = TraceOptions.ProcessId;
            sourceElement.TransactionType = MessageQueueTransactionType.Automatic;
            sourceElement.UseAuthentication = false;
            sourceElement.UseDeadLetterQueue = true;
            sourceElement.UseEncryption = false;
            List<ConfigurationSetting> settings = new List<ConfigurationSetting>(1);
            MsmqTraceListenerDataWmiMapper.GenerateWmiObjects(sourceElement, settings);
            Assert.AreEqual(1, settings.Count);
            MsmqTraceListenerSetting setting = settings[0] as MsmqTraceListenerSetting;
            Assert.IsNotNull(setting);
            setting.Formatter = "updated formatter";
            setting.MessagePriority = MessagePriority.High.ToString();
            setting.QueuePath = "updated queue";
            setting.Recoverable = true;
            setting.TimeToBeReceived = new TimeSpan(10000).ToString();
            setting.TimeToReachQueue = new TimeSpan(20000).ToString();
            setting.Filter = SourceLevels.All.ToString();
            setting.TraceOutputOptions = TraceOptions.ThreadId.ToString();
            setting.Name = "Foo";
            setting.Commit();
            Assert.AreEqual(setting.Name, sourceElement.Name);
            Assert.AreEqual(setting.Filter, sourceElement.Filter.ToString());
            Assert.AreEqual(setting.MessagePriority, sourceElement.MessagePriority.ToString());
            Assert.AreEqual(setting.Recoverable, sourceElement.Recoverable);
            Assert.AreEqual(setting.TimeToBeReceived, sourceElement.TimeToBeReceived.ToString());
            Assert.AreEqual(setting.TimeToReachQueue, sourceElement.TimeToReachQueue.ToString());
            Assert.AreEqual("updated queue", sourceElement.QueuePath);
            Assert.AreEqual(SourceLevels.All, sourceElement.Filter);
            Assert.AreEqual(TraceOptions.ThreadId, sourceElement.TraceOutputOptions);
        }
    }
}
