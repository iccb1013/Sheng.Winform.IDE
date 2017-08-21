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
using Sheng.SailingEase.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Components.DictionaryComponent.View
{
    class AddEnumEntityCommand : AbstactCommand
    {
        public AddEnumEntityCommand()
        {
            ExcuteHandler = () =>
            {
                using (EnumEntityEditView view = new EnumEntityEditView())
                {
                    view.ShowDialog();
                }
            };
        }
    }
    class EditEnumEntityCommand : AbstactCommand<EnumEntityDev>
    {
        public EditEnumEntityCommand()
        {
            ExcuteHandler = () =>
            {
                EnumEntityDev entity = GetArgument();
                if (entity == null)
                    return;
                if (entity.Sys)
                {
                }
                using (EnumEntityEditView view = new EnumEntityEditView())
                {
                    view.EnumEntity = entity;
                    view.ShowDialog();
                }
            };
        }
    }
    class DeleteEnumEntityCommand : AbstactCommand<List<EnumEntityDev>>
    {
        public DeleteEnumEntityCommand()
        {
            ExcuteHandler = () =>
            {
                List<EnumEntityDev> entityList = GetArgument();
                if (entityList == null || entityList.Count == 0)
                    return;
                StringBuilder entityName = new StringBuilder();
                StringBuilder entityNameSys = new StringBuilder();
                foreach (EnumEntityDev entity in entityList)
                {
                    if (entity.Sys)
                    {
                        entityNameSys.Append(String.Format("[ {0} ],", entity.Name));
                        continue;
                    }
                    entityName.Append(String.Format("[ {0} ],", entity.Name));
                }
                if (entityNameSys.Length > 0)
                {
                    entityNameSys = entityNameSys.Remove(entityNameSys.Length - 1, 1);
                    MessageBox.Show(
                        CommonLanguage.Current.SystemItemNoDeleteAllowed
                        + Environment.NewLine + Environment.NewLine + entityNameSys,
                        CommonLanguage.Current.MessageCaption_Notice,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                entityName = entityName.Remove(entityName.Length - 1, 1);
                string strConfirmDelete = String.Format(CommonLanguage.Current.ConfirmDelete, entityName);
                if (MessageBox.Show(strConfirmDelete,
                     CommonLanguage.Current.MessageCaption_Notice,
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.OK)
                {
                    return;
                }
                foreach (EnumEntityDev entity in entityList)
                {
                    if (entity.Sys) continue;
                    DictionaryArchive.Instance.Delete(entity);
                }
            };
        }
    }
    class AddEnumItemEntityCommand : AbstactCommand<EnumEntityDev>
    {
        public AddEnumItemEntityCommand()
        {
            ExcuteHandler = () =>
            {
                EnumEntityDev enumEntity = GetArgument();
                if (enumEntity == null)
                    return;
                using (EnumItemEntityEditView view = new EnumItemEntityEditView(enumEntity))
                {
                    view.ShowDialog();
                }
            };
        }
    }
    class EditEnumItemEntityCommand : AbstactCommand<EnumItemEntityDev>
    {
        public EditEnumItemEntityCommand()
        {
            ExcuteHandler = () =>
            {
                EnumItemEntityDev entity = GetArgument();
                if (entity == null)
                    return;
                if (entity.Sys)
                {
                }
                using (EnumItemEntityEditView view = new EnumItemEntityEditView(entity.Owner))
                {
                    view.EnumItemEntity = entity;
                    view.ShowDialog();
                }
            };
        }
    }
    class DeleteEnumItemEntityCommand : AbstactCommand<List<EnumItemEntityDev>>
    {
        public DeleteEnumItemEntityCommand()
        {
            ExcuteHandler = () =>
            {
                List<EnumItemEntityDev> entityList = GetArgument();
                if (entityList == null || entityList.Count == 0)
                    return;
                StringBuilder entityName = new StringBuilder();
                StringBuilder entityNameSys = new StringBuilder();
                foreach (EnumItemEntityDev entity in entityList)
                {
                    if (entity.Sys)
                    {
                        entityNameSys.Append(String.Format("[ {0} ],", entity.Name));
                        continue;
                    }
                    entityName.Append(String.Format("[ {0} ],", entity.Name));
                }
                if (entityNameSys.Length > 0)
                {
                    entityNameSys = entityNameSys.Remove(entityNameSys.Length - 1, 1);
                    MessageBox.Show(
                        CommonLanguage.Current.SystemItemNoDeleteAllowed
                        + Environment.NewLine + Environment.NewLine + entityNameSys,
                        CommonLanguage.Current.MessageCaption_Notice,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                entityName = entityName.Remove(entityName.Length - 1, 1);
                string strConfirmDelete = String.Format(CommonLanguage.Current.ConfirmDelete, entityName);
                if (MessageBox.Show(strConfirmDelete,
                     CommonLanguage.Current.MessageCaption_Notice,
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.OK)
                {
                    return;
                }
                foreach (EnumItemEntityDev entity in entityList)
                {
                    if (entity.Sys) continue;
                    DictionaryArchive.Instance.DeleteItem(entity);
                }
            };
        }
    }
}
