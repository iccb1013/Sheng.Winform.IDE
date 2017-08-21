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
    public class MenuEntityCollection : CollectionBase
    {
        public MenuEntityCollection()
        {
        }
        public MenuEntityCollection(MenuEntityCollection value)
        {
            this.AddRange(value);
        }
        public MenuEntityCollection(MenuEntity[] value)
        {
            this.AddRange(value);
        }
        public MenuEntity this[int index]
        {
            get
            {
                return ((MenuEntity)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }
        public int Add(MenuEntity value)
        {
            return List.Add(value);
        }
        public void AddRange(MenuEntity[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public void AddRange(MenuEntityCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public bool Contains(MenuEntity value)
        {
            return List.Contains(value);
        }
        public bool Contains(string id)
        {
            foreach (MenuEntity item in this)
            {
                if (item.Id == id)
                {
                    return true;
                }
            }
            return false;
        }
        public void CopyTo(MenuEntity[] array, int index)
        {
            List.CopyTo(array, index);
        }
        public int IndexOf(MenuEntity value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, MenuEntity value)
        {
            List.Insert(index, value);
        }
        public new FormElementEnumerator GetEnumerator()
        {
            return new FormElementEnumerator(this);
        }
        public void Remove(MenuEntity value)
        {
            List.Remove(value);
        }
        public MenuEntity GetEntityByCode(string code)
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
        public MenuEntity GetEntityById(string id)
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
        public class FormElementEnumerator : object, IEnumerator
        {
            private IEnumerator baseEnumerator;
            private IEnumerable temp;
            public FormElementEnumerator(MenuEntityCollection mappings)
            {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }
            public MenuEntity Current
            {
                get
                {
                    return ((MenuEntity)(baseEnumerator.Current));
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
