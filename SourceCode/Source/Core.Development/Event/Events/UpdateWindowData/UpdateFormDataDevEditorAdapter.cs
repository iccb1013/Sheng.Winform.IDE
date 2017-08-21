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
    class UpdateFormDataDevEditorAdapter : EventEditorAdapterAbstract
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
        public UpdateFormDataDevEditorAdapter(EventBase hostEvent)
            : base(hostEvent)
        {
        }
        protected override void CreateEditorNode()
        {
            this.ParameterPanels = new Panels();
            ParameterPanels.General = new UserControlEventEditorPanel_UpdateFormData_General(this);
            ParameterPanels.Update = new UserControlEventEditorPanel_UpdateFormData_Update(this);
            ParameterPanels.Where = new UserControlEventEditorPanel_UpdateFormData_Where(this);
            ParameterPanels.SqlRegex = new UserControlEventEditorPanel_UpdateFormData_SqlRegex(this);
            this.EventEditorPanelList = new List<IEventEditorPanel>();
            this.EventEditorPanelList.Add(ParameterPanels.General);
            this.EventEditorPanelList.Add(ParameterPanels.Update);
            this.EventEditorPanelList.Add(ParameterPanels.Where);
            this.EventEditorPanelList.Add(ParameterPanels.SqlRegex);
        }
        public class Panels
        {
            private UserControlEventEditorPanel_UpdateFormData_General general;
            public UserControlEventEditorPanel_UpdateFormData_General General
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
            private UserControlEventEditorPanel_UpdateFormData_Update update;
            public UserControlEventEditorPanel_UpdateFormData_Update Update
            {
                get
                {
                    return this.update;
                }
                set
                {
                    this.update = value;
                }
            }
            private UserControlEventEditorPanel_UpdateFormData_Where where;
            public UserControlEventEditorPanel_UpdateFormData_Where Where
            {
                get
                {
                    return this.where;
                }
                set
                {
                    this.where = value;
                }
            }
            private UserControlEventEditorPanel_UpdateFormData_SqlRegex sqlRegex;
            public UserControlEventEditorPanel_UpdateFormData_SqlRegex SqlRegex
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
