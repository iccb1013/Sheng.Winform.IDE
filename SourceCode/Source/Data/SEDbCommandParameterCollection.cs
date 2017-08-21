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
namespace Sheng.SailingEase.Data
{
    public class SEDbCommandParameterCollection : CollectionBase
    {
        public SEDbCommandParameterCollection()
        {
        }
        public SEDbCommandParameterCollection(SEDbCommandParameterCollection value)
        {
            this.AddRange(value);
        }
        public SEDbCommandParameterCollection(SEDbCommandParameter[] value)
        {
            this.AddRange(value);
        }
        public SEDbCommandParameter this[int index]
        {
            get
            {
                return ((SEDbCommandParameter)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }
        public int Add(SEDbCommandParameter value)
        {
            return List.Add(value);
        }
        public void AddRange(SEDbCommandParameter[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public void AddRange(SEDbCommandParameterCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public bool Contains(SEDbCommandParameter value)
        {
            return List.Contains(value);
        }
        public void CopyTo(SEDbCommandParameter[] array, int index)
        {
            List.CopyTo(array, index);
        }
        public int IndexOf(SEDbCommandParameter value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, SEDbCommandParameter value)
        {
            List.Insert(index, value);
        }
        public new DataItemEntityEnumerator GetEnumerator()
        {
            return new DataItemEntityEnumerator(this);
        }
        public void Remove(SEDbCommandParameter value)
        {
            List.Remove(value);
        }
        public class DataItemEntityEnumerator : object, IEnumerator
        {
            private IEnumerator baseEnumerator;
            private IEnumerable temp;
            public DataItemEntityEnumerator(SEDbCommandParameterCollection mappings)
            {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }
            public SEDbCommandParameter Current
            {
                get
                {
                    return ((SEDbCommandParameter)(baseEnumerator.Current));
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
