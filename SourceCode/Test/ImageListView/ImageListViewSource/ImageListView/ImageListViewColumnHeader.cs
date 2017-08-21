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
using System.Windows.Forms;
using System.ComponentModel;

namespace Manina.Windows.Forms
{
    public partial class ImageListView
    {
        /// <summary>
        /// Represents a column header displayed in details view mode.
        /// </summary>
        public class ImageListViewColumnHeader
        {
            #region Member Variables
            private int mDisplayIndex;
            internal ImageListView mImageListView;
            private string mText;
            private ColumnType mType;
            private bool mVisible;
            private int mWidth;

            internal ImageListViewColumnHeaderCollection owner;
            #endregion

            #region Properties
            /// <summary>
            /// Gets the default header text for this column type.
            /// </summary>
            [Category("Appearance"), Browsable(false), Description("Gets the default header text for this column type."), Localizable(true)]
            public virtual string DefaultText
            {
                get
                {
                    return owner.GetDefaultText(mType);
                }
            }
            /// <summary>
            /// Gets or sets the display order of the column.
            /// </summary>
            [Category("Appearance"), Browsable(true), Description("Gets the bounds of the item in client coordinates.")]
            public int DisplayIndex
            {
                get
                {
                    return mDisplayIndex;
                }
                set
                {
                    int oldIndex = mDisplayIndex;
                    int newIndex = value;
                    if (newIndex < 0 || newIndex > owner.Count - 1)
                        throw new IndexOutOfRangeException();

                    if (oldIndex == -1)
                        mDisplayIndex = newIndex;
                    else
                    {
                        ImageListViewColumnHeader targetColumn = null;
                        foreach (ImageListViewColumnHeader column in owner)
                        {
                            if (column.DisplayIndex == newIndex)
                            {
                                targetColumn = column;
                                break;
                            }
                        }
                        if (targetColumn != null)
                        {
                            this.mDisplayIndex = newIndex;
                            targetColumn.mDisplayIndex = oldIndex;
                            if (mImageListView != null)
                                mImageListView.Refresh();
                        }
                    }
                }
            }
            /// <summary>
            /// Gets the ImageListView owning this item.
            /// </summary>
            [Category("Behavior"), Browsable(false), Description("Gets the ImageListView owning this item.")]
            public ImageListView ImageListView { get { return mImageListView; } }
            /// <summary>
            /// Gets or sets the column header text.
            /// </summary>
            [Category("Appearance"), Browsable(true), Description("Gets or sets the column header text.")]
            public string Text
            {
                get
                {
                    if (!string.IsNullOrEmpty(mText))
                        return mText;
                    else
                        return DefaultText;
                }
                set
                {
                    mText = value;
                    if (mImageListView != null)
                        mImageListView.Refresh();
                }
            }
            /// <summary>
            /// Gets the type of information displayed by the column.
            /// </summary>
            [Category("Appearance"), Browsable(false), Description("Gets or sets the type of information displayed by the column.")]
            public ColumnType Type { get { return mType; } }
            /// <summary>
            /// Gets or sets a value indicating whether the control is displayed.
            /// </summary>
            [Category("Appearance"), Browsable(true), Description("Gets or sets a value indicating whether the control is displayed."), DefaultValue(true)]
            public bool Visible
            {
                get
                {
                    return mVisible;
                }
                set
                {
                    mVisible = value;
                    if (mImageListView != null)
                        mImageListView.Refresh();
                }
            }
            /// <summary>
            /// Gets or sets the column width.
            /// </summary>
            [Category("Appearance"), Browsable(true), Description("Gets or sets the column width."), DefaultValue(ImageListView.DefaultColumnWidth)]
            public int Width
            {
                get
                {
                    return mWidth;
                }
                set
                {
                    mWidth = System.Math.Max(12, value);
                    if (mImageListView != null)
                        mImageListView.Refresh();
                }
            }
            #endregion

            #region Constructors
            /// <summary>
            /// Initializes a new instance of the ImageListViewColumnHeader class.
            /// </summary>
            /// <param name="type">The type of data to display in this column.</param>
            /// <param name="text">Text of the column header.</param>
            /// <param name="width">Width in pixels of the column header.</param>
            public ImageListViewColumnHeader(ColumnType type, string text, int width)
            {
                mImageListView = null;
                owner = null;
                mText = text;
                mType = type;
                mWidth = width;
                mVisible = true;
                mDisplayIndex = -1;
            }
            /// <summary>
            /// Initializes a new instance of the ImageListViewColumnHeader class.
            /// </summary>
            /// <param name="type">The type of data to display in this column.</param>
            /// <param name="text">Text of the column header.</param>
            public ImageListViewColumnHeader(ColumnType type, string text)
                : this(type, text, ImageListView.DefaultColumnWidth)
            {
                ;
            }
            /// <summary>
            /// Initializes a new instance of the ImageListViewColumnHeader class.
            /// </summary>
            /// <param name="type">The type of data to display in this column.</param>
            /// <param name="width">Width in pixels of the column header.</param>
            public ImageListViewColumnHeader(ColumnType type, int width)
                : this(type, "", width)
            {
                ;
            }
            /// <summary>
            /// Initializes a new instance of the ImageListViewColumnHeader class.
            /// </summary>
            /// <param name="type">The type of data to display in this column.</param>
            public ImageListViewColumnHeader(ColumnType type)
                : this(type, "", ImageListView.DefaultColumnWidth)
            {
                ;
            }
            /// <summary>
            /// Initializes a new instance of the ImageListViewColumnHeader class.
            /// </summary>
            public ImageListViewColumnHeader()
                : this(ColumnType.Name)
            {
                ;
            }
            #endregion

            #region Instance Methods
            /// <summary>
            /// Resizes the width of the column based on the length of the column content.
            /// </summary>
            public void AutoFit()
            {
                if (mImageListView == null)
                    throw new InvalidOperationException("Cannot calculate column width. Owner image list view is null.");

                int width = 0;
                foreach (ImageListViewItem item in mImageListView.Items)
                {
                    int itemwidth = TextRenderer.MeasureText(item.GetSubItemText(Type), mImageListView.Font).Width;
                    width = System.Math.Max(width, itemwidth);
                }
                this.Width = width + 8;
                mImageListView.Refresh();
            }
            #endregion
        }
    }
}
