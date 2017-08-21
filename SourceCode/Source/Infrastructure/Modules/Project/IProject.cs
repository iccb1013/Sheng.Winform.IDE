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
    public interface IProject
    {
        IProjectSummary ProjectSummary { get; }
        string ProjectFile { get; }
        string ProjectPath { get; }
        string Name { get; }
        string Code { get; }
        string Version { get; }
        string Company { get; }
        string Summary { get; }
        string Copyright { get; }
    }
}
