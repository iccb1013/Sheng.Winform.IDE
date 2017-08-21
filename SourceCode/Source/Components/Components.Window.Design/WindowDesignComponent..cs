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
using Sheng.SailingEase.Controls.Extensions;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    public class WindowDesignComponent : IModule
    {
        IUnityContainer _container;
        INavigationService _navigationService;
        IProjectService _projectService;
        IEventAggregator _eventAggregator;
        IWorkbenchService _workbenchService;
        public WindowDesignComponent(IUnityContainer container, IEventAggregator eventAggregator,
            IWorkbenchService workbenchService)
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
            SubscribeEvent();
        }
        private void ConfigureContainer()
        {
            _container.RegisterInstance<IWindowDesignService>(WindowDesignService.Instance);
        }
        private void SubscribeEvent()
        {
        }
    }
}
