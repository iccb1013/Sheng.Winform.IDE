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
    public class EventTypeCollection : TypeCollection
    {
        Dictionary<EventProvideAttribute, Type> _attributes = new Dictionary<EventProvideAttribute, Type>();
        public EventTypeCollection()
        {
        }
        public override int Add(Type value)
        {
            object[] attributes = value.GetCustomAttributes(typeof(EventProvideAttribute), true);
            if (attributes.Length == 0)
            {
                Debug.Assert(false, "EventTypeCollection 中指定的类型没有 EventProvideAttribute 属性 ");
                throw new Exception();
            }
            _attributes.Add(attributes[0] as EventProvideAttribute, value);
            return base.Add(value);
        }
        public override void Remove(Type value)
        {
            _attributes.Remove(GetProvideAttribute(value));
            base.Remove(value);
        }
        public EventBase CreateInstance(int code)
        {
            foreach (KeyValuePair<EventProvideAttribute, Type> attr in this._attributes)
            {
                if (attr.Key.Code == code)
                {
                    return (EventBase)Activator.CreateInstance(attr.Value);
                }
            }
            return null;
        }
        public EventBase CreateInstance(EventProvideAttribute attribute)
        {
            if (attribute == null)
                return null;
            return CreateInstance(attribute.Code);
        }
        
        public EventProvideAttribute GetProvideAttribute(int code)
        {
            foreach (KeyValuePair<EventProvideAttribute, Type> attr in this._attributes)
            {
                if (attr.Key.Code == code)
                {
                    return attr.Key;
                }
            }
            return null;
        }
        public EventProvideAttribute GetProvideAttribute(EventBase entity)
        {
            return GetProvideAttribute(entity.GetType());
        }
        public EventProvideAttribute GetProvideAttribute(Type type)
        {
            foreach (KeyValuePair<EventProvideAttribute, Type> attr in this._attributes)
            {
                if (attr.Value.Equals(type) || type.IsSubclassOf(attr.Value))
                {
                    return attr.Key;
                }
            }
            return null;
        }
        public string GetName(EventBase entity)
        {
            return GetProvideAttribute(entity).Name;
        }
    }
}
