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
    public class UIElementDataListRowEntityCollection:CollectionBase
    {
        public UIElementDataListRowEntityCollection()
        {
        }
        public UIElementDataListRowEntityCollection(UIElementDataListRowEntityCollection value)
        {
            this.AddRange(value);
        }
        public UIElementDataListRowEntityCollection(UIElementDataListRowEntity[] value)
        {
            this.AddRange(value);
        }
        public UIElementDataListRowEntity this[int index]
        {
            get
            {
                return ((UIElementDataListRowEntity)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }
        public virtual int Add(UIElementDataListRowEntity value)
        {
            return List.Add(value);
        }
        public void AddRange(UIElementDataListRowEntity[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public void AddRange(UIElementDataListRowEntityCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public bool Contains(UIElementDataListRowEntity value)
        {
            return List.Contains(value);
        }
        public void CopyTo(UIElementDataListRowEntity[] array, int index)
        {
            List.CopyTo(array, index);
        }
        public int IndexOf(UIElementDataListRowEntity value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, UIElementDataListRowEntity value)
        {
            List.Insert(index, value);
        }
        public new FormElementDataListRowEntityEnumerator GetEnumerator()
        {
            return new FormElementDataListRowEntityEnumerator(this);
        }
        public virtual void Remove(UIElementDataListRowEntity value)
        {
            List.Remove(value);
        }
        public class FormElementDataListRowEntityEnumerator : object, IEnumerator
        {
            private IEnumerator baseEnumerator;
            private IEnumerable temp;
            public FormElementDataListRowEntityEnumerator(UIElementDataListRowEntityCollection mappings)
            {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }
            public UIElementDataListRowEntity Current
            {
                get
                {
                    return ((UIElementDataListRowEntity)(baseEnumerator.Current));
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
