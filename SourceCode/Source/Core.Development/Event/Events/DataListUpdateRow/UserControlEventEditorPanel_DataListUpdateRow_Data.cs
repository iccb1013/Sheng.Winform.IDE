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
    partial class UserControlEventEditorPanel_DataListUpdateRow_Data : UserControlEventEditorPanelBase
    {
        private BindingList<DataListUpdateRowEvent.DataItem> _datas;
        private Dictionary<DataListUpdateRowEvent.DataItem, bool> _warningTable = new Dictionary<DataListUpdateRowEvent.DataItem, bool>();
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
        public UserControlEventEditorPanel_DataListUpdateRow_Data(EventEditorAdapterAbstract hostAdapter)
            : base(hostAdapter)
        {
            InitializeComponent();
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            this.dataGridViewDataSet.AutoGenerateColumns = false;
            this._datas = new BindingList<DataListUpdateRowEvent.DataItem>();
            this.dataGridViewDataSet.DataSource = this._datas;      
            this.ColumnWhere.DataSource = Misc.TrueFalseTable;
            this.ColumnWhere.DisplayMember = "Text";
            this.ColumnWhere.ValueMember = "Value";
        }
        private void ApplyLanguageResource()
        {
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            DataListDataItemEditView view = new DataListDataItemEditView();
            view.WindowEntity = this.HostAdapter.HostFormEntity;
            view.DataListEntity = this.HostAdapter.HostFormEntity.FindFormElementById(this.DataListId) as UIElementDataListEntity;
            if (view.ShowDialog() == DialogResult.OK)
            {
                DataListUpdateRowEvent.DataItem data = new DataListUpdateRowEvent.DataItem()
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
            DataListUpdateRowEvent.DataItem data = this.dataGridViewDataSet.SelectedRows[0].DataBoundItem
               as DataListUpdateRowEvent.DataItem;
            this._datas.Remove(data);
            this._warningTable.Remove(data);
        }
        private void dataGridViewDataSet_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dr in this.dataGridViewDataSet.Rows)
            {
                if (dr != null)
                {
                    DataListUpdateRowEvent.DataItem data = dr.DataBoundItem as DataListUpdateRowEvent.DataItem;
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
                return Language.Current.DataListUpdateRowDev_EditorPanel_Data;
            }
        }
        public override List<XObject> GetXml()
        {
            List<XObject> _xObject = new List<XObject>();
            _xObject.Add(new XElement("DataList", this.DataListId));
            _xObject.Add(new XElement("TargetWindow", (int)this.TargetWindow));
            XElement dataElement = new XElement("Data");
            foreach (DataListUpdateRowEvent.DataItem data in this._datas)
            {
                dataElement.Add(XElement.Parse(data.ToXml()));
            }
            _xObject.Add(dataElement);
            return _xObject;
        }
        public override void SetParameter(EventBase even)
        {
            DataListUpdateRowDev _event = even as DataListUpdateRowDev;
            this.DataListId = _event.DataList;
            this.TargetWindow = _event.TargetWindow;
            this._datas = new BindingList<DataListUpdateRowEvent.DataItem>(_event.Data);
            this._warningTable.Clear();
            bool warningRow = false;
            DataSourceProvideArgs args = new DataSourceProvideArgs()
            {
                WindowEntity = this.HostAdapter.HostFormEntity
            };
             foreach (DataListUpdateRowEvent.DataItem data in _datas)
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
                             data .DataColumnName = dataColumn.Name;
                     }
                 }
                 else if (this.TargetWindow == EnumTargetWindow.Caller)
                 {
                     data.DataColumnName = data.DataColumn;
                 }
                 _warningTable.Add(data, warningRow);
             }
             this.dataGridViewDataSet.DataSource = this._datas;
        }
    }
}
