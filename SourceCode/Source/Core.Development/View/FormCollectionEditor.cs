using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.Localisation;
using Sheng.SailingEase.ComponentModel.Design;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.ComponentModel.Design.UndoEngine;

namespace Sheng.SailingEase.Core.Development
{
    public partial class FormCollectionEditor: FormViewBase
    {
        #region 公开事件

        public delegate bool CollectionEditorAddEventHandler(CollectionEditorEventArgs e, out object addValue);
        public event CollectionEditorAddEventHandler OnAdd;

        //不需要专门的newValue，编辑直接发生在obj上
        public delegate bool CollectionEditorEditEventHandler(object obj, CollectionEditorEventArgs e, out object oldValue);
        public event CollectionEditorEditEventHandler OnEdit;

        /// <summary>
        /// 编辑或删除之后执行的外部动作委托
        /// </summary>
        public delegate void OnEditedEventHandler(object sender, CollectionEditEventArgs e);

        /// <summary>
        /// 编辑项之后触发事件
        /// </summary>
        public event OnEditedEventHandler OnEdited;

        public delegate void OnPropertyChangedEventHandler(object sender, PropertyChangedEventArgs e);

        /// <summary>
        /// 在属性网格中更改了属性值
        /// </summary>
        public event OnPropertyChangedEventHandler OnPropertyChanged;

        #endregion

        #region 私有成员

        /// <summary>
        /// 用于编辑的列列表
        /// 使得点击"取消按钮"可以取消对数据列的编辑
        /// </summary>
        BindingSource _bindingSource = new BindingSource();

        private CollectionBase _collection;

        #endregion

        #region 公开属性

        public string Title
        {
            get { return this.Text; }
            set { this.Text = value; }
        }

        private bool _supportCancel = false;
        /// <summary>
        /// 是否支持撤销
        /// 如果不支持撤销，也就不支持封装可撤销工作单元
        /// </summary>
        public bool SupportCancel
        {
            get { return this._supportCancel; }
            private set
            {
                this._supportCancel = value;

                if (value)
                {
                    if (_undoTransaction == null)
                        _undoTransaction = new SEUndoTransaction();
                }
                else
                {
                    _undoTransaction = null;
                }

                this.btnCancel.Enabled = value;
            }
        }

        #endregion

        #region 窗体构造及事件

        public FormCollectionEditor(CollectionBase collection)
            : this(collection, false)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="supportCancel">是否需要支持撤销，重做</param>
        public FormCollectionEditor(CollectionBase collection,bool supportCancel)
        {
            InitializeComponent();

            Unity.ApplyResource(this);

            UIHelper.ProcessDataGridView(this.dataGridView);
            this.dataGridView.AutoGenerateColumns = false;

            ApplyIconResource();

            _collection = collection;

            this.SupportCancel = supportCancel;
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormCollectionEdit_Load(object sender, EventArgs e)
        {
            #region 此处代码原为支持点击取消按钮，取消对集合的编辑，现在改为用撤销/重做引擎实现

            ////把集合中的元素复制到List中以供编辑
            //foreach (object obj in _collection)
            //{
            //    ICloneable clone = obj as ICloneable;

            //    Debug.Assert(clone != null, "");

            //    if (clone != null)
            //        _editCollection.Add(clone.Clone());

            //}

            #endregion

            this._bindingSource.DataSource = this._collection;

            this.dataGridView.DataSource = this._bindingSource;

            //绑定事件
            this.propertyGrid.PropertyChanged += new PropertyGridPad.OnPropertyChangeHandler(propertyGrid_PropertyChanged);
            this.propertyGrid.ObjectPropertyValueChanged += new PropertyGridPad.OnObjectPropertyValueChangedHandler(propertyGrid_ObjectPropertyValueChanged);
        }

        private void FormCollectionEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK)
            {
                if (this.SupportCancel)
                {
                    Undo();
                }
            }
        }

        #endregion

        #region 应用资源

        /// <summary>
        /// 应用图标资源
        /// </summary>
        private void ApplyIconResource()
        {
            this.toolStripButtonAdd.Image = IconsLibrary.New2;
            this.toolStripButtonEdit.Image = IconsLibrary.Edit;
            this.toolStripButtonDelete.Image = IconsLibrary.Delete;

            this.toolStripButtonUp.Image = IconsLibrary.Up;
            this.toolStripButtonDown.Image = IconsLibrary.Down;

        }

