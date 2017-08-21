using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;

namespace Sheng.SailingEase.Kernal
{
    /*
     * MSDN ListBox:
     * 除了显示和选择功能外，ListBox 还提供一些功能，使您得以有效地将项添加到 ListBox 中以及在列表的项内查找文本。
     * BeginUpdate 和 EndUpdate 方法使您得以将大量项添加到 ListBox 中，而不必每次将一个项添加到列表中时都重新绘制该控件
     * 。FindString 和 FindStringExact 方法使您得以在列表中搜索包含特定搜索字符串的项。
     */

    public class ListBoxController
    {
        #region 私有成员

        private ListBox _listBox;

        /// <summary>
        /// 当前所绑定的对象的对象类型
        /// </summary>
        private Type _currentType;

        private BindingList<object> _bindingList = new BindingList<object>();

        #endregion

        #region 公开属性

        public bool _allowNullSelection = false;
        /// <summary>
        /// 是否在绑定数据之后，在列表头部添加一个"无"的选择（表示不选择任何一项）
        /// </summary>
        public bool AllowNullSelection
        {
            get { return _allowNullSelection; }
            set { _allowNullSelection = value; }
        }

        /// <summary>
        /// 是否允许多选
        /// </summary>
        public bool AllowMultiSelect
        {
            get { return _listBox.SelectionMode == SelectionMode.MultiExtended; }
            set
            {
                if (value)
                    _listBox.SelectionMode = SelectionMode.MultiExtended;
                else
                    _listBox.SelectionMode = SelectionMode.One;
            }
        }

        public string DisplayMember
        {
            get { return _listBox.DisplayMember; }
            set { _listBox.DisplayMember = value; }
        }

        #endregion

        #region 构造

        public ListBoxController(ListBox listBox)
        {
            _listBox = listBox;
            _listBox.SelectionMode = SelectionMode.One;

            _listBox.SelectedIndexChanged += new EventHandler(_listBox_SelectedIndexChanged);
        }

        #endregion

        #region 私有方法

