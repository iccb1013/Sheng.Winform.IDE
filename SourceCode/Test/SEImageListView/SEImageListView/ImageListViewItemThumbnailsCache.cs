using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;

namespace SEImageListView
{
    /// <summary>
    /// 缓存项的缩略图
    /// </summary>
    class ImageListViewItemThumbnailsCache
    {
        #region 私有成员

        private Dictionary<ImageListViewItem, Image> _thumbnails = new Dictionary<ImageListViewItem, Image>();

        #endregion

        #region 构造

        public ImageListViewItemThumbnailsCache()
        {

        }

        #endregion

        #region 公开方法

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

        /// <summary>
        /// 移除指定项的缓存缩略图，并dispose
        /// </summary>
        /// <param name="item"></param>
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

        #endregion
    }
}
