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
using System.Diagnostics;
namespace Sheng.SailingEase.Core
{
    public class DataItemEntityCollection : CollectionBase
    {
        public DataItemEntityCollection()
        {
        }
        public DataItemEntityCollection(DataItemEntityCollection value)
        {
            this.AddRange(value);
        }
        public DataItemEntityCollection(DataItemEntity[] value)
        {
            this.AddRange(value);
        }
        public DataItemEntity this[int index]
        {
            get
            {
                return ((DataItemEntity)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }
        public int Add(DataItemEntity value)
        {
            return List.Add(value);
        }
        public void Add(params DataItemEntity[] value)
        {
            this.AddRange(value);
        }
        public void AddRange(DataItemEntity[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public void AddRange(DataItemEntityCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public bool Contains(DataItemEntity value)
        {
            return List.Contains(value);
        }
        public void CopyTo(DataItemEntity[] array, int index)
        {
            List.CopyTo(array, index);
        }
        public int IndexOf(DataItemEntity value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, DataItemEntity value)
        {
            List.Insert(index, value);
        }
        public new DataItemEntityEnumerator GetEnumerator()
        {
            return new DataItemEntityEnumerator(this);
        }
        public void Remove(DataItemEntity value)
        {
            List.Remove(value);
        }
        public bool Contains(string id)
        {
            foreach (DataItemEntity item in this)
            {
                if (item.Id == id)
                {
                    return true;
                }
            }
            return false;
        }
        public DataItemEntity[] ToArray()
        {
            return this.ToList().ToArray();
        }
        public List<DataItemEntity> ToList()
        {
            List<DataItemEntity> list = new List<DataItemEntity>();
            foreach (DataItemEntity entity in this)
            {
                list.Add(entity);
            }
            return list;
        }
        public DataItemEntity GetEntityByCode(string code)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Code == code)
                {
                    return this[i];
                }
            }
            return null;
        }
        public DataItemEntity GetEntityById(string id)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Id == id)
                {
                    return this[i];
                }
            }
            return null;
        }
        public int Remove(string id)
        {
            DataItemEntity entity = GetEntityById(id);
            int index = this.IndexOf(entity);
            Debug.Assert(entity != null, "Remove(string id),entity = null");
            if (entity != null)
            {
                this.Remove(entity);
            }
            return index;
        }
        public class DataItemEntityEnumerator : object, IEnumerator
        {
            private IEnumerator baseEnumerator;
            private IEnumerable temp;
            public DataItemEntityEnumerator(DataItemEntityCollection mappings)
            {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }
            public DataItemEntity Current
            {
                get
                {
                    return ((DataItemEntity)(baseEnumerator.Current));
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
