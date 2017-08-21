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
    class ClearFormDataDevEditorAdapter : EventEditorAdapterAbstract
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
        public ClearFormDataDevEditorAdapter(EventBase hostEvent)
            : base(hostEvent)
        {
        }
        protected override void CreateEditorNode()
        {
            ParameterPanels = new Panels();
            ParameterPanels.General = new UserControlEventEditorPanel_ClearFormData_General(this);
            ParameterPanels.FormElement = new UserControlEventEditorPanel_ClearFormData_FormElement(this);
            this.EventEditorPanelList = new List<IEventEditorPanel>();
            this.EventEditorPanelList.Add(ParameterPanels.General);
            this.EventEditorPanelList.Add(ParameterPanels.FormElement);
        }
        public class Panels
        {
            private UserControlEventEditorPanel_ClearFormData_General general;
            public UserControlEventEditorPanel_ClearFormData_General General
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
            private UserControlEventEditorPanel_ClearFormData_FormElement formElement;
            public UserControlEventEditorPanel_ClearFormData_FormElement FormElement
            {
                get
                {
                    return this.formElement;
                }
                set
                {
                    this.formElement = value;
                }
            }
        }
    }
}
