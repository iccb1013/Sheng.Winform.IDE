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
    class EnumItemEntityFactory : EnumItemEntityFactoryAbstract
    {
        private static InstanceLazy<EnumItemEntityFactory> _instance =
            new InstanceLazy<EnumItemEntityFactory>(() => new EnumItemEntityFactory());
        public static EnumItemEntityFactory Instance
        {
            get { return _instance.Value; }
        }
        private EnumItemEntityFactory()
        {
        }
        public override EnumItemEntity CreateEnumItemEntity(EnumEntity owner)
        {
            return new EnumItemEntity(owner);
        }
    }
}
