/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Runtime.InteropServices;
using System.Text;
namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    internal sealed class NativeMethods
    {
        private NativeMethods()
        {
        }
        internal const uint OWNER_SECURITY_INFORMATION = 0x00000001;
        internal const uint GROUP_SECURITY_INFORMATION = 0x00000002;
        internal const uint DACL_SECURITY_INFORMATION = 0x00000004;
        internal const uint SACL_SECURITY_INFORMATION = 0x00000008;
        internal const int CONTEXT_E_NOCONTEXT = unchecked((int)(0x8004E004));
        internal const int E_NOINTERFACE = unchecked((int)(0x80004002));
        [DllImport("kernel32.dll")]
        internal static extern int QueryPerformanceCounter(out Int64 lpPerformanceCount);
        [DllImport("kernel32.dll")]
        internal static extern int QueryPerformanceFrequency(out Int64 lpPerformanceCount);
        [DllImport("mtxex.dll", CallingConvention=CallingConvention.Cdecl)]
        internal static extern int GetObjectContext([Out]
        [MarshalAs(UnmanagedType.Interface)] out IObjectContext pCtx);
        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetCurrentProcess();
        [DllImport("kernel32.dll")]
        public static extern int GetCurrentProcessId();
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [PreserveSig]
        public static extern int GetModuleFileName([In] IntPtr hModule, [Out] StringBuilder lpFilename, [In]
        [MarshalAs(UnmanagedType.U4)] int nSize);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr GetModuleHandle(string moduleName);
        [DllImport("secur32.dll", CharSet=CharSet.Unicode, EntryPoint="GetUserNameExW", SetLastError=true)]
		[return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool GetUserNameEx([In] ExtendedNameFormat nameFormat, StringBuilder nameBuffer, ref uint size);
        [DllImport("advapi32.dll")]
        internal static extern int GetSecurityInfo(IntPtr handle, SE_OBJECT_TYPE objectType, uint securityInformation, ref IntPtr ppSidOwner, ref IntPtr ppSidGroup, ref IntPtr ppDacl, ref IntPtr ppSacl, out IntPtr ppSecurityDescriptor);
        [DllImport("advapi32.dll", CharSet=CharSet.Unicode)]
        internal static extern bool LookupAccountSid(
            IntPtr systemName, 
            IntPtr sid, 
            StringBuilder accountName, 
            ref uint accountNameLength, 
            StringBuilder domainName, 
            ref uint domainNameLength, 
            out int sidType 
            );
		[DllImport("kernel32.dll")]
        public static extern int GetCurrentThreadId();
        [ComImport]
        [Guid("51372AE0-CAE7-11CF-BE81-00AA00A2FA25")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IObjectContext
        {
            [return : MarshalAs(UnmanagedType.Interface)]
            Object CreateInstance([MarshalAs(UnmanagedType.LPStruct)] Guid rclsid, [MarshalAs(UnmanagedType.LPStruct)] Guid riid);
            void SetComplete();
            void SetAbort();
            void EnableCommit();
            void DisableCommit();
            [PreserveSig]
            [return : MarshalAs(UnmanagedType.Bool)]
            bool IsInTransaction();
            [PreserveSig]
            [return : MarshalAs(UnmanagedType.Bool)]
            bool IsSecurityEnabled();
            [return : MarshalAs(UnmanagedType.Bool)]
            bool IsCallerInRole([In]
            [MarshalAs(UnmanagedType.BStr)] String role);
        }
        internal enum ExtendedNameFormat : int
        {
            NameUnknown = 0,
            NameFullyQualifiedDN = 1,
            NameSamCompatible = 2,
            NameDisplay = 3,
            NameUniqueId = 6,
            NameCanonical = 7,
            NameUserPrincipal = 8,
            NameCanonicalEx = 9,
            NameServicePrincipal = 10,
            NameDnsDomain = 12
        }
        internal enum SE_OBJECT_TYPE
        {
            SE_UNKNOWN_OBJECT_TYPE = 0,
            SE_FILE_OBJECT,
            SE_SERVICE,
            SE_PRINTER,
            SE_REGISTRY_KEY,
            SE_LMSHARE,
            SE_KERNEL_OBJECT,
            SE_WINDOW_OBJECT,
            SE_DS_OBJECT,
            SE_DS_OBJECT_ALL,
            SE_PROVIDER_DEFINED_OBJECT,
            SE_WMIGUID_OBJECT
        }
    }
}
