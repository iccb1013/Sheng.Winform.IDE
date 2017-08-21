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
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Core
{
    class EventTypes : EventTypesAbstract
    {
        private static InstanceLazy<EventTypes> _instance =
            new InstanceLazy<EventTypes>(() => new EventTypes());
        public static EventTypes Instance
        {
            get { return _instance.Value; }
        }
        private EventTypes()
        {
            Add(typeof(ClearFormDataEvent));
            Add(typeof(ExitEvent));
            Add(typeof(LoadDataToFormEvent));
            Add(typeof(LockProgramEvent));
            Add(typeof(NewGuidEvent));
            Add(typeof(OpenWindowEvent));
            Add(typeof(ReceiveDataEvent));
            Add(typeof(ReLoginEvent));
            Add(typeof(SaveDataEvent));
            Add(typeof(StartProcessEvent));
            Add(typeof(UpdateFormDataEvent));
            Add(typeof(CloseFormEvent));
            Add(typeof(OpenSystemFormEvent));
            Add(typeof(ValidateFormDataEvent));
            Add(typeof(CallAddInEvent));
            Add(typeof(ReturnDataToCallerFormEvent));
            Add(typeof(DeleteDataEvent));
            Add(typeof(DataListRefreshEvent));
            Add(typeof(CallUIElementMethodEvent));
            Add(typeof(DataListAddRowEvent));
            Add(typeof(DataListUpdateRowEvent));
            Add(typeof(DataListDeleteRowEvent));
        }
    }
}
