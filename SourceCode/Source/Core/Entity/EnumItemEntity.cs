/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Sheng.SailingEase.Kernal;
using System.Xml.Linq;
namespace Sheng.SailingEase.Core
{
    public class EnumItemEntity:EntityBase
    {
        public EnumEntity Owner
        {
            get;
            set;
        }
        private string _text = String.Empty;
        public string Text
        {
            get
            {
                return this._text;
            }
            set
            {
                this._text = value;
                this.Name = value;
            }
        }
        private string _value = String.Empty;
        public string Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }
        public EnumItemEntity(EnumEntity owner)
        {
            this.XmlRootName = "Item";
            Owner = owner;
        }
        public override void FromXml(string strXml)
        {
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.Id = xmlDoc.GetAttributeObject("Id");
            this.Text = xmlDoc.GetAttributeObject("Text");
            this.Value = xmlDoc.GetAttributeObject("Value");
        }
        public override string ToXml()
        {
            XElement xmlDoc = new XElement(XmlRootName,
                new XAttribute("Id",this.Id),
                new XAttribute("Text",this.Text),
                new XAttribute("Value",this.Value));
            return xmlDoc.ToString();
        }
    }
}
