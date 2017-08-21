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
    class SEListViewStandardRenderer : SEListViewRenderer
    {
        bool _headerHeightInited = false;
        int _headerHeight;
        Size _itemPadding = new Size(8, 4);
        Rectangle _headerBounds;
        StringFormat _itemHeaderStringFormat = new StringFormat();
        public SEListViewStandardRenderer(SEListViewLayoutManager layoutManager)
            : base(layoutManager)
        {
            layoutManager.ItemHeight = 24;
            _itemHeaderStringFormat.FormatFlags = StringFormatFlags.LineLimit| StringFormatFlags.NoWrap;
        }
        internal override void DrawForeground(Graphics g)
        {
        }
        internal override void DrawItemContent(Graphics g, Rectangle bounds, SEListViewItem item)
        {
            string header = LayoutManager.GetItemText(item.Value);
            if (String.IsNullOrEmpty(header))
                return;
            if (_headerHeightInited == false)
            {
                SizeF headerSize = g.MeasureString(header, Theme.ItemHeaderFont);
                _headerHeight = (int)Math.Ceiling(headerSize.Height);
                _headerHeightInited = true;
            }
            _headerBounds = new Rectangle();
            _headerBounds.X = _itemPadding.Width;
            _headerBounds.Y = _itemPadding.Height;
            _headerBounds.Width = bounds.Width;
            _headerBounds.Height = _headerHeight;
            _headerBounds.Offset(bounds.Location);
            if (String.IsNullOrEmpty(header) == false)
            {
                using (SolidBrush brush = new SolidBrush(Theme.ItemHeaderColor))
                {
                    g.DrawString(header, Theme.ItemHeaderFont, brush, _headerBounds, _itemHeaderStringFormat);
                }
            }
        }
    }
}
