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
    public class UIElementCollection : CollectionBase, IEnumerable<UIElement>
    {
        public UIElementCollection(WindowEntity hostFormEntity)
        {
            this._hostFormEntity = hostFormEntity;
        }
        public UIElementCollection(UIElementCollection value)
        {
            this.AddRange(value);
        }
        public UIElementCollection(UIElement[] value)
        {
            this.AddRange(value);
        }
        public UIElement this[int index]
        {
            get
            {
                return ((UIElement)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }
        public int Add(UIElement value)
        {
            if (value.HostFormEntity == null || (
                value.HostFormEntity != null && value.HostFormEntity.Equals(this._hostFormEntity) == false))
            {
                value.HostFormEntity = this._hostFormEntity;
            }
            return List.Add(value);
        }
        public void AddRange(UIElement[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public void AddRange(UIElementCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public bool Contains(UIElement value)
        {
            foreach (UIElement element in this)
            {
                if (element == value)
                {
                    return true;
                }
                foreach (UIElement innerElement in element.GetInnerElement())
                {
                    if (innerElement == value)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool Contains(string id)
        {
            foreach (UIElement element in this)
            {
                if (element.Id == id)
                {
                    return true;
                }
                foreach (UIElement innerElement in element.GetInnerElement())
                {
                    if (innerElement.Id == id)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void CopyTo(UIElement[] array, int index)
        {
            List.CopyTo(array, index);
        }
        public int IndexOf(UIElement value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, UIElement value)
        {
            if (value.HostFormEntity == null || (
                value.HostFormEntity != null && value.HostFormEntity.Equals(this._hostFormEntity) == false))
            {
                value.HostFormEntity = this._hostFormEntity;
            }
            List.Insert(index, value);
        }
        public void Remove(UIElement value)
        {
            if (this.Contains(value))
                List.Remove(value);
        }
        [NonSerialized]
        private WindowEntity _hostFormEntity;
        public UIElement GetFormElementByCode(string code)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Code == code)
                {
                    return this[i];
                }
                foreach (UIElement innerElement in this[i].GetInnerElement())
                {
                    if (innerElement.Code == code)
                    {
                        return innerElement;
                    }
                }
            }
            return null;
        }
        public UIElement GetFormElementById(string id)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Id == id)
                {
                    return this[i];
                }
                foreach (UIElement innerElement in this[i].GetInnerElement())
                {
                    if (innerElement.Id == id)
                    {
                        return innerElement;
                    }
                }
            }
            return null;
        }
        public static explicit operator UIElementCollection(UIElementDataListColumnEntityCollection dataColumnEntityCollection)
        {
            UIElementCollection collection = new UIElementCollection(dataColumnEntityCollection.HostFormEntity);
            foreach (UIElementDataListColumnEntityAbstract dataColumn in dataColumnEntityCollection)
                collection.Add(dataColumn);
            return collection;
        }
        public class FormElementEnumerator : object, IEnumerator, IEnumerator<UIElement>
        {
            private IEnumerator baseEnumerator;
            private IEnumerable temp;
            public FormElementEnumerator(UIElementCollection mappings)
            {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }
            public UIElement Current
            {
                get
                {
                    return ((UIElement)(baseEnumerator.Current));
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
        public new IEnumerator<UIElement> GetEnumerator()
        {
            return new FormElementEnumerator(this);
        }
    }
}
