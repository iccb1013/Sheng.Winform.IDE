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
    public class UIElementDataListColumnDataRuleTypeCollection : TypeCollection
    {
        Dictionary<UIElementDataListColumnDataRuleProvideAttribute, Type> _attributes = new Dictionary<UIElementDataListColumnDataRuleProvideAttribute, Type>();
        public UIElementDataListColumnDataRuleTypeCollection()
        {
        }
        public override int Add(Type value)
        {
            object[] attributes = value.GetCustomAttributes(typeof(UIElementDataListColumnDataRuleProvideAttribute), true);
            if (attributes.Length == 0)
            {
                Debug.Assert(false, "没有 FormElementDataListColumnDataRuleProvideAttribute 属性 ");
                throw new Exception();
            }
            _attributes.Add(attributes[0] as UIElementDataListColumnDataRuleProvideAttribute, value);
            return base.Add(value);
        }
        public override void Remove(Type value)
        {
            _attributes.Remove(GetProvideAttribute(value));
            base.Remove(value);
        }
        public UIElementDataListColumnDataRuleAbstract CreateInstance(int code)
        {
            foreach (KeyValuePair<UIElementDataListColumnDataRuleProvideAttribute, Type> attr in this._attributes)
            {
                if (attr.Key.Code == code)
                {
                    return (UIElementDataListColumnDataRuleAbstract)Activator.CreateInstance(attr.Value);
                }
            }
            return null;
        }
        public UIElementDataListColumnDataRuleAbstract CreateInstance(UIElementDataListColumnDataRuleProvideAttribute attribute)
        {
            return CreateInstance(attribute.Code);
        }
       
        public UIElementDataListColumnDataRuleProvideAttribute GetProvideAttribute(int code)
        {
            foreach (KeyValuePair<UIElementDataListColumnDataRuleProvideAttribute, Type> attr in this._attributes)
            {
                if (attr.Key.Code == code)
                {
                    return attr.Key;
                }
            }
            return null;
        }
        public UIElementDataListColumnDataRuleProvideAttribute GetProvideAttribute(UIElementDataListColumnDataRuleAbstract entity)
        {
            return GetProvideAttribute(entity.GetType());
        }
        public UIElementDataListColumnDataRuleProvideAttribute GetProvideAttribute(Type type)
        {
            foreach (KeyValuePair<UIElementDataListColumnDataRuleProvideAttribute, Type> attr in this._attributes)
            {
                if (attr.Value.Equals(type) || type.IsSubclassOf(attr.Value))
                {
                    return attr.Key;
                }
            }
            return null;
        }
        public string GetName(UIElementDataListColumnDataRuleAbstract entity)
        {
            return GetProvideAttribute(entity).Name;
        }
    }
}
