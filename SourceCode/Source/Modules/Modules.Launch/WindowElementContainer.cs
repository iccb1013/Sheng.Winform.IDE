/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Modules.LaunchModule
{
    class WindowElementContainer : IWindowElementContainer
    {
        private Type _windowEntityType;
        private List<Type> _devElementEntityTypes;
        private static InstanceLazy<WindowElementContainer> _instance =
           new InstanceLazy<WindowElementContainer>(() => new WindowElementContainer());
        public static WindowElementContainer Instance
        {
            get { return _instance.Value; }
        }
        private WindowElementContainer()
        {
        }
        internal void SetWindowEntityType(Type type)
        {
            Debug.Assert(type != null, "type 为 null");
            _windowEntityType = type;
        }
        internal void SetDevElementEntities(List<Type> types)
        {
            _devElementEntityTypes = types;
        }
        public WindowEntity CreateWindowEntity()
        {
            Debug.Assert(_windowEntityType != null, "_windowEntityType 为 null");
            WindowEntity entity = (WindowEntity)Activator.CreateInstance(_windowEntityType);
            return entity;
        }
        public List<Type> GetDevElementEntityTypes()
        {
            return _devElementEntityTypes;
        }
    }
}
