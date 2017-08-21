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
using Sheng.SailingEase.Localisation;
using System.Diagnostics;

namespace Sheng.SailingEase.Core.Development
{
    [ToolboxItem(false)]
    partial class UserControlEventEditorPanel_LoadDataToForm_Load : UserControlEventEditorPanelBase
    {
        #region 私有成员

        private IDataEntityComponentService _dataEntityComponentService
           = ServiceUnity.Container.Resolve<IDataEntityComponentService>();

        private BindingList<LoadDataToFormEvent.LoadItem> _loads;
        public BindingList<LoadDataToFormEvent.LoadItem> Loads
        {
            get { return _loads; }
        }

        /// <summary>
        /// 用于记录条件部分哪些条件项目产生警告
        /// DataGridView据此来决定哪些条目需要醒目显示
        /// </summary>
        private Dictionary<LoadDataToFormEvent.LoadItem, bool> _warningTable = new Dictionary<LoadDataToFormEvent.LoadItem, bool>();

        #endregion

        #region 公开属性


        #endregion

        #region 构造

        public UserControlEventEditorPanel_LoadDataToForm_Load(EventEditorAdapterAbstract hostAdapter)
            : base(hostAdapter)
        {
            InitializeComponent();

            UIHelper.ProcessDataGridView(this.dataGridViewLoadData);

            Unity.ApplyResource(this);
            ApplyLanguageResource();

            this.dataGridViewLoadData.AutoGenerateColumns = false;

            this._loads = new BindingList<LoadDataToFormEvent.LoadItem>();
            this.dataGridViewLoadData.DataSource = this._loads;

        }

        #endregion

        #region 应用资源

