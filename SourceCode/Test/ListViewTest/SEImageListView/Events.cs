using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ListViewTest
{

    /// <summary>
    /// 双击项事件参数
    /// </summary>
    public class SEListViewItemDoubleClickEventArgs : EventArgs
    {
        public SEListViewItem Item { get; private set; }

        public SEListViewItemDoubleClickEventArgs(SEListViewItem item)
        {
            Item = item;
        }
    }

    /// <summary>
    /// 项被删除事件参数
    /// </summary>
    public class SEListViewItemsRemovedEventArgs : EventArgs
    {
        public List<SEListViewItem> Items { get; private set; }

        public SEListViewItemsRemovedEventArgs(List<SEListViewItem> items)
        {
            Items = items;
        }
    }
}
