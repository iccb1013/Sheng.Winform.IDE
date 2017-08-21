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
namespace Sheng.SailingEase.Controls
{
    public class SEListViewDescriptiveRenderer : SEListViewRenderer
    {
        bool _headerHeightInited = false;
        int _headerHeight;
        Font _headerFont;
        Size _itemPadding = new Size(8, 4);
        StringFormat _itemHeaderStringFormat = new StringFormat();
        public SEListViewDescriptiveRenderer(SEListViewLayoutManager layoutManager)
            : base(layoutManager)
        {
            layoutManager.ItemHeight = 40;
            _itemHeaderStringFormat.FormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoWrap;
        }
        internal override void DrawForeground(Graphics g)
        {
        }
        internal override void DrawItemContent(Graphics g, Rectangle bounds, SEListViewItem item)
        {
            string header = LayoutManager.GetItemText(item.Value);
            if (String.IsNullOrEmpty(header))
                return;
            string description = null;
            if (LayoutManager.ContainerExtendMember(SEListViewDescriptiveMembers.DescriptioinMember))
            {
                description = LayoutManager.GetItemText(item.Value,
                    LayoutManager.GetExtendMember(SEListViewDescriptiveMembers.DescriptioinMember));
            }
            if (_headerHeightInited == false)
            {
                _headerFont = new Font(Theme.ItemHeaderFont, FontStyle.Bold);
                SizeF headerSize = g.MeasureString(header, _headerFont);
                _headerHeight = (int)Math.Ceiling(headerSize.Height);
                _headerHeightInited = true;
            }
            Rectangle _headerBounds = new Rectangle();
            _headerBounds.X = _itemPadding.Width;
            _headerBounds.Y = _itemPadding.Height;
            _headerBounds.Width = bounds.Width;
            _headerBounds.Height = _headerHeight;
            Rectangle _descriptionBounds = new Rectangle();
            _descriptionBounds.X = _itemPadding.Width;
            _descriptionBounds.Y = _headerBounds.Y + _headerBounds.Height + _itemPadding.Height;
            _descriptionBounds.Width = bounds.Width;
            _descriptionBounds.Height = _headerHeight;
            _headerBounds.Offset(bounds.Location);
            _descriptionBounds.Offset(bounds.Location);
            if (String.IsNullOrEmpty(header) == false)
            {
                using (SolidBrush brush = new SolidBrush(Theme.ItemHeaderColor))
                {
                    g.DrawString(header, _headerFont, brush, _headerBounds, _itemHeaderStringFormat);
                }
            }
            if (String.IsNullOrEmpty(description) == false)
            {
                using (SolidBrush brush = new SolidBrush(Theme.ItemDescriptioniColor))
                {
                    g.DrawString(description, Theme.ItemHeaderFont, brush, _descriptionBounds, _itemHeaderStringFormat);
                }
            }
        }
    }
}
