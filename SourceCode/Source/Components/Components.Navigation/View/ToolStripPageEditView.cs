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
using Sheng.SailingEase.Components.NavigationComponent.Localisation;
using Sheng.SailingEase.Localisation;
using Sheng.SailingEase.Core.Development;
namespace Sheng.SailingEase.Components.NavigationComponent.View
{
    partial class ToolStripPageEditView : FormViewBase
    {
        private ToolStripPageEntity _toolStripPage;
        public ToolStripPageEntity ToolStripPage
        {
            get
            {
                return this._toolStripPage;
            }
            set
            {
                this._toolStripPage = value;
            }
        }
        public ToolStripPageEditView()
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            this.txtCode.Regex = CoreConstant.ENTITY_CODE_REGEX;
            this.txtCode.CustomValidate = CodeValidateMethod;
        }
        private void ToolStripPageEditView_Load(object sender, EventArgs e)
        {
            if (_toolStripPage != null)
            {
                this.txtCode.Text = this.ToolStripPage.Code;
                this.txtName.Text = this.ToolStripPage.Name;
            }
        }
        private void ApplyLanguageResource()
        {
            this.txtName.Title = Language.Current.ToolStripItemEditView_LabelName;
            this.txtCode.Title = Language.Current.ToolStripItemEditView_LabelCode;
            this.txtCode.RegexMsg = CommonLanguage.Current.RegexMsg_EntityCode;
        }
        private bool CodeValidateMethod(object sender, out string msg)
        {
            msg = String.Empty;
            TextBox textBox = (TextBox)sender;
            string code = String.Empty;
            if (this._toolStripPage != null)
                code = this._toolStripPage.Code;
            if (Keywords.Container(code))
            {
                msg = Language.Current.ToolStripPageEditView_MessageUseKeywords;
                return false;
            }
            if (code != textBox.Text)
            {
                if (ToolStripArchive.Instance.PageExistByCode(textBox.Text))
                {
                    msg = Language.Current.ToolStripPageEditView_MessageElementCodeExist;
                    return false;
                }
            }
            return true;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.DoValidate() == false)
            {
                return;
            }
            if (this.ToolStripPage == null)
            {
                this.ToolStripPage = new ToolStripPageEntity();
            }
            this.ToolStripPage.Name = txtName.Text;
            this.ToolStripPage.Code = txtCode.Text;
            ToolStripArchive.Instance.Commit(this.ToolStripPage);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
