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
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.ComponentModel.Design.Serialization;
using System.Resources;
using System.Reflection;

namespace Manina.Windows.Forms
{
    /// <summary>
    /// Represents an image list view control.
    /// </summary>
    [ToolboxBitmap(typeof(ImageListView))]
    [Description("Represents an image list view control.")]
    [DefaultEvent("ItemClick")]
    [DefaultProperty("Items")]
    [Designer(typeof(ImageListViewDesigner))]
    [DesignerSerializer(typeof(ImageListViewSerializer), typeof(CodeDomSerializer))]
    [Docking(DockingBehavior.Ask)]
    public partial class ImageListView : Control
    {
        #region Constants
        /// <summary>
        /// Default width of column headers in pixels.
        /// </summary>
        internal const int DefaultColumnWidth = 100;
        /// <summary>
        /// Selection tolerance for column separators.
        /// </summary>
        internal const int SeparatorSize = 12;
        /// <summary>
        /// Selection tolerance for left-pane border.
        /// </summary>
        internal const int PaneBorderSize = 4;
        /// <summary>
        /// Creates a control with a border.
        /// </summary>
        private const int WS_BORDER = 0x00800000;
        /// <summary>
        /// Specifies that the control has a border with a sunken edge.
        /// </summary>
        private const int WS_EX_CLIENTEDGE = 0x00000200;
        #endregion

        #region Member Variables
        // Properties
        private BorderStyle mBorderStyle;
        private CacheMode mCacheMode;
        private int mCacheLimitAsItemCount;
        private long mCacheLimitAsMemory;
        private ImageListViewColumnHeaderCollection mColumns;
        private Image mDefaultImage;
        private Image mErrorImage;
        private Font mHeaderFont;
        private ImageListViewItemCollection mItems;
        private int mPaneWidth;
        internal ImageListViewRenderer mRenderer;
        private bool mRetryOnError;
        internal ImageListViewSelectedItemCollection mSelectedItems;
        private ColumnType mSortColumn;
        private SortOrder mSortOrder;
        private Size mThumbnailSize;
        private UseEmbeddedThumbnails mUseEmbeddedThumbnails;
        private View mView;
        private Point mViewOffset;

        // Layout variables
        internal System.Windows.Forms.HScrollBar hScrollBar;
        internal System.Windows.Forms.VScrollBar vScrollBar;
        internal ImageListViewLayoutManager layoutManager;
        private bool disposed;
        private bool forceRefresh;

        // Interaction variables
        internal ImageListViewNavigationManager navigationManager;

