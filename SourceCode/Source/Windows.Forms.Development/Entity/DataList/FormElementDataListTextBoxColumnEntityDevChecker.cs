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
using Sheng.SailingEase.Core.Development;
using Sheng.SailingEase.Windows.Forms.Development.Localisation;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Infrastructure;
namespace Sheng.SailingEase.Windows.Forms.Development
{
   static class FormElementDataListTextBoxColumnEntityDevChecker 
    {
       static IDataEntityComponentService _dataEntityComponentService = ServiceUnity.DataEntityComponentService;
       public static void CheckWarning(FormElementDataListTextBoxColumnEntityDev entity)
        {
            entity.Warning.Clear();
            if (entity.IsBind)
            {
                string[] ids = entity.DataItemId.Split('.');
                string dataEntityId = ids[0];
                string dataItemId = ids[1];
                DataEntity dataEntity = _dataEntityComponentService.GetDataEntity(dataEntityId);
                DataItemEntity dataItemEntity = dataEntity.Items.GetEntityById(dataItemId);
                if (dataEntity == null || dataItemEntity == null)
                {
                    entity.Warning.AddWarningSign(entity,Language.Current.EntityDev_FormElementDataColumnEntityDev_DataItemEntityNotExist);
                }
            }
            WarningCheckerHelper.EventsValidate(entity);
        }
    }
}
