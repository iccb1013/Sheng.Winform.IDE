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
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Localisation;
using Sheng.SailingEase.Components.DictionaryComponent.Localisation;
namespace Sheng.SailingEase.Components.DictionaryComponent.View
{
    partial class EnumEntityEditView : FormViewBase
    {
        private EnumEntityDev _enumEntity;
        public EnumEntityDev EnumEntity
        {
            get
            {
                return this._enumEntity;
            }
            set
            {
                this._enumEntity = value;
            }
        }
        public EnumEntityEditView()
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            this.txtCode.Regex = CoreConstant.ENTITY_CODE_REGEX;
            this.txtCode.CustomValidate = CodeValidateMethod;
        }
        private void EnumEditView_Load(object sender, EventArgs e)
        {
            if (this.EnumEntity != null)
            {
                this.txtName.Text = _enumEntity.Name;
                this.txtCode.Text = _enumEntity.Code;
            }
        }
        private void ApplyLanguageResource()
        {
            this.txtName.Title = Language.Current.EnumEditView_LabelName;
            this.txtCode.Title = Language.Current.EnumEditView_LabelCode;
            this.txtCode.RegexMsg = CommonLanguage.Current.RegexMsg_EntityCode;
        }
        private bool CodeValidateMethod(object sender, out string msg)
        {
            msg = String.Empty;
            TextBox textBox = (TextBox)sender;
            string code = String.Empty;
            if (this.EnumEntity != null)
                code = this.EnumEntity.Code;
            if (code != textBox.Text)
            {
                if (DictionaryArchive.Instance.CheckExistByCode(textBox.Text))
                {
                    msg = Language.Current.EnumEditView_MessageEnumCodeExist;
                    return false;
                }
            }
            return true;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.DoValidate() == false)
                return;
            if (this.EnumEntity == null)
            {
                this.EnumEntity = new EnumEntityDev();
            }
            this.EnumEntity.Name = this.txtName.Text;
            this.EnumEntity.Code = this.txtCode.Text;
            DictionaryArchive.Instance.Commit(this.EnumEntity);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
