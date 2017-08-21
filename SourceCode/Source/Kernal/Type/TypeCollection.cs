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
namespace Sheng.SailingEase.Kernal
{
    public class TypeCollection : CollectionBase
    {
        public TypeCollection()
        {
        }
        public TypeCollection(TypeCollection value)
        {
            this.AddRange(value);
        }
        public TypeCollection(Type[] value)
        {
            this.AddRange(value);
        }
        public Type this[int index]
        {
            get
            {
                return ((Type)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }
        public virtual int Add(Type value)
        {
            return List.Add(value);
        }
        public void AddRange(Type[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public void AddRange(TypeCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public bool Contains(Type value)
        {
            if (_actOnSub == false)
            {
                return List.Contains(value);
            }
            else
            {
                foreach (Type item in List)
                {
                    if (value.Equals(item) || ReflectionHelper.IsSubOf(value, item))
                        return true;
                }
                return false;
            }
        }
        public void CopyTo(Type[] array, int index)
        {
            List.CopyTo(array, index);
        }
        public int IndexOf(Type value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, Type value)
        {
            List.Insert(index, value);
        }
        public new TypeEnumerator GetEnumerator()
        {
            return new TypeEnumerator(this);
        }
        public virtual void Remove(Type value)
        {
            List.Remove(value);
        }
        private bool _actOnSub = false;
        public bool ActOnSub
        {
            get { return _actOnSub; }
            set { _actOnSub = value; }
        }
        public bool Contains(object obj)
        {
            Type type = obj.GetType();
            return this.Contains(type);
        }
        public List<T> GetInstanceList<T>()
        {
            List<T> list = new List<T>();
            foreach (Type type in this)
            {
                list.Add((T)Activator.CreateInstance(type));
            }
            return list;
        }
        public Type[] ToArray()
        {
            return this.ToList().ToArray();
        }
        public List<Type> ToList()
        {
            List<Type> list = new List<Type>();
            foreach (Type type in this)
            {
                list.Add(type);
            }
            return list;
        }
        public IEnumerable<Type> TypeEnum
        {
            get
            {
                foreach (Type type in this)
                    yield return type;
            }
        }
        public class TypeEnumerator : object, IEnumerator
        {
            private IEnumerator baseEnumerator;
            private IEnumerable temp;
            public TypeEnumerator(TypeCollection mappings)
            {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }
            public Type Current
            {
                get
                {
                    return ((Type)(baseEnumerator.Current));
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
