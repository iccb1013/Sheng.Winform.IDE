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
namespace Sheng.SailingEase.Components.DataEntityComponent
{
    class DataItemEntityDevFactory : DataItemEntityFactoryAbstract
    {
        private static InstanceLazy<DataItemEntityDevFactory> _instance =
           new InstanceLazy<DataItemEntityDevFactory>(() => new DataItemEntityDevFactory());
        public static DataItemEntityDevFactory Instance
        {
            get { return _instance.Value; }
        }
        private DataItemEntityDevFactory()
        {
        }
        public override DataItemEntity CreateDataItemEntity(DataEntity owner)
        {
            return new DataItemEntityDev(owner);
        }
    }
}
