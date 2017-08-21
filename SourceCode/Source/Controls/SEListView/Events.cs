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
namespace Sheng.SailingEase.Controls
{
    public class SEListViewItemDoubleClickEventArgs : EventArgs
    {
        public SEListViewItem Item { get; private set; }
        public SEListViewItemDoubleClickEventArgs(SEListViewItem item)
        {
            Item = item;
        }
    }
    public class SEListViewItemsRemovedEventArgs : EventArgs
    {
        public List<SEListViewItem> Items { get; private set; }
        public SEListViewItemsRemovedEventArgs(List<SEListViewItem> items)
        {
            Items = items;
        }
    }
    public class SEListViewGetItemTextEventArgs : EventArgs
    {
        public object Item { get; private set; }
        public string Text { get; set; }
        public SEListViewGetItemTextEventArgs(object item)
        {
            Item = item;
        }
    }
}
