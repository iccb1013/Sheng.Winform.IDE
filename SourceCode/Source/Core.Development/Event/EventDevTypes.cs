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
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Core.Development
{
    public class EventDevTypes : EventTypesAbstract
    {
        private static InstanceLazy<EventDevTypes> _instance =
            new InstanceLazy<EventDevTypes>(() => new EventDevTypes());
        public static EventDevTypes Instance
        {
            get { return _instance.Value; }
        }
        private EventDevTypes()
        {
            Add(typeof(ClearFormDataDev));
            Add(typeof(ExitDev));
            Add(typeof(LoadDataToFormDev));
            Add(typeof(LockProgramDev));
            Add(typeof(NewGuidDev));
            Add(typeof(OpenWindowDev));
            Add(typeof(ReceiveDataDev));
            Add(typeof(ReLoginDev));
            Add(typeof(SaveDataDev));
            Add(typeof(StartProcessDev));
            Add(typeof(UpdateFormDataDev));
            Add(typeof(CloseFormDev));
            Add(typeof(OpenSystemFormDev));
            Add(typeof(ValidateFormDataDev));
            Add(typeof(CallAddInDev));
            Add(typeof(ReturnDataToCallerFormDev));
            Add(typeof(DeleteDataDev));
            Add(typeof(DataListRefreshDev));
            Add(typeof(CallUIElementMethodDev));
            Add(typeof(DataListAddRowDev));
            Add(typeof(DataListUpdateRowDev));
            Add(typeof(DataListDeleteRowDev));
        }
    }
}
