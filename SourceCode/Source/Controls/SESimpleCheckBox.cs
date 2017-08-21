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
    public class SESimpleCheckBox:Control
    {
        private Graphics graphics;
        private bool check = false;
        public bool Check
        {
            get
            {
                return this.check;
            }
            set
            {
                this.check = value;
            }
        }
        public SESimpleCheckBox()
        {
            LicenseManager.Validate(typeof(SESimpleCheckBox)); 
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.Selectable, true);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            graphics = e.Graphics;
            if (this.Check)
            {
                graphics.Clear(Color.SkyBlue);
            }
            else
            {
                graphics.Clear(Color.White);
            }
            graphics.DrawRectangle(Pens.Black, 0, 0, this.Width - 1, this.Height - 1);
            if (this.ShowFocusCues && this.Focused)
            {
                ControlPaint.DrawFocusRectangle(graphics,new Rectangle(2,2,this.Width-4,this.Height-4));
            }
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.Select();
            this.Check = !this.Check;
            this.Invalidate();
        }
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            this.Invalidate();
        }
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            this.Invalidate();
        }
    }
}
