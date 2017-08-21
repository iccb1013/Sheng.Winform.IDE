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
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    public class ToolboxTabButton : Button
    {
        Pen selectedBorderPen = new Pen(Color.Black);
        Pen unSelectedBorderPen = new Pen(Color.Gray);
        LinearGradientBrush selectedBrush;
        LinearGradientBrush unSelectedBrush;
        bool mouseHover = false;
        private bool _selected = false;
        public bool Selected
        {
            get
            {
                return this._selected;
            }
            set
            {
                this._selected = value;
            }
        }
        public ToolboxItemContainer ItemContainer
        {
            get;
            set;
        }
        public ToolboxTabButton()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }
        protected override void OnEnter(EventArgs e)
        {
            this.Selected = true;
            base.OnEnter(e);
        }
        protected override void OnLeave(EventArgs e)
        {
            this.Selected = false;
            base.OnLeave(e);
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            this.mouseHover = true;
            this.Refresh();
            base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            this.mouseHover = false;
            this.Refresh();
            base.OnMouseLeave(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.Selected)
            {
                selectedBrush = new LinearGradientBrush(this.ClientRectangle, Color.Gainsboro, Color.GhostWhite, LinearGradientMode.Vertical);
                e.Graphics.FillRectangle(selectedBrush, ClientRectangle);
                e.Graphics.DrawRectangle(selectedBorderPen, 0, 0, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
                selectedBrush.Dispose();
            }
            else
            {
                unSelectedBrush = new LinearGradientBrush(this.ClientRectangle, Color.Gainsboro, Color.GhostWhite, LinearGradientMode.Vertical);
                if (mouseHover)
                {
                    e.Graphics.FillRectangle(unSelectedBrush, ClientRectangle);
                    e.Graphics.DrawRectangle(unSelectedBorderPen, 0, 0, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
                }
                else
                {
                    e.Graphics.FillRectangle(unSelectedBrush, ClientRectangle);
                }
                unSelectedBrush.Dispose();
            }
            Rectangle bitmapBounds = new Rectangle(ClientRectangle.Location.X+2, 
                ClientRectangle.Location.Y + ClientRectangle.Height / 2 - 9 / 2,
                9, 9);
            if (this.ItemContainer != null)
            {
                if (this.ItemContainer.Visible)
                {
                    e.Graphics.DrawImage(IconsLibrary.Minus2, bitmapBounds);
                }
                else
                {
                    e.Graphics.DrawImage(IconsLibrary.Plus2, bitmapBounds);
                }
            }
            Rectangle stringBounds = new Rectangle(ClientRectangle.Location.X + 16,
                ClientRectangle.Location.Y, ClientRectangle.Width - 15, ClientRectangle.Height);
            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Near;
            format.FormatFlags = StringFormatFlags.NoWrap;
            e.Graphics.DrawString(this.Text, new Font("Tahoma", 11, FontStyle.Regular, GraphicsUnit.World), Brushes.Black, stringBounds, format);
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.ItemContainer.Visible = !this.ItemContainer.Visible;
            this.Invalidate();
        }
    }
}
