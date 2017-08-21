/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests
{
    [TestClass]
    public class ConfigurationChangeNotificationCoordinatorFixture
    {
        ConfigurationChangeNotificationCoordinator coordinator;
        List<ConfigurationChangedEventArgs> notifiedChanges;
        int notificationsForException;
        [TestInitialize]
        public void SetUp()
        {
            coordinator = new ConfigurationChangeNotificationCoordinator();
            notifiedChanges = new List<ConfigurationChangedEventArgs>();
            notificationsForException = 0;
        }
        [TestMethod]
        public void CanRegisterForNotification()
        {
            coordinator.AddSectionChangeHandler("section1", ConfigurationChanged);
        }
        [TestMethod]
        public void CanUnegisterForNotificationForRegisteredSection()
        {
            coordinator.AddSectionChangeHandler("section1", ConfigurationChanged);
            coordinator.RemoveSectionChangeHandler("section1", ConfigurationChanged);
        }
        [TestMethod]
        public void CanUnegisterForNotificationForNonRegisteredSection()
        {
            coordinator.RemoveSectionChangeHandler("section1", ConfigurationChanged);
        }
        [TestMethod]
        public void GetsNotificationForRegisteredSection()
        {
            coordinator.AddSectionChangeHandler("section1", ConfigurationChanged);
            coordinator.NotifyUpdatedSections(new String[] { "section1" });
            Assert.AreEqual(1, notifiedChanges.Count);
            Assert.AreEqual("section1", notifiedChanges[0].SectionName);
        }
        [TestMethod]
        public void GetsNotificationForRegisteredSectionWithEqualButNotIdenticalName()
        {
            coordinator.AddSectionChangeHandler("section1", ConfigurationChanged);
            coordinator.NotifyUpdatedSections(new String[] { "Asection1".Remove(0, 1) });
            Assert.AreEqual(1, notifiedChanges.Count);
            Assert.AreEqual("section1", notifiedChanges[0].SectionName);
        }
        [TestMethod]
        public void DoesNotGetNotificationForUnregisteredSection()
        {
            coordinator.AddSectionChangeHandler("section1", ConfigurationChanged);
            coordinator.RemoveSectionChangeHandler("section1", ConfigurationChanged);
            coordinator.NotifyUpdatedSections(new String[] { "section1" });
            Assert.AreEqual(0, notifiedChanges.Count);
        }
        [TestMethod]
        public void DoesNotGetNotificationForNotRegisteredSection()
        {
            coordinator.AddSectionChangeHandler("section1", ConfigurationChanged);
            coordinator.NotifyUpdatedSections(new String[] { "section2" });
            Assert.AreEqual(0, notifiedChanges.Count);
        }
        [TestMethod]
        public void ContinuesToReceiveNotificationForSameSectionAfterFailure()
        {
            coordinator.AddSectionChangeHandler("section1", ConfigurationChanged);
            coordinator.AddSectionChangeHandler("section1", ConfigurationChanged);
            notificationsForException = 1;
            coordinator.NotifyUpdatedSections(new String[] { "section1" });
            Assert.AreEqual(2, notifiedChanges.Count);
        }
        [TestMethod]
        public void ContinuesToReceiveNotificationForDifferentSectionAfterFailure()
        {
            coordinator.AddSectionChangeHandler("section1", ConfigurationChanged);
            coordinator.AddSectionChangeHandler("section2", ConfigurationChanged);
            notificationsForException = 1;
            coordinator.NotifyUpdatedSections(new String[] { "section1", "section2" });
            Assert.AreEqual(2, notifiedChanges.Count);
            Assert.AreEqual("section1", notifiedChanges[0].SectionName);
            Assert.AreEqual("section2", notifiedChanges[1].SectionName);
        }
        public void ConfigurationChanged(object sender,
                                         ConfigurationChangedEventArgs e)
        {
            notifiedChanges.Add(e);
            if (notifiedChanges.Count == notificationsForException)
            {
                throw new Exception("bogus");
            }
        }
    }
}
