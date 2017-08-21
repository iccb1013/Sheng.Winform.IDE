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
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Core.Development.Localisation;
namespace Sheng.SailingEase.Core.Development
{
    [ToolboxItem(false)]
     partial class UserControlEventEditorPanel_UpdateFormData_Where : UserControlEventEditorPanelBase
    {
        private IDataEntityComponentService _dataEntityComponentService
           = ServiceUnity.Container.Resolve<IDataEntityComponentService>();
        private BindingList<UpdateFormDataEvent.WhereItem> _wheres;
        public BindingList<UpdateFormDataEvent.WhereItem> Wheres
        {
            get { return _wheres; }
        }
        private Dictionary<UpdateFormDataEvent.WhereItem, bool> _warningTable = new Dictionary<UpdateFormDataEvent.WhereItem, bool>();
        public UserControlEventEditorPanel_UpdateFormData_Where(EventEditorAdapterAbstract hostAdapter)
            : base(hostAdapter)
        {
            InitializeComponent();
            UIHelper.ProcessDataGridView(this.dataGridViewWhere);
            Unity.ApplyResource(this);
            ApplyLanguageResource();
            this.dataGridViewWhere.AutoGenerateColumns = false;
            this._wheres = new BindingList<UpdateFormDataEvent.WhereItem>();
            this.dataGridViewWhere.DataSource = this._wheres;
            this.ColumnWhereMatchType.DataSource = EnumDescConverter.Get(typeof(EnumMatchType));
            this.ColumnWhereMatchType.DisplayMember = "Text";
            this.ColumnWhereMatchType.ValueMember = "Object";
        }
        private void ApplyLanguageResource()
        {
        }
        private void btnAddWhere_Click(object sender, EventArgs e)
        {
            UpdateFormDataDevEditorAdapter adapter = this.HostAdapter as UpdateFormDataDevEditorAdapter;
            using (FormEventDataItemDataSet formEventDataItemDataSet = new FormEventDataItemDataSet(this.HostAdapter.HostFormEntity))
            {
                formEventDataItemDataSet.DataEntityId = adapter.ParameterPanels.Update.DataEntityId;
                formEventDataItemDataSet.AllowDataSourceType = UpdateFormDataDev.AllowDataSourceType;
                formEventDataItemDataSet.AllowFormElementControlType = UpdateFormDataDev.AllowFormElementControlType;
                if (formEventDataItemDataSet.ShowDialog() == DialogResult.OK)
                {
                    UpdateFormDataEvent.WhereItem where = new UpdateFormDataEvent.WhereItem()
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
        }
        private void btnDeleteWhere_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewWhere.SelectedRows.Count != 1)
            {
                return;
            }
            UpdateFormDataEvent.WhereItem where = this.dataGridViewWhere.SelectedRows[0].DataBoundItem
               as UpdateFormDataEvent.WhereItem;
            this._wheres.Remove(where);
            this._warningTable.Remove(where);
        }
        private void dataGridViewWhere_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dr in this.dataGridViewWhere.Rows)
            {
                if (dr != null)
                {
                    UpdateFormDataEvent.WhereItem where = dr.DataBoundItem as UpdateFormDataEvent.WhereItem;
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
                return Language.Current.UpdateFormDataDev_EditorPanel_Where;
            }
        }
        public override List<XObject> GetXml()
        {
            List<XObject> _xObject = new List<XObject>();
            XElement dataElement = new XElement("Where");
            foreach (UpdateFormDataEvent.WhereItem where in this._wheres)
            {
                dataElement.Add(XElement.Parse(where.ToXml()));
            }
            _xObject.Add(dataElement);
            return _xObject;
        }
        public override void SetParameter(EventBase even)
        {
            UpdateFormDataDev _event = even as UpdateFormDataDev;
            string dataEntityId = _event.DataEntityId;
            DataEntity dataEntity = _dataEntityComponentService.GetDataEntity(dataEntityId);
            this._wheres = new BindingList<UpdateFormDataEvent.WhereItem>(_event.Where);
            this._warningTable.Clear();
            bool warningRow;
            foreach (UpdateFormDataEvent.WhereItem where in _wheres)
            {
                warningRow = false;
                DataItemEntity dataItemEntityDev = dataEntity.Items.GetEntityById(where.DataItem);
                if (dataItemEntityDev == null)
                {
                    where .DataItemName = String.Empty;
                    warningRow = true;
                }
                else
                {
                    where.DataItemName = dataItemEntityDev.Name;
                }
                where.SourceName =
                    StringParserLogic.DataSourceVisibleString(this.HostAdapter.HostFormEntity, where.Source.ToString(), out warningRow);
                _warningTable.Add(where, warningRow);
            }
            this.dataGridViewWhere.DataSource = this._wheres;
        }
    }
}
