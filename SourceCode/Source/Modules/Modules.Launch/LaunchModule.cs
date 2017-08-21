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
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Unity;
using System.Reflection;
using Sheng.SailingEase.Core.Development;
using Sheng.SailingEase.Infrastructure;
namespace Sheng.SailingEase.Modules.LaunchModule
{
    public class LaunchModule : IModule
    {
        IUnityContainer _container;
        public LaunchModule(IUnityContainer container)
        {
            _container = container;
        }
        public void Initialize()
        {
            ConfigureContainer();
            DevelopmentAssemblyLoad();
        }
        private void ConfigureContainer()
        {
            _container.RegisterInstance<IWindowElementContainer>(WindowElementContainer.Instance);
            _container.RegisterInstance<IPropertyGirdCellsContainer>(new PropertyGirdCellsContainer());
            _container.RegisterInstance<IWindowCompontsContainer>(WindowCompontsContainer.Instance);
        }
        private void DevelopmentAssemblyLoad()
        {
            Assembly windowsForms = Assembly.GetAssembly(
              typeof(Sheng.SailingEase.Windows.Forms.Development.WindowsFormsDevelopment));
            DevelopmentAssemblyLoader developmentAssemblyLoader = new DevelopmentAssemblyLoader(windowsForms);
            developmentAssemblyLoader.Load();
            WindowCompontsLoader WindowCompontsLoader = new Modules.LaunchModule.WindowCompontsLoader(windowsForms);
            WindowCompontsLoader.Load();
            Assembly coreDevelopment = Assembly.GetAssembly(
              typeof(Sheng.SailingEase.Core.Development.CoreDevelopment));
            PropertyGridCellsLoader propertyGridCellsLoader = new PropertyGridCellsLoader(coreDevelopment);
            propertyGridCellsLoader.Load();
        }
    }
}
