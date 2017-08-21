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
namespace Sheng.SailingEase.Core.Development
{
    static class ReceiveDataDevChecker
    {
        public static void CheckWarning(ReceiveDataDev entity)
        {
            entity.Warning.Clear();
            XmlDocument xmlDocReceiveData = new XmlDocument();
            xmlDocReceiveData.LoadXml(entity.ReceiveDataXml);
            XmlNodeList xmlNodeListReceiveData = xmlDocReceiveData.SelectNodes("ReceiveData/Receive");
            foreach (XmlNode xmlNode in xmlNodeListReceiveData)
            {
                if (entity.HostFormEntity.Elements.Contains(xmlNode.Attributes["ReceiveTo"].Value.Split('.')[1]) == false)
                {
                    entity.Warning.AddWarningSign(entity, Language.Current.EventDev_ReceiveDataDev_FormElementNotExist);
                }
            }
        }
    }
}
