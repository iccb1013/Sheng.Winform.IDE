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
using Sheng.SailingEase.Core.Localisation;
using Sheng.SailingEase.ComponentModel;
namespace Sheng.SailingEase.Core
{
    class CorePropertyRelatorAttribute : PropertyRelatorAttribute
    {
        private bool _propertyDisplayNameInitialized = false;
        private string _propertyDisplayName;
        public override string PropertyDisplayName
        {
            get
            {
                if (_propertyDisplayNameInitialized == false)
                {
                    _propertyDisplayName = Language.GetString(_propertyDisplayName);
                    if (_propertyDisplayName == null)
                        _propertyDisplayName = String.Empty;
                    _propertyDisplayNameInitialized = true;
                }
                return this._propertyDisplayName;
            }
            set
            {
                this._propertyDisplayName = value;
                _propertyDisplayNameInitialized = false;
            }
        }
        private bool _catalogInitialized = false;
        private string _catalog;
        public override string Catalog
        {
            get
            {
                if (_catalogInitialized == false)
                {
                    _catalog = Language.GetString(_catalog);
                    if (_catalog == null)
                        _catalog = String.Empty;
                    _catalogInitialized = true;
                }
                return this._catalog;
            }
            set
            {
                this._catalog = value;
                this._catalogInitialized = false;
            }
        }
        private bool _descriptionInitialized = false;
        private string _description = String.Empty;
        public override string Description
        {
            get
            {
                if (_descriptionInitialized == false)
                {
                    _description = Language.GetString(_description);
                    if (_description == null)
                        _description = String.Empty;
                    _descriptionInitialized = true;
                }
                return this._description;
            }
            set
            {
                this._description = value;
                this._descriptionInitialized = false;
            }
        }
        public CorePropertyRelatorAttribute(string propertyDisplayName, string catalog)
            :base(propertyDisplayName,catalog)
        {
        }
    }
}
