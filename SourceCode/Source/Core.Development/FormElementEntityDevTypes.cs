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
using Sheng.SailingEase.Infrastructure;
namespace Sheng.SailingEase.Core.Development
{
    public class FormElementEntityDevTypes : UIElementEntityTypesAbstract
    {
        private static InstanceLazy<FormElementEntityDevTypes> _instance =
            new InstanceLazy<FormElementEntityDevTypes>(() => new FormElementEntityDevTypes());
        public static FormElementEntityDevTypes Instance
        {
            get { return _instance.Value; }
        }
        private FormElementEntityDevTypes()
        {
            AddRange(ServiceUnity.WindowElementContainer.GetDevElementEntityTypes().ToArray());
        }
    }
}
