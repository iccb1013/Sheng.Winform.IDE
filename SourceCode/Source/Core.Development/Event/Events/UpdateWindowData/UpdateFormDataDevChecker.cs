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
using Sheng.SailingEase.Core.Development.Localisation;
namespace Sheng.SailingEase.Core.Development
{
    static class UpdateFormDataDevChecker
    {
        static IDataEntityComponentService _dataEntityComponentService
           = ServiceUnity.Container.Resolve<IDataEntityComponentService>();
        public static void CheckWarning(UpdateFormDataDev entity)
        {
            entity.Warning.Clear();
            if (entity.UpdateMode == UpdateFormDataEvent.EnumUpdateFormDataMode.DataEntity)
            {
                DataEntity dataEntity = _dataEntityComponentService.GetDataEntity(entity.DataEntityId);
                if (dataEntity == null)
                {
                    entity.Warning.AddWarningSign(entity, Language.Current.EventDev_UpdateFormDataDev_DataEntityNotExist);
                }
                foreach (UpdateFormDataEvent.UpdateItem upate in entity.Update)
                {
                    if (dataEntity != null)
                    {
                        if (dataEntity.Items.Contains(upate.DataItem) == false)
                        {
                            entity.Warning.AddWarningSign(entity,Language.Current.EventDev_UpdateFormDataDev_DataItemEntityNotExist);
                        }
                    }
                    if (upate.Source.Type == EnumEventDataSource.FormElement)
                    {
                        if (entity.HostFormEntity.Elements.Contains(upate.Source.Source) == false)
                        {
                            entity.Warning.AddWarningSign(entity,Language.Current.EventDev_UpdateFormDataDev_FormElementNotExist);
                        }
                    }
                }
                foreach (UpdateFormDataEvent.WhereItem where in entity.Where)
                {
                    if (dataEntity != null)
                    {
                        if (dataEntity.Items.Contains(where.DataItem) == false)
                        {
                            entity.Warning.AddWarningSign(entity,Language.Current.EventDev_UpdateFormDataDev_DataItemEntityNotExist);
                        }
                    }
                    if (where.Source.Type == EnumEventDataSource.FormElement)
                    {
                        if (entity.HostFormEntity.Elements.Contains(where.Source.Source) == false)
                        {
                            entity.Warning.AddWarningSign(entity,Language.Current.EventDev_UpdateFormDataDev_FormElementNotExist);
                        }
                    }
                }
            }
        }
    }
}
