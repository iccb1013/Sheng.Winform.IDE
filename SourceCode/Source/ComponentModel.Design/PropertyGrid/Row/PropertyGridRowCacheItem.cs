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
namespace Sheng.SailingEase.ComponentModel.Design
{
    class PropertyGridRowCacheItem
    {
        public Type Type { get; set; }
        public PropertyGridRow PropertyGridRow { get; set; }
        public string PropertyName
        {
            get { return PropertyGridRow.PropertyName; }
        }
        public string Catalog
        {
            get { return PropertyGridRow.Catalog; }
        }
        public PropertyGridRowCacheItem(Type type, PropertyGridRow propertyGridRow)
        {
            Type = type;
            PropertyGridRow = propertyGridRow;
        }
    }
}
