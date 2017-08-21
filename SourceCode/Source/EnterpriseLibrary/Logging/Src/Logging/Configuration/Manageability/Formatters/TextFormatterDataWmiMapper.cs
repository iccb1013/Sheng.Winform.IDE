/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Formatters
{
    public static class TextFormatterDataWmiMapper
    {
        public static void GenerateWmiObjects(TextFormatterData configurationObject,
                                              ICollection<ConfigurationSetting> wmiSettings)
        {
            wmiSettings.Add(new TextFormatterSetting(configurationObject, configurationObject.Name, configurationObject.Template));
        }
        internal static void RegisterWmiTypes()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(TextFormatterSetting));
        }
        internal static bool SaveChanges(TextFormatterSetting setting,
                                         ConfigurationElement sourceElement)
        {
            TextFormatterData element = (TextFormatterData)sourceElement;
            element.Template = setting.Template;
            return true;
        }
    }
}
