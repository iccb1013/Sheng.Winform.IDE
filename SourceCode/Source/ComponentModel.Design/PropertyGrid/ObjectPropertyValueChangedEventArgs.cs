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
    public class ObjectPropertyValueChangedEventArgs : EventArgs
    {
        private object _targetObject;
        public object TargetObject
        {
            get
            {
                return this._targetObject;
            }
        }
        public object RootObject
        {
            get;
            set;
        }
        private PropertyGridRow _row;
        public PropertyGridRow Row
        {
            get
            {
                return this._row;
            }
        }
        public object OldValue
        {
            get;
            set;
        }
        public object NewValue
        {
            get;
            set;
        }
        public string Property
        {
            get;
            set;
        }
        public string PropertyName
        {
            get
            {
                return this.Row.PropertyRelatorAttribute.PropertyDisplayName;
            }
        }
        public ObjectPropertyValueChangedEventArgs(object rootObject, object obj, string property, object oldValue, object newValue, PropertyGridRow row)
        {
            this.RootObject = rootObject;
            this._targetObject = obj;
            this.Property = property;
            this.OldValue = oldValue;
            this.NewValue = newValue;
            this._row = row;
        }
    }
}