        #endregion

        #region 数据列列表事件

        private void dataGridViewDataColumn_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            //如果选中了多行，屏蔽编辑按钮
            //已屏蔽了多选
            if (this.dataGridView.SelectedRows.Count > 1)
            {
                this.toolStripButtonEdit.Enabled = false;
                this.toolStripButtonUp.Enabled = false;
                this.toolStripButtonDown.Enabled = false;
            }
            else
            {
                this.toolStripButtonEdit.Enabled = true;
                this.toolStripButtonUp.Enabled = true;
                this.toolStripButtonDown.Enabled = true;
            }

            if (this.dataGridView.SelectedRows.Count == 0)
            {
                this.propertyGrid.SelectedObjects = null;
                return;
            }

            this.propertyGrid.SelectedObjects = new object[] { this.dataGridView.SelectedRows[0].DataBoundItem };
        }

        private void dataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            CallOnEditButtonClickEvent();
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 点击确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            #region 此处代码原为支持点击取消按钮，取消对集合的编辑，现在改为用撤销/重做引擎实现

            ////将编辑集合拷贝到原集合引用
            //this._collection.Clear();
            //foreach (object obj in this._editCollection)
            //{
            //    ((IList)_collection).Add(obj);
            //}

            #endregion

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void propertyGrid_PropertyChanged(object sender, PropertyChangeEventArgs e)
        {
            _bindingSource.ResetBindings(false);
        }

        private void propertyGrid_ObjectPropertyValueChanged(object sender, ObjectPropertyValueChangedEventArgs e)
        {
            if (this.SupportCancel)
            {
                //封装可撤销的工作单元
                SEUndoUnitStandard unit = new SEUndoUnitStandard();
                unit.Value = e.TargetObject;
                unit.Members.Add(e.Property, e.OldValue, e.NewValue);
                this.AddUndoUnit(unit);
            }

            if (this.OnPropertyChanged != null)
            {
                OnPropertyChanged(this, new PropertyChangedEventArgs(e.TargetObject, e.Property, e.NewValue, e.OldValue));
            }
        }

        #endregion

        #region 工具栏按钮事件

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            if (this.OnAdd != null)
            {
                object objAdded;
                if (OnAdd(new CollectionEditorEventArgs(_bindingSource), out objAdded))
                {
                    if (objAdded != null)
                    {
                        this._bindingSource.Add(objAdded);
                    }

                    CollectionEditEventArgs collectionEditEventArgs =
                        new CollectionEditEventArgs(this._bindingSource, CollectionEditType.Add, this._bindingSource.IndexOf(objAdded), objAdded);

                    if (this.SupportCancel)
                    {
                        AddUndoUnit(collectionEditEventArgs);
                    }

                    if (this.OnEdited != null)
                    {
                        OnEdited(this, collectionEditEventArgs);
                    }
                }
            }
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
            CallOnEditButtonClickEvent();
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (this.dataGridView.SelectedRows.Count != 1)
            {
                return;
            }

            object obj = dataGridView.SelectedRows[0].DataBoundItem;

            Dictionary<int, object> values = new Dictionary<int, object>();
            values.Add(this._bindingSource.IndexOf(obj), obj);
            CollectionEditEventArgs collectionEditEventArgs = new CollectionEditEventArgs(this._bindingSource, values);

            this._bindingSource.Remove(obj);

            if (this.SupportCancel)
            {
                AddUndoUnit(collectionEditEventArgs);
            }

            if (this.OnEdited != null)
            {
                OnEdited(this, collectionEditEventArgs);
            }
        }

        private void toolStripButtonUp_Click(object sender, EventArgs e)
        {
            if (this.dataGridView.SelectedRows.Count != 1)
            {
                return;
            }

            object obj = dataGridView.SelectedRows[0].DataBoundItem;

            int index = _bindingSource.IndexOf(obj) - 1;
            if (index < 0)
                return;

            int currentColumn = dataGridView.CurrentCell.ColumnIndex;

            _bindingSource.RaiseListChangedEvents = false;

            _bindingSource.Remove(obj);
            _bindingSource.Insert(index, obj);

            _bindingSource.RaiseListChangedEvents = true;
            _bindingSource.ResetBindings(false);

            this.dataGridView.Rows[index].Selected = true;
            this.dataGridView.CurrentCell = dataGridView[currentColumn, index];

            if (this.OnEdited != null)
            {
                OnEdited(this, new CollectionEditEventArgs(this._bindingSource, CollectionEditType.Move, index, index + 1, obj));
            }
        }

        private void toolStripButtonDown_Click(object sender, EventArgs e)
        {
            if (this.dataGridView.SelectedRows.Count != 1)
            {
                return;
            }

            object obj = dataGridView.SelectedRows[0].DataBoundItem;

            int index = _bindingSource.IndexOf(obj) + 1;
            if (index >= _bindingSource.Count)
                return;

            int currentColumn = dataGridView.CurrentCell.ColumnIndex;

            _bindingSource.RaiseListChangedEvents = false;

            _bindingSource.Remove(obj);
            _bindingSource.Insert(index, obj);

            _bindingSource.RaiseListChangedEvents = true;
            _bindingSource.ResetBindings(false);

            this.dataGridView.Rows[index].Selected = true;
            this.dataGridView.CurrentCell = dataGridView[currentColumn, index];

            if (this.OnEdited != null)
            {
                OnEdited(this, new CollectionEditEventArgs(this._bindingSource, CollectionEditType.Move, index, index - 1, obj));
            }
        }

        #endregion

        #region 私有方法

        private void CallOnEditButtonClickEvent()
        {
            if (this.dataGridView.SelectedRows.Count != 1)
            {
                return;
            }

            if (this.OnEdit != null)
            {
                object value = dataGridView.SelectedRows[0].DataBoundItem;
                object oldValue;

                if (OnEdit(value, new CollectionEditorEventArgs(_bindingSource), out oldValue))
                {
                    CollectionEditEventArgs collectionEditEventArgs = new CollectionEditEventArgs(this._bindingSource, CollectionEditType.Edit, _bindingSource.IndexOf(value),
                          value);
                    collectionEditEventArgs.Members.Add(ObjectCompare.Compare(oldValue, value));

                    if (this.SupportCancel)
                    {
                        AddUndoUnit(collectionEditEventArgs);
                    }

                    if (this.OnEdited != null)
                    {
                        OnEdited(this, collectionEditEventArgs);
                    }
                }
            }
        }

        #endregion

        #region 与撤销/重做引擎有关的属性，方法

        private SEUndoTransaction _undoTransaction;
        public SEUndoTransaction UndoTransaction
        {
            get { return this._undoTransaction; }
        }

        private void AddUndoUnit(CollectionEditEventArgs collectionEditEventArgs)
        {
            if (this.SupportCancel)
            {
                this.AddUndoUnit(new SEUndoUnitCollectionEdit(String.Empty, collectionEditEventArgs));
            }
        }

        public void AddUndoUnit(SEUndoUnitAbstract unit)
        {
            if (this._undoTransaction != null)
                this._undoTransaction.AddUndoUnit(unit);
        }

        private void Undo()
        {
            _undoTransaction.Undo();
        }

        #endregion

        #region CollectionEditorEventArgs

        public class CollectionEditorEventArgs : EventArgs
        {
            public CollectionEditorEventArgs(IList editCollection)
            {
                this.EditCollection = editCollection;
            }

            /// <summary>
            /// 当前正在编辑的集合
            /// </summary>
            public IList EditCollection
            {
                get;
                set;
            }
        }

        #endregion

        #region PropertyChangedEventArgs

        /// <summary>
        /// 为在属性网络中更改属性提供事件参数
        /// </summary>
        public class PropertyChangedEventArgs : EventArgs
        {
            private object _object;
            public object Object
            {
                get { return this._object; }
                private set { this._object = value; }
            }

            private string _property;
            public string Property
            {
                get { return this._property; }
                private set { this._property = value; }
            }

            private object _oldValue;
            public object OldValue
            {
                get { return this._oldValue; }
                private set { this._oldValue = value; }
            }

            private object _newValue;
            public object NewValue
            {
                get { return this._newValue; }
                private set { this._newValue = value; }
            }

            public PropertyChangedEventArgs(object obj,string property,object newValue,object oldValue)
            {
                this.Object = obj;
                this.Property = property;
                this.NewValue = newValue;
                this.OldValue = oldValue;
            }
        }

        #endregion
    }
}
