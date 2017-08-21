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
using System.Diagnostics;
using System.Windows.Forms;
namespace Sheng.SailingEase.Controls.Extensions
{
    public class ToolStripItemCodonCollection : CollectionBase
    {
        public ToolStripItemCodonCollection()
        {
        }
        public ToolStripItemCodonCollection(ToolStripItemCodonCollection value)
        {
            this.AddRange(value);
        }
        public ToolStripItemCodonCollection(IToolStripItemCodon[] value)
        {
            this.AddRange(value);
        }
        public IToolStripItemCodon this[int index]
        {
            get
            {
                return ((IToolStripItemCodon)(List[index]));
            }
            set
            {
                List[index] = value;
            }
        }
        public virtual int Add(IToolStripItemCodon value)
        {
            value.Owner = this.Owner;
            return List.Add(value);
        }
        public void AddRange(IToolStripItemCodon[] value)
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public void AddRange(ToolStripItemCodonCollection value)
        {
            for (int i = 0; (i < value.Count); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public bool Contains(IToolStripItemCodon value)
        {
            return List.Contains(value);
        }
        public void CopyTo(IToolStripItemCodon[] array, int index)
        {
            List.CopyTo(array, index);
        }
        public int IndexOf(IToolStripItemCodon value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, IToolStripItemCodon value)
        {
            value.Owner = Owner;
            List.Insert(index, value);
        }
        public new ToolStripItemAbstractEnumerator GetEnumerator()
        {
            return new ToolStripItemAbstractEnumerator(this);
        }
        public virtual void Remove(IToolStripItemCodon value)
        {
            List.Remove(value);
        }
        private ToolStripCodonBase _owner;
        public ToolStripCodonBase Owner
        {
            get { return _owner; }
            set
            {
                _owner = value;
                foreach (IToolStripItemCodon item in this)
                {
                    item.Owner = value;
                }
            }
        }
        public void RegisterItem(string path, IToolStripItemCodon item)
        {
            if (String.IsNullOrEmpty(path) || item == null)
            {
                Debug.Assert(false, "RegisterToolStripItem，没有指定path或toolStripItem");
                return;
            }
            ToolStripPath toolStripPath = new ToolStripPath(path);
            if (toolStripPath.IsEmpty)
            {
                Debug.Assert(false, "RegisterToolStripItem，toolStripPath.IsEmpty");
                return;
            }
            IToolStripDropDownItemCodon targetItem = null;
            ToolStripItemCodonCollection targetItemCollection = this;
            for (int i = 0; i < toolStripPath.PathPoints.Count; i++)
            {
                foreach (IToolStripItemCodon targetItemCodon in targetItemCollection)
                {
                    if (targetItemCodon.PathPoint == toolStripPath.PathPoints[i].Name
                        && targetItemCodon is IToolStripDropDownItemCodon)
                    {
                        targetItem = targetItemCodon as IToolStripDropDownItemCodon;
                        targetItemCollection = targetItem.Items;
                        break;
                    }
                }
            }
            if (targetItem != null)
            {
                if (toolStripPath.PathPoints[toolStripPath.PathPoints.Count - 1].Index.HasValue)
                {
                    int index = toolStripPath.PathPoints[toolStripPath.PathPoints.Count - 1].Index.Value;
                    if (index >= targetItem.Items.Count)
                    {
                        index = targetItem.Items.Count;
                    }
                    targetItem.Items.Insert(index, item);
                }
                else
                {
                    targetItem.Items.Add(item);
                }
            }
        }
        public IEnumerable<IToolStripItemCodon> ToolStripItemAbstractEnum
        {
            get
            {
                foreach (IToolStripItemCodon ToolStripItemAbstract in this)
                    yield return ToolStripItemAbstract;
            }
        }
        public class ToolStripItemAbstractEnumerator : object, IEnumerator
        {
            private IEnumerator baseEnumerator;
            private IEnumerable temp;
            public ToolStripItemAbstractEnumerator(ToolStripItemCodonCollection mappings)
            {
                this.temp = ((IEnumerable)(mappings));
                this.baseEnumerator = temp.GetEnumerator();
            }
            public IToolStripItemCodon Current
            {
                get
                {
                    return ((IToolStripItemCodon)(baseEnumerator.Current));
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
