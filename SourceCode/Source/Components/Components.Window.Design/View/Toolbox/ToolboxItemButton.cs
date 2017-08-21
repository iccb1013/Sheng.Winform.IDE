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
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    public class ToolboxItemButton : Button
    {
        Pen selectedBorderPen = new Pen(SystemColors.ActiveCaption);
        Pen unSelectedBorderPen = new Pen(Color.Gray);
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
        private int _index = 0;
        public int Index
        {
            get
            {
                return this._index;
            }
            set
            {
                this._index = value;
            }
        }
        private System.Drawing.Design.ToolboxItem _toolboxItem;
        public System.Drawing.Design.ToolboxItem ToolboxItem
        {
            get
            {
                return this._toolboxItem;
            }
        }
        private string _displayName;
        public ToolboxItemButton(System.Drawing.Design.ToolboxItem toolboxItem, string displayName)
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this._displayName = displayName;
            this._toolboxItem = toolboxItem;
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
                e.Graphics.Clear(SystemColors.ControlLightLight);
                e.Graphics.DrawRectangle(selectedBorderPen, 0, 0, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
            }
            else
            {
                if (mouseHover)
                {
                    e.Graphics.Clear(SystemColors.Control);
                    e.Graphics.DrawRectangle(unSelectedBorderPen, 0, 0, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
                }
                else
                {
                    e.Graphics.Clear(SystemColors.Control);
                }
            }
            Rectangle bitmapBounds = new Rectangle(ClientRectangle.Location.X +5, ClientRectangle.Location.Y + ClientRectangle.Height / 2 - ToolboxItem.Bitmap.Height / 2,
                ToolboxItem.Bitmap.Width, ToolboxItem.Bitmap.Height);
            Rectangle stringBounds = new Rectangle(ClientRectangle.Location.X + bitmapBounds.Width + 10,
                ClientRectangle.Location.Y, ClientRectangle.Width - bitmapBounds.Width, ClientRectangle.Height);
            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Near;
            format.FormatFlags = StringFormatFlags.NoWrap;
            e.Graphics.DrawImage(ToolboxItem.Bitmap, bitmapBounds);
            e.Graphics.DrawString(this._displayName, new Font("Tahoma", 11, FontStyle.Regular, GraphicsUnit.World), Brushes.Black, stringBounds, format);
        }
    }
}
