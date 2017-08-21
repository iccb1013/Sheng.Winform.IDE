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
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Modules.LaunchModule
{
    public class WindowCompontsContainer : IWindowCompontsContainer
    {
        private Type _windowDesignerRootComponentType;
        private List<RuntimeControlToolboxItemAttribute> _controls;
        private static InstanceLazy<WindowCompontsContainer> _instance =
           new InstanceLazy<WindowCompontsContainer>(() => new WindowCompontsContainer());
        public static WindowCompontsContainer Instance
        {
            get { return _instance.Value; }
        }
        private  WindowCompontsContainer()
        {
        }
        internal void SetWindowDesignerRootComponent(Type type)
        {
            _windowDesignerRootComponentType = type;
        }
        internal void SetShellControls(List<RuntimeControlToolboxItemAttribute> controls)
        {
            _controls = controls;
        }
        public Type GetWindowDesignerRootComponent()
        {
            return _windowDesignerRootComponentType;
        }
        public List<RuntimeControlToolboxItemAttribute> GetShellControls()
        {
            return _controls;
        }
    }
}
