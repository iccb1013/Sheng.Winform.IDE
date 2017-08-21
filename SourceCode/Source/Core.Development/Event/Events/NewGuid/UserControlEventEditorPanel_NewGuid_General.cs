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
using Sheng.SailingEase.Core;
namespace Sheng.SailingEase.Core.Development
{
    [ToolboxItem(false)]
    partial class UserControlEventEditorPanel_NewGuid_General : UserControlEventEditorPanelBase
    {
        public UserControlEventEditorPanel_NewGuid_General(EventEditorAdapterAbstract hostAdapter)
            : base(hostAdapter)
        {
            InitializeComponent();
            this.txtCode.Regex = CoreConstant.ENTITY_CODE_REGEX;
            ddlFormElement.AllowDataSource = EnumEventDataSource.FormElement;
            ddlFormElement.AllowFormElementControlType = NewGuidDev.AllowTargetFormElementControlType;
            ddlFormElement.FormEntity = this.HostAdapter.HostFormEntity;
            Unity.ApplyResource(this);
            ApplyLanguageResource();
        }
        private void ApplyLanguageResource()
        {
            this.txtName.Title = Language.Current.UserControlEventEditorPanel_NewGuid_General_LabelName;
            this.txtCode.Title = Language.Current.UserControlEventEditorPanel_NewGuid_General_LabelCode;
            this.txtCode.RegexMsg = CommonLanguage.Current.RegexMsg_EntityCode;
            this.ddlFormElement.Title = Language.Current.UserControlEventEditorPanel_NewGuid_General_LabelFormElement;
        }
        public override string PanelTitle
        {
            get
            {
                return Language.Current.NewGuidDev_EditorPanel_General;
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
            _xObject.Add(new XElement("SetFormElement", ddlFormElement.SelectedFormElementId));
            return _xObject;
        }
        public override void SetParameter(EventBase even)
        {
            NewGuidDev _event = even as NewGuidDev;
            this.txtName.Text = _event.Name;
            this.txtCode.Text = _event.Code;
            this.ddlFormElement.SelectedFormElementId = _event.SetFormElementId;
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
