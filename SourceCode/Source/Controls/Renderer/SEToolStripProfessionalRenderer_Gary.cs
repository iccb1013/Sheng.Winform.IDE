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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace Sheng.SailingEase.Controls
{
    public class SEToolStripProfessionalRenderer_Gary : ToolStripProfessionalRenderer
    {
        static SEToolStripProfessionalRenderer_Gary()
        {
        }
        public SEToolStripProfessionalRenderer_Gary()
        {
        }
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.FromArgb(163, 163, 124)), 0, e.ToolStrip.Height, e.ToolStrip.Width, e.ToolStrip.Height);
        }
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip.GetType().Name == "ToolStrip")
            {
                LinearGradientBrush brush = new LinearGradientBrush
                    (new Point(0, 0), new Point(0, e.ToolStrip.Height), Color.White, Color.FromArgb(230, 225, 202));
                e.Graphics.FillRectangle(brush, new Rectangle(0, 0, e.ToolStrip.Width, e.ToolStrip.Height));
                brush.Dispose();
            }
            else
            {
                base.OnRenderToolStripBackground(e);
            }
        }
    }
}
