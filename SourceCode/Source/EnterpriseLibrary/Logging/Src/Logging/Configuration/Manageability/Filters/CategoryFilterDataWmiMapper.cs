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
    public static class CategoryFilterDataWmiMapper
    {
        public static void GenerateWmiObjects(CategoryFilterData configurationObject,
                                              ICollection<ConfigurationSetting> wmiSettings)
        {
            string[] categoryFilters = new string[configurationObject.CategoryFilters.Count];
            for (int i = 0; i < configurationObject.CategoryFilters.Count; i++)
            {
                categoryFilters[i]
                    = configurationObject.CategoryFilters.Get(i).Name;
            }
            wmiSettings.Add(
                new CategoryFilterSetting(configurationObject,
                                          configurationObject.Name,
                                          configurationObject.CategoryFilterMode.ToString(),
                                          categoryFilters));
        }
        internal static void RegisterWmiTypes()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(CategoryFilterSetting));
        }
        internal static bool SaveChanges(CategoryFilterSetting setting,
                                         ConfigurationElement sourceElement)
        {
            CategoryFilterData element = (CategoryFilterData)sourceElement;
            element.CategoryFilterMode = ParseHelper.ParseEnum<CategoryFilterMode>(setting.CategoryFilterMode, false);
            element.CategoryFilters.Clear();
            foreach (string name in setting.CategoryFilters)
            {
                element.CategoryFilters.Add(new CategoryFilterEntry(name));
            }
            return true;
        }
    }
}
