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
using System.Reflection;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Core.Development;
using Sheng.SailingEase.Infrastructure;
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    class DesignerToolboxManager : ToolboxManager
    {
        private static InstanceLazy<DesignerToolboxManager> _instance =
            new InstanceLazy<DesignerToolboxManager>(() => new DesignerToolboxManager());
        public static DesignerToolboxManager Instance
        {
            get { return _instance.Value; }
        }
        private DesignerToolboxManager()
        {
            List<RuntimeControlToolboxItemAttribute> controls = ServiceUnity.WindowCompontsContainer.GetShellControls();
            foreach (RuntimeControlToolboxItemAttribute shellControl in controls)
            {
                AddToolBoxItem(shellControl.Catalog, shellControl.ControlType, shellControl.Name);
            }
        }
    }
}
