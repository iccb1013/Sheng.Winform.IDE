/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections;
namespace Sheng.SailingEase.Core
{
    [Serializable]
    public class UIElementDataListColumnEntityCollection : CollectionBase
    {
        public UIElementDataListColumnEntityCollection(UIElementDataListEntity dataListEntity)
        {
            this._dataListEntity = dataListEntity;
        }
        public UIElementDataListColumnEntityCollection(UIElementDataListColumnEntityCollection value)
        {
            this.AddRange(value);
        }
        public UIElementDataListColumnEntityCollection(UIElementDataListColumnEntityAbstract[] value)
        {
            this.AddRange(value);
        }
        public UIElementDataListColumnEntityAbstract this[int index]
        {
            get
            {
                return ((UIElementDataListColumnEntityAbstract)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }
        public int Add(UIElementDataListColumnEntityAbstract value)
        {
            value.DataList = this._dataListEntity;
            return List.Add(value);
        }
        public void AddRange(UIElementDataListColumnEntityAbstract[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public void AddRange(UIElementDataListColumnEntityCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public bool Contains(UIElementDataListColumnEntityAbstract value)
        {
            return List.Contains(value);
        }
        public void CopyTo(UIElementDataListColumnEntityAbstract[] array, int index)
        {
            List.CopyTo(array, index);
        }
        public int IndexOf(UIElementDataListColumnEntityAbstract value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, UIElementDataListColumnEntityAbstract value)
        {
            List.Insert(index, value);
        }
        public new FormElementEnumerator GetEnumerator()
        {
            return new FormElementEnumerator(this);
        }
        public void Remove(UIElementDataListColumnEntityAbstract value)
        {
            List.Remove(value);
        }
        private UIElementDataListEntity _dataListEntity;
        public WindowEntity HostFormEntity
        {
            get
            {
                return _dataListEntity.HostFormEntity;
            }
        }
        public UIElementDataListColumnEntityAbstract this[string code]
        {
            get
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
        }
        public bool Contains(string id)
        {
            foreach (UIElementDataListColumnEntityAbstract column in this)
            {
                if (column.Id == id)
                {
                    return true;
                }
            }
            return false;
        }
        public UIElementDataListColumnEntityAbstract GetEntityByCode(string code)
        {
            return this[code];
        }
        public UIElementDataListColumnEntityAbstract GetEntityById(string id)
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
            public FormElementEnumerator(UIElementDataListColumnEntityCollection mappings)
            {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }
            public UIElementDataListColumnEntityAbstract Current
            {
                get
                {
                    return ((UIElementDataListColumnEntityAbstract)(baseEnumerator.Current));
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
