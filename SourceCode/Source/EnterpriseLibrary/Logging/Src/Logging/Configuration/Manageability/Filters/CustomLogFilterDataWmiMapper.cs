/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Filters
{
    public static class CustomLogFilterDataWmiMapper
    {
        public static void GenerateWmiObjects(CustomLogFilterData data,
                                              ICollection<ConfigurationSetting> wmiSettings)
        {
            wmiSettings.Add(
                new CustomFilterSetting(data,
                                        data.Name,
                                        data.Type.AssemblyQualifiedName,
                                        CustomDataWmiMapperHelper.GenerateAttributesArray(data.Attributes)));
        }
        internal static void RegisterWmiTypes()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(CustomFilterSetting));
        }
        internal static bool SaveChanges(CustomFilterSetting setting,
                                         ConfigurationElement sourceElement)
        {
            CustomLogFilterData element = (CustomLogFilterData)sourceElement;
            element.TypeName = setting.FilterType;
            CustomDataWmiMapperHelper.UpdateAttributes(setting.Attributes, element.Attributes);
            return true;
        }
    }
}
