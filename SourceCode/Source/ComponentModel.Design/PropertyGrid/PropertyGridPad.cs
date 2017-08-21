using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

using Sheng.SailingEase.Controls;
using Sheng.SailingEase.Localisation;

namespace Sheng.SailingEase.ComponentModel.Design
{
    /// <summary>
    /// 属性网格控件
    /// 主要用于处理 FormElement 对象
    /// </summary>
    public partial class PropertyGridPad : UserControl
    {
        #region 私有成员

        /// <summary>
        /// 行是否可共享
        /// 当新的选择与旧的选择完全类型兼容时，可行共享，即，直接使用现有属性行
        /// 在调用 SelectedObjects 的 set 时，设置此值
        /// </summary>
        private bool _rowsShared = false;

        /// <summary>
        /// 当前窗体是否(还)是活动窗体
        /// 用在DataGridView的DataError事件中,判断是否应提示一次即放弃更改
        /// </summary>
        private bool _active = false;

        /// <summary>
        /// 在调用UpdateProperty方法时，是否更新选中的对象的可视状态，以及是否触发PropertyChanged事件
        /// </summary>
        private bool _updateSelectedObject = true;

        /// <summary>
        /// 保存属性行的大分类行
        /// </summary>
        private CatalogRowCollection _catlogCollection = new CatalogRowCollection();

        /// <summary>
        /// 属性行缓存
        /// </summary>
        private PropertyGridRowCache _propertyGirdRowCache = new PropertyGridRowCache();

        #endregion

        #region 受保护属性

        //TODO:否决，通过外挂委托来实现
        private bool _inDesinger = true;
        /// <summary>
        /// 是否被应用在窗体设计中
        /// 将影响一些属性是否被显示
        /// </summary>
        internal bool InDesigner
        {
            get
            {
                return this._inDesinger;
            }
            set
            {
                this._inDesinger = value;
            }
        }

        private PropertyGridRowCollection _propertyGridRows = new PropertyGridRowCollection();
        internal PropertyGridRowCollection PropertyGridRows
        {
            get
            {
                return this._propertyGridRows;
            }
            set
            {
                this._propertyGridRows = value;
            }
        }

        private PropertyGridTypeWrapperCollection _typeWrapperCollection = new PropertyGridTypeWrapperCollection();

        private PropertyGridValidatorCollection _validatorCollection = new PropertyGridValidatorCollection();

        #endregion

        #region 公开属性

        private object[] _selectedObjects;
        /// <summary>
        /// 要在PropertyGid显示属性的(多个)目标对象(FormElement)
        /// </summary>
        public object[] SelectedObjects
        {
            get
            {
                return this._selectedObjects;
            }
            set
            {
                #region 测试对象类型兼容性

                //测试对象类型兼容性
                //判断传入的object数组是否与当前_selectedObjects类型兼容，如果类型兼容无需重新初始化属性行
                if (_selectedObjects != null && value != null)
                {
                    //注意，_selectedObjects和value都是数组
                    Type[] currentTypes = (from c in _selectedObjects select c == null ? null : c.GetType()).Distinct().ToArray();
                    Type[] valueTypes = (from c in value select c == null ? null : c.GetType()).Distinct().ToArray();
                    _rowsShared = currentTypes.SequenceEqual(valueTypes);
                }
                else
                {
                    _rowsShared = false;
                }

                #endregion

                this._selectedObjects = value;

                InitializePropertyGrid();

                if (value != null)
                {
                    this.ReadOnly = false;

                    //设置过SelectedObjects之后再设置ReadOnly属性，可确保此时ReadOnly只作用于当前对象的属性行
                    //而不是之前选择的对象的属性行
                    //只要有一个对象只读，就整个PropertyGrid只读
                    foreach (var obj in value)
                    {
                        //直接让对象中的所有属性在propertygrid中只读
                        IPropertyModel propertyModel = obj as IPropertyModel;
                        if (propertyModel != null && propertyModel.PropertyReadOnly)
                        {
                            this.ReadOnly = true;
                            break;
                        }
                    }
                }
            }
        }

