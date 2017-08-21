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
using Sheng.SailingEase.Core.Development.Localisation;
using Sheng.SailingEase.Infrastructure;
namespace Sheng.SailingEase.Core.Development
{
    static class SaveDataDevChecker
    {
        static IDataEntityComponentService _dataEntityComponentService
           = ServiceUnity.Container.Resolve<IDataEntityComponentService>();
        static DataSourceProvideFactory _dataSourceProvideFactory = DataSourceProvideFactory.Instance;
        public static void CheckWarning(SaveDataDev entity)
        {
            entity.Warning.Clear();
            if (entity.SaveMode == SaveDataEvent.EnumSaveDataMode.DataEntity)
            {
                DataEntity dataEntity = _dataEntityComponentService.GetDataEntity(entity.DataEntityId);
                if (dataEntity == null)
                {
                     entity.Warning.AddWarningSign(entity,Language.Current.EventDev_SaveFormDataDev_DataEntityNotExist);
                }
                DataSourceProvideArgs args = new DataSourceProvideArgs()
                {
                    WindowEntity = entity.HostFormEntity
                };
                foreach (SaveDataEvent.SaveItem save in entity.Save)
                {
                    if (dataEntity != null)
                    {
                        if (dataEntity.Items.Contains(save.DataItem) == false)
                        {
                            entity.Warning.AddWarningSign(entity, Language.Current.EventDev_SaveFormDataDev_DataItemEntityNotExist);
                        }
                    }
                    if (_dataSourceProvideFactory.Validate(save.Source, args) == false)
                    {
                        entity.Warning.AddWarningSign(entity, Language.Current.EventDev_SaveFormDataDev_FormElementNotExist);
                    }
                }
            }
        }
    }
}
