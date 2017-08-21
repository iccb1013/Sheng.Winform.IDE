using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEImageListView
{
    /// <summary>
    /// 测试坐标
    /// </summary>
    class ImageListViewHitInfo
    {
        /// <summary>
        /// 项的坐标
        /// </summary>
        public int ItemIndex { get; private set; }

        /// <summary>
        /// 是否点击了项
        /// </summary>
        public bool ItemHit { get; private set; }

        public ImageListViewHitInfo(int itemIndex,bool itemHit)
        {
            ItemIndex = itemIndex;
            ItemHit = itemHit;
        }
    }
}