        private DesignerVerbCollection _verbs;
        /// <summary>
        /// 当前可用的设计器谓词
        /// </summary>
        public DesignerVerbCollection Verbs
        {
            get
            {
                return _verbs;
            }
            set
            {
                this._verbs = value;

                InitializeVerb();
            }
        }

        /// <summary>
        /// 是否在下面显示注释
        /// </summary>
        public bool ShowDescription
        {
            get
            {
                return this.txtDescription.Visible;
            }
            set
            {
                this.toolStripButtonShowDescription.Checked = value;
                this.txtDescription.Visible = value;
            }
        }

        private bool _readOnly = false;
        /// <summary>
        /// 是否只读
        /// 优先级高于TypeWrapper及任何有关是否可编辑的属性
        /// </summary>
        public bool ReadOnly
        {
            get { return _readOnly; }
            set
            {
                _readOnly = value;

                foreach (PropertyGridRow row in _propertyGridRows)
                {
                    row.ReadOnly = value;
                }
            }
        }

        #endregion

        #region 构造

        public PropertyGridPad()
        {
            InitializeComponent();

            this.toolStripButtonShowDescription.Image = IconsLibrary.ActualSizeHS;
            this.toolStripButtonShowDescription.Checked = this.ShowDescription;

            this.propertyGridDataGridView.RowHeadersWidth = 20;
            this.propertyGridDataGridView.EditMode = DataGridViewEditMode.EditOnEnter;

            this.propertyGridDataGridView.DataError += new DataGridViewDataErrorEventHandler(dataGridViewProperty_DataError);
        }

        #endregion

        #region 事件处理

        #region 显示注释按钮

        private void toolStripButtonShowDescription_Click(object sender, EventArgs e)
        {
            this.toolStripButtonShowDescription.Checked = !this.toolStripButtonShowDescription.Checked;
            this.ShowDescription = this.toolStripButtonShowDescription.Checked;
        }

        #endregion

        #region OnEnter 和 OnLeave

        protected override void OnEnter(EventArgs e)
        {
            _active = true;
            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            _active = false;
            base.OnLeave(e);
        }

        #endregion

        #region 单击谓词

        /// <summary>
        /// 单击谓词连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabelVerb_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DesignerVerb verb = e.Link.LinkData as DesignerVerb;

            verb.Invoke();
        }

        #endregion

        #region RowEnter

