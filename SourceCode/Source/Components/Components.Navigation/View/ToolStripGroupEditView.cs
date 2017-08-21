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
using Sheng.SailingEase.Components.NavigationComponent.Localisation;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Core.Development;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Components.NavigationComponent.View
{
    partial class ToolStripGroupEditView : FormViewBase
    {
        private string _pageId;
        private ToolStripGroupEntity _toolStripGroup;
        public ToolStripGroupEntity ToolStripGroup
        {
            get
            {
                return this._toolStripGroup;
            }
            set
            {
                this._toolStripGroup = value;
            }
        }
        public ToolStripGroupEditView(string pageId)
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            this.txtCode.Regex = CoreConstant.ENTITY_CODE_REGEX;
            this.txtCode.CustomValidate = CodeValidateMethod;
            _pageId = pageId;
        }
        private void ToolStripGroupEditView_Load(object sender, EventArgs e)
        {
            if (_toolStripGroup != null)
            {
                this.txtCode.Text = this.ToolStripGroup.Code;
                this.txtName.Text = this.ToolStripGroup.Name;
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
            if (this._toolStripGroup != null)
                code = this._toolStripGroup.Code;
            if (Keywords.Container(code))
            {
                msg = Language.Current.ToolStripGroupEditView_MessageUseKeywords;
                return false;
            }
            if (code != textBox.Text)
            {
                if (ToolStripArchive.Instance.GroupExistByCode(textBox.Text))
                {
                    msg = Language.Current.ToolStripGroupEditView_MessageElementCodeExist;
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
            if (this.ToolStripGroup == null)
            {
                this.ToolStripGroup = new ToolStripGroupEntity();
            }
            this.ToolStripGroup.Name = txtName.Text;
            this.ToolStripGroup.Code = txtCode.Text;
            this.ToolStripGroup.PageId = _pageId;
            ToolStripArchive.Instance.Commit(this.ToolStripGroup);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
