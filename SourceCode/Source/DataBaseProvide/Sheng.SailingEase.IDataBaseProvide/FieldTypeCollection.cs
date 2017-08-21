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
namespace Sheng.SailingEase.IDataBaseProvide
{
    public class FieldTypeCollection : TypeCollection
    {
        Dictionary<FieldProvideAttribute, Type> _attributes = new Dictionary<FieldProvideAttribute, Type>();
        public override int Add(Type value)
        {
            object[] attributes = value.GetCustomAttributes(typeof(FieldProvideAttribute), true);
            if (attributes.Length == 0)
            {
                Debug.Assert(false, "ToolStripItemEntityTypeCollection.EventProvide 中指定的类型没有 FieldProvideAttribute 属性 ");
                throw new Exception();
            }
            _attributes.Add(attributes[0] as FieldProvideAttribute, value);
            return base.Add(value);
        }
        public override void Remove(Type value)
        {
            _attributes.Remove(GetProvideAttribute(value));
            base.Remove(value);
        }
        public IField CreateInstance(int code)
        {
            foreach (KeyValuePair<FieldProvideAttribute, Type> attr in this._attributes)
            {
                if (attr.Key.Code == code)
                {
                    return (IField)Activator.CreateInstance(attr.Value);
                }
            }
            return null;
        }
        public IField CreateInstance(FieldProvideAttribute attribute)
        {
            Debug.Assert(attribute != null, "attribute 为 null");
            return CreateInstance(attribute.Code);
        }
        
        public FieldProvideAttribute GetProvideAttribute(int code)
        {
            foreach (KeyValuePair<FieldProvideAttribute, Type> attr in this._attributes)
            {
                if (attr.Key.Code == code)
                {
                    return attr.Key;
                }
            }
            return null;
        }
        public FieldProvideAttribute GetProvideAttribute(IField field)
        {
            return GetProvideAttribute(field.GetType());
        }
        public FieldProvideAttribute GetProvideAttribute(Type type)
        {
            foreach (KeyValuePair<FieldProvideAttribute, Type> attr in this._attributes)
            {
                if (attr.Value.Equals(type) || type.IsSubclassOf(attr.Value))
                {
                    return attr.Key;
                }
            }
            return null;
        }
        public List<FieldProvideAttribute> GetProvideAttributeList()
        {
            return _attributes.Keys.ToList();
        }
        public string GetName(IField field)
        {
            return GetProvideAttribute(field).Name;
        }
    }
}