        private void ClearSelectedItems()
        {
            _listBox.ClearSelected();
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 清除数据
        /// </summary>
        public void Clear()
        {
            _currentType = null;
            _listBox.DataSource = null;
            _bindingList = null;
        }

        #region DataBind

        public void DataBind<T>(IList list)
        {
            DataBind(list, typeof(T));
        }

        public void DataBind(IList list, Type type)
        {
            _currentType = type;

            _listBox.DataSource = null;
            _bindingList = null;

            if (list != null)
            {
                _bindingList = new BindingList<object>(list.Cast<object>().ToList());

                if (AllowNullSelection)
                    _bindingList.Insert(0, "<Null>");

                _listBox.DataSource = _bindingList;
            }
        }

        #endregion

        #region Add,Update,Remove

        public void Add(object obj)
        {
            Debug.Assert(obj != null, "obj为null");

            if (obj == null)
                return;

            if (_bindingList != null && _bindingList.Contains(obj))
            {
                Debug.Assert(false, "_bindingList中已经存在了指定对象");
                return;
            }

            Type objType = obj.GetType();

            if (_currentType == null)
            {
                _currentType = objType;
            }

            if (objType.Equals(_currentType) || objType.IsSubclassOf(_currentType))
            {
                //如果调用了Clear之后没有Show新的数据，那么_bindingList是为null的
                //或者压根调用 Add 之前没有调用过 DataBind
                if (_bindingList == null)
                {
                    _bindingList = new BindingList<object>();
                }

                this._bindingList.Add(obj);

                if (_listBox.DataSource == null)
                {
                    _listBox.DataSource = _bindingList;
                }
            }
        }

        public void Update(object obj)
        {
            if (_bindingList == null)
                return;

            Debug.Assert(obj != null, "obj为null");

            if (obj == null)
                return;

            Debug.Assert(_bindingList.Contains(obj), "_bindingList中没有指定对象");

            if (_bindingList.Contains(obj) == false)
                return;

            Type objType = obj.GetType();

            if (objType.Equals(_currentType) || objType.IsSubclassOf(_currentType))
            {
                this._bindingList.ResetItem(this._bindingList.IndexOf(obj));
            }
        }

        public void Remove(object obj)
        {
            if (_bindingList == null)
                return;

            Debug.Assert(obj != null, "obj为null");

            if (obj == null)
                return;

            Debug.Assert(_bindingList.Contains(obj), "_bindingList中没有指定对象");

            if (_bindingList.Contains(obj) == false)
                return;

            Type objType = obj.GetType();

            if (objType.Equals(_currentType) || objType.IsSubclassOf(_currentType))
            {
                this._bindingList.Remove(obj);
            }
        }

        /// <summary>
        /// 将指定对象 obj 替换为 newObj
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="newObj"></param>
        public void Replace(object obj, object newObj)
        {
            if (_bindingList == null)
                return;

            Debug.Assert(obj != null && newObj != null, "obj为null");

            if (obj == null || newObj == null)
                return;

            Debug.Assert(_bindingList.Contains(obj), "_bindingList中没有指定对象");

            if (_bindingList.Contains(obj) == false)
                return;

            Type objType = obj.GetType();
            Type newObjType = newObj.GetType();

            if ((objType.Equals(_currentType) || objType.IsSubclassOf(_currentType)) &&
                (newObjType.Equals(_currentType) || newObjType.IsSubclassOf(_currentType)))
            {
                int index = _bindingList.IndexOf(obj);
                _bindingList.RemoveAt(index);
                _bindingList.Insert(index, newObj);
            }
        }

        #endregion

        #region GetItem(s)...

        /// <summary>
        /// 获取当前选定的对象
        /// 如果没有选定行，返回null，如果选了多行，返回选定行中第一行的绑定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetSelectedItem<T>()
        {
            if (_listBox.SelectedItems.Count == 0)
                return default(T);

            Type tType = typeof(T);

            if (tType.Equals(_currentType) == false && _currentType.IsSubclassOf(tType) == false)
                return default(T);

            object obj = _listBox.SelectedItems[0];

            //Debug.Assert(obj.GetType() == tType || obj.GetType().IsSubclassOf(tType),
            //    "DataGridView里的数据不是指定的类型");

            Type objType = obj.GetType();

            if (objType.Equals(_currentType) || objType.IsSubclassOf(_currentType))
                return (T)obj;
            else
                return default(T);
        }

        /// <summary>
        /// 获取指定索引行处的绑定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        public T GetItem<T>(int index)
        {
            if (index < 0)
                return default(T);

            Type tType = typeof(T);

            if (tType.Equals(_currentType) == false && _currentType.IsSubclassOf(tType) == false)
                return default(T);

            object obj = _listBox.Items[index];

            Debug.Assert(obj.GetType() == tType || obj.GetType().IsSubclassOf(tType),
                "DataGridView里的数据不是指定的类型");

            return (T)obj;
        }

        /// <summary>
        /// 当前选中的指定类型的对象集合
        /// 如果当前显示的对象类型不是指定类型或没有任何选则，返回一个空集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetSelectedItems<T>()
        {
            List<T> list = new List<T>();

            if (_listBox.SelectedItems.Count == 0)
                return list;

            Type tType = typeof(T);

            if (tType.Equals(_currentType) == false && _currentType.IsSubclassOf(tType) == false)
                return list;

            foreach (object obj in _listBox.SelectedItems)
            {
                list.Add((T)obj);
            }

            return list;
        }

        public T Find<T>(Filter<T> filter) where T : class
        {
            if (_bindingList == null || _bindingList.Count == 0)
                return null;

            Type tType = typeof(T);

            if (tType.Equals(_currentType) == false && _currentType.IsSubclassOf(tType) == false)
                return null;

            foreach (object obj in _bindingList)
            {
                //此处添加 一个 tObj并判断是否为null
                //是因为此controller支持在列表在顶上添加一个<无>的选择，这只是一个字符串
                //用as转T，如果T不是字符串，肯定为null，filter(null)没有什么意义，外部如果不判断也会报错
                T tObj = obj as T;
                if (tObj != null && filter(tObj))
                {
                    return obj as T;
                }
            }

            return null;
        }

        #endregion

        #region Select

        /// <summary>
        /// 中指定对象（所在的行）
        /// 排它性选择，取消现有选择
        /// </summary>
        /// <param name="obj"></param>
        public void Select(object obj)
        {
            Select(obj, true);
        }

        /// <summary>
        /// 中指定对象（所在的行）
        /// exclusive 用于指定是否是排它性选择，既取消现有选择（在允许多选的情况下才有意义）
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="exclusive"></param>
        public void Select(object obj, bool exclusive)
        {
            if (_bindingList == null || _bindingList.Count == 0)
                return;

            if (exclusive && AllowMultiSelect)
                ClearSelectedItems();

            Debug.Assert(obj != null, "obj 为 null");

            if (obj == null)
                return;

            int index = _bindingList.IndexOf(obj);
            if (index < 0)
            {
                return;
            }

            _listBox.SetSelected(index, true);
        }

        public void Select<T>(Filter<T> filter) where T : class
        {
            Select<T>(filter, true);
        }

        public void Select<T>(Filter<T> filter, bool exclusive) where T : class
        {
            Debug.Assert(filter != null, "Filter 为 null");

            if (filter == null)
                return;

            if (_bindingList == null || _bindingList.Count == 0)
                return;

            if (typeof(T).Equals(_currentType) == false && _currentType.IsSubclassOf(typeof(T)) == false)
                return;

            if (exclusive && AllowMultiSelect)
                ClearSelectedItems();

            for (int i = 0; i < _bindingList.Count; i++)
            {
                object obj = _bindingList[i];

                //同上
                T tObj = obj as T;
                if (tObj != null && filter(obj as T))
                {
                    _listBox.SetSelected(i, true);

                    if (AllowMultiSelect == false)
                        break;
                }
            }
        }

        #endregion

        #endregion

        #region 事件处理

        void _listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //因为有可能是多选的，所以不在事件中传递选择的项

            if (SelectedItemChanged != null)
            {
                SelectedItemChanged(this, new EventArgs());
            }
        }

        #endregion

        #region 事件

        /// <summary>
        /// 用于查找对象的过虑器
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public delegate bool Filter<T>(T data);

        public delegate void OnSelectedItemChangedHandler(object sender, EventArgs e);
        /// <summary>
        /// 选中行发生变化，可能会同时选中了多行，亦可能取消选择了当前选中的行
        /// </summary>
        public event OnSelectedItemChangedHandler SelectedItemChanged;

        #endregion
    }

    //public class ListBoxControllerEventArgs : EventArgs
    //{
    //    /// <summary>
    //    /// 目标行所绑定的对象
    //    /// </summary>
    //    public object Data
    //    {
    //        get;
    //        private set;
    //    }

    //    public ListBoxControllerEventArgs(object data)
    //    {
    //        Data = data;
    //    }

    //    public T GetData<T>()
    //    {
    //        if (Data == null)
    //            return default(T);

    //        Type tType = typeof(T);

    //        if (Data.GetType().Equals(tType) || Data.GetType().IsSubclassOf(tType))
    //        {
    //            return (T)Data;
    //        }
    //        else
    //        {
    //            return default(T);
    //        }
    //    }
    //}

}
