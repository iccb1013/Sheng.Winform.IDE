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
    partial class FormEventReceiveDataSet : FormViewBase
    {
        private WindowEntity formEntity;
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
        public FormEventReceiveDataSet(WindowEntity formEntity)
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            this.formEntity = formEntity;
            this.ddlFormElement.AllowDataSource = EnumEventDataSource.FormElement;
            this.ddlFormElement.AllowFormElementControlType = ReceiveDataDev.AllowTargetFormElementControlType;
            this.ddlFormElement.FormEntity = this.formEntity;
        }
        private void ApplyLanguageResource()
        {
            this.txtDataCode.Regex = CoreConstant.ENTITY_CODE_REGEX;
            this.txtDataCode.RegexMsg = CommonLanguage.Current.RegexMsg_EntityCode;
            this.txtDataCode.Title = Language.Current.FormEventReceiveDataSet_LabelDataCode;
            this.ddlFormElement.Title = Language.Current.FormEventReceiveDataSet_LabelReceiveTo;
        }
        private void FormEventReceiveDataSet_Load(object sender, EventArgs e)
        {
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!this.DoValidate())
            {
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
