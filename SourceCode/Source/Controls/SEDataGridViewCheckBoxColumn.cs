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
using System.ComponentModel;
namespace Sheng.SailingEase.Controls
{
    [LicenseProvider(typeof(SEControlLicenseProvider))]
    public class SEDataGridViewCheckBoxColumn : DataGridViewCheckBoxColumn
    {
        public SEDataGridViewCheckBoxColumn()
        {
            LicenseManager.Validate(typeof(SEDataGridViewCheckBoxColumn)); 
            this.CellTemplate = new SEDataGridViewCheckBoxCell();
        }
    }
    [LicenseProvider(typeof(SEControlLicenseProvider))]
    public class SEDataGridViewCheckBoxCell : DataGridViewCheckBoxCell
    {
        public override object Clone()
        {
            SEDataGridViewCheckBoxCell cell =
                (SEDataGridViewCheckBoxCell)base.Clone();
            return cell;
        }
        public SEDataGridViewCheckBoxCell()
        {
            LicenseManager.Validate(typeof(SEDataGridViewCheckBoxCell)); 
        }
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds,
            int rowIndex, DataGridViewElementStates elementState, object value,
            object formattedValue, string errorText, DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            SolidBrush cellBackground;
            if ((elementState & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
                cellBackground = new SolidBrush(cellStyle.SelectionBackColor);
            else
                cellBackground = new SolidBrush(cellStyle.BackColor);
            graphics.FillRectangle(cellBackground, cellBounds);
            cellBackground.Dispose();
            PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
            Point drawInPoint = new Point(cellBounds.X + cellBounds.Width / 2 - 7, cellBounds.Y + cellBounds.Height / 2 - 7);
            if (Convert.ToBoolean(value))
                CheckBoxRenderer.DrawCheckBox(graphics, drawInPoint, System.Windows.Forms.VisualStyles.CheckBoxState.CheckedDisabled);
            else
                CheckBoxRenderer.DrawCheckBox(graphics, drawInPoint, System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedDisabled);
            if (this.DataGridView.CurrentCell == this
                && (paintParts & DataGridViewPaintParts.Focus) == DataGridViewPaintParts.Focus)
            {
                ControlPaint.DrawFocusRectangle(graphics, cellBounds);
            }
        }
    }
}
