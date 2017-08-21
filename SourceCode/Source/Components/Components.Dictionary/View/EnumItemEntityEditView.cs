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
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Components.DictionaryComponent.Localisation;
using Sheng.SailingEase.Core;
namespace Sheng.SailingEase.Components.DictionaryComponent.View
{
    partial class EnumItemEntityEditView : FormViewBase
    {
        private EnumEntity _enumEntity;
        private EnumItemEntityDev _enumItemEntity;
        public EnumItemEntityDev EnumItemEntity
        {
            get
            {
                return this._enumItemEntity;
            }
            set
            {
                this._enumItemEntity = value;
            }
        }
        public EnumItemEntityEditView(EnumEntity enumEntity)
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            _enumEntity = enumEntity;
            this.txtValue.CustomValidate = ValueValidateMethod;
        }
        private void EnumItemEditView_Load(object sender, EventArgs e)
        {
            if (this.EnumItemEntity != null)
            {
                this.txtText.Text = this.EnumItemEntity.Text;
                this.txtValue.Text = this.EnumItemEntity.Value;
                if (txtText.Text == txtValue.Text)
                {
                    toolStripButtonEquals.Checked = true;
                }
                else
                {
                    toolStripButtonEquals.Checked = false;
                }
                txtValue.ReadOnly = toolStripButtonEquals.Checked;
            }
        }
        private void ApplyLanguageResource()
        {
            this.txtText.Title = Language.Current.EnumItemEditView_LabelText;
            this.txtValue.Title = Language.Current.EnumItemEditView_LabelValue;
        }
        private bool ValueValidateMethod(object sender, out string msg)
        {
            msg = String.Empty;
            TextBox textBox = (TextBox)sender;
            string code = String.Empty;
            if (this.EnumItemEntity != null)
                code = this.EnumItemEntity.Value;
            if (code != textBox.Text)
            {
                if (DictionaryArchive.Instance.CheckEnumItemExist(_enumEntity.Id,textBox.Text))
                {
                    msg = Language.Current.EnumItemEditView_MessageEnumItemValueExist;
                    return false;
                }
            }
            return true;
        }
        private void txtText_TextChanged(object sender, EventArgs e)
        {
            if (toolStripButtonEquals.Checked)
            {
                txtValue.Text = txtText.Text;
            }
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.DoValidate() == false)
                return;
            if (this.EnumItemEntity == null)
            {
                this.EnumItemEntity = new EnumItemEntityDev(_enumEntity);
            }
            this.EnumItemEntity.Text = this.txtText.Text;
            this.EnumItemEntity.Value = this.txtValue.Text;
            DictionaryArchive.Instance.Commit(this.EnumItemEntity);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void toolStripButtonEquals_Click(object sender, EventArgs e)
        {
            toolStripButtonEquals.Checked = !toolStripButtonEquals.Checked;
            txtValue.ReadOnly = toolStripButtonEquals.Checked;
            if (toolStripButtonEquals.Checked)
            {
                txtValue.Text = txtText.Text;
            }
        }
    }
}
