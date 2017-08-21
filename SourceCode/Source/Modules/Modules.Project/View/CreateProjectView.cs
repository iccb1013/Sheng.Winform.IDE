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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Localisation;
using Sheng.SailingEase.Modules.ProjectModule.Localisation;
namespace Sheng.SailingEase.Modules.ProjectModule.View
{
    partial class CreateProjectView : FormViewBase
    {
        IUnityContainer _container;
        ImageList imageListTemplate = new ImageList();
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        public CreateProjectView()
        {
            InitializeComponent();
            _container = ServiceUnity.Container;
            Unity.ApplyResource(this);
        }
        private void CreateProjectView_Load(object sender, EventArgs e)
        {
            folderBrowserDialog.Description = Language.Current.CreateProjectView_FolderBrowserDialog_Description;
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;
            folderBrowserDialog.ShowNewFolderButton = true;
            imageListTemplate.ImageSize = new Size(32, 32);
            this.listViewTemplate.LargeImageList = this.imageListTemplate;
            TreeNode tnRoot = new TreeNode(Language.Current.CreateProjectView_ProjectTypeTreeNode_All);
            this.treeViewProjectType.Nodes.Add(tnRoot);
            this.treeViewProjectType.SelectedNode = tnRoot;
        }
        private void treeViewProjectType_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Text = Language.Current.CreateProjectView_ProjectType_Empty;
            listViewItem.Tag = Language.Current.CreateProjectView_ProjectType_Empty_Description;
            listViewItem.ImageIndex = 0;
            this.listViewTemplate.Items.Add(listViewItem);
            if (this.listViewTemplate.Items.Count > 0)
            {
                this.listViewTemplate.Items[0].Selected = true;
            }
        }
        private void listViewTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listViewTemplate.SelectedItems.Count > 0)
            {
                this.txtDescription.Text = this.listViewTemplate.SelectedItems[0].Tag.ToString();
                this.btnBrowse.Enabled = true;
                this.btnOK.Enabled = true;
                this.txtProjectFileName.Enabled = true;
                this.txtFolder.Enabled = true;
            }
            else
            {
                this.txtDescription.Text = String.Empty;
                this.btnBrowse.Enabled = false;
                this.btnOK.Enabled = false;
                this.txtProjectFileName.Enabled = false;
                this.txtFolder.Enabled = false;
            }
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.DoValidate() == false)
            {
                return;
            }
            DirectoryInfo dirTarget = new DirectoryInfo(txtFolder.Text);
            if (!dirTarget.Exists)
            {
                if (MessageBox.Show(Language.Current.CreateProjectView_MessageFolderNotExist,
                    CommonLanguage.Current.MessageCaption_Notice, MessageBoxButtons.OKCancel, MessageBoxIcon.Information
                    ) == DialogResult.Cancel)
                {
                    return;
                }
                try
                {
                    dirTarget.Create();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, CommonLanguage.Current.MessageCaption_Warning, 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            ProjectArchive projectArchive = _container.Resolve<ProjectArchive>();
            projectArchive.NewProject(txtFolder.Text, txtProjectFileName.Text);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            txtFolder.Text = folderBrowserDialog.SelectedPath;
        }
        public static DialogResult ShowView()
        {
            DialogResult result;
            using (CreateProjectView view = new CreateProjectView())
            {
                result = view.ShowDialog();
            }
            return result;
        }
    }
}
