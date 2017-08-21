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
    public class ToolStripItemAddedEvent : CompositeEvent<ToolStripEventArgs>
    {
    }
    public class ToolStripItemRemovedEvent : CompositeEvent<ToolStripEventArgs>
    {
    }
    public class ToolStripItemUpdatedEvent : CompositeEvent<ToolStripEventArgs>
    {
    }
    public class ToolStripItemMoveBeforeEvent : CompositeEvent<ToolStripItemMoveBeforeEventArgs>
    {
    }
    public class ToolStripItemMoveAfterEvent : CompositeEvent<ToolStripItemMoveAfterEventArgs>
    {
    }
}
