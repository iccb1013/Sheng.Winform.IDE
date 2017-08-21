using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ListViewTest
{
    /// <summary>
    /// 测试坐标
    /// </summary>
    public class SEListViewHitInfo
    {
        /// <summary>
        /// 项的坐标
        /// </summary>
        public int ItemIndex { get; private set; }

        /// <summary>
        /// 是否点击了项
        /// </summary>
        public bool ItemHit { get; private set; }

        public SEListViewHitInfo(int itemIndex,bool itemHit)
        {
            ItemIndex = itemIndex;
            ItemHit = itemHit;
        }
    }
}
