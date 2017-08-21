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
    public class EntityCollection : CollectionBase
    {
        public EntityCollection()
        {
        }
        public EntityCollection(EntityCollection value)
        {
            this.AddRange(value);
        }
        public EntityCollection(EntityBase[] value)
        {
            this.AddRange(value);
        }
        public EntityBase this[int index]
        {
            get
            {
                return ((EntityBase)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }
        public int Add(EntityBase value)
        {
            return List.Add(value);
        }
        public void AddRange(EntityBase[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public void AddRange(EntityCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public bool Contains(EntityBase value)
        {
            return List.Contains(value);
        }
        public void CopyTo(EntityBase[] array, int index)
        {
            List.CopyTo(array, index);
        }
        public int IndexOf(EntityBase value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, EntityBase value)
        {
            List.Insert(index, value);
        }
        public new EntityEnumerator GetEnumerator()
        {
            return new EntityEnumerator(this);
        }
        public void Remove(EntityBase value)
        {
            List.Remove(value);
        }
        public class EntityEnumerator : object, IEnumerator
        {
            private IEnumerator baseEnumerator;
            private IEnumerable temp;
            public EntityEnumerator(EntityCollection mappings)
            {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }
            public EntityBase Current
            {
                get
                {
                    return ((EntityBase)(baseEnumerator.Current));
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
