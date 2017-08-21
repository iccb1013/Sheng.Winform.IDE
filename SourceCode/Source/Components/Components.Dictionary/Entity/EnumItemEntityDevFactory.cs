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
namespace Sheng.SailingEase.Components.DictionaryComponent
{
    class EnumItemEntityDevFactory : EnumItemEntityFactoryAbstract
    {
        private static InstanceLazy<EnumItemEntityDevFactory> _instance =
           new InstanceLazy<EnumItemEntityDevFactory>(() => new EnumItemEntityDevFactory());
        public static EnumItemEntityDevFactory Instance
        {
            get { return _instance.Value; }
        }
        private EnumItemEntityDevFactory()
        {
        }
        public override EnumItemEntity CreateEnumItemEntity(EnumEntity owner)
        {
            return new EnumItemEntityDev(owner);
        }
    }
}
