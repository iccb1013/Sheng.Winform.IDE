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
    public class DataEntityAddedEvent : CompositeEvent<DataEntityEventArgs>
    {
    }
    public class DataEntityRemovedEvent : CompositeEvent<DataEntityEventArgs>
    {
    }
    public class DataEntityUpdatedEvent : CompositeEvent<DataEntityEventArgs>
    {
    }
    public class DataItemEntityAddedEvent : CompositeEvent<DataItemEntityEventArgs>
    {
    }
    public class DataItemEntityRemovedEvent : CompositeEvent<DataItemEntityEventArgs>
    {
    }
    public class DataItemEntityUpdatedEvent : CompositeEvent<DataItemEntityEventArgs>
    {
    }
}
