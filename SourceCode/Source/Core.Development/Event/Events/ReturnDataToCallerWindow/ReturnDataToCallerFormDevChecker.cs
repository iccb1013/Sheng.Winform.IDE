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
using Sheng.SailingEase.Core.Development.Localisation;
using Sheng.SailingEase.Infrastructure;
namespace Sheng.SailingEase.Core.Development
{
    static class ReturnDataToCallerFormDevChecker
    {
       
        static IWindowComponentService _windowComponentService = ServiceUnity.WindowComponentService;
        public static void CheckWarning(ReturnDataToCallerFormDev entity)
        {
            entity.Warning.Clear();
            XmlDocument xmlDocReturnSet = new XmlDocument();
            xmlDocReturnSet.LoadXml(entity.ReturnSetXml);
            XmlNodeList xmlNodeListReturnSet = xmlDocReturnSet.SelectNodes("ReturnSet/Return");
            string[] strSource;
            UIElementDataListEntity sourceDataListEntity;
            foreach (XmlNode xmlNode in xmlNodeListReturnSet)
            {
                if (_windowComponentService.CheckElementExistByCode
                    (xmlNode.Attributes["FormElementCode"].Value) == false)
                {
                    entity.Warning.AddWarningSign(entity,Language.Current.EventDev_ReturnDataToCallerFormDev_FormElementNotExist);
                }
                strSource = xmlNode.Attributes["Source"].Value.Split('.');
                if (entity.HostFormEntity.Elements.Contains(strSource[1]) == false)
                {
                    entity.Warning.AddWarningSign(entity,Language.Current.EventDev_ReturnDataToCallerFormDev_FormElementNotExist);
                }
                if (strSource.Length == 3)
                {
                    sourceDataListEntity =
                            entity.HostFormEntity.Elements.GetFormElementById(strSource[1]) as UIElementDataListEntity;
                    if (sourceDataListEntity != null)
                    {
                        if (sourceDataListEntity.DataColumns.Contains(strSource[2]) == false)
                        {
                            entity.Warning.AddWarningSign(entity,Language.Current.EventDev_ReturnDataToCallerFormDev_FormElementNotExist);
                        }
                    }
                }
            }
        }
    }
}
