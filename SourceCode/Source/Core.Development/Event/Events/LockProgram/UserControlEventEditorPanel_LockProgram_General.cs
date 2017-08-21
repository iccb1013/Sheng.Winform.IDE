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
using Sheng.SailingEase.Core.Development.Localisation;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Core.Development
{
    [ToolboxItem(false)]
    partial class UserControlEventEditorPanel_LockProgram_General : UserControlEventEditorPanelBase
    {
        public UserControlEventEditorPanel_LockProgram_General(EventEditorAdapterAbstract hostAdapter)
            : base(hostAdapter)
        {
            InitializeComponent();
            this.txtCode.Regex = CoreConstant.ENTITY_CODE_REGEX;
            Unity.ApplyResource(this);
            ApplyLanguageResource();
        }
        private void ApplyLanguageResource()
        {
            this.txtName.Title = Language.Current.UserControlEventEditorPanel_LockProgram_General_LabelName;
            this.txtCode.Title = Language.Current.UserControlEventEditorPanel_LockProgram_General_LabelCode;
            this.txtCode.RegexMsg = CommonLanguage.Current.RegexMsg_EntityCode;
        }
        public override string PanelTitle
        {
            get
            {
                return Language.Current.LockProgramDev_EditorPanel_General;
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
            return _xObject;
        }
        public override void SetParameter(EventBase even)
        {
            LockProgramDev _event = even as LockProgramDev;
            this.txtName.Text = _event.Name;
            this.txtCode.Text = _event.Code;
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
