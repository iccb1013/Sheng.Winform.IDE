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
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.ComponentModel.Design.UndoEngine
{
    public class SEUndoMemberCollection : CollectionBase
    {
        public SEUndoMemberCollection()
        {
        }
        public SEUndoMemberCollection(SEUndoMemberCollection value)
        {
            this.AddRange(value);
        }
        public SEUndoMemberCollection(SEUndoMember[] value)
        {
            this.AddRange(value);
        }
        public SEUndoMember this[int index]
        {
            get
            {
                return ((SEUndoMember)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }
        public int Add(SEUndoMember value)
        {
            return List.Add(value);
        }
        public void AddRange(SEUndoMember[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public void AddRange(SEUndoMemberCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public bool Contains(SEUndoMember value)
        {
            return List.Contains(value);
        }
        public void CopyTo(SEUndoMember[] array, int index)
        {
            List.CopyTo(array, index);
        }
        public int IndexOf(SEUndoMember value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, SEUndoMember value)
        {
            List.Insert(index, value);
        }
        public new SEUndoMemberEnumerator GetEnumerator()
        {
            return new SEUndoMemberEnumerator(this);
        }
        public void Remove(SEUndoMember value)
        {
            List.Remove(value);
        }
        public string Members
        {
            get
            {
                string members = String.Empty;
                foreach (SEUndoMember member in this)
                {
                    members += member.MemberName + ",";
                }
                return members.TrimEnd(',');
            }
        }
        public int Add(string memberName, object oldValue, object newValue)
        {
            return List.Add(new SEUndoMember(memberName, oldValue, newValue));
        }
        public void Add(List<ObjectCompareResult> compareResult)
        {
            foreach (ObjectCompareResult result in compareResult)
            {
                this.Add(result.MemberName, result.SourceValue, result.CompareValue);
            }
        }
        public class SEUndoMemberEnumerator : object, IEnumerator
        {
            private IEnumerator baseEnumerator;
            private IEnumerable temp;
            public SEUndoMemberEnumerator(SEUndoMemberCollection mappings)
            {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }
            public SEUndoMember Current
            {
                get
                {
                    return ((SEUndoMember)(baseEnumerator.Current));
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
