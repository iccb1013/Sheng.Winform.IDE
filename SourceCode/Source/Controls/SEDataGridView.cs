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
using System.ComponentModel;
using System.Drawing;
namespace Sheng.SailingEase.Controls
{
    [LicenseProvider(typeof(SEControlLicenseProvider))]
    public class SEDataGridView : DataGridView
    {
        private string waterText = String.Empty;
        [Description("水印文本")]
        [Category("SEControl")]
        public string WaterText
        {
            get { return this.waterText; }
            set
            {
                this.waterText = value;
                this.Invalidate();
            }
        }
        public SEDataGridView()
        {
            LicenseManager.Validate(typeof(SEDataGridView));
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.ResizeRedraw = true;
            DataGridViewRenderer renderer = new DataGridViewRenderer(this);
        }
        protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                /*
                 * 注意一旦设置了CurrentCell属性
                 * 就会失去之前已经选中的行,所以先获取选中的行到selectedRows
                 */
                if (this.Rows[e.RowIndex].Selected)
                    return;
				if (e.ColumnIndex < 0 || e.RowIndex < 0)
                    return;
                DataGridViewSelectedRowCollection selectedRows = this.SelectedRows;
                this.CurrentCell = this[e.ColumnIndex, e.RowIndex];
                if (Control.ModifierKeys != Keys.Control && Control.ModifierKeys != Keys.Shift)
                {
                    foreach (DataGridViewRow row in selectedRows)
                        row.Selected = false;
                }
                else
                {
                    if (this.MultiSelect)
                        foreach (DataGridViewRow row in selectedRows)
                            row.Selected = true;
                }
                this.Rows[e.RowIndex].Selected = true;
            }
            base.OnCellMouseDown(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (this.Rows.Count == 0 && (this.waterText != null || this.waterText != String.Empty))
            {
                PaintWaterText();
            }
        }
        private TextFormatFlags textFlags = TextFormatFlags.WordBreak | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
        private Rectangle DrawStringRectangle
        {
            get
            {
                if (this.ClientRectangle == new Rectangle())
                {
                    return new Rectangle(0, 0, 1, 1);
                }
                Rectangle drawStringRectangle;
                drawStringRectangle = this.ClientRectangle;
                drawStringRectangle.X = drawStringRectangle.X + this.Padding.Left;
                drawStringRectangle.Y = drawStringRectangle.Y + this.Padding.Top + this.ColumnHeadersHeight;
                drawStringRectangle.Width = drawStringRectangle.Width - this.Padding.Left - this.Padding.Right;
                drawStringRectangle.Height = 50;
                return drawStringRectangle;
            }
        }
        private void PaintWaterText()
        {
            Graphics g = this.CreateGraphics();
            TextRenderer.DrawText(g, this.waterText, this.Font, this.DrawStringRectangle, this.ForeColor, textFlags);
        }
    }
}
