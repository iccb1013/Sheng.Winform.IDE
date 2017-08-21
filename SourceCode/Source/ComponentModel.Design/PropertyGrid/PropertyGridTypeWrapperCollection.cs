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
namespace Sheng.SailingEase.ComponentModel.Design
{
    public class PropertyGridTypeWrapperCollection : CollectionBase
    {
        public PropertyGridTypeWrapperCollection()
        {
        }
        public PropertyGridTypeWrapperCollection(PropertyGridTypeWrapperCollection value)
        {
            this.AddRange(value);
        }
        public PropertyGridTypeWrapperCollection(PropertyGridTypeWrapper[] value)
        {
            this.AddRange(value);
        }
        public PropertyGridTypeWrapper this[int index]
        {
            get
            {
                return ((PropertyGridTypeWrapper)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }
        public virtual int Add(PropertyGridTypeWrapper value)
        {
            return List.Add(value);
        }
        public void AddRange(PropertyGridTypeWrapper[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public void AddRange(PropertyGridTypeWrapperCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public bool Contains(PropertyGridTypeWrapper value)
        {
            return List.Contains(value);
        }
        public void CopyTo(PropertyGridTypeWrapper[] array, int index)
        {
            List.CopyTo(array, index);
        }
        public int IndexOf(PropertyGridTypeWrapper value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, PropertyGridTypeWrapper value)
        {
            List.Insert(index, value);
        }
        public new TypeWrapperEnumerator GetEnumerator()
        {
            return new TypeWrapperEnumerator(this);
        }
        public virtual void Remove(PropertyGridTypeWrapper value)
        {
            List.Remove(value);
        }
        public bool IsVisible(Type type, string property)
        {
            PropertyGridTypeWrapper warpper = GetWrapper(type);
            if (warpper == null)
                return true;
            return warpper.IsVisible(property);
        }
        public bool IsDisable(Type type, string property)
        {
            PropertyGridTypeWrapper warpper = GetWrapper(type);
            if (warpper == null)
                return false;
            return warpper.IsDisible(property);
        }
        public PropertyGridTypeWrapper GetWrapper(Type type)
        {
            PropertyGridTypeWrapper warpper = null;
            foreach (PropertyGridTypeWrapper item in this)
            {
                if (item.Type.Equals(type) || (item.ActOnSubClass && type.IsSubclassOf(item.Type)))
                {
                    warpper = item;
                    break;
                }
            }
            return warpper;
        }
        public PropertyGridTypeWrapper[] ToArray()
        {
            return this.ToList().ToArray();
        }
        public List<PropertyGridTypeWrapper> ToList()
        {
            List<PropertyGridTypeWrapper> list = new List<PropertyGridTypeWrapper>();
            foreach (PropertyGridTypeWrapper TypeWrapper in this)
            {
                list.Add(TypeWrapper);
            }
            return list;
        }
        public IEnumerable<PropertyGridTypeWrapper> TypeWrapperEnum
        {
            get
            {
                foreach (PropertyGridTypeWrapper TypeWrapper in this)
                    yield return TypeWrapper;
            }
        }
        public class TypeWrapperEnumerator : object, IEnumerator
        {
            private IEnumerator baseEnumerator;
            private IEnumerable temp;
            public TypeWrapperEnumerator(PropertyGridTypeWrapperCollection mappings)
            {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }
            public PropertyGridTypeWrapper Current
            {
                get
                {
                    return ((PropertyGridTypeWrapper)(baseEnumerator.Current));
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
