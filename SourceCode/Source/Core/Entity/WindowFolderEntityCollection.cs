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
    public class WindowFolderEntityCollection:CollectionBase
    {
         public WindowFolderEntityCollection()
        {
        }
        public WindowFolderEntityCollection(WindowFolderEntityCollection value)
        {
            this.AddRange(value);
        }
        public WindowFolderEntityCollection(WindowFolderEntity[] value)
        {
            this.AddRange(value);
        }
        public WindowFolderEntity this[int index]
        {
            get
            {
                return ((WindowFolderEntity)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }
        public int Add(WindowFolderEntity value)
        {
            return List.Add(value);
        }
        public void AddRange(WindowFolderEntity[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public void AddRange(WindowFolderEntityCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public bool Contains(WindowFolderEntity value)
        {
            return List.Contains(value);
        }
        public bool Contains(string id)
        {
            foreach (WindowFolderEntity item in this)
            {
                if (item.Id == id)
                {
                    return true;
                }
            }
            return false;
        }
        public void CopyTo(WindowFolderEntity[] array, int index)
        {
            List.CopyTo(array, index);
        }
        public int IndexOf(WindowFolderEntity value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, WindowFolderEntity value)
        {
            List.Insert(index, value);
        }
        public new FormFolderEntityEnumerator GetEnumerator()
        {
            return new FormFolderEntityEnumerator(this);
        }
        public void Remove(WindowFolderEntity value)
        {
            List.Remove(value);
        }
        public WindowFolderEntity GetEntityById(string id)
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
        public class FormFolderEntityEnumerator : object, IEnumerator
        {
            private IEnumerator baseEnumerator;
            private IEnumerable temp;
            public FormFolderEntityEnumerator(WindowFolderEntityCollection mappings)
            {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }
            public WindowFolderEntity Current
            {
                get
                {
                    return ((WindowFolderEntity)(baseEnumerator.Current));
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
