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
    public class ToolStripItemEntityTypeCollection : TypeCollection
    {
        Dictionary<ToolStripItemEntityProvideAttribute, Type> _attributes = new Dictionary<ToolStripItemEntityProvideAttribute, Type>();
        public ToolStripItemEntityTypeCollection()
        {
        }
        public override int Add(Type value)
        {
            object[] attributes = value.GetCustomAttributes(typeof(ToolStripItemEntityProvideAttribute), true);
            Debug.Assert(attributes.Length == 1, "ToolStripItemEntityTypeCollection.EventProvide 中指定的类型没有 ToolStripItemEntityProvideAttribute 属性 ");
            if (attributes.Length > 0)
            {
                _attributes.Add(attributes[0] as ToolStripItemEntityProvideAttribute, value);
                return base.Add(value);
            }
            return -1;
        }
        public override void Remove(Type value)
        {
            _attributes.Remove(GetProvideAttribute(value));
            base.Remove(value);
        }
        public ToolStripItemAbstract CreateInstance(int code)
        {
            foreach (KeyValuePair<ToolStripItemEntityProvideAttribute, Type> attr in this._attributes)
            {
                if (attr.Key.Code == code)
                {
                    return (ToolStripItemAbstract)Activator.CreateInstance(attr.Value);
                }
            }
            return null;
        }
        public ToolStripItemAbstract CreateInstance(ToolStripItemEntityProvideAttribute attribute)
        {
            return CreateInstance(attribute.Code);
        }
        
        public ToolStripItemEntityProvideAttribute GetProvideAttribute(int code)
        {
            foreach (KeyValuePair<ToolStripItemEntityProvideAttribute, Type> attr in this._attributes)
            {
                if (attr.Key.Code == code)
                {
                    return attr.Key;
                }
            }
            return null;
        }
        public ToolStripItemEntityProvideAttribute GetProvideAttribute(ToolStripItemAbstract entity)
        {
            return GetProvideAttribute(entity.GetType());
        }
        public ToolStripItemEntityProvideAttribute GetProvideAttribute(Type type)
        {
            foreach (KeyValuePair<ToolStripItemEntityProvideAttribute, Type> attr in this._attributes)
            {
                if (attr.Value.Equals(type) || type.IsSubclassOf(attr.Value))
                {
                    return attr.Key;
                }
            }
            return null;
        }
        public List<ToolStripItemEntityProvideAttribute> GetProvideAttributeList()
        {
            return _attributes.Keys.ToList(); 
        }
        public string GetName(ToolStripItemAbstract entity)
        {
            return GetProvideAttribute(entity).Name;
        }
    }
}
