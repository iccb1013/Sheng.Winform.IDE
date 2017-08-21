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
using System.Xml.Linq;
using System.Diagnostics;
namespace  Sheng.SailingEase.Kernal
{
    [Serializable]
    public class XmlableCollection<T> : CollectionBase, IList<T>, IXmlable where T : IXmlable
    {
        public XmlableCollection(string xmlRoot)
        {
            this.XmlRoot = xmlRoot;
        }
        public XmlableCollection(string xmlRoot, XmlableCollection<T> value)
            : this(xmlRoot)
        {
            this.AddRange(value);
        }
        public XmlableCollection(string xmlRoot, T[] value)
            : this(xmlRoot)
        {
            this.AddRange(value);
        }
        public T this[int index]
        {
            get
            {
                return ((T)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }
        public virtual int Add(T value)
        {
            return List.Add(value);
        }
        public void AddRange(T[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public void AddRange(XmlableCollection<T> value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public bool Contains(T value)
        {
            return List.Contains(value);
        }
        public void CopyTo(T[] array, int index)
        {
            List.CopyTo(array, index);
        }
        public int IndexOf(T value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, T value)
        {
            List.Insert(index, value);
        }
        public virtual void Remove(T value)
        {
            List.Remove(value);
        }
        private string _xmlRoot;
        public string XmlRoot
        {
            get { return _xmlRoot; }
            set { _xmlRoot = value; }
        }
        public T[] ToArray()
        {
            T[] array = new T[this.Count];
            for (int i = 0; i < this.Count; i++)
            {
                array[i] = this[i];
            }
            return array;
        }
        public class XmlableEnumerator : object, IEnumerator, IEnumerator<T>
        {
            private IEnumerator baseEnumerator;
            private IEnumerable temp;
            public XmlableEnumerator(XmlableCollection<T> mappings)
            {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }
            public T Current
            {
                get
                {
                    return ((T)(baseEnumerator.Current));
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
        public void FromXml(string strXml)
        {
            if (String.IsNullOrEmpty(this.XmlRoot))
            {
                Debug.Assert(false, "FromXml(strXml) IsNullOrEmpty");
                throw new ArgumentNullException();
            }
            this.Clear();
            XElement element = XElement.Parse(strXml);
            foreach (XElement e in element.Elements())
            {
                IXmlable obj = (IXmlable)Activator.CreateInstance<T>();
                obj.FromXml(e.ToString());
                this.Add((T)obj);
            }
        }
        public string ToXml()
        {
            if (String.IsNullOrEmpty(this.XmlRoot))
            {
                Debug.Assert(false, "没有指定XmlRoot");
                throw new ArgumentNullException();
            }
            XElement element = new XElement(this.XmlRoot);
            foreach (IXmlable xmlable in this)
            {
                element.Add(XElement.Parse(xmlable.ToXml()));
            }
            return element.ToString();
        }
        void ICollection<T>.Add(T item)
        {
            this.Add(item);
        }
        public bool IsReadOnly
        {
            get { return false; }
        }
        bool ICollection<T>.Remove(T item)
        {
            this.Remove(item);
            return true;
        }
        public new IEnumerator<T> GetEnumerator()
        {
            return new XmlableEnumerator(this);
        }
    }
}
