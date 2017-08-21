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
using Sheng.SailingEase.Core.Development;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Windows.Forms.Development.Localisation;
namespace Sheng.SailingEase.Windows.Forms.Development
{
    static class FormElementComboBoxEntityDevChecker
    {
        static IDataEntityComponentService _dataEntityComponentService = ServiceUnity.DataEntityComponentService;
        static IDictionaryComponentService _dictionaryComponentService = ServiceUnity.DictionaryComponentService;
        public static void CheckWarning(FormElementComboBoxEntityDev entity)
        {
            entity.Warning.Clear();
            if (entity.DataItemId != String.Empty)
            {
                string[] ids = entity.DataItemId.Split('.');
                string dataEntityId = ids[0];
                string dataItemEntityId = ids[1];
                DataEntity dataEntity = _dataEntityComponentService.GetDataEntity(dataEntityId);
                DataItemEntity dataItemEntity = null;
                if (dataEntity != null)
                    dataItemEntity = dataEntity.Items.GetEntityById(dataItemEntityId);
                if (dataEntity == null || dataItemEntity == null)
                {
                    entity.Warning.AddWarningSign(entity,Language.Current.EntityDev_FormElementEditComboBoxEntityDev_DataItemEntityNotExist);
                }
            }
            if (entity.DataSourceMode == UIElementComboBoxEntity.EnumComboBoxDataSourceMode.Enum
                &&  String.IsNullOrEmpty(entity.EnumId) == false)
            {
                EnumEntity enumEntity = _dictionaryComponentService.GetEnumEntity(entity.EnumId);
                if (enumEntity == null)
                {
                    entity.Warning.AddWarningSign(entity,Language.Current.EntityDev_FormElementEditComboBoxEntityDev_EnumNotExist);
                }
            }
            if (entity.DataSourceMode == UIElementComboBoxEntity.EnumComboBoxDataSourceMode.DataEntity
                && String.IsNullOrEmpty(entity.DataEntityId) == false)
            {
                DataEntity dataEntity = _dataEntityComponentService.GetDataEntity(entity.DataEntityId);
                if (dataEntity == null)
                {
                    entity.Warning.AddWarningSign(entity,Language.Current.EntityDev_FormElementEditComboBoxEntityDev_DataEntityNotExist);
                }
                else
                {
                    DataItemEntity dataItemEntity;
                    dataItemEntity = dataEntity.Items.GetEntityById(entity.TextDataItemId);
                    if (dataItemEntity == null)
                    {
                        entity.Warning.AddWarningSign(entity,Language.Current.EntityDev_FormElementEditComboBoxEntityDev_DataItemEntityNotExist);
                    }
                    dataItemEntity = dataEntity.Items.GetEntityById(entity.ValueDataItemId);
                    if (dataItemEntity == null)
                    {
                        entity.Warning.AddWarningSign(entity,Language.Current.EntityDev_FormElementEditComboBoxEntityDev_DataItemEntityNotExist);
                    }
                }
            }
            WarningCheckerHelper.EventsValidate(entity);
        }
    }
}
