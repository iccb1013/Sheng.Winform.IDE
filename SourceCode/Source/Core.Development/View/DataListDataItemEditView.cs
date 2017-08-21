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
    partial class DataListDataItemEditView : FormViewBase
    {
        private WindowEntity _windowEntity;
        public WindowEntity WindowEntity
        {
            get { return _windowEntity; }
            set { _windowEntity = value; }
        }
        private UIElementDataListEntity _dataListEntity;
        public UIElementDataListEntity DataListEntity
        {
            get { return _dataListEntity; }
            set
            {
                _dataListEntity = value;
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
        public string DataColumn
        {
            get
            {
                UIElementDataListColumnEntityAbstract column = ddlDataColumn.SelectedValue as UIElementDataListColumnEntityAbstract;
                if (column == null)
                    return String.Empty;
                return column.Id;
            }
        }
        public string DataColumnName
        {
            get
            {
                UIElementDataListColumnEntityAbstract column = ddlDataColumn.SelectedValue as UIElementDataListColumnEntityAbstract;
                if (column == null)
                    return String.Empty;
                return column.Name;
            }
        }
        public DataListDataItemEditView()
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            ddlDataColumn.DisplayMember = EntityBase.Property_Name;
            ddlDataColumn.Title = Language.Current.DataListDataItemEditView_LabelDataColumn;
        }
        private void DataListDataItemEditView_Load(object sender, EventArgs e)
        {
            if (_dataListEntity != null)
                ddlDataColumn.DataSource = _dataListEntity.DataColumns;
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
