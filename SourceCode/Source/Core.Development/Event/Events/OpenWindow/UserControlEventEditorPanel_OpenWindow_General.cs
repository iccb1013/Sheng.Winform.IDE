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
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Core.Development.Localisation;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Core.Development
{
    [ToolboxItem(false)]
    partial class UserControlEventEditorPanel_OpenWindow_General : UserControlEventEditorPanelBase
    {
        private IWindowComponentService _windowComponentService;
        private WindowEntity _selectedFormEntity;
        public UserControlEventEditorPanel_OpenWindow_General(EventEditorAdapterAbstract hostAdapter)
            : base(hostAdapter)
        {
            InitializeComponent();
            _windowComponentService = ServiceUnity.Container.Resolve<IWindowComponentService>();
            this.txtCode.Regex = CoreConstant.ENTITY_CODE_REGEX;
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            ddlIfOpend.DataSource = EnumDescConverter.Get(typeof(OpenWindowEvent.EnumOpenWindowIfOpend));
        }
        private void ApplyLanguageResource()
        {
            this.txtName.Title = Language.Current.UserControlEventEditorPanel_OpenForm_General_LabelName;
            this.txtCode.Title = Language.Current.UserControlEventEditorPanel_OpenForm_General_LabelCode;
            this.txtCode.RegexMsg = CommonLanguage.Current.RegexMsg_EntityCode;
            this.txtFormName.Title = Language.Current.UserControlEventEditorPanel_OpenForm_General_LabelFormName;
        }
        private void btnBrowseForm_Click(object sender, EventArgs e)
        {
            ChooseForm();
        }
        private void ChooseForm()
        {
            using (FormFormChoose formFormChoose = new FormFormChoose())
            {
                if (formFormChoose.ShowDialog() == DialogResult.OK)
                {
                    _selectedFormEntity = formFormChoose.SelectedForm;
                }
            }
            if (_selectedFormEntity != null)
            {
                txtFormName.Text = _selectedFormEntity.Name;
            }
            else
            {
                txtFormName.Text = String.Empty;
            }
        }
        public override string PanelTitle
        {
            get
            {
                return Language.Current.OpenFormDev_EditorPanel_General;
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
            _xObject.Add(new XElement("WindowId", _selectedFormEntity.Id));
            _xObject.Add(new XElement("IfOpend", this.ddlIfOpend.SelectedValue.ToString()));
            _xObject.Add(new XElement("IsDialog", this.cbIsDiablog.Checked));
            return _xObject;
        }
        public override void SetParameter(EventBase even)
        {
            OpenWindowDev _event = even as OpenWindowDev;
            this.txtName.Text = _event.Name;
            this.txtCode.Text = _event.Code;
            _selectedFormEntity = _windowComponentService.GetWindowEntity(_event.WindowId);
            if (_selectedFormEntity != null)
            {
                this.txtFormName.Text = this._selectedFormEntity.Name;
            }
            this.cbIsDiablog.Checked = _event.IsDialog;
            this.ddlIfOpend.SelectedValue = ((int)_event.IfOpend).ToString();
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
