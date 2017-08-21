/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
    public sealed class GroupPolicyWatcher : IGroupPolicyWatcher
    {
        AutoResetEvent currentThreadExitEvent;
        object lockObject = new object();
        GroupPolicyNotificationRegistrationBuilder registrationBuilder;
        public GroupPolicyWatcher()
            : this(new GroupPolicyNotificationRegistrationBuilder()) {}
        public GroupPolicyWatcher(GroupPolicyNotificationRegistrationBuilder registrationBuilder)
        {
            this.registrationBuilder = registrationBuilder;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        void Dispose(bool disposing)
        {
            if (disposing)
            {
                StopWatching();
            }
        }
        void DoWatch(object parameter)
        {
            AutoResetEvent exitEvent = (AutoResetEvent)parameter;
            try
            {
                using (GroupPolicyNotificationRegistration registration = registrationBuilder.CreateRegistration())
                {
                    AutoResetEvent[] policyEvents
                        = new AutoResetEvent[] { exitEvent, registration.MachinePolicyEvent, registration.UserPolicyEvent };
                    bool listening = true;
                    while (listening)
                    {
                        int result = WaitHandle.WaitAny(policyEvents); 
                        if (result != 0)
                        {
                            if (GroupPolicyUpdated != null)
                            {
                                GroupPolicyUpdated(result == 1);
                            }
                        }
                        else
                        {
                            listening = false;
                        }
                    }
                }
            }
            finally
            {
                exitEvent.Close();
            }
        }
        ~GroupPolicyWatcher()
        {
            Dispose(false);
        }
        public event GroupPolicyUpdateDelegate GroupPolicyUpdated;
        public void StartWatching()
        {
            lock (lockObject)
            {
                if (currentThreadExitEvent == null)
                {
                    currentThreadExitEvent = new AutoResetEvent(false);
                    Thread watchingThread = new Thread(new ParameterizedThreadStart(DoWatch));
                    watchingThread.IsBackground = true;
                    watchingThread.Name = Resources.GroupPolicyWatcherThread;
                    watchingThread.Start(currentThreadExitEvent);
                }
            }
        }
        public void StopWatching()
        {
            lock (lockObject)
            {
                if (currentThreadExitEvent != null)
                {
                    currentThreadExitEvent.Set();
                    currentThreadExitEvent = null;
                }
            }
        }
    }
}
