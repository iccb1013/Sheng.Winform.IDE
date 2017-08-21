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
using Sheng.SailingEase.Infrastructure;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Unity;
using System.Windows.Forms;
using System.IO;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Modules.ProjectModule.View;
namespace Sheng.SailingEase.Modules.ProjectModule
{
    class ProjectService : IProjectService
    {
        IUnityContainer _container;
        IEventAggregator _eventAggregator;
        IPackageService _packageService;
        public ProjectService(IUnityContainer container, IEventAggregator eventAggregator,IPackageService packageService)
        {
            _container = container;
            _eventAggregator = eventAggregator;
            _packageService = packageService;
        }
        private IProject _current;
        public IProject Current
        {
            get { return _current; }
            internal set { _current = value; }
        }
        public void NewProject()
        {
            CreateProjectView.ShowView();
        }
        public void OpenProject()
        {
            string file;
            if (DialogUnity.OpenFile(Constant.OPEN_PROJECT_FILTER, out file))
            {
                OpenProject(file);
            }
        }
        public void OpenProject(string path)
        {
            if (_current != null)
                CloseProject();
            ProjectArchive projectArchive = _container.Resolve<ProjectArchive>();
            projectArchive.Open(path);           
        }
        public void CloseProject()
        {
            ProjectAvailableEventArgs preCloseEventArgs = new ProjectAvailableEventArgs(_current);
            _eventAggregator.GetEvent<ProjectPreCloseEvent>().Publish(preCloseEventArgs);
            _current = null;
            ProjectEventArgs closedEventArgs = new ProjectEventArgs(_packageService.Current.PackageFile);
            _eventAggregator.GetEvent<ProjectClosedEvent>().Publish(closedEventArgs);
        }
    }
}
