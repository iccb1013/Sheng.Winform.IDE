using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manina.Windows.Forms
{
    class ImageListViewItemFactory
    {
        public ImageListViewItem Create(string filename)
        {
            ImageListViewItem item = new ImageListViewItem(filename);
            return item;
        }

        public ImageListViewItem Create(object key, string text)
        {
            ImageListViewItem item = new ImageListViewItem(key, text);
            return item;
        }
    }
}
