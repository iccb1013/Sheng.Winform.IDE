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
namespace Sheng.SailingEase.Infrastructure
{
    public interface IWindowComponentService
    {
        WindowFolderEntity GetFolderEntity(string id);
        WindowFolderEntityCollection GetFolderCollection();
        WindowFolderEntityCollection GetFolderCollection(string folderId);
        string GetFolderFullPath(WindowFolderEntity folder);
        List<IEntityIndex> GetIndexList();
        List<IEntityIndex> GetIndexList(string folderId);
        WindowEntityCollection GetWindowList(string folder);
        WindowEntity GetWindowEntity(string id);
        void Save(WindowEntity formEntity);
        bool CheckExist(string id);
        bool CheckExistByCode(string code);
        bool CheckElementExistByCode(string code, UIElementEntityProvideAttribute type);
        bool CheckElementExistByCode(string code);
        bool CheckDataColumnExistByCode(string dataListCode, string columnCode);
    }
}