        // Cache threads
        internal ImageListViewCacheManager cacheManager;
        internal ImageListViewItemCacheManager itemCacheManager;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets whether column headers respond to mouse clicks.
        /// </summary>
        [Category("Behavior"), Description("Gets or sets whether column headers respond to mouse clicks."), DefaultValue(true)]
        public bool AllowColumnClick { get; set; }
        /// <summary>
        /// Gets or sets whether column headers can be resized with the mouse.
        /// </summary>
        [Category("Behavior"), Description("Gets or sets whether column headers can be resized with the mouse."), DefaultValue(true)]
        public bool AllowColumnResize { get; set; }
        /// <summary>
        /// Gets or sets whether the user can drag items for drag-and-drop operations.
        /// </summary>
        [Category("Behavior"), Description("Gets or sets whether the user can drag items for drag-and-drop operations."), DefaultValue(false)]
        public bool AllowDrag { get; set; }
        /// <summary>
        /// Gets or sets whether duplicate items (image files pointing to the same path 
        /// on the file system) are allowed.
        /// </summary>
        [Category("Behavior"), Description("Gets or sets whether duplicate items (image files pointing to the same path on the file system) are allowed."), DefaultValue(false)]
        public bool AllowDuplicateFileNames { get; set; }
        /// <summary>
        /// Gets or sets whether the left-pane can be resized with the mouse.
        /// </summary>
        [Category("Behavior"), Description("Gets or sets whether the left-pane can be resized with the mouse."), DefaultValue(true)]
        public bool AllowPaneResize { get; set; }
        /// <summary>
        /// Gets or sets the background color of the control.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the background color of the control."), DefaultValue(typeof(Color), "Window")]
        public override Color BackColor { get { return base.BackColor; } set { base.BackColor = value; Refresh(); } }
        /// <summary>
        /// Gets or sets the border style of the control.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the border style of the control."), DefaultValue(typeof(BorderStyle), "Fixed3D")]
        public BorderStyle BorderStyle { get { return mBorderStyle; } set { mBorderStyle = value; UpdateStyles(); } }
        /// <summary>
        /// Gets or sets the cache mode. Setting the the CacheMode to Continuous disables the CacheLimit.
        /// </summary>
        [Category("Behavior"), Description("Gets or sets the cache mode."), DefaultValue(typeof(CacheMode), "OnDemand"), RefreshProperties(RefreshProperties.All)]
        public CacheMode CacheMode
        {
            get
            {
                return mCacheMode;
            }
            set
            {
                if (mCacheMode != value)
                {
                    mCacheMode = value;
                    if (mCacheMode == CacheMode.Continuous)
                    {
                        mCacheLimitAsItemCount = 0;
                        mCacheLimitAsMemory = 0;
                    }
                    if (cacheManager != null)
                        cacheManager.CacheMode = mCacheMode;
                }
            }
        }
        /// <summary>
        /// Gets or sets the cache limit as either the count of thumbnail images or the memory allocated for cache (e.g. 10MB).
        /// </summary>
        [Category("Behavior"), Description("Gets or sets the cache limit as either the count of thumbnail images or the memory allocated for cache (e.g. 10MB)."), DefaultValue("20MB"), RefreshProperties(RefreshProperties.All)]
        public string CacheLimit
        {
            get
            {
                if (mCacheLimitAsMemory != 0)
                    return (mCacheLimitAsMemory / 1024 / 1024).ToString() + "MB";
                else
                    return mCacheLimitAsItemCount.ToString();
            }
            set
            {
                string slimit = value;
                int limit = 0;
                mCacheMode = CacheMode.OnDemand;
                if ((slimit.EndsWith("MB", StringComparison.OrdinalIgnoreCase) &&
                    int.TryParse(slimit.Substring(0, slimit.Length - 2).Trim(), out limit)) ||
                    (slimit.EndsWith("MiB", StringComparison.OrdinalIgnoreCase) &&
                    int.TryParse(slimit.Substring(0, slimit.Length - 3).Trim(), out limit)))
                {
                    mCacheLimitAsItemCount = 0;
                    mCacheLimitAsMemory = limit * 1024 * 1024;
                    if (cacheManager != null)
                        cacheManager.CacheLimitAsMemory = mCacheLimitAsMemory;
                }
                else if (int.TryParse(slimit, out limit))
                {
                    mCacheLimitAsMemory = 0;
                    mCacheLimitAsItemCount = limit;
                    if (cacheManager != null)
                        cacheManager.CacheLimitAsItemCount = mCacheLimitAsItemCount;
                }
                else
                    throw new ArgumentException("Cache limit must be specified as either the count of thumbnail images or the memory allocated for cache (eg 10MB)", "value");
            }
        }
        /// <summary>
        /// Gets or sets the collection of columns of the image list view.
        /// </summary>
        [Category("Appearance"), Description("Gets the collection of columns of the image list view."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ImageListViewColumnHeaderCollection Columns { get { return mColumns; } internal set { mColumns = value; Refresh(); } }
        /// <summary>
        /// Gets or sets the placeholder image.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the placeholder image.")]
        public Image DefaultImage { get { return mDefaultImage; } set { mDefaultImage = value; Refresh(); } }
        /// <summary>
        /// Gets the rectangle that represents the display area of the control.
        /// </summary>
        [Category("Appearance"), Browsable(false), Description("Gets the rectangle that represents the display area of the control.")]
        public override Rectangle DisplayRectangle { get { return layoutManager.ClientArea; } }
        /// <summary>
        /// Gets or sets the error image.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the error image.")]
        public Image ErrorImage { get { return mErrorImage; } set { mErrorImage = value; Refresh(); } }
        /// <summary>
        /// Gets or sets the font of the column headers.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the font of the column headers."), DefaultValue(typeof(Font), "Microsoft Sans Serif; 8.25pt")]
        public Font HeaderFont
        {
            get
            {
                return mHeaderFont;
            }
            set
            {
                if (mHeaderFont != null)
                    mHeaderFont.Dispose();
                mHeaderFont = (Font)value.Clone();
                Refresh();
            }
        }
        /// <summary>
        /// Gets the collection of items contained in the image list view.
        /// </summary>
        [Browsable(false), Category("Behavior"), Description("Gets the collection of items contained in the image list view.")]
        public ImageListView.ImageListViewItemCollection Items { get { return mItems; } }
        /// <summary>
        /// Gets or sets the width of the left pane.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the width of the left pane."), DefaultValue(240)]
        public int PaneWidth
        {
            get
            {
                return mPaneWidth;
            }
            set
            {
                if (mPaneWidth != value)
                {
                    if (mPaneWidth < 2) mPaneWidth = 2;
                    mPaneWidth = value;
                    Refresh();
                }
            }
        }
        /// <summary>
        /// Gets or sets whether the control will retry loading thumbnails on an error.
        /// </summary>
        [Category("Behavior"), Description("Gets or sets whether the control will retry loading thumbnails on an error."), DefaultValue(true)]
        public bool RetryOnError
        {
            get
            {
                return mRetryOnError;
            }
            set
            {
                mRetryOnError = value;
                if (cacheManager != null)
                    cacheManager.RetryOnError = mRetryOnError;
            }
        }
        /// <summary>
        /// Gets the collection of selected items contained in the image list view.
        /// </summary>
        [Browsable(false), Category("Behavior"), Description("Gets the collection of selected items contained in the image list view.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ImageListViewSelectedItemCollection SelectedItems { get { return mSelectedItems; } }
        /// <summary>
        /// Gets or sets the sort column.
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(ColumnType), "Name"), Description("Gets or sets the sort column.")]
        public ColumnType SortColumn
        {
            get
            {
                return mSortColumn;
            }
            set
            {
                if (value != mSortColumn)
                {
                    mSortColumn = value;
                    Sort();
                }
            }
        }
        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(SortOrder), "None"), Description("Gets or sets the sort order.")]
        public SortOrder SortOrder
        {
            get
            {
                return mSortOrder;
            }
            set
            {
                if (value != mSortOrder)
                {
                    mSortOrder = value;
                    Sort();
                }
            }
        }
        /// <summary>
        /// This property is not relevant for this class.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), Bindable(false), DefaultValue(null)]
        public override string Text { get; set; }
        /// <summary>
        /// Gets or sets the size of image thumbnails.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the size of image thumbnails."), DefaultValue(typeof(Size), "96,96")]
        public Size ThumbnailSize
        {
            get
            {
                return mThumbnailSize;
            }
            set
            {
                if (mThumbnailSize != value)
                {
                    mThumbnailSize = value;
                    cacheManager.Clear();
                    Refresh();
                }
            }
        }
        /// <summary>
        /// Gets or sets the embedded thumbnails extraction behavior.
        /// </summary>
        [Category("Behavior"), Description("Gets or sets the embedded thumbnails extraction behavior."), DefaultValue(typeof(UseEmbeddedThumbnails), "Auto")]
        public UseEmbeddedThumbnails UseEmbeddedThumbnails
        {
            get
            {
                return mUseEmbeddedThumbnails;
            }
            set
            {
                if (mUseEmbeddedThumbnails != value)
                {
                    mUseEmbeddedThumbnails = value;
                    cacheManager.Clear();
                    Refresh();
                }
            }
        }
        /// <summary>
        /// Gets or sets the view mode of the image list view.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the view mode of the image list view."), DefaultValue(typeof(View), "Thumbnails")]
        public View View
        {
            get
            {
                return mView;
            }
            set
            {
                mRenderer.SuspendPaint();
                int current = layoutManager.FirstVisible;
                mView = value;
                layoutManager.Update();
                EnsureVisible(current);
                Refresh();
                mRenderer.ResumePaint();
            }
        }
        /// <summary>
        /// Gets or sets the scroll offset.
        /// </summary>
        internal Point ViewOffset { get { return mViewOffset; } set { mViewOffset = value; } }
        /// <summary>
        /// Gets the scroll orientation.
        /// </summary>
        internal ScrollOrientation ScrollOrientation { get { return (mView == View.Gallery ? ScrollOrientation.HorizontalScroll : ScrollOrientation.VerticalScroll); } }
        /// <summary>
        /// Gets the required creation parameters when the control handle is created.
        /// </summary>
        /// <value></value>
        /// <returns>A CreateParams that contains the required creation parameters.</returns>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams p = base.CreateParams;
                p.Style &= ~WS_BORDER;
                p.ExStyle &= ~WS_EX_CLIENTEDGE;
                if (mBorderStyle == BorderStyle.Fixed3D)
                    p.ExStyle |= WS_EX_CLIENTEDGE;
                else if (mBorderStyle == BorderStyle.FixedSingle)
                    p.Style |= WS_BORDER;
                return p;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the ImageListView class.
        /// </summary>
        public ImageListView()
        {
            SetRenderer(new ImageListViewRenderer());

            AllowColumnClick = true;
            AllowColumnResize = true;
            AllowDrag = false;
            AllowDuplicateFileNames = false;
            AllowPaneResize = true;
            BackColor = SystemColors.Window;
            mBorderStyle = BorderStyle.Fixed3D;
            mCacheMode = CacheMode.OnDemand;
            mCacheLimitAsItemCount = 0;
            mCacheLimitAsMemory = 20 * 1024 * 1024;
            mColumns = new ImageListViewColumnHeaderCollection(this);
            ResourceManager manager = new ResourceManager("Manina.Windows.Forms.ImageListViewResources",
                Assembly.GetExecutingAssembly());
            mDefaultImage = manager.GetObject("DefaultImage") as Image;
            mErrorImage = manager.GetObject("ErrorImage") as Image;
            HeaderFont = this.Font;
            mItems = new ImageListViewItemCollection(this);
            mPaneWidth = 240;
            mRetryOnError = true;
            mSelectedItems = new ImageListViewSelectedItemCollection(this);
            mSortColumn = ColumnType.Name;
            mSortOrder = SortOrder.None;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.Opaque |
                ControlStyles.Selectable | ControlStyles.UserMouse, true);
            Text = string.Empty;
            mThumbnailSize = new Size(96, 96);
            mUseEmbeddedThumbnails = UseEmbeddedThumbnails.Auto;
            mView = View.Thumbnails;

