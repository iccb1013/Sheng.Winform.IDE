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
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Components.WindowComponent.Localisation;
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Components.WindowComponent.View
{
    partial class WindowEditView : FormViewBase
    {
        string folderId = String.Empty;
        WindowEntity formEntity;
        public WindowEntity FormEntity
        {
            get
            {
                return this.formEntity;
            }
            private set
            {
                this.formEntity = value;
            }
        }
        public WindowEditView()
            : this(null)
        {
        }
        public WindowEditView(string folderId)
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            this.txtCode.Regex = CoreConstant.ENTITY_CODE_REGEX;
            this.folderId = folderId;
        }
        private void ApplyLanguageResource()
        {
            this.txtName.Title = Language.Current.WindowEditView_LabelName;
            this.txtCode.Title = Language.Current.WindowEditView_LabelCode;
            this.txtCode.RegexMsg = CommonLanguage.Current.RegexMsg_EntityCode;
        }
        private void WindowEditView_Load(object sender, EventArgs e)
        {
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!this.DoValidate())
            {
                return;
            }
            if (WindowArchive.Instance.CheckExistByCode(txtCode.Text))
            {
                MessageBox.Show(Language.Current.WindowEditView_MessageFormEntityCodeExist,
                    CommonLanguage.Current.MessageCaption_Notice,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            this.FormEntity = ServiceUnity.WindowElementContainer.CreateWindowEntity();
            this.FormEntity.Name = txtName.Text;
            this.FormEntity.Code = txtCode.Text;
            this.FormEntity.Text = txtName.Text;
            this.FormEntity.FolderId = this.folderId;
            WindowArchive.Instance.Add(this.FormEntity);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
