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
using Sheng.SailingEase.Components.DictionaryComponent.Localisation;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Core.Development;
using Sheng.SailingEase.Infrastructure;
namespace Sheng.SailingEase.Components.DictionaryComponent
{
    class EnumEntityDev : EnumEntity
    {
        public EnumEntityDev()
        {
            base.ItemFactory = EnumItemEntityDevFactory.Instance;
        }
    }
}
