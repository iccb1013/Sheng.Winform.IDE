using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;
using Sheng.SailingEase.Drawing;

namespace Sheng.SailingEase.Controls
{
    /// <summary>
    /// SEToolStrip 上的图像尺寸
    /// </summary>
    public enum SEToolStripImageSize
    {
        /// <summary>
        /// An image of 16x16 pixels.
        /// </summary>
        [LocalizedDescription("SEToolStripImageSize_Small")]
        Small,
        /// <summary>
        /// An image of 24x24 pixels.
        /// </summary>
        [LocalizedDescription("SEToolStripImageSize_Medium")]
        Medium,
        /// <summary>
        /// An image of 32x32 pixels.
        /// </summary>
        [LocalizedDescription("SEToolStripImageSize_Large")]
        Large,
        /// <summary>
        /// An image of 48x48 pixels.
        /// </summary>
        [LocalizedDescription("SEToolStripImageSize_ExtraLarge")]
        ExtraLarge,
    }

    /// <summary>
    /// Implementations of this interface must indicate whether or not the
    /// requested image size is supported. A default image override could
    /// be enforced should the provider report with no support.
    /// </summary>
    public interface ISEToolStripImageProvider
    {
        #region Methods

        /// <summary>
        /// Queries the image provider for support of a specific size.
        /// </summary>
        /// <param name="size">Indicated image size.</param>
        /// <returns>Returns true when the requested size is supported.</returns>
        bool IsImageSupported(SEToolStripImageSize size);

        /// <summary>
        /// Fetches an image of the requested size.
        /// </summary>
        /// <param name="size">Size of image to obtain.</param>
        /// <returns>If supported, returns requested image. A value of null
        /// indicates that the requested size is not supported.</returns>
        Image GetImage(SEToolStripImageSize size);

        #endregion
    }

    /// <summary>
    /// Implementations of this interface can provide access to multiple
    /// different images of multiple different sizes. A default image override
    /// could be enforced should the provider report with no support.
    /// </summary>
    public interface ISEToolStripMultipleImageProvider
    {
        #region Methods

        /// <summary>
        /// Queries the image provider for support of a specific size.
        /// </summary>
        /// <param name="key">Key used to identify an image.</param>
        /// <param name="size">Indicated image size</param>
        /// <returns>Returns true when the requested size is supported.</returns>
        bool IsImageSupported(object key, SEToolStripImageSize size);

        /// <summary>
        /// Fetches an image of the requested size.
        /// </summary>
        /// <param name="key">Key used to identify an image.</param>
        /// <param name="size">Size of image to obtain.</param>
        /// <returns>If supported, returns requested image. A value of null
        /// indicates that the requested size is not supported.</returns>
        Image GetImage(object key, SEToolStripImageSize size);

        #endregion

        #region Properties

        /// <summary>
        /// Gets count of registered image providers.
        /// </summary>
        int ImageProviderCount { get; }

        #endregion
    }

    /// <summary>
    /// Provides a collection which pairs image providers with a key object.
    /// The collection can be used to provide access to different images of
    /// different sizes.
    /// </summary>
    public class SEToolStripImageProviderCollection : Dictionary<object, ISEToolStripImageProvider>, ISEToolStripMultipleImageProvider
    {
        #region Construction and destruction

        public SEToolStripImageProviderCollection()
            : base()
        {
        }

        public SEToolStripImageProviderCollection(int capacity)
            : base(capacity)
        {
        }

        public SEToolStripImageProviderCollection(SEToolStripImageProviderCollection collection)
            : base(collection)
        {
        }

        public SEToolStripImageProviderCollection(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion

        #region IMultipleImageProvider Members

        /// <summary>
        /// Queries the image provider for support of a specific size.
        /// </summary>
        /// <param name="key">Key used to identify an image.</param>
        /// <param name="size">Indicated image size</param>
        /// <returns>Returns true when the requested size is supported.</returns>
        public bool IsImageSupported(object key, SEToolStripImageSize size)
        {
            if (!this.ContainsKey(key))
                return false;
            return this[key].IsImageSupported(size);
        }

        /// <summary>
        /// Fetches an image of the requested size.
        /// </summary>
        /// <param name="key">Key used to identify an image.</param>
        /// <param name="size">Size of image to obtain.</param>
        /// <returns>If supported, returns requested image. A value of null
        /// indicates that the requested size is not supported.</returns>
        public Image GetImage(object key, SEToolStripImageSize size)
        {
            if (!this.ContainsKey(key))
                throw new NullReferenceException();
            return this[key].GetImage(size);
        }

        /// <summary>
        /// Gets count of registered image providers.
        /// </summary>
        int ISEToolStripMultipleImageProvider.ImageProviderCount
        {
            get { return Count; }
        }

        #endregion
    }