            mViewOffset = new Point(0, 0);
            hScrollBar = new HScrollBar();
            vScrollBar = new VScrollBar();
            hScrollBar.Visible = false;
            vScrollBar.Visible = false;
            hScrollBar.Scroll += new ScrollEventHandler(hScrollBar_Scroll);
            vScrollBar.Scroll += new ScrollEventHandler(vScrollBar_Scroll);
            layoutManager = new ImageListViewLayoutManager(this);
            forceRefresh = false;

            navigationManager = new ImageListViewNavigationManager(this);

            cacheManager = new ImageListViewCacheManager(this);
            itemCacheManager = new ImageListViewItemCacheManager(this);

            disposed = false;
        }
        #endregion

        #region Instance Methods
        /// <summary>
        /// Clears the thumbnail cache.
        /// </summary>
        public void ClearThumbnailCache()
        {
            cacheManager.Clear();
            Refresh();
        }
        /// <summary>
        /// Temporarily suspends the layout logic for the control.
        /// </summary>
        public new void SuspendLayout()
        {
            base.SuspendLayout();
            mRenderer.SuspendPaint(true);
        }
        /// <summary>
        /// Resumes usual layout logic.
        /// </summary>
        public new void ResumeLayout()
        {
            ResumeLayout(false);
        }
        /// <summary>
        /// Resumes usual layout logic, optionally forcing an immediate layout of pending layout requests.
        /// </summary>
        /// <param name="performLayout">true to execute pending layout requests; otherwise, false.</param>
        public new void ResumeLayout(bool performLayout)
        {
            base.ResumeLayout(performLayout);
            if (performLayout) Refresh();
            mRenderer.ResumePaint(true);
        }
        /// <summary>
        /// Sets the properties of the specified column header.
        /// </summary>
        /// <param name="type">The column header to modify.</param>
        /// <param name="text">Column header text.</param>
        /// <param name="width">Width (in pixels) of the column header.</param>
        /// <param name="displayIndex">Display index of the column header.</param>
        /// <param name="visible">true if the column header will be shown; otherwise false.</param>
        public void SetColumnHeader(ColumnType type, string text, int width, int displayIndex, bool visible)
        {
            mRenderer.SuspendPaint();
            ImageListViewColumnHeader col = Columns[type];
            col.Text = text;
            col.Width = width;
            col.DisplayIndex = displayIndex;
            col.Visible = visible;
            Refresh();
            mRenderer.ResumePaint();
        }
        /// <summary>
        /// Sets the properties of the specified column header.
        /// </summary>
        /// <param name="type">The column header to modify.</param>
        /// <param name="width">Width (in pixels) of the column header.</param>
        /// <param name="displayIndex">Display index of the column header.</param>
        /// <param name="visible">true if the column header will be shown; otherwise false.</param>
        public void SetColumnHeader(ColumnType type, int width, int displayIndex, bool visible)
        {
            mRenderer.SuspendPaint();
            ImageListViewColumnHeader col = Columns[type];
            col.Width = width;
            col.DisplayIndex = displayIndex;
            col.Visible = visible;
            Refresh();
            mRenderer.ResumePaint();
        }
        /// <summary>
        /// Sets the renderer for this instance.
        /// </summary>
        public void SetRenderer(ImageListViewRenderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException("renderer");

            if (mRenderer != null)
                mRenderer.Dispose();
            mRenderer = renderer;
            mRenderer.mImageListView = this;
            if (layoutManager != null)
                layoutManager.Update(true);
            Refresh();
        }
        /// <summary>
        /// Sorts the items.
        /// </summary>
        public void Sort()
        {
            mItems.Sort();
            Refresh();
        }
        /// <summary>
        /// Marks all items as selected.
        /// </summary>
        public void SelectAll()
        {
            mRenderer.SuspendPaint();

            foreach (ImageListViewItem item in Items)
                item.mSelected = true;

            OnSelectionChangedInternal();

            Refresh();
            mRenderer.ResumePaint();
        }
        /// <summary>
        /// Marks all items as unselected.
        /// </summary>
        public void ClearSelection()
        {
            mRenderer.SuspendPaint();
            mSelectedItems.Clear();
            Refresh();
            mRenderer.ResumePaint();
        }
        /// <summary>
        /// Determines the image list view element under the specified coordinates.
        /// </summary>
        /// <param name="pt">The client coordinates of the point to be tested.</param>
        /// <param name="hitInfo">Details of the hit test.</param>
        public void HitTest(Point pt, out HitInfo hitInfo)
        {
            if (View == View.Details && pt.Y <= mRenderer.MeasureColumnHeaderHeight())
            {
                int i = 0;
                int x = layoutManager.ColumnHeaderBounds.Left;
                ColumnType colIndex = (ColumnType)(-1);
                ColumnType sepIndex = (ColumnType)(-1);
                foreach (ImageListViewColumnHeader col in Columns.GetDisplayedColumns())
                {
                    // Over a column?
                    if (pt.X >= x && pt.X < x + col.Width + SeparatorSize / 2)
                        colIndex = col.Type;

                    // Over a colummn separator?
                    if (pt.X > x + col.Width - SeparatorSize / 2 && pt.X < x + col.Width + SeparatorSize / 2)
                        sepIndex = col.Type;

                    if (colIndex != (ColumnType)(-1)) break;
                    x += col.Width;
                    i++;
                }
                hitInfo = new HitInfo(colIndex, sepIndex);
            }
            else if (View == View.Pane && pt.X <= mPaneWidth)
            {
                bool overBorder = (pt.X >= mPaneWidth - PaneBorderSize);
                hitInfo = new HitInfo(overBorder);
            }
            else
            {
                int itemIndex = -1;

                // Normalize to item area coordinates
                pt.X -= layoutManager.ItemAreaBounds.Left;
                pt.Y -= layoutManager.ItemAreaBounds.Top;

                if (pt.X > 0 && pt.Y > 0)
                {
                    int col = (pt.X + mViewOffset.X) / layoutManager.ItemSizeWithMargin.Width;
                    int row = (pt.Y + mViewOffset.Y) / layoutManager.ItemSizeWithMargin.Height;

                    if (ScrollOrientation == ScrollOrientation.HorizontalScroll ||
                        (ScrollOrientation == ScrollOrientation.VerticalScroll && col <= layoutManager.Cols))
                    {
                        int index = row * layoutManager.Cols + col;
                        if (index >= 0 && index <= Items.Count - 1)
                        {
                            Rectangle bounds = layoutManager.GetItemBounds(index);
                            if (bounds.Contains(pt.X + layoutManager.ItemAreaBounds.Left, pt.Y + layoutManager.ItemAreaBounds.Top))
                                itemIndex = index;
                        }
                    }
                }

                hitInfo = new HitInfo(itemIndex);
            }
        }
        /// <summary>
        /// Scrolls the image list view to ensure that the item with the specified 
        /// index is visible on the screen.
        /// </summary>
        /// <param name="itemIndex">The index of the item to make visible.</param>
        /// <returns>true if the item was made visible; otherwise false (item is already visible or the image list view is empty).</returns>
        public bool EnsureVisible(int itemIndex)
        {
            if (itemIndex == -1) return false;
            if (Items.Count == 0) return false;

            // Already visible?
            Rectangle bounds = layoutManager.ItemAreaBounds;
            Rectangle itemBounds = layoutManager.GetItemBounds(itemIndex);
            if (!bounds.Contains(itemBounds))
            {
                if (ScrollOrientation == ScrollOrientation.HorizontalScroll)
                {
                    int delta = 0;
                    if (itemBounds.Left < bounds.Left)
                        delta = bounds.Left - itemBounds.Left;
                    else
                    {
                        int topItemIndex = itemIndex - (layoutManager.Cols - 1) * layoutManager.Rows;
                        if (topItemIndex < 0) topItemIndex = 0;
                        delta = bounds.Left - layoutManager.GetItemBounds(topItemIndex).Left;
                    }
                    int newXOffset = mViewOffset.X - delta;
                    if (newXOffset > hScrollBar.Maximum - hScrollBar.LargeChange + 1)
                        newXOffset = hScrollBar.Maximum - hScrollBar.LargeChange + 1;
                    if (newXOffset < hScrollBar.Minimum)
                        newXOffset = hScrollBar.Minimum;
                    mViewOffset.X = newXOffset;
                    mViewOffset.Y = 0;
                    hScrollBar.Value = newXOffset;
                    vScrollBar.Value = 0;
                }
                else
                {
                    int delta = 0;
                    if (itemBounds.Top < bounds.Top)
                        delta = bounds.Top - itemBounds.Top;
                    else
                    {
                        int topItemIndex = itemIndex - (layoutManager.Rows - 1) * layoutManager.Cols;
                        if (topItemIndex < 0) topItemIndex = 0;
                        delta = bounds.Top - layoutManager.GetItemBounds(topItemIndex).Top;
                    }
                    int newYOffset = mViewOffset.Y - delta;
                    if (newYOffset > vScrollBar.Maximum - vScrollBar.LargeChange + 1)
                        newYOffset = vScrollBar.Maximum - vScrollBar.LargeChange + 1;
                    if (newYOffset < vScrollBar.Minimum)
                        newYOffset = vScrollBar.Minimum;
                    mViewOffset.X = 0;
                    mViewOffset.Y = newYOffset;
                    hScrollBar.Value = 0;
                    vScrollBar.Value = newYOffset;
                }
                Refresh();
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// Determines whether the specified item is visible on the screen.
        /// </summary>
        /// <param name="item">The item to test.</param>
        /// <returns>An ItemVisibility value.</returns>
        public ItemVisibility IsItemVisible(ImageListViewItem item)
        {
            return IsItemVisible(item.Index);
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Refreshes the control.
        /// </summary>
        internal void Refresh(bool force)
        {
            forceRefresh = force;
            Refresh();
            forceRefresh = false;
        }
        /// <summary>
        /// Determines whether the specified item is visible on the screen.
        /// </summary>
        /// <param name="guid">The Guid of the item to test.</param>
        /// <returns>true if the item is visible or partially visible; otherwise false.</returns>
        internal bool IsItemVisible(Guid guid)
        {
            return layoutManager.IsItemVisible(guid);
        }
        /// <summary>
        /// Determines whether the specified item is visible on the screen.
        /// </summary>
        /// <param name="itemIndex">The index of the item to test.</param>
        /// <returns>An ItemVisibility value.</returns>
        internal ItemVisibility IsItemVisible(int itemIndex)
        {
            if (mItems.Count == 0) return ItemVisibility.NotVisible;
            if (itemIndex < 0 || itemIndex > mItems.Count - 1) return ItemVisibility.NotVisible;

            if (itemIndex < layoutManager.FirstPartiallyVisible || itemIndex > layoutManager.LastPartiallyVisible)
                return ItemVisibility.NotVisible;
            else if (itemIndex >= layoutManager.FirstVisible && itemIndex <= layoutManager.LastVisible)
                return ItemVisibility.Visible;
            else
                return ItemVisibility.PartiallyVisible;
        }
        /// <summary>
        /// Gets the guids of visible items.
        /// </summary>
        internal Dictionary<Guid, bool> GetVisibleItems()
        {
            Dictionary<Guid, bool> visible = new Dictionary<Guid, bool>();
            if (layoutManager.FirstPartiallyVisible != -1 && layoutManager.LastPartiallyVisible != -1)
            {
                int start = layoutManager.FirstPartiallyVisible;
                int end = layoutManager.LastPartiallyVisible;

                start -= layoutManager.Cols * layoutManager.Rows;
                end += layoutManager.Cols * layoutManager.Rows;

                start = Math.Min(mItems.Count - 1, Math.Max(0, start));
                end = Math.Min(mItems.Count - 1, Math.Max(0, end));

                for (int i = start; i <= end; i++)
                    visible.Add(mItems[i].Guid, false);
            }
            return visible;
        }

        #endregion

        #region Event Handlers
        /// <summary>
        /// Handles the CreateControl event.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            Size = new Size(120, 100);
            if (!Controls.Contains(hScrollBar))
            {
                Controls.Add(hScrollBar);
                Controls.Add(vScrollBar);
            }
        }
        /// <summary>
        /// Handles the DragOver event.
        /// </summary>
        protected override void OnDragOver(DragEventArgs e)
        {
            navigationManager.DragOver(e);
            base.OnDragOver(e);
        }
        /// <summary>
        /// Handles the DragEnter event.
        /// </summary>
        protected override void OnDragEnter(DragEventArgs e)
        {
            navigationManager.DragEnter(e);
            base.OnDragEnter(e);
        }
        /// <summary>
        /// Handles the DragLeave event.
        /// </summary>
        protected override void OnDragLeave(EventArgs e)
        {
            navigationManager.DragLeave();
            base.OnDragLeave(e);
        }

        /// <summary>
        /// Handles the DragDrop event.
        /// </summary>
        protected override void OnDragDrop(DragEventArgs e)
        {
            navigationManager.DragDrop(e);
            base.OnDragDrop(e);
        }
        /// <summary>
        /// Handles the Scroll event of the vScrollBar control.
        /// </summary>
        private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            mViewOffset.Y = e.NewValue;
            Refresh();
        }
        /// <summary>
        /// Handles the Scroll event of the hScrollBar control.
        /// </summary>
        private void hScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            mViewOffset.X = e.NewValue;
            Refresh();
        }
        /// <summary>
        /// Handles the Resize event.
        /// </summary>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (!disposed && mRenderer != null)
                mRenderer.RecreateBuffer();

