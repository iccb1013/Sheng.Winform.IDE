/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Sheng.SailingEase.Drawing;
using Sheng.SailingEase.Kernal;
using System.Reflection;
using System.Xml.Linq;
namespace Sheng.SailingEase.Core
{
    class UIElementColor : IXmlable
    {
        public string Value
        {
            get
            {
                if (_color == Color.Empty)
                    return String.Empty;
                else
                    return ((int)_type).ToString() + "." + this._name + "." + _color.ToArgb();
            }
        }
        private ChooseColorType _type = ChooseColorType.Custom;
        public ChooseColorType Type
        {
            get { return _type; }
            set { _type = value; }
        }
        private string _name = String.Empty;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private Color _color = Color.Empty;
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }
        public int A
        {
            get { return _color.A; }
        }
        public int R
        {
            get { return _color.R; }
        }
        public int G
        {
            get { return _color.G; }
        }
        public int B
        {
            get { return _color.B; }
        }
        public UIElementColor()
        {
        }
        public UIElementColor(string colorValueString)
        {
            if (colorValueString == null || colorValueString == String.Empty)
            {
                _color = Color.Empty;
            }
            else
            {
                string[] strArray = colorValueString.Split('.');
                ChooseColorType type =
                    (ChooseColorType)Convert.ToInt32(strArray[0]);
                Color color = Color.Empty;
                switch (type)
                {
                    case ChooseColorType.Custom:
                        color = Color.FromArgb(Convert.ToInt32(strArray[2]));
                        break;
                    case ChooseColorType.Define:
                        color = Color.FromArgb(Convert.ToInt32(strArray[2]));
                        break;
                    case ChooseColorType.System:
                        Type typeSystemColors = typeof(SystemColors);
                        PropertyInfo p = typeSystemColors.GetProperty(strArray[1]);
                        color = (Color)p.GetValue(typeSystemColors, null);
                        break;
                }
                _type = type;
                _name = strArray[1];
                _color = color;
            }
        }
        public UIElementColor(ChooseColorType type, string name, Color color)
        {
            _type = type;
            _name = name;
            _color = color;
        }
        public void FromXml(string strXml)
        {
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this._name = xmlDoc.GetAttributeObject("Name");
            this._type = (ChooseColorType)xmlDoc.GetAttributeObject<int>("Type", 0);
            int a = xmlDoc.GetAttributeObject<int>("A", 0);
            int r = xmlDoc.GetAttributeObject<int>("R", 0);
            int g = xmlDoc.GetAttributeObject<int>("G", 0);
            int b = xmlDoc.GetAttributeObject<int>("B", 0);
            this._color = Color.FromArgb(a, r, g, b);
        }
        public string ToXml()
        {
            XElement xmlDoc = new XElement("Color",
                new XAttribute("Value",this.Value),
                new XAttribute("Name",this.Name),
                new XAttribute("Type",(int)this.Type),
                new XAttribute("A",this.A),
                new XAttribute("R",this.R),
                new XAttribute("G",this.G),
                new XAttribute("B",this.B));
            return xmlDoc.ToString();
        }
    }
}
