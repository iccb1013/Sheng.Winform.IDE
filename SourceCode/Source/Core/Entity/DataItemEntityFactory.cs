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
    class DataItemEntityFactory : DataItemEntityFactoryAbstract
    {
        private static InstanceLazy<DataItemEntityFactory> _instance =
            new InstanceLazy<DataItemEntityFactory>(() => new DataItemEntityFactory());
        public static DataItemEntityFactory Instance
        {
            get { return _instance.Value; }
        }
        private  DataItemEntityFactory()
        {
        }
        public override DataItemEntity CreateDataItemEntity(DataEntity owner)
        {
            return new DataItemEntity(owner);
        }
    }
}
