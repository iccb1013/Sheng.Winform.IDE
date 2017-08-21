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
using Sheng.SailingEase.Controls;
using Sheng.SailingEase.Controls.SEAdressBar;
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Core.Development
{
    partial class FormFormChoose : FormViewBase
    {
        private IWindowComponentService _windowComponentService;
        private BindingList<IEntityIndex> _formList;
        private WindowFolderEntity _folder = null;
        private WindowFolderEntity Folder
        {
            get
            {
                return this._folder;
            }
            set
            {
                if (_folder == value)
                    return;
                _folder = value;
                string folderId;
                if (value == null)
                {
                    folderId = String.Empty;
                    folderAddressBar.CurrentNode = null;
                }
                else
                {
                    folderId = value.Id;
                    folderAddressBar.SetAddress(_windowComponentService.GetFolderFullPath(value));
                }
                LoadFormList();
            }
        }
        private string selectedId = string.Empty;
        public string SelectedId
        {
            get
            {
                return this._selectedFormIndex.Id;
            }
        }
        private string selectedName = string.Empty;
        public string SelectedName
        {
            get
            {
                return this._selectedFormIndex.Name;
            }
        }
        private IEntityIndex _selectedFormIndex;
        public WindowEntity SelectedForm
        {
            get
            {
                if (_selectedFormIndex == null || _selectedFormIndex is FolderEntityIndex)
                    return null;
                WindowEntityIndex windowEntityIndex = (WindowEntityIndex)_selectedFormIndex;
                return windowEntityIndex.Window;
            }
        }
        public FormFormChoose()
        {
            InitializeComponent();
            DataGridViewImageBinderColumn iconColumn = new DataGridViewImageBinderColumn()
            {
                Width = 25
            };
            iconColumn.Mapping(typeof(FolderEntityIndex), IconsLibrary.Folder, true);
            iconColumn.Mapping(typeof(WindowEntityIndex), IconsLibrary.Form, true);
            dataGridViewForms.Columns.Insert(0, iconColumn);
            _windowComponentService = ServiceUnity.Container.Resolve<IWindowComponentService>();
            UIHelper.ProcessDataGridView(this.dataGridViewForms);
            this.folderAddressBar.Renderer = ToolStripRenders.ControlToControlLight;
            this.folderAddressBar.DropDownRenderer = ToolStripRenders.Control;
            Unity.ApplyResource(this);
        }
        private void FormFormChoose_Load(object sender, EventArgs e)
        {
            this.dataGridViewForms.AutoGenerateColumns = false;
            this.folderAddressBar.InitializeRoot(new FolderAddressNode());
            this.Folder = null;
            System.Threading.Thread.Sleep(500);
            LoadFormList();
        }
        private void folderAddressBar_SelectionChange(object sender, SEAddressBarStrip.NodeChangedArgs e)
        {
            string folderId = this.Folder == null ? String.Empty : this.Folder.Id;
            if (String.IsNullOrEmpty(e.UniqueID))
            {
                this.Folder = null;
            }
            else if (folderId != e.UniqueID)
            {
                this.Folder = _windowComponentService.GetFolderEntity(e.UniqueID);
            }
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (dataGridViewForms.SelectedRows.Count == 0)
            {
                _selectedFormIndex = null;
            }
            else
            {
                IEntityIndex entityIndex = (IEntityIndex)dataGridViewForms.SelectedRows[0].DataBoundItem;
                _selectedFormIndex = entityIndex;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void dataGridViewForms_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            IEntityIndex entityIndex = (IEntityIndex)dataGridViewForms.SelectedRows[0].DataBoundItem;
            if (entityIndex is FolderEntityIndex)
            {
                FolderEntityIndex folderIndex = (FolderEntityIndex)entityIndex;
                this.Folder = folderIndex.Folder;
            }
        }
        private void ReturnToParent()
        {
            if (this.Folder != null)
            {
                this.Folder = _windowComponentService.GetFolderEntity(this.Folder.Parent);
            }
        }
        private void LoadFormList()
        {
            string folderId = String.Empty;
            if (this.Folder != null)
            {
                folderId = this.Folder.Id;
            }
            this._formList = new BindingList<IEntityIndex>(_windowComponentService.GetIndexList(folderId));
            dataGridViewForms.DataSource = this._formList;
        }
    }
}
