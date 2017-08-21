using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.Controls;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Drawing;
using System.Xml.Linq;

namespace Sheng.SailingEase.Core
{
    //FormEntity不需要序列化
    //目前情况下，如果运行时抛异常说FormEntity需要序列化，必然是程序Bug有地方没处理好
    //窗体元素实体对象需要序列化的原因是DesignSurface在处理复制，粘贴时需要序列化对象，而Form不存在这个问题
    //[Serializable]
    [DesignerHostEntityAttribute]
    public class WindowEntity : EntityBase, IEventSupport
    {
        public string ControlTypeName
        {
            get
            {
                //TODO:资源
                return "窗体";
            }
        }

        #region 受保护的属性

        [NonSerialized]
        private EventTypesAbstract _eventTypesAdapter = EventTypes.Instance;
        /// <summary>
        /// 事件类型适配器
        /// 在继承的类中设置此属性，可使FromXml方法初始化相应的类型，如IDE中初始化DEV结尾的类型，SHELL中初始化EX结尾的类型
        /// 用Adapter结尾命名是避免和 EventTypes 类混淆和不明确引用
        /// </summary>
        protected EventTypesAbstract EventTypesAdapter
        {
            get { return this._eventTypesAdapter; }
            set { this._eventTypesAdapter = value; }
        }

        [NonSerialized]
        private UIElementEntityTypesAbstract _formElementEntityTypesAdapter = UIElementEntityTypes.Instance;
        /// <summary>
        /// 窗体元素类型适配器
        /// 在继承的类中设置此属性，可使FromXml方法初始化相应的类型，如IDE中初始化DEV结尾的类型，SHELL中初始化EX结尾的类型
        /// 用Adapter结尾命名是避免和 EventTypes 类混淆和不明确引用
        /// </summary>
        protected UIElementEntityTypesAbstract FormElementEntityTypesAdapter
        {
            get { return this._formElementEntityTypesAdapter; }
            set { this._formElementEntityTypesAdapter = value; }
        }

        #endregion

        #region 公开属性

        private string _text = String.Empty;
        /// <summary>
        /// 文字
        /// </summary>
        [DefaultValue(StringUnity.EmptyString)]
        [CorePropertyRelator("FormEntity_Text", "PropertyCatalog_Normal", Description = "FormEntity_Text_Description", XmlNodeName = "Text")]
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
        [CorePropertyRelator("FormEntity_BackColorValue", "PropertyCatalog_Style", 
            Description = "FormEntity_BackColorValue_Description", XmlNodeName = "BackColorValue")]
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
        [CorePropertyRelator("FormEntity_ForeColorValue", "PropertyCatalog_Style", 
            Description = "FormEntity_ForeColorValue_Description", XmlNodeName = "ForeColorValue")]
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

        private int _width = 300;
        /// <summary>
        /// 窗体的宽度
        /// </summary>
        [DefaultValue(300)]
        [CorePropertyRelator("FormEntity_Width", "PropertyCatalog_Layout", 
            Description = "FormEntity_Width_Description", XmlNodeName = "Width")]
        [PropertyTextBoxEditorAttribute(TypeCode = TypeCode.Int32, AllowEmpty = false)]
        public int Width
        {
            get
            {
                return this._width;
            }
            set
            {
                this._width = value;
            }
        }

        private int _height = 300;
        /// <summary>
        /// 窗体的高度
        /// </summary>
        [DefaultValue(300)]
        [CorePropertyRelator("FormEntity_Height", "PropertyCatalog_Layout", 
            Description = "FormEntity_Height_Description", XmlNodeName = "Height")]
        [PropertyTextBoxEditorAttribute(TypeCode = TypeCode.Int32, AllowEmpty = false)]
        public int Height
        {
            get
            {
                return this._height;
            }
            set
            {
                this._height = value;
            }
        }

        public Size Size
        {
            get
            {
                return new Size(this.Width, this.Height);
            }
            set
            {
                this.Width = value.Width;
                this.Height = value.Height;
            }
        }

        private int _clientWidth;
        /// <summary>
        /// 窗体的内部宽度（去除左右边框后宽度）
        /// </summary>
        public int ClientWidth
        {
            get { return _clientWidth; }
            set { _clientWidth = value; }
        }

