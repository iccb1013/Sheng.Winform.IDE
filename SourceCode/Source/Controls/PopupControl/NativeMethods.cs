/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Security;
using System.Security.Permissions;
namespace Sheng.SailingEase.Controls.PopupControl
{
    internal static class NativeMethods
    {
        internal const int WM_NCHITTEST = 0x0084,
                           WM_NCACTIVATE = 0x0086,
                           WS_EX_NOACTIVATE = 0x08000000,
                           HTTRANSPARENT = -1,
                           HTLEFT = 10,
                           HTRIGHT = 11,
                           HTTOP = 12,
                           HTTOPLEFT = 13,
                           HTTOPRIGHT = 14,
                           HTBOTTOM = 15,
                           HTBOTTOMLEFT = 16,
                           HTBOTTOMRIGHT = 17,
                           WM_PRINT = 0x0317,
                           WM_USER = 0x0400,
                           WM_REFLECT = WM_USER + 0x1C00,
                           WM_COMMAND = 0x0111,
                           CBN_DROPDOWN = 7,
                           WM_GETMINMAXINFO = 0x0024;
        [Flags]
        internal enum AnimationFlags : int
        {
            Roll = 0x0000, 
            HorizontalPositive = 0x00001, 
            HorizontalNegative = 0x00002, 
            VerticalPositive = 0x00004, 
            VerticalNegative = 0x00008, 
            Center = 0x00010, 
            Hide = 0x10000, 
            Activate = 0x20000, 
            Slide = 0x40000, 
            Blend = 0x80000, 
            Mask = 0xfffff,
        }
        [SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int AnimateWindow(HandleRef windowHandle, int time, AnimationFlags flags);
        internal static void AnimateWindow(Control control, int time, AnimationFlags flags)
        {
            try
            {
                SecurityPermission sp = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
                sp.Demand();
                AnimateWindow(new HandleRef(control, control.Handle), time, flags);
            }
            catch (SecurityException) { }
        }
        internal static int HIWORD(int n)
        {
            return (n >> 16) & 0xffff;
        }
        internal static int HIWORD(IntPtr n)
        {
            return HIWORD(unchecked((int)(long)n));
        }
        internal static int LOWORD(int n)
        {
            return n & 0xffff;
        }
        internal static int LOWORD(IntPtr n)
        {
            return LOWORD(unchecked((int)(long)n));
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct MINMAXINFO
        {
            public Point reserved;
            public Size maxSize;
            public Point maxPosition;
            public Size minTrackSize;
            public Size maxTrackSize;
        }
    }
}
