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
namespace Sheng.SailingEase.Windows.Forms.Development
{
    static class FormElementButtonEntityDevChecker
    {
        public static void CheckWarning(FormElementButtonEntityDev entity)
        {
            entity.Warning.Clear();
            WarningCheckerHelper.EventsValidate(entity);
        }
    }
}
