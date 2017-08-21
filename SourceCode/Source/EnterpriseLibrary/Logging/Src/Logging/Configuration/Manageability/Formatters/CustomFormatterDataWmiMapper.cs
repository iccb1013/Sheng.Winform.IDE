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
    public static class CustomFormatterDataWmiMapper
    {
        public static void GenerateWmiObjects(CustomFormatterData data,
                                              ICollection<ConfigurationSetting> wmiSettings)
        {
            wmiSettings.Add(
                new CustomFormatterSetting(data,
                                           data.Name,
                                           data.Type.AssemblyQualifiedName,
                                           CustomDataWmiMapperHelper.GenerateAttributesArray(data.Attributes)));
        }
        internal static void RegisterWmiTypes()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(CustomFormatterSetting));
        }
        internal static bool SaveChanges(CustomFormatterSetting setting,
                                         ConfigurationElement sourceElement)
        {
            CustomFormatterData element = (CustomFormatterData)sourceElement;
            element.TypeName = setting.FormatterType;
            CustomDataWmiMapperHelper.UpdateAttributes(setting.Attributes, element.Attributes);
            return true;
        }
    }
}
