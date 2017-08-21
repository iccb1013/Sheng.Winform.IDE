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
using Microsoft.Practices.Unity;
using Microsoft.Practices.Composite.Events;
using Sheng.SailingEase.Infrastructure;
using System.IO;
using Sheng.SailingEase.Kernal;
using System.Xml.Linq;
using System.Windows.Forms;
using Sheng.SailingEase.Core;
namespace Sheng.SailingEase.Modules.ProjectModule
{
    class ProjectArchive
    {
        private IEventAggregator _eventAggregator = ServiceUnity.EventAggregator;
        private IPackageService _packageService = ServiceUnity.PackageService;
        private ProjectService projectService = (ProjectService)ServiceUnity.ProjectService;
        private static InstanceLazy<ProjectArchive> _instance =
            new InstanceLazy<ProjectArchive>(() => new ProjectArchive());
        public static ProjectArchive Instance
        {
            get { return _instance.Value; }
        }
        private ProjectArchive()
        {
        }
        public void NewProject(string folder, string name)
        {
            string path = Path.Combine(folder, name + Constant.PROJECT_FILE_EXTENSION);
            IPackage package = _packageService.Create(path);
            ProjectSummary summary = new ProjectSummary();
            summary.FirstRun = true;
            package.AddFileContent(summary.ToXml(), Constant.PACKAGE_SUMMARY_FILE_NAME);
            ProjectEntityDev projectEntity = new ProjectEntityDev();
            projectEntity.Name = name;
            projectEntity.Code = "NewProject";
            projectEntity.UserModel = true;
            projectEntity.UserPopedomModel = true;
            projectEntity.UserSubsequent = true;
            package.AddFileContent(projectEntity.ToXml(), Constant.PACKAGE_PROJECT_FILE_NAME);
            package.Close();
            ProjectEventArgs args = new ProjectEventArgs(path);
            _eventAggregator.GetEvent<ProjectCreatedEvent>().Publish(args);
        }
        public void Open(string path)
        {
            ProjectEventArgs preOpenEventArgs = new ProjectEventArgs(path);
            _eventAggregator.GetEvent<ProjectPreOpenEvent>().Publish(preOpenEventArgs);
            _packageService.Open(path);
            string strProject = _packageService.Current.GetFileContent(Constant.PACKAGE_PROJECT_FILE_NAME);
            ProjectEntityDev projectEntity = new ProjectEntityDev();
            projectEntity.FromXml(strProject);
            string strProjectSummary = _packageService.Current.GetFileContent(Constant.PACKAGE_SUMMARY_FILE_NAME);
            ProjectSummary projectSummary = new ProjectSummary();
            projectSummary.FromXml(strProjectSummary);
            Project project = new Project(projectEntity, projectSummary);
            project.ProjectFile = path;
            projectService.Current = project;
            ProjectAvailableEventArgs openedEventArgs = new ProjectAvailableEventArgs(project);
            _eventAggregator.GetEvent<ProjectOpenedEvent>().Publish(openedEventArgs);
            projectSummary.FirstRun = false;
            Save(projectSummary);
            return;
        }
        public void Save(ProjectEntity projectEntity)
        {
            _packageService.Current.AddFileContent(projectEntity.ToXml(), Constant.PACKAGE_PROJECT_FILE_NAME);
            ProjectAvailableEventArgs args = new ProjectAvailableEventArgs(projectService.Current);
            _eventAggregator.GetEvent<ProjectSavedEvent>().Publish(args);
        }
        public void Save(ProjectSummary projectSummary)
        {
            _packageService.Current.AddFileContent(projectSummary.ToXml(), Constant.PACKAGE_SUMMARY_FILE_NAME);
        }
    }
}
