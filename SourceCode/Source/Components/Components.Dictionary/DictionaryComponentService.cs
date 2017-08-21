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
namespace Sheng.SailingEase.Components.DictionaryComponent
{
    class DictionaryComponentService : IDictionaryComponentService
    {
        DictionaryArchive _archive = DictionaryArchive.Instance;
        private static InstanceLazy<DictionaryComponentService> _instance =
           new InstanceLazy<DictionaryComponentService>(() => new DictionaryComponentService());
        public static DictionaryComponentService Instance
        {
            get { return _instance.Value; }
        }
        private DictionaryComponentService()
        {
        }
        public EnumEntity GetEnumEntity(string id)
        {
            return _archive.GetEnumEntity(id);
        }
        public List<EnumEntity> GetEnumEntityList()
        {
            return _archive.GetEnumEntityList();
        }
    }
}
