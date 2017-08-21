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
    class DataListRefreshDevEditorAdapter : EventEditorAdapterAbstract
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
        public DataListRefreshDevEditorAdapter(EventBase hostEvent)
            : base(hostEvent)
        {
        }
        protected override void CreateRootNode()
        {
            EditorNode = new EventEditorNode(Language.Current.DataListRefreshDev_EditorPanel_Parameter);
            EditorNode.ImageIndex = FormEventEditor.EditorTreeNodeIcons.Method;
            EditorNode.SelectedImageIndex = FormEventEditor.EditorTreeNodeIcons.Method;
        }
        protected override void CreateEditorNode()
        {
            ParameterPanels = new Panels();
            ParameterPanels.General = new UserControlEventEditorPanel_DataListRefresh_General(this);
            ParameterPanels.Where = new UserControlEventEditorPanel_DataListRefresh_Where(this);
            ParameterPanels.SqlRegex = new UserControlEventEditorPanel_DataListRefresh_SqlRegex(this);
            this.EventEditorPanelList = new List<IEventEditorPanel>();
            this.EventEditorPanelList.Add(ParameterPanels.General);
            this.EventEditorPanelList.Add(ParameterPanels.Where);
            this.EventEditorPanelList.Add(ParameterPanels.SqlRegex);
        }
        public class Panels
        {
            private UserControlEventEditorPanel_DataListRefresh_General general;
            public UserControlEventEditorPanel_DataListRefresh_General General
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
            private UserControlEventEditorPanel_DataListRefresh_Where where;
            public UserControlEventEditorPanel_DataListRefresh_Where Where
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
            private UserControlEventEditorPanel_DataListRefresh_SqlRegex sqlRegex;
            public UserControlEventEditorPanel_DataListRefresh_SqlRegex SqlRegex
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
