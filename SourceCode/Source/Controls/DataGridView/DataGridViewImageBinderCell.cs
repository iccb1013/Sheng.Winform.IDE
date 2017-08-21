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
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
namespace Sheng.SailingEase.Controls
{
    public class DataGridViewImageBinderCell : DataGridViewImageCell
    {
        public DataGridViewImageBinderCell()
        {
        }
        protected override object GetValue(int rowIndex)
        {
            if (rowIndex == -1)
                return base.GetValue(rowIndex);
            DataGridViewImageBinderColumn column = this.OwningColumn as DataGridViewImageBinderColumn;
            Debug.Assert(column != null, "column 为 null");
            if (column == null)
            {
                return base.GetValue(rowIndex);
            }
            
            DataGridViewRow row = this.OwningRow;
            object value = null;
            if (row == row.DataGridView.Rows[rowIndex])
            {
                Debug.Assert(row.DataBoundItem != null, "row.DataBoundItem 为 null");
                value = row.DataBoundItem;
            }
            else
            {
                value = row.DataGridView.Rows[rowIndex].DataBoundItem;
                Debug.Assert(value != null, "value 为 null");
            }
            if (value == null)
                return null;
            return column.GetImage(value);
        }
    }
}
