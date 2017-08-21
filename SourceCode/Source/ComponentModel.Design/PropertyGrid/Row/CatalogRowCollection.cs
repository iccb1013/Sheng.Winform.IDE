/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
namespace Sheng.SailingEase.ComponentModel.Design
{
    class CatalogRowCollection : CollectionBase
    {
        public CatalogRowCollection()
        {
        }
        public CatalogRowCollection(CatalogRowCollection value)
        {
            this.AddRange(value);
        }
        public CatalogRowCollection(CatalogRow[] value)
        {
            this.AddRange(value);
        }
        public CatalogRow this[int index]
        {
            get
            {
                return ((CatalogRow)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }
        public CatalogRow this[string catalogName]
        {
            get
            {
                foreach (CatalogRow row in List)
                {
                    if (row.CatalogName == catalogName)
                        return row;
                }
                return null;
            }
        }
        public int Add(CatalogRow value)
        {
            return List.Add(value);
        }
        public int Add(string catalogName)
        {
            if (!Contains(catalogName))
            {
                CatalogRow collapseDataGridViewRow = new CatalogRow(catalogName);
                return List.Add(collapseDataGridViewRow);
            }
            return List.Count;
        }
        public void AddPropertyGirdRow(PropertyGridRow propertyGirdRow, string catalogName)
        {
            if (!Contains(catalogName))
            {
                Add(catalogName);
            }
            this[catalogName].SubRows.Add(propertyGirdRow);
        }
        public void AddRange(CatalogRow[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public void AddRange(CatalogRowCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public bool Contains(CatalogRow value)
        {
            return List.Contains(value);
        }
        public bool Contains(string catalogName)
        {
            foreach (CatalogRow row in List)
            {
                if (row.CatalogName == catalogName)
                    return true;
            }
            return false;
        }
        public void CopyTo(CatalogRow[] array, int index)
        {
            List.CopyTo(array, index);
        }
        public int IndexOf(CatalogRow value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, CatalogRow value)
        {
            List.Insert(index, value);
        }
        public new CollapseDataGridViewRowEnumerator GetEnumerator()
        {
            return new CollapseDataGridViewRowEnumerator(this);
        }
        public void Remove(CatalogRow value)
        {
            List.Remove(value);
        }
        public class CollapseDataGridViewRowEnumerator : object, IEnumerator
        {
            private IEnumerator baseEnumerator;
            private IEnumerable temp;
            public CollapseDataGridViewRowEnumerator(CatalogRowCollection mappings)
            {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }
            public CatalogRow Current
            {
                get
                {
                    return ((CatalogRow)(baseEnumerator.Current));
                }
            }
            object IEnumerator.Current
            {
                get
                {
                    return baseEnumerator.Current;
                }
            }
            public bool MoveNext()
            {
                return baseEnumerator.MoveNext();
            }
            bool IEnumerator.MoveNext()
            {
                return baseEnumerator.MoveNext();
            }
            public void Reset()
            {
                baseEnumerator.Reset();
            }
            void IEnumerator.Reset()
            {
                baseEnumerator.Reset();
            }
        }
    }
}
