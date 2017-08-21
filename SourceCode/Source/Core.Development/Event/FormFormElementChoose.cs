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
namespace Sheng.SailingEase.Core.Development
{
    partial class FormFormElementChoose : FormViewBase
    {
        private WindowEntity formEntity;
        public string FormElementId
        {
            get
            {
                return this.ddlFormElement.SelectedFormElementId;
            }
        }
        public string FormElementName
        {
            get
            {
                return this.ddlFormElement.SelectedFormElementName;
            }
        }
        public UIElement SelectedFormElement
        {
            get
            {
                return ddlFormElement.SelectedItem as UIElement;
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
        public FormFormElementChoose(WindowEntity formEntity)
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            this.formEntity = formEntity;
        }
        private void ApplyLanguageResource()
        {
            this.ddlFormElement.Title = Language.Current.FormFormElementChoose_LabelFormElement;
        }
        private void FormFormElementChoose_Load(object sender, EventArgs e)
        {
            this.ddlFormElement.FormEntity = this.formEntity;
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
