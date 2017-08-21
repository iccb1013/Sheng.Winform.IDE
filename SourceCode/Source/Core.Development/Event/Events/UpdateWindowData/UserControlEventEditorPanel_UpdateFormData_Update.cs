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
using Sheng.SailingEase.Core.Development.Localisation;

namespace Sheng.SailingEase.Core.Development
{
    [ToolboxItem(false)]
     partial class UserControlEventEditorPanel_UpdateFormData_Update : UserControlEventEditorPanelBase
    {
        #region 私有成员

        private IDataEntityComponentService _dataEntityComponentService
           = ServiceUnity.Container.Resolve<IDataEntityComponentService>();

        private DataEntityTreeChooseView _formDataEntityTreeChoose = new DataEntityTreeChooseView(false);

        private BindingList<UpdateFormDataEvent.UpdateItem> _updates;
        public BindingList<UpdateFormDataEvent.UpdateItem> Updates
        {
            get { return _updates; }
        }

        /// <summary>
        /// 用于记录条件部分哪些条件项目产生警告
        /// DataGridView据此来决定哪些条目需要醒目显示
        /// </summary>
        private Dictionary<UpdateFormDataEvent.UpdateItem, bool> _warningTable = new Dictionary<UpdateFormDataEvent.UpdateItem, bool>();

        #endregion

        #region 公开属性

        /// <summary>
        /// 选择的数据实体Id
        /// </summary>
        public string DataEntityId
        {
            get
            {
                return _formDataEntityTreeChoose.SelectedId;
            }
            set
            {
                this._formDataEntityTreeChoose.SelectedId = value;
                this._formDataEntityTreeChoose.SelectedName = _dataEntityComponentService.GetDataEntity(value).Name;
                this.txtDataEntityName.Text = _formDataEntityTreeChoose.SelectedName;
            }
        }

        #endregion

        #region 构造

        public UserControlEventEditorPanel_UpdateFormData_Update(EventEditorAdapterAbstract hostAdapter)
            : base(hostAdapter)
        {
            InitializeComponent();

            UIHelper.ProcessDataGridView(this.dataGridViewUpdateData);

            Unity.ApplyResource(this);
            ApplyLanguageResource();

            this.dataGridViewUpdateData.AutoGenerateColumns = false;

            this._updates = new BindingList<UpdateFormDataEvent.UpdateItem>();
            this.dataGridViewUpdateData.DataSource = this._updates;
        }

        #endregion

        #region 应用资源

