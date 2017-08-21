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
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Core.Development.Localisation;
namespace Sheng.SailingEase.Core.Development
{
    [ToolboxItem(false)]
    partial class UserControlEventEditorPanel_DataListRefresh_Where : UserControlEventEditorPanelBase
    {
        private IDataEntityComponentService _dataEntityComponentService;
        private BindingList<DataListRefreshDev.WhereItem> _wheres;
        public BindingList<DataListRefreshDev.WhereItem> Wheres
        {
            get { return _wheres; }
        }
        private Dictionary<DataListRefreshDev.WhereItem, bool> _warningTable = new Dictionary<DataListRefreshDev.WhereItem, bool>();
        public UserControlEventEditorPanel_DataListRefresh_Where(EventEditorAdapterAbstract hostAdapter)
            : base(hostAdapter)
        {
            InitializeComponent();
            _dataEntityComponentService = ServiceUnity.Container.Resolve<IDataEntityComponentService>();
            UIHelper.ProcessDataGridView(this.dataGridViewWhere);
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            this.dataGridViewWhere.AutoGenerateColumns = false;
            this._wheres = new BindingList<DataListRefreshDev.WhereItem>();
            this.dataGridViewWhere.DataSource = this._wheres;
            this.ColumnMatchType.DataSource = EnumDescConverter.Get(typeof(EnumMatchType));
            this.ColumnMatchType.DisplayMember = "Text";
            this.ColumnMatchType.ValueMember = "Object";
        }
        private void ApplyLanguageResource()
        {
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            DataListRefreshDevEditorAdapter adapter = this.HostAdapter as DataListRefreshDevEditorAdapter;
            using (FormEventDataItemDataSet formEventDataItemDataSet =
                new FormEventDataItemDataSet(this.HostAdapter.HostFormEntity))
            {
                formEventDataItemDataSet.DataEntityId = adapter.ParameterPanels.General.DataEntityId;
                formEventDataItemDataSet.AllowDataSourceType = DataListRefreshDev.AllowWhereSetDataSourceType;
                formEventDataItemDataSet.AllowFormElementControlType = DataListRefreshDev.AllowWhereSetFormElementControlType;
                if (formEventDataItemDataSet.ShowDialog() != DialogResult.OK)
                    return;
                DataListRefreshEvent.WhereItem where = new DataListRefreshEvent.WhereItem()
                {
                    DataItem = formEventDataItemDataSet.SelectedDataItemId,
                    DataItemName = formEventDataItemDataSet.SelectedDataItemName,
                    Source = new DataSource(formEventDataItemDataSet.SelectedDataSourceString),
                    SourceName = formEventDataItemDataSet.SelectedDataSourceVisibleString,
                    MatchType = EnumMatchType.Equal
                };
                this._wheres.Add(where);
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewWhere.SelectedRows.Count != 1)
                return;
            DataListRefreshEvent.WhereItem where = this.dataGridViewWhere.SelectedRows[0].DataBoundItem
               as DataListRefreshEvent.WhereItem;
            this._wheres.Remove(where);
            this._warningTable.Remove(where);
        }
        private void dataGridViewWhere_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dr in this.dataGridViewWhere.Rows)
            {
                if (dr != null)
                {
                    DataListRefreshEvent.WhereItem where = dr.DataBoundItem as DataListRefreshEvent.WhereItem;
                    if (this._warningTable.Keys.Contains(where) == false)
                        continue;
                    if (this._warningTable[where])
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
                return Language.Current.DataListRefreshDev_EditorPanel_Where;
            }
        }
        public override List<XObject> GetXml()
        {
            List<XObject> _xObject = new List<XObject>();
            XElement whereElement = new XElement("Where");
            foreach (DataListRefreshDev.WhereItem where in this._wheres)
            {
                whereElement.Add(XElement.Parse(where.ToXml()));
            }
            _xObject.Add(whereElement);
            return _xObject;
        }
        public override void SetParameter(EventBase even)
        {
            DataListRefreshDev _event = even as DataListRefreshDev;
            DataListRefreshDevEditorAdapter adapter = this.HostAdapter as DataListRefreshDevEditorAdapter;
            string dataEntityId = adapter.ParameterPanels.General.DataEntityId;
            DataEntity dataEntity = _dataEntityComponentService.GetDataEntity(dataEntityId);
            this._wheres = new BindingList<DataListRefreshEvent.WhereItem>(_event.Where);
            this._warningTable.Clear();
            bool warningRow;
            foreach (DataListRefreshEvent.WhereItem where in _event.Where)
            {
                warningRow = false;
                DataItemEntity dataItemEntityDev = dataEntity.Items.GetEntityById(where.DataItem);
                if (dataItemEntityDev == null)
                {
                    where.DataItemName = String.Empty;
                    warningRow = true;
                }
                else
                {
                    where.DataItemName = dataItemEntityDev.Name;
                }
                where.SourceName =
                   StringParserLogic.DataSourceVisibleString(this.HostAdapter.HostFormEntity, where.Source.ToString(), out warningRow);
                this._warningTable.Add(where, warningRow);
            }
            this.dataGridViewWhere.DataSource = this._wheres;
        }
    }
}
