/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;
using System.ComponentModel;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.ComponentModel;
using System.Xml.Linq;
namespace Sheng.SailingEase.Core
{
    [Serializable]
    public class UIElementFont : BaseObject, IXmlable
    {
        public UIElementFont()
        {
        }
        public Font Font
        {
            get
            {
                FontStyle style = new FontStyle();
                if (this.Bold)
                    style = style | FontStyle.Bold;
                if (this.Italic)
                    style = style | FontStyle.Italic;
                if (this.Underline)
                    style = style | FontStyle.Underline;
                System.Drawing.Font font = new Font(this.FontFamily, this.Size, style, GraphicsUnit.Point);
                return font;
            }
            set
            {
                this.FontFamily = value.FontFamily.Name;
                this.Bold = value.Bold;
                this.Italic = value.Italic;
                this.Size = value.Size;
                this.Underline = value.Underline;
            }
        }
        private string _fontFamily = SystemFonts.DefaultFont.FontFamily.Name;
        [CorePropertyRelator("ElementFont_FontFamily", "PropertyCatalog_Style", Description = "ElementFont_FontFamily_Description", XmlNodeName = "FontFamily")]
        [PropertyTextBoxEditorAttribute(AllowEmpty = false)]
        public string FontFamily
        {
            get { return this._fontFamily; }
            set { this._fontFamily = value; }
        }
        private bool _bold = SystemFonts.DefaultFont.Bold;
        [CorePropertyRelator("ElementFont_Bold", "PropertyCatalog_Style", Description = "ElementFont_Bold_Description", XmlNodeName = "Bold")]
        [PropertyBooleanEditorAttribute()]
        public bool Bold
        {
            get { return this._bold; }
            set { this._bold = value; }
        }
        private bool _italic = SystemFonts.DefaultFont.Italic;
        [CorePropertyRelator("ElementFont_Italic", "PropertyCatalog_Style", Description = "ElementFont_Italic_Description", XmlNodeName = "Italic")]
        [PropertyBooleanEditorAttribute()]
        public bool Italic
        {
            get { return this._italic; }
            set { this._italic = value; }
        }
        private float _size = SystemFonts.DefaultFont.Size;
        [CorePropertyRelator("ElementFont_Size", "PropertyCatalog_Style", Description = "ElementFont_Size_Description", XmlNodeName = "Size")]
        [PropertyTextBoxEditorAttribute(TypeCode = TypeCode.Single, AllowEmpty = false)]
        public float Size
        {
            get { return this._size; }
            set { this._size = value; }
        }
        private bool _underline = SystemFonts.DefaultFont.Underline;
        [CorePropertyRelator("ElementFont_Underline", "PropertyCatalog_Style", Description = "ElementFont_Underline_Description", XmlNodeName = "Underline")]
        [PropertyBooleanEditorAttribute()]
        public bool Underline
        {
            get { return this._underline; }
            set { this._underline = value; }
        }
        public string ToXml()
        {
            XElement xmlDoc = new XElement("Font",
                new XElement("FontFamily", this.FontFamily),
                new XElement("Bold", this.Bold),
                new XElement("Italic", this.Italic),
                new XElement("Size", this.Size),
                new XElement("Underline", this.Underline));
            return xmlDoc.ToString();
        }
        public void FromXml(string strXml)
        {
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.FontFamily = xmlDoc.GetInnerObject("FontFamily");
            this.Bold = xmlDoc.GetInnerObject("Bold", false);
            this.Italic = xmlDoc.GetInnerObject("Italic", false);
            this.Size = xmlDoc.GetInnerObject<float>("Size", 11);
            this.Underline = xmlDoc.GetInnerObject("Underline", false);
        }
        public override string ToString()
        {
            if (FontFamily == String.Empty)
                return String.Empty;
            return this.FontFamily + " " + this.Size.ToString();
        }
        public override bool Equals(object obj)
        {
            if (!(obj is UIElementFont))
            {
                return false;
            }
            UIElementFont temp = (UIElementFont)obj;
            return this.ToXml().Equals(temp.ToXml());
        }
    }
}
