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
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Core.Development.Localisation;
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Core.Development
{
    [ToolboxItem(false)]
    partial class UserControlEventEditorPanel_ClearFormData_General : UserControlEventEditorPanelBase
    {
        public UserControlEventEditorPanel_ClearFormData_General(EventEditorAdapterAbstract hostAdapter)
            : base(hostAdapter)
        {
            InitializeComponent();
            this.txtCode.Regex = CoreConstant.ENTITY_CODE_REGEX;
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            this.ddlMode.DataSource = EnumDescConverter.Get(typeof(ClearFormDataEvent.EnumClearFormDataMode));
        }
        private void ApplyLanguageResource()
        {
            this.txtName.Title = Language.Current.UserControlEventEditorPanel_ClearFormData_General_LabelName;
            this.txtCode.Title = Language.Current.UserControlEventEditorPanel_ClearFormData_General_LabelCode;
            this.txtCode.RegexMsg = CommonLanguage.Current.RegexMsg_EntityCode;
            this.ddlMode.Title = Language.Current.UserControlEventEditorPanel_ClearFormData_General_LabelMode;
        }
        public override string PanelTitle
        {
            get
            {
                return Language.Current.ClearFormDataDev_EditorPanel_General;
            }
        }
        public override List<XObject> GetXml()
        {
            List<XObject> _xObject = new List<XObject>();
            _xObject.Add(new XAttribute("Name", txtName.Text));
            _xObject.Add(new XAttribute("Code", txtCode.Text));
            _xObject.Add(new XElement("Mode", this.ddlMode.SelectedValue.ToString()));
            return _xObject;
        }
        public override void SetParameter(EventBase even)
        {
            ClearFormDataDev _event = even as ClearFormDataDev;
            this.txtName.Text = _event.Name;
            this.txtCode.Text = _event.Code;
            this.ddlMode.SelectedValue = ((int)_event.ClearFormDataMode).ToString();
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
