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
namespace Sheng.SailingEase.ComponentModel
{
   
    [Serializable]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PropertyRelatorAttribute : Attribute
    {
        private string _xmlNodeName;
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
        public virtual string PropertyDisplayName
        {
            get;
            set;
        }
        public virtual string Catalog
        {
            get;
            set;
        }
        public virtual string Description
        {
            get;
            set;
        }
        private bool _cloneValue = false;
        public bool CloneValue
        {
            get { return _cloneValue; }
            set { _cloneValue = value; }
        }
        private EunmPropertyVisibility _visibility = EunmPropertyVisibility.ANY;
        public EunmPropertyVisibility Visibility
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
        private bool _readOnly = false;
        public bool ReadOnly
        {
            get { return _readOnly; }
            set { _readOnly = value; }
        }
        public PropertyRelatorAttribute(string propertyDisplayName, string catalog)
        {
            this.XmlNodeName = _xmlNodeName;
            this.PropertyDisplayName = propertyDisplayName;
            this.Catalog = catalog;
        }
    }
}
