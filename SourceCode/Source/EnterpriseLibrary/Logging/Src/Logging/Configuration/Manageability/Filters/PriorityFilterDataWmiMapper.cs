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
    public static class PriorityFilterDataWmiMapper
    {
        public static void GenerateWmiObjects(PriorityFilterData configurationObject,
                                              ICollection<ConfigurationSetting> wmiSettings)
        {
            wmiSettings.Add(
                new PriorityFilterSetting(configurationObject,
                                          configurationObject.Name,
                                          configurationObject.MaximumPriority,
                                          configurationObject.MinimumPriority));
        }
        internal static void RegisterWmiTypes()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(PriorityFilterSetting));
        }
        internal static bool SaveChanges(PriorityFilterSetting setting,
                                         ConfigurationElement sourceElement)
        {
            PriorityFilterData element = (PriorityFilterData)sourceElement;
            element.MaximumPriority = setting.MaximumPriority;
            element.MinimumPriority = setting.MinimumPriority;
            return true;
        }
    }
}
