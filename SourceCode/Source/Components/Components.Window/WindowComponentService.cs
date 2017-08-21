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
namespace Sheng.SailingEase.Components.WindowComponent
{
    class WindowComponentService : IWindowComponentService
    {
        private WindowArchive _windowArchive = WindowArchive.Instance;
        private static InstanceLazy<WindowComponentService> _instance =
           new InstanceLazy<WindowComponentService>(() => new WindowComponentService());
        public static WindowComponentService Instance
        {
            get { return _instance.Value; }
        }
        private WindowComponentService()
        {
        }
        public WindowFolderEntity GetFolderEntity(string id)
        {
            return _windowArchive.GetFolderEntity(id);
        }
        public WindowFolderEntityCollection GetFolderCollection()
        {
            return _windowArchive.GetFolderCollection();
        }
        public WindowFolderEntityCollection GetFolderCollection(string folderId)
        {
            return _windowArchive.GetFolderCollection(folderId);
        }
        public string GetFolderFullPath(WindowFolderEntity folder)
        {
            return _windowArchive.GetFolderFullPath(folder);
        }
        public List<IEntityIndex> GetIndexList()
        {
            return _windowArchive.GetIndexList();
        }
        public List<IEntityIndex> GetIndexList(string folderId)
        {
            return _windowArchive.GetIndexList(folderId);
        }
        public WindowEntityCollection GetWindowList(string folder)
        {
            return _windowArchive.GetWindowList(folder);
        }
        public WindowEntity GetWindowEntity(string id)
        {
            return _windowArchive.GetWindowEntity(id);
        }
        public bool CheckExist(string id)
        {
            return _windowArchive.CheckExistById(id);
        }
        public void Save(WindowEntity formEntity)
        {
            _windowArchive.Commit(formEntity);
        }
        public bool CheckExistByCode(string code)
        {
            return _windowArchive.CheckExistByCode(code);
        }
        public bool CheckElementExistByCode(string code, UIElementEntityProvideAttribute type)
        {
            return _windowArchive.CheckElementExistByCode(code, type);
        }
        public bool CheckElementExistByCode(string code)
        {
            return _windowArchive.CheckElementExistByCode(code);
        }
        public bool CheckDataColumnExistByCode(string dataListCode, string columnCode)
        {
            return _windowArchive.CheckDataColumnExistByCode(dataListCode, columnCode);
        }
    }
}
