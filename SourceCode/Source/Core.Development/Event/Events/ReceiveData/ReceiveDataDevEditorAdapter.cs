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
namespace Sheng.SailingEase.Core.Development
{
    class ReceiveDataDevEditorAdapter : EventEditorAdapterAbstract
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
        public ReceiveDataDevEditorAdapter(EventBase hostEvent)
            : base(hostEvent)
        {
        }
        protected override void CreateEditorNode()
        {
            this.ParameterPanels = new Panels();
            ParameterPanels.General = new UserControlEventEditorPanel_ReceiveData_General(this);
            ParameterPanels.ReceiveData = new UserControlEventEditorPanel_ReceiveData_ReceiveData(this);
            this.EventEditorPanelList = new List<IEventEditorPanel>();
            this.EventEditorPanelList.Add(ParameterPanels.General);
            this.EventEditorPanelList.Add(ParameterPanels.ReceiveData);
        }
        public class Panels
        {
            private UserControlEventEditorPanel_ReceiveData_General general;
            public UserControlEventEditorPanel_ReceiveData_General General
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
            private UserControlEventEditorPanel_ReceiveData_ReceiveData receiveData;
            public UserControlEventEditorPanel_ReceiveData_ReceiveData ReceiveData
            {
                get
                {
                    return this.receiveData;
                }
                set
                {
                    this.receiveData = value;
                }
            }
        }
    }
}
