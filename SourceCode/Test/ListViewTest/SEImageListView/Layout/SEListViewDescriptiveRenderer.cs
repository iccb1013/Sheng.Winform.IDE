using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ListViewTest
{
    /// <summary>
    /// 为项绘制带有描述信息的渲染器
    /// </summary>
    public class SEListViewDescriptiveRenderer : SEListViewRenderer
    {
        #region 私有成员

        /// <summary>
        /// 字的高度是否已初始化
        /// 在第一次绘制时，测量文本的高度
        /// </summary>
        bool _headerHeightInited = false;

        int _headerHeight;
        Font _headerFont;
        Size _itemPadding = new Size(8, 4);
        StringFormat _itemHeaderStringFormat = new StringFormat();

        #endregion

        #region 构造

        public SEListViewDescriptiveRenderer(SEListViewLayoutManager layoutManager)
            : base(layoutManager)
        {
            layoutManager.ItemSize = 40;

            //_itemHeaderStringFormat.Alignment = StringAlignment.Center;
            _itemHeaderStringFormat.FormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoWrap;
        }

        #endregion

        #region 受保护的方法

        internal override void DrawForeground(Graphics g)
        {
            
        }

        internal override void DrawItemContent(Graphics g, Rectangle bounds, SEListViewItem item)
        {
            if (_headerHeightInited == false)
            {
                _headerFont = new Font(Theme.ItemHeaderFont, FontStyle.Bold);

                SizeF headerSize = g.MeasureString(item.Header, _headerFont);
                _headerHeight = (int)Math.Ceiling(headerSize.Height);

                _headerHeightInited = true;
            }

            #region 绘制文本

            Rectangle _headerBounds = new Rectangle();
            _headerBounds.X = _itemPadding.Width;
            _headerBounds.Y = _itemPadding.Height;//LayoutManager.ItemSize - _headerHeight - _itemPadding.Height;
            _headerBounds.Width = bounds.Width;
            _headerBounds.Height = _headerHeight;

            Rectangle _descriptionBounds = new Rectangle();
            _descriptionBounds.X = _itemPadding.Width;
            _descriptionBounds.Y = _headerBounds.Y + _headerBounds.Height + _itemPadding.Height;
            _descriptionBounds.Width = bounds.Width;
            _descriptionBounds.Height = _headerHeight;

            //注意，offset必须在最后，如果先offset了_headerBounds，再带入_headerBounds来计算_descriptionBounds
            //就不对了
            _headerBounds.Offset(bounds.Location);
            _descriptionBounds.Offset(bounds.Location);

            if (String.IsNullOrEmpty(item.Header) == false)
            {
                using (SolidBrush brush = new SolidBrush(Theme.ItemHeaderColor))
                {
                    g.DrawString(item.Header, _headerFont, brush, _headerBounds, _itemHeaderStringFormat);
                }
            }

            if (String.IsNullOrEmpty(item.Description) == false)
            {
                using (SolidBrush brush = new SolidBrush(Theme.ItemDescriptioniColor))
                {
                    g.DrawString(item.Description, Theme.ItemHeaderFont, brush, _descriptionBounds, _itemHeaderStringFormat);
                }
            }

            #endregion
        }

        #endregion
    }
}