    /// <summary>
    /// Allows an icon to be used to provide images of different sizes.
    /// </summary>
    public class SEToolStripIconImageProvider : ISEToolStripImageProvider
    {
        #region Construction and destruction

        public SEToolStripIconImageProvider(Icon icon)
        {
            this.m_sourceIcon = icon;
        }

        public SEToolStripIconImageProvider(System.IO.Stream stream)
        {
            this.m_sourceIcon = new Icon(stream);
        }

        public SEToolStripIconImageProvider(string fileName)
        {
            this.m_sourceIcon = new Icon(fileName);
        }

        public SEToolStripIconImageProvider(Type type, string resource)
        {
            this.m_sourceIcon = new Icon(type, resource);
        }

        #endregion

        #region Global Utility Methods

        /// <summary>
        /// A utility method which transforms an enumerated image value into
        /// a two-dimensional size.
        /// </summary>
        /// <param name="size">Requested image size.</param>
        /// <returns>Returns a two-dimensional size.</returns>
        public static Size GetIconSize(SEToolStripImageSize size)
        {
            switch (size)
            {
                case SEToolStripImageSize.Small:
                    return new Size(16, 16);
                case SEToolStripImageSize.Medium:
                    return new Size(24, 24);
                case SEToolStripImageSize.Large:
                    return new Size(32, 32);
                case SEToolStripImageSize.ExtraLarge:
                    return new Size(48, 48);
                default:
                    throw new NotSupportedException("Invalid image size requested.");
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Raised when the icon property is changed.
        /// </summary>
        public SEToolStripOldNewEventHandler<Icon> IconChanged;

        /// <summary>
        /// Invoked the 'IconChanged' event.
        /// </summary>
        /// <param name="e">Provides access to old and new icons.</param>
        public virtual void OnIconChanged(SEToolStripOldNewEventArgs<Icon> e)
        {
            if (this.IconChanged != null)
                this.IconChanged(this, e);
        }

        #endregion

        #region IImageProvider Members

        /// <summary>
        /// Queries the image provider for support of a specific size.
        /// </summary>
        /// <param name="size">Indicated image size.</param>
        /// <returns>Returns true when the requested size is supported.</returns>
        public bool IsImageSupported(SEToolStripImageSize size)
        {
            return true;
        }

        /// <summary>
        /// Fetches an image of the requested size.
        /// </summary>
        /// <param name="size">Size of image to obtain.</param>
        /// <returns>If supported, returns requested image. A value of null
        /// indicates that the requested size is not supported.</returns>
        public Image GetImage(SEToolStripImageSize size)
        {
            Icon desiredSize = new Icon(SourceIcon, SEToolStripIconImageProvider.GetIconSize(size));
            return desiredSize.ToBitmap();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the source icon.
        /// </summary>
        public Icon SourceIcon
        {
            get { return this.m_sourceIcon; }
            set
            {
                if (value != this.m_sourceIcon)
                {
                    Icon oldIcon = this.m_sourceIcon;
                    this.m_sourceIcon = value;
                    OnIconChanged(new SEToolStripOldNewEventArgs<Icon>(oldIcon, value));
                }
            }
        }

        #endregion

        #region Attributes

        private Icon m_sourceIcon;

        #endregion
    }

    /// <summary>
    /// Provides event handlers with the current and proposed image sizes.
    /// Event handlers can decide whether or not to cancel this procedure.
    /// </summary>
    public class SEToolStripImageSizeChangingEventArgs : CancelEventArgs
    {
        #region Construction and destruction

        public SEToolStripImageSizeChangingEventArgs(SEToolStripImageSize oldValue, SEToolStripImageSize newValue)
            : base(false)
        {
            this.m_currentValue = oldValue;
            this.m_newValue = newValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current image size.
        /// </summary>
        public SEToolStripImageSize CurrentValue
        {
            get { return this.m_currentValue; }
        }

        /// <summary>
        /// Gets the proposed image size.
        /// </summary>
        public SEToolStripImageSize NewValue
        {
            get { return this.m_newValue; }
        }

        #endregion

        #region Attributes

        private SEToolStripImageSize m_currentValue;
        private SEToolStripImageSize m_newValue;

        #endregion
    }

    public delegate void SEToolStripImageSizeChangingEventHandler(object sender, SEToolStripImageSizeChangingEventArgs e);

    /// <summary>
    /// This toolstrip implements the multiple image provider interface and thus
    /// provides support for automatic image changes based upon the selected
    /// image size.
    /// </summary>
    [LicenseProvider(typeof(SEControlLicenseProvider))]
    public class SEToolStrip : ToolStrip, ISEToolStripMultipleImageProvider, ISEToolStripImageProvider
    {
        #region Construction and destruction

        public SEToolStrip()
        {
            LicenseManager.Validate(typeof(SEToolStrip)); 

            this.m_imageProvider = new SEToolStripImageProviderCollection();
            this.m_defaultProvider = this;
        }

        #endregion

        #region Events

        /// <summary>
        /// Raised when image size is about to be changed.
        /// </summary>
        public event SEToolStripImageSizeChangingEventHandler ImageSizeChanging;
        /// <summary>
        /// Raised when image size is changed.
        /// </summary>
        public event SEToolStripOldNewEventHandler<SEToolStripImageSize> ImageSizeChanged;
        /// <summary>
        /// Raised when the property 'UseUnknownImageSizeIcon' is changed.
        /// </summary>
        public event EventHandler UseUnknownImageSizeIconChanged;

        /// <summary>
        /// Invokes the 'ImageSizeChanging' event handler.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        /// <returns>Returns true when proposed change is accepted.</returns>
        protected virtual bool OnImageSizeChanging(SEToolStripImageSizeChangingEventArgs e)
        {
            if (e.Cancel)
                return false;

            SEToolStripImageSizeChangingEventHandler handler = this.ImageSizeChanging;
            if (handler != null)
            {
                handler(this, e);
                return !e.Cancel;
            }

            return true;
        }

        /// <summary>
        /// Invokes the 'ImageSizeChanged' event handler.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnImageSizeChanged(SEToolStripOldNewEventArgs<SEToolStripImageSize> e)
        {
            SEToolStripOldNewEventHandler<SEToolStripImageSize> handler = this.ImageSizeChanged;
            if (handler != null)
                this.ImageSizeChanged(this, e);
        }

        /// <summary>
        /// Invokes the 'UseUnknownImageSizeIconChanged' event handler.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnUseUnknownImageSizeIconChanged(EventArgs e)
        {
            // Refresh toolstrip images from providers?
            if (!IsUpdatingImages)
                RefreshItemImages();

            // Raise the associated event handler.
            EventHandler handler = this.UseUnknownImageSizeIconChanged;
            if (handler != null)
                this.UseUnknownImageSizeIconChanged(this, e);
        }

        #endregion

        #region IMultipleImageProvider Members

        /// <summary>
        /// Queries the image provider for support of a specific size.
        /// </summary>
        /// <param name="key">Key used to identify an image.</param>
        /// <param name="size">Indicated image size</param>
        /// <returns>Returns true when the requested size is supported.</returns>
        public virtual bool IsImageSupported(object key, SEToolStripImageSize size)
        {
            return ImageProvider.IsImageSupported(key, size);
        }

        /// <summary>
        /// Fetches an image of the requested size.
        /// </summary>
        /// <param name="key">Key used to identify an image.</param>
        /// <param name="size">Size of image to obtain.</param>
        /// <returns>If supported, returns requested image. A value of null
        /// indicates that the requested size is not supported.</returns>
        public virtual Image GetImage(object key, SEToolStripImageSize size)
        {
            return ImageProvider.GetImage(key, size);
        }

        /// <summary>
        /// Gets count of registered image providers.
        /// </summary>
        public virtual int ImageProviderCount
        {
            get { return ImageProvider.Count; }
        }

        #endregion

        #region IImageProvider Members

        /// <summary>
        /// Queries the image provider for support of a specific size.
        /// </summary>
        /// <param name="size">Indicated image size.</param>
        /// <returns>Returns true when the requested size is supported.</returns>
        public virtual bool IsImageSupported(SEToolStripImageSize size)
        {
            if (DefaultImageProvider == null)
                return false;
            if (DefaultImageProvider == this)
                return true;

            return DefaultImageProvider.IsImageSupported(size);
        }

        /// <summary>
        /// Fetches an image of the requested size.
        /// </summary>
        /// <param name="size">Size of image to obtain.</param>
        /// <returns>If supported, returns requested image. A value of null
        /// indicates that the requested size is not supported.</returns>
        public virtual Image GetImage(SEToolStripImageSize size)
        {
            if (DefaultImageProvider == null)
                throw new NullReferenceException();

            if (DefaultImageProvider == this)
            {
                Size iconSize = SEToolStripIconImageProvider.GetIconSize(size);
                Bitmap icon = new Bitmap(16, 16);
                Icon iconResult = new Icon(DrawingTool.ImageToIcon(icon), iconSize);
                return iconResult.ToBitmap();
            }

            return DefaultImageProvider.GetImage(size);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Call to begin a batch image provider update more efficiently.
        /// Each 'BeginImageProviderUpdate' call <b>MUST</b> be paired with
        /// an 'EndImageProviderUpdate' call.
        /// </summary>
        public virtual void BeginUpdateImages()
        {
            IsUpdatingImages = true;
        }

        /// <summary>
        /// Call to end a batch image provider update. Please note that any
        /// image refreshements only occur when all nested updates are ended.
        /// </summary>
        /// <param name="refresh">Indicates if image sizes are to be refreshed.</param>
        public virtual void EndUpdateImages(bool refresh)
        {
            if (!IsUpdatingImages)
                throw new NotSupportedException();

            IsUpdatingImages = false;

            // Only apply updates when image providers have been changed.
            if (HasImagesChanged)
            {
                HasImagesChanged = false;

                // If no longer updating image providers (i.e. no nested calls), then
                // refresh the image sizes.
                if (!IsUpdatingImages && refresh)
                    RefreshItemImages();
            }
        }

        /// <summary>
        /// Call to end a batch image provider update. Please note that
        /// image refreshements only occur when all nested updates are ended.
        /// </summary>
        public void EndUpdateImages()
        {
            EndUpdateImages(true);
        }

        /// <summary>
        /// Assigns an image provider for the specified item.
        /// </summary>
        /// <param name="item">Associated toolstrip item.</param>
        /// <param name="provider">Image provider.</param>
        /// <returns>Returns true when successful.</returns>
        public bool AssignImage(ToolStripItem item, ISEToolStripImageProvider provider)
        {
            if (item == null || provider == null)
                throw new ArgumentException("One or more arguments were null references.");
            if (ContainsImage(item))
                return false;

            ImageProvider.Add(item, provider);
            HasImagesChanged = true;

            if (!IsUpdatingImages)
                RefreshItemImages();

            return true;
        }

        /// <summary>
        /// Assigns an image provider for the specified item.
        /// </summary>
        /// <param name="item">Associated toolstrip item.</param>
        /// <param name="item">Associated multi-icon.</param>
        /// <returns>Returns true when successful.</returns>
        public bool AssignImage(ToolStripItem item, Icon icon)
        {
            return AssignImage(item, new SEToolStripIconImageProvider(icon));
        }

        /// <summary>
        /// Unregisters an image provider.
        /// </summary>
        /// <param name="item">Associated toolstrip item.</param>
        /// <returns>Returns true when successful.</returns>
        public bool RemoveImage(ToolStripItem item)
        {
            if (ImageProvider.Remove(item))
            {
                HasImagesChanged = true;

                if (!IsUpdatingImages)
                    RefreshItemImages();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove image providers which are not referenced with a <c>ToolStripItem</c>.
        /// </summary>
        /// <returns>Returns count of items removed.</returns>
        public int RemoveUnusedImages()
        {
            List<ToolStripItem> removeList = new List<ToolStripItem>();
            int count = 0;

            // Compile a list of all items which are to be removed.
            foreach (ToolStripItem key in ImageProvider.Keys)
                if (!Items.Contains(key as ToolStripItem))
                    removeList.Add(key);
            count = removeList.Count;

            // Remove each item from provider collection.
            foreach (ToolStripItem item in removeList)
                RemoveImage(item);

            // Make sure that the removal list is disposed of.
            removeList = null;
            return count;
        }

        /// <summary>
        /// Searches for the a provider which is associated with a toolstrip item.
        /// </summary>
        /// <param name="item">Toolstrip item.</param>
        /// <returns>Returns true when an associated provider is found.</returns>
        public bool ContainsImage(ToolStripItem item)
        {
            return ImageProvider.ContainsKey(item);
        }

        /// <summary>
        /// Forces all images sizes to be refreshed from the respective providers.
        /// </summary>
        protected void RefreshItemImages()
        {
            Size imageSize = SEToolStripIconImageProvider.GetIconSize(ImageSize);
            ImageScalingSize = imageSize;

            bool changesMade = false;
            ISEToolStripImageProvider imageProvider = null;

            foreach (ToolStripItem item in Items)
            {
                if (item.Size != imageSize)
                {
                    imageProvider = null;

                    // If an image provider was registered with the toolstrip then...
                    if (ContainsImage(item))
                    {
                        if (IsImageSupported(item, ImageSize))
                            item.Image = GetImage(item, ImageSize);
                        else if (UseUnknownImageSizeIcon && IsImageSupported(ImageSize))
                            item.Image = GetImage(ImageSize);

                        changesMade = true;
                    }
                    else if (item is ISEToolStripImageProvider)
                    {
                        imageProvider = item as ISEToolStripImageProvider;
                    }
                    else if (item.Tag is ISEToolStripImageProvider)
                    {
                        imageProvider = item.Tag as ISEToolStripImageProvider;
                    }

                    // If an alternative image provider was found, attempt to use that.
                    if (!changesMade && imageProvider != null)
                    {
                        if (imageProvider.IsImageSupported(ImageSize))
                        {
                            item.Image = imageProvider.GetImage(ImageSize);
                            changesMade = true;
                        }
                    }

                    // Were changes made?
                    if (changesMade)
                    {
                        // Automatically adjust the image scaling mode.
                        if (item.Image != null && item.Image.Size == imageSize)
                            item.ImageScaling = ToolStripItemImageScaling.None;
                        else
                            item.ImageScaling = ToolStripItemImageScaling.SizeToFit;
                    }
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the default image provider.
        /// </summary>
        public ISEToolStripImageProvider DefaultImageProvider
        {
            get { return this.m_defaultProvider; }
            set { this.m_defaultProvider = value; }
        }

        /// <summary>
        /// Gets the active multiple image provider.
        /// </summary>
        protected SEToolStripImageProviderCollection ImageProvider
        {
            get { return this.m_imageProvider; }
        }

        /// <summary>
        /// Gets or sets the active toolstrip item images sizes.
        /// </summary>
        public SEToolStripImageSize ImageSize
        {
            get { return this.m_imageSize; }
            set
            {
                if (value != this.m_imageSize)
                {
                    SEToolStripImageSizeChangingEventArgs e = new SEToolStripImageSizeChangingEventArgs(this.m_imageSize, value);
                    if (OnImageSizeChanging(e))
                    {
                        // Adjust image scaling mode.
                        ImageScalingSize = SEToolStripIconImageProvider.GetIconSize(value);

                        // Adjust image size as specified.
                        this.m_imageSize = value;
                        RefreshItemImages();
                        OnImageSizeChanged(new SEToolStripOldNewEventArgs<SEToolStripImageSize>(e.CurrentValue, value));
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets whether a default icon is used to represent unsupported
        /// image sizes.
        /// </summary>
        public bool UseUnknownImageSizeIcon
        {
            get { return this.m_useUnknownIcon; }
            set
            {
                if (value != this.m_useUnknownIcon)
                {
                    this.m_useUnknownIcon = value;
                    OnUseUnknownImageSizeIconChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating if image providers are being updated.
        /// </summary>
        public bool IsUpdatingImages
        {
            get { return this.m_updatingProviders > 0; }
            private set { this.m_updatingProviders += value ? +1 : -1; }
        }

        /// <summary>
        /// Gets or sets a value indicating if one or more image providers have been changed.
        /// </summary>
        protected bool HasImagesChanged
        {
            get { return this.m_changesMade; }
            set { this.m_changesMade = value; }
        }

        #endregion

        #region Attributes

        private SEToolStripImageProviderCollection m_imageProvider;
        private ISEToolStripImageProvider m_defaultProvider;
        private SEToolStripImageSize m_imageSize = SEToolStripImageSize.Small;
        private bool m_useUnknownIcon = false;
        private int m_updatingProviders = 0;
        private bool m_changesMade = false;

        #endregion
    }


    public class SEToolStripOldNewEventArgs<T> : EventArgs
    {
        public SEToolStripOldNewEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public T OldValue
        {
            get { return this.m_oldValue; }
            protected set { this.m_oldValue = value; }
        }
        public T NewValue
        {
            get { return this.m_newValue; }
            protected set { this.m_newValue = value; }
        }

        T m_oldValue = default(T);
        T m_newValue = default(T);
    }

    public delegate void SEToolStripOldNewEventHandler<T>(object sender, SEToolStripOldNewEventArgs<T> e);

}
