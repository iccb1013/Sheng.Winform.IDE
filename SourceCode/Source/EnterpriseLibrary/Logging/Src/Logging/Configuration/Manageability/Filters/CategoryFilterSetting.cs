/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Filters
{
    [ManagementEntity]
    public partial class CategoryFilterSetting : LogFilterSetting
    {
        string categoryFilterMode;
        string[] categoryFilters;
        public CategoryFilterSetting(CategoryFilterData sourceElement,
                                     string name,
                                     string categoryFilterMode,
                                     string[] categoryFilters)
            : base(sourceElement, name)
        {
            this.categoryFilterMode = categoryFilterMode;
            this.categoryFilters = categoryFilters;
        }
        [ManagementConfiguration]
        public string CategoryFilterMode
        {
            get { return categoryFilterMode; }
            set { categoryFilterMode = value; }
        }
        [ManagementConfiguration]
        public string[] CategoryFilters
        {
            get { return categoryFilters; }
            set { categoryFilters = value; }
        }
        [ManagementBind]
        public static CategoryFilterSetting BindInstance(string ApplicationName,
                                                         string SectionName,
                                                         string Name)
        {
            return BindInstance<CategoryFilterSetting>(ApplicationName, SectionName, Name);
        }
        [ManagementEnumerator]
        public static IEnumerable<CategoryFilterSetting> GetInstances()
        {
            return GetInstances<CategoryFilterSetting>();
        }
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return CategoryFilterDataWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}
