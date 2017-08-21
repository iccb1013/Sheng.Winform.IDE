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
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Windows.Forms.Development.Localisation;
namespace Sheng.SailingEase.Windows.Forms.Development
{
    static class FormElementTextBoxEntityDevChecker
    {
        static IDataEntityComponentService _dataEntityComponentService = ServiceUnity.DataEntityComponentService;
        public static void CheckWarning(FormElementTextBoxEntityDev entity)
        {
            entity.Warning.Clear();
            if ( String.IsNullOrEmpty(entity.DataItemId) == false)
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
                    entity.Warning.AddWarningSign(entity,Language.Current.EntityDev_FormElementEditTextBoxEntityDev_DataItemEntityNotExist);
                }
            }
            WarningCheckerHelper.EventsValidate(entity);
        }
    }
}
