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
namespace Sheng.SailingEase.Core
{
    public class EventTypesAbstract
    {
        private EventTypeCollection _collection = new EventTypeCollection();
        public EventBase CreateInstance(int code)
        {
            return _collection.CreateInstance(code);
        }
        public EventBase CreateInstance(EventProvideAttribute attribute)
        {
            return _collection.CreateInstance(attribute);
        }
        public EventProvideAttribute GetProvideAttribute(int code)
        {
            return _collection.GetProvideAttribute(code);
        }
        public EventProvideAttribute GetProvideAttribute(EventBase entity)
        {
            return _collection.GetProvideAttribute(entity);
        }
        public EventProvideAttribute GetProvideAttribute(Type type)
        {
            return _collection.GetProvideAttribute(type);
        }
        public string GetName(EventBase entity)
        {
            return _collection.GetName(entity);
        }
        public void Add(Type type)
        {
            _collection.Add(type);
        }
    }
}
