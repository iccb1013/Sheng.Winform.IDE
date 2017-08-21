using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Controls;
using System.Diagnostics;
using System.Xml.Linq;

namespace Sheng.SailingEase.Core
{
    /// <summary>
    /// 表示列表控件的实体类
    /// </summary>
    [Serializable]
    [NormalUIElementEntityProvideAttribute("FormElementDataListEntity", 0x000066)]
    public class UIElementDataListEntity:UIElement
    {
        #region 受保护的属性

        [NonSerialized]
        private UIElementDataListColumnEntityTypesAbstract _columnEntityTypesAdapter = UIElementDataListColumnEntityTypes.Instance;
        /// <summary>
        /// 数据列类型适配器
        /// 在继承的类中设置此属性，可使FromXml方法初始化相应的类型，如IDE中初始化DEV结尾的类型，SHELL中初始化EX结尾的类型
        /// </summary>
        protected UIElementDataListColumnEntityTypesAbstract ColumnEntityTypesAdapter
        {
            get { return this._columnEntityTypesAdapter; }
            set { this._columnEntityTypesAdapter = value; }
        }

        #endregion

        #region 公开属性

        //private SEPaginationDataGridView.EnumNavigationLocation _navigationLocation =
        //    SEPaginationDataGridView.EnumNavigationLocation.Bottom;
        ///// <summary>
        ///// 分页导航位置
        ///// </summary>
        //[DefaultValue(SEPaginationDataGridView.EnumNavigationLocation.Bottom)]
        //[CorePropertyRelator("FormElementDataListEntity_NavigationLocation", "PropertyCatalog_Pagination", 
        //    Description = "FormElementDataListEntity_NavigationLocation_Description", XmlNodeName = "NavigationLocation")]
        //[PropertyComboBoxEditorAttribute(Enum = typeof(SEPaginationDataGridView.EnumNavigationLocation))]
        //public SEPaginationDataGridView.EnumNavigationLocation NavigationLocation
        //{
        //    get
        //    {
        //        return this._navigationLocation;
        //    }
        //    set
        //    {
        //        this._navigationLocation = value;
        //    }
        //}

        //private bool _pagination = false;
        ///// <summary>
        ///// 是否分页
        ///// </summary>
        //[DefaultValue(false)]
        //[CorePropertyRelator("FormElementDataListEntity_Pagination", "PropertyCatalog_Pagination", 
        //    Description = "FormElementDataListEntity_Pagination_Description", XmlNodeName = "Pagination")]
        //[PropertyBooleanEditorAttribute()]
        //public bool Pagination
        //{
        //    get
        //    {
        //        return this._pagination;
        //    }
        //    set
        //    {
        //        this._pagination = value;
        //    }
        //}

        //private bool _showItemCount = false;
        ///// <summary>
        ///// 是否显示条目数
        ///// </summary>
        //[DefaultValue(false)]
        //[CorePropertyRelator("FormElementDataListEntity_ShowItemCount", "PropertyCatalog_Pagination", 
        //    Description = "FormElementDataListEntity_ShowItemCount_Description", XmlNodeName = "ShowItemCount")]
        //[PropertyBooleanEditorAttribute()]
        //public bool ShowItemCount
        //{
        //    get
        //    {
        //        return this._showItemCount;
        //    }
        //    set
        //    {
        //        this._showItemCount = value;
        //    }
        //}

        //private bool _showPageCount = false;
        ///// <summary>
        ///// 是否显示页数
        ///// </summary>
        //[DefaultValue(false)]
        //[CorePropertyRelator("FormElementDataListEntity_ShowPageCount", "PropertyCatalog_Pagination", 
        //    Description = "FormElementDataListEntity_ShowPageCount_Description", XmlNodeName = "ShowPageCount")]
        //[PropertyBooleanEditorAttribute()]
        //public bool ShowPageCount
        //{
        //    get
        //    {
        //        return this._showPageCount;
        //    }
        //    set
        //    {
        //        this._showPageCount = value;
        //    }
        //}

        //private bool _showPageHomeEnd = false;
        ///// <summary>
        ///// 是否显示首页尾页
        ///// </summary>
        //[DefaultValue(false)]
        //[CorePropertyRelator("FormElementDataListEntity_ShowPageHomeEnd", "PropertyCatalog_Pagination", 
        //    Description = "FormElementDataListEntity_ShowPageHomeEnd_Description", XmlNodeName = "ShowPageHomeEnd")]
        //[PropertyBooleanEditorAttribute()]
        //public bool ShowPageHomeEnd
        //{
        //    get
        //    {
        //        return this._showPageHomeEnd;
        //    }
        //    set
        //    {
        //        this._showPageHomeEnd = value;
        //    }
        //}

