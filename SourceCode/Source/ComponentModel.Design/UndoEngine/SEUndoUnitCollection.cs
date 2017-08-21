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
    public class SEUndoUnitCollection : CollectionBase
    {
        public SEUndoUnitCollection()
        {
        }
        public SEUndoUnitCollection(SEUndoUnitCollection value)
        {
            this.AddRange(value);
        }
        public SEUndoUnitCollection(SEUndoUnitAbstract[] value)
        {
            this.AddRange(value);
        }
        public SEUndoUnitAbstract this[int index]
        {
            get
            {
                return ((SEUndoUnitAbstract)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }
        public int Add(SEUndoUnitAbstract value)
        {
            return List.Add(value);
        }
        public void AddRange(SEUndoUnitAbstract[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public void AddRange(SEUndoUnitCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public bool Contains(SEUndoUnitAbstract value)
        {
            return List.Contains(value);
        }
        public void CopyTo(SEUndoUnitAbstract[] array, int index)
        {
            List.CopyTo(array, index);
        }
        public int IndexOf(SEUndoUnitAbstract value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, SEUndoUnitAbstract value)
        {
            List.Insert(index, value);
        }
        public new SEUndoUnitEnumerator GetEnumerator()
        {
            return new SEUndoUnitEnumerator(this);
        }
        public void Remove(SEUndoUnitAbstract value)
        {
            List.Remove(value);
        }
        public Action<SEUndoUnitAbstract, SEUndoEngine.Type> Action { get; set; }
        public class SEUndoUnitEnumerator : object, IEnumerator
        {
            private IEnumerator baseEnumerator;
            private IEnumerable temp;
            public SEUndoUnitEnumerator(SEUndoUnitCollection mappings)
            {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }
            public SEUndoUnitAbstract Current
            {
                get
                {
                    return ((SEUndoUnitAbstract)(baseEnumerator.Current));
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
