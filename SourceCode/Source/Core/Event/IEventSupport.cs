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
namespace Sheng.SailingEase.Core
{
    public delegate void OnEventUpdatedHandler(object sender, IEventSupport eventSupport);
    public interface IEventSupport
    {
       
        event OnEventUpdatedHandler EventUpdated;
        EventCollection Events { get; set; }
        EventTypeCollection EventProvide { get; }
        List<EventTimeAbstract> EventTimeProvide { get; }
        string GetEventTimeName(int code);
        void EventUpdate(object sender);
    }
}
