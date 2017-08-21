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
    static class DeleteDataDevChecker
    {
        static IDataEntityComponentService _dataEntityComponentService;
        static DeleteDataDevChecker()
        {
            _dataEntityComponentService = ServiceUnity.Container.Resolve<IDataEntityComponentService>();
        }
        public static void CheckWarning(DeleteDataDev entity)
        {
            entity.Warning.Clear();
            if (entity.DeleteMode == DeleteDataEvent.EnumDeleteDataMode.DataEntity)
            {
                DataEntity dataEntity = _dataEntityComponentService.GetDataEntity(entity.DataEntityId);
                if (dataEntity == null)
                {
                    entity.Warning.AddWarningSign(entity,Language.Current.EventDev_DeleteDataDev_DataEntityNotExist);
                }
                foreach (DeleteDataEvent.WhereItem where in entity.Where)
                {
                    if (dataEntity != null)
                    {
                        if (dataEntity.Items.Contains(where.DataItem) == false)
                        {
                            entity.Warning.AddWarningSign(entity,Language.Current.EventDev_DeleteDataDev_DataItemEntityNotExist);
                        }
                    }
                    if (where.Source.Type == EnumEventDataSource.FormElement)
                    {
                        if (entity.HostFormEntity.Elements.Contains(where.Source.Source) == false)
                        {
                            entity.Warning.AddWarningSign(entity,Language.Current.EventDev_DeleteDataDev_FormElementNotExist);
                        }
                    }
                }
            }
        }
    }
}
