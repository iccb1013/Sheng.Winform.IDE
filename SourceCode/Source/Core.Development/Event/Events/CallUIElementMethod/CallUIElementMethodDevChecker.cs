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
namespace Sheng.SailingEase.Core.Development
{
    static class CallUIElementMethodDevChecker
    {
        public static void CheckWarning(CallUIElementMethodDev entity)
        {
            entity.Warning.Clear();
            IWarningable callEventWarningable = entity.CallEvent as IWarningable;
            if (callEventWarningable != null)
            {
                callEventWarningable.CheckWarning();
                if (callEventWarningable.Warning.ExistWarning)
                {
                    entity.Warning.AddWarningSign(callEventWarningable.Warning.Warnings);
                }
            }
        }
    }
}
