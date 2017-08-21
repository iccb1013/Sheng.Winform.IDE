/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
/*
 * VirtualListDataSource - Encapsulate how data is provided to a virtual list
 * Author: Phillip Piper
 * Date: 28/08/2009 11:10am
 * Change log:
 * 2009-08-28   JPP  - Initial version (Separated from VirtualObjectListView.cs)
 * To do:
 * Copyright (C) 2009 Phillip Piper
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
using System.Windows.Forms;
namespace Sheng.SailingEase.Controls.ObjectListView
{
    public interface IVirtualListDataSource
    {
        Object GetNthObject(int n);
        int GetObjectCount();
        int GetObjectIndex(Object model);
        void PrepareCache(int first, int last);
        int SearchText(string value, int first, int last, OLVColumn column);
        void Sort(OLVColumn column, SortOrder order);
        void AddObjects(ICollection modelObjects);
        void RemoveObjects(ICollection modelObjects);
        void SetObjects(IEnumerable collection);
    }
    public class AbstractVirtualListDataSource : IVirtualListDataSource
    {
        public AbstractVirtualListDataSource(VirtualObjectListView listView) {
            this.listView = listView;
        }
        protected VirtualObjectListView listView;
        public virtual object GetNthObject(int n) {
            return null;
        }
        public virtual int GetObjectCount() {
            return -1;
        }
        public virtual int GetObjectIndex(object model) {
            return -1;
        }
        public virtual void PrepareCache(int from, int to) {
        }
        public virtual int SearchText(string value, int first, int last, OLVColumn column) {
            return -1;
        }
        public virtual void Sort(OLVColumn column, SortOrder order) {
        }
        public virtual void AddObjects(ICollection modelObjects) {
        }
        public virtual void RemoveObjects(ICollection modelObjects) {
        }
        public virtual void SetObjects(IEnumerable collection) {
        }
        static public int DefaultSearchText(string value, int first, int last, OLVColumn column, IVirtualListDataSource source) {
            if (first <= last) {
                for (int i = first; i <= last; i++) {
                    string data = column.GetStringValue(source.GetNthObject(i));
                    if (data.StartsWith(value, StringComparison.CurrentCultureIgnoreCase))
                        return i;
                }
            } else {
                for (int i = first; i >= last; i--) {
                    string data = column.GetStringValue(source.GetNthObject(i));
                    if (data.StartsWith(value, StringComparison.CurrentCultureIgnoreCase))
                        return i;
                }
            }
            return -1;
        }
    }
    public class VirtualListVersion1DataSource : AbstractVirtualListDataSource
    {
        public VirtualListVersion1DataSource(VirtualObjectListView listView)
            : base(listView) {
        }
        public RowGetterDelegate RowGetter {
            get { return rowGetter; }
            set { rowGetter = value; }
        }
        private RowGetterDelegate rowGetter;
        public override object GetNthObject(int n) {
            if (this.RowGetter == null)
                return null;
            else
                return this.RowGetter(n);
        }
        public override int SearchText(string value, int first, int last, OLVColumn column) {
            return DefaultSearchText(value, first, last, column, this);
        }
    }
}
