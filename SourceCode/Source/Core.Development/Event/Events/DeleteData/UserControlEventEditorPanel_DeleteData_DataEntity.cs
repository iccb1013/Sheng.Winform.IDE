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
    partial class UserControlEventEditorPanel_DeleteData_DataEntity : UserControlEventEditorPanelBase
    {
        #region 私有成员

        private IDataEntityComponentService _dataEntityComponentService;

        //FormDataEntityTreeChoose
        private DataEntityTreeChooseView _formDataEntityTreeChoose = new DataEntityTreeChooseView(false);

        private BindingList<DeleteDataEvent.WhereItem> _wheres;
        public BindingList<DeleteDataEvent.WhereItem> Where
        {
            get { return _wheres; }
        }

        /// <summary>
        /// 用于记录条件部分哪些条件项目产生警告
        /// DataGridView据此来决定哪些条目需要醒目显示
        /// </summary>
        private Dictionary<DeleteDataEvent.WhereItem, bool> _warningTable = new Dictionary<DeleteDataEvent.WhereItem, bool>();

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

        ////否决
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

        #endregion

        #region 构造

        public UserControlEventEditorPanel_DeleteData_DataEntity(EventEditorAdapterAbstract hostAdapter)
            : base(hostAdapter)
        {
            InitializeComponent();

            _dataEntityComponentService = ServiceUnity.Container.Resolve<IDataEntityComponentService>();

            UIHelper.ProcessDataGridView(this.dataGridViewWhere);

            Unity.ApplyResource(this);
            ApplyLanguageResource();

            this.dataGridViewWhere.AutoGenerateColumns = false;

            this._wheres = new BindingList<DeleteDataEvent.WhereItem>();
            this.dataGridViewWhere.DataSource = this._wheres;

            ////初始化条件设置表
            //dtWhere = new DataTable();
            //dtWhere.Columns.Add("DataItem");
            //dtWhere.Columns.Add("DataItemName");
            //dtWhere.Columns.Add("Source");
            //dtWhere.Columns.Add("SourceName");
            //dtWhere.Columns.Add("MatchType"); //匹配方式
            //dtWhere.Columns.Add("Warning", typeof(bool));

            ////关联条件设置表
            //this.dataGridViewWhere.AutoGenerateColumns = false;
            //this.dataGridViewWhere.DataSource = this.dtWhere;

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


            this.txtDataEntityName.Title = Language.Current.UserControlEventEditorPanel_DeleteData_DataEntity_LabelDataEntity;

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

                formEventDataItemDataSet.AllowDataSourceType = DeleteDataDev.AllowDataSourceType;

                formEventDataItemDataSet.AllowFormElementControlType = DeleteDataDev.AllowFormElementControlType;

                if (formEventDataItemDataSet.ShowDialog() == DialogResult.OK)
                {
                    DeleteDataEvent.WhereItem where = new DeleteDataEvent.WhereItem()
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

            DeleteDataEvent.WhereItem where = this.dataGridViewWhere.SelectedRows[0].DataBoundItem
               as DeleteDataEvent.WhereItem;
            this._wheres.Remove(where);

            this._warningTable.Remove(where);
        }

        /// <summary>
        /// 标记出警告的行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewWhere_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dr in this.dataGridViewWhere.Rows)
            {
                if (dr != null)
                {
                    DeleteDataEvent.WhereItem where = dr.DataBoundItem as DeleteDataEvent.WhereItem;

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
                return Language.Current.DeleteDataDev_EditorPanel_DataEntity;
            }
        }

        public override List<XObject> GetXml()
        {
            List<XObject> _xObject = new List<XObject>();

            _xObject.Add(new XElement("DataEntityId", this.DataEntityId));

            XElement whereElement = new XElement("Where");
            foreach (DeleteDataEvent.WhereItem where in this._wheres)
            {
                whereElement.Add(XElement.Parse(where.ToXml()));
            }

            _xObject.Add(whereElement);

            return _xObject;
        }

        public override void SetParameter(EventBase even)
        {
            DeleteDataDev _event = even as DeleteDataDev;

            this.DataEntityId = _event.DataEntityId;
            DataEntity dataEntity = _dataEntityComponentService.GetDataEntity(this.DataEntityId);

            this._wheres = new BindingList<DeleteDataEvent.WhereItem>(_event.Where);
            this._warningTable.Clear();

            //为用于显示的DataColumnName和SourceName赋值
            bool warningRow;
            foreach (DeleteDataEvent.WhereItem where in _wheres)
            {
                warningRow = false;

                //DataItemEntityDev
                DataItemEntity dataImteEntity = dataEntity.Items.GetEntityById(where.DataItem);

                if (dataImteEntity == null)
                {
                    where.DataItemName = String.Empty;
                    warningRow = true;
                }
                else
                {
                    where.DataItemName = dataImteEntity.Name;
                }

                where.SourceName = StringParserLogic.DataSourceVisibleString(this.HostAdapter.HostFormEntity, where.Source.ToString(), out warningRow);

                this._warningTable.Add(where, warningRow);
            }

            this.dataGridViewWhere.DataSource = this._wheres;
        }

        #endregion
    }
}
