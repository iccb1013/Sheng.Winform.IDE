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
using Sheng.SailingEase.Components.ResourceComponent.Localisation;
using Sheng.SailingEase.Components.ResourceComponent.View;
using Sheng.SailingEase.Controls.Extensions;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Components.ResourceComponent
{
    public class ResourceComponent : IModule
    {
        IUnityContainer _container;
        INavigationService _navigationService;
        IProjectService _projectService;
        IEventAggregator _eventAggregator;
        IWorkbenchService _workbenchService;
        public ResourceComponent(IUnityContainer container, IEventAggregator eventAggregator, IWorkbenchService workbenchService)
        {
            _container = container;
            _eventAggregator = eventAggregator;
            _workbenchService = workbenchService;
            _navigationService = _container.Resolve<INavigationService>();
            _projectService = _container.Resolve<IProjectService>();
        }
        public void Initialize()
        {
            ConfigureContainer();
            RegisterNavigationItem();
            SubscribeEvent();
        }
        private void RegisterNavigationItem()
        {
            Func<IToolStripItemCodon, bool> projectIsOpend = (codon) => { return _projectService.Current != null; };
            _navigationService.RegisterMenu("Main/Edit[5]", new ToolStripMenuItemCodon("Resource",
                Language.Current.Navigation_Menu_Resource, IconsLibrary.Image2,
                  (sender, codon) => { _workbenchService.Show<ExplorerView>(ExplorerView.SINGLEKEY); })
            {
                IsEnabled = projectIsOpend
            });
            _navigationService.RegisterToolStrip("Main", new ToolStripButtonCodon("Resource",
                Language.Current.Navigation_ToolStrip_Resource, IconsLibrary.Image2,
                (sender, e) => { _workbenchService.Show<ExplorerView>(ExplorerView.SINGLEKEY); })
            {
                IsEnabled = projectIsOpend
            });
        }
        private void ConfigureContainer()
        {
            _container.RegisterInstance<IResourceComponentService>(ResourceComponentService.Instance);
        }
        private void SubscribeEvent()
        {
        }
    }
}
