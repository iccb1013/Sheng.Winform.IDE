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
    public class ToolStripItemEntityAttributeCollection : CollectionBase, IList<ToolStripItemEntityAttribute>
    {
        public ToolStripItemEntityAttributeCollection()
        {
        }
        public ToolStripItemEntityAttributeCollection(ToolStripItemEntityAttributeCollection value)
        {
            this.AddRange(value);
        }
        public ToolStripItemEntityAttributeCollection(ToolStripItemEntityAttribute[] value)
        {
            this.AddRange(value);
        }
        public ToolStripItemEntityAttribute this[int index]
        {
            get
            {
                return ((ToolStripItemEntityAttribute)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }
        public int Add(ToolStripItemEntityAttribute value)
        {
            return List.Add(value);
        }
        public void AddRange(ToolStripItemEntityAttribute[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public void AddRange(ToolStripItemEntityAttributeCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public bool Contains(ToolStripItemEntityAttribute value)
        {
            return List.Contains(value);
        }
        public void CopyTo(ToolStripItemEntityAttribute[] array, int index)
        {
            List.CopyTo(array, index);
        }
        public int IndexOf(ToolStripItemEntityAttribute value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, ToolStripItemEntityAttribute value)
        {
            List.Insert(index, value);
        }
        public void Remove(ToolStripItemEntityAttribute value)
        {
            List.Remove(value);
        }
        public ToolStripItemEntityAttribute[] ToArray()
        {
            return this.ToList().ToArray();
        }
        public List<ToolStripItemEntityAttribute> ToList()
        {
            List<ToolStripItemEntityAttribute> list = new List<ToolStripItemEntityAttribute>();
            foreach (ToolStripItemEntityAttribute e in this)
            {
                list.Add(e);
            }
            return list;
        }
        public class ToolStripItemEntityAttributeEnumerator : object, IEnumerator, IEnumerator<ToolStripItemEntityAttribute>
        {
            private IEnumerator baseEnumerator;
            private IEnumerable temp;
            public ToolStripItemEntityAttributeEnumerator(ToolStripItemEntityAttributeCollection mappings)
            {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }
            public ToolStripItemEntityAttribute Current
            {
                get
                {
                    return ((ToolStripItemEntityAttribute)(baseEnumerator.Current));
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
            public void Dispose()
            {
            }
        }
        void ICollection<ToolStripItemEntityAttribute>.Add(ToolStripItemEntityAttribute item)
        {
            this.Add(item);
        }
        public bool IsReadOnly
        {
            get { return false; }
        }
        bool ICollection<ToolStripItemEntityAttribute>.Remove(ToolStripItemEntityAttribute item)
        {
            this.Remove(item);
            return true;
        }
        public new IEnumerator<ToolStripItemEntityAttribute> GetEnumerator()
        {
            return new ToolStripItemEntityAttributeEnumerator(this);
        }
    }
}
