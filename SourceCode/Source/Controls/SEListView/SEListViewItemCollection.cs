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
namespace Sheng.SailingEase.Controls
{
    public class SEListViewItemCollection : CollectionBase, IList<SEListViewItem>
    {
        public SEListViewItemCollection()
        {
        }
        public SEListViewItemCollection(SEListViewItemCollection value)
        {
            this.AddRange(value);
        }
        public SEListViewItemCollection(SEListViewItem[] value)
        {
            this.AddRange(value);
        }
        public SEListViewItem this[int index]
        {
            get
            {
                return ((SEListViewItem)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }
        public int Add(SEListViewItem value)
        {
            value.OwnerCollection = this;
            int index = List.Add(value);
            _owner.Refresh();
            return index;
        }
        public void AddRange(SEListViewItem[] value)
        {
            _owner.SuspendLayout();
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
            _owner.ResumeLayout(true);
        }
        public void AddRange(SEListViewItemCollection value)
        {
            _owner.SuspendLayout();
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add(value[i]);
            }
            _owner.ResumeLayout(true);
        }
        public bool Contains(SEListViewItem value)
        {
            return List.Contains(value);
        }
        public void CopyTo(SEListViewItem[] array, int index)
        {
            List.CopyTo(array, index);
        }
        public int IndexOf(SEListViewItem value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, SEListViewItem value)
        {
            value.OwnerCollection = this;
            List.Insert(index, value);
        }
        public void Remove(SEListViewItem value)
        {
            value.OwnerCollection = null;
            List.Remove(value);
            _owner.Refresh();
            _owner.OnItemsRemoved(new List<SEListViewItem>() { value });
        }
        public void Remove(List<SEListViewItem> items)
        {
            _owner.SuspendLayout();
            foreach (var item in items)
            {
                item.OwnerCollection = null;
                List.Remove(item);
            }
            _owner.ResumeLayout(true);
            _owner.OnItemsRemoved(items);
        }
        protected override void OnClear()
        {
            _owner.SuspendLayout();
            base.OnClear();
            _owner.ResumeLayout(true);
        }
        private SEListView _owner;
        internal SEListView Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }
        public SEListViewItem[] ToArray()
        {
            return this.ToList().ToArray();
        }
        public List<SEListViewItem> ToList()
        {
            List<SEListViewItem> list = new List<SEListViewItem>();
            foreach (SEListViewItem e in this)
            {
                list.Add(e);
            }
            return list;
        }
        public void PreTo(SEListViewItem targetEvent, SEListViewItem referEvent)
        {
            if (targetEvent == null || referEvent == null)
                return;
            if (this.Contains(targetEvent) == false || this.Contains(referEvent) == false)
                return;
            int referIndex = this.IndexOf(referEvent);
            if (this.IndexOf(targetEvent) < referIndex)
                referIndex--;
            this.Remove(targetEvent);
            this.Insert(referIndex, targetEvent);
        }
        public void NextTo(SEListViewItem targetEvent, SEListViewItem referEvent)
        {
            if (targetEvent == null || referEvent == null)
                return;
            if (this.Contains(targetEvent) == false || this.Contains(referEvent) == false)
                return;
            int referIndex = this.IndexOf(referEvent);
            if (this.IndexOf(targetEvent) > referIndex)
                referIndex++;
            this.Remove(targetEvent);
            this.Insert(referIndex, targetEvent);
        }
        [Serializable]
        public class ImageListViewItemEnumerator : object, IEnumerator, IEnumerator<SEListViewItem>
        {
            private IEnumerator baseEnumerator;
            private IEnumerable temp;
            public ImageListViewItemEnumerator(SEListViewItemCollection mappings)
            {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }
            public SEListViewItem Current
            {
                get
                {
                    return ((SEListViewItem)(baseEnumerator.Current));
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
        void ICollection<SEListViewItem>.Add(SEListViewItem item)
        {
            this.Add(item);
        }
        public bool IsReadOnly
        {
            get { return false; }
        }
        bool ICollection<SEListViewItem>.Remove(SEListViewItem item)
        {
            this.Remove(item);
            return true;
        }
        public new IEnumerator<SEListViewItem> GetEnumerator()
        {
            return new ImageListViewItemEnumerator(this);
        }
    }
}
