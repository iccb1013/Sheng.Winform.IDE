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
using System.Collections;
namespace Sheng.SailingEase.Core
{
    [Serializable]
    public class UIElementDataListRowCellEntityCollection : CollectionBase
    {
        public UIElementDataListRowCellEntityCollection()
        {
        }
        public UIElementDataListRowCellEntityCollection(UIElementDataListRowCellEntityCollection value)
        {
            this.AddRange(value);
        }
        public UIElementDataListRowCellEntityCollection(UIElementDataListRowCellEntity[] value)
        {
            this.AddRange(value);
        }
        public UIElementDataListRowCellEntity this[int index]
        {
            get
            {
                return ((UIElementDataListRowCellEntity)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }
        public virtual int Add(UIElementDataListRowCellEntity value)
        {
            return List.Add(value);
        }
        public void AddRange(UIElementDataListRowCellEntity[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public void AddRange(UIElementDataListRowCellEntityCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public bool Contains(UIElementDataListRowCellEntity value)
        {
            return List.Contains(value);
        }
        public void CopyTo(UIElementDataListRowCellEntity[] array, int index)
        {
            List.CopyTo(array, index);
        }
        public int IndexOf(UIElementDataListRowCellEntity value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, UIElementDataListRowCellEntity value)
        {
            List.Insert(index, value);
        }
        public new FormElementDataListRowCellEntityEnumerator GetEnumerator()
        {
            return new FormElementDataListRowCellEntityEnumerator(this);
        }
        public virtual void Remove(UIElementDataListRowCellEntity value)
        {
            List.Remove(value);
        }
        public UIElementDataListRowCellEntity this[string code]
        {
            get
            {
                foreach (UIElementDataListRowCellEntity cell in this)
                {
                    if (cell.OwningColumn.Code == code)
                    {
                        return cell;
                    }
                }
                return null;
            }
        }
        public class FormElementDataListRowCellEntityEnumerator : object, IEnumerator
        {
            private IEnumerator baseEnumerator;
            private IEnumerable temp;
            public FormElementDataListRowCellEntityEnumerator(UIElementDataListRowCellEntityCollection mappings)
            {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }
            public UIElementDataListRowCellEntity Current
            {
                get
                {
                    return ((UIElementDataListRowCellEntity)(baseEnumerator.Current));
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