            if (hScrollBar == null)
                return;

            layoutManager.Update();
            Refresh();
        }
        /// <summary>
        /// Handles the Paint event.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!disposed && mRenderer != null)
                mRenderer.Refresh(e.Graphics, forceRefresh);
        }
        /// <summary>
        /// Handles the MouseDown event.
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            // Capture focus if right clicked
            if (!Focused && (e.Button & MouseButtons.Right) == MouseButtons.Right)
                Focus();

            navigationManager.MouseDown(e);
            base.OnMouseDown(e);
        }
        /// <summary>
        /// Handles the MouseUp event.
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            navigationManager.MouseUp(e);
            base.OnMouseUp(e);
        }
        /// <summary>
        /// Handles the MouseMove event.
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            navigationManager.MouseMove(e);
            base.OnMouseMove(e);
        }
        /// <summary>
        /// Handles the MouseWheel event.
        /// </summary>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            mRenderer.SuspendPaint();

            if (ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                int newYOffset = mViewOffset.Y - (e.Delta / 120) * vScrollBar.SmallChange;
                if (newYOffset > vScrollBar.Maximum - vScrollBar.LargeChange + 1)
                    newYOffset = vScrollBar.Maximum - vScrollBar.LargeChange + 1;
                if (newYOffset < 0)
                    newYOffset = 0;
                if (newYOffset < vScrollBar.Minimum) newYOffset = vScrollBar.Minimum;
                if (newYOffset > vScrollBar.Maximum) newYOffset = vScrollBar.Maximum;
                mViewOffset.Y = newYOffset;
                vScrollBar.Value = newYOffset;
            }
            else
            {
                int newXOffset = mViewOffset.X - (e.Delta / 120) * hScrollBar.SmallChange;
                if (newXOffset > hScrollBar.Maximum - hScrollBar.LargeChange + 1)
                    newXOffset = hScrollBar.Maximum - hScrollBar.LargeChange + 1;
                if (newXOffset < 0)
                    newXOffset = 0;
                if (newXOffset < hScrollBar.Minimum) newXOffset = hScrollBar.Minimum;
                if (newXOffset > hScrollBar.Maximum) newXOffset = hScrollBar.Maximum;
                mViewOffset.X = newXOffset;
                hScrollBar.Value = newXOffset;
            }

            Refresh(true);
            mRenderer.ResumePaint();

            base.OnMouseWheel(e);
        }
        /// <summary>
        /// Handles the MouseLeave event.
        /// </summary>
        protected override void OnMouseLeave(EventArgs e)
        {
            navigationManager.MouseLeave();
            base.OnMouseLeave(e);
        }
        /// <summary>
        /// Handles the MouseDoubleClick event.
        /// </summary>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            navigationManager.MouseDoubleClick(e);
            base.OnMouseDoubleClick(e);
        }
        /// <summary>
        /// Handles the IsInputKey event.
        /// </summary>
        protected override bool IsInputKey(Keys keyData)
        {
            if ((keyData & Keys.Left) == Keys.Left ||
                (keyData & Keys.Right) == Keys.Right ||
                (keyData & Keys.Up) == Keys.Up ||
                (keyData & Keys.Down) == Keys.Down)
                return true;
            else
                return base.IsInputKey(keyData);
        }
        /// <summary>
        /// Handles the KeyDown event.
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            navigationManager.KeyDown(e);
            base.OnKeyDown(e);
        }
        /// <summary>
        /// Handles the KeyUp event.
        /// </summary>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            navigationManager.KeyUp(e);
            base.OnKeyUp(e);
        }
        /// <summary>
        /// Handles the GotFocus event.
        /// </summary>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Refresh();
        }
        /// <summary>
        /// Handles the LostFocus event.
        /// </summary>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Refresh();
        }
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.HandleDestroyed"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnHandleDestroyed(EventArgs e)
        {
            cacheManager.Stop();
            itemCacheManager.Stop();
        }
        /// <summary>
        /// Releases the unmanaged resources used by the control and its child controls 
        /// and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; 
        /// false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (mRenderer != null)
                        mRenderer.Dispose();

                    if (mHeaderFont != null)
                        mHeaderFont.Dispose();

                    cacheManager.Dispose();
                    itemCacheManager.Dispose();
                    navigationManager.Dispose();
                }

                disposed = true;
            }

            base.Dispose(disposing);
        }
        #endregion

        #region Virtual Functions
        /// <summary>
        /// Raises the DropFiles event.
        /// </summary>
        /// <param name="e">A DropFileEventArgs that contains event data.</param>
        protected virtual void OnDropFiles(DropFileEventArgs e)
        {
            if (DropFiles != null)
                DropFiles(this, e);

            if (e.Cancel)
                return;

            int index = e.Index;
            int firstItemIndex = 0;
            mSelectedItems.Clear(false);

            // Add items
            foreach (string filename in e.FileNames)
            {
                ImageListViewItem item = new ImageListViewItem(filename);
                item.mSelected = true;
                mItems.InsertInternal(index, item);
                if (firstItemIndex == 0) firstItemIndex = item.Index;
                index++;
            }

            EnsureVisible(firstItemIndex);
            OnSelectionChangedInternal();
        }
        /// <summary>
        /// Raises the ColumnWidthChanged event.
        /// </summary>
        /// <param name="e">A ColumnEventArgs that contains event data.</param>
        protected virtual void OnColumnWidthChanged(ColumnEventArgs e)
        {
            if (ColumnWidthChanged != null)
                ColumnWidthChanged(this, e);
        }
        /// <summary>
        /// Raises the ColumnClick event.
        /// </summary>
        /// <param name="e">A ColumnClickEventArgs that contains event data.</param>
        protected virtual void OnColumnClick(ColumnClickEventArgs e)
        {
            if (ColumnClick != null)
                ColumnClick(this, e);
        }
        /// <summary>
        /// Raises the ColumnHover event.
        /// </summary>
        /// <param name="e">A ColumnClickEventArgs that contains event data.</param>
        protected virtual void OnColumnHover(ColumnHoverEventArgs e)
        {
            if (ColumnHover != null)
                ColumnHover(this, e);
        }
        /// <summary>
        /// Raises the ItemClick event.
        /// </summary>
        /// <param name="e">A ItemClickEventArgs that contains event data.</param>
        protected virtual void OnItemClick(ItemClickEventArgs e)
        {
            if (ItemClick != null)
                ItemClick(this, e);
        }
        /// <summary>
        /// Raises the ItemHover event.
        /// </summary>
        /// <param name="e">A ItemClickEventArgs that contains event data.</param>
        protected virtual void OnItemHover(ItemHoverEventArgs e)
        {
            if (ItemHover != null)
                ItemHover(this, e);
        }
        /// <summary>
        /// Raises the ItemDoubleClick event.
        /// </summary>
        /// <param name="e">A ItemClickEventArgs that contains event data.</param>
        protected virtual void OnItemDoubleClick(ItemClickEventArgs e)
        {
            if (ItemDoubleClick != null)
                ItemDoubleClick(this, e);
        }
        /// <summary>
        /// Raises the SelectionChanged event.
        /// </summary>
        /// <param name="e">A EventArgs that contains event data.</param>
        protected virtual void OnSelectionChanged(EventArgs e)
        {
            if (SelectionChanged != null)
                SelectionChanged(this, e);
        }
        /// <summary>
        /// Raises the SelectionChanged event.
        /// </summary>
        internal void OnSelectionChangedInternal()
        {
            OnSelectionChanged(new EventArgs());
        }
        /// <summary>
        /// Raises the ThumbnailCached event.
        /// </summary>
        /// <param name="e">A ThumbnailCachedEventArgs that contains event data.</param>
        protected virtual void OnThumbnailCached(ThumbnailCachedEventArgs e)
        {
            if (ThumbnailCached != null)
                ThumbnailCached(this, e);
        }
        /// <summary>
        /// Raises the ThumbnailCached event.
        /// This method is invoked from the thumbnail thread.
        /// </summary>
        /// <param name="guid">The guid of the item whose thumbnail is cached.</param>
        /// <param name="error">Determines whether an error occurred during thumbnail extraction.</param>
        internal void OnThumbnailCachedInternal(Guid guid, bool error)
        {
            int itemIndex = Items.IndexOf(guid);
            if (itemIndex != -1)
                OnThumbnailCached(new ThumbnailCachedEventArgs(Items[itemIndex], error));
        }
        /// <summary>
        /// Raises the refresh event.
        /// This method is invoked from the thumbnail thread.
        /// </summary>
        internal void OnRefreshInternal()
        {
            Refresh();
        }
        /// <summary>
        /// Updates item details.
        /// This method is invoked from the item cache thread.
        /// </summary>
        internal void UpdateItemDetailsInternal(ImageListViewItem item, Utility.ShellImageFileInfo info)
        {
            item.UpdateDetailsInternal(info);
        }
        /// <summary>
        /// Updates item details.
        /// This method is invoked from the item cache thread.
        /// </summary>
        internal void UpdateItemDetailsInternal(ImageListViewItem item, VirtualItemDetailsEventArgs info)
        {
            item.UpdateDetailsInternal(info);
        }
        /// <summary>
        /// Raises the ThumbnailCaching event.
        /// </summary>
        /// <param name="e">A ItemEventArgs that contains event data.</param>
        protected virtual void OnThumbnailCaching(ItemEventArgs e)
        {
            if (ThumbnailCaching != null)
                ThumbnailCaching(this, e);
        }
        /// <summary>
        /// Raises the RetrieveVirtualItem event.
        /// </summary>
        /// <param name="e">A VirtualItemThumbnailEventArgs that contains event data.</param>
        protected virtual void OnRetrieveVirtualItemThumbnail(VirtualItemThumbnailEventArgs e)
        {
            if (RetrieveVirtualItemThumbnail != null)
                RetrieveVirtualItemThumbnail(this, e);
        }
        /// <summary>
        /// Raises the RetrieveVirtualItemImage event.
        /// </summary>
        /// <param name="e">A VirtualItemImageEventArgs that contains event data.</param>
        protected virtual void OnRetrieveVirtualItemImage(VirtualItemImageEventArgs e)
        {
            if (RetrieveVirtualItemImage != null)
                RetrieveVirtualItemImage(this, e);
        }
        /// <summary>
        /// Raises the RetrieveVirtualItemDetails event.
        /// </summary>
        /// <param name="e">A VirtualItemDetailsEventArgs that contains event data.</param>
        protected virtual void OnRetrieveVirtualItemDetails(VirtualItemDetailsEventArgs e)
        {
            if (RetrieveVirtualItemDetails != null)
                RetrieveVirtualItemDetails(this, e);
        }
        /// <summary>
        /// Raises the RetrieveVirtualItem event.
        /// This method is invoked from the thumbnail thread.
        /// </summary>
        /// <param name="e">A VirtualItemThumbnailEventArgs that contains event data.</param>
        internal virtual void RetrieveVirtualItemThumbnailInternal(VirtualItemThumbnailEventArgs e)
        {
            OnRetrieveVirtualItemThumbnail(e);
        }
        /// <summary>
        /// Raises the RetrieveVirtualItemImage event.
        /// This method is invoked from the thumbnail thread.
        /// </summary>
        /// <param name="e">A VirtualItemImageEventArgs that contains event data.</param>
        internal virtual void RetrieveVirtualItemImageInternal(VirtualItemImageEventArgs e)
        {
            OnRetrieveVirtualItemImage(e);
        }
        /// <summary>
        /// Raises the RetrieveVirtualItemDetails event.
        /// This method is invoked from the thumbnail thread.
        /// </summary>
        /// <param name="e">A VirtualItemDetailsEventArgs that contains event data.</param>
        internal virtual void RetrieveVirtualItemDetailsInternal(VirtualItemDetailsEventArgs e)
        {
            OnRetrieveVirtualItemDetails(e);
        }
        #endregion

        #region Public Events
        /// <summary>
        /// Occurs after the user drops files on to the control.
        /// </summary>
        [Category("Drag Drop"), Browsable(true), Description("Occurs after the user drops files on to the control.")]
        public event DropFilesEventHandler DropFiles;
        /// <summary>
        /// Occurs after the user successfully resized a column header.
        /// </summary>
        [Category("Action"), Browsable(true), Description("Occurs after the user successfully resized a column header.")]
        public event ColumnWidthChangedEventHandler ColumnWidthChanged;
        /// <summary>
        /// Occurs when the user clicks a column header.
        /// </summary>
        [Category("Action"), Browsable(true), Description("Occurs when the user clicks a column header.")]
        public event ColumnClickEventHandler ColumnClick;
        /// <summary>
        /// Occurs when the user moves the mouse over (and out of) a column header.
        /// </summary>
        [Category("Action"), Browsable(true), Description("Occurs when the user moves the mouse over (and out of) a column header.")]
        public event ColumnHoverEventHandler ColumnHover;
        /// <summary>
        /// Occurs when the user clicks an item.
        /// </summary>
        [Category("Action"), Browsable(true), Description("Occurs when the user clicks an item.")]
        public event ItemClickEventHandler ItemClick;
        /// <summary>
        /// Occurs when the user moves the mouse over (and out of) an item.
        /// </summary>
        [Category("Action"), Browsable(true), Description("Occurs when the user moves the mouse over (and out of) an item.")]
        public event ItemHoverEventHandler ItemHover;
        /// <summary>
        /// Occurs when the user double-clicks an item.
        /// </summary>
        [Category("Action"), Browsable(true), Description("Occurs when the user double-clicks an item.")]
        public event ItemDoubleClickEventHandler ItemDoubleClick;
        /// <summary>
        /// Occurs when the selected items collection changes.
        /// </summary>
        [Category("Behavior"), Browsable(true), Description("Occurs when the selected items collection changes.")]
        public event EventHandler SelectionChanged;
        /// <summary>
        /// Occurs after an item thumbnail is cached.
        /// </summary>
        [Category("Behavior"), Browsable(true), Description("Occurs after an item thumbnail is cached.")]
        public event ThumbnailCachedEventHandler ThumbnailCached;
        /// <summary>
        /// Occurs before an item thumbnail is cached.
        /// </summary>
        [Category("Behavior"), Browsable(true), Description("Occurs before an item thumbnail is cached.")]
        public event ThumbnailCachingEventHandler ThumbnailCaching;
        /// <summary>
        /// Occurs when thumbnail image for a virtual item is requested.
        /// The lifetime of the image will be controlled by the control.
        /// This event will be run in the worker thread context.
        /// </summary>
        [Category("Behavior"), Browsable(true), Description("Occurs when thumbnail image for a virtual item is requested.")]
        public event RetrieveVirtualItemThumbnailEventHandler RetrieveVirtualItemThumbnail;
        /// <summary>
        /// Occurs when source image for a virtual item is requested.
        /// The lifetime of the image will be controlled by the control.
        /// This event will be run in the worker thread context.
        /// </summary>
        [Category("Behavior"), Browsable(true), Description("Occurs when source image for a virtual item is requested.")]
        public event RetrieveVirtualItemImageEventHandler RetrieveVirtualItemImage;
        /// <summary>
        /// Occurs when details of a virtual item are requested.
        /// This event will be run in the worker thread context.
        /// </summary>
        [Category("Behavior"), Browsable(true), Description("Occurs when details of a virtual item are requested.")]
        public event RetrieveVirtualItemDetailsEventHandler RetrieveVirtualItemDetails;
        #endregion
    }
}
