

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

using System.Diagnostics;
using System.ComponentModel;

namespace Sheng.SailingEase.ComponentModel.Design
{
    public class PropertyGridRow : DataGridViewRow, ICollapseRow
    {
        #region 公开属性

        private bool _valueChangingBySelf = false;
        /// <summary>
        /// 是否由自身引发的更新
        /// </summary>
        public bool ValueChangingBySelf
        {
            get { return this._valueChangingBySelf; }
            set { this._valueChangingBySelf = value; }
        }

        private PropertyGridRow _parentRow;
        /// <summary>
        /// 此属性行的父行
        /// 分类行不算
        /// </summary>
        public PropertyGridRow ParentRow
        {
            get
            {
                return this._parentRow;
            }
            set
            {
                this._parentRow = value;
            }
        }

        /// <summary>
        /// 此属性行是否有效
        /// 造成无效的原因如：
        /// 此属性行包含了多个不同的对象,但是这些对象不是每个都具备本属性(Property)行指定的属性(Property)
        /// 或Attrbute上设置了多选时不显示等
        /// </summary>
        public bool CanVisible
        {
            get
            {
                //_propertyRelatorAttribute肯定是有的，不然就不会存在这么个属性行
                //如果可见性为仅在指定一个对象时可见并当前选定了多个对象,将其跳过
                if (((_propertyRelatorAttribute.Visibility & EunmPropertyVisibility.Single) == EunmPropertyVisibility.Single)
                    && this.SelectedObjects.Count > 1)
                {
                    return false;
                }

                //如果指定了此属性仅在设计器中显示,而当前不在设计器中,将其跳过
                if (((_propertyRelatorAttribute.Visibility & EunmPropertyVisibility.Desinger) == EunmPropertyVisibility.Desinger)
                    && _propertyGrid.InDesigner == false)
                {
                    return false;
                }

                //判断当前关联的对象，是否每个都有此行的PropertyInfo
                foreach (object obj in this.SelectedObjects)
                {
                    if (this.PropertyPath.ContainerProperty(obj) == false)
                        return false;
                }

                //通过TypeWarpper判断
                foreach (object obj in this.SelectedObjects)
                {
                    if (this._propertyGrid.CanVisible(obj.GetType(), this.PropertyName) == false)
                        return false;
                }

                //如果前面的判断都通过，至此认为此行有效，反回true
                return true;
            }
        }

        private PropertyRelatorAttribute _propertyRelatorAttribute;
        /// <summary>
        /// 相关联的Attribute
        /// </summary>
        public PropertyRelatorAttribute PropertyRelatorAttribute
        {
            get
            {
                return this._propertyRelatorAttribute;
            }
        }

        /// <summary>
        /// 获取属性(Property)名
        /// </summary>
        public string PropertyName
        {
            get
            {
                return this._propertyInfo.Name;
            }
        }

        public string Catalog
        {
            get { return PropertyRelatorAttribute.Catalog; }
        }

        /// <summary>
        /// 获取属性(Property)名完整路径
        /// 对于子属性，如 Font/Bold
        /// </summary>
        public string PropertyFullName
        {
            get
            {
                string str = this.PropertyName;
                if (this.ParentRow != null)
                {
                    str = this.ParentRow.PropertyFullName + "\\" + str;
                }

                return str;
            }
        }

        private PropertyPath _propertyPath;
        /// <summary>
        /// 此属性（property）行的属性路径
        /// </summary>
        public PropertyPath PropertyPath
        {
            get
            {
                if (_propertyPath == null)
                {
                    _propertyPath = new PropertyPath();
                    _propertyPath.CombineToFore(this._propertyInfo);
                    if (this.ParentRow != null)
                    {
                        _propertyPath.CombineToFore(this.ParentRow.PropertyPath);
                    }
                }

                return _propertyPath;
            }
        }

        private List<object> _selectedObjects = new List<object>();
        /// <summary>
        /// 当前属性行所属的目标对象
        /// 若要设置此属性,应使用 SetSelectedObjects  方法!
        /// </summary>
        public List<object> SelectedObjects
        {
            get
            {
                return this._selectedObjects;
            }
            private set
            {
                this._selectedObjects = value;
            }
        }

