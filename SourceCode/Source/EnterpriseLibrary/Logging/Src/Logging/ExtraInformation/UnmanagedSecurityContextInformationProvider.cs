/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Security.Permissions;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation
{
    public class UnmanagedSecurityContextInformationProvider : IExtraInformationProvider
    {
        public void PopulateDictionary(IDictionary<string, object> dict)
        {
            dict.Add(Properties.Resources.UnmanagedSecurity_CurrentUser, CurrentUser);
            dict.Add(Properties.Resources.UnmanagedSecurity_ProcessAccountName, ProcessAccountName);
        }
        public string CurrentUser
        {
            get
            {
                uint size = 256;
                StringBuilder userNameBuffer = new StringBuilder((int) size);
                if (NativeMethods.GetUserNameEx(NativeMethods.ExtendedNameFormat.NameSamCompatible, userNameBuffer, ref size))
                {
                    return userNameBuffer.ToString();
                }
                else
                {
                    return null;
                }
            }
        }
		public string ProcessAccountName
        {
			[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
			get
            {
                IntPtr processHandle = NativeMethods.GetCurrentProcess();
                IntPtr pSidOwner = IntPtr.Zero;
                IntPtr pSidGroup = IntPtr.Zero;
                IntPtr pDacl = IntPtr.Zero;
                IntPtr pSacl = IntPtr.Zero;
                IntPtr pSecurityDescriptor = IntPtr.Zero;
                string processAccountName;
                NativeMethods.GetSecurityInfo(processHandle, NativeMethods.SE_OBJECT_TYPE.SE_KERNEL_OBJECT, NativeMethods.OWNER_SECURITY_INFORMATION | NativeMethods.GROUP_SECURITY_INFORMATION,
                                              ref pSidOwner,
                                              ref pSidGroup,
                                              ref pDacl,
                                              ref pSacl,
                                              out pSecurityDescriptor);
                StringBuilder accountName = new StringBuilder(1024);
                uint accountNameLength = (uint) accountName.Capacity;
                StringBuilder domainName = new StringBuilder(1024);
                uint domainNameLength = (uint) domainName.Capacity;
                int sidType;
                bool successful = NativeMethods.LookupAccountSid(IntPtr.Zero,
                                                                 pSidOwner, 
                                                                 accountName, 
                                                                 ref accountNameLength, 
                                                                 domainName, 
                                                                 ref domainNameLength, 
                                                                 out sidType 
                    );
                if (successful)
                {
                    processAccountName = domainName.ToString() + Path.DirectorySeparatorChar + accountName.ToString();
                } 
                else
                {
                    processAccountName = Properties.Resources.CouldNotLookupAccountSid;
                }
                Marshal.FreeHGlobal(pSecurityDescriptor);
                return processAccountName;
            }
        }
    }
}
