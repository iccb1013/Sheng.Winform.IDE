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
    static class DataListRefreshDevChecker
    {
        static IDataEntityComponentService _dataEntityComponentService;
        static DataListRefreshDevChecker()
        {
            _dataEntityComponentService = ServiceUnity.Container.Resolve<IDataEntityComponentService>();
        }
        public static void CheckWarning(DataListRefreshDev entity)
        {
            entity.Warning.Clear();
            UIElementDataListEntity dataListEntity =
                entity.HostFormEntity.Elements.GetFormElementById(entity.DataListId) as UIElementDataListEntity;
            if (dataListEntity == null)
            {
                entity.Warning.AddWarningSign(entity, Language.Current.EventDev_RefreshListDev_DataListNotExist);
            }
            DataEntity dataEntity = null;
            if (dataListEntity != null)
            {
                dataEntity = _dataEntityComponentService.GetDataEntity(dataListEntity.DataEntityId);
                if (dataEntity == null)
                {
                    entity.Warning.AddWarningSign(entity,Language.Current.EventDev_RefreshListDev_DataEntityNotExist);
                }
            }
            foreach (DataListRefreshDev.WhereItem where in entity.Where)
            {
                if (dataEntity != null)
                {
                    if (dataEntity.Items.Contains(where.DataItem) == false)
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
    }
}
