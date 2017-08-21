/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Sheng.SailingEase.Core
{
    [Serializable]
    public class UIElementDataListRowEntity
    {
        private UIElementDataListRowCellEntityCollection _cells = new UIElementDataListRowCellEntityCollection();
        public UIElementDataListRowCellEntityCollection Cells
        {
            get { return this._cells; }
            set { this._cells = value; }
        }
        public object[] Values
        {
            get
            {
                object[] values = new object[Cells.Count];
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = Cells[i].Value;
                }
                return values;
            }
        }
        private UIElementDataListEntity _dataList;
        public UIElementDataListEntity DataList
        {
            get { return this._dataList; }
            set { this._dataList = value; }
        }
        public UIElementDataListRowEntity()
        {
        }
        public UIElementDataListRowEntity(UIElementDataListEntity dataList)
        {
            DataList = dataList;
            if (dataList != null)
            {
                foreach (UIElementDataListColumnEntityAbstract column in dataList.DataColumns)
                {
                    Cells.Add(new UIElementDataListRowCellEntity(this, column));
                }
            }
        }
        public virtual void SetValues(object[] values)
        {
            for (int i = 0; i < Cells.Count; i++)
            {
                Cells[i].Value = values[i];
            }
        }
        public void SetValue(string column, object value)
        {
            this.Cells[column].Value = value;
        }
    }
}
