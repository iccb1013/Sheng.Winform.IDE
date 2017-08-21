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
using IDesign.Composite.UnityExtensions;
using Microsoft.Practices.Composite.Modularity;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Shell.View;
namespace Sheng.SailingEase.Shell
{
    internal class Bootstrapper : SimpleUnityBootstrapper
    {
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            Container.RegisterInstance<IShellView>(Container.Resolve<ShellView>());
            Container.RegisterInstance<IEnvironmentService>(Container.Resolve<EnvironmentService>());
            Container.RegisterInstance<INavigationService>(Container.Resolve<NavigationService>());
            Container.RegisterInstance<IPackageService>(Container.Resolve<PackageService>());
            Container.RegisterInstance<IWorkbenchService>(Container.Resolve<WorkbenchService>());
            Container.RegisterInstance<IArchiveServiceUnity>(Container.Resolve<ArchiveServiceUnity>());
            Container.RegisterInstance<ICachingService>(Container.Resolve<CachingService>());
            Shell = (ShellView)Container.Resolve<IShellView>();
        }
        protected override IModuleCatalog GetModuleCatalog()
        {
            return new ModuleCatalog().
                AddModule(typeof(Sheng.SailingEase.Modules.LaunchModule.LaunchModule)).
                AddModule(typeof(Sheng.SailingEase.Modules.ProjectModule.ProjectModule), "LaunchModule").
                AddModule(typeof(Sheng.SailingEase.Modules.StartPageModule.StartPageModule), "ProjectModule").
                AddModule(typeof(Sheng.SailingEase.Modules.DataBaseSourceModule.DataBaseSourceModule)).
                AddModule(typeof(Sheng.SailingEase.Components.DataEntityComponent.DataEntityComponent), "ProjectModule").
                AddModule(typeof(Sheng.SailingEase.Components.DictionaryComponent.DictionaryComponent), "ProjectModule").
                AddModule(typeof(Sheng.SailingEase.Components.NavigationComponent.NavigationComponent), "ProjectModule").
                AddModule(typeof(Sheng.SailingEase.Components.WindowComponent.WindowComponent), "ProjectModule").
                AddModule(typeof(Sheng.SailingEase.Components.ResourceComponent.ResourceComponent), "ProjectModule").
                AddModule(typeof(Sheng.SailingEase.Components.Window.DesignComponent.WindowDesignComponent), "ProjectModule");
        }
        public ShellView Shell { get; private set; }
    }
}
