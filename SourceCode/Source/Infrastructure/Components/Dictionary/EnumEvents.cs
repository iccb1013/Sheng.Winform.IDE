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
    public class EnumAddedEvent : CompositeEvent<EnumEventArgs>
    {
    }
    public class EnumRemovedEvent : CompositeEvent<EnumEventArgs>
    {
    }
    public class EnumUpdatedEvent : CompositeEvent<EnumEventArgs>
    {
    }
    public class EnumItemEntityAddedEvent : CompositeEvent<EnumItemEventArgs>
    {
    }
    public class EnumItemEntityRemovedEvent : CompositeEvent<EnumItemEventArgs>
    {
    }
    public class EnumItemEntityUpdatedEvent : CompositeEvent<EnumItemEventArgs>
    {
    }
    public class EnumItemMoveAlongEvent : CompositeEvent<EnumItemMoveAlongEventArgs>
    {
    }
    public class EnumItemMoveBackEvent : CompositeEvent<EnumItemMoveBackEventArgs>
    {
    }
}
