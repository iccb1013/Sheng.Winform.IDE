using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ListViewTest
{
    class SEListViewStandardLayoutManager : SEListViewLayoutManager
    {
        public SEListViewStandardLayoutManager(SEListView imageListView)
            : base(imageListView)
        {
            this.ItemSize = 24;
            this.Renderer = new SEListViewStandardRenderer(this);
            this.Renderer.Theme = imageListView.Theme;
        }
    }
}
