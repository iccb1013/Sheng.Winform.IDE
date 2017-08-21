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
using System.Xml.Linq;
using System.ComponentModel;
using Sheng.SailingEase.IDataBaseProvide;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Core
{
    public class DataItemEntity : EntityBase
    {
        public override string Code
        {
            get
            {
                return base.Code;
            }
            set
            {
                base.Code = value;
                if (this.Field != null)
                    this.Field.Name = value;
            }
        }
        public DataEntity Owner
        {
            get;
            set;
        }
        private IField _field;
        public IField Field
        {
            get { return _field; }
            set
            {
                _field = value;
                if (_field != null)
                    _field.Name = this.Code;
            }
        }
        public FieldLength Length
        {
            get { return this.Field.Length; }
            set { this.Field.Length = value; }
        }
        public byte DecimalDigits
        {
            get { return this.Field.Length.DecimalDigits; }
            set { this.Field.Length.DecimalDigits = value; }
        }
        private string _defaultValue = String.Empty;
        [DefaultValue(StringUnity.EmptyString)]
        [CorePropertyRelator("DataItemEntity_DefaultValue", "PropertyCatalog_Normal", Description = "DataItemEntity_DefaultValue_Description", XmlNodeName = "Text")]
        [PropertyTextBoxEditorAttribute()]
        public string DefaultValue
        {
            get
            {
                return this._defaultValue;
            }
            set
            {
                this._defaultValue = value;
                if (this.Field != null)
                    this.Field.DefaultValue = value;
            }
        }
        private bool _allowEmpty;
        [DefaultValue(true)]
        [CorePropertyRelator("DataItemEntity_AllowEmpty", "PropertyCatalog_Normal", Description = "DataItemEntity_AllowEmpty_Description", XmlNodeName = "Enabled")]
        [PropertyBooleanEditorAttribute()]
        public bool AllowEmpty
        {
            get
            {
                return this._allowEmpty;
            }
            set
            {
                this._allowEmpty = value;
                if (this.Field != null)
                    this.Field.AllowEmpty = value;
            }
        }
        private bool _exclusive;
        public bool Exclusive
        {
            get
            {
                return this._exclusive;
            }
            set
            {
                this._exclusive = value;
            }
        }
        public DataItemEntity(DataEntity owner)
        {
            this.XmlRootName = "Item";
            this.Owner = owner;
        }
        private XElement GetLengthXml()
        {
            XElement xml = new XElement("Length", this.Field.Length.ToString());
            xml.Add(new XAttribute("DecimalDigits", this.Field.Length.DecimalDigits));
            return xml;
        }
        private void SetLengthXml(XElement xml)
        {
            if (xml.Value.ToUpper() == "MAX")
            {
                this.Field.Length.Max = true;
            }
            else
            {
                this.Field.Length.Length = Convert.ToInt32(xml.Value);
            }
            this.Field.Length.DecimalDigits = Convert.ToByte(xml.Attribute("DecimalDigits").Value);
        }
        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.Field = DataBaseProvide.Current.FieldFactory.CreateInstance(xmlDoc.GetAttributeObject<int>("ItemCode", 0));
            SetLengthXml(xmlDoc.SelectSingleNode("Length"));
            this.DefaultValue = xmlDoc.GetInnerObject("DefaultValue");
            this.AllowEmpty = xmlDoc.GetInnerObject<bool>("AllowEmpty", true);
            this.Exclusive = xmlDoc.GetInnerObject<bool>("Exclusive", false);
        }
        public override string ToXml()
        {
            XElement xml = XElement.Parse(base.ToXml());
            xml.Add(new XAttribute("ItemCode", DataBaseProvide.Current.FieldFactory.GetProvideAttribute(this.Field).Code));
            xml.Add(GetLengthXml(),
                new XElement("DefaultValue", this.DefaultValue),
                new XElement("AllowEmpty", this.AllowEmpty),
                new XElement("Exclusive", this.Exclusive));
            return xml.ToString();
        }
    }
}
