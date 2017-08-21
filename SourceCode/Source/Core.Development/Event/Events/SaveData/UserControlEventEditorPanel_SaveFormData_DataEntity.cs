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
    partial class UserControlEventEditorPanel_SaveFormData_DataEntity : UserControlEventEditorPanelBase
    {
        #region 私有成员

        private IDataEntityComponentService _dataEntityComponentService
           = ServiceUnity.Container.Resolve<IDataEntityComponentService>();

        private DataEntityTreeChooseView _formDataEntityTreeChoose = new DataEntityTreeChooseView(false);

        private BindingList<SaveDataEvent.SaveItem> _saves;
        public BindingList<SaveDataEvent.SaveItem> Saves
        {
            get { return _saves; }
        }

        /// <summary>
        /// 用于记录条件部分哪些条件项目产生警告
        /// DataGridView据此来决定哪些条目需要醒目显示
        /// </summary>
        private Dictionary<SaveDataEvent.SaveItem, bool> _warningTable = new Dictionary<SaveDataEvent.SaveItem, bool>();

        private DataSourceProvideFactory _dataSourceProvideFactory = DataSourceProvideFactory.Instance;

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
                DataEntity dataEntityDev = _dataEntityComponentService.GetDataEntity(value);

                if (dataEntityDev == null)
                {
                    this._formDataEntityTreeChoose.SelectedId = String.Empty;
                    this._formDataEntityTreeChoose.SelectedName = String.Empty;
                    this.txtDataEntityName.Text = String.Empty;
                }
                else
                {
                    this._formDataEntityTreeChoose.SelectedId = value;
                    this._formDataEntityTreeChoose.SelectedName = dataEntityDev.Name;
                    this.txtDataEntityName.Text = dataEntityDev.Name;
                }
            }
        }

        #endregion

        #region 构造

        public UserControlEventEditorPanel_SaveFormData_DataEntity(EventEditorAdapterAbstract hostAdapter)
            : base(hostAdapter)
        {
            InitializeComponent();

            UIHelper.ProcessDataGridView(this.dataGridViewSaveData);

            Unity.ApplyResource(this);
            ApplyLanguageResource();

            this.dataGridViewSaveData.AutoGenerateColumns = false;

            this._saves = new BindingList<SaveDataEvent.SaveItem>();
            this.dataGridViewSaveData.DataSource = this._saves;
        }

        #endregion

        #region 应用资源

        private void ApplyLanguageResource()
        {
            //应用本地化语言

            this.ColumnDataItemName.HeaderText = Language.Current.UserControlEventEditorPanel_SaveFormData_DataEntity_ColumnDataItemName;
            this.ColumnDataSource.HeaderText = Language.Current.UserControlEventEditorPanel_SaveFormData_DataEntity_ColumnDataSource;

            this.txtDataEntityName.Title = Language.Current.UserControlEventEditorPanel_SaveFormData_DataEntity_LabelDataEntity;
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            DataEntityItemDataSourceEditView view = new DataEntityItemDataSourceEditView();
            view.WindowEntity = this.HostAdapter.HostFormEntity;
            view.DataEntity = _formDataEntityTreeChoose.SelectedDataEntity;
            if (view.ShowDialog() == DialogResult.OK)
            {
                SaveDataEvent.SaveItem save = new SaveDataEvent.SaveItem()
                {
                    DataItem = view.DataItem.Id,
                    DataItemName = view.DataItem.Name,
                    Source = view.DataSource,
                    SourceName = view.DataSourceName
                };

                this._saves.Add(save);
            }
            view.Dispose();
        }

        /// <summary>
        /// 删除选中的保存数据设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewSaveData.SelectedRows.Count !=1)
            {
                return;
            }

            SaveDataEvent.SaveItem save = this.dataGridViewSaveData.SelectedRows[0].DataBoundItem
               as SaveDataEvent.SaveItem;
            this._saves.Remove(save);

            this._warningTable.Remove(save);
        }

        /// <summary>
        /// 自动加载进所有编辑控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAll_Click(object sender, EventArgs e)
        {
            this._saves.Clear();
            this._warningTable.Clear();

            DataEntity dataEntity = _formDataEntityTreeChoose.SelectedDataEntity;

            WindowEntity windowEntity = this.HostAdapter.HostFormEntity;
            DataSourceProvideArgs dataSourceProvideArgs = new DataSourceProvideArgs()
            {
                WindowEntity = windowEntity
            };

            foreach (UIElement formElement in windowEntity.Elements)
            {
                if (formElement.DataSourceUseable == false)
                    continue;

                IUIElementEditControl iFormElementEditControl = formElement as IUIElementEditControl;
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

                string source = _dataSourceProvideFactory.GetDataSourceString(formElement, dataSourceProvideArgs);
                string sourceName = _dataSourceProvideFactory.GetDisplayString(source, dataSourceProvideArgs);

                SaveDataEvent.SaveItem save = new SaveDataEvent.SaveItem()
                {
                    DataItem = dataItemId,
                    DataItemName = dataItemEntity.Name,
                    Source = source,
                    SourceName = sourceName
                };

                this._saves.Add(save);
            }

        }

        /// <summary>
        /// 标记出警告的行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewSaveData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dr in this.dataGridViewSaveData.Rows)
            {
                if (dr != null)
                {
                    SaveDataEvent.SaveItem save = dr.DataBoundItem as SaveDataEvent.SaveItem;

                    //新增时，警告字典没有相应的警告信息，也不需要在新增时checkwarning
                    if (this._warningTable.Keys.Contains(save) == false)
                        continue;

                    if (this._warningTable[save])
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

        #endregion

        #region IEventEditorPanel 成员

        public override string PanelTitle
        {
            get
            {
                return Language.Current.SaveFormDataDev_EditorPanel_DataEntity;
            }
        }

        public override List<XObject> GetXml()
        {
            List<XObject> _xObject = new List<XObject>();

            _xObject.Add(new XElement("DataEntityId", this.DataEntityId));

            XElement whereElement = new XElement("Save");
            foreach (SaveDataEvent.SaveItem save in this._saves)
            {
                whereElement.Add(XElement.Parse(save.ToXml()));
            }

            _xObject.Add(whereElement);

            return _xObject;
        }

        public override void SetParameter(EventBase even)
        {
            SaveDataDev _event = even as SaveDataDev;

            this.DataEntityId = _event.DataEntityId;
            this._formDataEntityTreeChoose.SelectedId = this.DataEntityId;

            DataEntity dataEntity = _dataEntityComponentService.GetDataEntity(this.DataEntityId);

            this._saves = new BindingList<SaveDataEvent.SaveItem>(_event.Save);
            this._warningTable.Clear();

            //为用于显示的DataColumnName和SourceName赋值
            bool warningRow = false;
            DataSourceProvideArgs args = new DataSourceProvideArgs()
            {
                WindowEntity = this.HostAdapter.HostFormEntity
            };
            foreach (SaveDataEvent.SaveItem save in _saves)
            {
                save.SourceName = DataSourceProvideFactory.Instance.GetDisplayString(save.Source, args);
                if (String.IsNullOrEmpty(save.SourceName))
                {
                    warningRow = true;
                }

                DataItemEntity dataItemEntity = dataEntity.Items.GetEntityById(save.DataItem);
                if (dataItemEntity == null)
                {
                    save.DataItemName = String.Empty;
                    warningRow = true;
                }
                else
                {
                    save.DataItemName = dataItemEntity.Name;
                }

                _warningTable.Add(save, warningRow);
            }

            this.dataGridViewSaveData.DataSource = this._saves;
        }


        #endregion

    }
}
