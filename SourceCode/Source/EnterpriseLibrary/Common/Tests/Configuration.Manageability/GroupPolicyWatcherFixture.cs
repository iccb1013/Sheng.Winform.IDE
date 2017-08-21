/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests
{
    [TestClass]
    public class GroupPolicyWatcherFixture
    {
        int notifications;
        bool lastNotifiedValue;
        MockGroupPolicyNotificationRegistrationBuilder builder;
        GroupPolicyWatcher watcher;
        [TestInitialize]
        public void SetUp()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
            notifications = 0;
            lastNotifiedValue = false;
            builder = new MockGroupPolicyNotificationRegistrationBuilder();
            watcher = new GroupPolicyWatcher(builder);
            watcher.GroupPolicyUpdated += GroupPolicyUpdated;
        }
        [TestMethod]
        public void RegistrationsAreRequestedOnStart()
        {
            Assert.AreEqual(0, builder.issuedRegistrations.Count);
            try
            {
                watcher.StartWatching();
                Thread.Sleep(300);
                Assert.AreEqual(1, builder.issuedRegistrations.Count);
            }
            finally
            {
                watcher.StopWatching();
            }
        }
        [TestMethod]
        public void NoEventsAreFiredIfGPNotificationsAreNotReceived()
        {
            try
            {
                watcher.StartWatching();
                Thread.Sleep(300);
                Assert.AreEqual(0, notifications);
            }
            finally
            {
                watcher.StopWatching();
            }
        }
        [TestMethod]
        public void EventIsFiredIfMachineGPNotificationIsReceived()
        {
            try
            {
                watcher.StartWatching();
                Thread.Sleep(100);
                builder.LastRegistration.MachinePolicyEvent.Set();
                Thread.Sleep(300);
                Assert.AreEqual(1, notifications);
                Assert.AreEqual(true, lastNotifiedValue);
            }
            finally
            {
                watcher.StopWatching();
            }
        }
        [TestMethod]
        public void EventIsFiredIfUserGPNotificationIsReceived()
        {
            try
            {
                watcher.StartWatching();
                Thread.Sleep(100);
                builder.LastRegistration.MachinePolicyEvent.Set();
                Thread.Sleep(300);
                Assert.AreEqual(1, notifications);
                Assert.AreEqual(true, lastNotifiedValue);
            }
            finally
            {
                watcher.StopWatching();
            }
        }
        void GroupPolicyUpdated(bool machine)
        {
            notifications++;
            lastNotifiedValue = machine;
        }
    }
    class MockGroupPolicyNotificationRegistrationBuilder : GroupPolicyNotificationRegistrationBuilder
    {
        public List<GroupPolicyNotificationRegistration> issuedRegistrations
            = new List<GroupPolicyNotificationRegistration>();
        public GroupPolicyNotificationRegistration LastRegistration
        {
            get
            {
                if (issuedRegistrations.Count > 0)
                {
                    return issuedRegistrations[issuedRegistrations.Count - 1];
                }
                return null;
            }
        }
        public override GroupPolicyNotificationRegistration CreateRegistration()
        {
            GroupPolicyNotificationRegistration registration = base.CreateRegistration();
            issuedRegistrations.Add(registration);
            return registration;
        }
    }
}
