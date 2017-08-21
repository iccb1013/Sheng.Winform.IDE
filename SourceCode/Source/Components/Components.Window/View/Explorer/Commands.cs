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
using System.Windows.Forms;
using Sheng.SailingEase.Localisation;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Infrastructure;
namespace Sheng.SailingEase.Components.WindowComponent.View
{
    class AddFolderCommand : AbstactCommand<WindowFolderEntity>
    {
        public AddFolderCommand()
        {
            ExcuteHandler = () =>
            {
                WindowFolderEntity folder = GetArgument();
                string folderId = String.Empty;
                if (folder != null)
                    folderId = folder.Id;
                using (FolderEditView view = new FolderEditView(folderId))
                {
                    view.ShowDialog();
                }
            };
        }
    }
    class EditFolderCommand : AbstactCommand<WindowFolderEntity>
    {
        public EditFolderCommand()
        {
            ExcuteHandler = () =>
            {
                WindowFolderEntity entity = GetArgument();
                if (entity == null)
                {
                    Debug.Assert(false, "entity 为 null");
                    return;
                }
                using (FolderEditView view = new FolderEditView())
                {
                    view.FormFolderEntity = entity;
                    view.ShowDialog();
                }
            };
        }
    }
    class DeleteFolderCommand : AbstactCommand<List<WindowFolderEntity>>
    {
        public DeleteFolderCommand()
        {
            ExcuteHandler = () =>
            {
                List<WindowFolderEntity> entityList = GetArgument();
                if (entityList == null || entityList.Count == 0)
                    return;
                StringBuilder folderName = new StringBuilder();
                foreach (WindowFolderEntity entity in entityList)
                {
                    Debug.Assert(entity != null, "entity 为 null");
                    folderName.Append(String.Format("[ {0} ],", entity.Name));
                }
                folderName = folderName.Remove(folderName.Length - 1, 1);
                string strConfirmDelete = String.Format(CommonLanguage.Current.ConfirmDelete, folderName);
                if (MessageBox.Show(strConfirmDelete,
                     CommonLanguage.Current.MessageCaption_Notice,
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.OK)
                {
                    return;
                }
                foreach (WindowFolderEntity entity in entityList)
                {
                    WindowArchive.Instance.DeleteFolder(entity);
                }
            };
        }
    }
    class AddWindowCommand : AbstactCommand<WindowFolderEntity>
    {
        public AddWindowCommand()
        {
            ExcuteHandler = () =>
            {
                WindowFolderEntity folder = GetArgument();
                string folderId = String.Empty;
                if (folder != null)
                    folderId = folder.Id;
                using (WindowEditView formDataItemAdd = new WindowEditView(folderId))
                {
                    formDataItemAdd.ShowDialog();
                }
            };
        }
    }
    class EditWindowCommand : AbstactCommand<WindowEntity>
    {
        public EditWindowCommand()
        {
            ExcuteHandler = () =>
            {
                WindowEntity entity = GetArgument();
                if (entity == null)
                    return;
                if (entity.Sys)
                {
                    MessageBox.Show(CommonLanguage.Current.SystemItemNoEditAllowed,
                        CommonLanguage.Current.MessageCaption_Notice,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                ServiceUnity.WindowDesignService.OpenDesigner(entity);
            };
        }
    }
    class DeleteWindowCommand : AbstactCommand<List<WindowEntity>>
    {
        public DeleteWindowCommand()
        {
            ExcuteHandler = () =>
            {
                List<WindowEntity> entityList = GetArgument();
                if (entityList == null || entityList.Count == 0)
                    return;
                StringBuilder dataEntityName = new StringBuilder();
                foreach (WindowEntity entity in entityList)
                {
                    dataEntityName.Append(String.Format("[ {0} ],", entity.Name));
                }
                dataEntityName = dataEntityName.Remove(dataEntityName.Length - 1, 1);
                string strConfirmDelete = String.Format(CommonLanguage.Current.ConfirmDelete, dataEntityName);
                if (MessageBox.Show(strConfirmDelete, CommonLanguage.Current.MessageCaption_Notice,
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.OK)
                {
                    return;
                }
                foreach (WindowEntity entity in entityList)
                {
                    WindowArchive.Instance.Delete(entity);
                }
            };
        }
    }
    class EditCommand : AbstactCommand<IEntityIndex>
    {
        public EditCommand()
        {
            ExcuteHandler = () =>
            {
                IEntityIndex entity = GetArgument();
                if (entity == null)
                    return;
                if (entity is FolderEntityIndex)
                {
                    using (FolderEditView view = new FolderEditView())
                    {
                        view.FormFolderEntity = ((FolderEntityIndex)entity).Folder;
                        view.ShowDialog();
                    }
                }
                else if (entity is WindowEntityIndex)
                {
                    WindowEntityIndex windowEntityIndex = (WindowEntityIndex)entity;
                    ServiceUnity.WindowDesignService.OpenDesigner(windowEntityIndex.Window);
                }
                else
                    Debug.Assert(false, "未处理的 IEntityIndex 类型");
            };
        }
    }
    class DeleteCommand : AbstactCommand<List<IEntityIndex>>
    {
        public DeleteCommand()
        {
            ExcuteHandler = () =>
            {
                List<IEntityIndex> entityList = GetArgument();
                if (entityList == null || entityList.Count == 0)
                    return;
                StringBuilder dataEntityName = new StringBuilder();
                foreach (IEntityIndex entity in entityList)
                {
                    dataEntityName.Append(String.Format("[ {0} ],", entity.Name));
                }
                dataEntityName = dataEntityName.Remove(dataEntityName.Length - 1, 1);
                string strConfirmDelete = String.Format(CommonLanguage.Current.ConfirmDelete, dataEntityName);
                if (MessageBox.Show(strConfirmDelete, CommonLanguage.Current.MessageCaption_Notice,
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.OK)
                {
                    return;
                }
                foreach (IEntityIndex entity in entityList)
                {
                    if (entity is FolderEntityIndex)
                        WindowArchive.Instance.DeleteFolder(entity.Id);
                    else if (entity is WindowEntityIndex)
                        WindowArchive.Instance.Delete(entity.Id);
                    else
                        Debug.Assert(false, "未处理的 IEntityIndex 类型");
                }
            };
        }
    }
}
