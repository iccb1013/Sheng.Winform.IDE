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
using System.Diagnostics;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Core
{
    public class UIElementDataListColumnEntityTypeCollection : TypeCollection
    {
        Dictionary<UIElementDataListColumnEntityProvideAttribute, Type> _attributes = new Dictionary<UIElementDataListColumnEntityProvideAttribute, Type>();
        public UIElementDataListColumnEntityTypeCollection()
        {
        }
        public override int Add(Type value)
        {
            object[] attributes = value.GetCustomAttributes(typeof(UIElementDataListColumnEntityProvideAttribute), true);
            if (attributes.Length == 0)
            {
                Debug.Assert(false, "FormElementEntityTypeCollection.EventProvide 中指定的类型没有 FormElementDataListColumnEntityProvideAttribute 属性 ");
                throw new Exception();
            }
            _attributes.Add(attributes[0] as UIElementDataListColumnEntityProvideAttribute, value);
            return base.Add(value);
        }
        public override void Remove(Type value)
        {
            _attributes.Remove(GetProvideAttribute(value));
            base.Remove(value);
        }
        public UIElementDataListColumnEntityAbstract CreateInstance(int code)
        {
            foreach (KeyValuePair<UIElementDataListColumnEntityProvideAttribute, Type> attr in this._attributes)
            {
                if (attr.Key.Code == code)
                {
                    return (UIElementDataListColumnEntityAbstract)Activator.CreateInstance(attr.Value);
                }
            }
            return null;
        }
        public UIElementDataListColumnEntityAbstract CreateInstance(UIElementDataListColumnEntityProvideAttribute attribute)
        {
            return CreateInstance(attribute.Code);
        }
        /*
         * GetProvideAttribute的判断依据是传入对象或集合内对象的ProvideAttribute的Code是否相同
         * 所以即时集合中是core中的对象，但传入的是DEV结尾的对象，也能取到其对应的Attribute
         */
        public UIElementDataListColumnEntityProvideAttribute GetProvideAttribute(int code)
        {
            foreach (KeyValuePair<UIElementDataListColumnEntityProvideAttribute, Type> attr in this._attributes)
            {
                if (attr.Key.Code == code)
                {
                    return attr.Key;
                }
            }
            return null;
        }
        public UIElementDataListColumnEntityProvideAttribute GetProvideAttribute(UIElementDataListColumnEntityAbstract entity)
        {
            return GetProvideAttribute(entity.GetType());
        }
        public UIElementDataListColumnEntityProvideAttribute GetProvideAttribute(Type type)
        {
            foreach (KeyValuePair<UIElementDataListColumnEntityProvideAttribute, Type> attr in this._attributes)
            {
                if (attr.Value.Equals(type) || type.IsSubclassOf(attr.Value))
                {
                    return attr.Key;
                }
            }
            return null;
        }
        public string GetName(UIElementDataListColumnEntityAbstract entity)
        {
            return GetProvideAttribute(entity).Name;
        }
    }
}
