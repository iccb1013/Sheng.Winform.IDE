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
using Sheng.SailingEase.Core.Development.Localisation;
namespace Sheng.SailingEase.Core.Development
{
    class DataListAddRowDevEditorAdapter : EventEditorAdapterAbstract
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
        public DataListAddRowDevEditorAdapter(EventBase hostEvent)
            : base(hostEvent)
        {
        }
        protected override void CreateRootNode()
        {
            EditorNode = new EventEditorNode(Language.Current.DataListAddRowDev_EditorPanel_Parameter);
            EditorNode.ImageIndex = FormEventEditor.EditorTreeNodeIcons.Method;
            EditorNode.SelectedImageIndex = FormEventEditor.EditorTreeNodeIcons.Method;
        }
        protected override void CreateEditorNode()
        {
            this.ParameterPanels = new Panels();
            ParameterPanels.Data = new UserControlEventEditorPanel_DataListAddRow_Data(this);
            this.EventEditorPanelList = new List<IEventEditorPanel>();
            this.EventEditorPanelList.Add(ParameterPanels.Data);
        }
        public class Panels
        {
            private UserControlEventEditorPanel_DataListAddRow_Data data;
            public UserControlEventEditorPanel_DataListAddRow_Data Data
            {
                get
                {
                    return this.data;
                }
                set
                {
                    this.data = value;
                }
            }
        }
    }
}
