/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
/*
 * Comparers - Various Comparer classes used within ObjectListView
 * Author: Phillip Piper
 * Date: 25/11/2008 17:15 
 * Change log:
 * 2009-08-24  JPP  - Added OLVGroupComparer
 * 2009-06-01  JPP  - ModelObjectComparer would crash if secondary sort column was null.
 * 2008-12-20  JPP  - Fixed bug with group comparisons when a group key was null (SF#2445761)
 * 2008-11-25  JPP  Initial version
 * TO DO:
 * Copyright (C) 2006-2009 Phillip Piper
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http:
 * If you wish to use this code in a closed source application, please contact phillip_piper@bigfoot.com.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
namespace Sheng.SailingEase.Controls.ObjectListView
{
    public class ColumnComparer : IComparer, IComparer<OLVListItem>
    {
        public ColumnComparer(OLVColumn col, SortOrder order)
        {
            this.column = col;
            this.sortOrder = order;
        }
        public ColumnComparer(OLVColumn col, SortOrder order, OLVColumn col2, SortOrder order2)
            : this(col, order)
        {
            if (col != col2)
                this.secondComparer = new ColumnComparer(col2, order2);
        }
        public int Compare(object x, object y)
        {
            return this.Compare((OLVListItem)x, (OLVListItem)y);
        }
        public int Compare(OLVListItem x, OLVListItem y)
        {
            int result = 0;
            object x1 = this.column.GetValue(x.RowObject);
            object y1 = this.column.GetValue(y.RowObject);
            if (this.sortOrder == SortOrder.None)
                return 0;
            bool xIsNull = (x1 == null || x1 == System.DBNull.Value);
            bool yIsNull = (y1 == null || y1 == System.DBNull.Value);
            if (xIsNull || yIsNull) {
                if (xIsNull && yIsNull)
                    result = 0;
                else
                    result = (xIsNull ? -1 : 1);
            } else {
                result = this.CompareValues(x1, y1);
            }
            if (this.sortOrder == SortOrder.Descending)
                result = 0 - result;
            if (result == 0 && this.secondComparer != null)
                result = this.secondComparer.Compare(x, y);
            return result;
        }
        public int CompareValues(object x, object y)
        {
            String xAsString = x as String;
            if (xAsString != null)
                return String.Compare(xAsString, (String)y, StringComparison.CurrentCultureIgnoreCase);
            else {
                IComparable comparable = x as IComparable;
                if (comparable != null)
                    return comparable.CompareTo(y);
                else
                    return 0;
            }
        }
        private OLVColumn column;
        private SortOrder sortOrder;
        private ColumnComparer secondComparer;
    }
    public class ListViewGroupComparer : IComparer<ListViewGroup>
    {
        public ListViewGroupComparer(SortOrder order) {
            this.sortOrder = order;
        }
        public int Compare(ListViewGroup x, ListViewGroup y) {
            int result;
            IComparable comparable = x.Tag as IComparable;
            if (comparable != null && y.Tag != null && y.Tag != System.DBNull.Value)
                result = comparable.CompareTo(y.Tag);
            else
                result = String.Compare(x.Header, y.Header, StringComparison.CurrentCultureIgnoreCase);
            if (this.sortOrder == SortOrder.Descending)
                result = 0 - result;
            return result;
        }
        private SortOrder sortOrder;
    }
    public class OLVGroupComparer : IComparer<OLVGroup>
    {
        public OLVGroupComparer(SortOrder order) {
            this.sortOrder = order;
        }
        public int Compare(OLVGroup x, OLVGroup y) {
            int result;
            if (x.SortValue != null && y.SortValue != null)
                result = x.SortValue.CompareTo(y.SortValue);
            else
                result = String.Compare(x.Header, y.Header, StringComparison.CurrentCultureIgnoreCase);
            if (this.sortOrder == SortOrder.Descending)
                result = 0 - result;
            return result;
        }
        private SortOrder sortOrder;
    }
    public class ModelObjectComparer : IComparer, IComparer<object>
    {
        public ModelObjectComparer(OLVColumn col, SortOrder order)
        {
            this.column = col;
            this.sortOrder = order;
        }
        public ModelObjectComparer(OLVColumn col, SortOrder order, OLVColumn col2, SortOrder order2)
            : this(col, order)
        {
            if (col != col2 && col2 != null && order2 != SortOrder.None)
                this.secondComparer = new ModelObjectComparer(col2, order2);
        }
        public int Compare(object x, object y)
        {
            int result = 0;
            object x1 = this.column.GetValue(x);
            object y1 = this.column.GetValue(y);
            if (this.sortOrder == SortOrder.None)
                return 0;
            bool xIsNull = (x1 == null || x1 == System.DBNull.Value);
            bool yIsNull = (y1 == null || y1 == System.DBNull.Value);
            if (xIsNull || yIsNull) {
                if (xIsNull && yIsNull)
                    result = 0;
                else
                    result = (xIsNull ? -1 : 1);
            } else {
                result = this.CompareValues(x1, y1);
            }
            if (this.sortOrder == SortOrder.Descending)
                result = 0 - result;
            if (result == 0 && this.secondComparer != null)
                result = this.secondComparer.Compare(x, y);
            return result;
        }
        public int CompareValues(object x, object y)
        {
            String xStr = x as String;
            if (xStr != null)
                return String.Compare(xStr, (String)y, StringComparison.CurrentCultureIgnoreCase);
            else {
                IComparable comparable = x as IComparable;
                if (comparable != null)
                    return comparable.CompareTo(y);
                else
                    return 0;
            }
        }
        private OLVColumn column;
        private SortOrder sortOrder;
        private ModelObjectComparer secondComparer;
    }
}
