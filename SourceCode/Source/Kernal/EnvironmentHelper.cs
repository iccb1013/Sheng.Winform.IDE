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
using Sheng.SailingEase.Win32;
namespace Sheng.SailingEase.Kernal
{
    public static class EnvironmentHelper
    {
        public static bool SupportAreo
        {
            get
            {
                if (Environment.OSVersion.Version.Major < 6)
                {
                    return false;
                }
                return true;
            }
        }
        public static bool DwmIsCompositionEnabled
        {
            get
            {
                return DwmApi.DwmIsCompositionEnabled();
            }
        }
        public static System.Windows.Forms.Form MainForm
        {
            get;
            set;
        }
    }
}
