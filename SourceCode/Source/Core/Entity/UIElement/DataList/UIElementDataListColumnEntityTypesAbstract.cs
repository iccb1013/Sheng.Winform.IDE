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
    public class UIElementDataListColumnEntityTypesAbstract
    {
        protected UIElementDataListColumnEntityTypeCollection _collection;
        public UIElementDataListColumnEntityAbstract CreateInstance(int code)
        {
            return _collection.CreateInstance(code);
        }
        public UIElementDataListColumnEntityAbstract CreateInstance(UIElementDataListColumnEntityProvideAttribute attribute)
        {
            return _collection.CreateInstance(attribute);
        }
        public UIElementDataListColumnEntityProvideAttribute GetProvideAttribute(int code)
        {
            return _collection.GetProvideAttribute(code);
        }
        public UIElementDataListColumnEntityProvideAttribute GetProvideAttribute(UIElementDataListColumnEntityAbstract entity)
        {
            return _collection.GetProvideAttribute(entity);
        }
        public UIElementDataListColumnEntityProvideAttribute GetProvideAttribute(Type type)
        {
            return _collection.GetProvideAttribute(type);
        }
        public string GetName(UIElementDataListColumnEntityAbstract entity)
        {
            return _collection.GetName(entity);
        }
    }
}
