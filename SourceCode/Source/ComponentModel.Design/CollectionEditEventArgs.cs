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
using Sheng.SailingEase.ComponentModel.Design.UndoEngine;
namespace Sheng.SailingEase.ComponentModel
{
    public class CollectionEditEventArgs : EventArgs
    {
        private IList _list;
        public IList List
        {
            get { return this._list; }
            private set { this._list = value; }
        }
        private CollectionEditType _editType;
        public CollectionEditType EditType
        {
            get { return this._editType; }
            private set { this._editType = value; }
        }
        private SEUndoMemberCollection _members;
        public SEUndoMemberCollection Members
        {
            get
            {
                if (_members == null)
                    _members = new SEUndoMemberCollection();
                return _members;
            }
            set { _members = value; }
        }
        private object _value;
        public object Value
        {
            get { return this._value; }
            private set { this._value = value; }
        }
        private int _index = -1;
        public int Index
        {
            get { return this._index; }
            private set { this._index = value; }
        }
        private int _oldIndex = -1;
        public int OldIndex
        {
            get { return this._oldIndex; }
            private set { this._oldIndex = value; }
        }
        private Dictionary<int, object> _values;
        public Dictionary<int, object> Values
        {
            get { return this._values; }
            set
            {
                if (this._values != null)
                    this._values.Clear();
                else
                    this._values = new Dictionary<int, object>();
                foreach (KeyValuePair<int, object> item in value.OrderBy(x => x.Key))
                {
                    this._values.Add(item.Key, item.Value);
                }
            }
        }
        public CollectionEditEventArgs(IList list, CollectionEditType editType)
            : this(list, editType, -1, null)
        {
        }
        public CollectionEditEventArgs(IList list, CollectionEditType editType, int index, object value)
            : this(list, editType, index, -1, value)
        {
        }
        public CollectionEditEventArgs(IList list, Dictionary<int, object> values)
            : this(list, CollectionEditType.Delete)
        {
            this.Values = values;
        }
        public CollectionEditEventArgs(IList list, CollectionEditType editType, int index, int oldIndex, object value)
        {
            this.List = list;
            this.EditType = editType;
            this.Index = index;
            this.OldIndex = oldIndex;
            this.Value = value;
        }
    }
    public enum CollectionEditType
    {
        Add,
        Edit,
        Delete,
        Move
    }
}