        private int _clientHeight;
        /// <summary>
        /// 窗体的内部高度（去除标题栏和下边框后宽度）
        /// </summary>
        public int ClientHeight
        {
            get { return _clientHeight; }
            set { _clientHeight = value; }
        }

        private EnumFormWindowState _windowState = EnumFormWindowState.Normal;
        /// <summary>
        /// 窗体打开时的状态
        /// 对弹出式窗口起作用
        /// </summary>
        [DefaultValue(EnumFormWindowState.Normal)]
        [CorePropertyRelator("FormEntity_WindowState", "PropertyCatalog_Style", 
            Description = "FormEntity_WindowState_Description", XmlNodeName = "WindowState")]
        [PropertyComboBoxEditorAttribute(Enum = typeof(EnumFormWindowState))]
        public EnumFormWindowState WindowState
        {
            get
            {
                return this._windowState;
            }
            set
            {
                this._windowState = value;
            }
        }

        private UIElementFont _font;
        /// <summary>
        /// 字体,没有默认值,为了能使子窗体控件能继承容器的字体设置
        /// </summary>
        [CorePropertyRelator("FormEntity_Font", "PropertyCatalog_Style", 
            Description = "FormEntity_Font_Description", CloneValue = true, XmlNodeName = "Font")]
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

        private string _folderId = String.Empty;
        /// <summary>
        /// 所在文件夹Id
        /// </summary>
        public string FolderId
        {
            get
            {
                return this._folderId;
            }
            set
            {
                this._folderId = value;
            }
        }

        private UIElementCollection _elements;
        /// <summary>
        /// 窗体元素集合
        /// 窗体元素集合必须按ZOrder由大到小的顺序排列
        /// </summary>
        public UIElementCollection Elements
        {
            get
            {
                return this._elements;
            }
            set
            {
                this._elements = value;
            }
        }

        #endregion

        #region 构造

        public WindowEntity()
        {
            this.XmlRootName = "Window";

            this.Elements = new UIElementCollection(this);
            this.Events = new EventCollection(this, this);
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 使用指定前缀生成一个用于窗体元素的code
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public string CreationElementCode(string prefix)
        {
          

            int value = 0;
            //当前编号数组
            List<int> codes = new List<int>();
            //这里需要连同InnerElement一起获取
            foreach (UIElement element in this.GetFormElement(true))
            {
                string name = element.Code;
                if (name.StartsWith(prefix))
                {
                    if (Int32.TryParse(name.Substring(prefix.Length), out value))
                    {
                        codes.Add(value);
                    }
                }
            }

            codes.Sort();

            int lastValue = 0; //上一个值，用于判断是否中间有空缺要补

            foreach (int code in codes)
            {
                if (code - lastValue > 1)
                {
                    break;
                }
                else
                {
                    lastValue = code;
                }
            }

            return prefix + (lastValue + 1);
        }

        /// <summary>
        /// 获取其中的窗体元素
        /// 可能为null
        /// 这一方法是为未来功能预留的,当实现容器控件时,此方法在整个窗体内查找元素,包括容器里的
        /// 支持InnerElement
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UIElement FindFormElementById(string id)
        {
            return this.Elements.GetFormElementById(id);
        }

        /// <summary>
        /// 获取其中的窗体元素
        /// 可能为null
        /// 这一方法是为未来功能预留的,当实现容器控件时,此方法在整个窗体内查找元素,包括容器里的
        /// 支持InnerElement
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public UIElement FindFormElementByCode(string code)
        {
            return this.Elements.GetFormElementByCode(code);
        }

        #region GetFormElement

        //这一方法是为未来功能预留的,当实现容器控件时,此方法获取窗体内所有控件,包括容器里的
        /// <summary>
        /// 获取窗体元素集合
        /// 但不包括 InnerElement
        /// </summary>
        /// <returns></returns>
        public UIElementCollection GetFormElement()
        {
            return GetFormElement(false);
        }

        /// <summary>
        /// 获取窗体元素集合
        /// findInnerElement指定是否包括InnerElement
        /// </summary>
        /// <param name="findInnerElement">是否包括InnerElement</param>
        /// <returns></returns>
        public UIElementCollection GetFormElement(bool findInnerElement)
        {
            UIElementEntityTypeCollection enumFormElementControlTypeCollection =
                new UIElementEntityTypeCollection();

            return GetFormElement(enumFormElementControlTypeCollection, findInnerElement);
        }

        /// <summary>
        /// 包括InnerElement
        /// </summary>
        /// <param name="controlType"></param>
        /// <returns></returns>
        public UIElementCollection GetFormElement(UIElementEntityTypeCollection controlType)
        {
            return GetFormElement(controlType, true);
        }

        public UIElementCollection GetFormElement(UIElementEntityTypeCollection controlType, bool findInnerElement)
        {
            UIElementCollection collection = new UIElementCollection(this);

            foreach (UIElement formElement in this.Elements)
            {
                if (controlType.Allowable(formElement))
                {
                    collection.Add(formElement);
                }

                if (findInnerElement)
                {
                    //查找InnerElement
                    foreach (UIElement innerElement in formElement.GetInnerElement())
                    {
                        if (controlType.Allowable(innerElement) == false)
                            continue;

                        collection.Add(innerElement);
                    }
                }
            }

            return collection;
        }

        #endregion

        #region ValidateCode

        /// <summary>
        /// 检查指定的代码在指定的窗体元素中是否可用
        /// 包括InnerElement
        /// 若已被占用,包括窗体元素本身,则无效
        /// 可用返回true
        /// </summary>
        /// <param name="code"></param>
        /// <param name="formEntity"></param>
        /// <returns></returns>
        public bool ValidateCode(string code)
        {
            return ValidateCode(code, null);
        }

        public bool ValidateCode(string code, string ignoreId)
        {
            if (ignoreId != null)
            {
                if (this.Id == ignoreId)
                    return true;
            }

            if (this.Code == code)
                return false;

            foreach (UIElement element in this.GetFormElement(true))
            {
                if (ignoreId != null)
                {
                    if (element.Id == ignoreId)
                        continue;
                }

                if (element.Code == code)
                    return false;
            }

            return true;
        }

        #endregion

        #region IXmlable

        /// <summary>
        /// 由XML构造对象
        /// </summary>
        /// <param name="strXml"></param>
        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);

