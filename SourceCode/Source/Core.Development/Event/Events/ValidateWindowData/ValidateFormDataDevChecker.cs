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
    static class ValidateFormDataDevChecker
    {
        public static void CheckWarning(ValidateFormDataDev entity)
        {
            entity.Warning.Clear();
            if (entity.ValidateMode == ValidateFormDataEvent.EnumValidateFormDataMode.Appoint)
            {
                XmlDocument xmlDocValidateSet = new XmlDocument();
                xmlDocValidateSet.LoadXml(entity.ValidateSetXml);
                XmlNodeList xmlNodeListValidate = xmlDocValidateSet.SelectNodes("ValidateSet/Validate");
                foreach (XmlNode xmlNode in xmlNodeListValidate)
                {
                    if (entity.HostFormEntity.Elements.Contains(xmlNode.Attributes["FormElement"].Value.Split('.')[1]) == false)
                    {
                        entity.Warning.AddWarningSign(entity,Language.Current.EventDev_ValidateFormDataDev_FormElementNotExist);
                    }
                }
            }
        }
    }
}
