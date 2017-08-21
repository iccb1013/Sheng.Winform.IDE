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
using Sheng.SailingEase.Modules.ProjectModule.Localisation;
using Sheng.SailingEase.Modules.ProjectModule.View;
namespace Sheng.SailingEase.Modules.ProjectModule
{
    public class ProjectModule : IModule
    {
        IUnityContainer _container;
        INavigationService _navigationService;
        IProjectService _projectService;
        IEventAggregator _eventAggregator;
        IWorkbenchService _workbenchService;
        public ProjectModule(IUnityContainer container, IEventAggregator eventAggregator, IWorkbenchService workbenchService)
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
            _navigationService.RegisterMenu("Main/File[0]", new ToolStripMenuItemCodon("NewProject",
                    Language.Current.Navigation_Menu_NewProject, (sender, codon) => { _projectService.NewProject(); }));
            _navigationService.RegisterMenu("Main/File[1]", new ToolStripMenuItemCodon("OpenProject",
                   Language.Current.Navigation_Menu_OpenProject, (sender, codon) => { _projectService.OpenProject(); }));
            _navigationService.RegisterMenu("Main/File[2]", new ToolStripMenuItemCodon("CloseProject",
                   Language.Current.Navigation_Menu_CloseProject, (sender, codon) => { _projectService.CloseProject(); })
                   {
                       IsEnabled = projectIsOpend
                   });
            _navigationService.RegisterMenu("Main/File[3]", new ToolStripSeparatorCodon());
            _navigationService.RegisterMenu("Main/Edit[0]", new ToolStripMenuItemCodon("ProjectProperty",
                    Language.Current.Navigation_Menu_ProjectProperty, (sender, codon) => { ProjectPropertyView.ShowView(); })
                    {
                        IsEnabled = projectIsOpend
                    });
            _navigationService.RegisterMenu("Main/View[1]", new ToolStripMenuItemCodon("ProjectStartPage",
                    Language.Current.Navigation_Menu_ProjectStartPage, (sender, codon) =>
                    {
                        _workbenchService.Show<ProjectStartPageView>(ProjectStartPageScheme.PROJECT_STARTPAGE_URI);
                    })
                    {
                        IsEnabled = projectIsOpend
                    });
            _navigationService.RegisterMenu("Main/Build", new ToolStripMenuItemCodon("BuildProject",
                    Language.Current.Navigation_Menu_BuildProject, (sender, codon) => { })
                    {
                        IsEnabled = projectIsOpend
                    });
            _navigationService.RegisterToolStrip("Main", new ToolStripButtonCodon("NewProject",
                Language.Current.Navigation_ToolStrip_NewProject, IconsLibrary.New2, (sender, e) => { _projectService.NewProject(); }));
            _navigationService.RegisterToolStrip("Main", new ToolStripButtonCodon("OpenProject",
             Language.Current.Navigation_ToolStrip_OpenProject, IconsLibrary.OpenFolder, ToolStripItemDisplayStyle.ImageAndText,
                 (sender, e) => { _projectService.OpenProject(); }));
            _navigationService.RegisterToolStrip("Main", new ToolStripSeparatorCodon());
        }
        private void ConfigureContainer()
        {
            _container.RegisterInstance<IProjectService>(_container.Resolve<ProjectService>());
            _container.RegisterInstance<ProjectArchive>(ProjectArchive.Instance);
        }
        private void SubscribeEvent()
        {
            _eventAggregator.GetEvent<ProjectCreatedEvent>().Subscribe((e) =>
            {
                _projectService.OpenProject(e.ProjectFile);
            });
            _eventAggregator.GetEvent<ProjectOpenedEvent>().Subscribe((e) =>
            {
                _workbenchService.Show<ProjectStartPageView>(ProjectStartPageScheme.PROJECT_STARTPAGE_URI);
            });
        }
    }
}