            SEXElement xmlDoc = SEXElement.Parse(strXml);

            this.Width = xmlDoc.GetInnerObject<int>("/Width", 0);
            this.Height = xmlDoc.GetInnerObject<int>("/Height", 0);
            this.ClientWidth = xmlDoc.GetInnerObject<int>("/ClientWidth", 0);
            this.ClientHeight = xmlDoc.GetInnerObject<int>("/ClientHeight", 0);
            this.WindowState = (EnumFormWindowState)xmlDoc.GetInnerObject<int>("/OpenState", 0);
            this.FolderId = xmlDoc.GetInnerObject("/Folder");

            this.Text = xmlDoc.GetInnerObject("/Text");
            this.BackColorValue = xmlDoc.GetAttributeObject("/BackColorValue/Color", "Value");
            this.ForeColorValue = xmlDoc.GetAttributeObject("/ForeColorValue/Color", "Value");

            if (xmlDoc.SelectSingleNode("/Font").HasElements)
            {
                this.Font = new UIElementFont();
                this.Font.FromXml(xmlDoc.GetOuterXml("/Font"));
            }

            //添加动作对象
            EventBase eventBase;
            foreach (XElement eventNode in xmlDoc.SelectNodes("/Events/Event"))
            {
                eventBase = this.EventTypesAdapter.CreateInstance(Convert.ToInt32(eventNode.Attribute("EventCode").Value));
                eventBase.FromXml(eventNode.ToString());
                eventBase.HostFormEntity = this;
                this.Events.Add(eventBase);
            }

            //添加元素对象
            UIElement formElement;
            foreach (XElement elementNode in xmlDoc.SelectNodes("/Elements/Element"))
            {
                formElement = (UIElement)this.FormElementEntityTypesAdapter.CreateInstance(
                    Convert.ToInt32(elementNode.Element("ControlType").Value));
                Debug.Assert(formElement != null, "创建 FormEntity 时,FormElement 没创建出来");
                formElement.FromXml(elementNode.ToString());
                formElement.HostFormEntity = this;
                this.Elements.Add(formElement);
            }
        }

