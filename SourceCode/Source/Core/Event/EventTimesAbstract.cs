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
using System.Diagnostics;
namespace Sheng.SailingEase.Core
{
    public abstract class EventTimesAbstract
    {
        
        protected List<EventTimeAbstract> _times;
        public List<EventTimeAbstract> Times
        {
            get
            {
                return _times;
            }
        }
        public string GetEventName(int code)
        {
            if (_times != null)
            {
                foreach (EventTimeAbstract eventTime in _times)
                {
                    if (eventTime.Code == code)
                        return eventTime.Name;
                }
                Debug.Assert(false, "根据指定的事件代码没有在 _times 中查到指定的事件");
                return String.Empty;
            }
            else
            {
                Debug.Assert(false, "_times 未初始化");
                return String.Empty;
            }
        }
    }
}
