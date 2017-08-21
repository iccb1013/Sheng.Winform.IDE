/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests
{
    [TestClass]
    public class ConfigurationChangeWatcherFixture
    {
        int notifications;
        [TestInitialize]
        public void SetUp()
        {
            notifications = 0;
        }
        [TestMethod]
        public void RunningWatcherKeepsOnlyOnePollingThread()
        {
            ConfigurationChangeWatcher.SetDefaultPollDelayInMilliseconds(50);
            TestConfigurationChangeWatcher watcher = new TestConfigurationChangeWatcher();
            try
            {
                watcher.ConfigurationChanged += new ConfigurationChangedEventHandler(OnConfigurationChanged);
                for (int i = 0; i < 20; i++)
                {
                    watcher.StopWatching();
                    watcher.StartWatching();
                }
                Thread.Sleep(50);
                watcher.DoNotification();
                Thread.Sleep(150);
                Assert.AreEqual(1, notifications);
            }
            finally
            {
                watcher.StopWatching();
                ConfigurationChangeWatcher.ResetDefaultPollDelay();
            }
        }
        void OnConfigurationChanged(object sender,
                                    ConfigurationChangedEventArgs e)
        {
            lock (this)
            {
                notifications++;
            }
        }
    }
    class TestConfigurationChangeWatcher : ConfigurationChangeWatcher
    {
        [ThreadStatic]
        static bool notified;
        DateTime lastWriteTime = DateTime.Now;
        bool notify = false;
        public override string SectionName
        {
            get { return "section"; }
        }
        protected override ConfigurationChangedEventArgs BuildEventData()
        {
            return new ConfigurationChangedEventArgs(SectionName);
        }
        protected override string BuildThreadName()
        {
            return "Test Watcher";
        }
        internal void DoNotification()
        {
            notify = true;
        }
        protected override DateTime GetCurrentLastWriteTime()
        {
            if (notify && !notified)
            {
                notified = true;
                lastWriteTime = DateTime.Now;
            }
            return lastWriteTime;
        }
        protected override string GetEventSourceName()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
