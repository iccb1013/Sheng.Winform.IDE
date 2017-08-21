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
    public class WindowEntityCollection : CollectionBase
    {
        public WindowEntityCollection()
        {
        }
        public WindowEntityCollection(WindowEntityCollection value)
        {
            this.AddRange(value);
        }
        public WindowEntityCollection(WindowEntity[] value)
        {
            this.AddRange(value);
        }
        public WindowEntity this[int index]
        {
            get
            {
                return ((WindowEntity)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }
        public int Add(WindowEntity value)
        {
            return List.Add(value);
        }
        public void AddRange(WindowEntity[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public void AddRange(WindowEntityCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public bool Contains(WindowEntity value)
        {
            return List.Contains(value);
        }
        public bool Contains(string id)
        {
            foreach (WindowEntity item in this)
            {
                if (item.Id == id)
                {
                    return true;
                }
            }
            return false;
        }
        public void CopyTo(WindowEntity[] array, int index)
        {
            List.CopyTo(array, index);
        }
        public int IndexOf(WindowEntity value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, WindowEntity value)
        {
            List.Insert(index, value);
        }
        public new FormEntityEnumerator GetEnumerator()
        {
            return new FormEntityEnumerator(this);
        }
        public void Remove(WindowEntity value)
        {
            List.Remove(value);
        }
        public WindowEntity GetEntityByCode(string code)
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
        public WindowEntity GetEntityById(string id)
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
        public class FormEntityEnumerator : object, IEnumerator
        {
            private IEnumerator baseEnumerator;
            private IEnumerable temp;
            public FormEntityEnumerator(WindowEntityCollection mappings)
            {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }
            public WindowEntity Current
            {
                get
                {
                    return ((WindowEntity)(baseEnumerator.Current));
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
