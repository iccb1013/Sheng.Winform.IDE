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
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Core.Development;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
using System.Reflection;
using System.Diagnostics;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.ComponentModel.Design;
namespace Sheng.SailingEase.Modules.LaunchModule
{
    class WindowCompontsLoader
    {
        private Assembly _assembly;
        private WindowCompontsContainer _windowCompontsContainer = WindowCompontsContainer.Instance;
        public WindowCompontsLoader(Assembly assembly)
        {
            _assembly = assembly;
        }
        public void Load()
        {
            LoadWindowComponts();
        }
        private void LoadWindowComponts()
        {
            List<Type> rootComponet =
                ReflectionAttributeHelper.GetImplementInterfaceTypes<IWindowDesignerRootComponent>(_assembly);
            Debug.Assert(rootComponet.Count == 1, "找不到 DesignerHostEntityAttribute ，或 DesignerHostEntityAttribute 不止一个");
            _windowCompontsContainer.SetWindowDesignerRootComponent(rootComponet[0]);
            List<RuntimeControlToolboxItemAttribute> controls =
                ReflectionAttributeHelper.GetCustomAttributes<RuntimeControlToolboxItemAttribute>(_assembly, false);
            _windowCompontsContainer.SetShellControls(controls);
        }
    }
}
