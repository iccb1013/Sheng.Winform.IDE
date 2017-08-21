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
using Sheng.SailingEase.Core.Development.Localisation;
using Sheng.SailingEase.Infrastructure;
namespace Sheng.SailingEase.Core.Development
{
    static class LoadDataToFormDevChecker
    {
        static IDataEntityComponentService _dataEntityComponentService = ServiceUnity.DataEntityComponentService;
        public static void CheckWarning(LoadDataToFormDev entity)
        {
            entity.Warning.Clear();
            DataEntity dataEntity = null;
            if (entity.LoadMode == LoadDataToFormEvent.EnumLoadDataToFormMode.DataEntity)
            {
                dataEntity = _dataEntityComponentService.GetDataEntity(entity.DataEntityId);
                if (dataEntity == null)
                {
                    entity.Warning.AddWarningSign(entity,Language.Current.EventDev_LoadDataToFormDev_DataEntityNotExist);
                }
                foreach (LoadDataToFormEvent.WhereItem where in entity.Where)
                {
                    if (dataEntity != null)
                    {
                        if (!dataEntity.Items.Contains(where.DataItem))
                        {
                            entity.Warning.AddWarningSign(entity,Language.Current.EventDev_RefreshListDev_DataItemEntityNotExist);
                        }
                    }
                    if (where.Source.Type == EnumEventDataSource.FormElement)
                    {
                        if (entity.HostFormEntity.Elements.Contains(where.Source.Source) == false)
                        {
                            entity.Warning.AddWarningSign(entity,Language.Current.EventDev_RefreshListDev_FormElementNotExist);
                        }
                    }
                }
            }
            foreach (LoadDataToFormEvent.WhereItem where in entity.Where)
            {
                if (entity.LoadMode == LoadDataToFormEvent.EnumLoadDataToFormMode.DataEntity)
                {
                    if (dataEntity != null)
                    {
                        if (dataEntity.Items.Contains(where.DataItem) == false)
                        {
                            entity.Warning.AddWarningSign(entity,Language.Current.EventDev_LoadDataToFormDev_DataItemEntityNotExist);
                        }
                    }
                }
                if (where.Source.Type == EnumEventDataSource.FormElement)
                {
                    if (entity.HostFormEntity.Elements.Contains(where.Source.Source) == false)
                    {
                        entity.Warning.AddWarningSign(entity,Language.Current.EventDev_LoadDataToFormDev_FormElementNotExist);
                    }
                }
            }
        }
    }
}
