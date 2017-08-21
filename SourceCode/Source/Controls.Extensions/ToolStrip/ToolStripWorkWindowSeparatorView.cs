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
using System.Drawing.Drawing2D;
using System.Drawing;
namespace Sheng.SailingEase.Controls.Extensions
{
    public class ToolStripWorkWindowSeparatorView : System.Windows.Forms.ToolStripSeparator, IToolStripItemView
    {
        public ToolStripWorkWindowSeparatorView()
        {
        }
        private IToolStripItemCodon _codon;
        public IToolStripItemCodon Codon
        {
            get { return _codon; }
            set
            {
                _codon = value;
                ToolStripItemViewHelper.Update(this, _codon);
            }
        }
        public void UpdateSataus()
        {
            ToolStripItemViewHelper.UpdateStatus(this);
        }
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            Rectangle lineRect;
            ToolStripWorkWindowSeparatorCodon<ToolStripWorkWindowSeparatorView> codon =
                this.Codon as ToolStripWorkWindowSeparatorCodon<ToolStripWorkWindowSeparatorView>;
            if (codon.PaintAlignment == ToolStripWorkWindowSeparatorCodon < ToolStripWorkWindowSeparatorView >.EnumPaintAlignment.Left)
                lineRect = new Rectangle(0, 0, 1, this.Height);
            else
                lineRect = new Rectangle(this.Width - 1, 0, 1, this.Height);
            LinearGradientBrush line = new LinearGradientBrush(lineRect,
                        SystemColors.Control, Color.FromArgb(198, 198, 0), LinearGradientMode.Vertical);
            e.Graphics.FillRectangle(line, lineRect);
        }
    }
}
