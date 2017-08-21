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
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Components.WindowComponent.Localisation;
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Components.WindowComponent.View
{
    partial class FolderEditView : FormViewBase
    {
        private string _parentId = String.Empty;
        private WindowFolderEntity _formFolderEntity;
        public WindowFolderEntity FormFolderEntity
        {
            get
            {
                return this._formFolderEntity;
            }
            set
            {
                this._formFolderEntity = value;
            }
        }
        public FolderEditView()
            : this(null)
        {
        }
        public FolderEditView(string parentId)
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            _parentId = parentId;
        }
        private void ApplyLanguageResource()
        {
            this.txtName.Title = Language.Current.FolderEditView_LabelName;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.DoValidate() == false)
            {
                return;
            }
            if (this.FormFolderEntity == null)
            {
                this.FormFolderEntity = new WindowFolderEntity();
                this.FormFolderEntity.Parent = _parentId;
            }
            this.FormFolderEntity.Name = txtName.Text;
            WindowArchive.Instance.Commit(this.FormFolderEntity);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void FolderEditView_Load(object sender, EventArgs e)
        {
            if (this.FormFolderEntity != null)
            {
                this.txtName.Text = this.FormFolderEntity.Name;
            }           
        }
    }
}
