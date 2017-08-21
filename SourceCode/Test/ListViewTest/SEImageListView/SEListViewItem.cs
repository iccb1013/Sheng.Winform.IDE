using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ListViewTest
{
    public class SEListViewItem
    {
        #region 私有成员


        #endregion

        #region 受保护的成员

        private SEListViewItemCollection _ownerCollection;
        internal SEListViewItemCollection OwnerCollection
        {
            get { return _ownerCollection; }
            set { _ownerCollection = value; }
        }

        #endregion

        #region 公开属性

        public int Index
        {
            get
            {
                return _ownerCollection.IndexOf(this);
            }
        }

        private ListViewItemState _state = ListViewItemState.None;
        /// <summary>
        /// 该项当前的选中状态
        /// </summary>
        public ListViewItemState State
        {
            get { return _state; }
        }

        public bool Selected
        {
            get
            {
                return (_state & ListViewItemState.Selected) == ListViewItemState.Selected;
            }
            set
            {
                bool selected = Selected;

                if (value)
                    _state = _state | ListViewItemState.Selected;
                else
                    _state = _state ^ ListViewItemState.Selected;

                if (selected != Selected)
                    Render();
            }
        }

        public bool Hovered
        {
            get
            {
                return (_state & ListViewItemState.Hovered) == ListViewItemState.Hovered;
            }
            set
            {
                bool hovered = Hovered;

                if (value)
                    _state = _state | ListViewItemState.Hovered;
                else
                    _state = _state ^ ListViewItemState.Hovered;

                if (hovered != Hovered)
                    Render();
            }
        }

        public bool Focused
        {
            get
            {
                return (_state & ListViewItemState.Focused) == ListViewItemState.Focused;
            }
            set
            {
                bool focused = Focused;

                if (value)
                    _state = _state | ListViewItemState.Focused;
                else
                    _state = _state ^ ListViewItemState.Focused;

                if (focused != Focused)
                    Render();
            }
        }

        /// <summary>
        /// 呈现在缩略图下方的标题文本
        /// </summary>
        public string Header
        {
            get;
            set;
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        #endregion

        #region 构造

        public SEListViewItem(string header)
        {
            this.Header = header;
        }

        #endregion

        #region 私有方法

        private void Render()
        {
            _ownerCollection.Owner.RenderItem(this);
        }

        #endregion
    }
}
