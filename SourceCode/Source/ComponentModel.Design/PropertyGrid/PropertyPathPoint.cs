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
using System.Reflection;
namespace Sheng.SailingEase.ComponentModel.Design
{
    public class PropertyPathPoint
    {
        public PropertyInfo Property
        {
            get;
            set;
        }
        private PropertyPathPoint _pre;
        public PropertyPathPoint Pre
        {
            get { return this._pre; }
            set
            {
                this._pre = value;
                if (value.Next != this)
                    value.Next = this;
            }
        }
        private PropertyPathPoint _next;
        public PropertyPathPoint Next
        {
            get { return this._next; }
            set
            {
                this._next = value;
                if (value.Pre != this)
                    value.Pre = this;
            }
        }
        public PropertyPathPoint(PropertyInfo propertyInfo)
        {
            this.Property = propertyInfo;
        }
    }
}
