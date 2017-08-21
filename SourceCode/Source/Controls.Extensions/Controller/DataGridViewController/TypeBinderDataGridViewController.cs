using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;
using Sheng.SailingEase.Kernal;

namespace Sheng.SailingEase.Controls.Extensions
{
   

    /// <summary>
    /// 支持按类型绑定的 DataGridView 控制器
    /// </summary>
    public class TypeBinderDataGridViewController
    {
        #region 私有成员

        /// <summary>
        /// DataGridView
        /// </summary>
        private DataGridView _dataGridView;

        /// <summary>
        /// 当前列集合
        /// 用于判断是否需要重新加载列
        /// </summary>
        private List<DataGridViewColumn> _currentColumns;

        /// <summary>
        /// 所绑定的数据集
        /// 用于没有指定泛型参数的默认 codon
        /// </summary>
        //private IBindingListEx _bindingList;

        /// <summary>
        /// 用于关联codon和指定了泛型参数的专用bindingList
        /// </summary>
        private Dictionary<ITypeBinderDataGridViewTypeCodon, IBindingListEx> _bindingListPool =
            new Dictionary<ITypeBinderDataGridViewTypeCodon, IBindingListEx>();

        private ITypeBinderDataGridViewTypeCodon _currentCodon;

        private List<ITypeBinderDataGridViewTypeCodon> _typeBinderDataGridViewTypeCodons =
            new List<ITypeBinderDataGridViewTypeCodon>();

        /// <summary>
        /// 当前右键菜单
        /// </summary>
        private ContextMenuStrip ContextMenuStrip
        {
            get { return this._dataGridView.ContextMenuStrip; }
            set { this._dataGridView.ContextMenuStrip = value; }
        }

        /// <summary>
        /// 上下文对象
        /// 在某些情况下，需要知道当前显示的数据列表的相关对象，如这些数据的父对象
        /// 但是如果使用了对象下钻，则上级对象可通过属性获得
        /// </summary>
        private object _contextData;

        #endregion

        #region 公开属性

        private bool _goingDown = false;
        /// <summary>
        /// 是否自动下钻（双击时）
        /// 默认 false
        /// </summary>
        public bool GoingDown
        {
            get { return _goingDown; }
            set { _goingDown = value; }
        }

        /// <summary>
        /// 当前选中的行数
        /// </summary>
        public int SelectedItemsCount
        {
            get { return _dataGridView.SelectedRows.Count; }
        }

        /// <summary>
        /// DataGridView中选中的行的下标
        /// 如果没有选中任何行，返回-1，如果选中多行,返回其中第一行索引
        /// </summary>
        public int SelectedItemIndex
        {
            get
            {
                if (_dataGridView.SelectedRows.Count == 0)
                    return -1;
                else
                    return _dataGridView.SelectedRows[0].Index;
            }
        }

        /// <summary>
        /// _dataGridView中的总行数
        /// </summary>
        public int ItemsCount
        {
            get { return _dataGridView.Rows.Count; }
        }

        /// <summary>
        /// 当前所绑定的数据对应的类型（当前Codon中的DataBoundType）
        /// 实际是从Codon是取的，可能是所绑定的数据的基类类型
        /// </summary>
        public Type CurrentType
        {
            get
            {
                if (_currentCodon == null)
                    return null;

                return _currentCodon.DataBoundType;
            }
        }

        #endregion

        #region 构造

