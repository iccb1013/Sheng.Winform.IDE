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
    public class SEListViewHitInfo
    {
        public int ItemIndex { get; private set; }
        public bool ItemHit { get; private set; }
        public SEListViewHitInfo(int itemIndex,bool itemHit)
        {
            ItemIndex = itemIndex;
            ItemHit = itemHit;
        }
    }
}
