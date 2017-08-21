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

namespace Manina.Windows.Forms
{
    /// <summary>
    /// Provides a type converter for the column header collection.
    /// This is only used to replace the generic collection text in the
    /// property browser with "(Collection)".
    /// </summary>
    internal class ColumnHeaderCollectionTypeConverter : TypeConverter
    {
        #region TypeConverter Overrides
        /// <summary>
        /// Returns whether this converter can convert the object to the specified type, using the specified context.
        /// </summary>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;

            return base.CanConvertTo(context, destinationType);
        }
        /// <summary>
        /// Converts the given value object to the specified type, using the specified context and culture information.
        /// </summary>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (value is ImageListView.ImageListViewColumnHeaderCollection && 
                (destinationType == typeof(string)))
                return "(Collection)";

            return base.ConvertTo(context, culture, value, destinationType);
        }
        #endregion
    }
}
