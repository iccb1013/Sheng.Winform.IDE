using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Drawing;
using Sheng.SailingEase.Kernal;
using System.Xml.Linq;

namespace Sheng.SailingEase.Core
{
    /// <summary>
    /// 所有窗体元素的实体基类
    /// 在IDE和SHELL中继承的类注意为 EventTypes 属性赋值
    /// </summary>
    [Serializable]
    public abstract class UIElement : EntityBase, IEventSupport
    {
        #region 受保护的属性

        [NonSerialized]
        private EventTypesAbstract _eventTypesAdapter = Sheng.SailingEase.Core.EventTypes.Instance;
        /// <summary>
        /// 事件类型适配器
        /// 在继承的类中设置此属性，可使FromXml方法初始化相应的类型，如IDE中初始化DEV结尾的类型，SHELL中初始化EX结尾的类型
        /// 但是注意，IDE中必须通过 OnDeserialized 属性标记的方法再次分配合适的引用
        /// 用Adapter结尾命名是避免和 EventTypes 类混淆和不明确引用
        /// </summary>
        protected EventTypesAbstract EventTypesAdapter
        {
            get { return this._eventTypesAdapter; }
            set { this._eventTypesAdapter = value; }
        }

        #endregion

        #region 公开属性

        /// <summary>
        /// 是否可用作其它控件的数据源
        /// </summary>
        public virtual bool DataSourceUseable
        {
            get { return false; }
        }

       
        [NonSerialized]
        private WindowEntity _hostFormEntity;
        /// <summary>
        /// 此窗体元素实体所属的FormEntity
        /// </summary>
        public WindowEntity HostFormEntity
        {
            get
            {
                return this._hostFormEntity;
            }
            set
            {
                this._hostFormEntity = value;

                if (this.Events != null)
                {
                    if (this.Events.HostFormEntity == null ||
                        (this.Events.HostFormEntity != null && this.Events.HostFormEntity.Equals(value) == false))
                    {
                        this.Events.HostFormEntity = value;
                    }
                }
            }
        }

        private string _text = String.Empty;
        /// <summary>
        /// 与元素关联的文本
        /// </summary>
        [DefaultValue(StringUnity.EmptyString)]
        [CorePropertyRelator("FormElement_Text", "PropertyCatalog_Normal", Description = "FormElement_Text_Description", XmlNodeName = "Text")]
        [PropertyTextBoxEditorAttribute()]
        public string Text
        {
            get
            {
                return this._text;
            }
            set
            {
                this._text = value;
            }
        }

        private string _backColorValue = String.Empty;
        /// <summary>
        /// 存储背景颜色设置值
        /// </summary>
        [DefaultValue(StringUnity.EmptyString)]
        [CorePropertyRelator("FormElement_BackColorValue", "PropertyCatalog_Style", Description = "FormElement_BackColorValue_Description", XmlNodeName = "BackColorValue")]
        [PropertyColorChooseEditorAttribute()]
        public string BackColorValue
        {
            get
            {
                return this._backColorValue;
            }
            set
            {
                this._backColorValue = value;
            }
        }

        /// <summary>
        /// 根据颜色设置值获取背景色
        /// </summary>
        public Color BackColor
        {
            get
            {
                return ColorRepresentationHelper.GetColorByValue(this.BackColorValue);
            }
        }

        private string _foreColorValue = String.Empty;
        /// <summary>
        /// 存储前景颜色设置值
        /// </summary>
        [DefaultValue(StringUnity.EmptyString)]
        [CorePropertyRelator("FormElement_ForeColorValue", "PropertyCatalog_Style", Description = "FormElement_ForeColorValue_Description", XmlNodeName = "ForeColorValue")]
        [PropertyColorChooseEditorAttribute()]
        public string ForeColorValue
        {
            get
            {
                return this._foreColorValue;
            }
            set
            {
                this._foreColorValue = value;
            }
        }

        /// <summary>
        /// 根据颜色设置值获取前景色
        /// </summary>
        public Color ForeColor
        {
            get
            {
                return ColorRepresentationHelper.GetColorByValue(this.ForeColorValue);
            }
        }

        private bool _enabled = true;
        /// <summary>
        /// 启用
        /// </summary>
        [DefaultValue(true)]
        [CorePropertyRelator("FormElement_Enabled", "PropertyCatalog_Normal", Description = "FormElement_Enabled_Description", XmlNodeName = "Enabled")]
        [PropertyBooleanEditorAttribute()]
        public bool Enabled
        {
            get
            {
                return this._enabled;
            }
            set
            {
                this._enabled = value;
            }
        }

