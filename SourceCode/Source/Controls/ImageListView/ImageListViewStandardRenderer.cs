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
using Sheng.SailingEase.Drawing;
namespace Sheng.SailingEase.Controls
{
    public class ImageListViewStandardRenderer : ImageListViewRenderer
    {
        bool _thumbnailSizeInited = false;
        int _headerHeight;
        Size _itemPadding = new Size(4, 4);
        Size _thumbnailSize;
        Rectangle _headerBounds;
        StringFormat _itemHeaderStringFormat = new StringFormat();
        ImageListViewItemThumbnailsCache _thumbnailsCache = new ImageListViewItemThumbnailsCache();
        public ImageListViewStandardRenderer(ImageListViewLayoutManager layoutManager)
            : base(layoutManager)
        {
            _itemHeaderStringFormat.Alignment = StringAlignment.Center;
            _itemHeaderStringFormat.FormatFlags = StringFormatFlags.LineLimit| StringFormatFlags.NoWrap;
        }
        internal override void OnItemsRemoved(List<ImageListViewItem> items)
        {
            foreach (var item in items)
            {
                _thumbnailsCache.RemoveThumbnail(item);
            }
            base.OnItemsRemoved(items);
        }
        internal override void DrawForeground(Graphics g)
        {
        }
        internal override void DrawItemContent(Graphics g, Rectangle bounds, ImageListViewItem item)
        {
            if (_thumbnailSizeInited == false)
            {
                SizeF headerSize = g.MeasureString(item.Header, Theme.ItemHeaderFont);
                _headerHeight = (int)Math.Ceiling(headerSize.Height);
                int width = LayoutManager.ItemSize.Width - _itemPadding.Width * 2;
                int height = LayoutManager.ItemSize.Height - _itemPadding.Height * 3 - _headerHeight;
                _thumbnailSize = new Size(width, height);
                _thumbnailSizeInited = true;
            }
            Image img = null;
            if (_thumbnailsCache.Container(item))
            {
                img = _thumbnailsCache.GetThumbnail(item);
            }
            else
            {
                img = DrawingTool.GetScaleImage(item.Image, _thumbnailSize);
                _thumbnailsCache.AddThumbnail(item, img);
            }
            if (img != null)
            {
                Rectangle pos = DrawingTool.GetSizedImageBounds(img, new Rectangle(bounds.Location + _itemPadding, _thumbnailSize));
                g.DrawImage(img, pos);
                if (Math.Min(pos.Width, pos.Height) > 32)
                {
                    using (Pen pOuterBorder = new Pen(Theme.ImageOuterBorderColor))
                    {
                        g.DrawRectangle(pOuterBorder, pos);
                    }
                    if (System.Math.Min(_thumbnailSize.Width, _thumbnailSize.Height) > 32)
                    {
                        using (Pen pInnerBorder = new Pen(Theme.ImageInnerBorderColor))
                        {
                            g.DrawRectangle(pInnerBorder, Rectangle.Inflate(pos, -1, -1));
                        }
                    }
                }
            }
            _headerBounds = new Rectangle();
            _headerBounds.X = _itemPadding.Width;
            _headerBounds.Y = LayoutManager.ItemSize.Height - _headerHeight - _itemPadding.Height;
            _headerBounds.Width = _thumbnailSize.Width;
            _headerBounds.Height = _headerHeight;
            _headerBounds.Offset(bounds.Location);
            using (SolidBrush brush = new SolidBrush(Theme.ItemHeaderColor))
            {
                g.DrawString(item.Header, Theme.ItemHeaderFont, brush, _headerBounds, _itemHeaderStringFormat);
            }
        }
    }
}
