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
    public class ToolStripItemEntityCollection : CollectionBase
    {
        public ToolStripItemEntityCollection()
        {
        }
        public ToolStripItemEntityCollection(ToolStripItemEntityCollection value)
        {
            this.AddRange(value);
        }
        public ToolStripItemEntityCollection(ToolStripItemAbstract[] value)
        {
            this.AddRange(value);
        }
        public ToolStripItemAbstract this[int index]
        {
            get
            {
                return ((ToolStripItemAbstract)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }
        public int Add(ToolStripItemAbstract value)
        {
            return List.Add(value);
        }
        public void AddRange(ToolStripItemAbstract[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public void AddRange(ToolStripItemEntityCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public bool Contains(ToolStripItemAbstract value)
        {
            return List.Contains(value);
        }
        public bool Contains(string id)
        {
            foreach (ToolStripItemAbstract item in this)
            {
                if (item.Id == id)
                {
                    return true;
                }
            }
            return false;
        }
        public void CopyTo(ToolStripItemAbstract[] array, int index)
        {
            List.CopyTo(array, index);
        }
        public int IndexOf(ToolStripItemAbstract value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, ToolStripItemAbstract value)
        {
            List.Insert(index, value);
        }
        public new ToolStripItemEntityEnumerator GetEnumerator()
        {
            return new ToolStripItemEntityEnumerator(this);
        }
        public void Remove(ToolStripItemAbstract value)
        {
            List.Remove(value);
        }
        public ToolStripItemAbstract GetEntityByCode(string code)
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
        public ToolStripItemAbstract GetEntityById(string id)
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
        public class ToolStripItemEntityEnumerator : object, IEnumerator
        {
            private IEnumerator baseEnumerator;
            private IEnumerable temp;
            public ToolStripItemEntityEnumerator(ToolStripItemEntityCollection mappings)
            {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }
            public ToolStripItemAbstract Current
            {
                get
                {
                    return ((ToolStripItemAbstract)(baseEnumerator.Current));
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
