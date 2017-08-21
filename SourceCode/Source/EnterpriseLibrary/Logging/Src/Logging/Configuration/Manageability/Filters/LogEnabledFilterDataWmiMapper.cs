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
    public static class LogEnabledFilterDataWmiMapper
    {
        public static void GenerateWmiObjects(LogEnabledFilterData configurationObject,
                                              ICollection<ConfigurationSetting> wmiSettings)
        {
            wmiSettings.Add(new LogEnabledFilterSetting(configurationObject, configurationObject.Name, configurationObject.Enabled));
        }
        internal static void RegisterWmiTypes()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(LogEnabledFilterSetting));
        }
        internal static bool SaveChanges(LogEnabledFilterSetting setting,
                                         ConfigurationElement sourceElement)
        {
            LogEnabledFilterData element = (LogEnabledFilterData)sourceElement;
            element.Enabled = setting.Enabled;
            return true;
        }
    }
}