        private bool _visible = true;
        /// <summary>
        /// 可见
        /// </summary>
        [DefaultValue(true)]
        [CorePropertyRelator("FormElement_Visible", "PropertyCatalog_Normal", Description = "FormElement_Visible_Description", XmlNodeName = "Visible")]
        [PropertyBooleanEditorAttribute()]
        public bool Visible
        {
            get
            {
                return this._visible;
            }
            set
            {
                this._visible = value;
                
            }
        }

        /// <summary>
        /// 宽度
        /// </summary>
        [DefaultValue(100)]
        [CorePropertyRelator("FormElement_Width", "PropertyCatalog_Layout", Description = "FormElement_Width_Description", XmlNodeName = "Width")]
        [PropertyTextBoxEditorAttribute(TypeCode = TypeCode.Int32, AllowEmpty = false)]
        public int Width
        {
            get
            {
                return this.Size.Width;
            }
            set
            {
                this.Size = new Size(value, this.Size.Height);
            }
        }

        /// <summary>
        /// 高度
        /// </summary>
        [DefaultValue(21)]
        [CorePropertyRelator("FormElement_Height", "PropertyCatalog_Layout", Description = "FormElement_Height_Description", XmlNodeName = "Height")]
        [PropertyTextBoxEditorAttribute(TypeCode = TypeCode.Int32, AllowEmpty = false)]
        public int Height
        {
            get
            {
                return this.Size.Height;
            }
            set
            {
                this.Size = new Size(this.Size.Width,value);
            }
        }

        private Size _size = new Size(100,21);
        /// <summary>
        /// 大小
        /// </summary>
        public Size Size
        {
            get
            {
                return this._size;
            }
            set
            {
                this._size = value;
            }
        }

        private UIElementAnchor _anchor = new UIElementAnchor();
        /// <summary>
        /// 边缘锚定
        /// </summary>
        //因为元数据中的值必须是常量，所以不能是new ElementAnchor()，const ElementAnchor anchor = new ElementAnchor(); 也不行
        //引用类型的常量不能用 new object() 来初始化
        //这就要求ElementAnchor必须实现 IConvertible 接口，以供  Convert.ChangeType 能把ElementAnchor转成字符串以和DefaultValue对比
        //或显示调用 ElementAnchor.Equals ，因为ElementAnchor.Equals已经重写了
        [DefaultValue("Top,Left")]  
        [CorePropertyRelator("FormElement_Anchor", "PropertyCatalog_Layout", Description = "FormElement_Anchor_Description", XmlNodeName = "Anchor")]
        [PropertyAnchorEditorAttribute()]
        public UIElementAnchor Anchor
        {
            get
            {
                return this._anchor;
            }
            set
            {
                this._anchor = value;
            }
        }

        [DefaultValue(0)]
        [CorePropertyRelator("FormElement_Top", "PropertyCatalog_Layout", Description = "FormElement_Top_Description", XmlNodeName = "Top")]
        [PropertyTextBoxEditorAttribute(TypeCode = TypeCode.Int32, AllowEmpty = false)]
        public int Top
        {
            get
            {
                return this.Location.Y;
            }
            set
            {
                this.Location = new Point(this.Location.X, value);
            }
        }

        [DefaultValue(0)]
        [CorePropertyRelator("FormElement_Left", "PropertyCatalog_Layout", Description = "FormElement_Left_Description", XmlNodeName = "Left")]
        [PropertyTextBoxEditorAttribute(TypeCode = TypeCode.Int32, AllowEmpty = false)]
        public int Left
        {
            get
            {
                return this.Location.X;
            }
            set
            {
                this.Location = new Point(value, this.Location.Y);
            }
        }

        private UIElementFont _font ;
        /// <summary>
        /// 字体,
        /// 注意! 默认值为null,为了能使子窗体控件能继承其容器的字体设置
        /// </summary>
        [CorePropertyRelator("FormElement_Font", "PropertyCatalog_Style", Description = "FormElement_Font_Description", CloneValue = true, XmlNodeName = "Font")]
        [PropertyFontEditorAttribute()]
        public UIElementFont Font
        {
            get { return this._font; }
            set
            {
                if (value != null)
                    this._font = value;
                else
                    this._font = null;
            }
        }

