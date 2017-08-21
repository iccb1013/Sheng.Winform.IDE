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
namespace Sheng.SailingEase.Controls
{
    public class SEComboSelectorItemCollection : CollectionBase
    {
        public SEComboSelectorItemCollection()
        {
        }
        public SEComboSelectorItemCollection(SEComboSelectorItemCollection value)
        {
            this.AddRange(value);
        }
        public SEComboSelectorItemCollection(SEComboSelectorItem[] value)
        {
            this.AddRange(value);
        }
        public SEComboSelectorItem this[int index]
        {
            get
            {
                return ((SEComboSelectorItem)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }
        public virtual int Add(SEComboSelectorItem value)
        {
            return List.Add(value);
        }
        public void AddRange(SEComboSelectorItem[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public void AddRange(SEComboSelectorItemCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public bool Contains(SEComboSelectorItem value)
        {
            return List.Contains(value);
        }
        public void CopyTo(SEComboSelectorItem[] array, int index)
        {
            List.CopyTo(array, index);
        }
        public int IndexOf(SEComboSelectorItem value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, SEComboSelectorItem value)
        {
            List.Insert(index, value);
        }
        public new SERichComboBoxDropItemEnumerator GetEnumerator()
        {
            return new SERichComboBoxDropItemEnumerator(this);
        }
        public virtual void Remove(SEComboSelectorItem value)
        {
            List.Remove(value);
        }
        public class SERichComboBoxDropItemEnumerator : object, IEnumerator
        {
            private IEnumerator baseEnumerator;
            private IEnumerable temp;
            public SERichComboBoxDropItemEnumerator(SEComboSelectorItemCollection mappings)
            {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }
            public SEComboSelectorItem Current
            {
                get
                {
                    return ((SEComboSelectorItem)(baseEnumerator.Current));
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
