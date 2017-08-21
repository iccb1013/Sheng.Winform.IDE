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
namespace Sheng.SailingEase.Core
{
    class UIElementDataListColumnDataRuleTypes : UIElementDataListColumnDataRuleTypesAbstract
    {
        private static InstanceLazy<UIElementDataListColumnDataRuleTypes> _instance =
            new InstanceLazy<UIElementDataListColumnDataRuleTypes>(() => new UIElementDataListColumnDataRuleTypes());
        public static UIElementDataListColumnDataRuleTypes Instance
        {
            get { return _instance.Value; }
        }
        private  UIElementDataListColumnDataRuleTypes()
        {
            _collection = new UIElementDataListColumnDataRuleTypeCollection();
            _collection.Add(typeof(UIElementDataListColumnDataRules.Normal));
            _collection.Add(typeof(UIElementDataListColumnDataRules.RelationEnum));
            _collection.Add(typeof(UIElementDataListColumnDataRules.RelationDataEntity));
        }
    }
}
