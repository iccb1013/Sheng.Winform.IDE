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
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Localisation;
using Sheng.SailingEase.Modules.ProjectModule.Localisation;
using Sheng.SailingEase.Core;
namespace Sheng.SailingEase.Modules.ProjectModule.View
{
    partial class ProjectPropertyView : FormViewBase
    {
        IUnityContainer _container;
        IProjectService _projectService;
        Project _project;
        public ProjectPropertyView()
        {
            InitializeComponent();
            _container = ServiceUnity.Container;
            _projectService = _container.Resolve<IProjectService>();
            _project = (Project)_projectService.Current;
            Unity.ApplyResource(this);
            ApplyLanguageResource();
        }
        private void ProjectPropertyView_Load(object sender, EventArgs e)
        {
            ddlDataBase.DataSource = EnumDescConverter.Get(typeof(EnumDataBase));
            ddlDataBase.DisplayMember = "Text";
            ddlDataBase.ValueMember = "Value";
            txtPrjName.Text = _project.ProjectEntity.Name;
            txtPrjCode.Text = _project.ProjectEntity.Code;
            ddlDataBase.SelectedValue = (int)_project.ProjectEntity.DataBase;
            cbUserModel.Checked = _project.ProjectEntity.UserModel;
            cbUserPopedomModel.Checked = _project.ProjectEntity.UserPopedomModel;
            cbUserSubsequent.Checked = _project.ProjectEntity.UserSubsequent;
            txtCompany.Text = _project.ProjectEntity.Company;
            txtVersion.Text = _project.ProjectEntity.Version;
            txtSummary.Text = _project.ProjectEntity.Summary;
            txtCopyright.Text = _project.ProjectEntity.Copyright;
            txtSplashImg.Text = _project.ProjectEntity.SplashImg;
            txtBackImg.Text = _project.ProjectEntity.BackImg;
            txtAboutImg.Text = _project.ProjectEntity.AboutImg;
            txtRemark.Text = _project.ProjectEntity.Remark;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.DoValidate() == false)
            {
                return;
            }
            _project.ProjectEntity.Name = txtPrjName.Text;
            _project.ProjectEntity.Code = txtPrjCode.Text;
            _project.ProjectEntity.DataBase = (EnumDataBase)Convert.ToInt32(ddlDataBase.SelectedValue);
            _project.ProjectEntity.UserModel = cbUserModel.Checked;
            _project.ProjectEntity.UserPopedomModel = cbUserPopedomModel.Checked;
            _project.ProjectEntity.UserSubsequent = cbUserSubsequent.Checked;
            _project.ProjectEntity.Company = txtCompany.Text;
            _project.ProjectEntity.Version = txtVersion.Text;
            _project.ProjectEntity.Summary = txtSummary.Text;
            _project.ProjectEntity.Copyright = txtCopyright.Text;
            _project.ProjectEntity.SplashImg = txtSplashImg.Text;
            _project.ProjectEntity.BackImg = txtBackImg.Text;
            _project.ProjectEntity.AboutImg = txtAboutImg.Text;
            _project.ProjectEntity.Remark = txtRemark.Text;
            ProjectArchive.Instance.Save(_project.ProjectEntity);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void btnBrowseSplashImg_Click(object sender, EventArgs e)
        {
            if (openFileDialogImg.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            FileInfo fi = new FileInfo(openFileDialogImg.FileName);
            SelectImg(fi);
            txtSplashImg.Text = "Resources\\" + fi.Name;
        }
        private void btnBrowseBackImg_Click(object sender, EventArgs e)
        {
            if (openFileDialogImg.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            FileInfo fi = new FileInfo(openFileDialogImg.FileName);
            SelectImg(fi);
            txtBackImg.Text = "Resources\\" + fi.Name;
        }
        private void btnBrowseAboutImg_Click(object sender, EventArgs e)
        {
            if (openFileDialogImg.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            FileInfo fi = new FileInfo(openFileDialogImg.FileName);
            SelectImg(fi);
            txtAboutImg.Text = "Resources\\" + fi.Name;
        }
        private void ApplyLanguageResource()
        {
            this.tabPageGeneral.Text = Language.Current.ProjectPropertyView_TabPageGeneral;
            this.tabPageAbout.Text = Language.Current.ProjectPropertyView_TabPageAbout;
            this.tabPageRemark.Text = Language.Current.ProjectPropertyView_TabPageRemark;
            this.txtPrjName.Title = Language.Current.ProjectPropertyView_ProjectName;
            this.txtPrjCode.Title = Language.Current.ProjectPropertyView_LabelCode;
            this.txtPrjCode.Regex = CoreConstant.ENTITY_CODE_REGEX;
            this.txtPrjCode.RegexMsg = CommonLanguage.Current.RegexMsg_EntityCode;
        }
        private void SelectImg(FileInfo fi)
        {
        }
        public static void ShowView()
        {
            using (ProjectPropertyView view = new ProjectPropertyView())
            {
                view.ShowDialog();
            }
        }
    }
}
