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
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Windows.Forms.Development
{
    class FormElementDataListColumnDataRuleDevTypes : UIElementDataListColumnDataRuleTypesAbstract
    {
        private static InstanceLazy<FormElementDataListColumnDataRuleDevTypes> _instance =
            new InstanceLazy<FormElementDataListColumnDataRuleDevTypes>(() => new FormElementDataListColumnDataRuleDevTypes());
        public static FormElementDataListColumnDataRuleDevTypes Instance
        {
            get { return _instance.Value; }
        }
        private FormElementDataListColumnDataRuleDevTypes()
        {
            _collection = new UIElementDataListColumnDataRuleTypeCollection();
            _collection.Add(typeof(FormElementDataListColumnDataRulesDev.NormalDev));
            _collection.Add(typeof(FormElementDataListColumnDataRulesDev.RelationEnumDev));
            _collection.Add(typeof(FormElementDataListColumnDataRulesDev.RelationDataEntityDev));
        }
    }
}
