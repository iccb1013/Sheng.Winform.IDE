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
    public static class Shell32
    {
        public const int SHCNE_ASSOCCHANGED = 0x08000000;
        public const int SHCNF_IDLIST = 0x0;
        [DllImport("shell32.dll")]
        public static extern void SHChangeNotify(int wEventId, int uFlags, IntPtr dwItem1, IntPtr dwItem2);
    }
}
