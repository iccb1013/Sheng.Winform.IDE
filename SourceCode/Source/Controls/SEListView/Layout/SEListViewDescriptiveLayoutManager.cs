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
    class SEListViewDescriptiveLayoutManager : SEListViewLayoutManager
    {
        public SEListViewDescriptiveLayoutManager(SEListView imageListView)
            : base(imageListView)
        {
            this.ItemHeight = 40;
            this.Renderer = new SEListViewDescriptiveRenderer(this);
            this.Renderer.Theme = imageListView.Theme;
        }
    }
}
