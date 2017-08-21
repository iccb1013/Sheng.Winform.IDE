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
using Sheng.SailingEase.Controls.Extensions;
namespace Sheng.SailingEase.Shell.View
{
    class MainToolStripPanel : ToolStripPanelView
    {
        protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(
                 new Rectangle(0, 0, this.Width > 0 ? this.Width : 1, this.Height > 0 ? this.Height : 1), Color.White, Color.FromArgb(244, 247, 252), LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }
    }
}
