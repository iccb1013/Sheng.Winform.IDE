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
using Sheng.SailingEase.Components.DataEntityComponent.Localisation;
namespace Sheng.SailingEase.Components.DataEntityComponent.View
{
    partial class DataEntityEditView : FormViewBase
    {
        private DataEntityDev _dataEntity;
        public DataEntityDev DataEntity
        {
            get
            {
                return this._dataEntity;
            }
            set
            {
                this._dataEntity = value;
            }
        }
        public DataEntityEditView()
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            this.txtCode.Regex = CoreConstant.ENTITY_CODE_REGEX;
            this.txtCode.CustomValidate = CodeValidateMethod;
        }
        private void FormDataEntityAdd_Load(object sender, EventArgs e)
        {
            if (this.DataEntity != null)
            {
                this.txtName.Text = this.DataEntity.Name;
                this.txtCode.Text = this.DataEntity.Code;
                this.txtRemark.Text = this.DataEntity.Remark;
            }
        }
        private void ApplyLanguageResource()
        {
            this.tabPageGeneral.Text = Language.Current.DataEntityEditView_TabPageGeneral;
            this.tabPageRemark.Text = Language.Current.DataEntityEditView_TabPageRemark;
            this.txtName.Title = Language.Current.DataEntityEditView_LabelDataEntityName;
            this.txtCode.Title = Language.Current.DataEntityEditView_LabelCode;
            this.txtCode.RegexMsg = CommonLanguage.Current.RegexMsg_EntityCode;
        }
        private bool CodeValidateMethod(object sender, out string msg)
        {
            msg = String.Empty;
            TextBox textBox = (TextBox)sender;
            string code = String.Empty;
            if (this.DataEntity != null)
                code = this.DataEntity.Code;
            if (code != textBox.Text)
            {
                if (DataEntityArchive.Instance.EntityExist(textBox.Text))
                {
                    msg = Language.Current.DataEntityEditView_MessageDataEntityCodeExist;
                    return false;
                }
            }
            return true;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.DoValidate() == false)
                return;
            if (this.DataEntity == null)
            {
                this.DataEntity = new DataEntityDev();
            }
            this.DataEntity.Name = txtName.Text;
            this.DataEntity.Code = txtCode.Text;
            this.DataEntity.Remark = txtRemark.Text;
            DataEntityArchive.Instance.Commit(_dataEntity);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
