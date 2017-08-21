// ImageListView - A listview control for image files
// Copyright (C) 2009 Ozgur Ozcitak
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// Ozgur Ozcitak (ozcitak@yahoo.com)

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace Manina.Windows.Forms
{
    /// <summary>
    /// Represents the layout of the image list view drawing area.
    /// </summary>
    internal class ImageListViewLayoutManager
    {
        #region Member Variables
        private Rectangle mClientArea;
        private ImageListView mImageListView;
        private Rectangle mItemAreaBounds;
        private Rectangle mColumnHeaderBounds;
        private Size mItemSize;
        private Size mItemSizeWithMargin;
        private int mCols;
        private int mRows;
        private int mFirstPartiallyVisible;
        private int mLastPartiallyVisible;
        private int mFirstVisible;
        private int mLastVisible;

        private View cachedView;
        private Point cachedViewOffset;
        private Size cachedSize;
        private int cachedItemCount;
        private Size cachedItemSize;
        private int cachedHeaderHeight;
        private Size cachedItemMargin;
        private int cachedPaneWidth;
        private Dictionary<Guid, bool> cachedVisibleItems;

        private bool vScrollVisible;
        private bool hScrollVisible;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the bounds of the entire client area.
        /// </summary>
        public Rectangle ClientArea { get { return mClientArea; } }
        /// <summary>
        /// Gets the owner image list view.
        /// </summary>
        public ImageListView ImageListView { get { return mImageListView; } }
        /// <summary>
        /// Gets the extends of the item area.
        /// </summary>
        public Rectangle ItemAreaBounds { get { return mItemAreaBounds; } }
        /// <summary>
        /// Gets the extents of the column header area.
        /// </summary>
        public Rectangle ColumnHeaderBounds { get { return mColumnHeaderBounds; } }
        /// <summary>
        /// Gets the items size.
        /// </summary>
        public Size ItemSize { get { return mItemSize; } }
        /// <summary>
        /// Gets the items size including the margin around the item.
        /// </summary>
        public Size ItemSizeWithMargin { get { return mItemSizeWithMargin; } }
        /// <summary>
        /// Gets the maximum number of columns that can be displayed.
        /// </summary>
        public int Cols { get { return mCols; } }
        /// <summary>
        /// Gets the maximum number of rows that can be displayed.
        /// </summary>
        public int Rows { get { return mRows; } }
        /// <summary>
        /// Gets the index of the first partially visible item.
        /// </summary>
        public int FirstPartiallyVisible { get { return mFirstPartiallyVisible; } }
        /// <summary>
        /// Gets the index of the last partially visible item.
        /// </summary>
        public int LastPartiallyVisible { get { return mLastPartiallyVisible; } }
        /// <summary>
        /// Gets the index of the first fully visible item.
        /// </summary>
        public int FirstVisible { get { return mFirstVisible; } }
        /// <summary>
        /// Gets the index of the last fully visible item.
        /// </summary>
        public int LastVisible { get { return mLastVisible; } }
        /// <summary>
        /// Determines whether an update is required.
        /// </summary>
        public bool UpdateRequired
        {
            get
            {
                if (mImageListView.View != cachedView)
                    return true;
                else if (mImageListView.ViewOffset != cachedViewOffset)
                    return true;
                else if (mImageListView.ClientSize != cachedSize)
                    return true;
                else if (mImageListView.Items.Count != cachedItemCount)
                    return true;
                else if (mImageListView.mRenderer.MeasureItem(mImageListView.View) != cachedItemSize)
                    return true;
                else if (mImageListView.mRenderer.MeasureColumnHeaderHeight() != cachedHeaderHeight)
                    return true;
                else if (mImageListView.mRenderer.MeasureItemMargin(mImageListView.View) != cachedItemMargin)
                    return true;
                else if (mImageListView.PaneWidth != cachedPaneWidth)
                    return true;
                else
                    return false;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the ImageListViewLayoutManager class.
        /// </summary>
        /// <param name="owner">The owner control.</param>
        public ImageListViewLayoutManager(ImageListView owner)
        {
            mImageListView = owner;
            cachedVisibleItems = new Dictionary<Guid, bool>();

            vScrollVisible = false;
            hScrollVisible = false;

            Update();
        }
        #endregion

        #region Instance Methods
        /// <summary>
        /// Returns the bounds of the item with the specified index.
        /// </summary>
        public Rectangle GetItemBounds(int itemIndex)
        {
            Point location = mItemAreaBounds.Location;
            location.X += cachedItemMargin.Width / 2 - mImageListView.ViewOffset.X;
            location.Y += cachedItemMargin.Height / 2 - mImageListView.ViewOffset.Y;
            if (mImageListView.View == View.Gallery)
            {
                location.X += itemIndex * mItemSizeWithMargin.Width;
            }
            else
            {
                location.X += (itemIndex % mCols) * mItemSizeWithMargin.Width;
                location.Y += (itemIndex / mCols) * mItemSizeWithMargin.Height;
            }
            return new Rectangle(location, mItemSize);
        }
        /// <summary>
        /// Returns the bounds of the item with the specified index, 
        /// including the margin around the item.
        /// </summary>
        public Rectangle GetItemBoundsWithMargin(int itemIndex)
        {
            Rectangle rec = GetItemBounds(itemIndex);
            rec.Inflate(cachedItemMargin.Width / 2, cachedItemMargin.Height / 2);
            return rec;
        }
        /// <summary>
        /// Recalculates the control layout.
        /// </summary>
        public void Update()
        {
            Update(false);
        }
        /// <summary>
        /// Recalculates the control layout.
        /// </summary>
        public void Update(bool forceUpdate)
        {
            if (mImageListView.ClientRectangle.Width == 0 || mImageListView.ClientRectangle.Height == 0) return;
            if (!forceUpdate && !UpdateRequired) return;
            // Cache current properties to determine if we will need an update later
            cachedView = mImageListView.View;
            cachedViewOffset = mImageListView.ViewOffset;
            cachedSize = mImageListView.ClientSize;
            cachedItemCount = mImageListView.Items.Count;
            cachedItemSize = mImageListView.mRenderer.MeasureItem(mImageListView.View);
            cachedHeaderHeight = mImageListView.mRenderer.MeasureColumnHeaderHeight();
            cachedItemMargin = mImageListView.mRenderer.MeasureItemMargin(mImageListView.View);
            cachedPaneWidth = mImageListView.PaneWidth;
            cachedVisibleItems.Clear();

            // Calculate drawing area
            mClientArea = mImageListView.ClientRectangle;
            mItemAreaBounds = mImageListView.ClientRectangle;

            // Item size
            mItemSize = cachedItemSize;
            mItemSizeWithMargin = mItemSize + cachedItemMargin;

            // Allocate space for scrollbars
            if (mImageListView.hScrollBar.Visible)
            {
                mClientArea.Height -= mImageListView.hScrollBar.Height;
                mItemAreaBounds.Height -= mImageListView.hScrollBar.Height;
            }
            if (mImageListView.vScrollBar.Visible)
            {
                mClientArea.Width -= mImageListView.vScrollBar.Width;
                mItemAreaBounds.Width -= mImageListView.vScrollBar.Width;
            }

            // Allocate space for column headers
            if (mImageListView.View == View.Details)
            {
                int headerHeight = cachedHeaderHeight;

                // Location of the column headers
                mColumnHeaderBounds.X = mClientArea.Left - mImageListView.ViewOffset.X;
                mColumnHeaderBounds.Y = mClientArea.Top;
                mColumnHeaderBounds.Height = headerHeight;
                mColumnHeaderBounds.Width = mClientArea.Width + mImageListView.ViewOffset.X;

                mItemAreaBounds.Y += headerHeight;
                mItemAreaBounds.Height -= headerHeight;
            }
            else
            {
                mColumnHeaderBounds = Rectangle.Empty;
            }
            // Modify item area for the gallery view mode
            if (mImageListView.View == View.Gallery)
            {
                mItemAreaBounds.Height = mItemSizeWithMargin.Height;
                mItemAreaBounds.Y = mClientArea.Bottom - mItemSizeWithMargin.Height;
            }
            // Modify item area for the pane view mode
            if (mImageListView.View == View.Pane)
            {
                mItemAreaBounds.Width -= cachedPaneWidth;
                mItemAreaBounds.X += cachedPaneWidth;
            }

            if (mItemAreaBounds.Width < 1 || mItemAreaBounds.Height < 1) return;

            // Let the calculated bounds modified by the renderer
            LayoutEventArgs eLayout = new LayoutEventArgs(mItemAreaBounds);
            mImageListView.mRenderer.OnLayout(eLayout);
            mItemAreaBounds = eLayout.ItemAreaBounds;

            // Maximum number of rows and columns that can be fully displayed
            mCols = (int)System.Math.Floor((float)mItemAreaBounds.Width / (float)mItemSizeWithMargin.Width);
            mRows = (int)System.Math.Floor((float)mItemAreaBounds.Height / (float)mItemSizeWithMargin.Height);
            if (mImageListView.View == View.Details) mCols = 1;
            if (mImageListView.View == View.Gallery) mRows = 1;
            if (mCols < 1) mCols = 1;
            if (mRows < 1) mRows = 1;

            // Check if we need the horizontal scroll bar
            bool hScrollRequired = false;
            if (mImageListView.View == View.Gallery)
                hScrollRequired = (mImageListView.Items.Count > 0) && (mCols * mRows < mImageListView.Items.Count);
            else
                hScrollRequired = (mImageListView.Items.Count > 0) && (mItemAreaBounds.Width < mCols * mItemSizeWithMargin.Width);
            if (hScrollRequired != hScrollVisible)
            {
                hScrollVisible = hScrollRequired;
                mImageListView.hScrollBar.Visible = hScrollRequired;
                Update(true);
                return;
            }

            // Check if we need the vertical scroll bar
            bool vScrollRequired = false;
            if (mImageListView.View == View.Gallery)
                vScrollRequired = (mImageListView.Items.Count > 0) && (mItemAreaBounds.Height < mRows * mItemSizeWithMargin.Height);
            else
                vScrollRequired = (mImageListView.Items.Count > 0) && (mCols * mRows < mImageListView.Items.Count);
            if (vScrollRequired != vScrollVisible)
            {
                vScrollVisible = vScrollRequired;
                mImageListView.vScrollBar.Visible = vScrollRequired;
                Update(true);
                return;
            }

            // Horizontal scroll range
            if (mImageListView.View == View.Gallery)
            {
                mImageListView.hScrollBar.SmallChange = mItemSizeWithMargin.Width;
                mImageListView.hScrollBar.LargeChange = mItemAreaBounds.Width;
                mImageListView.hScrollBar.Minimum = 0;
                mImageListView.hScrollBar.Maximum = Math.Max(0, (int)System.Math.Ceiling((float)mImageListView.Items.Count / (float)mRows) * mItemSizeWithMargin.Width - 1);
            }
            else
            {
                mImageListView.hScrollBar.SmallChange = 1;
                mImageListView.hScrollBar.LargeChange = mItemAreaBounds.Width;
                mImageListView.hScrollBar.Minimum = 0;
                mImageListView.hScrollBar.Maximum = mCols * mItemSizeWithMargin.Width;
            }
            if (mImageListView.ViewOffset.X > mImageListView.hScrollBar.Maximum - mImageListView.hScrollBar.LargeChange + 1)
            {
                mImageListView.hScrollBar.Value = mImageListView.hScrollBar.Maximum - mImageListView.hScrollBar.LargeChange + 1;
                mImageListView.ViewOffset = new Point(mImageListView.hScrollBar.Value, mImageListView.ViewOffset.Y);
            }

            // Vertical scroll range
            if (mImageListView.View == View.Gallery)
            {
                mImageListView.vScrollBar.SmallChange = 1;
                mImageListView.vScrollBar.LargeChange = mItemAreaBounds.Height;
                mImageListView.vScrollBar.Minimum = 0;
                mImageListView.vScrollBar.Maximum = mRows * mItemSizeWithMargin.Height;
            }
            else
            {
                mImageListView.vScrollBar.SmallChange = mItemSizeWithMargin.Height;
                mImageListView.vScrollBar.LargeChange = mItemAreaBounds.Height;
                mImageListView.vScrollBar.Minimum = 0;
                mImageListView.vScrollBar.Maximum = Math.Max(0, (int)System.Math.Ceiling((float)mImageListView.Items.Count / (float)mCols) * mItemSizeWithMargin.Height - 1);
            }
            if (mImageListView.ViewOffset.Y > mImageListView.vScrollBar.Maximum - mImageListView.vScrollBar.LargeChange + 1)
            {
                mImageListView.vScrollBar.Value = mImageListView.vScrollBar.Maximum - mImageListView.vScrollBar.LargeChange + 1;
                mImageListView.ViewOffset = new Point(mImageListView.ViewOffset.X, mImageListView.vScrollBar.Value);
            }

            // Zero out the scrollbars if we don't have any items
            if (mImageListView.Items.Count == 0)
            {
                mImageListView.hScrollBar.Minimum = 0;
                mImageListView.hScrollBar.Maximum = 0;
                mImageListView.hScrollBar.Value = 0;
                mImageListView.vScrollBar.Minimum = 0;
                mImageListView.vScrollBar.Maximum = 0;
                mImageListView.vScrollBar.Value = 0;
                mImageListView.ViewOffset = new Point(0, 0);
            }

            // Horizontal scrollbar position
            mImageListView.hScrollBar.Left = 0;
            mImageListView.hScrollBar.Top = mImageListView.ClientRectangle.Bottom - mImageListView.hScrollBar.Height;
            mImageListView.hScrollBar.Width = mImageListView.ClientRectangle.Width - (mImageListView.vScrollBar.Visible ? mImageListView.vScrollBar.Width : 0);
            // Vertical scrollbar position
            mImageListView.vScrollBar.Left = mImageListView.ClientRectangle.Right - mImageListView.vScrollBar.Width;
            mImageListView.vScrollBar.Top = 0;
            mImageListView.vScrollBar.Height = mImageListView.ClientRectangle.Height - (mImageListView.hScrollBar.Visible ? mImageListView.hScrollBar.Height : 0);

            // Find the first and last partially visible items
            if (mImageListView.View == View.Gallery)
            {
                mFirstPartiallyVisible = (int)System.Math.Floor((float)mImageListView.ViewOffset.X / (float)mItemSizeWithMargin.Width) * mRows;
                mLastPartiallyVisible = (int)System.Math.Ceiling((float)(mImageListView.ViewOffset.X + mItemAreaBounds.Width) / (float)mItemSizeWithMargin.Width) * mRows - 1;
            }
            else
            {
                mFirstPartiallyVisible = (int)System.Math.Floor((float)mImageListView.ViewOffset.Y / (float)mItemSizeWithMargin.Height) * mCols;
                mLastPartiallyVisible = (int)System.Math.Ceiling((float)(mImageListView.ViewOffset.Y + mItemAreaBounds.Height) / (float)mItemSizeWithMargin.Height) * mCols - 1;
            }
            if (mFirstPartiallyVisible < 0) mFirstPartiallyVisible = 0;
            if (mFirstPartiallyVisible > mImageListView.Items.Count - 1) mFirstPartiallyVisible = mImageListView.Items.Count - 1;
            if (mLastPartiallyVisible < 0) mLastPartiallyVisible = 0;
            if (mLastPartiallyVisible > mImageListView.Items.Count - 1) mLastPartiallyVisible = mImageListView.Items.Count - 1;

            // Find the first and last visible items
            if (mImageListView.View == View.Gallery)
            {
                mFirstVisible = (int)System.Math.Ceiling((float)mImageListView.ViewOffset.X / (float)mItemSizeWithMargin.Width) * mRows;
                mLastVisible = (int)System.Math.Floor((float)(mImageListView.ViewOffset.X + mItemAreaBounds.Width) / (float)mItemSizeWithMargin.Width) * mRows - 1;
            }
            else
            {
                mFirstVisible = (int)System.Math.Ceiling((float)mImageListView.ViewOffset.Y / (float)mItemSizeWithMargin.Height) * mCols;
                mLastVisible = (int)System.Math.Floor((float)(mImageListView.ViewOffset.Y + mItemAreaBounds.Height) / (float)mItemSizeWithMargin.Height) * mCols - 1;
            }
            if (mFirstVisible < 0) mFirstVisible = 0;
            if (mFirstVisible > mImageListView.Items.Count - 1) mFirstVisible = mImageListView.Items.Count - 1;
            if (mLastVisible < 0) mLastVisible = 0;
            if (mLastVisible > mImageListView.Items.Count - 1) mLastVisible = mImageListView.Items.Count - 1;

            // Cache visible items
            if (mFirstPartiallyVisible >= 0 &&
                mLastPartiallyVisible >= 0 &&
                mFirstPartiallyVisible <= mImageListView.Items.Count - 1 &&
                mLastPartiallyVisible <= mImageListView.Items.Count - 1)
            {
                for (int i = mFirstPartiallyVisible; i <= mLastPartiallyVisible; i++)
                    cachedVisibleItems.Add(mImageListView.Items[i].Guid, false);
            }
        }
        /// <summary>
        /// Determines whether the item with the given guid is
        /// (partially) visible.
        /// </summary>
        /// <param name="guid">The guid of the item to check.</param>
        public bool IsItemVisible(Guid guid)
        {
            return cachedVisibleItems.ContainsKey(guid);
        }
        #endregion
    }
}
