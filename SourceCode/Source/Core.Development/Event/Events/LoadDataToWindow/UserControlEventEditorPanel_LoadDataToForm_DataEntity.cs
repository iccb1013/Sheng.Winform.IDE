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
    partial class UserControlEventEditorPanel_LoadDataToForm_DataEntity : UserControlEventEditorPanelBase
    {
        #region 私有成员

        private IDataEntityComponentService _dataEntityComponentService = ServiceUnity.DataEntityComponentService;

        private DataEntityTreeChooseView _formDataEntityTreeChoose = new DataEntityTreeChooseView(false);

        private BindingList<LoadDataToFormEvent.WhereItem> _wheres;
        public BindingList<LoadDataToFormEvent.WhereItem> Wheres
        {
            get { return _wheres; }
        }

        /// <summary>
        /// 用于记录条件部分哪些条件项目产生警告
        /// DataGridView据此来决定哪些条目需要醒目显示
        /// </summary>
        private Dictionary<LoadDataToFormEvent.WhereItem, bool> _warningTable = new Dictionary<LoadDataToFormEvent.WhereItem, bool>();

        #endregion

        #region 公开属性

        /// <summary>
        /// 数据实体是否允许空
        /// 如果首要那边选的是sql方式,就可以为空
        /// </summary>
        public bool DataEntityAllowEmpty
        {
            set
            {
                this.txtDataEntityName.AllowEmpty = value;
            }
        }

        //否决
        //private DataTable dtWhere;
        ///// <summary>
        ///// 条件设置
        ///// </summary>
        //public DataTable Where
        //{
        //    get
        //    {
        //        return this.dtWhere;
        //    }
        //    set
        //    {
        //        this.dtWhere = value;
        //        this.dataGridViewWhere.DataSource = this.dtWhere;
        //    }
        //}

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
                //DataEntityDev
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

        public UserControlEventEditorPanel_LoadDataToForm_DataEntity(EventEditorAdapterAbstract hostAdapter)
            : base(hostAdapter)
        {
            InitializeComponent();

            UIHelper.ProcessDataGridView(this.dataGridViewWhere);

            Unity.ApplyResource(this);
            ApplyLanguageResource();

            this.dataGridViewWhere.AutoGenerateColumns = false;

            this._wheres = new BindingList<LoadDataToFormEvent.WhereItem>();
            this.dataGridViewWhere.DataSource = this._wheres;


           //关联条件表中匹配方式枚举
            this.ColumnWhereMatchType.DataSource = EnumDescConverter.Get(typeof(EnumMatchType));
            this.ColumnWhereMatchType.DisplayMember = "Text";
            this.ColumnWhereMatchType.ValueMember = "Object";
        }

        #endregion

        #region 应用资源

        private void ApplyLanguageResource()
        {
            //应用本地化语言


            this.txtDataEntityName.Title = Language.Current.UserControlEventEditorPanel_LoadDataToForm_DataEntity_LabelDataEntity;

        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 添加条件设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddWhere_Click(object sender, EventArgs e)
        {
            using (FormEventDataItemDataSet formEventDataItemDataSet =
               new FormEventDataItemDataSet(this.HostAdapter.HostFormEntity))
            {
                formEventDataItemDataSet.DataEntityId = this.DataEntityId;

                formEventDataItemDataSet.AllowDataSourceType = LoadDataToFormDev.AllowWhereSetDataSourceType;

                formEventDataItemDataSet.AllowFormElementControlType = LoadDataToFormDev.AllowFormElementControlType;

                if (formEventDataItemDataSet.ShowDialog() == DialogResult.OK)
                {
                    LoadDataToFormEvent.WhereItem where = new LoadDataToFormEvent.WhereItem()
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

        /// <summary>
        /// 删除条件设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteWhere_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewWhere.SelectedRows.Count != 1)
            {
                return;
            }

            LoadDataToFormEvent.WhereItem where = this.dataGridViewWhere.SelectedRows[0].DataBoundItem
               as LoadDataToFormEvent.WhereItem;
            this._wheres.Remove(where);

            this._warningTable.Remove(where);
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

        /// <summary>
        /// 标记出警告的行 条件列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewWhere_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dr in this.dataGridViewWhere.Rows)
            {
                if (dr != null)
                {
                    LoadDataToFormEvent.WhereItem where = dr.DataBoundItem as LoadDataToFormEvent.WhereItem;

                    //新增时，警告字典没有相应的警告信息，也不需要在新增时checkwarning
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

        #endregion

        #region IEventEditorPanel 成员

        public override string PanelTitle
        {
            get
            {
                return Language.Current.LoadDataToFormDev_EditorPanel_DataEntity;
            }
        }

        public override List<XObject> GetXml()
        {
            List<XObject> _xObject = new List<XObject>();

            _xObject.Add(new XElement("DataEntityId", this.DataEntityId));

            XElement dataElement = new XElement("Where");
            foreach (LoadDataToFormEvent.WhereItem where in this._wheres)
            {
                dataElement.Add(XElement.Parse(where.ToXml()));
            }

            _xObject.Add(dataElement);

            return _xObject;
        }

        public override void SetParameter(EventBase even)
        {
            LoadDataToFormDev _event = even as LoadDataToFormDev;

            this.DataEntityId = _event.DataEntityId;
            DataEntity dataEntity = _dataEntityComponentService.GetDataEntity(this.DataEntityId);

            this._wheres = new BindingList<LoadDataToFormEvent.WhereItem>(_event.Where);
            this._warningTable.Clear();

            //为用于显示的DataColumnName和SourceName赋值
            bool warningRow;
            foreach (LoadDataToFormEvent.WhereItem where in _wheres)
            {
                warningRow = false;

                //DataItemEntityDev
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

                _warningTable.Add(where, warningRow);
            }

            this.dataGridViewWhere.DataSource = this._wheres;
        }


        #endregion

    }
}
