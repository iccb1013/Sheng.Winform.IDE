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
    public class ImageListViewCollection : CollectionBase, IList<ImageListViewItem>
    {
        public ImageListViewCollection()
        {
        }
        public ImageListViewCollection(ImageListViewCollection value)
        {
            this.AddRange(value);
        }
        public ImageListViewCollection(ImageListViewItem[] value)
        {
            this.AddRange(value);
        }
        public ImageListViewItem this[int index]
        {
            get
            {
                return ((ImageListViewItem)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }
        public int Add(ImageListViewItem value)
        {
            value.OwnerCollection = this;
            int index = List.Add(value);
            _owner.Refresh();
            return index;
        }
        public void AddRange(ImageListViewItem[] value)
        {
            _owner.SuspendLayout();
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
            _owner.ResumeLayout();
        }
        public void AddRange(ImageListViewCollection value)
        {
            _owner.SuspendLayout();
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add(value[i]);
            }
            _owner.ResumeLayout();
        }
        public bool Contains(ImageListViewItem value)
        {
            return List.Contains(value);
        }
        public void CopyTo(ImageListViewItem[] array, int index)
        {
            List.CopyTo(array, index);
        }
        public int IndexOf(ImageListViewItem value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, ImageListViewItem value)
        {
            value.OwnerCollection = this;
            List.Insert(index, value);
        }
        public void Remove(ImageListViewItem value)
        {
            value.OwnerCollection = null;
            List.Remove(value);
            _owner.Refresh();
            _owner.OnItemsRemoved(new List<ImageListViewItem>() { value });
        }
        public void Remove(List<ImageListViewItem> items)
        {
            _owner.SuspendLayout();
            foreach (var item in items)
            {
                item.OwnerCollection = null;
                List.Remove(item);
            }
            _owner.ResumeLayout();
            _owner.OnItemsRemoved(items);
        }
        public new void RemoveAt(int index)
        {
            ImageListViewItem removedItem = this[index];
            List.RemoveAt(index);
            _owner.Refresh();
            _owner.OnItemsRemoved(new List<ImageListViewItem>() { removedItem });
        }
        private ImageListView _owner;
        internal ImageListView Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }
        public ImageListViewItem[] ToArray()
        {
            return this.ToList().ToArray();
        }
        public List<ImageListViewItem> ToList()
        {
            List<ImageListViewItem> list = new List<ImageListViewItem>();
            foreach (ImageListViewItem e in this)
            {
                list.Add(e);
            }
            return list;
        }
        public void PreTo(ImageListViewItem targetEvent, ImageListViewItem referEvent)
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
        public void NextTo(ImageListViewItem targetEvent, ImageListViewItem referEvent)
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
        public class ImageListViewItemEnumerator : object, IEnumerator, IEnumerator<ImageListViewItem>
        {
            private IEnumerator baseEnumerator;
            private IEnumerable temp;
            public ImageListViewItemEnumerator(ImageListViewCollection mappings)
            {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }
            public ImageListViewItem Current
            {
                get
                {
                    return ((ImageListViewItem)(baseEnumerator.Current));
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
        void ICollection<ImageListViewItem>.Add(ImageListViewItem item)
        {
            this.Add(item);
        }
        public bool IsReadOnly
        {
            get { return false; }
        }
        bool ICollection<ImageListViewItem>.Remove(ImageListViewItem item)
        {
            this.Remove(item);
            return true;
        }
        public new IEnumerator<ImageListViewItem> GetEnumerator()
        {
            return new ImageListViewItemEnumerator(this);
        }
    }
}
