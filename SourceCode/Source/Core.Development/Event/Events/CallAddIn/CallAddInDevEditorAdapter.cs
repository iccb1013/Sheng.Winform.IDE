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
using System.Xml;
namespace Sheng.SailingEase.Core.Development
{
    class CallAddInDevEditorAdapter : EventEditorAdapterAbstract
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
        public CallAddInDevEditorAdapter(EventBase hostEvent)
            : base(hostEvent)
        {
        }
        protected override void CreateEditorNode()
        {
            this.ParameterPanels = new Panels();
            ParameterPanels.General = new UserControlEventEditorPanel_CallAddIn_General(this);
            this.EventEditorPanelList = new List<IEventEditorPanel>();
            this.EventEditorPanelList.Add(ParameterPanels.General);
        }
        public class Panels
        {
            private UserControlEventEditorPanel_CallAddIn_General general;
            public UserControlEventEditorPanel_CallAddIn_General General
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
        }
    }
}
