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
    class ReturnDataToCallerFormDevEditorAdapter : EventEditorAdapterAbstract
    {
        private Panels _parameterPanels;
        public Panels ParameterPanels
        {
            get
            {
                return this._parameterPanels;
            }
            set
            {
                this._parameterPanels = value;
            }
        }
        public ReturnDataToCallerFormDevEditorAdapter(EventBase hostEvent)
            : base(hostEvent)
        {
        }
        protected override void CreateEditorNode()
        {
            this.ParameterPanels = new Panels();
            ParameterPanels.General = new UserControlEventEditorPanel_ReturnDataToCallerForm_General(this);
            ParameterPanels.ReturnData = new UserControlEventEditorPanel_ReturnDataToCallerForm_ReturnData(this);
            this.EventEditorPanelList = new List<IEventEditorPanel>();
            this.EventEditorPanelList.Add(ParameterPanels.General);
            this.EventEditorPanelList.Add(ParameterPanels.ReturnData);
        }
        public class Panels
        {
            private UserControlEventEditorPanel_ReturnDataToCallerForm_General general;
            public UserControlEventEditorPanel_ReturnDataToCallerForm_General General
            {
                get
                {
                    return this.general;
                }
                set
                {
                    this.general = value;
                }
            }
            private UserControlEventEditorPanel_ReturnDataToCallerForm_ReturnData returnData;
            public UserControlEventEditorPanel_ReturnDataToCallerForm_ReturnData ReturnData
            {
                get
                {
                    return this.returnData;
                }
                set
                {
                    this.returnData = value;
                }
            }
        }
    }
}