        public override string ToXml()
        {
            SEXElement xmlDoc = SEXElement.Parse(base.ToXml());
            //XmlNode xmlnode;

            xmlDoc.AppendChild(String.Empty, "Folder", this.FolderId);
            xmlDoc.AppendChild(String.Empty, "Width", this.Width);
            xmlDoc.AppendChild(String.Empty, "Height", this.Height);
            xmlDoc.AppendChild(String.Empty, "ClientWidth", this.ClientWidth);
            xmlDoc.AppendChild(String.Empty, "ClientHeight", this.ClientHeight);
            xmlDoc.AppendChild(String.Empty, "OpenState", (int)this.WindowState);
            xmlDoc.AppendChild(String.Empty, "Text", this.Text);
            xmlDoc.AppendChild("BackColorValue");
            xmlDoc.AppendInnerXml("/BackColorValue", new UIElementColor(this.BackColorValue).ToXml());
            xmlDoc.AppendChild("ForeColorValue");
            xmlDoc.AppendInnerXml("/ForeColorValue", new UIElementColor(this.ForeColorValue).ToXml());
            xmlDoc.AppendChild(String.Empty, "Remark", this.Remark);

            if (this.Font != null)
                xmlDoc.AppendInnerXml(this.Font.ToXml());
            else
                xmlDoc.AppendInnerXml("<Font/>");

            xmlDoc.AppendChild("Events");
            xmlDoc.AppendChild("Elements");

            foreach (EventBase eve in this.Events)
            {
                xmlDoc.AppendInnerXml("/Events", eve.ToXml());
            }

            //对窗体元素的ZOrder进行排序
            var elements = from c in this.Elements orderby c.ZOrder ascending select c;

            foreach (UIElement formElement in elements)
            {

                xmlDoc.AppendInnerXml("/Elements", formElement.ToXml());
            }

            return xmlDoc.ToString();
        }

        #endregion

        #endregion

        #region IEventSupport 成员

        public event OnEventUpdatedHandler EventUpdated;

        private EventCollection events;
        public EventCollection Events
        {
            get
            {
                return this.events;
            }
            set
            {
                this.events = value;
            }
        }

        public virtual EventTypeCollection EventProvide
        {
            get { throw new NotImplementedException(); }
        }

        private static EventTimes _eventTimes;
        public List<EventTimeAbstract> EventTimeProvide
        {
            get
            {
                if (_eventTimes == null)
                {
                    _eventTimes = new EventTimes();
                }

                return _eventTimes.Times;
            }
        }

        public string GetEventTimeName(int code)
        {
            if (_eventTimes == null)
            {
                _eventTimes = new EventTimes();
            }

            return _eventTimes.GetEventName(code);
        }

        public void EventUpdate(object sender)
        {
            if (this.EventUpdated != null)
                EventUpdated(sender,this);
        }

        #endregion

        #region EventTimes

        public class EventTimes : EventTimesAbstract
        {
            public EventTimes()
            {
                _times = new List<EventTimeAbstract>();
                _times.Add(new Load());
                _times.Add(new Closing());
                _times.Add(new Click());
            }

            public class Load : EventTimeAbstract
            {
                public static int XCode
                {
                    get { return 1; }
                }

                public override int Code
                {
                    get { return XCode; }
                }

                public override string Name
                {
                    get { return "载入"; }
                }
            }

            public class Closing : EventTimeAbstract
            {
                public static int XCode
                {
                    get { return 2; }
                }

                public override int Code
                {
                    get { return XCode; }
                }

                public override string Name
                {
                    get { return "关闭"; }
                }
            }

            public class Click : EventTimeAbstract
            {
                public static int XCode
                {
                    get { return 3; }
                }

                public override int Code
                {
                    get { return XCode; }
                }

                public override string Name
                {
                    get { return "单击"; }
                }
            }
        }

        #endregion

        #region 枚举

        /// <summary>
        /// 指定窗体窗口如何显示。
        /// 与System.Windows.Forms下FormWindowState同步
        /// </summary>
        public enum EnumFormWindowState
        {
            /// <summary>
            /// 默认大小的窗口
            /// </summary>
            [LocalizedDescription("EnumFormWindowState_Normal")]
            Normal = 0,
            /// <summary>
            /// 最小化的窗口
            /// </summary>
            [LocalizedDescription("EnumFormWindowState_Minimized")]
            Minimized = 1,
            /// <summary>
            ///  最大化的窗口
            /// </summary>
            [LocalizedDescription("EnumFormWindowState_Maximized")]
            Maximized = 2
        }

        #endregion
    }
}
