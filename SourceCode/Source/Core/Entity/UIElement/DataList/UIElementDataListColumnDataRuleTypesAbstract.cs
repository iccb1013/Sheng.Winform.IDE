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
    public abstract class UIElementDataListColumnDataRuleTypesAbstract
    {
        protected UIElementDataListColumnDataRuleTypeCollection _collection;
        public UIElementDataListColumnDataRuleAbstract CreateInstance(int code)
        {
            return _collection.CreateInstance(code);
        }
        public UIElementDataListColumnDataRuleAbstract CreateInstance(UIElementDataListColumnDataRuleProvideAttribute attribute)
        {
            return _collection.CreateInstance(attribute);
        }
        public UIElementDataListColumnDataRuleProvideAttribute GetProvideAttribute(int code)
        {
            return _collection.GetProvideAttribute(code);
        }
        public UIElementDataListColumnDataRuleProvideAttribute GetProvideAttribute(UIElementDataListColumnDataRuleAbstract entity)
        {
            return _collection.GetProvideAttribute(entity);
        }
        public UIElementDataListColumnDataRuleProvideAttribute GetProvideAttribute(Type type)
        {
            return _collection.GetProvideAttribute(type);
        }
        public string GetName(UIElementDataListColumnDataRuleAbstract entity)
        {
            return _collection.GetName(entity);
        }
        public List<T> GetInstanceList<T>()
        {
            return _collection.GetInstanceList<T>();
        }
    }
}
