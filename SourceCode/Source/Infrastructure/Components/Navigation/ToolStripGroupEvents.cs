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
    public class ToolStripGroupAddedEvent : CompositeEvent<ToolStripGroupEventArgs>
    {
    }
    public class ToolStripGroupRemovedEvent : CompositeEvent<ToolStripGroupEventArgs>
    {
    }
    public class ToolStripGroupUpdatedEvent : CompositeEvent<ToolStripGroupEventArgs>
    {
    }
    public class ToolStripGroupMoveBeforeEvent : CompositeEvent<ToolStripGroupMoveBeforeEventArgs>
    {
    }
    public class ToolStripGroupMoveAfterEvent : CompositeEvent<ToolStripGroupMoveAfterEventArgs>
    {
    }
}
