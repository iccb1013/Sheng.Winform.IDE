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
using Sheng.SailingEase.Components.DataEntityComponent.Localisation;
using Sheng.SailingEase.Components.DataEntityComponent.View;
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Components.DataEntityComponent
{
    public class DataEntityComponent : IModule
    {
        IUnityContainer _container;
        INavigationService _navigationService;
        IProjectService _projectService;
        IEventAggregator _eventAggregator;
        IWorkbenchService _workbenchService;
        public DataEntityComponent(IUnityContainer container, IEventAggregator eventAggregator, IWorkbenchService workbenchService)
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
            _navigationService.RegisterMenu("Main/Edit[2]", new ToolStripMenuItemCodon("DataEntity",
                Language.Current.Navigation_Menu_DataEntity, Resources.DataEntity,
                  (sender, codon) => { _workbenchService.Show<ExplorerView>(ExplorerView.SINGLEKEY); })
                {
                    IsEnabled = projectIsOpend
                });
            _navigationService.RegisterMenu("Main/Build[0]", new ToolStripMenuItemCodon("DataBaseCreateWizard",
                Language.Current.Navigation_Menu_DataBaseCreateWizard, IconsLibrary.DataSource2,
                  (sender, codon) => { DataBaseCreateWizard.Show(); })
            {
                IsEnabled = projectIsOpend
            });
            _navigationService.RegisterToolStrip("Main", new ToolStripButtonCodon("DataEntity",
                Language.Current.Navigation_ToolStrip_DataEntity, Resources.DataEntity,
                (sender, e) => { _workbenchService.Show<ExplorerView>(ExplorerView.SINGLEKEY); })
                {
                    IsEnabled = projectIsOpend
                });
        }
        private void ConfigureContainer()
        {
            _container.RegisterInstance<DataEntityArchive>(DataEntityArchive.Instance);
            _container.RegisterInstance<IDataEntityComponentService>(DataEntityComponentService.Instance);
        }
        private void SubscribeEvent()
        {
        }
    }
}