        private string _dataEntityId;
        /// <summary>
        /// 关联的数据实体的Id
        /// </summary>
        [CorePropertyRelator("FormElementDataListEntity_DataEntityId", "PropertyCatalog_Normal", 
            Description = "FormElementDataListEntity_DataEntityId_Description", XmlNodeName = "DataEntityId")]
        [PropertyDataItemChooseEditorAttribute(ShowDataItem = false)]
        public string DataEntityId
        {
            get
            {
                return this._dataEntityId;
            }
            set
            {
                this._dataEntityId = value;
            }
        }

        private UIElementDataListColumnEntityCollection _dataColumns;
        /// <summary>
        /// 包含的列
        /// </summary>
        public UIElementDataListColumnEntityCollection DataColumns
        {
            get
            {
                return this._dataColumns;
            }
            set
            {
                this._dataColumns = value;
            }
        }

       

        #endregion

        #region 构造

        public UIElementDataListEntity()
            : base()
        {
            this.DataColumns = new UIElementDataListColumnEntityCollection(this);
        }

        #endregion

        #region 公开方法

        public override string ToXml()
        {
            SEXElement xmlDoc = SEXElement.Parse(base.ToXml());

            xmlDoc.AppendChild(String.Empty, "DataEntity", this.DataEntityId);
            //xmlDoc.AppendChild(XmlRootName, "NavigationLocation", (int)this.NavigationLocation);
            //xmlDoc.AppendChild(String.Empty, "Pagination", this.Pagination);
            //xmlDoc.AppendChild(String.Empty, "ShowItemCount", this.ShowItemCount);
            //xmlDoc.AppendChild(String.Empty, "ShowPageCount", this.ShowPageCount);
            //xmlDoc.AppendChild(String.Empty, "ShowPageHomeEnd", this.ShowPageHomeEnd);

            xmlDoc.AppendChild("Columns");
            
            foreach (UIElementDataListColumnEntityAbstract column in this.DataColumns)
            {
                xmlDoc.AppendInnerXml("/Columns", column.ToXml());
            }

            return xmlDoc.ToString();
        }

        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);

            SEXElement xmlDoc = SEXElement.Parse(strXml);

            this.DataEntityId = xmlDoc.GetInnerObject("/DataEntity");
            //this.NavigationLocation = 
            //    (SEPaginationDataGridView.EnumNavigationLocation)xmlDoc.GetInnerObject<int>(XmlRootName + "/NavigationLocation",0);
            //this.Pagination = xmlDoc.GetInnerObject<bool>("/Pagination", false);
            //this.ShowItemCount = xmlDoc.GetInnerObject<bool>("/ShowItemCount", false);
            //this.ShowPageCount = xmlDoc.GetInnerObject<bool>("/ShowPageCount", false);
            //this.ShowPageHomeEnd = xmlDoc.GetInnerObject<bool>("/ShowPageHomeEnd", false);

            //添加列对象
            foreach (XElement node in xmlDoc.SelectNodes("/Columns/Column"))
            {
                UIElementDataListColumnEntityAbstract formElementDataColumnEntity =
                    ColumnEntityTypesAdapter.CreateInstance(Convert.ToInt32(node.Attribute("ColumnType").Value));
                formElementDataColumnEntity.FromXml(node.ToString());
                this.DataColumns.Add(formElementDataColumnEntity);
            }

        }

        /// <summary>
        /// 获取其中的列实体对象
        /// 可能为null
        /// 
        /// 原Dev结尾对象中的 GetDataColumnDev
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UIElementDataListColumnEntityAbstract GetDataColumn(string id)
        {
            return this.DataColumns.GetEntityById(id);
        }

        //TODO:好像这里是个BUG，转换不能，待测试确认
        public override UIElementCollection GetInnerElement()
        {
            return (UIElementCollection)this.DataColumns;
        }

        /// <summary>
        /// 允许shell中重写，以返回Ex结尾对象
        /// </summary>
        /// <returns></returns>
        public virtual UIElementDataListRowEntity NewRow()
        {
            UIElementDataListRowEntity row = new UIElementDataListRowEntity(this);
            return row;
        }

        #endregion

        #region IEventSupport 成员

        private static UIElementDataListEventTimes _eventTimes;
        public override List<EventTimeAbstract> EventTimeProvide
        {
            get
            {
                if (_eventTimes == null)
                {
                    _eventTimes = new UIElementDataListEventTimes();
                }

                return _eventTimes.Times;
            }
        }

        public override string GetEventTimeName(int code)
        {
            if (_eventTimes == null)
            {
                _eventTimes = new UIElementDataListEventTimes();
            }

            return _eventTimes.GetEventName(code);
        }

        #endregion
    }
}
