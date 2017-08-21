using System;
using System.Collections;
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
    partial class UserControlEventEditorPanel_DataListDeleteRow_Where : UserControlEventEditorPanelBase
    {
        #region 私有成员

        private BindingList<DataListDeleteRowDev.WhereItem> _wheres;

        /// <summary>
        /// 用于记录条件部分哪些条件项目产生警告
        /// DataGridView据此来决定哪些条目需要醒目显示
        /// </summary>
        private Dictionary<DataListDeleteRowDev.WhereItem, bool> _warningTable = new Dictionary<DataListDeleteRowEvent.WhereItem, bool>();

        #endregion

        #region 公开属性

        /// <summary>
        /// 调用者窗体
        /// </summary>
        public EnumTargetWindow TargetWindow
        {
            get;
            set;
        }

        private string _dataListId;
        /// <summary>
        /// 要操作的数据列表的Id
        /// </summary>
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

        ////TODO:否决
        //private DataTable dtDataSet;
        ///// <summary>
        ///// 接收数据设置
        ///// </summary>
        //public DataTable DataSet
        //{
        //    get
        //    {
        //        return this.dtDataSet;
        //    }
        //    set
        //    {
        //        this.dtDataSet = value;
        //    }
        //}

        #endregion

        #region 构造

        public UserControlEventEditorPanel_DataListDeleteRow_Where(EventEditorAdapterAbstract hostAdapter)
            : base(hostAdapter)
        {
            InitializeComponent();

            UIHelper.ProcessDataGridView(this.dataGridViewDataSet);

            Unity.ApplyResource(this);
            ApplyLanguageResource();

            this.dataGridViewDataSet.AutoGenerateColumns = false;

            this._wheres = new BindingList<DataListDeleteRowEvent.WhereItem>();
            this.dataGridViewDataSet.DataSource = this._wheres;
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
        /// 点击添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            DataListDataItemEditView view = new DataListDataItemEditView();
            view.WindowEntity = this.HostAdapter.HostFormEntity;
            view.DataListEntity = this.HostAdapter.HostFormEntity.FindFormElementById(this.DataListId) as UIElementDataListEntity;
            if (view.ShowDialog() == DialogResult.OK)
            {
                DataListDeleteRowEvent.WhereItem data = new DataListDeleteRowEvent.WhereItem()
                {
                    DataColumn = view.DataColumn,
                    DataColumnName = view.DataColumnName,
                    Source = view.DataSource,
                    SourceName = view.DataSourceName
                };

                this._wheres.Add(data);
            }
            view.Dispose();


            //using (FormEventDataListDataSet formEventDataListOperatorDataSetSet =
            //    new FormEventDataListDataSet(this.HostAdapter.HostFormEntity))
            //{

            //    formEventDataListOperatorDataSetSet.AllowDataSourceType = DataListDeleteRowDev.AllowDataDataSourceType;
            //    formEventDataListOperatorDataSetSet.AllowFormElementControlType = DataListDeleteRowDev.AllowFormElementControlType;

            //    if (this.TargetForm == EnumTargetWindow.Current)
            //        formEventDataListOperatorDataSetSet.DataListId = this.DataListId;

            //    if (formEventDataListOperatorDataSetSet.ShowDialog() == DialogResult.OK)
            //    {
            //        DataListDeleteRowEvent.WhereItem where = new DataListDeleteRowEvent.WhereItem()
            //        {
            //            DataColumn = formEventDataListOperatorDataSetSet.DataColumn,
            //            DataColumnName = formEventDataListOperatorDataSetSet.DataColumnName,
            //            Source = new DataSource(formEventDataListOperatorDataSetSet.SelectedDataSourceString),
            //            SourceName = formEventDataListOperatorDataSetSet.SelectedDataSourceVisibleString
            //        };

            //        this._wheres.Add(where);
            //    }
            //}
        }

        /// <summary>
        /// 点击删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewDataSet.SelectedRows.Count != 1)
                return;

            DataListDeleteRowEvent.WhereItem where = this.dataGridViewDataSet.SelectedRows[0].DataBoundItem
                as DataListDeleteRowEvent.WhereItem;
            this._wheres.Remove(where);

            this._warningTable.Remove(where);
        }

        private void dataGridViewDataSet_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dr in this.dataGridViewDataSet.Rows)
            {
                if (dr != null)
                {
                    DataListDeleteRowEvent.WhereItem where = dr.DataBoundItem as DataListDeleteRowEvent.WhereItem;

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
                return Language.Current.DataListDeleteRowDev_EditorPanel_Where;
            }
        }

        public override List<XObject> GetXml()
        {
            List<XObject> _xObject = new List<XObject>();

            _xObject.Add(new XElement("DataList", this.DataListId));
            _xObject.Add(new XElement("TargetWindow", (int)this.TargetWindow));

            XElement whereElement = new XElement("Where");
            foreach (DataListDeleteRowDev.WhereItem where in this._wheres)
            {
                whereElement.Add(XElement.Parse(where.ToXml()));
            }

            _xObject.Add(whereElement);

            return _xObject;
        }

        public override void SetParameter(EventBase even)
        {
            DataListDeleteRowDev _event = even as DataListDeleteRowDev;

            this.DataListId = _event.DataList;

            this.TargetWindow = _event.TargetWindow;

            this._wheres = new BindingList<DataListDeleteRowEvent.WhereItem>(_event.Where);
            this._warningTable.Clear();

            //为用于显示的DataColumnName和SourceName赋值
            bool warningRow = false;
            DataSourceProvideArgs args = new DataSourceProvideArgs()
            {
                WindowEntity = this.HostAdapter.HostFormEntity
            };
            foreach (DataListDeleteRowEvent.WhereItem where in _wheres)
            {
                //where.SourceName = StringParserLogic.DataSourceVisibleString(this.HostAdapter.HostFormEntity, where.Source.ToString(), out warningRow);
                where.SourceName = DataSourceProvideFactory.Instance.GetDisplayString(where.Source, args);
                if (String.IsNullOrEmpty(where.SourceName))
                {
                    warningRow = true;
                }
                
                //如果是当前窗体
                if (this.TargetWindow == EnumTargetWindow.Current)
                {
                    UIElement formElementDataList = this.HostAdapter.HostFormEntity.FindFormElementById(_event.DataList);
                    if (formElementDataList == null)
                    {
                        warningRow = true;
                    }
                    else
                    {
                        //FormElementDataListEntityDev
                        UIElementDataListEntity dataList = (UIElementDataListEntity)formElementDataList;
                        UIElementDataListColumnEntityAbstract dataColumn = dataList.GetDataColumn(where.DataColumn);

                        if (dataColumn == null)
                            warningRow = true;
                        else
                            where.DataColumnName = dataColumn.Name;
                    }
                }
                //如果是调用者窗体
                else
                {
                    where.DataColumnName = where.DataColumn;
                }

                this._warningTable.Add(where, warningRow);
            }

            this.dataGridViewDataSet.DataSource = this._wheres;
        }

        #endregion
    }
}
