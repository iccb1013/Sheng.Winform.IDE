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
using Sheng.SailingEase.Core.Development;
namespace Sheng.SailingEase.Windows.Forms.Development
{
    class FormElementDataListColumnEntityDevTypes : UIElementDataListColumnEntityTypesAbstract
    {
        private static InstanceLazy<FormElementDataListColumnEntityDevTypes> _instance =
            new InstanceLazy<FormElementDataListColumnEntityDevTypes>(() => new FormElementDataListColumnEntityDevTypes());
        public static FormElementDataListColumnEntityDevTypes Instance
        {
            get { return _instance.Value; }
        }
        private FormElementDataListColumnEntityDevTypes()
        {
            _collection = new UIElementDataListColumnEntityTypeCollection();
            _collection.Add(typeof(FormElementDataListTextBoxColumnEntityDev));
            FormElementEntityDevTypes.Instance.AddRange(_collection);
        }
    }
}
