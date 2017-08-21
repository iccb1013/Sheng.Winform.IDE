using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEImageListView
{

    /// <summary>
    /// 双击项事件参数
    /// </summary>
    public class ImageListViewItemDoubleClickEventArgs : EventArgs
    {
        public ImageListViewItem Item { get; private set; }

        public ImageListViewItemDoubleClickEventArgs(ImageListViewItem item)
        {
            Item = item;
        }
    }

    /// <summary>
    /// 项被删除事件参数
    /// </summary>
    public class ImageListViewItemsRemovedEventArgs : EventArgs
    {
        public List<ImageListViewItem> Items { get; private set; }

        public ImageListViewItemsRemovedEventArgs(List<ImageListViewItem> items)
        {
            Items = items;
        }
    }
}
