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
using System.Drawing.Drawing2D;
using System.ComponentModel;
namespace Sheng.SailingEase.Controls
{
    [LicenseProvider(typeof(SEControlLicenseProvider))]
    public class SEToolStripSeparator : ToolStripSeparator
    {
        private bool defaultPaint = false;
        public bool DefaultPaint
        {
            get
            {
                return this.defaultPaint;
            }
            set
            {
                this.defaultPaint = value;
            }
        }
        public SEToolStripSeparator()
        {
            LicenseManager.Validate(typeof(SEToolStripSeparator)); 
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.defaultPaint)
            {
                base.OnPaint(e);
                return;
            }
            SolidBrush backBrush_Normal = new SolidBrush(SystemColors.ControlLightLight);
            Rectangle fillRect = new Rectangle(0, 0, this.Bounds.Width, this.Bounds.Height);
            LinearGradientBrush leftBrush_DropDown = new LinearGradientBrush(new Point(0, 0), new Point(23, 0),
                        Color.White, Color.FromArgb(233, 230, 215));
            Pen leftLine = new Pen(Color.FromArgb(197, 194, 184));
            e.Graphics.FillRectangle(backBrush_Normal, fillRect);
            e.Graphics.FillRectangle(leftBrush_DropDown, 0, 0, 23, this.Height);
            e.Graphics.DrawLine(leftLine, 23, 0, 23, this.Height);
            int lineY = (int)Math.Round((double)(this.ContentRectangle.Height / 2));;
            e.Graphics.DrawLine(leftLine, 25, lineY, this.Width - 2, lineY);
        }
    }
}
