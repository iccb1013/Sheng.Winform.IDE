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
namespace Sheng.SailingEase.Infrastructure
{
    public class RuntimeControlToolboxItemAttribute : Attribute
    {
        private string _name;
        public string Name
        {
            get { return _name; }
        }
        private string _catalog;
        public string Catalog
        {
            get { return _catalog; }
        }
        private Type _controlType;
        public Type ControlType
        {
            get { return _controlType; }
        }
        public RuntimeControlToolboxItemAttribute(string name, string catalog, Type controlType)
        {
            this._name = name;
            this._catalog = catalog;
            this._controlType = controlType;
        }
    }
}
