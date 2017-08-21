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
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Core.Development
{
    [ToolboxItem(false)]
    partial class UserControlEventEditorPanel_LoadDataToForm_General : UserControlEventEditorPanelBase
    {
        public LoadDataToFormEvent.EnumLoadDataToFormMode Mode
        {
            get
            {
                if (radioButtonDataEntity.Checked)
                {
                    return LoadDataToFormEvent.EnumLoadDataToFormMode.DataEntity;
                }
                else
                {
                    return LoadDataToFormEvent.EnumLoadDataToFormMode.SQL;
                }
            }
        }
        public UserControlEventEditorPanel_LoadDataToForm_General(EventEditorAdapterAbstract hostAdapter)
            : base(hostAdapter)
        {
            InitializeComponent();
            this.txtCode.Regex = CoreConstant.ENTITY_CODE_REGEX;
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            radioButtonDataEntity.Checked = true;
        }
        private void UserControlEventEditorPanel_LoadDataToForm_General_Load(object sender, EventArgs e)
        {
            this.radioButtonDataEntity.CheckedChanged += new System.EventHandler(this.RadioCheckedChanged);
            this.radioButtonSql.CheckedChanged += new System.EventHandler(this.RadioCheckedChanged);
        }
        private void ApplyLanguageResource()
        {
            this.txtName.Title = Language.Current.UserControlEventEditorPanel_LoadDataToForm_General_LabelName;
            this.txtCode.Title = Language.Current.UserControlEventEditorPanel_LoadDataToForm_General_LabelCode;
            this.txtCode.RegexMsg = CommonLanguage.Current.RegexMsg_EntityCode;
        }
        private void RadioCheckedChanged(object sender, EventArgs e)
        {
            LoadDataToFormDevEditorAdapter adapter = this.HostAdapter as LoadDataToFormDevEditorAdapter;
            adapter.ParameterPanels.Load.Loads.Clear();
            adapter.ParameterPanels.DataEntity.DataEntityAllowEmpty = !radioButtonDataEntity.Checked;
        }
        public override string PanelTitle
        {
            get
            {
                return Language.Current.LoadDataToFormDev_EditorPanel_General;
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
            _xObject.Add(new XAttribute("Name", txtName.Text));
            _xObject.Add(new XAttribute("Code", txtCode.Text));
            if (radioButtonDataEntity.Checked)
            {
                _xObject.Add(new XElement("Mode", (int)LoadDataToFormEvent.EnumLoadDataToFormMode.DataEntity));
            }
            else
            {
                _xObject.Add(new XElement("Mode", (int)LoadDataToFormEvent.EnumLoadDataToFormMode.SQL));
            }
            return _xObject;
        }
        public override void SetParameter(EventBase even)
        {
            LoadDataToFormDev _event = even as LoadDataToFormDev;
            this.txtName.Text = _event.Name;
            this.txtCode.Text = _event.Code;
            if (_event.LoadMode == LoadDataToFormEvent.EnumLoadDataToFormMode.DataEntity)
            {
                this.radioButtonDataEntity.Checked = true;
            }
            else
            {
                this.radioButtonSql.Checked = true;
            }
        }
        public override bool ValidateParameter(out string validateMsg)
        {
            bool result = true;
            result = base.ValidateParameter(out validateMsg);
            if (result == false)
            {
                return result;
            }
            result = EditorHelper.ValidateCodeExist(this.HostAdapter, this.txtCode.Text, out validateMsg);
            return result;
        }
    }
}
