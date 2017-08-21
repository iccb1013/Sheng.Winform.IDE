/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Text;
using System.Runtime.InteropServices;
namespace Sheng.SailingEase.Win32
{
    public static class Kernel32
    {
        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section,
            string key, string def, StringBuilder retVal,
            int size, string filePath);
    }
}
