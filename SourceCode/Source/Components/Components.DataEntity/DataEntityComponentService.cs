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
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Components.DataEntityComponent
{
    class DataEntityComponentService : IDataEntityComponentService
    {
        DataEntityArchive _archive = DataEntityArchive.Instance;
        private static InstanceLazy<DataEntityComponentService> _instance =
           new InstanceLazy<DataEntityComponentService>(() => new DataEntityComponentService());
        public static DataEntityComponentService Instance
        {
            get { return _instance.Value; }
        }
        private DataEntityComponentService()
        {
        }
        public DataEntity GetDataEntity(string id)
        {
            return _archive.GetDataEntity(id);
        }
        public DataItemEntity GetDataItemEntity(string id, string dataEntityId)
        {
            return _archive.GetDataItemEntity(id, dataEntityId);
        }
        public List<DataEntity> GetDataEntityList()
        {
            return _archive.GetDataEntityList();
        }
    }
}
