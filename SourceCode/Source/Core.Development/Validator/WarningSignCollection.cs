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
namespace Sheng.SailingEase.Core.Development
{
    public class WarningSignCollection : CollectionBase
    {
        public WarningSignCollection()
        {
        }
        public WarningSignCollection(WarningSignCollection value)
        {
            this.AddRange(value);
        }
        public WarningSignCollection(WarningSign[] value)
        {
            this.AddRange(value);
        }
        public WarningSign this[int index]
        {
            get
            {
                return ((WarningSign)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }
        public virtual int Add(WarningSign value)
        {
            return List.Add(value);
        }
        public void AddRange(WarningSign[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public void AddRange(WarningSignCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public bool Contains(WarningSign value)
        {
            return List.Contains(value);
        }
        public void CopyTo(WarningSign[] array, int index)
        {
            List.CopyTo(array, index);
        }
        public int IndexOf(WarningSign value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, WarningSign value)
        {
            List.Insert(index, value);
        }
        public new WarningSignEnumerator GetEnumerator()
        {
            return new WarningSignEnumerator(this);
        }
        public virtual void Remove(WarningSign value)
        {
            List.Remove(value);
        }
        public class WarningSignEnumerator : object, IEnumerator
        {
            private IEnumerator baseEnumerator;
            private IEnumerable temp;
            public WarningSignEnumerator(WarningSignCollection mappings)
            {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }
            public WarningSign Current
            {
                get
                {
                    return ((WarningSign)(baseEnumerator.Current));
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
