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
using IDesign.Composite.Events;
namespace Sheng.SailingEase.Infrastructure
{
    public class ProjectCreatedEvent : CompositeEvent<ProjectEventArgs>
    {
    }
    public class ProjectPreOpenEvent : CompositeEvent<ProjectEventArgs>
    {
    }
    public class ProjectOpenedEvent : CompositeEvent<ProjectAvailableEventArgs>
    {
    }
    public class ProjectPreCloseEvent : CompositeEvent<ProjectAvailableEventArgs>
    {
    }
    public class ProjectClosedEvent : CompositeEvent<ProjectEventArgs>
    {
    }
    public class ProjectSavedEvent : CompositeEvent<ProjectAvailableEventArgs>
    {
    }
}
