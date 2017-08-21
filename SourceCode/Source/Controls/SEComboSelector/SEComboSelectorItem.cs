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
using System.Reflection;
using System.Diagnostics;
namespace Sheng.SailingEase.Controls
{
    public class SEComboSelectorItem
    {
        private Point _titlePoint = new Point(10, 3);
        private Point _descriptionPoint = new Point(10, 20);
        private Font _titleFont = new Font(SystemFonts.DefaultFont.Name, SystemFonts.DefaultFont.Size, FontStyle.Bold);
        private Font _descriptionFont = new Font(SystemFonts.DefaultFont.Name, SystemFonts.DefaultFont.Size);
        private TextFormatFlags textFlags = TextFormatFlags.SingleLine | TextFormatFlags.WordEllipsis;
        private Rectangle DrawAreaRectangle
        {
            get
            {
                return this.ItemContainer.GetItemRectangle(this.Index);
            }
        }
        private Rectangle TitleStringAreaRectangle
        {
            get
            {
                Rectangle r = new Rectangle();
                Point locationPoint = DrawAreaRectangle.Location;
                locationPoint.Offset(this._titlePoint);
                r.Location = locationPoint;  
                r.Width = DrawAreaRectangle.Width - r.Left;
                r.Height = this._titleFont.Height;
                return r;
            }
        }
        private Rectangle DescriptionStringAreaRectangle
        {
            get
            {
                Rectangle r = new Rectangle();
                Point descriptionPoint = DrawAreaRectangle.Location;
                descriptionPoint.Offset(new Point(10, TitleStringAreaRectangle.Height + 7));
                r.Location = descriptionPoint;
                r.Width = DrawAreaRectangle.Width - r.Left;
                r.Height = this._descriptionFont.Height;
                return r;
            }
        }
        public object Value
        {
            get;
            set;
        }
        public int Index
        {
            get
            {
                if (this.ItemContainer == null)
                    return -1;
                return this.ItemContainer.Items.IndexOf(this);
            }
        }
        public SEComboSelectorItemContainer ItemContainer
        {
            get;
            set;
        }
        private bool _selected = false;
        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                ItemContainer.DrawItem(this.Index);
            }
        }
        public SEComboSelectorItem(object value)
        {
            Value = value;
        }
        public void DrawItem(Graphics g)
        {
            if (ItemContainer.InDisplayRange(this.Index) == false)
                return;
            Rectangle clientRect = this.ItemContainer.GetItemRectangle(this.Index);
            SolidBrush backBrush;
            Pen borderPen;
            if (this.Selected)
            {
                backBrush = new SolidBrush(ItemContainer.SelectedColor);
                borderPen = new Pen(ItemContainer.SelectedBorderColor);
            }
            else if (this.ItemContainer.HotedItem == this)
            {
                backBrush = new SolidBrush(ItemContainer.FocusColor);
                borderPen = new Pen(ItemContainer.FocusBorderColor);
            }
            else
            {
                backBrush = new SolidBrush(ItemContainer.BackgroundColor);
                borderPen = new Pen(ItemContainer.BorderColor);
            }
            g.FillRectangle(backBrush, clientRect);
            g.DrawRectangle(borderPen, clientRect);
            backBrush.Dispose();
            borderPen.Dispose();
            string text = String.Empty;
            if (String.IsNullOrEmpty(ItemContainer.DisplayMember) == false)
            {
                if (ItemContainer.DisplayPropertyInfo != null)
                {
                    object textObj = ItemContainer.DisplayPropertyInfo.GetValue(this.Value, null);
                    if (textObj != null)
                        text = textObj.ToString();
                }
            }
            else
            {
                text = this.Value.ToString();
            }
            Color textColor;
            if (this.Selected)
            {
                textColor = ItemContainer.SelectedTextColor;
            }
            else if (this.ItemContainer.HotedItem == this)
            {
                textColor = ItemContainer.FocusTextColor;
            }
            else
            {
                textColor = ItemContainer.TextColor;
            }
            TextRenderer.DrawText(g, text, _titleFont, TitleStringAreaRectangle, textColor, textFlags);
            string description = String.Empty;
            if (String.IsNullOrEmpty(ItemContainer.DescriptionMember) == false)
            {
                if (ItemContainer.DiscriptionPropertyInfo != null)
                {
                    object descriptionObj = ItemContainer.DiscriptionPropertyInfo.GetValue(this.Value, null);
                    if (descriptionObj != null)
                        description = descriptionObj.ToString();
                }
            }
            if (String.IsNullOrEmpty(description) == false)
            {
                Color descriptionColor;
                if (this.Selected)
                {
                    descriptionColor = ItemContainer.SelectedDescriptionColor;
                }
                else if (this.ItemContainer.HotedItem == this)
                {
                    descriptionColor = ItemContainer.FocusDescriptionColor;
                }
                else
                {
                    descriptionColor = ItemContainer.DescriptionColor;
                }
                TextRenderer.DrawText(g, description, _descriptionFont, DescriptionStringAreaRectangle, descriptionColor, textFlags);
            }
            g.Dispose();
        }
    }
}
