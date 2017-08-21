using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Sheng.SailingEase.ComponentModel
{
    /// <summary>
    /// property编辑器属性,应用在对象的property上
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Property,AllowMultiple = false,Inherited=true)]
    public class PropertyDescriberAttribute : Attribute
    {
        #region 公开属性

        //this
        private string _xmlNodeName;
        /// <summary>
        /// XML节点的名称
        /// 获取值时,创建以此命名的xml节点,将字符串值放在其中
        /// </summary>
        public string XmlNodeName
        {
            get
            {
                return this._xmlNodeName;
            }
            set
            {
                this._xmlNodeName = value;
            }
        }

        //this
        private string _propertyDisplayName;
        /// <summary>
        /// 属性名
        /// </summary>
        public string PropertyDisplayName
        {
            get
            {
                return this._propertyDisplayName;
            }
            set
            {
                this._propertyDisplayName = value;
            }
        }

        private EnumEditingControl _editingControl;
        /// <summary>
        /// 编辑控件
        /// </summary>
        public EnumEditingControl EditingControl
        {
            get
            {
                return this._editingControl;
            }
            set
            {
                this._editingControl = value;
            }
        }

        //this
        private string _catalog;
        /// <summary>
        /// 分组,于对子属性此属性将被忽略
        /// </summary>
        public string Catalog
        {
            get
            {
                return this._catalog;
            }
            set
            {
                this._catalog = value;
            }
        }

        //this
        private string _description = String.Empty;
        /// <summary>
        /// 显示在PropertyGrid下方文本框中的注释
        /// </summary>
        public string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        //combobox
        private EnumComboBoxEnum _comboBoxEnumKey;
        /// <summary>
        /// 当编辑控件(PropertyGirdEditingControl)为ComboBox时,(必须)指定一个要加载的枚举
        /// </summary>
        public EnumComboBoxEnum ComboBoxEnumKey
        {
            get
            {
                return this._comboBoxEnumKey;
            }
            set
            {
                this._comboBoxEnumKey = value;
            }
        }

        //NumericUpDown
        private int _numericMin = 1;
        /// <summary>
        /// 在编辑控件为数据调整控件时的最小值
        /// </summary>
        public int NumericMin
        {
            get
            {
                return this._numericMin;
            }
            set
            {
                this._numericMin = value;
            }
        }

        //NumericUpDown
        private int _numericMax = 10000;
        /// <summary>
        /// 在编辑控件为数据调整控件时的最大值
        /// </summary>
        public int NumericMax
        {
            get
            {
                return this._numericMax;
            }
            set
            {
                this._numericMax = value;
            }
        }

        //textbox
        private TypeCode _typeCode = TypeCode.String;
        /// <summary>
        /// 在使用TextBox时,指定属性(Property)允许的数据类型
        /// </summary>
        public TypeCode TypeCode
        {
            get
            {
                return this._typeCode;
            }
            set
            {
                this._typeCode = value;
            }
        }

        //textbox
        /// <summary>
        /// 在使用TextBox时,指定一个要匹配的正则表达式
        /// </summary>
        public string Regex
        {
            get;
            set;
        }

        //textbox 
        /// <summary>
        /// 在使用TextBox时,指定匹配正则表达式失败后的提示信息
        /// </summary>
        public string RegexMsg
        {
            get;
            set;
        }

        private EnumProcess _process = PropertyDescriberAttribute.EnumProcess.Null;
        /// <summary>
        /// 指定一个特殊操作
        /// </summary>
        public EnumProcess Process
        {
            get
            {
                return this._process;
            }
            set
            {
                this._process = value;
            }
        }

        //textbox 
        private bool _allowEmpty = true;
        /// <summary>
        /// 在使用TextBox时,指定是否允许留空
        /// </summary>
        public bool AllowEmpty
        {
            get
            {
                return this._allowEmpty;
            }
            set
            {
                this._allowEmpty = value;
            }
        }

        //this
        private EunmVisibility _visibility = PropertyDescriberAttribute.EunmVisibility.ANY;
        /// <summary>
        /// 在属性网格中的可见性
        /// </summary>
        public EunmVisibility Visibility
        {
            get
            {
                return this._visibility;
            }
            set
            {
                this._visibility = value;
            }
        }

        //textbox
        private int _maxLength = 32767;
        /// <summary>
        /// 在使用TextBox时,允许的文本长度
        /// </summary>
        public int MaxLength
        {
            get
            {
                return this._maxLength;
            }
            set
            {
                this._maxLength = value;
            }
        }

        //DataItemChoose
        private bool _showDataItem = true;
        /// <summary>
        /// 在使用DataEntityChoose时,是否在数据实体下显示数据项
        /// </summary>
        public bool ShowDataItem
        {
            get
            {
                return this._showDataItem;
            }
            set
            {
                this._showDataItem = value;
            }
        }

        //this
        /// <summary>
        /// 默认值
        /// </summary>
        public object DefaultValue
        {
            get;
            set;
        }

        #endregion

        #region 构造

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="editingControl"></param>
        public PropertyDescriberAttribute(string propertyName,
            EnumEditingControl editingControl, string catalog)
        {
            this.XmlNodeName = _xmlNodeName;
            this.PropertyDisplayName = propertyName;
            this.EditingControl = editingControl;
            this.Catalog = catalog;
        }

        #endregion

        #region 枚举

        /// <summary>
        /// 在属性编辑器网格中使用的控件
        /// </summary>
        public enum EnumEditingControl
        {
            /// <summary>
            /// 文本框
            /// </summary>
            TextBox = 0,
            /// <summary>
            /// 颜色选择
            /// </summary>
            ColorChoose = 1,
            /// <summary>
            /// 复合文本框
            /// </summary>
            ComboBox = 2,
            /// <summary>
            /// 数字调整控件
            /// </summary>
            NumericUpDown = 3,
            /// <summary>
            /// 窗体元素边缘锚定设置
            /// </summary>
            Anchor = 4,
            /// <summary>
            /// 数据项选择
            /// </summary>
            DataItemChoose = 5,
            /// <summary>
            /// 图像资源选择
            /// </summary>
            ImageResourceChoose = 6,
            /// <summary>
            /// 字体设置
            /// </summary>
            Font = 7,
            /// <summary>
            /// 布尔值编辑
            /// </summary>
            Boolean = 8
        }

        /// <summary>
        /// 在属性编辑器中的分组
        /// </summary>
        public enum PropertyGroup
        {
            /// <summary>
            /// 常规
            /// </summary>
            Normal = 0,
            /// <summary>
            /// 布局
            /// </summary>
            Layout = 1,
            /// <summary>
            /// 外观
            /// </summary>
            Style = 2,
            /// <summary>
            /// 杂项
            /// </summary>
            Other = 3
        }

        //否决
        /// <summary>
        /// 当编辑控件(PropertyGirdEditingControl)为ComboBox时,(必须)指定一个要加载的枚举
        /// </summary>
        public enum EnumComboBoxEnum
        {
            /// <summary>
            /// 提供是/否选择
            /// </summary>
            TrueFalse = 0,
            /// <summary>
            /// 提供下拉框样式选择
            /// </summary>
            ComboBoxStyle = 1,
            /// <summary>
            /// 窗体的初始可视状态
            /// </summary>
            FormWindowState = 2,
            /// <summary>
            /// 绘图表面上内容的对齐方式
            /// </summary>
            ContentAlignment = 3,
            /// <summary>
            /// 控制如何显示图像位置和控件大小
            /// </summary>
            PictureBoxSizeMode = 4,
            /// <summary>
            /// 可分页DataGridView分页导航显示位置
            /// </summary>
            PaginationDataGridViewNavigationLocation = 5,
            /// <summary>
            /// 指定ToolStripItem显示何种对像
            /// </summary>
            ToolStripItemDisplayStyle = 6,
            /// <summary>
            /// 控件上图像和文本的相对位置
            /// </summary>
            TextImageRelation = 7,
            /// <summary>
            /// 对齐方式
            /// </summary>
            ToolStripItemAlignment = 8
        }

        /// <summary>
        /// 指定一个特殊操作
        /// </summary>
        public enum EnumProcess
        {
            /// <summary>
            /// 无特殊操作
            /// </summary>
            Null = 0,
            /// <summary>
            /// 所标识的字段是代码,需要验证唯一性
            /// </summary>
            Code = 1
        }

        /// <summary>
        /// 在属性网格中的可见性
        /// 注意,枚举的常量应定义为2 的幂,即 1、2、4、8 等,这意味着组合的枚举常量中的各个标志都不重叠
        /// ^ 异或,有的去掉,没的加上
        /// | 没的加上
        /// & 将数值中与标志不对应的所有位都设置为零
        /// </summary>
        [Flags]
        public enum EunmVisibility
        {
            /// <summary>
            /// 在所有情况下可见
            /// </summary>
            ANY = 0,
            /// <summary>
            /// 仅在目标对象是单个对象的情况下可见
            /// </summary>
            Single = 1,
            /// <summary>
            /// 仅在窗体设计器的属性网格中显示
            /// </summary>
            Desinger = 2
        }

        #endregion
    }

    //TODO:否决，文本资源的问题已可以解决
    public static class PropertyCatalog
    {
        public const string Style = "样式";
        public const string Normal = "常规";
        public const string Layout = "布局";
        public const string Other = "其它";
        public const string Pagination = "分页";
    }
}
