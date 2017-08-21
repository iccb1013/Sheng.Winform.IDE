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
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.ComponentModel;
namespace Sheng.SailingEase.Core
{
    public abstract class UIElementEntityTypesAbstract
    {
        private UIElementEntityTypeCollection _collection = new UIElementEntityTypeCollection();
        public EntityBase CreateInstance(int code)
        {
            return _collection.CreateInstance(code);
        }
        public EntityBase CreateInstance(UIElementEntityProvideAttribute attribute)
        {
            return _collection.CreateInstance(attribute);
        }
        public UIElementEntityProvideAttribute GetProvideAttribute(int code)
        {
            return _collection.GetProvideAttribute(code);
        }
        public UIElementEntityProvideAttribute GetProvideAttribute(EntityBase entity)
        {
            return _collection.GetProvideAttribute(entity);
        }
        public UIElementEntityProvideAttribute GetProvideAttribute(Type type)
        {
            return _collection.GetProvideAttribute(type);
        }
        public string GetName(EntityBase entity)
        {
            return _collection.GetName(entity);
        }
        public void AddRange(TypeCollection value)
        {
            _collection.AddRange(value);
        }
        public void AddRange(Type[] types)
        {
            _collection.AddRange(types);
        }
        public void Add(Type type)
        {
            _collection.Add(type);
        }
    }
}
