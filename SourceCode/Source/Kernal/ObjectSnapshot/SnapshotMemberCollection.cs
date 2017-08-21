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
namespace Sheng.SailingEase.Kernal
{
    class SnapshotMemberCollection:CollectionBase
    {
        public SnapshotMemberCollection()
        {
        }
        public SnapshotMemberCollection(SnapshotMemberCollection value)
        {
            this.AddRange(value);
        }
        public SnapshotMemberCollection(SnapshotMember[] value)
        {
            this.AddRange(value);
        }
        public SnapshotMember this[int index]
        {
            get
            {
                return ((SnapshotMember)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }
        public int Add(SnapshotMember value)
        {
            return List.Add(value);
        }
        public void AddRange(SnapshotMember[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public void AddRange(SnapshotMemberCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public bool Contains(SnapshotMember value)
        {
            return List.Contains(value);
        }
        public void CopyTo(SnapshotMember[] array, int index)
        {
            List.CopyTo(array, index);
        }
        public int IndexOf(SnapshotMember value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, SnapshotMember value)
        {
            List.Insert(index, value);
        }
        public new SnapshotMemberEnumerator GetEnumerator()
        {
            return new SnapshotMemberEnumerator(this);
        }
        public void Remove(SnapshotMember value)
        {
            List.Remove(value);
        }
        public string Members
        {
            get
            {
                string members = String.Empty;
                foreach (SnapshotMember member in this)
                {
                    members += member.MemberName + ",";
                }
                return members.TrimEnd(',');
            }
        }
        public int Add(string memberName, object oldValue, object newValue)
        {
            return List.Add(new SnapshotMember(memberName, oldValue, newValue));
        }
        public class SnapshotMemberEnumerator : object, IEnumerator
        {
            private IEnumerator baseEnumerator;
            private IEnumerable temp;
            public SnapshotMemberEnumerator(SnapshotMemberCollection mappings)
            {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }
            public SnapshotMember Current
            {
                get
                {
                    return ((SnapshotMember)(baseEnumerator.Current));
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
