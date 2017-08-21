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
using System.ComponentModel.Design;

namespace Manina.Windows.Forms
{
    /// <summary>
    /// Provides an editor for the column header collection.
    /// </summary>
    internal class ColumnHeaderCollectionEditor : CollectionEditor
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the ColumnHeaderCollectionEditor class.
        /// </summary>
        public ColumnHeaderCollectionEditor()
            : base(typeof(ImageListView.ImageListViewColumnHeaderCollection))
        {
        }
        #endregion

        #region CollectionEditor Overrides
        /// <summary>
        /// Indicates whether original members of the collection can be removed.
        /// </summary>
        protected override bool CanRemoveInstance(object value)
        {
            // Disable the Remove button
            return false;
        }
        /// <summary>
        /// Gets the data types that this collection editor can contain.
        /// </summary>
        protected override Type[] CreateNewItemTypes()
        {
            // Disable the Add button
            return new Type[0];
        }
        /// <summary>
        /// Retrieves the display text for the given list item.
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            return ((ImageListView.ImageListViewColumnHeader)value).Type.ToString();
        }
        /// <summary>
        /// Indicates whether multiple collection items can be selected at once.
        /// </summary>
        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }
        /// <summary>
        /// Gets an array of objects containing the specified collection.
        /// </summary>
        protected override object[] GetItems(object editValue)
        {
            ImageListView.ImageListViewColumnHeaderCollection columns = 
                (ImageListView.ImageListViewColumnHeaderCollection)editValue;
            object[] list = new object[columns.Count];
            for (int i = 0; i < columns.Count; i++)
                list[i] = columns[i];
            return list;
        }
        /// <summary>
        /// Creates a new form to display and edit the current collection.
        /// </summary>
        protected override CollectionEditor.CollectionForm CreateCollectionForm()
        {
            return base.CreateCollectionForm();
        }
        #endregion
    }
}
