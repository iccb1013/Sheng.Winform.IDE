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
using System.Diagnostics;
namespace Sheng.SailingEase.Controls
{
    class ImageListViewItemThumbnailsCache
    {
        private Dictionary<ImageListViewItem, Image> _thumbnails = new Dictionary<ImageListViewItem, Image>();
        public ImageListViewItemThumbnailsCache()
        {
        }
        public bool Container(ImageListViewItem item)
        {
            Debug.Assert(item != null, "ImageListViewItem 为 null");
            if (item == null)
                return false;
            return _thumbnails.Keys.Contains(item);
        }
        public Image GetThumbnail(ImageListViewItem item)
        {
            Debug.Assert(item != null, "ImageListViewItem 为 null");
            if (item == null)
                throw new ArgumentNullException();
            if (Container(item) == false)
                throw new ArgumentOutOfRangeException();
            return _thumbnails[item];
        }
        public void AddThumbnail(ImageListViewItem item, Image thumbnail)
        {
            Debug.Assert(item != null, "ImageListViewItem 为 null");
            Debug.Assert(thumbnail != null, "thumbnail 为 null");
            if (item == null || thumbnail == null)
                return;
            if (Container(item))
            {
                Debug.Assert(false, "已经缓存过了指定 ImageListViewItem 的缩略图");
                return;
            }
            _thumbnails.Add(item, thumbnail);
        }
        public void RemoveThumbnail(ImageListViewItem item)
        {
            Debug.Assert(item != null, "ImageListViewItem 为 null");
            if (item == null)
                return;
            if (Container(item) == false)
            {
                Debug.Assert(false, "不存在指定 ImageListViewItem 的缓存缩略图");
                return;
            }
            _thumbnails[item].Dispose();
            _thumbnails.Remove(item);
        }
    }
}
