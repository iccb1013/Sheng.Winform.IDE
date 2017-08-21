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
using System.IO;
using Sheng.SailingEase.Core;
namespace Sheng.SailingEase.Modules.ProjectModule
{
    class Project : IProject
    {
        private ProjectEntity _projectEntity;
        internal ProjectEntity ProjectEntity
        {
            get { return _projectEntity; }
        }
        public Project(ProjectEntity projectEntity, IProjectSummary projectSummary)
        {
            _projectEntity = projectEntity;
            _projectSummary = projectSummary;
        }
        private IProjectSummary _projectSummary;
        public IProjectSummary ProjectSummary
        {
            get { return _projectSummary; }
        }
        private string _projectFile;
        public string ProjectFile
        {
            get { return _projectFile; }
            internal set
            {
                _projectFile = value;
                _projectPath = Path.GetDirectoryName(value);
            }
        }
        private string _projectPath;
        public string ProjectPath
        {
            get { return _projectPath; }
        }
        public string ResourcesPath
        {
            get { return Path.Combine(Path.GetDirectoryName(_projectPath), Constant.RESOURCES_FOLDER_NAME); }
        }
        public string Name
        {
            get { return _projectEntity.Name; }
        }
        public string Code
        {
            get { return _projectEntity.Code; }
        }
        public string Version
        {
            get { return _projectEntity.Version; }
        }
        public string Company
        {
            get { return _projectEntity.Company; }
        }
        public string Summary
        {
            get { return _projectEntity.Summary; }
        }
        public string Copyright
        {
            get { return _projectEntity.Copyright; }
        }
    }
}
