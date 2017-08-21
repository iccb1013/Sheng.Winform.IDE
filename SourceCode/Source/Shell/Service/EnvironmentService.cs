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
using Sheng.SailingEase.Infrastructure;
using System.IO;
using System.Windows.Forms;
namespace Sheng.SailingEase.Shell
{
    class EnvironmentService : IEnvironmentService
    {
        public string DataPath
        {
            get
            {
                string path = Path.Combine(Application.StartupPath, Constant.DATAPATH_NAME);
                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);
                return path;
            }
        }
    }
}
