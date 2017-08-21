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
using Sheng.SailingEase.Core.Development.Localisation;
namespace Sheng.SailingEase.Core.Development
{
    static class EditorHelper
    {
        public static bool ValidateCodeExist(EventEditorAdapterAbstract hostAdapter, string code, out string validateMsg)
        {
            bool result = true;
            validateMsg = String.Empty;
            if (hostAdapter.EventCollection != null)
            {
                if (hostAdapter.EventCollection.CodeExist(code, hostAdapter.HostEvent.Id))
                {
                    validateMsg += Language.Current.EditorHelper_MessageEventCodeExist;
                    result = false;
                }
            }
            return result;
        }
    }
}
