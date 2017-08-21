/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Runtime.InteropServices;
using System.Threading;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
    public class GroupPolicyNotificationRegistration : IDisposable
    {
        readonly AutoResetEvent machinePolicyEvent;
        readonly AutoResetEvent userPolicyEvent;
        public GroupPolicyNotificationRegistration()
        {
            machinePolicyEvent = new AutoResetEvent(false);
            userPolicyEvent = new AutoResetEvent(false);
            CheckReturnValue(NativeMethods.RegisterGPNotification(machinePolicyEvent.SafeWaitHandle, true));
            CheckReturnValue(NativeMethods.RegisterGPNotification(userPolicyEvent.SafeWaitHandle, false));
        }
        public AutoResetEvent MachinePolicyEvent
        {
            get { return machinePolicyEvent; }
        }
        public AutoResetEvent UserPolicyEvent
        {
            get { return userPolicyEvent; }
        }
        static void CheckReturnValue(bool returnValue)
        {
            if (!returnValue)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
        }
        public virtual void Dispose()
        {
            try
            {
                CheckReturnValue(NativeMethods.UnregisterGPNotification(machinePolicyEvent.SafeWaitHandle));
                CheckReturnValue(NativeMethods.UnregisterGPNotification(userPolicyEvent.SafeWaitHandle));
            }
            finally
            {
                machinePolicyEvent.Close();
                userPolicyEvent.Close();
            }
        }
    }
}
