/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Runtime.InteropServices;
namespace Sheng.SailingEase.Win32
{
    public static class User32
    {
        public struct CopyDataStruct
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }
        public const int WM_COPYDATA = 0x004A;
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage
            (
            int hWnd,                        
            int Msg,                        
            int wParam,                    
            ref  CopyDataStruct lParam        
            );
        public const int SC_RESTORE = 0xF120; 
        public const int SC_MOVE = 0xF010; 
        public const int SC_SIZE = 0xF000; 
        public const int SC_MINIMIZE = 0xF020; 
        public const int SC_MAXIMIZE = 0xF030; 
        public const int SC_CLOSE = 0xF060; 
        public const int MF_DISABLE = 0x1;
        public const int MF_ENABLE = 0x0;
        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        public static extern int EnableMenuItem(IntPtr hMenu, int wIDEnableItem, int wEnable);
        public const int WM_PAINT = 0x000f;
        public const int WM_ERASEBKGND = 0x0014;
        public const int WM_NCPAINT = 0x0085;
    }
}
