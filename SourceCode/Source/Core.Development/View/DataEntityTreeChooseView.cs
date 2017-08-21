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
using Sheng.SailingEase.Localisation;
using Sheng.SailingEase.Core.Development.Localisation;
namespace Sheng.SailingEase.Core.Development
{
    partial class DataEntityTreeChooseView : FormViewBase
    {
        private IDataEntityComponentService _dataEntityComponentService;
        private ImageList _imageList;
        private IDataEntityTreeNode _dataEntityTreeNode;
        private bool _showDataItem = true;
        public bool ShowDataItem
        {
            get
            {
                return this._showDataItem;
            }
            set
            {
                this._showDataItem = value;
                LoadTree();
            }
        }
        public string SelectedId
        {
            get
            {
                if (_dataEntityTreeNode == null)
                {
                    return String.Empty;
                }
                return this._dataEntityTreeNode.IdPath;
            }
            set
            {
                if (value == String.Empty)
                {
                    return;
                }
                string[] ids = value.Split('.');
                foreach (IDataEntityTreeNode entityNode in this.treeViewDataEntity.Nodes)
                {
                    if (entityNode.Id == ids[0])
                    {
                        this.treeViewDataEntity.SelectedNode = entityNode.Node;
                        entityNode.Node.Expand();
                        if (ids.Length == 2)
                        {
                            foreach (IDataEntityTreeNode itemNode in entityNode.Node.Nodes)
                            {
                                if (itemNode.Id == ids[1])
                                {
                                    this.treeViewDataEntity.SelectedNode = itemNode.Node;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
                this._dataEntityTreeNode = this.treeViewDataEntity.SelectedNode as DataEntityTreeNode;
                treeViewDataEntity_AfterSelect(null, null);
            }
        }
        public string SelectedName
        {
            get
            {
                if (this._dataEntityTreeNode == null)
                {
                    return String.Empty;
                }
                return this._dataEntityTreeNode.FullName;
            }
            set
            {
                this.txtSelectedName.Text = value;
            }
        }
        public DataEntity SelectedDataEntity
        {
            get
            {
                DataEntityTreeNode selectedTreeNode = _dataEntityTreeNode as DataEntityTreeNode;
                if (selectedTreeNode == null)
                    return null;
                return selectedTreeNode.Entity;
            }
        }
        public DataItemEntity SelectedDataItemEntity
        {
            get
            {
                DataEntityItemTreeNode selectedTreeNode = _dataEntityTreeNode as DataEntityItemTreeNode;
                if (selectedTreeNode == null)
                    return null;
                return selectedTreeNode.Entity;
            }
        }
        public DataEntityTreeChooseView()
            : this(true)
        {
        }
        public DataEntityTreeChooseView(bool showDataItem)
        {
            InitializeComponent();
            _dataEntityComponentService = ServiceUnity.Container.Resolve<IDataEntityComponentService>();
            Unity.ApplyResource(this);
            this.ShowDataItem = showDataItem;
            _imageList = new ImageList();
            _imageList.Images.Add(IconsLibrary.DataEntity3);  
            _imageList.Images.Add(IconsLibrary.Cube2);  
            this.treeViewDataEntity.ImageList = this._imageList;
            this.ShowDataItem = showDataItem;
        }
        private void FormDataEntityTreeChoose_Load(object sender, EventArgs e)
        {
        }
        private void treeViewDataEntity_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this._dataEntityTreeNode = this.treeViewDataEntity.SelectedNode as IDataEntityTreeNode;
            if (this._dataEntityTreeNode != null)
                this.txtSelectedName.Text = this._dataEntityTreeNode.FullName;
            else
                this.txtSelectedName.Text = String.Empty;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            this._dataEntityTreeNode = this.treeViewDataEntity.SelectedNode as IDataEntityTreeNode;
            if (this._dataEntityTreeNode == null)
            {
                MessageBox.Show(Language.Current.DataEntityTreeChooseView_NoItemSelected,
                        CommonLanguage.Current.MessageCaption_Notice,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if ( this.ShowDataItem && this._dataEntityTreeNode.IsDataEntity)
            {
                MessageBox.Show(Language.Current.DataEntityTreeChooseView_NoDataItemSelected,
                        CommonLanguage.Current.MessageCaption_Notice,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            this._dataEntityTreeNode = null;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void LoadTree()
        {
            this.treeViewDataEntity.Nodes.Clear();
            List<DataEntity> _dataEntitys = _dataEntityComponentService.GetDataEntityList();
            foreach (var entity in _dataEntitys)
            {
                this.treeViewDataEntity.Nodes.Add(new DataEntityTreeNode(entity, _showDataItem));
            }
        }
    }
    public class DataEntityTreeChooseResult
    {
        private bool _dialogResult = false;
        public bool DialogResult
        {
            get { return _dialogResult; }
            internal set { _dialogResult = value; }
        }
        public string SelectedId
        {
            get;
            internal set;
        }
        public string SelectedName
        {
            get;
            internal set;
        }
        public DataEntity SelectedDataEntity
        {
            get;
            internal set;
        }
        public DataItemEntity SelectedDataItemEntity
        {
            get;
            internal set;
        }
        public DataEntityTreeChooseResult()
        {
        }
    }
    public class DataEntityTreeChooseArgs
    {
        private bool _showDataItem = true;
        public bool ShowDataItem
        {
            get { return _showDataItem; }
            set { _showDataItem = value; }
        }
        public string SelectedId
        {
            get;
            set;
        }
        public string SelectedName
        {
            get;
            set;
        }
        public DataEntityTreeChooseArgs()
        {
        }
    }
}