        public override bool ReadOnly
        {
            get
            {
                return base.ReadOnly;
            }
            set
            {
                if (value == this.ReadOnly)
                    return;

                //当 propertyRelatorAttribute.ReadOnly 为true时，是关于是否只读的，有最高优先级别的设置
                //但当其为 false 时，则允许外部变更此行是否readonly
                if (value == false && _propertyRelatorAttribute.ReadOnly)
                    return;

                base.ReadOnly = value;

                //DataGridView为null时，是拿不到 this.Cells["PropertyValue"] 的
                //根据名称拿Cell貌似要从DataGridView.Columns里取的
                //如果DataGridView为null，也没有必要改颜色了，因为表示此行没有加到DataGridView里
                //不过这里直接用 _propertyGridCell 访问就不存在这个问题
                if (ReadOnly)
                {
                    if (this.DataGridView != null)
                        _propertyGridCell.Style.Font = this.DataGridView.DefaultCellStyle.Font;

                    _propertyGridCell.Style.ForeColor = Color.Gray;
                    _propertyGridCell.Style.SelectionForeColor = Color.Gray;
                }
                else
                {
                    _propertyGridCell.Style.ForeColor = Color.Black;
                    _propertyGridCell.Style.SelectionForeColor = Color.Black;
                }

                //取消（ReadOnly）或者恢复（Not ReadOnly）粗体突出显示
                this.ContrastDefaultValue();
            }
        }

        #region 单元格访问器

        //这些获取列的属性不使用RowIndex到DataGridView中取
        //因为RowIndex不能保证总是有效，在未把行加到DataGridView中时，index是无效的

        public DataGridViewCell PropertyNameCell
        {
            get { return this.Cells["PropertyName"]; }
        }

        public DataGridViewCell DescriptionCell
        {
            get { return this.Cells["Description"]; }
        }

        public DataGridViewCell PropertyValueCell
        {
            get { return this.Cells["PropertyValue"]; }
        }

        public DataGridViewCell PropertyCell
        {
            get { return this.Cells["Property"]; }
        }

        #endregion

        #endregion

        #region 私有成员

        private TypeConverter _typeConverter = new TypeConverter();

        private PropertyInfo _propertyInfo;

        /// <summary>
        /// 用于操作属性值编辑列
        /// </summary>
        private IPropertyGirdCell _iPropertyGirdCell;

        /// <summary>
        /// 本行使用的编辑cell
        /// </summary>
        private DataGridViewCell _propertyGridCell;

        /// <summary>
        /// 当前行所属的属性网格控件
        /// </summary>
        private PropertyGridPad _propertyGrid;

        #endregion

        #region 构造

