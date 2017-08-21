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
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Controls;
namespace Sheng.SailingEase.Components.ResourceComponent.View
{
    class ImageListViewController
    {
        private ImageListView _imageListView;
        private ImageListViewItem.GetImageHandler _getImage;
        public int SelectedCount
        {
            get
            {
                return _imageListView.GetSelectedItems().Count;
            }
        }
        public ImageListViewController(ImageListView imageListView)
        {
            _imageListView = imageListView;
            _imageListView.AllowDrop = false;
            _imageListView.AllowMultiSelection = true;
            _getImage = new ImageListViewItem.GetImageHandler((key) =>
                {
                    ImageResourceInfo resourceInfo = (ImageResourceInfo)key;
                    return resourceInfo.GetImage();
                });
        }
        public void DataBind(List<ImageResourceInfo> imageList)
        {
            _imageListView.SuspendLayout();
            foreach (ImageResourceInfo imageResource in imageList)
            {
                ImageListViewItem item = new ImageListViewItem(imageResource, imageResource.Name, _getImage);
                _imageListView.Items.Add(item);
            }
            _imageListView.ResumeLayout();
        }
        public void Add(ImageResourceInfo imageResource)
        {
            ImageListViewItem item = new ImageListViewItem(imageResource, imageResource.Name, _getImage);
            _imageListView.Items.Add(item);
        }
        public void Remove(string resourceName)
        {
            for (int i = 0; i < _imageListView.Items.Count; i++)
            {
                ImageResourceInfo imageResource = (ImageResourceInfo)_imageListView.Items[i].Key;
                if (imageResource.Name.Equals(resourceName))
                {
                    _imageListView.Items.RemoveAt(i);
                    break;
                }
            }
        }
        public void Update(ImageResourceInfo imageResource)
        {
            Remove(imageResource.Name);
            Add(imageResource);
        }
        public List<ImageResourceInfo> GetSelectedItems()
        {
            List<ImageResourceInfo> list = new List<ImageResourceInfo>();
            foreach (var item in _imageListView.GetSelectedItems())
            {
                list.Add((ImageResourceInfo)item.Key);
            }
            return list;
        }
    }
}
