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
using System.Drawing;
using System.Windows.Forms.VisualStyles;
namespace Sheng.SailingEase.Controls
{
    class DataGridViewCheckBoxCellRenderer : IDataGridViewCellRenderer
    {
        private Type _renderCellType = typeof(DataGridViewCheckBoxCell);
        public Type RenderCellType
        {
            get { return _renderCellType; }
        }
        public void Paint(System.Drawing.Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates elementState, object value, object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle)
        {
            CheckBoxState checkBoxState = CheckBoxState.UncheckedDisabled;
            if (value is bool && Convert.ToBoolean(value))
            {
                checkBoxState = CheckBoxState.CheckedDisabled;
            }
            Size checkBoxSize = CheckBoxRenderer.GetGlyphSize(graphics, checkBoxState);
            Point drawInPoint = new Point(cellBounds.X + cellBounds.Width / 2 - checkBoxSize.Width / 2,
                cellBounds.Y + cellBounds.Height / 2 - checkBoxSize.Height / 2);
            CheckBoxRenderer.DrawCheckBox(graphics, drawInPoint, checkBoxState);
        }
    }
}