        /// <summary>
        /// 构造,应使用此构造,将SelectedObject的初始化留在后面处理,因为要兼容多对象同时选中
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="propertyInfoName"></param>
        public PropertyGridRow(PropertyRelatorAttribute attribute, PropertyInfo propertyInfo, PropertyGridPad propertyGrid)
        {
            this._propertyRelatorAttribute = attribute;
            this._propertyInfo = propertyInfo;
            this._propertyGrid = propertyGrid;

            #region 初始化单元格

            DataGridViewCell dcPropertyName = new DataGridViewTextBoxCell();
            dcPropertyName.Value = attribute.PropertyDisplayName;
            this.Cells.Add(dcPropertyName);

            DataGridViewCell dcDescription = new DataGridViewTextBoxCell();
            dcDescription.Value = attribute.Description;
            this.Cells.Add(dcDescription);

            #region 初始化用于编辑属性值的单元格

            //获取PropertyEditorAttribute
            PropertyEditorAttribute[] editorAttribute = 
                (PropertyEditorAttribute[])propertyInfo.GetCustomAttributes(typeof(PropertyEditorAttribute), true);
            if (editorAttribute == null || editorAttribute.Length == 0)
            {
                //没有加PropertyEditorAttribute的Attribute，分配一个默认的文本框做为编辑控件
                //TODO:分配一个默认的文本框做为编辑控件
            }
            else
            {
                //创建属性(Property)单元格
                _propertyGridCell = CellFactory.Instance.Create(editorAttribute[0]) as DataGridViewCell;

                //TODO:这里要考虑没有创建出合适的cell的情况，既_propertyGridCell为null的情况

                _iPropertyGirdCell = _propertyGridCell as IPropertyGirdCell;
                _iPropertyGirdCell.PropertyRelatorAttribute = attribute;

                //查找并设置（如果有）DefaultValueAttribute
                DefaultValueAttribute[] defaultValueAttribute =
                    (DefaultValueAttribute[])propertyInfo.GetCustomAttributes(typeof(DefaultValueAttribute), true);
                if (defaultValueAttribute != null && defaultValueAttribute.Length > 0)
                {
                    _iPropertyGirdCell.DefaultValueAttribute = defaultValueAttribute[0];
                }

                //查找并设置（如果有）PropertyEditorAttribute
                if (editorAttribute != null && editorAttribute.Length > 0)
                {
                    _iPropertyGirdCell.PropertyEditorAttribute = editorAttribute[0];
                }
            }

            _iPropertyGirdCell.Owner = this._propertyGrid;
            _iPropertyGirdCell.OwnerRow = this;

            _propertyGridCell.Style.BackColor = Color.White;
            _propertyGridCell.Style.ForeColor = Color.Black;
            _propertyGridCell.Style.SelectionBackColor = Color.White;
            _propertyGridCell.Style.SelectionForeColor = Color.Black;

            this.Cells.Add(_propertyGridCell);

            #endregion

            DataGridViewCell dcProperty = new DataGridViewTextBoxCell();  //属性的真实名
            dcProperty.Value = propertyInfo.Name;
            this.Cells.Add(dcProperty);

            #endregion

            //此处的只读设置是享有最高优先级的
            this.ReadOnly = attribute.ReadOnly;

            //初始化子属性行
            InitSubRow();
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 初始化子属性行
        /// </summary>
        private void InitSubRow()
        {
            if (this.SubRows != null)
                this.SubRows.Clear();

            if (this._propertyInfo == null)
                return;

            //看这个属性对象是否还有需要初始化为子属性行的属性(Property)
            foreach (PropertyInfo propertyInfoChild in this._propertyInfo.PropertyType.GetProperties())
            {
                PropertyRelatorAttribute propertyGirdAttribute = null;

                PropertyRelatorAttribute[] attributes = (PropertyRelatorAttribute[])propertyInfoChild.GetCustomAttributes(typeof(PropertyRelatorAttribute), true);
                if (attributes == null || attributes.Length == 0)
                    continue;

                propertyGirdAttribute = attributes[0];

                //至此,确定这是一个要显示的子属性
                PropertyGridRow propertyGirdRowChild = new PropertyGridRow(propertyGirdAttribute, propertyInfoChild, this._propertyGrid);

                if (this.SubRows == null)
                    this.SubRows = new List<PropertyGridRow>();

                propertyGirdRowChild.ParentRow = this;

                this.SubRows.Add(propertyGirdRowChild);
            }
        }

        /// <summary>
        /// 增加一个选择的对象
        /// 目前实际只在此对象内部调用,外部调用SetSelectedObjects
        /// </summary>
        /// <param name="obj"></param>
        private void AddSelectedObject(object obj)
        {
            if (obj == null)
                return;

            this.SelectedObjects.Add(obj);

            #region 判断新的对象是否存在这个属性(PropertyInfo)

            ////判断新的对象是否存在这个属性(PropertyInfo)
            if (this.PropertyPath.ContainerProperty(obj) == false)
                return;

            #endregion

            #region SetPropertyValue

            //如果这是第一个设置过来的对象
            //直接设置属性值
            if (this.SelectedObjects.Count == 1)
            {
                SetPropertyValue(GetObjectPropertyValue(obj));
            }
            else
            {
                //如果不是第一个设置过来的对象
                //判断值是否相同
                object objValue = GetPropertyValue();
                if (objValue == null || objValue.ToString() == String.Empty)
                {
                    SetPropertyValue(objValue);
                }
                else
                {
                   
                    object obj1, obj2;
                    if (this._propertyInfo.PropertyType.IsEnum)
                    {
                        obj1 = Enum.Parse(this._propertyInfo.PropertyType, GetPropertyValue().ToString());
                        obj2 = Enum.Parse(this._propertyInfo.PropertyType, this._propertyInfo.GetValue(obj, null).ToString());
                    }
                    else
                    {
                        obj1 = Convert.ChangeType(GetPropertyValue(), this._propertyInfo.PropertyType);
                        obj2 = this.PropertyPath.GetValue(obj);
                    }

                    if (obj1.Equals(obj2) == false)
                    {
                        SetPropertyValue(null);
                    }
                }
            }

            #endregion
        }

        private object GetObjectPropertyValue(object obj)
        {
            return this.PropertyPath.GetValue(obj);
        }

        private void propertyGirdRowChild_PropertyChanged()
        {
            if (this.DataGridView != null)
            {
                this.DataGridView.InvalidateRow(this.Index);
            }
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 设置该行关联的对象
        /// </summary>
        /// <param name="objs"></param>
        public void SetSelectedObjects(object[] objs)
        {
            this.SelectedObjects.Clear();

            if (objs == null)
                return;

            foreach (object obj in objs)
            {
                AddSelectedObject(obj);
            }

            //将此对象也分配到此行的子属性行中（如果有）
            if (this.SubRows != null)
            {
                foreach (PropertyGridRow row in this.SubRows)
                {
                    //row.SetSelectedObjects(new object[] { this.GetPropertyValue() });
                    row.SetSelectedObjects(objs);

                    //订阅子行的PropertyChanged事件
                    if (row.PropertyChanged == null)
                        row.PropertyChanged += new Action(propertyGirdRowChild_PropertyChanged);
                }
            }
        }

        /// <summary>
        /// 获取该属性(property)的完整xml形式
        /// </summary>
        public string PropertyXml
        {
            get
            {
                _iPropertyGirdCell = this.Cells[2] as IPropertyGirdCell;

                if (_iPropertyGirdCell == null)
                {
                    throw new NotImplementedException();
                }

                return _iPropertyGirdCell.GetPropertyXml(this.PropertyRelatorAttribute.XmlNodeName);
            }
        }

        public string PropertyString
        {
            get
            {
                _iPropertyGirdCell = this.Cells[2] as IPropertyGirdCell;

                if (_iPropertyGirdCell == null)
                {
                    throw new NotImplementedException();
                }

                return _iPropertyGirdCell.GetPropertyString();
            }
        }

        /// <summary>
        /// 向属性行设置一个值
        /// 仅在初始化属性行时调用，用于初始化属性行上的属性(Property)值
        /// </summary>
        /// <param name="value"></param>
        public void SetPropertyValue(object value)
        {
            //注意
            //进这个方法时，该行还没有被加到DataGridView中，此时还在行初始化阶段
            //所以在这里调用任何和DataGridView有关的方法都是无效的

            IPropertyGirdCell iPropertyGirdCell = this._propertyGridCell as IPropertyGirdCell;
            if (iPropertyGirdCell == null)
            {
                throw new NotImplementedException();
            }

            iPropertyGirdCell.SetPropertyValue(value);

            //如果属性值是null,把可能包含的子属性行去掉
            if (value == null)
            {
                #region 移除可能存在的子属性行

                //移除可能存在的子属性行
                if (this.SubRows != null && this.DataGridView != null)
                {
                    IsCollapse = true;
                }

                this.SubRows = null;

                #endregion
            }
            else
            {
                #region 判断是否需要添加子属性行

                //判断是否需要添加子属性行
                //因为可能子属性行之前因为value为null被清掉了,在重新设置不为null的值后,还要再把它们加载进来
                if (this.SubRows == null)
                {
                    InitSubRow();
                }

                #endregion
            }

            if (this.Index > -1)
                this.DataGridView.InvalidateRow(this.Index);


            //处理此属性行可能包含的子属性行
            if (this.SubRows != null)
            {
                foreach (PropertyGridRow row in this.SubRows)
                    row.UpdateProperty();
            }

            //如此当前行是一个子属性行,需要通知它的父行重绘,以显示新的属性值
            //但是注意可能这些行还没有被加载到DataGridView中
            if (this.ParentRow != null && this.DataGridView != null)
                this.DataGridView.InvalidateRow(this.ParentRow.Index);
        }

        /// <summary>
        /// 取出 PropertyGirdCell中的值
        /// </summary>
        /// <returns></returns>
        public object GetPropertyValue()
        {
            IPropertyGirdCell iPropertyGirdCell = this._propertyGridCell as IPropertyGirdCell;
            if (iPropertyGirdCell == null)
            {
                throw new NotImplementedException();
            }

            return iPropertyGirdCell.GetPropertyValue();
        }

        /// <summary>
        /// 从PropertyGirdCell中取出旧值
        /// </summary>
        /// <returns></returns>
        public object GetPropertyOldValue()
        {
            IPropertyGirdCell iPropertyGirdCell = this._propertyGridCell as IPropertyGirdCell;
            if (iPropertyGirdCell == null)
            {
                throw new NotImplementedException();
            }

            return iPropertyGirdCell.GetPropertyOldValue();
        }

        /// <summary>
        /// 目标对象中的属性已改变,指示立即更新属性行中的值
        /// </summary>
        public void UpdateProperty()
        {
            //如果改行无效，直接返回
            //这是因为在多选时，有些行是无效的，但属性网格中用于保存所有可能的行的PropertyGridRows中
            //还是保存了这些行的，虽然它们最终没有被显示
            //在更新对像时，属性网格对象会循环调用PropertyGridRows中的行的UpdateProperty方法
            //所以这里需要做一次有效性的判断 
            if (this.CanVisible == false)
                return;

            if (this.SelectedObjects == null || SelectedObjects.Count == 0)
                return;

            //在多选情况下,需判断所有对象的新值是否相同,不同则不显示
            object objValue = GetObjectPropertyValue(SelectedObjects[0]);

            //如果为null直接返回就行了，相同不相同都是不显示内容的
            if (objValue == null)
            {
                this.SetPropertyValue(objValue);
                return;
            }

            for (int i = 1; i < this.SelectedObjects.Count; i++)
            {
                //如果为null直接返回就行了，相同不相同都是不显示内容的
                if (GetObjectPropertyValue(this.SelectedObjects[i]) == null)
                    return;

                if (objValue.Equals(GetObjectPropertyValue(this.SelectedObjects[i])) == false)
                    return;
            }

            this.SetPropertyValue(objValue);
        }

        /// <summary>
        /// 更新目标对象的属性值
        /// 用于在Property编辑cell中更改了值后,更新目标对象的相应Property
        /// 此方法在UserControlPropertyGrid中的单元格失去焦点事件中调用
        /// </summary>
        public void UpdateSelectedObject()
        {
            foreach (object obj in this.SelectedObjects)
            {
                object newValue, oldValue = null;

                #region 取编辑单元框中的 newValue, oldValue

                if (this._propertyInfo.PropertyType.IsEnum)
                {
                    newValue = Enum.Parse(this._propertyInfo.PropertyType, this.GetPropertyValue().ToString());
                }
                else
                {
                   

                    //注意,对于引用型对象,这里拿到的是一个新实例
                    object temp = this.GetPropertyValue();
                    if (temp != null)
                        try
                        {
                            newValue = Convert.ChangeType(temp, this._propertyInfo.PropertyType);
                        }
                        catch (InvalidCastException ex)
                        {
                            Debug.Assert(false, "Convert.ChangeType(temp, propertyInfo.PropertyType);\r\n" + ex.Message);
                            newValue = null;
                        }
                    else
                        newValue = null;
                }

                //oldValue = this._propertyInfo.GetValue(obj, null);
                oldValue = this.PropertyPath.GetValue(obj);

                #endregion

                //对于某些属性，在多选后设置新值，必须克隆新对象分配到每个选定的对象
                //而不能把同一个新值的引用分配到每个选定的对象，因为它们不能共用，如字体

                if (this._propertyRelatorAttribute.CloneValue)
                {
                    ICloneable clone = newValue as ICloneable;

                    Debug.Assert(clone != null, "对象没有实现ICloneable接口");

                    if (clone != null)
                        newValue = clone.Clone();
                }

                this.PropertyPath.SetValue(obj, newValue);

                //设置完毕后，使属性网络触发属性值已变更的事件
                if (newValue.Equals(oldValue) == false)
                {
                    _propertyGrid.PropertyValueChanage(obj, this.PropertyPath.GetParentValue(obj), this._propertyInfo.Name, oldValue, newValue, this);
                }

                //向父行(如果有)发出属性已更改的通知
                if (PropertyChanged != null)
                {
                    PropertyChanged();
                }
            }
        }

        /// <summary>
        /// 将当前属性值与默认属性值（如果有）对比
        /// 如果不同，就把当前属性值粗体显示，否则常规显示
        /// </summary>
        internal void ContrastDefaultValue()
        {
            if (this.DataGridView == null)
                return;

            //如果是只读，直接默认字体就行了，没必要加粗显示，也就没必要比较值
            if (this.ReadOnly)
            {
                _propertyGridCell.Style.Font = this.DataGridView.DefaultCellStyle.Font;
                return;
            }

            object objCurrentValue = GetPropertyValue();

            if (objCurrentValue == null)
                return;

            //这里必须判断当前值是否是空串，如果是空串，在下面进行类型转换时肯定会异常
            //如尝试把空串转成Int
            if (objCurrentValue.ToString() == String.Empty)
                return;

            if (_iPropertyGirdCell.DefaultValueAttribute == null)
                return;

            bool equal = true;

            if (_iPropertyGirdCell.DefaultValueAttribute.Value.GetType().IsEnum)
            {
                PropertyComboBoxEditorAttribute editorAttribute = (PropertyComboBoxEditorAttribute)_iPropertyGirdCell.PropertyEditorAttribute;

                equal = Enum.Parse(editorAttribute.Enum, objCurrentValue.ToString()).Equals(_iPropertyGirdCell.DefaultValueAttribute.Value);
            }
            else
            {
                //设置过来的值是String，如 "300"，而默认值是Property的类型，如Int
                //在这种情况下需要类型转换后再做比较判断，否则结果永远是false

                if (Type.Equals(objCurrentValue, _iPropertyGirdCell.DefaultValueAttribute.Value))
                    equal = objCurrentValue.Equals(_iPropertyGirdCell.DefaultValueAttribute.Value);
                else
                {
                    IConvertible convertible = objCurrentValue as IConvertible;
                    if (convertible != null)
                    {
                        equal = Convert.ChangeType(
                            objCurrentValue, _iPropertyGirdCell.DefaultValueAttribute.Value.GetType()
                            ).Equals(_iPropertyGirdCell.DefaultValueAttribute.Value);
                    }
                    else
                    {
                        equal = objCurrentValue.Equals(_iPropertyGirdCell.DefaultValueAttribute.Value);
                    }
                }
            }

            if (equal)
            {
                _propertyGridCell.Style.Font = this.DataGridView.DefaultCellStyle.Font;
            }
            else
            {
                _propertyGridCell.Style.Font = new Font(this.DataGridView.DefaultCellStyle.Font, FontStyle.Bold);
            }
        }

        #endregion

        #region 公开事件

        /// <summary>
        /// 属性(Property)已更改
        /// 此事件用于父行子行间的属性显示协调
        /// 父行订阅子行的这个事件以得知子行属性已变更
        /// </summary>
        public event Action PropertyChanged;

        #endregion

        #region ICollapseRow 成员

        private List<PropertyGridRow> _subRows;
       
        /// <summary>
        /// 子属性行
        /// </summary>
        public List<PropertyGridRow> SubRows
        {
            get
            {
                return _subRows;
            }
            set
            {
                _subRows = value;
            }
        }

        private bool _isCollapse = true;
        /// <summary>
        /// 是否处在折叠显示状态
        /// </summary>
        public bool IsCollapse
        {
            get
            {
                return _isCollapse;
            }
            set
            {
                _isCollapse = value;

                if (value)
                {
                    HideSubRow();
                }
                else
                {
                    ShowSubRow();
                }
            }
        }

        /// <summary>
        /// 不要直接调用
        /// 通过设置 IsCollapse 属性来获得相应的功能
        /// </summary>
        private void ShowSubRow()
        {
            if (this.SubRows == null || this.SubRows.Count == 0)
            {
                return;
            }

            this.DataGridView.SuspendLayout();

            for (int i = 0; i < this.SubRows.Count; i++)
            {
                this.DataGridView.Rows.Insert(this.Index + 1 + i, this.SubRows[i]);
            }

            this.DataGridView.ResumeLayout();
        }

        /// <summary>
        /// 不要直接调用
        /// 通过设置 IsCollapse 属性来获得相应的功能
        /// </summary>
        private void HideSubRow()
        {
            if (this.SubRows == null || this.SubRows.Count == 0)
            {
                return;
            }

            this.DataGridView.SuspendLayout();

            for (int i = 0; i < this.SubRows.Count; i++)
            {
                if (this.SubRows[i].DataGridView == this.DataGridView)
                {
                    //首先处理可能的子行和子行的子行]
                    ICollapseRow collapseRow = this.SubRows[i] as ICollapseRow;
                    if (collapseRow != null)
                        collapseRow.IsCollapse = true;

                    this.DataGridView.Rows.Remove(this.SubRows[i]);
                }
            }

            this.DataGridView.ResumeLayout();
        }

        #endregion
    }
}
