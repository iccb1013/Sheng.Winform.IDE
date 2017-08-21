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
    class SaveDataDevEditorAdapter : EventEditorAdapterAbstract
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
        public SaveDataDevEditorAdapter(EventBase hostEvent)
            : base(hostEvent)
        {
        }
        protected override void CreateEditorNode()
        {
            this.ParameterPanels = new Panels();
            ParameterPanels.General = new UserControlEventEditorPanel_SaveFormData_General(this);
            ParameterPanels.DataEntity = new UserControlEventEditorPanel_SaveFormData_DataEntity(this);
            this.EventEditorPanelList = new List<IEventEditorPanel>();
            this.EventEditorPanelList.Add(ParameterPanels.General);
            this.EventEditorPanelList.Add(ParameterPanels.DataEntity);
        }
        public class Panels
        {
            private UserControlEventEditorPanel_SaveFormData_General general;
            public UserControlEventEditorPanel_SaveFormData_General General
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
            private UserControlEventEditorPanel_SaveFormData_DataEntity dataEntity;
            public UserControlEventEditorPanel_SaveFormData_DataEntity DataEntity
            {
                get
                {
                    return this.dataEntity;
                }
                set
                {
                    this.dataEntity = value;
                }
            }
        }
    }
}
