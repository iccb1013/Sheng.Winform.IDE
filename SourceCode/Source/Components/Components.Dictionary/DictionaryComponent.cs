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
using Sheng.SailingEase.Controls.Extensions;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Components.DictionaryComponent.Localisation;
using Sheng.SailingEase.Components.DictionaryComponent.View;
using Microsoft.Practices.Unity;
namespace Sheng.SailingEase.Components.DictionaryComponent
{
    public class DictionaryComponent : IModule
    {
        IUnityContainer _container;
        INavigationService _navigationService;
        IProjectService _projectService;
        IEventAggregator _eventAggregator;
        IWorkbenchService _workbenchService;
        public DictionaryComponent(IUnityContainer container, IEventAggregator eventAggregator, IWorkbenchService workbenchService)
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
            _navigationService.RegisterMenu("Main/Edit[2]", new ToolStripMenuItemCodon("Dictionary",
                Language.Current.Navigation_Menu_Dictionary, Resources.Enum,
                  (sender, codon) => { _workbenchService.Show<EnumView>(EnumView.SINGLEKEY); })
            {
                IsEnabled = projectIsOpend
            });
            _navigationService.RegisterToolStrip("Main", new ToolStripButtonCodon("Dictionary",
                Language.Current.Navigation_ToolStrip_Dictionary, Resources.Enum,
                (sender, e) => { _workbenchService.Show<EnumView>(EnumView.SINGLEKEY); })
            {
                IsEnabled = projectIsOpend
            });
        }
        private void ConfigureContainer()
        {
            _container.RegisterInstance<DictionaryArchive>(DictionaryArchive.Instance);
            _container.RegisterInstance<IDictionaryComponentService>(DictionaryComponentService.Instance);
        }
        private void SubscribeEvent()
        {
        }
    }
}