        private void ApplyLanguageResource()
        {
            //应用本地化语言

        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 添加加载数据设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddLoad_Click(object sender, EventArgs e)
        {
            LoadDataToFormDevEditorAdapter adapter = this.HostAdapter as LoadDataToFormDevEditorAdapter;

            if (adapter.ParameterPanels.General.Mode == LoadDataToFormEvent.EnumLoadDataToFormMode.DataEntity &&
                adapter.ParameterPanels.DataEntity.DataEntityId == String.Empty)
            {
                MessageBox.Show(
                    Language.Current.UserControlEventEditorPanel_LoadDataToForm_Load_MessageChooseDataEntityFirst,
                          CommonLanguage.Current.MessageCaption_Notice,
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (FormEventDataItemDataSet formEventDataItemDataSet = new FormEventDataItemDataSet(this.HostAdapter.HostFormEntity))
            {
                formEventDataItemDataSet.DataSourceLabel = Language.Current.FormEventDataItemDataSet_LabelDataSource_LoadTo;
                formEventDataItemDataSet.AllowDataSourceType = LoadDataToFormDev.AllowLoadDataDataSourceType;
                formEventDataItemDataSet.AllowFormElementControlType = LoadDataToFormDev.AllowFormElementControlType;

                if (adapter.ParameterPanels.General.Mode == LoadDataToFormEvent.EnumLoadDataToFormMode.DataEntity)
                {
                    formEventDataItemDataSet.DataEntityId = adapter.ParameterPanels.DataEntity.DataEntityId;
                }
                else
                {
                    formEventDataItemDataSet.DataEntityId = null;
                }

                if (formEventDataItemDataSet.ShowDialog() == DialogResult.OK)
                {
                    LoadDataToFormEvent.LoadItem load = new LoadDataToFormEvent.LoadItem()
                    {
                        DataItem = formEventDataItemDataSet.SelectedDataItemId,
                        DataItemName = formEventDataItemDataSet.SelectedDataItemName,
                        Source = new DataSource(formEventDataItemDataSet.SelectedDataSourceString),
                        SourceName = formEventDataItemDataSet.SelectedDataSourceVisibleString
                    };

                    this._loads.Add(load);
                }
            }
        }

        /// <summary>
        /// 删除加载数据设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteLoad_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewLoadData.SelectedRows.Count == 0)
            {
                return;
            }

            LoadDataToFormEvent.LoadItem load = this.dataGridViewLoadData.SelectedRows[0].DataBoundItem
               as LoadDataToFormEvent.LoadItem;
            this._loads.Remove(load);

            this._warningTable.Remove(load);
        }

        /// <summary>
        /// 自动填充加载设置，将数据项加载到对应的编辑控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadAll_Click(object sender, EventArgs e)
        {
            LoadDataToFormDevEditorAdapter adapter = this.HostAdapter as LoadDataToFormDevEditorAdapter;

            this._loads.Clear();
            this._warningTable.Clear();

            DataEntity dataEntity = _dataEntityComponentService.GetDataEntity(adapter.ParameterPanels.DataEntity.DataEntityId);

            IUIElementEditControl iFormElementEditControl;
            foreach (UIElement formElement in this.HostAdapter.HostFormEntity.Elements)
            {
                iFormElementEditControl = formElement as IUIElementEditControl;
                //没有实现IFormElementEditControl接口，认为不是编辑控件
                if (iFormElementEditControl == null)
                    continue;

                if (String.IsNullOrEmpty(iFormElementEditControl.DataItemId))
                    continue;

                string[] ids = iFormElementEditControl.DataItemId.Split('.');
                string dataEntityId = ids[0];
                string dataItemId = ids[1];

                if (dataEntityId != dataEntity.Id)
                    continue;

                DataItemEntity dataItemEntity = dataEntity.Items.GetEntityById(dataItemId);
                if (dataItemEntity == null)
                    continue;

                LoadDataToFormEvent.LoadItem load = new LoadDataToFormEvent.LoadItem()
                {
                    DataItem = dataItemId,
                    DataItemName = dataItemEntity.Name,
                    Source = new DataSource(StringParserLogic.DataSourceString(formElement)),
                    SourceName = StringParserLogic.DataSourceVisibleString(formElement)
                };

                this._loads.Add(load);

                this._warningTable.Add(load, false);

            }

        }

        /// <summary>
        /// 标记出警告的行 载入设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewLoadData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dr in this.dataGridViewLoadData.Rows)
            {
                if (dr != null)
                {
                    LoadDataToFormEvent.LoadItem load = dr.DataBoundItem as LoadDataToFormEvent.LoadItem;

                    //新增时，警告字典没有相应的警告信息，也不需要在新增时checkwarning
                    if (this._warningTable.Keys.Contains(load) == false)
                        continue;

                    if (this._warningTable[load])
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

        #endregion

        #region IEventEditorPanel 成员

        public override string PanelTitle
        {
            get
            {
                return Language.Current.LoadDataToFormDev_EditorPanel_Load;
            }
        }

        public override List<XObject> GetXml()
        {
            List<XObject> _xObject = new List<XObject>();

            XElement dataElement = new XElement("Load");
            foreach (LoadDataToFormEvent.LoadItem load in this._loads)
            {
                dataElement.Add(XElement.Parse(load.ToXml()));
            }

            _xObject.Add(dataElement);

            return _xObject;
        }

        public override void SetParameter(EventBase even)
        {
            LoadDataToFormDev _event = even as LoadDataToFormDev;

            this._loads = new BindingList<LoadDataToFormEvent.LoadItem>(_event.Load);
            this._warningTable.Clear();

            DataEntity dataEntity = _dataEntityComponentService.GetDataEntity(_event.DataEntityId);

            Debug.Assert(dataEntity != null, " dataEntity 为 null");

            if (dataEntity != null)
            {
                bool warningRow;
                foreach (LoadDataToFormEvent.LoadItem load in _loads)
                {
                    warningRow = false;
                    load.SourceName = StringParserLogic.DataSourceVisibleString(this.HostAdapter.HostFormEntity, load.Source.ToString(), out warningRow);

                    if (_event.LoadMode == LoadDataToFormEvent.EnumLoadDataToFormMode.DataEntity)
                    {
                        //DataItemEntityDev
                        DataItemEntity dataItemEntityDev = dataEntity.Items.GetEntityById(load.DataItem);

                        if (dataItemEntityDev == null)
                        {
                            load.DataItemName = String.Empty;
                            warningRow = true;
                        }
                        else
                        {
                            load.DataItemName = dataItemEntityDev.Name;
                        }
                    }
                    else
                    {
                        load.DataItemName = load.DataItem;
                    }

                    _warningTable.Add(load, warningRow);
                }
            }

            this.dataGridViewLoadData.DataSource = this._loads;
        }


        #endregion
    }
}
