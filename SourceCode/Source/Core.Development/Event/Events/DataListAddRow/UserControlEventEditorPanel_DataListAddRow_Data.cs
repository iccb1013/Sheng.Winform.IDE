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
using System.Xml;
using System.Xml.Linq;
using Sheng.SailingEase.Core.Development;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Core.Development.Localisation;
namespace Sheng.SailingEase.Core.Development
{
    [ToolboxItem(false)]
    partial class UserControlEventEditorPanel_DataListAddRow_Data : UserControlEventEditorPanelBase
    {
        private BindingList<DataListAddRowEvent.DataItem> _datas;
        private Dictionary<DataListAddRowEvent.DataItem, bool> _warningTable = new Dictionary<DataListAddRowEvent.DataItem, bool>();
        public EnumTargetWindow TargetWindow
        {
            get;
            set;
        }
        private string _dataListId;
        public string DataListId
        {
            get
            {
                return this._dataListId;
            }
            set
            {
                this._dataListId = value;
            }
        }
        public UserControlEventEditorPanel_DataListAddRow_Data(EventEditorAdapterAbstract hostAdapter)
            : base(hostAdapter)
        {
            InitializeComponent();
            UIHelper.ProcessDataGridView(this.dataGridViewDataSet);
            Unity.ApplyResource(this);
            this.dataGridViewDataSet.AutoGenerateColumns = false;
            this._datas = new BindingList<DataListAddRowEvent.DataItem>();
            this.dataGridViewDataSet.DataSource = this._datas;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            DataListDataItemEditView view = new DataListDataItemEditView();
            view.WindowEntity = this.HostAdapter.HostFormEntity;
            view.DataListEntity = this.HostAdapter.HostFormEntity.FindFormElementById(this.DataListId) as UIElementDataListEntity;
            if (view.ShowDialog() == DialogResult.OK)
            {
                DataListAddRowEvent.DataItem data = new DataListAddRowEvent.DataItem()
                {
                    DataColumn = view.DataColumn,
                    DataColumnName = view.DataColumnName,
                    Source = view.DataSource,
                    SourceName = view.DataSourceName
                };
                this._datas.Add(data);
            }
            view.Dispose();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewDataSet.SelectedRows.Count != 1)
                return;
            DataListAddRowEvent.DataItem data = this.dataGridViewDataSet.SelectedRows[0].DataBoundItem
               as DataListAddRowEvent.DataItem;
            this._datas.Remove(data);
            this._warningTable.Remove(data);
        }
        private void dataGridViewDataSet_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dr in this.dataGridViewDataSet.Rows)
            {
                if (dr != null)
                {
                    DataListAddRowEvent.DataItem data = dr.DataBoundItem as DataListAddRowEvent.DataItem;
                    if (this._warningTable.Keys.Contains(data) == false)
                        continue;
                    if (this._warningTable[data])
                    {
                        dr.DefaultCellStyle.BackColor = UIHelper.DataGridViewRowBackColorWarning;
                        dr.DefaultCellStyle.SelectionBackColor = UIHelper.DataGridViewSelectedRowBackColorWarning;
                        dr.DefaultCellStyle.ForeColor = UIHelper.DataGridViewRowForeColorWarning;
                        dr.DefaultCellStyle.SelectionForeColor = UIHelper.DataGridViewSelectedRowForeColorWarning;
                    }
                    else
                    {
                        dr.DefaultCellStyle.BackColor = UIHelper.DataGridViewRowBackColorNormal;
                        dr.DefaultCellStyle.SelectionBackColor = UIHelper.DataGridViewSelectedRowBackColorNormal;
                        dr.DefaultCellStyle.ForeColor = UIHelper.DataGridViewRowForeColorNormal;
                        dr.DefaultCellStyle.SelectionForeColor = UIHelper.DataGridViewSelectedRowForeColorNormal;
                    }
                }
            }
        }
        public override string PanelTitle
        {
            get
            {
                return Language.Current.DataListAddRowDev_EditorPanel_Data;
            }
        }
        public override List<XObject> GetXml()
        {
            List<XObject> _xObject = new List<XObject>();
            _xObject.Add(new XElement("DataList", this.DataListId));
            _xObject.Add(new XElement("TargetWindow", (int)this.TargetWindow));
            XElement whereElement = new XElement("Data");
            foreach (DataListAddRowEvent.DataItem data in this._datas)
            {
                whereElement.Add(XElement.Parse(data.ToXml()));
            }
            _xObject.Add(whereElement);
            return _xObject;
        }
        public override void SetParameter(EventBase even)
        {
            DataListAddRowDev _event = even as DataListAddRowDev;
            this.DataListId = _event.DataList;
            this.TargetWindow = _event.TargetWindow;
            this._datas = new BindingList<DataListAddRowEvent.DataItem>(_event.Data);
            this._warningTable.Clear();
            bool warningRow = false;
            DataSourceProvideArgs args = new DataSourceProvideArgs(){
                    WindowEntity = this.HostAdapter.HostFormEntity
                };
            foreach (DataListAddRowEvent.DataItem data in _event.Data)
            {
                data.SourceName = DataSourceProvideFactory.Instance.GetDisplayString(data.Source, args);
                if (String.IsNullOrEmpty(data.SourceName))
                {
                    warningRow = true;
                }
                if (this.TargetWindow == EnumTargetWindow.Current)
                {
                    UIElement formElementDataList = this.HostAdapter.HostFormEntity.FindFormElementById(_event.DataList);
                    if (formElementDataList == null)
                    {
                        warningRow = true;
                    }
                    else
                    {
                        UIElementDataListEntity dataList = (UIElementDataListEntity)formElementDataList;
                        UIElementDataListColumnEntityAbstract dataColumn = dataList.GetDataColumn(data.DataColumn);
                        if (dataColumn == null)
                            warningRow = true;
                        else
                            data.DataColumnName = dataColumn.Name;
                    }
                }
                else
                {
                    data.DataColumnName = data.DataColumn;
                }
                this._warningTable.Add(data, warningRow);
            }
            this.dataGridViewDataSet.DataSource = this._datas;
        }
    }
}
