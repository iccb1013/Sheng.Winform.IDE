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
    class DataListDeleteRowDevEditorAdapter : EventEditorAdapterAbstract
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
        public DataListDeleteRowDevEditorAdapter(EventBase hostEvent)
            : base(hostEvent)
        {
        }
        protected override void CreateRootNode()
        {
            EditorNode = new EventEditorNode(Language.Current.DataListDeleteRowDev_EditorPanel_Parameter);
            EditorNode.ImageIndex = FormEventEditor.EditorTreeNodeIcons.Method;
            EditorNode.SelectedImageIndex = FormEventEditor.EditorTreeNodeIcons.Method;
        }
        protected override void CreateEditorNode()
        {
            this.ParameterPanels = new Panels();
            ParameterPanels.Where = new UserControlEventEditorPanel_DataListDeleteRow_Where(this);
            this.EventEditorPanelList = new List<IEventEditorPanel>();
            this.EventEditorPanelList.Add(ParameterPanels.Where);
        }
        public class Panels
        {
            private UserControlEventEditorPanel_DataListDeleteRow_Where where;
            public UserControlEventEditorPanel_DataListDeleteRow_Where Where
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
        }
    }
}
