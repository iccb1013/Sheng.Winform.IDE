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
namespace Sheng.SailingEase.ComponentModel.Design
{
    class PropertyGridRowCollection : CollectionBase
    {
        public PropertyGridRowCollection()
        {
        }
        public PropertyGridRowCollection(PropertyGridRowCollection value)
        {
            this.AddRange(value);
        }
        public PropertyGridRowCollection(PropertyGridRow[] value)
        {
            this.AddRange(value);
        }
        public PropertyGridRow this[int index]
        {
            get
            {
                return ((PropertyGridRow)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }
        public int Add(PropertyGridRow value)
        {
            if (this.Contains(value.PropertyName) == false)
            {
                return List.Add(value);
            }
            else
            {
                return this.Count;
            }
        }
        public void AddRange(PropertyGridRow[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public void AddRange(PropertyGridRowCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public bool Contains(PropertyGridRow value)
        {
            return List.Contains(value);
        }
        public void CopyTo(PropertyGridRow[] array, int index)
        {
            List.CopyTo(array, index);
        }
        public int IndexOf(PropertyGridRow value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, PropertyGridRow value)
        {
            if (this.Contains(value.PropertyName) == false)
            {
                List.Insert(index, value);
            }
        }
        public new PropertyGirdRowEnumerator GetEnumerator()
        {
            return new PropertyGirdRowEnumerator(this);
        }
        public void Remove(PropertyGridRow value)
        {
            List.Remove(value);
        }
        public bool Contains(string name)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].PropertyName == name)
                {
                    return true;
                }
            }
            return false;
        }
        public class PropertyGirdRowEnumerator : object, IEnumerator
        {
            private IEnumerator baseEnumerator;
            private IEnumerable temp;
            public PropertyGirdRowEnumerator(PropertyGridRowCollection mappings)
            {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }
            public PropertyGridRow Current
            {
                get
                {
                    return ((PropertyGridRow)(baseEnumerator.Current));
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
