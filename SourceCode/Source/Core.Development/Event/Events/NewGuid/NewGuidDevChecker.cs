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
namespace Sheng.SailingEase.Core.Development
{
    static class NewGuidDevChecker
    {
        public static void CheckWarning(NewGuidDev entity)
        {
            entity.Warning.Clear();
            if (entity.HostFormEntity.Elements.Contains(entity.SetFormElementId) == false)
            {
                entity.Warning.AddWarningSign(entity,Language.Current.EventDev_NewGuidDev_FormElementNotExist);
            }
        }
    }
}
