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
using System.Reflection;
using System.Text;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Core.Development;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Modules.LaunchModule
{
    class DevelopmentAssemblyLoader
    {
        private Assembly _assembly;
        public DevelopmentAssemblyLoader(Assembly assembly)
        {
            _assembly = assembly;
        }
        public void Load()
        {
            LoadNavigationEntities();
            LoadWindowDevelopmentEntities();
        }
        private void LoadNavigationEntities()
        {
            List<Type> avaliableToolStripItems =
                ReflectionAttributeHelper.GetCompriseCustomAttributeTypes<ToolStripItemEntityProvideAttribute>(_assembly, true);
            ToolStripItemEntityTypesFactory.Instance.Register(avaliableToolStripItems.ToArray());
        }
        private void LoadWindowDevelopmentEntities()
        {
            List<Type> windowEntity =
                ReflectionAttributeHelper.GetCompriseCustomAttributeTypes<DesignerHostEntityAttribute>(_assembly, true);
            Debug.Assert(windowEntity.Count == 1, "找不到 DesignerHostEntityAttribute ，或 DesignerHostEntityAttribute 不止一个");
            WindowElementContainer windowElementContainer = WindowElementContainer.Instance;
            windowElementContainer.SetWindowEntityType(windowEntity[0]);
            List<Type> devElementEntities =
                ReflectionAttributeHelper.GetCompriseCustomAttributeTypes<NormalUIElementEntityProvideAttribute>(
                _assembly, true);
            windowElementContainer.SetDevElementEntities(devElementEntities);
        }
    }
}