        public TypeBinderDataGridViewController(DataGridView dataGridView)
        {
            //RowLeave 和 RowEnter 事件的问题在于他们都发生在相应的动作之前……
            //这就造成如果向外抛出这个事件，外部代码取当前行，实际上不是最新选择的行

            _dataGridView = dataGridView;
            _dataGridView.AutoGenerateColumns = false;
            _dataGridView.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(_dataGridView_CellMouseDoubleClick);
            _dataGridView.RowStateChanged += new DataGridViewRowStateChangedEventHandler(_dataGridView_RowStateChanged);
            _dataGridView.SelectionChanged += new EventHandler(_dataGridView_SelectionChanged);

            _dataGridView.DataBindingComplete += (sender, e) =>
            {
                if (_currentCodon != null)
                {
                    this.ContextMenuStrip = _currentCodon.ContextMenuStrip;
                }
                else
                {
                    this.ContextMenuStrip = null;
                }
            };
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取与指定类型兼容的Codon
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private ITypeBinderDataGridViewTypeCodon GetCodon(Type type)
        {
            if (type == null)
                return null;

            foreach (var code in _typeBinderDataGridViewTypeCodons)
            {
                if (code.DataBoundType == null)
                    continue;

                if (code.Compatible(type))
                    return code;
            }

            return null;
        }

        /// <summary>
        /// 根据指定的 codon 获取 BindingList
        /// 如果有与之关联的特定 BindingList，就返回，如果没有，就返回默认 BindingList
        /// </summary>
        /// <param name="codon"></param>
        /// <returns></returns>
        private IBindingListEx GetBindingList(ITypeBinderDataGridViewTypeCodon codon)
        {
            Debug.Assert(codon != null, "codon 为 null");

            if (codon == null)
                return null;

            Debug.Assert(_bindingListPool.Keys.Contains(codon), "_bindingListPool 中没有指定的 codon 键");

            return _bindingListPool[codon];
        }

        /// <summary>
        /// 判断指定的 codon 是否使用特定的 BindingList 
        /// 如果是，返回 true，如果使用的是默认 BindingList ，返回 false
        /// </summary>
        /// <param name="codon"></param>
        /// <returns></returns>
        private bool IsSpecialBindingList(ITypeBinderDataGridViewTypeCodon codon)
        {
            return _bindingListPool.Keys.Contains(codon);
        }

        /// <summary>
        /// 将指定的 bindingList 分配给指定的 codon
        /// 如果之前没有为指定的 codon 建立专用的 BindingList,则在 _bindingListPool 建立之
        /// </summary>
        /// <param name="codon"></param>
        /// <param name="bindingList"></param>
        private void AllotBindingList(ITypeBinderDataGridViewTypeCodon codon, IBindingListEx bindingList)
        {
            Debug.Assert(codon != null && bindingList != null, "codon 或 bindingList 为 null");

            if (_bindingListPool.Keys.Contains(codon))
            {
                _bindingListPool[codon] = bindingList;
            }
            else
            {
                _bindingListPool.Add(codon, bindingList);
            }
        }

        /// <summary>
        /// 加载指定的列集合
        /// </summary>
        /// <param name="columns"></param>
        private void LoadColumns(List<DataGridViewColumn> columns)
        {
            if (_currentColumns == columns)
                return;

            _currentColumns = columns;

            _dataGridView.Columns.Clear();

            foreach (DataGridViewColumn col in columns)
            {
                _dataGridView.Columns.Add(col);
            }
        }

        /// <summary>
        /// 获取当前选定对象的下钻对象集合
        /// </summary>
        /// <returns></returns>
        private IList GetGoingDownItems()
        {
            if (_currentCodon == null || SelectedItemsCount != 1)
                return null;

            object selectedObject = GetSelectedItem();

            if (String.IsNullOrEmpty(_currentCodon.ItemsMember) || selectedObject == null)
                return null;

            object listObject = ReflectionPool.GetPropertyValue(selectedObject, _currentCodon.ItemsMember);
            if (listObject == null)
                return null;

            return listObject as IList;
        }

        private void ClearSelectedRows()
        {
            DataGridViewSelectedRowCollection rows = _dataGridView.SelectedRows;
            foreach (DataGridViewRow item in rows)
            {
                item.Selected = false;
            }
        }

        #endregion

        #region 公开方法

        #region Codon

        public void AddCodon(ITypeBinderDataGridViewTypeCodon codon)
        {
            if (codon == null)
            {
                Debug.Assert(false, "codon 为 null");
                throw new ArgumentNullException();
            }

            if (_typeBinderDataGridViewTypeCodons.Contains(codon))
            {
                Debug.Assert(false, "_typeBinderDataGridViewTypeCodons 重复添加:" + codon.ToString());
                throw new ArgumentException();
            }

            Debug.Assert(GetCodon(codon.DataBoundType) == null,
                "_typeBinderDataGridViewTypeCodons 重复添加类型:" + codon.ToString());

            _typeBinderDataGridViewTypeCodons.Add(codon);

            IBindingListEx bindingList = null;
            _bindingListPool.Add(codon, bindingList);
        }

        /// <summary>
        /// 判断当前Codon能否与指定的对象的类型相兼容
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Compatible(object obj)
        {
            if (_currentCodon == null)
            {
                Debug.Assert(false, "_currentCodon 为 null");
                return false;
            }

            return _currentCodon.Compatible(obj);
        }

        #endregion

        #region Clear

        /// <summary>
        /// 清除
        /// 包括列、数据源
        /// </summary>
        public void Clear()
        {
            ClearData();

            _currentCodon = null;
            _currentColumns = null;
            _dataGridView.Columns.Clear();

            ContextMenuStrip = null;
        }

        /// <summary>
        /// 清除列，数据源
        /// </summary>
        public void ClearData()
        {
            _contextData = null;
            _dataGridView.DataSource = null;

            if (_currentCodon != null)
            {
                IBindingListEx bindingList = GetBindingList(_currentCodon);
                bindingList = null;
            }
        }

        #endregion

        #region DataBind

        public void DataBind<T>(IList list)
        {
            DataBind(list, typeof(T), null);
        }

        public void DataBind<T>(IList list, object contextData)
        {
            DataBind(list, typeof(T), contextData);
        }

        //这里为什么不用泛型方法
        //因为类型可能是由外部变量在运行时决定的，如 Show<e.Type>() ，而实际上这样的写法是不行的
        //泛型参数必须在编译之前静态指定
        /// <summary>
        /// 如果 contextData 为 null，不会把当前上下文对象置为null，而是保持当前上下文对象的值
        /// </summary>
        /// <param name="list"></param>
        /// <param name="type"></param>
        /// <param name="contextData"></param>
        public void DataBind(IList list, Type type, object contextData)
        {
            //这个地方是没有办法通过 list 取到对象类型的
            //集合内可能没有数据，集合本身也无法说明内部数据类型
            _currentCodon = GetCodon(type);

            if (_currentCodon == null)
            {
                Clear();
                return;
            }

            IBindingListEx bindingList = GetBindingList(_currentCodon);

            LoadColumns(_currentCodon.Columns);
            this.ContextMenuStrip = _currentCodon.ContextMenuStrip;

            _dataGridView.DataSource = null;

            //因为BindingList需要通过内部元素做类型推导，以便绑定数据呈现数据，
            //只是通过泛型参数object，是不可能获取指定Property的值的
            //所以如果list中没有任何元素，就要把对BindingList的初始化和绑定，推迟到添加元素时（否则单元格无法呈现数据）
            //这种情况主要是针对使用默认 BindingList 的，即 BindingList<object>
            if (list != null && list.Count > 0)
            {
                //_bindingList = new BindingListEx<object>(list.Cast<object>().ToList());
                bindingList = _currentCodon.InitializeBindingList(list);
                if (IsSpecialBindingList(_currentCodon))
                {
                    AllotBindingList(_currentCodon, bindingList);
                }

                _dataGridView.DataSource = bindingList;
            }
            else
            {
                bindingList = null;
                _dataGridView.DataSource = null;
            }

            if (contextData != null)
                _contextData = contextData;
        }

        #endregion

        #region Add,Update,Remove

        public void Add(object obj)
        {
            Debug.Assert(obj != null, "obj为null");

            if (obj == null)
                return;

            if (_currentCodon == null)
                return;

            IBindingListEx bindingList = GetBindingList(_currentCodon);

            if (bindingList != null && bindingList.Contains(obj))
            {
                Debug.Assert(false, "_bindingList中已经存在了指定对象");
                return;
            }

            if (_currentCodon.Compatible(obj))
            {
                //如果调用了Clear之后没有Show新的数据，那么_bindingList是为null的
                //如果调用DataBind时list中没有元素，也不会初始化_bindingList，因为BindingList需要用内部元素作类型推导
                if (bindingList == null)
                {
                    bindingList = _currentCodon.InitializeBindingList();
                    if (IsSpecialBindingList(_currentCodon))
                    {
                        AllotBindingList(_currentCodon, bindingList);
                    }
                }

                bindingList.Add(obj);

                if (_dataGridView.DataSource == null)
                {
                    _dataGridView.DataSource = bindingList;
                }
            }
        }

        public void Update(object obj)
        {
            Debug.Assert(obj != null, "obj为null");

            if (obj == null)
                return;

            if ( _currentCodon == null)
                return;

            IBindingListEx bindingList = GetBindingList(_currentCodon);

            if (bindingList == null)
                return;

            //Debug.Assert(bindingList.Contains(obj), "_bindingList中没有指定对象");

            if (bindingList.Contains(obj) == false)
                return;

            if (_currentCodon.Compatible(obj))
            {
                bindingList.ResetItem(bindingList.IndexOf(obj));
            }
        }

        public void Update<T>(Filter<T> filter) where T : class
        {
            Debug.Assert(filter != null, "Filter 为 null");

            if (filter == null)
                return;

            if (_currentCodon == null)
                return;

            IBindingListEx bindingList = GetBindingList(_currentCodon);

            if (bindingList == null || bindingList.Count == 0)
                return;

            Type tType = typeof(T);

            if (_currentCodon.UpwardCompatible(tType) == false &&
                _currentCodon.Compatible(tType) == false)
                return;

            for (int i = 0; i < bindingList.Count; i++)
            {
                object obj = bindingList[i];

                if (Compatible(tType, obj.GetType()) && filter((T)obj))
                {
                    bindingList.ResetItem(i);
                }
            }
        }

        public void Remove(object obj)
        {
            Debug.Assert(obj != null, "obj为null");

            if (obj == null)
                return;

            if (_currentCodon == null)
                return;

            IBindingListEx bindingList = GetBindingList(_currentCodon);

            if (bindingList == null)
                return;

            Debug.Assert(bindingList.Contains(obj), "_bindingList中没有指定对象");

            if (bindingList.Contains(obj) == false)
                return;

            if (_currentCodon.Compatible(obj))
            {
                bindingList.Remove(obj);
            }
        }

        public void Remove<T>(Filter<T> filter) where T : class
        {
            Debug.Assert(filter != null, "Filter 为 null");

            if (filter == null)
                return;

            if (_currentCodon == null)
                return;

            IBindingListEx bindingList = GetBindingList(_currentCodon);

            if (bindingList == null || bindingList.Count == 0)
                return;

            Type tType = typeof(T);

            if (_currentCodon.UpwardCompatible(tType) == false &&
                _currentCodon.Compatible(tType) == false)
                return;

            List<object> willbeDelete = new List<object>();
            for (int i = 0; i < bindingList.Count; i++)
            {
                object obj = bindingList[i];

                //在使用复合 codon 时，BindingList 中会存在多种不同类型的对象
                //所以必须先判断对象是否是泛型参数类型的向下兼容类型
                if (Compatible(tType,obj.GetType()) && filter((T)obj))
                {
                    willbeDelete.Add(obj);
                }
            }

            foreach (var item in willbeDelete)
            {
                bindingList.Remove(item);
            }
        }

        #endregion

        #region Move

        //MoveBefore和MoveAfter方法共用，用于获取要移动的对象
        //该方法的作用实际上就是用两个过虑器委托分别取出两个对应的对象
        private void GetMoveObject<T>(Filter<T> sourceFilter, Filter<T> targetFilter,
            out object sourceObj, out object targetObj) where T : class
        {
            //如果不加 where T : class，不能用 obj as T

            sourceObj = null;
            targetObj = null;

            Debug.Assert(sourceFilter != null && targetFilter != null, "Filter 为 null");

            if (sourceFilter == null || targetFilter == null)
                return;

            if (_currentCodon == null)
                return;

            IBindingListEx bindingList = GetBindingList(_currentCodon);

            if (bindingList == null || bindingList.Count == 0)
                return;

            Type tType = typeof(T);

            if (_currentCodon.UpwardCompatible(tType) == false &&
                _currentCodon.Compatible(tType) == false)
                return;

            foreach (object obj in bindingList)
            {
                if (sourceObj != null && targetObj != null)
                    break;

                if (sourceObj == null && sourceFilter(obj as T))
                {
                    sourceObj = obj;
                }

                if (targetObj == null && targetFilter(obj as T))
                {
                    targetObj = obj;
                }
            }
        }

        /// <summary>
        /// 把目标对象移动到另一个对象之前
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceFilter"></param>
        /// <param name="beforeFilter"></param>
        public void MoveBefore<T>(Filter<T> sourceFilter, Filter<T> targetFilter) where T : class
        {
            object sourceObj, targetObj;

            GetMoveObject<T>(sourceFilter, targetFilter, out sourceObj, out targetObj);

            //Debug.Assert(sourceObj != null && targetObj != null, "指定的对象为null");

            if (sourceObj == null && targetObj == null)
                return;

            Debug.Assert(sourceObj != targetObj, "指定的两个对象是同一个对象");

            if (sourceObj == targetObj)
                return;

            if (_currentCodon == null)
                return;

            IBindingListEx bindingList = GetBindingList(_currentCodon);

            int sourceIndex = bindingList.IndexOf(sourceObj);
            int targetIndex = bindingList.IndexOf(targetObj);

            //如果targetObj刚好在beforeObj之前，就无需移动了
            if (targetIndex - sourceIndex == 1)
                return;

            bindingList.Remove(sourceObj);
            //要重新 IndexOf(beforeObj) ，因为targetObj被remove之后，beforeObj的下标就可能被改变了
            bindingList.Insert(bindingList.IndexOf(targetObj), sourceObj);
        }

        /// <summary>
        /// 把目标对象移动到另一个对象之后
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceFilter"></param>
        /// <param name="targetFilter"></param>
        public void MoveAfter<T>(Filter<T> sourceFilter, Filter<T> targetFilter) where T : class
        {
            object sourceObj, targetObj;

            GetMoveObject<T>(sourceFilter, targetFilter, out sourceObj, out targetObj);

            //Debug.Assert(sourceObj != null && targetObj != null, "指定的对象为null");

            if (sourceObj == null && targetObj == null)
                return;

            Debug.Assert(sourceObj != targetObj, "指定的两个对象是同一个对象");

            if (sourceObj == targetObj)
                return;

            if (_currentCodon == null)
                return;

            IBindingListEx bindingList = GetBindingList(_currentCodon);

            int sourceIndex = bindingList.IndexOf(sourceObj);
            int targetIndex = bindingList.IndexOf(targetObj);

            //如果targetObj刚好在beforeObj之后，就无需移动了
            if (sourceIndex - targetIndex == 1)
                return;

            bindingList.Remove(sourceObj);
            //要重新 IndexOf(beforeObj) ，因为targetObj被remove之后，beforeObj的下标就可能被改变了
            bindingList.Insert(bindingList.IndexOf(targetObj) + 1, sourceObj);
        }

        #endregion

        #region GetItem(s)...

        /// <summary>
        /// 获取当前选定的对象
        /// 如果没有选定行，返回null，如果选了多行，返回选定行中第一行的绑定对象
        /// </summary>
        /// <returns></returns>
        public object GetSelectedItem()
        {
            return GetSelectedItem<object>();
        }

      

        /// <summary>
        /// 判断 type 是否是 targetType 是同一个类型或其子类型
        /// 即判断 targetType 是否 兼容 type
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool Compatible(Type targetType, Type type)
        {
            bool compatible = false;

            if (targetType.IsClass)
            {
                compatible = type.Equals(targetType) || type.IsSubclassOf(targetType);
            }
            else if (targetType.IsInterface)
            {
                compatible = type == targetType || type.GetInterface(targetType.ToString()) != null;
            }

            return compatible;
        }

        /// <summary>
        /// 获取当前选定的对象
        /// 如果没有选定行，返回null，如果选了多行，返回选定行中第一行的绑定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetSelectedItem<T>()
        {
            if (_dataGridView.SelectedRows.Count == 0)
                return default(T);

            Type tType = typeof(T);

            if (_currentCodon.UpwardCompatible(tType) == false &&
                _currentCodon.Compatible(tType) == false)
                return default(T);

            object obj = _dataGridView.SelectedRows[0].DataBoundItem;

            Type objType = obj.GetType();

            if (Compatible(tType, objType) == false)
                return default(T);

            return (T)obj;
        }

        /// <summary>
        /// 获取指定索引行处的绑定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public T GetItem<T>(int rowIndex)
        {
            if (rowIndex < 0)
                return default(T);

            Type tType = typeof(T);

            if (_currentCodon.UpwardCompatible(tType) == false &&
                _currentCodon.Compatible(tType) == false)
                return default(T);

            object obj = _dataGridView.Rows[rowIndex].DataBoundItem;

            Type objType = obj.GetType();

            if (Compatible(tType, objType) == false)
                return default(T);

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

            if (_dataGridView.SelectedRows.Count == 0)
                return list;

            Type tType = typeof(T);

            if (_currentCodon.UpwardCompatible(tType) == false &&
                _currentCodon.Compatible(tType) == false)
                return list;

            foreach (DataGridViewRow item in _dataGridView.SelectedRows)
            {
                list.Add((T)item.DataBoundItem);
            }

            return list;
        }

        public T Find<T>(Filter<T> filter) where T : class
        {
            if (_currentCodon == null)
                return null;

            IBindingListEx bindingList = GetBindingList(_currentCodon);

            if (bindingList == null || bindingList.Count == 0)
                return null;

            Type tType = typeof(T);

            if (_currentCodon.UpwardCompatible(tType) == false &&
                _currentCodon.Compatible(tType) == false)
                return null;

            foreach (object obj in bindingList)
            {
                if (Compatible(tType, obj.GetType()) && filter((T)obj))
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
        /// exclusive 用于指定是否是排它性选择，既取消现有选择
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="exclusive"></param>
        public void Select(object obj, bool exclusive)
        {
            Debug.Assert(obj != null, "obj 为 null");

            if (_currentCodon == null)
                return;

            IBindingListEx bindingList = GetBindingList(_currentCodon);

            if (bindingList == null || bindingList.Count == 0)
                return;

            if (exclusive)
                ClearSelectedRows();

            if (obj == null)
            {
                return;
            }

            int index = bindingList.IndexOf(obj);
            if (index < 0)
            {
                return;
            }

            _dataGridView.Rows[index].Selected = true;
        }

        public void Select<T>(Filter<T> filter) where T : class
        {
            Select<T>(filter, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <param name="exclusive">是否清除之前选定项的选定状态</param>
        public void Select<T>(Filter<T> filter, bool exclusive) where T : class
        {
            Debug.Assert(filter != null, "Filter 为 null");

            if (filter == null)
                return;

            if (_currentCodon == null)
                return;

            IBindingListEx bindingList = GetBindingList(_currentCodon);

            if (bindingList == null || bindingList.Count == 0)
                return;

            Type tType = typeof(T);

            if (_currentCodon.UpwardCompatible(tType) == false &&
                _currentCodon.Compatible(tType) == false)
                return;

            if (exclusive)
                ClearSelectedRows();

            for (int i = 0; i < bindingList.Count; i++)
            {
                object obj = bindingList[i];
                if (obj == null)
                    continue;

                if (Compatible(tType, obj.GetType()) && filter((T)obj))
                {
                    _dataGridView.Rows[i].Selected = true;
                }
            }
        }

        #endregion

        #region ContextData

        /// <summary>
        /// 设置上下文对象
        /// </summary>
        /// <param name="contextData"></param>
        public void SetContextData(object contextData)
        {
            _contextData = contextData;
        }

        /// <summary>
        /// 获取上下文对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetContextData<T>()
        {
            if (_contextData == null)
                return default(T);

            if (_contextData.GetType().Equals(typeof(T)) == false && 
                _contextData.GetType().IsSubclassOf(typeof(T)) == false)
                return default(T);

            return (T)_contextData;
        }

        #endregion

        #endregion

        #region 事件处理

        private void _dataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (DoubleClick != null)
            {
                GridViewControllerEventArgs args = new GridViewControllerEventArgs(
                    this.GetSelectedItem<object>(), _currentCodon.DataBoundType);
                DoubleClick(this, args);
            }

            //下钻
            if (GoingDown)
            {
                IList goingDownList = GetGoingDownItems();
                if (goingDownList != null)
                    DataBind(goingDownList, _currentCodon.ItemType, GetSelectedItem());
            }
        }

        private void _dataGridView_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            //这里的 DataGridViewElementStates.Selected 表示发生变更的状态是 Selected ，是否选中的状态发生了变化
            //而不是表示 由没选中变为被选中 
            if (e.StateChanged == DataGridViewElementStates.Selected)
            {
                if (e.Row.Selected && ItemSelected != null)
                {
                    //因为有可能会选中多行，所以不能用 GetSelectedItem
                    //多选时GetSelectedItem只能拿一选中行中的第一行的绑定对象
                    GridViewControllerEventArgs args = new GridViewControllerEventArgs(
                        this.GetItem<object>(e.Row.Index),_currentCodon.DataBoundType);
                    ItemSelected(this, args);
                }

                if (e.Row.Selected == false && ItemUnSelected != null)
                {
                    GridViewControllerEventArgs args = new GridViewControllerEventArgs(
                        this.GetItem<object>(e.Row.Index), _currentCodon.DataBoundType);
                    ItemUnSelected(this, args);
                }
            }

            //如果是删除选中的行，不会触发 e.StateChanged == DataGridViewElementStates.Selected
            //而是会走到
            //e.StateChanged == DataGridViewElementStates.Displayed && e.Row.Index == -1
            //所以无法在删除选中行时触发 ItemUnSelected 事件
            //如果通过判断 e..StateChanged == DataGridViewElementStates.Displayed && e.Row.Index == -1
            //来实现，也不合理，因为删除行并不一定是手动删除的，可能是代码删除的，被删除的行之前就不是选中状态
        }

        private void _dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("_dataGridView_RowLeave : " + this._dataGridView.Name);

            if (SelectedItemChanged != null)
            {
                SelectedItemChanged(this, e);
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

        public delegate void OnDoubleClickHandler(object sender, GridViewControllerEventArgs e);
        /// <summary>
        /// 双击数据列表中的行
        /// </summary>
        public event OnDoubleClickHandler DoubleClick;

        public delegate void OnItemSelectedHandler(object sender, GridViewControllerEventArgs e);
        /// <summary>
        /// 选中某行
        /// 因为可能会多选，所以叫 ItemSelected ，不叫 SelectedItemChanged
        /// </summary>
        public event OnItemSelectedHandler ItemSelected;

        public delegate void OnItemUnSelectedHandler(object sender, GridViewControllerEventArgs e);
        /// <summary>
        /// 取消选中某行
        /// </summary>
        public event OnItemUnSelectedHandler ItemUnSelected;

        public delegate void OnSelectedItemChangedHandler(object sender, EventArgs e);
        /// <summary>
        /// 选中行发生变化，可能会同时选中了多行，亦可能取消选择了当前选中的行
        /// </summary>
        public event OnSelectedItemChangedHandler SelectedItemChanged;

        #endregion
    }

    public class GridViewControllerEventArgs : EventArgs
    {
        /// <summary>
        /// 目标行所绑定的对象
        /// </summary>
        public object Data
        {
            get;
            private set;
        }

        /// <summary>
        /// 所对应的绑定数据类型
        /// </summary>
        public Type DataBoundType
        {
            get;
            private set;
        }

        public GridViewControllerEventArgs(object data, Type dataBoundType)
        {
            Data = data;
            DataBoundType = dataBoundType;
        }

        public T GetData<T>()
        {
            if (Data == null)
                return default(T);

            if (Data.GetType().Equals(typeof(T)) || Data.GetType().IsSubclassOf(typeof(T)))
            {
                return (T)Data;
            }
            else
            {
                return default(T); 
            }
        }
    }
}
