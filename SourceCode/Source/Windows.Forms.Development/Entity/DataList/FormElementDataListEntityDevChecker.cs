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
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Windows.Forms.Development.Localisation;
namespace Sheng.SailingEase.Windows.Forms.Development
{
    static class FormElementDataListEntityDevChecker
    {
        static IDataEntityComponentService _dataEntityComponentService = ServiceUnity.DataEntityComponentService;
        public static void CheckWarning(FormElementDataListEntityDev entity)
        {
            entity.Warning.Clear();
            if (String.IsNullOrEmpty(entity.DataEntityId) == false)
            {
                DataEntity dataEntity = _dataEntityComponentService.GetDataEntity(entity.DataEntityId);
                if (dataEntity == null)
                {
                    entity.Warning.AddWarningSign(entity, Language.Current.EntityDev_FormElementDataListEntityDev_DataEntityNotExist);
                }
            }
            foreach (UIElementDataListColumnEntityAbstract dc in entity.DataColumns)
            {
                IWarningable columnWarning = dc as IWarningable;
                if (columnWarning == null)
                    continue;
                columnWarning.CheckWarning();
                if (columnWarning.Warning.ExistWarning)
                {
                    entity.Warning.AddWarningSign(columnWarning.Warning);
                }
            }
            WarningCheckerHelper.EventsValidate(entity);
        }
    }
}