        /// <summary>
        /// 选中某行时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewProperty_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            //在下方显示注释
            if (this.propertyGridDataGridView.GetDescriptionCell(e.RowIndex).Value != null)
            {
                this.txtDescription.Text = this.propertyGridDataGridView.GetDescriptionCell(e.RowIndex).Value.ToString();
            }
            else
            {
                this.txtDescription.Text = String.Empty;
            }
        }

        #endregion

        #region CellValueChanged

        private void dataGridViewProperty_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (this._updateSelectedObject == false)
                return;

            if (this.propertyGridDataGridView.SelectedObjectSeting)
                return;

            //更新子行的值是,这个事件会触发两次,并不是一次父行一次子行触发
            //而是在通知父行在更新值是,父行会自动更新其下子行,就又一次进来
            //更新子行时子行去更新父行,不会使父行进入这个事件
            //为了避免子行两次进来,增加一个valueChanging属性

            //首先判断是否是一个实现了IPropertyGirdCell接口的属性编辑单元格
            IPropertyGirdCell iPropertyGirdCell = this.propertyGridDataGridView[e.ColumnIndex, e.RowIndex] as IPropertyGirdCell;
            if (iPropertyGirdCell == null)
                return;

            PropertyGridRow propertyGirdRow = this.propertyGridDataGridView.Rows[e.RowIndex] as PropertyGridRow;

            //如果是自己引发的更新
            if (propertyGirdRow.ValueChangingBySelf)
                return;

            //如果当前行是子行,设置ValueChangingBySelf=true,表示接下来由于 UpdateSelectedObject 再次进入的这个事件
            //是自己引发的
            //保证PropertyChanged委托只调用一次
            if (propertyGirdRow.ParentRow != null)
                propertyGirdRow.ValueChangingBySelf = true;

            propertyGirdRow.ContrastDefaultValue();
            propertyGirdRow.UpdateSelectedObject();

            //调用属性已更改事件
            if (this.PropertyChanged != null)
            {
                PropertyChangeEventArgs eventArgs = new PropertyChangeEventArgs(this.SelectedObjects, propertyGirdRow);
                PropertyChanged(sender, eventArgs);

                Debug.WriteLine("PropertyChanged > CellValueChanged : " + propertyGirdRow.PropertyName);
            }

            propertyGirdRow.ValueChangingBySelf = false;
        }

        #endregion

        #region DataError

        private void dataGridViewProperty_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
            e.ThrowException = false;
            PropertyGridDataErrorView formDataError = new PropertyGridDataErrorView(this.propertyGridDataGridView[e.ColumnIndex, e.RowIndex].ErrorText);
            //Debug.Assert(String.IsNullOrEmpty(this.propertyGridDataGridView[e.ColumnIndex, e.RowIndex].ErrorText) == false, e.Exception.Message);
            if (_active)
            {
                if (formDataError.ShowDialog() == DialogResult.Cancel)
                {
                    this.propertyGridDataGridView.CancelEdit();
                }
            }
            else
            {
                this.propertyGridDataGridView.CancelEdit();
                this.FindForm().AddOwnedForm(formDataError);
                //此处不能showdialog,showdialog后,窗体会显示为当前活动窗体但是必须点击一次才能响应操作
                //类似于显示为当前活动窗体但实际上不是的感觉
                formDataError.Show();
            }

            this.propertyGridDataGridView[e.ColumnIndex, e.RowIndex].ErrorText = String.Empty;
        }

        #endregion

        #endregion

        #region 私有方法

        /// <summary>
        /// 清除现有的属性行，分类行
        /// </summary>
        private void ClearPropertyGridRows()
        {
            this.propertyGridDataGridView.Rows.Clear();

            this._catlogCollection.Clear();

            //清除行上的对象选择
            foreach (PropertyGridRow row in this.PropertyGridRows)
                row.SetSelectedObjects(null);

            this.PropertyGridRows.Clear();
        }

        /// <summary>
        /// 将目标对象上需要编辑的属性加载到网格中
        /// </summary>
        private void InitializePropertyGrid()
        {
            //临时挂起布局逻辑
            //希望可以提高性能，但没有明显作用
            propertyGridDataGridView.SuspendLayout();

            #region 如果当前选定的对象是 null （没有选定的对象），清除现有行，然后return

            if (this.SelectedObjects == null)
            {
                ClearPropertyGridRows();

                propertyGridDataGridView.ResumeLayout();
                return;
            }

            #endregion

            #region 如果类型不兼容 (_rowsShared == false)，清除现有行，重新初始化属性行

            //判断选择的对象的类型是否和之前的选择相同，如果相同，无需 InitializePropertyRow
            //但及时是类型兼容的，可共享（_rowsShared=true），也要在下面负责呈现的部分再次判断行的当前有效性
            if (_rowsShared == false)
            {
                ClearPropertyGridRows();

                foreach (object obj in this.SelectedObjects)
                {
                    if (obj == null)
                        continue;

                    //这一步会把所的可能的属性行都初始化出来，包括在多选情况下所有的行
                    //行是否会被显示，在最终呈现之前，向分类行添加时，判断其有效性
                    //因为在行没有绑定任何对象时，是无法判断其有效性的，只有在下面的SetSelectedObjects之后
                    InitializePropertyRow(obj);
                }
            }

            #endregion

            #region 向属性行上设置选定的对象

            //通知dataGrid开始设置选择的对象
            //这将使需要数据验证的单元格暂时停止数据验证，直到调用EndSelectedObject()
            this.propertyGridDataGridView.BeginSelectedObject();

            Debug.WriteLine("propertyGirdRow.SetSelectedObjects:" + this.SelectedObjects.Length.ToString());

            //向初始化完毕的属性行传入对象
            foreach (PropertyGridRow propertyGirdRow in this.PropertyGridRows)
            {
                propertyGirdRow.SetSelectedObjects(this.SelectedObjects);
            }

            this.propertyGridDataGridView.EndSelectedObject();

            #endregion

            #region 控制属性行和分类行的呈现

           

            if (_rowsShared == false)
            {
                #region 将属性行挂到分组行下并显示分组行

                /*
                 * 将属性行挂到分组行下
                 * 注意,必须在属性行都初始化完毕后再挂到DataGridView中
                 * 一些属性行需要通过是否已挂到DataGridView中来判断是否需要进行数据验证
                 */
                foreach (PropertyGridRow dr in this.PropertyGridRows)
                {
                    dr.Visible = dr.CanVisible;
                    //dr.ReadOnly = this.ReadOnly;

                    _catlogCollection.AddPropertyGirdRow(dr, dr.PropertyRelatorAttribute.Catalog);
                }

                foreach (CatalogRow collapseRow in _catlogCollection)
                {
                    this.propertyGridDataGridView.Rows.Add(collapseRow);
                }

                #endregion
            }
            else
            {
                //行共享时，重新判定所有可能行的有效性
                foreach (PropertyGridRow dr in this.PropertyGridRows)
                {
                    dr.Visible = dr.CanVisible;
                    //dr.ReadOnly = this.ReadOnly;
                }
            }

            //判断分类下是否有显示出的有效行，如果没有，将分类行隐藏
            foreach (CatalogRow collapseRow in _catlogCollection)
            {
                collapseRow.Visible = (from c in collapseRow.SubRows where c.Visible == true select c).ToArray().Length > 0;
            }

            //同步行的一些属性或初始化一些行添加到DataGridView之后才能进行的操作
            foreach (PropertyGridRow row in this.PropertyGridRows)
            {
                //ReadOnly要在这里设置，如果在上面行还没有加到DataGridView中时就设置的话
                //是不能设置属性单元格的颜色的，因为DataGridView为null时，是拿不到 this.Cells["PropertyValue"] 的
                row.ReadOnly = this.ReadOnly;

                row.ContrastDefaultValue();
            }

            #endregion

            propertyGridDataGridView.ResumeLayout();
        }

        /// <summary>
        /// 初始化对象的属性行
        /// </summary>
        /// <param name="attribute"></param>
        private void InitializePropertyRow(Object obj)
        {
            Type type = obj.GetType();

            //将类型注册到行集合

            #region 判断类型是否已经缓存过属性行，如果有，使用缓存的行，然后return

            if (_propertyGirdRowCache.IsCached(type))
            {
                //使用缓存时有一个情况必须考虑，那就是行在当前情况下（如选择的对象个数）是否还有效的问题
                foreach (PropertyGridRow row in _propertyGirdRowCache.GetPropertyGridRow(type))
                {
                    //确保清除当前缓存的行上的对象选择
                    row.SetSelectedObjects(null);
                    if (row.CanVisible)
                    {
                        PropertyGridRows.Add(row);
                    }
                }

                return;
            }

            #endregion

            #region 迭代类型的Properties

            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                PropertyRelatorAttribute propertyGirdAttribute = null;

                PropertyRelatorAttribute[] attributes = (PropertyRelatorAttribute[])propertyInfo.GetCustomAttributes(typeof(PropertyRelatorAttribute), true);
                if (attributes == null || attributes.Length == 0)
                    continue;

                propertyGirdAttribute = attributes[0];

                //至此,确定需要加载当前的属性行

                /*
                 * 原本会用一个if查找 PropertyGirdRowCollection 中是否已经有了同样(同名)的属性信息，如果有就不new新的行了，也不走到cache
                 * 这样有一个问题，如果一上来就多选了多个不同的对象，这些对象会有共有的属性(Property)
                 * 如果判断this.PropertyGridRows中有了就不new 新row，不cache这个新的row，就会造成缓存中被选中的一些类型的对象的属性行不完整
                 * 因为他们没有机会被初始化，被缓存，因为在走到他们的这些属性(propertyinfo时，其它对象同名的属性行已经被加到 this.PropertyGridRows中了
                 * 所以这里不能因为 this.PropertyGridRows中有了就不初始化不缓存
                 * this.PropertyGridRows.Add方法可以照调，方法内部会判断是否已经存在了同名的属性行
                 */
                PropertyGridRow propertyGirdRow = new PropertyGridRow(propertyGirdAttribute, propertyInfo, this);

                if (this.PropertyGridRows.Contains(propertyGirdRow.PropertyName) == false)
                {
                    this.PropertyGridRows.Add(propertyGirdRow);
                }

                _propertyGirdRowCache.Cache(new PropertyGridRowCacheItem(type, propertyGirdRow));
            }

            #endregion
        }

        /// <summary>
        /// 加载目标对象上可用的设计器谓词
        /// </summary>
        private void InitializeVerb()
        {
            if (this.Verbs == null || this.Verbs.Count == 0)
            {
                this.panelVerb.Visible = false;
                return;
            }

            StringBuilder sbLink = new StringBuilder();
            this.linkLabelVerb.Links.Clear();
            this.linkLabelVerb.Text = String.Empty;

            foreach (DesignerVerb verb in this.Verbs)
            {
                sbLink.Append(verb.Text);
                this.linkLabelVerb.Links.Add(sbLink.Length - verb.Text.Length, verb.Text.Length, verb);
                sbLink.Append(",");
            }

            sbLink.Remove(sbLink.Length - 1, 1);

            this.linkLabelVerb.Text = sbLink.ToString();

            this.linkLabelVerb.Visible = true;
            this.panelVerb.Visible = true;
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 获取所有属性设置的xml形式
        /// </summary>
        /// <returns></returns>
        public string GetXml()
        {
            StringBuilder strProperty = new StringBuilder();

            //迭代所有dataGridView行
            foreach (PropertyGridRow dr in this.PropertyGridRows)
            {
                strProperty.Append(dr.PropertyXml);
            }

            return strProperty.ToString();
        }

        public void SetPropertyValue(string property, object value)
        {
            foreach (PropertyGridRow dr in this.PropertyGridRows)
            {
                if (dr.PropertyName.Equals(property))
                {
                    dr.SetPropertyValue(value);
                    break;
                }
            }
        }

        public object GetPropertyValue(string property)
        {
            foreach (PropertyGridRow dr in this.PropertyGridRows)
            {
                if (dr.PropertyName.Equals(property))
                {
                    return dr.GetPropertyValue();
                }
            }

            return null;
        }

        /// <summary>
        /// TargetObject中的属性已改变,指示立即更新属性行中的值
        /// </summary>
        public void UpdateProperty()
        {
            UpdateProperty(true);
        }

        /// <summary>
        /// TargetObject中的属性已改变,指示立即更新属性行中的值
        /// </summary>
        /// <param name="updateSelectedObject">是否更新选中的对象的可视状态，以及是否触发PropertyChanged事件</param>
        public void UpdateProperty(bool updateSelectedObject)
        {
            this._updateSelectedObject = updateSelectedObject;

            foreach (PropertyGridRow propertyGirdRow in this.PropertyGridRows)
            {
                //行自身的UpdateProperty会负责判断自己是否有效
                propertyGirdRow.UpdateProperty();
            }

            //重置为true
            this._updateSelectedObject = true;
        }

        /// <summary>
        /// TargetObject中的属性已改变,指示立即更新属性行中的值
        /// 根据指定的属性(Property)的名称
        /// </summary>
        /// <param name="name"></param>
        public void UpdateProperty(string name)
        {
            foreach (PropertyGridRow propertyGirdRow in this.PropertyGridRows)
            {
                //找到指定的属性(property),调用UpdateProperty方法更新其值
                if (propertyGirdRow.PropertyName == name)
                {
                    propertyGirdRow.UpdateProperty();
                    break;
                }
            }
        }

        public void AddTypeWrapper(PropertyGridTypeWrapper wrapper)
        {
            Debug.Assert(wrapper != null, "wrapper 为 null");
            Debug.Assert(_typeWrapperCollection.Contains(wrapper) == false, "已经添加过了指定的 wrapper");

            if (wrapper == null || _typeWrapperCollection.Contains(wrapper))
                return;

            _typeWrapperCollection.Add(wrapper);
        }

        public void AddValidator(PropertyGridValidator validator)
        {
            Debug.Assert(validator != null, "validator 为 null");
            Debug.Assert(_validatorCollection.Contains(validator) == false, "已经添加过了指定的 wrapper");

            if (validator == null || _validatorCollection.Contains(validator))
                return;

            _validatorCollection.Add(validator);
        }

        #endregion

        #region 受保护的方法

        internal bool CanVisible(Type type, string propertyName)
        {
            return _typeWrapperCollection.IsVisible(type, propertyName);
        }

        /// <summary>
        /// 选中的对象的属性(property)值 发生改变
        /// 此方法由属性行调用
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="row"></param>
        internal void PropertyValueChanage(object rootObject, object targetObject, string propertyName, object oldValue, object newValue, 
            PropertyGridRow row)
        {
            if (rootObject == null || targetObject == null || propertyName == null || propertyName == String.Empty)
            {
                throw new ArgumentNullException();
            }

            if (ObjectPropertyValueChanged != null)
            {
                ObjectPropertyValueChangedEventArgs eventArgs =
                    new ObjectPropertyValueChangedEventArgs(rootObject, targetObject, propertyName, oldValue, newValue, row);
                ObjectPropertyValueChanged(this, eventArgs);
            }
        }
        
        /// <summary>
        /// 请求验证单元格中欲设置的属性(Property)值
        /// 此方法提供给编辑单元格调用，只有这样才能在验证失败时阻止值设置到属性行中
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <param name="changed"></param>
        /// <returns></returns>
        public PropertyGridValidateResult ValidateValue(string property, object value, bool changed)
        {
            if (changed == false)
                return new PropertyGridValidateResult();

            PropertyGridValidator validator = _validatorCollection.GetValidator(this.SelectedObjects);
            if (validator == null)
                return new PropertyGridValidateResult();

            if (validator.Validator == null)
            {
                Debug.Assert(false, "validator.Validator 为空");
                return new PropertyGridValidateResult();
            }

            PropertyGridValidateArgs argss = new PropertyGridValidateArgs(property, value, this.SelectedObjects);

            return validator.Validator(argss);
        }

        ///// <summary>
        ///// 请求验证单元格中欲设置的属性(Property)值
        ///// 此方法提供给编辑单元格调用
        ///// </summary>
        ///// <param name="property"></param>
        ///// <param name="value"></param>
        ///// <param name="msg"></param>
        ///// <returns></returns>
        //public bool ValidateValue(string property, object value, bool changed, out string msg)
        //{
        //    bool result = true;
        //    if (this.ValidatePropertyValue != null)
        //    {
        //        ValidatePropertyValueEventArgs args = new ValidatePropertyValueEventArgs(property, value, this.SelectedObjects);
        //        args.Changed = changed;
        //        result = ValidatePropertyValue(this, args);
        //        msg = args.Message;
        //    }
        //    else
        //    {
        //        msg = null;
        //    }
        //    return result;
        //}

        #endregion

        #region 事件

        public delegate void OnPropertyChangeHandler(object sender, PropertyChangeEventArgs e);
        /// <summary>
        /// 属性网格中的某个属性(Property)值已更改
        /// 但是注意，这个事件无法处理对象多选时，需要取每个对象旧值的情况
        /// 这个事件把所有选中的对象放在一起处理，因为每个对象的旧值可能都不一样，也就不能取旧值
        /// </summary>
        public event OnPropertyChangeHandler PropertyChanged;

        /// <summary>
        /// 属性网格中选中的对象的属性(Property)值发生改变的事件委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void OnObjectPropertyValueChangedHandler(object sender, ObjectPropertyValueChangedEventArgs e);
        /// <summary>
        /// 属性网格中选中的对象的属性(Property)值发生改变的事件
        /// 此事件寄于每个选中的对象，选中三个对象，就会触发三次，可以取每个对象的新旧值，尤其是可能不同的旧值
        /// 在此事件全部执行完之后，才会触发PropertyChanged事件
        /// </summary>
        public event OnObjectPropertyValueChangedHandler ObjectPropertyValueChanged;

        ////此事件需要在单元格的SetValue方法中调用，只有这样才能在验证失败时阻止值设置到属性行中
        //public delegate bool OnValidatePropertyValueHandler(PropertyGridPad sender, ValidatePropertyValueEventArgs e);
        ///// <summary>
        ///// 在Property单元格验证数据时调用
        ///// 如果结果为false，在ValidatePropertyValueEventArgs中返回验证消息
        ///// 
        ///// 应该用附件验证器来实现此功能。
        ///// </summary>
        //public event OnValidatePropertyValueHandler ValidatePropertyValue;

        #endregion
    }
}
