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
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace Manina.Windows.Forms
{
    /// <summary>
    /// Represents an item in the image list view.
    /// </summary>
    public class ImageListViewItem
    {
        #region Member Variables
        // Property backing fields
        internal int mIndex;
        private Color mBackColor;
        private Color mForeColor;
        private Guid mGuid;
        internal ImageListView mImageListView;
        internal bool mSelected;
        private string mText;
        private int mZOrder;
        // File info
        private DateTime mDateAccessed;
        private DateTime mDateCreated;
        private DateTime mDateModified;
        private string mFileType;
        private string mFileName;
        private string mFilePath;
        private long mFileSize;
        private Size mDimensions;
        private SizeF mResolution;
        // Exif tags
        private string mImageDescription;
        private string mEquipmentModel;
        private DateTime mDateTaken;
        private string mArtist;
        private string mCopyright;
        private string mExposureTime;
        private float mFNumber;
        private ushort mISOSpeed;
        private string mShutterSpeed;
        private string mAperture;
        private string mUserComment;
        // Used for virtual items
        internal bool isVirtualItem;
        internal object mVirtualItemKey;

        internal ImageListView.ImageListViewItemCollection owner;
        internal bool isDirty;
        private bool editing;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the background color of the item.
        /// </summary>
        [Category("Appearance"), Browsable(true), Description("Gets or sets the background color of the item."), DefaultValue(typeof(Color), "Transparent")]
        public Color BackColor
        {
            get
            {
                return mBackColor;
            }
            set
            {
                if (value != mBackColor)
                {
                    mBackColor = value;
                    if (mImageListView != null)
                        mImageListView.Refresh();
                }
            }
        }
        /// <summary>
        /// Gets the cache state of the item thumbnail.
        /// </summary>
        [Category("Behavior"), Browsable(false), Description("Gets the cache state of the item thumbnail.")]
        public CacheState ThumbnailCacheState { get { return mImageListView.cacheManager.GetCacheState(mGuid); } }
        /// <summary>
        /// Gets a value determining if the item is focused.
        /// </summary>
        [Category("Appearance"), Browsable(false), Description("Gets a value determining if the item is focused.")]
        public bool Focused
        {
            get
            {
                if (owner == null || owner.FocusedItem == null) return false;
                return (this == owner.FocusedItem);
            }
            set
            {
                if (owner != null)
                    owner.FocusedItem = this;
            }
        }
        /// <summary>
        /// Gets or sets the foreground color of the item.
        /// </summary>
        [Category("Appearance"), Browsable(true), Description("Gets or sets the foreground color of the item."), DefaultValue(typeof(Color), "WindowText")]
        public Color ForeColor
        {
            get
            {
                return mForeColor;
            }
            set
            {
                if (value != mForeColor)
                {
                    mForeColor = value;
                    if (mImageListView != null)
                        mImageListView.Refresh();
                }
            }
        }
        /// <summary>
        /// Gets the unique identifier for this item.
        /// </summary>
        [Category("Behavior"), Browsable(false), Description("Gets the unique identifier for this item.")]
        internal Guid Guid { get { return mGuid; } private set { mGuid = value; } }
        /// <summary>
        /// Gets the virtual item key associated with this item.
        /// Returns null if the item is not a virtual item.
        /// </summary>
        [Category("Behavior"), Browsable(false), Description("Gets the virtual item key associated with this item.")]
        public object VirtualItemKey { get { return mVirtualItemKey; } }
        /// <summary>
        /// Gets the ImageListView owning this item.
        /// </summary>
        [Category("Behavior"), Browsable(false), Description("Gets the ImageListView owning this item.")]
        public ImageListView ImageListView { get { return mImageListView; } private set { mImageListView = value; } }
        /// <summary>
        /// Gets the index of the item.
        /// </summary>
        [Category("Behavior"), Browsable(false), Description("Gets the index of the item."), EditorBrowsable(EditorBrowsableState.Advanced)]
        public int Index { get { return mIndex; } }
        /// <summary>
        /// Gets or sets a value determining if the item is selected.
        /// </summary>
        [Category("Appearance"), Browsable(true), Description("Gets or sets a value determining if the item is selected."), DefaultValue(false)]
        public bool Selected
        {
            get
            {
                return mSelected;
            }
            set
            {
                if (value != mSelected)
                {
                    mSelected = value;
                    if (mImageListView != null)
                        mImageListView.OnSelectionChangedInternal();
                }
            }
        }
        /// <summary>
        /// Gets or sets the user-defined data associated with the item.
        /// </summary>
        [Category("Data"), Browsable(true), Description("Gets or sets the user-defined data associated with the item.")]
        public object Tag { get; set; }
        /// <summary>
        /// Gets or sets the text associated with this item. If left blank, item Text 
        /// reverts to the name of the image file.
        /// </summary>
        [Category("Appearance"), Browsable(true), Description("Gets or sets the text associated with this item. If left blank, item Text reverts to the name of the image file.")]
        public string Text
        {
            get
            {
                return mText;
            }
            set
            {
                mText = value;
                if (mImageListView != null)
                    mImageListView.Refresh();
            }
        }
        /// <summary>
        /// Gets the thumbnail image. If the thumbnail image is not cached, it will be 
        /// added to the cache queue and DefaultImage of the owner image list view will
        /// be returned. If the thumbnail could not be cached ErrorImage of the owner
        /// image list view will be returned.
        /// </summary>
        [Category("Appearance"), Browsable(false), Description("Gets the thumbnail image.")]
        public Image ThumbnailImage
        {
            get
            {
                if (mImageListView == null)
                    throw new InvalidOperationException("Owner control is null.");

                CacheState state = ThumbnailCacheState;
                if (state == CacheState.Error)
                    return mImageListView.ErrorImage;

                Image img = mImageListView.cacheManager.GetImage(Guid);
                if (img != null)
                    return img;

                if (isVirtualItem)
                    mImageListView.cacheManager.Add(Guid, mVirtualItemKey, mImageListView.ThumbnailSize, mImageListView.UseEmbeddedThumbnails);
                else
                    mImageListView.cacheManager.Add(Guid, FileName, mImageListView.ThumbnailSize, mImageListView.UseEmbeddedThumbnails);
                return mImageListView.DefaultImage;
            }
        }
        /// <summary>
        /// Gets or sets the draw order of the item.
        /// </summary>
        [Category("Appearance"), Browsable(true), Description("Gets or sets the draw order of the item."), DefaultValue(0)]
        public int ZOrder { get { return mZOrder; } set { mZOrder = value; } }
        /// <summary>
        /// Gets the last access date of the image file represented by this item.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the last access date of the image file represented by this item.")]
        public DateTime DateAccessed { get { UpdateFileInfo(); return mDateAccessed; } }
        /// <summary>
        /// Gets the creation date of the image file represented by this item.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the creation date of the image file represented by this item.")]
        public DateTime DateCreated { get { UpdateFileInfo(); return mDateCreated; } }
        /// <summary>
        /// Gets the modification date of the image file represented by this item.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the modification date of the image file represented by this item.")]
        public DateTime DateModified { get { UpdateFileInfo(); return mDateModified; } }
        /// <summary>
        /// Gets the shell type of the image file represented by this item.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the shell type of the image file represented by this item.")]
        public string FileType { get { UpdateFileInfo(); return mFileType; } }
        /// <summary>
        /// Gets or sets the name of the image fie represented by this item.
        /// </summary>        
        [Category("Data"), Browsable(false), Description("Gets or sets the name of the image fie represented by this item.")]
        public string FileName
        {
            get
            {
                return mFileName;
            }
            set
            {
                if (mFileName != value)
                {
                    mFileName = value;
                    if (!isVirtualItem)
                    {
                        isDirty = true;
                        if (mImageListView != null)
                        {
                            mImageListView.cacheManager.Remove(Guid);
                            mImageListView.itemCacheManager.Add(this);
                            mImageListView.Refresh();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Gets the path of the image fie represented by this item.
        /// </summary>        
        [Category("Data"), Browsable(false), Description("Gets the path of the image fie represented by this item.")]
        public string FilePath { get { UpdateFileInfo(); return mFilePath; } }
        /// <summary>
        /// Gets file size in bytes.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets file size in bytes.")]
        public long FileSize { get { UpdateFileInfo(); return mFileSize; } }
        /// <summary>
        /// Gets image dimensions.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets image dimensions.")]
        public Size Dimensions { get { UpdateFileInfo(); return mDimensions; } }
        /// <summary>
        /// Gets image resolution in pixels per inch.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets image resolution in pixels per inch.")]
        public SizeF Resolution { get { UpdateFileInfo(); return mResolution; } }
        /// <summary>
        /// Gets image deascription.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets image deascription.")]
        public string ImageDescription { get { UpdateFileInfo(); return mImageDescription; } }
        /// <summary>
        /// Gets the camera model.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the camera model.")]
        public string EquipmentModel { get { UpdateFileInfo(); return mEquipmentModel; } }
        /// <summary>
        /// Gets the date and time the image was taken.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the date and time the image was taken.")]
        public DateTime DateTaken { get { UpdateFileInfo(); return mDateTaken; } }
        /// <summary>
        /// Gets the name of the artist.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the name of the artist.")]
        public string Artist { get { UpdateFileInfo(); return mArtist; } }
        /// <summary>
        /// Gets image copyright information.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets image copyright information.")]
        public string Copyright { get { UpdateFileInfo(); return mCopyright; } }
        /// <summary>
        /// Gets the exposure time in seconds.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the exposure time in seconds.")]
        public string ExposureTime { get { UpdateFileInfo(); return mExposureTime; } }
        /// <summary>
        /// Gets the F number.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the F number.")]
        public float FNumber { get { UpdateFileInfo(); return mFNumber; } }
        /// <summary>
        /// Gets the ISO speed.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the ISO speed.")]
        public ushort ISOSpeed { get { UpdateFileInfo(); return mISOSpeed; } }
        /// <summary>
        /// Gets the shutter speed.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the shutter speed.")]
        public string ShutterSpeed { get { UpdateFileInfo(); return mShutterSpeed; } }
        /// <summary>
        /// Gets the lens aperture value.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets the lens aperture value.")]
        public string Aperture { get { UpdateFileInfo(); return mAperture; } }
        /// <summary>
        /// Gets user comments.
        /// </summary>
        [Category("Data"), Browsable(false), Description("Gets user comments.")]
        public string UserComment { get { UpdateFileInfo(); return mUserComment; } }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the ImageListViewItem class.
        /// </summary>
        public ImageListViewItem()
        {
            mIndex = -1;
            owner = null;

            mBackColor = Color.Transparent;
            mForeColor = SystemColors.WindowText;
            mZOrder = 0;

            Guid = Guid.NewGuid();
            ImageListView = null;
            Selected = false;

            isDirty = true;
            editing = false;

            mVirtualItemKey = null;
            isVirtualItem = false;
        }
        /// <summary>
        /// Initializes a new instance of the ImageListViewItem class.
        /// </summary>
        /// <param name="filename">The image filename representing the item.</param>
        public ImageListViewItem(string filename)
            : this()
        {
            mFileName = filename;
            mText = Path.GetFileName(filename);
        }
        /// <summary>
        /// Initializes a new instance of a virtual ImageListViewItem class.
        /// </summary>
        /// <param name="key">The key identifying this item.</param>
        /// <param name="text">Text of this item.</param>
        /// <param name="dimensions">Pixel dimensions of the source image.</param>
        public ImageListViewItem(object key, string text, Size dimensions)
            : this()
        {
            isVirtualItem = true;
            mVirtualItemKey = key;
            mText = text;
            mDimensions = dimensions;
        }
        /// <summary>
        /// Initializes a new instance of a virtual ImageListViewItem class.
        /// </summary>
        /// <param name="key">The key identifying this item.</param>
        /// <param name="text">Text of this item.</param>
        public ImageListViewItem(object key, string text)
            : this(key, text, Size.Empty)
        {
            ;
        }
        /// <summary>
        /// Initializes a new instance of a virtual ImageListViewItem class.
        /// </summary>
        /// <param name="key">The key identifying this item.</param>
        public ImageListViewItem(object key)
            : this(key, string.Empty, Size.Empty)
        {
            ;
        }
        #endregion

        #region Instance Methods
        /// <summary>
        /// Gets the item image.
        /// </summary>
        public Image GetImage()
        {
            if (!editing) BeginEdit();
            Image img = null;
            if (isVirtualItem)
            {
                VirtualItemImageEventArgs e = new VirtualItemImageEventArgs(mVirtualItemKey);
                mImageListView.RetrieveVirtualItemImageInternal(e);
                img = Image.FromFile(e.FileName);
            }
            else
                img = Image.FromFile(mFileName);
            if (!editing) EndEdit();
            return img;
        }
        /// <summary>
        /// Begins editing the item.
        /// This method must be used while editing the item
        /// to prevent collisions with the cache manager.
        /// </summary>
        public void BeginEdit()
        {
            if (editing == true)
                throw new InvalidOperationException("Already editing this item.");

            if (mImageListView == null)
                throw new InvalidOperationException("Owner control is null.");

            UpdateFileInfo();
            mImageListView.cacheManager.BeginItemEdit(mGuid, mFileName);
            mImageListView.itemCacheManager.BeginItemEdit(mGuid);

            editing = true;
        }
        /// <summary>
        /// Ends editing and updates the item.
        /// </summary>
        /// <param name="update">If set to true, the item will be immediately updated.</param>
        public void EndEdit(bool update)
        {
            if (editing == false)
                throw new InvalidOperationException("This item is not being edited.");

            if (mImageListView == null)
                throw new InvalidOperationException("Owner control is null.");

            mImageListView.cacheManager.EndItemEdit(mGuid);
            mImageListView.itemCacheManager.EndItemEdit(mGuid);

            editing = false;
            if (update) Update();
        }
        /// <summary>
        /// Ends editing and updates the item.
        /// </summary>
        public void EndEdit()
        {
            EndEdit(true);
        }
        /// <summary>
        /// Updates item thumbnail and item details.
        /// </summary>
        public void Update()
        {
            isDirty = true;
            if (mImageListView != null)
            {
                mImageListView.cacheManager.Remove(mGuid, true);
                mImageListView.itemCacheManager.Add(this);
                mImageListView.Refresh();
            }
        }
        /// <summary>
        /// Returns the sub item item text corresponding to the specified column type.
        /// </summary>
        /// <param name="type">The type of information to return.</param>
        /// <returns>Formatted text for the given column type.</returns>
        public string GetSubItemText(ColumnType type)
        {
            switch (type)
            {
                case ColumnType.DateAccessed:
                    if (DateAccessed == DateTime.MinValue)
                        return "";
                    else
                        return DateAccessed.ToString("g");
                case ColumnType.DateCreated:
                    if (DateCreated == DateTime.MinValue)
                        return "";
                    else
                        return DateCreated.ToString("g");
                case ColumnType.DateModified:
                    if (DateModified == DateTime.MinValue)
                        return "";
                    else
                        return DateModified.ToString("g");
                case ColumnType.FileName:
                    return FileName;
                case ColumnType.Name:
                    return Text;
                case ColumnType.FilePath:
                    return FilePath;
                case ColumnType.FileSize:
                    if (FileSize == 0)
                        return "";
                    else
                        return Utility.FormatSize(FileSize);
                case ColumnType.FileType:
                    return FileType;
                case ColumnType.Dimensions:
                    if (Dimensions == Size.Empty)
                        return "";
                    else
                        return string.Format("{0} x {1}", Dimensions.Width, Dimensions.Height);
                case ColumnType.Resolution:
                    if (Resolution == SizeF.Empty)
                        return "";
                    else
                        return string.Format("{0} x {1}", Resolution.Width, Resolution.Height);
                case ColumnType.ImageDescription:
                    return ImageDescription;
                case ColumnType.EquipmentModel:
                    return EquipmentModel;
                case ColumnType.DateTaken:
                    if (DateTaken == DateTime.MinValue)
                        return "";
                    else
                        return DateTaken.ToString("g");
                case ColumnType.Artist:
                    return Artist;
                case ColumnType.Copyright:
                    return Copyright;
                case ColumnType.ExposureTime:
                    return ExposureTime;
                case ColumnType.FNumber:
                    if (FNumber == 0.0f)
                        return "";
                    else
                        return FNumber.ToString("f2");
                case ColumnType.ISOSpeed:
                    if (ISOSpeed == 0)
                        return "";
                    else
                        return ISOSpeed.ToString();
                case ColumnType.ShutterSpeed:
                    return ShutterSpeed;
                case ColumnType.Aperture:
                    return Aperture;
                case ColumnType.UserComment:
                    return UserComment;
                default:
                    throw new ArgumentException("Unknown column type", "type");
            }
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Updates file info for the image file represented by this item.
        /// </summary>
        private void UpdateFileInfo()
        {
            if (!isDirty) return;

            if (isVirtualItem)
            {
                if (mImageListView != null)
                {
                    VirtualItemDetailsEventArgs e = new VirtualItemDetailsEventArgs(mVirtualItemKey);
                    mImageListView.RetrieveVirtualItemDetailsInternal(e);
                    UpdateDetailsInternal(e);
                }
            }
            else
            {
                Utility.ShellImageFileInfo info = new Utility.ShellImageFileInfo(mFileName);
                UpdateDetailsInternal(info);
            }
            isDirty = false;
        }
        /// <summary>
        /// Invoked by the worker thread to update item details.
        /// </summary>
        internal void UpdateDetailsInternal(Utility.ShellImageFileInfo info)
        {
            if (!isDirty) return;

            mDateAccessed = info.LastAccessTime;
            mDateCreated = info.CreationTime;
            mDateModified = info.LastWriteTime;
            mFileSize = info.Size;
            mFileType = info.TypeName;
            mFilePath = info.DirectoryName;
            mDimensions = info.Dimensions;
            mResolution = info.Resolution;
            // Exif tags
            mImageDescription = info.ImageDescription;
            mEquipmentModel = info.EquipmentModel;
            mDateTaken = info.DateTaken;
            mArtist = info.Artist;
            mCopyright = info.Copyright;
            mExposureTime = info.ExposureTime;
            mFNumber = info.FNumber;
            mISOSpeed = info.ISOSpeed;
            mShutterSpeed = info.ShutterSpeed;
            mAperture = info.ApertureValue;
            mUserComment = info.UserComment;

            isDirty = false;
        }
        /// <summary>
        /// Invoked by the worker thread to update item details.
        /// </summary>
        internal void UpdateDetailsInternal(VirtualItemDetailsEventArgs info)
        {
            if (!isDirty) return;

            mDateAccessed = info.DateAccessed;
            mDateCreated = info.DateCreated;
            mDateModified = info.DateModified;
            mFileName = info.FileName;
            mFileSize = info.FileSize;
            mFileType = info.FileType;
            mFilePath = info.FilePath;
            mDimensions = info.Dimensions;
            mResolution = info.Resolution;
            // Exif tags
            mImageDescription = info.ImageDescription;
            mEquipmentModel = info.EquipmentModel;
            mDateTaken = info.DateTaken;
            mArtist = info.Artist;
            mCopyright = info.Copyright;
            mExposureTime = info.ExposureTime;
            mFNumber = info.FNumber;
            mISOSpeed = info.ISOSpeed;
            mShutterSpeed = info.ShutterSpeed;
            mAperture = info.Aperture;
            mUserComment = info.UserComment;

            isDirty = false;
        }
        #endregion
    }
}
