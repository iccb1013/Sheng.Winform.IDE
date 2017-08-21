/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Sheng.SailingEase.Win32;
namespace Sheng.SailingEase.Kernal
{
    public class IniFile
    {
        private string path;
        public IniFile(string iniFile)
        {
            path = iniFile;
        }
        public void WriteValue(string section, string key, string value)
        {
            Kernel32.WritePrivateProfileString(section, key, value, this.path);
        }
        public string ReadValue(string section, string key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = Kernel32.GetPrivateProfileString(section, key, "", temp,
            255, this.path);
            return temp.ToString();
        }
    }
}
