/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
namespace Sheng.SIMBE.SEControl.Docking
{
	internal static class ResourceHelper
	{
        private static ResourceManager _resourceManager = null;
        private static ResourceManager ResourceManager
        {
            get
            {
                if (_resourceManager == null)
                    _resourceManager = new ResourceManager("Sheng.SIMBE.SEControl.Docking.Strings", typeof(ResourceHelper).Assembly);
                return _resourceManager;
            }
        }
		public static string GetString(string name)
		{
			return ResourceManager.GetString(name);
		}
	}
}
