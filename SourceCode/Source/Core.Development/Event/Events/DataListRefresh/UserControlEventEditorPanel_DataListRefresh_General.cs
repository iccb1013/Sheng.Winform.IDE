/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Sheng.SailingEase.Core.Development;
using Sheng.SailingEase.Core.Development.Localisation;
namespace Sheng.SailingEase.Core.Development
{
    [ToolboxItem(false)]
    partial class UserControlEventEditorPanel_DataListRefresh_General : UserControlEventEditorPanelBase
    {
        public EnumTargetWindow ObjectForm
        {
            get;
            set;
        }
        private string dataEntityId;
        public string DataEntityId
        {
            get
            {
                return this.dataEntityId;
            }
            set
            {
                this.dataEntityId = value;
            }
        }
        private string dataListId;
        public string DataListId
        {
            get
            {
                return this.dataListId;
            }
            set
            {
                this.dataListId = value;
            }
        }
        public UserControlEventEditorPanel_DataListRefresh_General(EventEditorAdapterAbstract hostAdapter)
            : base(hostAdapter)
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            radioButtonDataEntity.Checked = true;
        }
        private void ApplyLanguageResource()
        {
        }
        public override string PanelTitle
        {
            get
            {
                return Language.Current.DataListRefreshDev_EditorPanel_General;
            }
        }
        public override bool DefaultPanel
        {
            get
            {
                return true;
            }
        }
        public override List<XObject> GetXml()
        {
            List<XObject> _xObject = new List<XObject>();
            if (radioButtonDataEntity.Checked)
            {
                _xObject.Add(new XElement("Mode",  ((int)SaveDataEvent.EnumSaveDataMode.DataEntity).ToString()));
            }
            else
            {
                _xObject.Add(new XElement("Mode", ((int)SaveDataEvent.EnumSaveDataMode.SQL).ToString()));
            }
            _xObject.Add(new XElement("DataListId", this.DataListId));
            return _xObject;
        }
        public override void SetParameter(EventBase even)
        {
            DataListRefreshDev _event = even as DataListRefreshDev;
            if (_event.RefreshMode == DataListRefreshEvent.EnumRefreshMode.DataEntity)
            {
                this.radioButtonDataEntity.Checked = true;
            }
            else
            {
                this.radioButtonSql.Checked = true;
            }
            this.DataListId = _event.DataListId;
        }
        public override bool ValidateParameter(out string validateMsg)
        {
            return base.ValidateParameter(out validateMsg);
        }
    }
}
