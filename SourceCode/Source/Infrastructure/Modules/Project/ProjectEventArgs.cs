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
namespace Sheng.SailingEase.Infrastructure
{
    public class ProjectAvailableEventArgs
    {
        public ProjectAvailableEventArgs(IProject project)
        {
            Project = project;
        }
        public IProject Project
        {
            get;
            private set;
        }
    }
    public class ProjectEventArgs
    {
        public ProjectEventArgs(string projectFile)
        {
            ProjectFile = projectFile;
        }
        public string ProjectFile
        {
            get;
            set;
        }
    }
}
