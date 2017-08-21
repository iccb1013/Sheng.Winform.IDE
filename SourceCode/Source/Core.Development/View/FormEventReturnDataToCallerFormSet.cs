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
using Sheng.SailingEase.Core.Development.Localisation;
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Core.Development
{
    partial class FormEventReturnDataToCallerFormSet : FormViewBase
    {
        private WindowEntity formEntity;
        public string FormElementCode
        {
            get
            {
                return this.txtFormElementCode.Text;
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
        public FormEventReturnDataToCallerFormSet(WindowEntity formEntity)
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            this.formEntity = formEntity;
            this.ddlFormElement.AllowDataSource = EnumEventDataSource.FormElement;
            this.ddlFormElement.AllowFormElementControlType.Add(typeof(UIElementTextBoxEntity));
            this.ddlFormElement.AllowFormElementControlType.Add(typeof(UIElementComboBoxEntity));
            this.ddlFormElement.AllowFormElementControlType.Add(typeof(UIElementDataListTextBoxColumnEntity));
            this.ddlFormElement.FormEntity = this.formEntity;
        }
        private void ApplyLanguageResource()
        {
            this.txtFormElementCode.Regex = CoreConstant.ENTITY_CODE_REGEX;
            this.txtFormElementCode.RegexMsg = CommonLanguage.Current.RegexMsg_EntityCode;
            this.txtFormElementCode.Title = Language.Current.FormEventReturnDataToCallerFormSet_LabelFormElementCode;
        }
        private void FormEventReturnDataToCallerFormSet_Load(object sender, EventArgs e)
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
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FormGlobalFormElementChoose formGlobalFormElementChoose = new FormGlobalFormElementChoose();
            formGlobalFormElementChoose.AllowFormElementControlType = ReturnDataToCallerFormDev.AllowTargetFormElementControlType;
            formGlobalFormElementChoose.AllowSelectFormElementControlType = ReturnDataToCallerFormDev.AllowTargetFormElementControlType;
            formGlobalFormElementChoose.InitFormElementTree();
            if (formGlobalFormElementChoose.ShowDialog() == DialogResult.OK)
            {
                this.txtFormElementCode.Text = formGlobalFormElementChoose.FormElementCode;
            }
        }
    }
}
