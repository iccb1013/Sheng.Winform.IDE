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
namespace Sheng.SailingEase.Core.Development
{
    public static class WarningCheckerHelper
    {
        public static void EventsValidate(IWarningable entity)
        {
            IEventSupport eventSupport = entity as IEventSupport;
            if (eventSupport != null)
            {
                foreach (EventBase _event in eventSupport.Events)
                {
                    IWarningable eventWarning = _event as IWarningable;
                    if (eventWarning == null)
                        continue;
                    eventWarning.CheckWarning();
                    if (eventWarning.Warning.ExistWarning)
                    {
                        entity.Warning.AddWarningSign(eventWarning.Warning);
                    }
                }
            }
        }
        [Obsolete("对事件的 Check 改用 EventsValidate 方法", true)]
        public static void CheckEventWarning(IWarningable entity)
        {
        }
    }
}
