/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	internal static class NativeMethods
	{
		[DllImport("userenv.dll", SetLastError = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool RegisterGPNotification(SafeWaitHandle hEvent,
			[MarshalAs(UnmanagedType.Bool)] bool bMachine);
		[DllImport("userenv.dll", SetLastError = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool UnregisterGPNotification(SafeWaitHandle hEvent);
		[DllImport("userenv.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern IntPtr EnterCriticalPolicySection([MarshalAs(UnmanagedType.Bool)] bool bMachine);
		[DllImport("userenv.dll", SetLastError = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool LeaveCriticalPolicySection(IntPtr handle);
	}
}
