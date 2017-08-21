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
using Sheng.SailingEase.Components.DataEntityComponent.Localisation;
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Infrastructure;
namespace Sheng.SailingEase.Components.DataEntityComponent.View
{
    class AddDataEntityCommand : AbstactCommand
    {
        public AddDataEntityCommand()
        {
            ExcuteHandler = () =>
            {
                using (DataEntityEditView view = new DataEntityEditView())
                {
                    view.ShowDialog();
                }
            };
        }
    }
    class EditDataEntityCommand : AbstactCommand<DataEntityDev>
    {
        public EditDataEntityCommand()
        {
            ExcuteHandler = () =>
            {
                DataEntityDev entity = GetArgument();
                if (entity == null)
                    return;
                if (entity.Sys)
                {
                    MessageBox.Show(CommonLanguage.Current.SystemItemNoEditAllowed,
                        CommonLanguage.Current.MessageCaption_Notice,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                using (DataEntityEditView view = new DataEntityEditView())
                {
                    view.DataEntity = entity;
                    view.ShowDialog();
                }
            };
        }
    }
    class DeleteDataEntityCommand : AbstactCommand<List<DataEntityDev>>
    {
        public DeleteDataEntityCommand()
        {
            ExcuteHandler = () =>
            {
                List<DataEntityDev> entityList = GetArgument();
                if (entityList == null || entityList.Count == 0)
                    return;
                StringBuilder dataEntityName = new StringBuilder();
                StringBuilder dataEntityNameSys = new StringBuilder();
                foreach (DataEntityDev entity in entityList)
                {
                    if (entity.Sys)
                    {
                        dataEntityNameSys.Append(String.Format("[ {0} ],", entity.Name));
                        continue;
                    }
                    dataEntityName.Append(String.Format("[ {0} ],", entity.Name));
                }
                if (dataEntityNameSys.Length > 0)
                {
                    dataEntityNameSys = dataEntityNameSys.Remove(dataEntityNameSys.Length - 1, 1);
                    MessageBox.Show(
                        CommonLanguage.Current.SystemItemNoDeleteAllowed
                        + Environment.NewLine + Environment.NewLine + dataEntityNameSys,
                        CommonLanguage.Current.MessageCaption_Notice,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                dataEntityName = dataEntityName.Remove(dataEntityName.Length - 1, 1);
                string strConfirmDelete = String.Format(CommonLanguage.Current.ConfirmDelete, dataEntityName);
                if (MessageBox.Show(strConfirmDelete,
                     CommonLanguage.Current.MessageCaption_Notice,
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.OK)
                {
                    return;
                }
                foreach (DataEntityDev entity in entityList)
                {
                    if (entity.Sys) continue;
                    DataEntityArchive.Instance.Delete(entity);
                }
            };
        }
    }
    class CreateSqlCommand : AbstactCommand<List<DataEntity>>
    {
        private IWorkbenchService _workbenchService = ServiceUnity.WorkbenchService;
        public CreateSqlCommand()
        {
            ExcuteHandler = () =>
            {
                List<DataEntity> entityList = GetArgument();
                if (entityList == null || entityList.Count == 0)
                    return;
                DataEntityScriptView formDataEntityCreateSql = new DataEntityScriptView(entityList);
                formDataEntityCreateSql.HideOnClose = false;
                formDataEntityCreateSql.TabText = Language.Current.DataEntityCreateSqlView_TabText +
                    " - " +
                    entityList[0].Name;
                if (entityList.Count > 1)
                {
                    formDataEntityCreateSql.TabText += "...";
                }
                _workbenchService.Show(formDataEntityCreateSql);
            };
        }
    }
    class AddDataItemEntityCommand : AbstactCommand<DataEntityDev>
    {
        public AddDataItemEntityCommand()
        {
            ExcuteHandler = () =>
            {
                DataEntityDev dataEntity = GetArgument();
                if (dataEntity == null)
                    return;
                using (DataItemEditView formDataItemAdd = new DataItemEditView(dataEntity))
                {
                    formDataItemAdd.ShowDialog();
                }
            };
        }
    }
    class EditDataItemEntityCommand : AbstactCommand<DataItemEntityDev>
    {
        public EditDataItemEntityCommand()
        {
            ExcuteHandler = () =>
            {
                DataItemEntityDev entity = GetArgument();
                if (entity == null)
                    return;
                if (entity.Sys)
                {
                    MessageBox.Show(CommonLanguage.Current.SystemItemNoEditAllowed,
                        CommonLanguage.Current.MessageCaption_Notice,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                using (DataItemEditView formDataItemAdd = new DataItemEditView((DataEntityDev)entity.Owner))
                {
                    formDataItemAdd.DataItemEntity = entity;
                    formDataItemAdd.ShowDialog();
                }
            };
        }
    }
    class DeleteDataItemEntityCommand : AbstactCommand<List<DataItemEntityDev>>
    {
        public DeleteDataItemEntityCommand()
        {
            ExcuteHandler = () =>
            {
                List<DataItemEntityDev> entityList = GetArgument();
                if (entityList == null || entityList.Count == 0)
                    return;
                StringBuilder dataEntityName = new StringBuilder();
                StringBuilder dataEntityNameSys = new StringBuilder();
                foreach (DataItemEntityDev entity in entityList)
                {
                    if (entity.Sys)
                    {
                        dataEntityNameSys.Append(String.Format("[ {0} ],", entity.Name));
                        continue;
                    }
                    dataEntityName.Append(String.Format("[ {0} ],", entity.Name));
                }
                if (dataEntityNameSys.Length > 0)
                {
                    dataEntityNameSys = dataEntityNameSys.Remove(dataEntityNameSys.Length - 1, 1);
                    MessageBox.Show(CommonLanguage.Current.SystemItemNoDeleteAllowed +
                        Environment.NewLine + Environment.NewLine + dataEntityNameSys,
                        CommonLanguage.Current.MessageCaption_Notice,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                dataEntityName = dataEntityName.Remove(dataEntityName.Length - 1, 1);
                string strConfirmDelete = String.Format(CommonLanguage.Current.ConfirmDelete, dataEntityName);
                if (MessageBox.Show(strConfirmDelete, CommonLanguage.Current.MessageCaption_Notice,
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.OK)
                {
                    return;
                }
                foreach (DataItemEntityDev entity in entityList)
                {
                    if (entity.Sys) continue;
                    DataEntityArchive.Instance.DeleteDataItem(entity);
                }
            };
        }
    }
}
