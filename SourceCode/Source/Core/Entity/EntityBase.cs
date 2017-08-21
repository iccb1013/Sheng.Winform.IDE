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
using System.Diagnostics;
using System.ComponentModel;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.Kernal;
using System.Xml.Linq;
namespace Sheng.SailingEase.Core
{
    /*
     * �������������������У���ΪNameProperty������������ʽ��WPF�������Ե�������ʽ��ͻ
     * public const string NameProperty = "Name";
     * ��Ϊ Property_Name
     */
    [Serializable]
    public abstract class EntityBase : BaseObject, IXmlable, INotifyPropertyChanged, IPropertyModel
    {
        private string _xmlRootName;
        protected string XmlRootName
        {
            get
            {
                return this._xmlRootName;
            }
            set
            {
                this._xmlRootName = value;
            }
        }
        public const string Property_Id = "Id";
        private string _id = Guid.NewGuid().ToString();
#if DEBUG
        [CorePropertyRelator("EntityBase_Name", "PropertyCatalog_Normal", XmlNodeName = "Id", Visibility = EunmPropertyVisibility.Single)]
        [PropertyTextBoxEditorAttribute()]
#endif
        public string Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Id")); }
            }
        }
        public const string Property_Name = "Name";
        private string _name;
        [CorePropertyRelator("EntityBase_Name", "PropertyCatalog_Normal", XmlNodeName = "Name", Visibility = EunmPropertyVisibility.Desinger)]
        [PropertyTextBoxEditorAttribute()]
        [DefaultValue(StringUnity.EmptyString)]
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Name")); }
            }
        }
        public const string Property_Code = "Code";
        private string _code;
        [CorePropertyRelator("EntityBase_Code", "PropertyCatalog_Normal", Description = "EntityBase_Code_Description", XmlNodeName = "Code",
            Visibility = EunmPropertyVisibility.Single | EunmPropertyVisibility.Desinger)]
        [PropertyTextBoxEditorAttribute(Regex = CoreConstant.ENTITY_CODE_REGEX, RegexMsg = "EntityBase_Code_RegexMsg", AllowEmpty = false)]
        [DefaultValue(StringUnity.EmptyString)]
        public virtual string Code
        {
            get
            {
                return this._code;
            }
            set
            {
                this._code = value;
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Code")); }
            }
        }
        public const string Property_Remark = "Remark";
        private string _remark;
        [CorePropertyRelator("EntityBase_Remark", "PropertyCatalog_Other", Description = "EntityBase_Remark_Description", XmlNodeName = "Remark")]
        [PropertyTextBoxEditorAttribute(MaxLength = 1000)]
        public string Remark
        {
            get
            {
                return this._remark;
            }
            set
            {
                this._remark = value;
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Remark")); }
            }
        }
        private bool _system = false;
        public bool Sys
        {
            get
            {
                return this._system;
            }
            set
            {
                this._system = value;
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Sys")); }
            }
        }
        public virtual string ToXml()
        {
            XElement xmlDoc = new XElement(XmlRootName,
                new XAttribute("Id", this.Id),
                new XAttribute("Name", this.Name),
                new XAttribute("Code", this.Code),
                new XAttribute("Sys", this.Sys),
                new XElement("Remark", this.Remark));
            return xmlDoc.ToString();
        }
        public virtual void FromXml(string strXml)
        {
            SEXElement xmlDoc = SEXElement.Parse(strXml);
            this.Id = xmlDoc.GetAttributeObject("Id");
            this.Name = xmlDoc.GetAttributeObject("Name");
            this.Code = xmlDoc.GetAttributeObject("Code");
            this.Sys = xmlDoc.GetAttributeObject<bool>("Sys", false);
            this.Remark = xmlDoc.GetInnerObject("/Remark");            
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public bool PropertyReadOnly
        {
            get { return this.Sys; }
        }
    }
}
