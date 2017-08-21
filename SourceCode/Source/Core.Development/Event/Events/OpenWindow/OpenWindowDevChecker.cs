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
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Infrastructure;
namespace Sheng.SailingEase.Core.Development
{
    static class OpenWindowDevChecker
    {
        static IWindowComponentService _windowComponentService;
        static OpenWindowDevChecker()
        {
            _windowComponentService = ServiceUnity.Container.Resolve<IWindowComponentService>();
        }
        public static void CheckWarning(OpenWindowDev entity)
        {
            entity.Warning.Clear();
            if (_windowComponentService.CheckExist(entity.WindowId) == false)
            {
                entity.Warning.AddWarningSign(entity, Language.Current.EventDev_OpenFormDev_FormNotExist);
            }
            XmlDocument xmlDocSendData = new XmlDocument();
            xmlDocSendData.LoadXml(entity.SendDataXml);
            XmlNodeList xmlNodeListSendData = xmlDocSendData.SelectNodes("SendData/Send");
            string[] strSource;
            UIElementDataListEntity sourceDataListEntity;
            foreach (XmlNode xmlNode in xmlNodeListSendData)
            {
                strSource = xmlNode.Attributes["Source"].Value.Split('.');
                if (strSource[0] == "FormElement")
                {
                    if (entity.HostFormEntity.Elements.Contains(strSource[1]) == false)
                    {
                        entity.Warning.AddWarningSign(entity, Language.Current.EventDev_OpenFormDev_FormElementNotExist);
                    }
                    if (strSource.Length == 3)
                    {
                        sourceDataListEntity =
                            entity.HostFormEntity.Elements.GetFormElementById(strSource[1]) as UIElementDataListEntity;
                        if (sourceDataListEntity != null)
                        {
                            if (!sourceDataListEntity.DataColumns.Contains(strSource[2]))
                            {
                                entity.Warning.AddWarningSign(entity, Language.Current.EventDev_OpenFormDev_FormElementNotExist);
                            }
                        }
                    }
                }
            }
        }
    }
}
