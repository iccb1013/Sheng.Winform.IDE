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
namespace Sheng.SailingEase.Controls
{
    public class ImageListViewItem
    {
        public delegate Image GetImageHandler(object key);
        private GetImageHandler _getImageHandler;
        private ImageListViewCollection _ownerCollection;
        internal ImageListViewCollection OwnerCollection
        {
            get { return _ownerCollection; }
            set { _ownerCollection = value; }
        }
        public int Index
        {
            get
            {
                return _ownerCollection.IndexOf(this);
            }
        }
        private ImageListViewItemState _state = ImageListViewItemState.None;
        public ImageListViewItemState State
        {
            get { return _state; }
        }
        public bool Selected
        {
            get
            {
                return (_state & ImageListViewItemState.Selected) == ImageListViewItemState.Selected;
            }
            set
            {
                bool selected = Selected;
                if (value)
                    _state = _state | ImageListViewItemState.Selected;
                else
                    _state = _state ^ ImageListViewItemState.Selected;
                if (selected != Selected)
                    Render();
            }
        }
        public bool Hovered
        {
            get
            {
                return (_state & ImageListViewItemState.Hovered) == ImageListViewItemState.Hovered;
            }
            set
            {
                bool hovered = Hovered;
                if (value)
                    _state = _state | ImageListViewItemState.Hovered;
                else
                    _state = _state ^ ImageListViewItemState.Hovered;
                if (hovered != Hovered)
                    Render();
            }
        }
        public bool Focused
        {
            get
            {
                return (_state & ImageListViewItemState.Focused) == ImageListViewItemState.Focused;
            }
            set
            {
                bool focused = Focused;
                if (value)
                    _state = _state | ImageListViewItemState.Focused;
                else
                    _state = _state ^ ImageListViewItemState.Focused;
                if (focused != Focused)
                    Render();
            }
        }
        public object Key
        {
            get;
            set;
        }
        public string Header
        {
            get;
            set;
        }
        private Image _image;
        public Image Image
        {
            get
            {
                if (_image == null)
                    _image = _getImageHandler(Key);
                return _image;
            }
        }
        public ImageListViewItem(object key, GetImageHandler getImageHandler)
            : this(key, key.ToString(), getImageHandler)
        {
        }
        public ImageListViewItem(object key, string header, GetImageHandler getImageHandler)
        {
            this.Key = key;
            this.Header = header;
            this._getImageHandler = getImageHandler;
        }
        private void Render()
        {
            _ownerCollection.Owner.RenderItem(this);
        }
    }
}