        private void ApplyLanguageResource()
        {
            //应用本地化语言


            this.txtDataEntityName.Title = Language.Current.UserControlEventEditorPanel_UpdateFormData_Update_LabelDataEntity;

        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 点击浏览数据实体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowseDataEntity_Click(object sender, EventArgs e)
        {
            if (_formDataEntityTreeChoose.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            txtDataEntityName.Text = _formDataEntityTreeChoose.SelectedName;
        }

        /// <summary>
        /// 添加更新数据设置 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddUpdate_Click(object sender, EventArgs e)
        {
            using (FormEventDataItemDataSet formEventDataItemDataSet =
               new FormEventDataItemDataSet(this.HostAdapter.HostFormEntity))
            {
                formEventDataItemDataSet.DataEntityId = this._formDataEntityTreeChoose.SelectedId;
                formEventDataItemDataSet.AllowDataSourceType = UpdateFormDataDev.AllowDataSourceType;
                formEventDataItemDataSet.AllowFormElementControlType = UpdateFormDataDev.AllowFormElementControlType;

                if (formEventDataItemDataSet.ShowDialog() == DialogResult.OK)
                {
                    UpdateFormDataEvent.UpdateItem update = new UpdateFormDataEvent.UpdateItem()
                    {
                        DataItem = formEventDataItemDataSet.SelectedDataItemId,
                        DataItemName = formEventDataItemDataSet.SelectedDataItemName,
                        Source = new DataSource(formEventDataItemDataSet.SelectedDataSourceString),
                        SourceName = formEventDataItemDataSet.SelectedDataSourceVisibleString,
                    };

                    this._updates.Add(update);
                }
            }
        }

        /// <summary>
        /// 删除更新数据设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteUpdate_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewUpdateData.SelectedRows.Count != 1)
            {
                return;
            }

            UpdateFormDataEvent.UpdateItem update = this.dataGridViewUpdateData.SelectedRows[0].DataBoundItem
               as UpdateFormDataEvent.UpdateItem;
            this._updates.Remove(update);

            this._warningTable.Remove(update);
        }

        /// <summary>
        /// 自动填充更新数据设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateAll_Click(object sender, EventArgs e)
        {
            this._updates.Clear();
            this._warningTable.Clear();

            string selectedDataEntityId = _formDataEntityTreeChoose.SelectedId;
            DataEntity dataEntity = _dataEntityComponentService.GetDataEntity(selectedDataEntityId);

            IUIElementEditControl iFormElementEditControl;
            foreach (UIElement formElement in this.HostAdapter.HostFormEntity.Elements)
            {
                iFormElementEditControl = formElement as IUIElementEditControl;
                if (iFormElementEditControl == null)
                    continue;

                if (String.IsNullOrEmpty(iFormElementEditControl.DataItemId))
                    continue;

                string[] ids = iFormElementEditControl.DataItemId.Split('.');
                string dataEntityId = ids[0];
                string dataItemId = ids[1];

                if (dataEntityId != selectedDataEntityId)
                    continue;

                DataItemEntity dataItemEntity = dataEntity.Items.GetEntityById(dataItemId);
                if (dataItemEntity == null)
                    continue;

                UpdateFormDataEvent.UpdateItem update = new UpdateFormDataEvent.UpdateItem()
                {
                    DataItem = dataItemId,
                    DataItemName = dataItemEntity.Name,
                    Source = new DataSource(StringParserLogic.DataSourceString(formElement)),
                    SourceName = StringParserLogic.DataSourceVisibleString(formElement)
                };

                this._updates.Add(update);
                this._warningTable.Add(update, false);
            }
        }

        /// <summary>
        /// 标记出警告的行 更新设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewUpdateData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dr in this.dataGridViewUpdateData.Rows)
            {
                if (dr != null)
                {
                    UpdateFormDataEvent.UpdateItem update = dr.DataBoundItem as UpdateFormDataEvent.UpdateItem;

                    //新增时，警告字典没有相应的警告信息，也不需要在新增时checkwarning
                    if (this._warningTable.Keys.Contains(update) == false)
                        continue;

                    if (this._warningTable[update])
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
                return Language.Current.UpdateFormDataDev_EditorPanel_Update;
            }
        }

        public override List<XObject> GetXml()
        {
            List<XObject> _xObject = new List<XObject>();

            _xObject.Add(new XElement("DataEntityId", this.DataEntityId));

            XElement dataElement = new XElement("Update");
            foreach (UpdateFormDataEvent.UpdateItem update in this._updates)
            {
                dataElement.Add(XElement.Parse(update.ToXml()));
            }

            _xObject.Add(dataElement);

            return _xObject;
        }

        public override void SetParameter(EventBase even)
        {
            UpdateFormDataDev _event = even as UpdateFormDataDev;

            this.DataEntityId = _event.DataEntityId;

            DataEntity dataEntity = _dataEntityComponentService.GetDataEntity(this.DataEntityId);

            this._updates = new BindingList<UpdateFormDataEvent.UpdateItem>(_event.Update);
            this._warningTable.Clear();

            //为用于显示的DataColumnName和SourceName赋值
            bool warningRow;
            foreach (UpdateFormDataEvent.UpdateItem update in _updates)
            {
                warningRow = false;

                DataItemEntity dataItemEntityDev = dataEntity.Items.GetEntityById(update.DataItem);
                if (dataItemEntityDev == null)
                {
                    update .DataItemName = String.Empty;
                    warningRow = true;
                }
                else
                {
                    update.DataItemName = dataItemEntityDev.Name;
                }

                update.SourceName =
                    StringParserLogic.DataSourceVisibleString(this.HostAdapter.HostFormEntity, update.Source.ToString(), out warningRow);

                _warningTable.Add(update, warningRow);
            }

            this.dataGridViewUpdateData.DataSource = this._updates;

        }

        #endregion
    }
}
