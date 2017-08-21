/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
/*
 * Styles - A style is a group of formatting attributes that can be applied to a row or a cell
 * Author: Phillip Piper
 * Date: 29/07/2009 23:09
 * Change log:
 * 2009-08-15  JPP  - Added Decoration and Overlay properties to HotItemStyle
 * 2009-07-29  JPP  - Initial version
 * To do:
 * - These should be more generally available. It should be possible to do something like this:
 *       this.olv.GetItem(i).Style = new ItemStyle();
 *       this.olv.GetItem(i).GetSubItem(j).Style = new CellStyle();
 * Copyright (C) 2009 Phillip Piper
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http:
 * If you wish to use this code in a closed source application, please contact phillip_piper@bigfoot.com.
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace Sheng.SailingEase.Controls.ObjectListView
{
    public interface IItemStyle
    {
        Font Font { get; set; }
        FontStyle FontStyle { get; set; }
        Color ForeColor { get; set; }
        Color BackColor { get; set; }
    }
    public class HotItemStyle : System.ComponentModel.Component, IItemStyle
    {
        public Font Font {
            get { return this.font; }
            set { this.font = value; }
        }
        private Font font;
        [DefaultValue(FontStyle.Regular)]
        public FontStyle FontStyle {
            get { return this.fontStyle; }
            set { this.fontStyle = value; }
        }
        private FontStyle fontStyle;
        [DefaultValue(typeof(Color), "")]
        public Color ForeColor {
            get { return this.foreColor; }
            set { this.foreColor = value; }
        }
        private Color foreColor;
        [DefaultValue(typeof(Color), "")]
        public Color BackColor {
            get { return this.backColor; }
            set { this.backColor = value; }
        }
        private Color backColor;
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IOverlay Overlay {
            get { return this.overlay; }
            set { this.overlay = value; }
        }
        private IOverlay overlay;
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IDecoration Decoration {
            get { return this.decoration; }
            set { this.decoration = value; }
        }
        private IDecoration decoration;
    }
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class CellStyle : IItemStyle
    {
        public Font Font {
            get { return this.font; }
            set { this.font = value; }
        }
        private Font font;
        [DefaultValue(FontStyle.Regular)]
        public FontStyle FontStyle {
            get { return this.fontStyle; }
            set { this.fontStyle = value; }
        }
        private FontStyle fontStyle;
        [DefaultValue(typeof(Color), "")]
        public Color ForeColor {
            get { return this.foreColor; }
            set { this.foreColor = value; }
        }
        private Color foreColor;
        [DefaultValue(typeof(Color), "")]
        public Color BackColor {
            get { return this.backColor; }
            set { this.backColor = value; }
        }
        private Color backColor;
    }
    public class HyperlinkStyle : System.ComponentModel.Component
    {
        public HyperlinkStyle() {
            this.Normal = new CellStyle();
            this.Normal.ForeColor = Color.Blue;
            this.Over = new CellStyle();
            this.Over.FontStyle = FontStyle.Underline;
            this.Visited = new CellStyle();
            this.Visited.ForeColor = Color.Purple;
            this.OverCursor = Cursors.Hand;
        }
        [Category("Appearance"),
         Description("How should hyperlinks be drawn")]
        public CellStyle Normal {
            get { return this.normalStyle; }
            set { this.normalStyle = value; }
        }
        private CellStyle normalStyle;
        [Category("Appearance"),
         Description("How should hyperlinks be drawn when the mouse is over them?")]
        public CellStyle Over {
            get { return this.overStyle; }
            set { this.overStyle = value; }
        }
        private CellStyle overStyle;
        [Category("Appearance"),
         Description("How should hyperlinks be drawn after they have been clicked")]
        public CellStyle Visited {
            get { return this.visitedStyle; }
            set { this.visitedStyle = value; }
        }
        private CellStyle visitedStyle;
        [Category("Appearance"),
         Description("What cursor should be shown when the mouse is over a link?")]
        public Cursor OverCursor {
            get { return this.overCursor; }
            set { this.overCursor = value; }
        }
        private Cursor overCursor;
    }
}
