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
namespace Sheng.SailingEase.ComponentModel.Design.UndoEngine
{
    public class StepListItemCollection:CollectionBase
    {
        private StepList _stepList;
        public StepList StepList
        {
            get { return this._stepList; }
            set { this._stepList = value; }
        }
        public StepListItemCollection()
        {
        }
        public StepListItemCollection(StepListItemCollection value)
        {
            this.AddRange(value);
        }
        public StepListItemCollection(StepListItem[] value)
        {
            this.AddRange(value);
        }
        public StepListItem this[int index]
        {
            get
            {
                return ((StepListItem)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }
        public int Add(StepListItem value)
        {
            value.List = this.StepList;
            return List.Add(value);
        }
        public int Add(SEUndoUnitAbstract obj)
        {
            return this.Add(new StepListItem(obj));
        }
        public void AddRange(SEUndoUnitAbstract[] collection)
        {
            foreach (SEUndoUnitAbstract obj in collection)
            {
                this.Add(new StepListItem(obj));
            }
        }
        public void AddRange(StepListItem[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public void AddRange(StepListItemCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public void SetRange(SEUndoUnitAbstract[] collection)
        {
            this.List.Clear();
            this.AddRange(collection);
        }
        public void SetRange(StepListItem[] value)
        {
            this.List.Clear();
            this.AddRange(value);
        }
        public bool Contains(StepListItem value)
        {
            return List.Contains(value);
        }
        public void CopyTo(StepListItem[] array, int index)
        {
            List.CopyTo(array, index);
        }
        public int IndexOf(StepListItem value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, StepListItem value)
        {
            List.Insert(index, value);
        }
        public new StepListItemEnumerator GetEnumerator()
        {
            return new StepListItemEnumerator(this);
        }
        public void Remove(StepListItem value)
        {
            List.Remove(value);
        }
        public class StepListItemEnumerator : object, IEnumerator
        {
            private IEnumerator baseEnumerator;
            private IEnumerable temp;
            public StepListItemEnumerator(StepListItemCollection mappings)
            {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }
            public StepListItem Current
            {
                get
                {
                    return ((StepListItem)(baseEnumerator.Current));
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
