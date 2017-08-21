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
using System.Collections;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.ComponentModel;
namespace Sheng.SailingEase.Core
{
    public class UIElementEntityTypeCollection : TypeCollection
    {
        Dictionary<UIElementEntityProvideAttribute, Type> _attributes = new Dictionary<UIElementEntityProvideAttribute, Type>();
        public bool Any
        {
            get { return this.Count == 0; }
        }
        public UIElementEntityTypeCollection()
        {
            this.ActOnSub = true;
        }
        public UIElementEntityTypeCollection(Type[] value)
            :this()
        {
            for (int i = 0; (i < value.Length); i = (i + 1))
            {
                this.Add(value[i]);
            }
        }
        public override int Add(Type value)
        {
            object[] attributes = value.GetCustomAttributes(typeof(UIElementEntityProvideAttribute), true);
            if (attributes.Length == 0)
            {
                Debug.Assert(false, "FormElementEntityTypeCollection.EventProvide 中指定的类型没有 FormElementEntityProvideAttribute 属性 ");
                throw new Exception();
            }
            _attributes.Add(attributes[0] as UIElementEntityProvideAttribute, value);
            return base.Add(value);
        }
        public override void Remove(Type value)
        {
            _attributes.Remove(GetProvideAttribute(value));
            base.Remove(value);
        }
        public EntityBase CreateInstance(int code)
        {
            foreach (KeyValuePair<UIElementEntityProvideAttribute,Type> attr in this._attributes)
            {
                if (attr.Key.Code == code)
                {
                    return (EntityBase)Activator.CreateInstance(attr.Value);
                }
            }
            return null;
        }
        public EntityBase CreateInstance(UIElementEntityProvideAttribute attribute)
        {
            return CreateInstance(attribute.Code);
        }
       
        public UIElementEntityProvideAttribute GetProvideAttribute(int code)
        {
            foreach (KeyValuePair<UIElementEntityProvideAttribute, Type> attr in this._attributes)
            {
                if (attr.Key.Code == code)
                {
                    return attr.Key;
                }
            }
            return null;
        }
        public UIElementEntityProvideAttribute GetProvideAttribute(EntityBase entity)
        {
            return GetProvideAttribute(entity.GetType());
        }
        public UIElementEntityProvideAttribute GetProvideAttribute(Type type)
        {
            foreach (KeyValuePair<UIElementEntityProvideAttribute, Type> attr in this._attributes)
            {
                if (attr.Value.Equals(type) || type.IsSubclassOf(attr.Value))
                {
                    return attr.Key;
                }
            }
            return null;
        }
        public string GetName(EntityBase entity)
        {
            UIElementEntityProvideAttribute attribute = GetProvideAttribute(entity);
            if (attribute != null)
            {
                return GetProvideAttribute(entity).Name;
            }
            else
            {
                return String.Empty;
            }
        }
        public bool Allowable(EntityBase obj)
        {
            if (this.Any)
            {
                return true;
            }
            return base.Contains(obj);
        }
        public IEnumerable<UIElementEntityProvideAttribute> EntityProvideAttributeEnum
        {
            get
            {
                foreach (KeyValuePair<UIElementEntityProvideAttribute, Type> attribute in _attributes)
                    yield return attribute.Key;
            }
        }
    }
}
