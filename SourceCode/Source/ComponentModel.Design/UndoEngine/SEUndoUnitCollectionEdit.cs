

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Sheng.SailingEase.ComponentModel.Design.Localisation;

namespace Sheng.SailingEase.ComponentModel.Design.UndoEngine
{
    /// <summary>
    /// 为集合操作提供可撤销的工作单元
    /// </summary>
    public class SEUndoUnitCollectionEdit : SEUndoUnitAbstract
    {
        #region 公开属性

        //private string _name;
        private string _itemName = String.Empty;
        public override string Name
        {
            get
            {
                //if (String.IsNullOrEmpty(_name) == false)
                //{
                //    return _name;
                //}
                //else
                //{
                    string name = Language.Current.SEUndoUnitCollectionEdit_Name;

                    //不能直接对Value进行ToString放到name中
                    //对于没有override ToString方法的类，会泄漏程序的结构信息
                    switch (this.EditType)
                    {
                        case CollectionEditType.Add:
                            name = String.Format(Language.Current.SEUndoUnitCollectionEdit_Name_Add, _itemName);
                            break;
                        case CollectionEditType.Edit:
                            name = String.Format(Language.Current.SEUndoUnitCollectionEdit_Name_Edit, _itemName);
                            break;
                        case CollectionEditType.Delete:
                            if (this.Values.Count == 1)
                                name = String.Format(Language.Current.SEUndoUnitCollectionEdit_Name_Delete, _itemName);
                            else
                                name = String.Format(Language.Current.SEUndoUnitCollectionEdit_Name_MultiDelete, this.Values.Count);
                            break;
                        case CollectionEditType.Move:
                            name = String.Format(Language.Current.SEUndoUnitCollectionEdit_Name_Move, _itemName);
                            break;
                    }

                    return name;
                //}
            }
        }

        private IList _list;
        /// <summary>
        /// 所操作的集合
        /// </summary>
        public IList List
        {
            get { return this._list; }
            set { this._list = value; }
        }

        private CollectionEditType _editType;
        /// <summary>
        /// 编辑操作的类型
        /// Edit类形需要使用OldValue
        /// Up,Down需要使用OldIndex
        /// </summary>
        public CollectionEditType EditType
        {
            get { return this._editType; }
            set { this._editType = value; }
        }

        //否决，改用Member
        //private object _oldValue;
        ///// <summary>
        ///// 旧值
        ///// </summary>
        //public object OldValue
        //{
        //    get { return this._oldValue; }
        //    private set { this._oldValue = value; }
        //}

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
        /// <summary>
        /// 值
        /// </summary>
        public object Value
        {
            get { return this._value; }
            set { this._value = value; }
        }

        private int _index = -1;
        /// <summary>
        /// 下标索引
        /// </summary>
        public int Index
        {
            get { return this._index; }
            set { this._index = value; }
        }

        private int _oldIndex = -1;
        /// <summary>
        /// 旧下标索引
        /// </summary>
        public int OldIndex
        {
            get { return this._oldIndex; }
            set { this._oldIndex = value; }
        }

        //private int[] _indexs;
        ///// <summary>
        ///// EditType为Delete时专用
        ///// 因为可能同时删除集合中的多个项
        ///// </summary>
        //public int[] Indexs
        //{
        //    get { return this._indexs; }
        //    private set
        //    {
        //        this._indexs = value;

        //        SortIndexs();
        //    }
        //}

        //private object[] _values;
        ///// <summary>
        ///// EditType为Delete时专用
        ///// 因为可能同时删除集合中的多个项
        ///// 与Indexs，11对应
        ///// </summary>
        //public object[] Values
        //{
        //    get { return this._values; }
        //    private set { this._values = value; }
        //}

        private Dictionary<int, object> _values;
        /// <summary>
        /// EditType为Delete时专用
        /// 用于表示被删除的项，键即项在列表中的索引
        /// 用Dictionary　是因为可能要对其进行排序，使索引由小到大排序，避免在撤销操作重新插入数据时项超出索引
        /// 但是注意，Redo时如果还使用索引，需要从大到小来操作
        /// </summary>
        public Dictionary<int, object> Values
        {
            get { return this._values; }
            set { this._values = value; }
        }

        #endregion

        #region 构造

        public SEUndoUnitCollectionEdit(string itemName)
        {
            _itemName = itemName;
        }

        public SEUndoUnitCollectionEdit(string itemName, CollectionEditEventArgs collectionEditEventArgs)
            : this(itemName)
        {
            this.List = collectionEditEventArgs.List;
            this.EditType = collectionEditEventArgs.EditType;
            this.Value = collectionEditEventArgs.Value;
            this.Index = collectionEditEventArgs.Index;
            this.OldIndex = collectionEditEventArgs.OldIndex;
            this.Values = collectionEditEventArgs.Values;
            this.Members = collectionEditEventArgs.Members;
        }

        #endregion

        #region 公开方法

        //对于Undo和 Redo在处理EditType.Edit时，不能直接用Value，和OldValue去互换
        //而应该直接用Index定位，详见上面的注释

        public override void Undo(bool action)
        {
            switch (this.EditType)
            {
                case CollectionEditType.Add:
                    this.List.Remove(this.Value);
                    break;
                case CollectionEditType.Edit:
                    foreach (SEUndoMember member in this.Members)
                    {
                        member.SetMember(this.Value, SEUndoMember.EnumMemberValue.OldValue);
                    }
                    break;
                case CollectionEditType.Delete:
                    foreach (KeyValuePair<int, object> value in this.Values)
                    {
                        this.List.Insert(value.Key, value.Value);
                    }
                    break;
                case CollectionEditType.Move:
                    SwapIndex(this.Index, this.OldIndex);
                    break;
            }

            if (action && Action != null)
            {
                Action(this, SEUndoEngine.Type.Undo);
            }
        }

        public override void Redo(bool action)
        {
            switch (this.EditType)
            {
                case CollectionEditType.Add:
                    this.List.Add(this.Value);
                    break;
                case CollectionEditType.Edit:
                    //SwapValue(this.OldValue, this.Value);
                    //this.List[this.Index] = this.Value;
                    foreach (SEUndoMember member in this.Members)
                    {
                        member.SetMember(this.Value, SEUndoMember.EnumMemberValue.NewValue);
                    }
                    break;
                case CollectionEditType.Delete:
                    foreach (KeyValuePair<int, object> value in this.Values)
                    {
                        this.List.Remove(value.Value);
                    }
                    break;
                case CollectionEditType.Move:
                    SwapIndex(this.OldIndex, this.Index);
                    break;
            }

            if (action && Action != null)
            {
                Action(this, SEUndoEngine.Type.Redo);
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 用swapValue换掉List中的valueInList
        /// </summary>
        /// <param name="valueInList"></param>
        /// <param name="swapValue"></param>
        private void SwapValue(object valueInList, object swapValue)
        {
            int i = this.List.IndexOf(valueInList);
            this.List.RemoveAt(i);
            this.List.Insert(i, swapValue);
        }

        private void SwapIndex(int index1, int index2)
        {
            object obj1 = this.List[index1];
            object obj2 = this.List[index2];

            this.List.RemoveAt(index1);
            this.List.Insert(index1, obj2);

            this.List.RemoveAt(index2);
            this.List.Insert(index2, obj1);
        }

        #endregion

    }
}