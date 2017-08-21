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
namespace Sheng.SailingEase.Windows.Forms.Development
{
    static class FormEntityDevChecker
    {
        public static void CheckWarning(FormEntityDev entity)
        {
            entity.Warning.Clear();
            foreach (UIElement element in entity.Elements)
            {
                IWarningable elementWarning = element as IWarningable;
                if (elementWarning == null)
                    continue;
                elementWarning.CheckWarning();
                if (elementWarning.Warning.ExistWarning)
                {
                    entity.Warning.AddWarningSign(elementWarning.Warning);
                }
            }
            WarningCheckerHelper.EventsValidate(entity);
        }
    }
}
