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
    public class ImageListViewItemDoubleClickEventArgs : EventArgs
    {
        public ImageListViewItem Item { get; private set; }
        public ImageListViewItemDoubleClickEventArgs(ImageListViewItem item)
        {
            Item = item;
        }
    }
    public class ImageListViewItemsRemovedEventArgs : EventArgs
    {
        public List<ImageListViewItem> Items { get; private set; }
        public ImageListViewItemsRemovedEventArgs(List<ImageListViewItem> items)
        {
            Items = items;
        }
    }
}
