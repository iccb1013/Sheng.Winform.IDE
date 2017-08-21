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
using Sheng.SailingEase.Modules.StartPageModule.Localisation;
using Sheng.SailingEase.Modules.StartPageModule.View;
namespace Sheng.SailingEase.Modules.StartPageModule
{
    public class StartPageModule : IModule
    {
        IUnityContainer _container;
        INavigationService _navigationService;
        IEventAggregator _eventAggregator;
        IWorkbenchService _workbenchService;
        public StartPageModule(IUnityContainer container, IEventAggregator eventAggregator, IWorkbenchService workbenchService)
        {
            _container = container;
            _eventAggregator = eventAggregator;
            _workbenchService = workbenchService;
        }
        public void Initialize()
        {
            RegisterNavigationItem();
            SubscribeEvent();
        }
        private void RegisterNavigationItem()
        {
            _navigationService = _container.Resolve<INavigationService>();
            _navigationService.RegisterMenu("Main/View[0]", new ToolStripMenuItemCodon("Welcome",
                    Language.Current.Navigation_Menu_StartPage, (sender, codon) =>
                    {
                        _workbenchService.Show<StartPageView>(StartPageScheme.STARTPAGE_URI);
                    }));
        }
        private void SubscribeEvent()
        {
            _eventAggregator.GetEvent<ApplicationRunEvent>().Subscribe((e) =>
            {
                _workbenchService.Show<StartPageView>(StartPageScheme.STARTPAGE_URI);
            });
            _eventAggregator.GetEvent<ProjectOpenedEvent>().Subscribe((e) =>
            {
                History.Add(e.Project.ProjectFile);
            });
        }
    }
}
