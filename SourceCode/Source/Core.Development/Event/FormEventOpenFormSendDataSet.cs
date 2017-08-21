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
using Sheng.SailingEase.Core.Development.Localisation;
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Core.Development
{
    partial class FormEventOpenFormSendDataSet : FormViewBase
    {
        public DataTable SendData
        {
            get;
            set;
        }
        private WindowEntity formEntity;
        public string DataName
        {
            get
            {
                return this.txtDataName.Text;
            }
        }
        public string DataCode
        {
            get
            {
                return this.txtDataCode.Text;
            }
        }
        public string SelectedDataSourceVisibleString
        {
            get
            {
                return this.ddlFormElement.SelectedDataSourceVisibleString;
            }
        }
        public string SelectedDataSourceString
        {
            get
            {
                return this.ddlFormElement.SelectedDataSourceString;
            }
        }
        public FormEventOpenFormSendDataSet(WindowEntity formEntity)
        {
            InitializeComponent();
            this.txtDataCode.Regex = CoreConstant.ENTITY_CODE_REGEX;
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            this.formEntity = formEntity;
            this.ddlFormElement.AllowDataSource = EnumEventDataSource.FormElement;
            this.ddlFormElement.AllowFormElementControlType = OpenWindowDev.AllowFormElementControlType;
            this.ddlFormElement.FormEntity = this.formEntity;
        }
        private void ApplyLanguageResource()
        {
            this.txtDataCode.RegexMsg = CommonLanguage.Current.RegexMsg_EntityCode;
            this.txtDataName.Title = Language.Current.FormEventOpenFormSendDataSet_LabelDataName ;
            this.txtDataCode.Title = Language.Current.FormEventOpenFormSendDataSet_LabelDataCode;
            this.ddlFormElement.Title = Language.Current.FormEventOpenFormSendDataSet_ComboBoxFormElement;
        }
        private void FormEventOpenFormSendDataSet_Load(object sender, EventArgs e)
        {
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!this.DoValidate())
            {
                return;
            }
            if (this.SendData != null)
            {
                if (this.SendData.Select("DataCode = '" + this.txtDataCode.Text + "'").Length > 0)
                {
                    MessageBox.Show("指定的数据代码已被占用",
                          CommonLanguage.Current.MessageCaption_Notice,
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
