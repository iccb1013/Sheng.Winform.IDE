using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ListViewTest
{
    class SEListViewDescriptiveLayoutManager : SEListViewLayoutManager
    {
        public SEListViewDescriptiveLayoutManager(SEListView imageListView)
            : base(imageListView)
        {
            this.ItemSize = 40;
            this.Renderer = new SEListViewDescriptiveRenderer(this);
            this.Renderer.Theme = imageListView.Theme;
        }
    }
}