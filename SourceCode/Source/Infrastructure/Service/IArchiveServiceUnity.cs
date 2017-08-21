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
using System.Diagnostics;
namespace Sheng.SailingEase.Infrastructure
{
    public interface IArchiveServiceUnity
    {
        void Register(IArchiveService archiveService);
        void Save(object obj);
        void Delete(object obj);
        bool CheckExistByCode(Type type, string code);
    }
    public class ArchiveServiceUnity : IArchiveServiceUnity
    {
        private List<IArchiveService> _services = new List<IArchiveService>();
        private IArchiveService GetService(Type type)
        {
            foreach (var item in _services)
            {
                if (item.CoverType.Equals(type) || (item.ActOnSubClass && type.IsSubclassOf(item.CoverType)))
                {
                    return item;
                }
            }
            return null;
        }
        public void Register(IArchiveService archiveService)
        {
            Debug.Assert(archiveService != null, "archiveService 为 null");
            if (archiveService == null)
                return;
            Debug.Assert(_services.Contains(archiveService) == false, "已经添加了 archiveService");
            if (_services.Contains(archiveService))
                return;
            _services.Add(archiveService);
        }
        public void Save(object obj)
        {
            IArchiveService service = GetService(obj.GetType());
            Debug.Assert(service != null, "没有找到对应的 IArchiveService 服务");
            if (service != null)
                service.Save(obj);
        }
        public void Delete(object obj)
        {
            IArchiveService service = GetService(obj.GetType());
            Debug.Assert(service != null, "没有找到对应的 IArchiveService 服务");
            if (service != null)
                service.Delete(obj);
        }
        public bool CheckExistByCode(Type type, string code)
        {
            IArchiveService service = GetService(type);
            Debug.Assert(service != null, "没有找到对应的 IArchiveService 服务");
            if (service != null)
                return service.CheckExistByCode(code);
            else
                return false;
        }
    }
}
