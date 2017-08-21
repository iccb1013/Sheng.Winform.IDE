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

using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System;

namespace Manina.Windows.Forms
{
    /// <summary>
    /// Represents the cache manager responsible for asynchronously loading
    /// item details.
    /// </summary>
    internal class ImageListViewItemCacheManager : IDisposable
    {
        #region Member Variables
        private readonly object lockObject;

        private ImageListView mImageListView;
        private Thread mThread;

        private Queue<CacheItem> toCache;
        private Dictionary<Guid, bool> editCache;

        private volatile bool stopping;
        private bool stopped;
        private bool disposed;
        #endregion

        #region Private Classes
        /// <summary>
        /// Represents an item in the item cache.
        /// </summary>
        private class CacheItem
        {
            private ImageListViewItem mItem;
            private string mFileName;
            private bool mIsVirtualItem;
            private object mVirtualItemKey;

            /// <summary>
            /// Gets the ImageListViewItem associated with this request.
            /// </summary>
            public ImageListViewItem Item { get { return mItem; } }
            /// <summary>
            /// Gets the name of the image file.
            /// </summary>
            public string FileName { get { return mFileName; } }
            /// <summary>
            /// Gets whether Item is a virtual item.
            /// </summary>
            public bool IsVirtualItem { get { return mIsVirtualItem; } }
            /// <summary>
            /// Gets the public key for the virtual item.
            /// </summary>
            public object VirtualItemKey { get { return mVirtualItemKey; } }

            /// <summary>
            /// Initializes a new instance of the CacheItem class.
            /// </summary>
            /// <param name="item">The ImageListViewItem associated 
            /// with this request.</param>
            public CacheItem(ImageListViewItem item)
            {
                mItem = item;
                mIsVirtualItem = item.isVirtualItem;
                mVirtualItemKey = item.mVirtualItemKey;
                mFileName = item.FileName;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Determines whether the cache thread is being stopped.
        /// </summary>
        private bool Stopping { get { lock (lockObject) { return stopping; } } }
        /// <summary>
        /// Determines whether the cache thread is stopped.
        /// </summary>
        public bool Stopped { get { lock (lockObject) { return stopped; } } }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the ImageListViewItemCacheManager class.
        /// </summary>
        /// <param name="owner">The owner control.</param>
        public ImageListViewItemCacheManager(ImageListView owner)
        {
            lockObject = new object();

            mImageListView = owner;

            toCache = new Queue<CacheItem>();
            editCache = new Dictionary<Guid, bool>();

            mThread = new Thread(new ThreadStart(DoWork));
            mThread.IsBackground = true;

            stopping = false;
            stopped = false;
            disposed = false;

            mThread.Start();
            while (!mThread.IsAlive) ;
        }
        #endregion

        #region Instance Methods
        /// <summary>
        /// Starts editing an item. While items are edited,
        /// their original images will be seperately cached
        /// instead of fetching them from the file.
        /// </summary>
        /// <param name="guid">The GUID of the item</param>
        public void BeginItemEdit(Guid guid)
        {
            lock (lockObject)
            {
                if (!editCache.ContainsKey(guid))
                    editCache.Add(guid, false);
            }
        }
        /// <summary>
        /// Ends editing an item. After this call, item
        /// image will be continued to be fetched from the
        /// file.
        /// </summary>
        /// <param name="guid"></param>
        public void EndItemEdit(Guid guid)
        {
            lock (lockObject)
            {
                if (editCache.ContainsKey(guid))
                {
                    editCache.Remove(guid);
                }
            }
        }
        /// <summary>
        /// Adds the item to the cache queue.
        /// </summary>
        public void Add(ImageListViewItem item)
        {
            lock (lockObject)
            {
                toCache.Enqueue(new CacheItem(item));
                Monitor.Pulse(lockObject);
            }
        }
        /// <summary>
        /// Stops the cache manager.
        /// </summary>
        public void Stop()
        {
            lock (lockObject)
            {
                if (!stopping)
                {
                    stopping = true;
                    Monitor.Pulse(lockObject);
                }
            }
        }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                // Nothing to dispose
                disposed = true;
            }
        }
        #endregion

        #region Worker Method
        /// <summary>
        /// Used by the worker thread to read item data.
        /// </summary>
        private void DoWork()
        {
            while (!Stopping)
            {

                CacheItem item = null;
                lock (lockObject)
                {
                    // Wait until we have items waiting to be cached
                    if (toCache.Count == 0)
                        Monitor.Wait(lockObject);

                    // Get an item from the queue
                    if (toCache.Count != 0)
                    {
                        item = toCache.Dequeue();

                        // Is it being edited?
                        if (editCache.ContainsKey(item.Item.Guid))
                            item = null;
                    }
                }

                // Read file info
                if (item != null)
                {
                    if (item.IsVirtualItem)
                    {
                        if (mImageListView != null && mImageListView.IsHandleCreated && !mImageListView.IsDisposed)
                        {
                            VirtualItemDetailsEventArgs e = new VirtualItemDetailsEventArgs(item.VirtualItemKey);
                            mImageListView.RetrieveVirtualItemDetailsInternal(e);
                            mImageListView.Invoke(new UpdateVirtualItemDetailsDelegateInternal(
                                mImageListView.UpdateItemDetailsInternal), item.Item, e);
                        }
                    }
                    else
                    {
                        Utility.ShellImageFileInfo info = new Utility.ShellImageFileInfo(item.FileName);
                        // Update file info
                        if (!Stopping)
                        {
                            try
                            {
                                if (mImageListView != null && mImageListView.IsHandleCreated && !mImageListView.IsDisposed)
                                {
                                    mImageListView.Invoke(new UpdateItemDetailsDelegateInternal(
                                        mImageListView.UpdateItemDetailsInternal), item.Item, info);
                                }
                            }
                            catch (ObjectDisposedException)
                            {
                                if (!Stopping) throw;
                            }
                            catch (InvalidOperationException)
                            {
                                if (!Stopping) throw;
                            }
                        }
                    }
                }
            }

            lock (lockObject)
            {
                stopped = true;
            }
        }
        #endregion
    }
}
