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
using System.Windows.Forms;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Unity;
using Sheng.SailingEase.Controls.Extensions;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Localisation;
using Sheng.SailingEase.Modules.DataBaseSourceModule.Localisation;
namespace Sheng.SailingEase.Modules.DataBaseSourceModule
{
    public class DataBaseSourceModule : IModule
    {
        IUnityContainer _container;
        INavigationService _navigationService;
        IProjectService _projectService;
        IEventAggregator _eventAggregator;
        IWorkbenchService _workbenchService;
        public DataBaseSourceModule(IUnityContainer container, IEventAggregator eventAggregator, IWorkbenchService workbenchService)
        {
            _container = container;
            _eventAggregator = eventAggregator;
            _workbenchService = workbenchService;
        }
        public void Initialize()
        {
            ConfigureContainer();
            _navigationService = _container.Resolve<INavigationService>();
            _projectService = _container.Resolve<IProjectService>();
            RegisterNavigationItem();
            SubscribeEvent();
        }
        private void RegisterNavigationItem()
        {
            Func<IToolStripItemCodon, bool> projectIsOpend = (codon) => { return _projectService.Current != null; };
            _navigationService.RegisterMenu("Main/Tool[0]", new ToolStripMenuItemCodon("DataSourceSet",
                    Language.Current.Navigation_Menu_DataSourceSet, (sender, codon) => {
                        using (DataSourceSetView view = new DataSourceSetView())
                        {
                            view.ShowDialog();
                        }
                    }));
        }
        private void ConfigureContainer()
        {
        }
        private void SubscribeEvent()
        {
        }
    }
}
