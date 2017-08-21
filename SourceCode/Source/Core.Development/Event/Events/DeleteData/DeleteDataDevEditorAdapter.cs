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
    class DeleteDataDevEditorAdapter : EventEditorAdapterAbstract
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
        public DeleteDataDevEditorAdapter(EventBase hostEvent)
            : base(hostEvent)
        {
        }
        protected override void CreateEditorNode()
        {
            this.ParameterPanels = new Panels();
            ParameterPanels.General = new UserControlEventEditorPanel_DeleteData_General(this);
            ParameterPanels.DataEntity = new UserControlEventEditorPanel_DeleteData_DataEntity(this);
            ParameterPanels.SqlRegex = new UserControlEventEditorPanel_DeleteData_SqlRegex(this);
            this.EventEditorPanelList = new List<IEventEditorPanel>();
            this.EventEditorPanelList.Add(ParameterPanels.General);
            this.EventEditorPanelList.Add(ParameterPanels.DataEntity);
            this.EventEditorPanelList.Add(ParameterPanels.SqlRegex);
        }
        public class Panels
        {
            private UserControlEventEditorPanel_DeleteData_General general;
            public UserControlEventEditorPanel_DeleteData_General General
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
            private UserControlEventEditorPanel_DeleteData_DataEntity dataEntity;
            public UserControlEventEditorPanel_DeleteData_DataEntity DataEntity
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
            private UserControlEventEditorPanel_DeleteData_SqlRegex sqlRegex;
            public UserControlEventEditorPanel_DeleteData_SqlRegex SqlRegex
            {
                get
                {
                    return this.sqlRegex;
                }
                set
                {
                    this.sqlRegex = value;
                }
            }
        }
    }
}