        private Point _location = new Point();
        /// <summary>
        /// 位置
        /// </summary>
        public Point Location
        {
            get
            {
                return this._location;
            }
            set
            {
                this._location = value;
            }
        }

        /// <summary>
        /// 获取元素的代码全路径
        /// 主要用于数据列表中的数据列
        /// 可以获取,数据列表code.数据列code
        /// 一般元素等价于Code属性
        /// 需要注意的一点，数据列控件，在windows窗体上直接continer是出不来的，必须先找到datagrid，再通过cells属性去找
        /// 所以要用到数据列的地方，必须是fullcode，带上dataGridview的code
        /// 最好在所有的地方都用fullcode，兼容性比较好，因为一般元素,fullcode等同于code
        /// </summary>
        public virtual string FullCode
        {
            get
            {
                return this.Code;
            }
        }

        /// <summary>
        /// 获取元素的名称全路径
        /// </summary>
        public virtual string FullName
        {
            get
            {
                return this.Name;
            }
        }

        private int _tabIndex = 0;
        public int TabIndex
        {
            get { return this._tabIndex; }
            set { this._tabIndex = value; }
        }

        private int _zOrder = 0;
        public int ZOrder
        {
            get { return this._zOrder; }
            set { this._zOrder = value; }
        }

        //这个要考虑粘贴后不在原父元素上的情况
        private string _parentId;
        /// <summary>
        /// 此窗体元素的父元素Id
        /// </summary>
        public string ParentId
        {
            get
            {
                return this._parentId;
            }
            set
            {
                this._parentId = value;
            }
        }

        #endregion

        #region 构造

        public UIElement()
        {
            this.XmlRootName = "Element";
        }

        #endregion

        #region 处理序列化与反序列化

        [OnDeserialized]
        void AfterDeserialized(StreamingContext ctx)
        {
            this.EventTypesAdapter = Sheng.SailingEase.Core.EventTypes.Instance;
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 根据xml字串构建元素对象
        /// 这里完成共有部分，其余由继承的类具体实现
        /// </summary>
        /// <param name="strXml"></param>
        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);

            SEXElement xmlDoc = SEXElement.Parse(strXml);

            //设置参数
            this.Enabled = xmlDoc.GetInnerObject<bool>("/Enabled", true);
            this.Visible = xmlDoc.GetInnerObject<bool>("/Visible", true);
            this.Size = new Size(xmlDoc.GetInnerObject<int>("/Width", 0), xmlDoc.GetInnerObject<int>("/Height", 0));

            //FormXml的时候不需要ControlType，因为在调用FormXml的时候，对象必然已经被实例化，对象类型已知了
            //this.ControlType = FormElementEntityTypes.GetProvideAttribute(xmlDoc.GetInnerObject<int>(XmlRootName + "/ControlType", -0));
            this.Text = xmlDoc.GetInnerObject("/Text");

            this.BackColorValue = xmlDoc.GetAttributeObject("/BackColorValue/Color","Value");
            this.ForeColorValue = xmlDoc.GetAttributeObject("/ForeColorValue/Color", "Value");

            this.TabIndex = xmlDoc.GetInnerObject<int>("/TabIndex", 0);
            this.ZOrder = xmlDoc.GetInnerObject<int>("/ZOrder", 0);

            //有些窗体元素没有这个属性,比如工具栏项目,就用默认
            this.Location = new Point(xmlDoc.GetInnerObject<int>("/Left", 0), xmlDoc.GetInnerObject<int>("/Top", 0));

            //设置锚定部分
            //有些窗体元素没有这个属性,比如工具栏项目,就用默认
            this.Anchor.Top = xmlDoc.GetInnerObject<bool>("/Anchor/Top", true);
            this.Anchor.Bottom = xmlDoc.GetInnerObject<bool>("/Anchor/Bottom", false);
            this.Anchor.Left = xmlDoc.GetInnerObject<bool>("/Anchor/Left", true);
            this.Anchor.Right = xmlDoc.GetInnerObject<bool>("/Anchor/Right", false);

            if (xmlDoc.SelectSingleNode("/Font").HasElements)
            {
                this.Font = new UIElementFont();
                this.Font.FromXml(xmlDoc.GetOuterXml("/Font"));
            }

           

