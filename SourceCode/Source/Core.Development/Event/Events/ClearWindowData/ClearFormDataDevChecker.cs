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
using System.Xml;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Core.Development.Localisation;
namespace Sheng.SailingEase.Core.Development
{
    static class ClearFormDataDevChecker
    {
        public static void CheckWarning(ClearFormDataDev entity)
        {
            entity.Warning.Clear();
            if (entity.ClearFormDataMode == ClearFormDataEvent.EnumClearFormDataMode.Appoint)
            {
                XmlDocument xmlDocClear = new XmlDocument();
                xmlDocClear.LoadXml(entity.ClearsSetXml);
                XmlNodeList xmlNodeListClear = xmlDocClear.SelectNodes("Clears/Clear");
                foreach (XmlNode xmlNode in xmlNodeListClear)
                {
                    if (entity.HostFormEntity.Elements.Contains(xmlNode.Attributes["FormElement"].Value) == false)
                    {
                        entity.Warning.AddWarningSign(entity,Language.Current.EventDev_ClearFormDataDev_FormElementNotExist);
                    }
                }
            }
        }
    }
}
