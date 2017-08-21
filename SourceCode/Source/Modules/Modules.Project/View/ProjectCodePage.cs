/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Modules.ProjectModule.Localisation;
using Microsoft.Practices.ServiceLocation;
namespace Sheng.SailingEase.Modules.ProjectModule.View
{
    class ProjectCodePage
    {
        private IProjectService _projectService;
        private IEnvironmentService _environmentService;
        public ProjectCodePage()
        {
            _projectService = ServiceUnity.Container.Resolve<IProjectService>();
            _environmentService = ServiceUnity.Container.Resolve<IEnvironmentService>();
        }
        public virtual void RenderSection(StringBuilder builder)
        {
            if (_projectService.Current == null)
                return;
            builder.Append(Resources.ProjectStartPage);
            builder.Replace("${Title}", Language.Current.ProjectStartPage_Title);
            builder.Replace("${CssFile}", _environmentService.DataPath +
                Path.DirectorySeparatorChar + "resources" +
                Path.DirectorySeparatorChar + "projectstartpage" +
                Path.DirectorySeparatorChar + "style.css" );
            builder.Replace("${ProjectName}", _projectService.Current.Name);
            builder.Replace("${ProjectCode}", _projectService.Current.Code);
            builder.Replace("${Version}", _projectService.Current.Version);
            builder.Replace("${Company}", _projectService.Current.Company);
            builder.Replace("${Summary}", _projectService.Current.Summary);
            builder.Replace("${Copyright}", _projectService.Current.Copyright);
        }
        public string Render(string section)
        {
            StringBuilder builder = new StringBuilder();
            switch (section.ToLowerInvariant())
            {
                case "start":
                    RenderSection(builder);
                    break;
            }
            return builder.ToString();
        }
    }
}