            //添加事件对象
            EventBase eventBase;
            foreach (XElement eventNode in xmlDoc.SelectNodes("/Events/Event"))
            {
                eventBase = this.EventTypesAdapter.CreateInstance(Convert.ToInt32(eventNode.Attribute("EventCode").Value));
                eventBase.FromXml(eventNode.ToString());
                this.Events.Add(eventBase);
            }
        }

        /// <summary>
        /// 获取无素的完整xml形式
        /// 这里完成共有部分，其余由继承的类具体实现
        /// </summary>
        /// <returns></returns>
        public override string ToXml()
        {
            SEXElement xmlDoc = SEXElement.Parse(base.ToXml());

            xmlDoc.AppendChild(String.Empty, "ControlType", UIElementEntityTypes.Instance.GetProvideAttribute(this).Code);
            xmlDoc.AppendChild(String.Empty, "Text", this.Text);
            xmlDoc.AppendChild("BackColorValue");
            xmlDoc.AppendInnerXml("/BackColorValue", new UIElementColor(this.BackColorValue).ToXml());
            xmlDoc.AppendChild("ForeColorValue");
            xmlDoc.AppendInnerXml("/ForeColorValue", new UIElementColor(this.ForeColorValue).ToXml());
            xmlDoc.AppendChild(String.Empty, "Enabled", this.Enabled);
            xmlDoc.AppendChild(String.Empty, "Visible", this.Visible);
            xmlDoc.AppendChild(String.Empty, "Top", this.Location.Y);
            xmlDoc.AppendChild(String.Empty, "Left", this.Location.X);
            xmlDoc.AppendChild(String.Empty, "Width", this.Width);
            xmlDoc.AppendChild(String.Empty, "Height", this.Height);
            xmlDoc.AppendChild(String.Empty, "TabIndex", this.TabIndex);
            xmlDoc.AppendChild(String.Empty, "ZOrder", this.ZOrder);
            xmlDoc.AppendChild("Anchor");
            xmlDoc.AppendInnerXml("/Anchor", this.Anchor.GetXml());

            //字体
            if (this.Font != null)
                xmlDoc.AppendInnerXml(this.Font.ToXml());
            else
                xmlDoc.AppendInnerXml("<Font/>");

            //事件
            xmlDoc.AppendChild("Events");

            foreach (EventBase even in this.Events)
            {
                xmlDoc.AppendInnerXml("/Events", even.ToXml());
            }

            return xmlDoc.ToString() ;
        }

        /// <summary>
        /// 获取不可分隔的内部元素集合
        /// 注意这不是用来获取例如panel这样容器控件的子控件的
        /// 而是如数据列表这样的包含不可分隔的数据列的元素
        /// 对于数据列表,此方法就返回数据列对象集合
        /// InnerElement不能独立存在,必须存在于所依赖的宿主元素
        /// 如果不存在这样不可分隔的内部元素集合，就返回一个空的FormElementCollection
        /// </summary>
        /// <returns></returns>
        public virtual UIElementCollection GetInnerElement()
        {
            return new UIElementCollection(this.HostFormEntity);
        }

        #endregion

        #region IEventSupport 成员

        [field: NonSerialized]
        public event OnEventUpdatedHandler EventUpdated;

        private EventCollection _events;
        public EventCollection Events
        {
            get
            {

                if (this._events == null)
                {
                    this._events = new EventCollection(this.HostFormEntity, this);
                }

                //因为反序列化后会丢失HostEntity，所以这里做一个判断来保证HostEntity
                //FormEntity这里保证不了，这里本身也不知道FormEntity
                //所以FormEntity通过实现了IShellControlDev接口的Entity属性来保证
                if (this._events.HostEntity == null || this._events.HostEntity.Equals(this) == false)
                {
                    this._events.HostEntity = this;
                }

                return this._events;
            }
            set
            {
                this._events = value;
            }
        }

        //这几个虚方法要提供一下默认实现
        //因为不一定每个从这个类继承的工具栏对象都会重写这几个属性或者方法

        public virtual EventTypeCollection EventProvide
        {
            get { return new EventTypeCollection(); }
        }

        public virtual List<EventTimeAbstract> EventTimeProvide
        {
            get { return new List<EventTimeAbstract>(); }
        }

        public virtual string GetEventTimeName(int code)
        {
            return String.Empty;
        }

        public void EventUpdate(object sender)
        {
            if (this.EventUpdated != null)
                EventUpdated(sender, this);
        }

        #endregion
    }
}
