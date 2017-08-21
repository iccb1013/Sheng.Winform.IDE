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
namespace Sheng.SailingEase.Core.Development
{
    public partial class DataEntityItemDataSourceEditView : FormViewBase
    {
        private WindowEntity _windowEntity;
        public WindowEntity WindowEntity
        {
            get { return _windowEntity; }
            set { _windowEntity = value; }
        }
        private DataEntity _dataEntity;
        public DataEntity DataEntity
        {
            get { return _dataEntity; }
            set
            {
                _dataEntity = value;
            }
        }
        public string DataSource
        {
            get
            {
                return this.dataSourceSelector.DataSource;
            }
        }
        public string DataSourceName
        {
            get
            {
                return this.dataSourceSelector.DataSourceName;
            }
        }
        public DataItemEntity DataItem
        {
            get
            {
                DataItemEntity item = ddlDataItem.SelectedValue as DataItemEntity;
                return item;
            }
        }
        public DataEntityItemDataSourceEditView()
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            ddlDataItem.DisplayMember = EntityBase.Property_Name;
            ddlDataItem.Title = Language.Current.DataEntityItemDataSourceEditView_LabelDataItem;
        }
        private void DataEntityItemDataSourceEditView_Load(object sender, EventArgs e)
        {
            if (_dataEntity != null)
                ddlDataItem.DataSource = _dataEntity.Items;
            dataSourceSelector.WindowEntity = _windowEntity;
        }
        private void dataSourceSelector_DataSourceChanged(object sender, View.DataSourceSelector.DataSourceChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(e.DataSource) == false)
            {
                btnOK.Enabled = true;
            }
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.DoValidate() == false)
                return;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
