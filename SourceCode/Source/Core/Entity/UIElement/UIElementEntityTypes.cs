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
   
    class UIElementEntityTypes : UIElementEntityTypesAbstract
    {
        private static InstanceLazy<UIElementEntityTypes> _instance =
            new InstanceLazy<UIElementEntityTypes>(() => new UIElementEntityTypes());
        public static UIElementEntityTypes Instance
        {
            get { return _instance.Value; }
        }
        private  UIElementEntityTypes()
        {
            /*
             * 数据列不在这里注册了，单独提供一个数据列Types类
             */
            Add(typeof(UIElementTextBoxEntity));
            Add(typeof(UIElementComboBoxEntity));
            Add(typeof(UIElementDataListEntity));
            Add(typeof(UIElementLabelEntity));
            Add(typeof(UIElementPictureBoxEntity));
            Add(typeof(UIElementButtonEntity));
        }
    }
}
