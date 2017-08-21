/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
/*
 * FastObjectListView - A listview that behaves like an ObjectListView but has the speed of a virtual list
 * Author: Phillip Piper
 * Date: 27/09/2008 9:15 AM
 * Change log:
 * 2009-08-27   JPP  - Added GroupingStrategy
 *                   - Added optimized Objects property
 * v2.2.1
 * 2009-01-07   JPP  - Made all public and protected methods virtual
 * 2008-09-27   JPP  - Separated from ObjectListView.cs
 * Copyright (C) 2006-2008 Phillip Piper
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
using System.ComponentModel;
using System.Windows.Forms;
namespace Sheng.SailingEase.Controls.ObjectListView
{
    public class FastObjectListView : VirtualObjectListView
    {
        public FastObjectListView()
        {
            this.DataSource = new FastObjectListDataSource(this);
            this.GroupingStrategy = new FastListGroupingStrategy();
        }
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override IEnumerable Objects {
            get {
                return ((FastObjectListDataSource)this.DataSource).ObjectList; 
            }
            set { base.Objects = value; }
        }
    }
    public class FastObjectListDataSource : AbstractVirtualListDataSource
    {
        public FastObjectListDataSource(FastObjectListView listView)
            : base(listView)
        {
        }
        internal ArrayList ObjectList {
            get { return objectList; }
        }
        public override object GetNthObject(int n)
        {
            return this.objectList[n];
        }
        public override int GetObjectCount()
        {
            return this.objectList.Count;
        }
        public override int GetObjectIndex(object model)
        {
            int index;
            if (model != null && this.objectsToIndexMap.TryGetValue(model, out index))
                return index;
            else
                return -1;
        }
        public override int SearchText(string value, int first, int last, OLVColumn column)
        {
            return DefaultSearchText(value, first, last, column, this);
        }
        public override void Sort(OLVColumn column, SortOrder sortOrder)
        {
            if (sortOrder != SortOrder.None)
                this.objectList.Sort(new ModelObjectComparer(column, sortOrder, this.listView.SecondarySortColumn, this.listView.SecondarySortOrder));
            this.RebuildIndexMap();
        }
        public override void AddObjects(ICollection modelObjects)
        {
            foreach (object modelObject in modelObjects) {
                if (modelObject != null)
                    this.objectList.Add(modelObject);
            }
            this.RebuildIndexMap();
        }
        public override void RemoveObjects(ICollection modelObjects)
        {
            List<int> indicesToRemove = new List<int>();
            foreach (object modelObject in modelObjects) {
                int i = this.GetObjectIndex(modelObject);
                if (i >= 0)
                    indicesToRemove.Add(i);
            }
            indicesToRemove.Sort();
            indicesToRemove.Reverse();
            foreach (int i in indicesToRemove)
                this.listView.SelectedIndices.Remove(i);
            foreach (int i in indicesToRemove)
                this.objectList.RemoveAt(i);
            this.RebuildIndexMap();
        }
        public override void SetObjects(IEnumerable collection)
        {
            ArrayList newObjects = new ArrayList();
            if (collection != null) {
                if (collection is ICollection)
                    newObjects = new ArrayList((ICollection)collection);
                else {
                    foreach (object x in collection)
                        newObjects.Add(x);
                }
            }
            this.objectList = newObjects;
            this.RebuildIndexMap();
        }
        private ArrayList objectList = new ArrayList();
        protected void RebuildIndexMap()
        {
            this.objectsToIndexMap.Clear();
            for (int i = 0; i < this.objectList.Count; i++)
                this.objectsToIndexMap[this.objectList[i]] = i;
        }
        Dictionary<Object, int> objectsToIndexMap = new Dictionary<Object, int>();
    }
}
