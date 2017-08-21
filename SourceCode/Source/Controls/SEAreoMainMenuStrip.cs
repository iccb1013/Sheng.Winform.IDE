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
using System.Drawing.Text;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Controls
{
    [LicenseProvider(typeof(SEControlLicenseProvider))]
    public class SEAreoMainMenuStrip : MenuStrip
    {
        public SEAreoMainMenuStrip()
        {
            LicenseManager.Validate(typeof(SEAreoMainMenuStrip));
            this.Renderer = new SEAreoMainMenuStripRenderer();
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            int curveSize = 8;
            int heightSpace = 0;
            int widthSpace = 3;
            if (this.Items.Count > 0)
            {
                widthSpace = this.Items[0].Padding.Left;
            }
            int xStart = -1;
            int height = this.Height;
            int width = this.Width ;
            int itemHeight = this.MaxItemSize.Height;
            int itemWidth = 100;
            if (this.Items.Count > 0)
            {
                ToolStripItem lastItem = this.Items[this.Items.Count - 1];
                itemWidth = lastItem.Bounds.Location.X + lastItem.Width + lastItem.Padding.Right;
            }
            Graphics g = e.Graphics;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            if (EnvironmentHelper.SupportAreo && EnvironmentHelper.DwmIsCompositionEnabled)
            {
                g.Clear(Color.Transparent);
            }
            else
            {
                g.Clear(SystemColors.Control);
            }
            GraphicsPath path = new GraphicsPath();
            path.AddArc(new Rectangle(widthSpace, heightSpace, curveSize, curveSize), 270, -90); 
            path.AddLine(widthSpace, curveSize, widthSpace, itemHeight - curveSize / 2);
            path.AddArc(new Rectangle(widthSpace - curveSize, itemHeight - curveSize, curveSize, curveSize), 0, 90);  
            path.AddLine(widthSpace - curveSize / 2, itemHeight, xStart, itemHeight); 
            path.AddLine(xStart, itemHeight, xStart, height); 
            path.AddLine(xStart, height, width, height);
            path.AddLine(width, height, width, itemHeight); 
            path.AddLine(width, itemHeight, itemWidth + curveSize / 2, itemHeight); 
            path.AddArc(new Rectangle(itemWidth, itemHeight - curveSize, curveSize, curveSize), 90, 90);  
            path.AddLine(itemWidth, itemHeight - curveSize / 2, itemWidth, curveSize); 
            path.AddArc(new Rectangle(itemWidth - curveSize, heightSpace, curveSize, curveSize), 0, -90); 
            path.AddLine(itemWidth - curveSize, heightSpace, curveSize / 2 + widthSpace, heightSpace);
            using (LinearGradientBrush brush = new LinearGradientBrush(
                new Rectangle(widthSpace, heightSpace, width, height), Color.White, Color.White, LinearGradientMode.Vertical))
            {
                g.FillPath(brush, path);
            }
            using (Pen pen = new Pen(Color.FromArgb(66, 92, 119)))
            {
                g.DrawPath(pen, path);
            }
        }
    }
    public class SEAreoMainMenuStripRenderer : SEToolStripRender
    {
        StringFormat stringFormat = new StringFormat();
        public SEAreoMainMenuStripRenderer()
        {
            stringFormat.HotkeyPrefix = HotkeyPrefix.Show;
            this.Panels.ContentPanelTop = SystemColors.Control;
            this.AlterColor = true;
            this.OverrideColor = Color.Black;
        }
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            if (e.Item.IsOnDropDown)
            {
                base.OnRenderItemText(e);
            }
            else
            {
                if (_smoothText)
                    e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                if (_overrideText)
                    e.TextColor = _overrideColor;
                Color colorMenuHighLight = ControlPaint.LightLight(SystemColors.MenuHighlight);
                int imageLocationX = 4;
                int imageLocationY = 2;
                int textLocationX = 6;
                int textLocationY = (int)Math.Round((e.Item.ContentRectangle.Height - e.Graphics.MeasureString(e.Item.Text, e.Item.Font).Height) / 2);
                SolidBrush textBrush = new SolidBrush(e.TextColor);
                Rectangle imageRect = new Rectangle(imageLocationX, imageLocationY, 16, 16);
                if (e.Item.Image != null)
                {
                    if (e.Item.DisplayStyle == ToolStripItemDisplayStyle.ImageAndText)
                    {
                        if (e.Item.Enabled)
                            e.Graphics.DrawImage(e.Item.Image, imageRect);
                        else
                            ControlPaint.DrawImageDisabled(e.Graphics, e.Item.Image, imageRect.X, imageRect.Y, e.Item.BackColor);
                        e.Graphics.DrawString(e.Item.Text, e.Item.Font, textBrush, new Point(textLocationX + 14, textLocationY), stringFormat);
                    }
                    else if (e.Item.DisplayStyle == ToolStripItemDisplayStyle.Image)
                    {
                        if (e.Item.Enabled)
                            e.Graphics.DrawImage(e.Item.Image, imageRect);
                        else
                            ControlPaint.DrawImageDisabled(e.Graphics, e.Item.Image, imageRect.X, imageRect.Y, e.Item.BackColor);
                    }
                    else if (e.Item.DisplayStyle == ToolStripItemDisplayStyle.Text)
                    {
                        e.Graphics.DrawString(e.Item.Text, e.Item.Font, textBrush, new Point(textLocationX, textLocationY), stringFormat);
                    }
                }
                else
                {
                    e.Graphics.DrawString(e.Item.Text, e.Item.Font, textBrush, new Point(textLocationX, textLocationY), stringFormat);
                }
                textBrush.Dispose();
            }
        }
    }
}
