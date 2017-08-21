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
    static class DataListDeleteRowDevChecker
    {
        static IWindowComponentService _windowComponentService;
        static DataSourceProvideFactory _dataSourceProvideFactory = DataSourceProvideFactory.Instance;
        static DataListDeleteRowDevChecker()
        {
            _windowComponentService = ServiceUnity.Container.Resolve<IWindowComponentService>();
        }
        public static void CheckWarning(DataListDeleteRowDev entity)
        {
            entity.Warning.Clear();
            if (entity.TargetWindow == EnumTargetWindow.Current)
            {
                if (entity.HostFormEntity.Elements.Contains(entity.DataList) == false)
                {
                    entity.Warning.AddWarningSign(entity, Language.Current.EventDev_DataListOperatorDev_DataListNotExist);
                }
                else
                {
                    foreach (DataListDeleteRowEvent.WhereItem where in entity.Where)
                    {
                        if (entity.HostFormEntity.Elements.Contains(where.DataColumn) == false)
                        {
                            entity.Warning.AddWarningSign(entity, Language.Current.EventDev_DataListOperatorDev_DataColumnNotExist);
                        }
                    }
                }
            }
            else if (entity.TargetWindow == EnumTargetWindow.Caller)
            {
                if (_windowComponentService.CheckElementExistByCode(
                    entity.DataList, FormElementEntityDevTypes.Instance.GetProvideAttribute(typeof(UIElementDataListEntity))) == false)
                {
                    entity.Warning.AddWarningSign(entity,Language.Current.EventDev_DataListOperatorDev_DataListNotExist);
                }
                else
                {
                    foreach (DataListDeleteRowEvent.WhereItem where in entity.Where)
                    {
                        if (_windowComponentService.CheckDataColumnExistByCode(entity.DataList, where.DataColumn) == false)
                        {
                            entity.Warning.AddWarningSign(entity,Language.Current.EventDev_DataListOperatorDev_DataColumnNotExist);
                        }
                    }
                }
            }
            DataSourceProvideArgs args = new DataSourceProvideArgs()
            {
                WindowEntity = entity.HostFormEntity
            };
            foreach (DataListDeleteRowEvent.WhereItem where in entity.Where)
            {
                if (_dataSourceProvideFactory.Validate(where.Source, args) == false)
                {
                    entity.Warning.AddWarningSign(entity, Language.Current.EventDev_DataListOperatorDev_FormElementNotExist);
                }
            }
        }
    }
}
