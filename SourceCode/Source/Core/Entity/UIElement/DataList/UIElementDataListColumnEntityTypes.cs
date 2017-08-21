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
    class UIElementDataListColumnEntityTypes : UIElementDataListColumnEntityTypesAbstract
    {
        private static InstanceLazy<UIElementDataListColumnEntityTypes> _instance =
            new InstanceLazy<UIElementDataListColumnEntityTypes>(() => new UIElementDataListColumnEntityTypes());
        public static UIElementDataListColumnEntityTypes Instance
        {
            get { return _instance.Value; }
        }
        private UIElementDataListColumnEntityTypes()
        {
            _collection = new UIElementDataListColumnEntityTypeCollection();
            _collection.Add(typeof(UIElementDataListTextBoxColumnEntity));
            UIElementEntityTypes.Instance.AddRange(_collection);
        }
    }
}
